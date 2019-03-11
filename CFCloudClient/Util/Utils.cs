using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.Util
{
    public class Utils
    {
        public static void DeleteFolder(string path)
        {
            bool retry = true;
            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            foreach (string dir in dirs)
            {
                DeleteFolder(dir);
            }
            foreach (string file in files)
            {
                UnLockFile(file);
                retry = true;
                while (retry)
                {
                    try
                    {
                        new FileInfo(file).Attributes = FileAttributes.Normal;
                        File.Delete(file);
                        retry = false;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5000);
                    }
                }
                SqliteHelper.Delete(file);
            }
            retry = true;
            while (retry)
            {
                try
                {
                    Directory.Delete(path);
                    retry = false;
                }
                catch (IOException)
                {
                    Thread.Sleep(5000);
                }
            }
            SqliteHelper.Delete(path);
        }

        public static void LockAllFiles(string Folder)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(Folder);
                string[] files = Directory.GetFiles(Folder);
                foreach (string dir in dirs)
                {
                    Models.SQLDataType sdt = SqliteHelper.Select(dir);
                    if (sdt != null)
                    {
                        if (sdt.IsShared.Equals("true"))
                            LockAllFilesInaSharedFolder(dir);
                        else
                            LockAllFiles(dir);
                    }
                    LockAllFiles(dir);
                }
                foreach (string file in files)
                {
                    Models.SQLDataType sdt = SqliteHelper.Select(file);
                    if (sdt != null)
                    {
                        if (sdt.IsShared.Equals("true"))
                            LockFile(file);
                    }
                }
            }
            catch (IOException)
            {

            }
        }

        public static void LockAllFilesInaSharedFolder(string Folder)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(Folder);
                string[] files = Directory.GetFiles(Folder);
                foreach (string dir in dirs)
                    LockAllFilesInaSharedFolder(dir);
                foreach (string file in files)
                    LockFile(file);
            }
            catch (IOException)
            {

            }
        }

        public static void UnLockAllFiles()
        {
            foreach (FileStream stream in Global.LockedFiles.Values)
            {
                try
                {
                    stream.Close();
                }
                catch (IOException)
                {

                }
            }
        }

        public static void LockFile(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                Global.LockedFiles.Add(path, stream);
            }
            catch (IOException)
            {

            }
        }

        public static void UnLockFile(string path)
        {
            try
            {
                if (!Global.LockedFiles.ContainsKey(path))
                    return;
                FileStream stream = null;
                Global.LockedFiles.TryGetValue(path, out stream);
                stream.Close();
                Global.LockedFiles.Remove(path);
            }
            catch (IOException)
            {

            }
        }

        public static string LocalPathtoCloudPath(string path)
        {
            if (path == null)
                return null;
            return path.Substring(Properties.Settings.Default.Workspace.Length).Replace('\\', '/');
        }

        public static string CloudPathtoLocalPath(string path)
        {
            if (path == null)
                return null;
            return Properties.Settings.Default.Workspace + path.Replace('/', '\\');
        }

        public static void CheckConsistency(string folder)
        {
            string[] dirs = Directory.GetDirectories(folder);
            string[] files = Directory.GetFiles(folder);
            List<Models.Metadata> cloudEntries = BackgroundWorks.NetworkManager.ListFolder(LocalPathtoCloudPath(folder));
            List<Models.Metadata> cloudDirs = new List<Models.Metadata>();
            List<Models.Metadata> cloudFiles = new List<Models.Metadata>();
            foreach (Models.Metadata metadata in cloudEntries)
            {
                if (metadata.Tag.Equals("Folder"))
                    cloudDirs.Add(metadata);
                else
                    cloudFiles.Add(metadata);
            }
            foreach (Models.Metadata metadata in cloudDirs)
            {
                string LocalPath = CloudPathtoLocalPath(metadata.FullPath);
                if (!dirs.Contains(LocalPath))
                {
                    if (metadata.Modifier.Equal(Util.Global.info.user))
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientDelete, LocalPath);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                    else
                    {
                        DownloadFolder(LocalPath);
                    }
                }
            }
            foreach (string dir in dirs)
            {
                if (cloudDirs.Find((d) =>
                {
                    return LocalPathtoCloudPath(dir).Equals(d.FullPath);
                }) == null)
                {
                    if (SqliteHelper.Select(dir) == null)
                        UploadFolder(dir);
                    else
                        DeleteFolder(dir);
                }
                else
                    CheckConsistency(dir);
            }
            foreach (Models.Metadata metadata in cloudFiles)
            {
                string LocalPath = CloudPathtoLocalPath(metadata.FullPath);
                if (!files.Contains(LocalPath))
                {
                    if (metadata.Modifier.Equal(Util.Global.info.user))
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientDelete, LocalPath);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                    else
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ServerCreate, LocalPath);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                }
            }
            foreach (string file in files)
            {
                Models.Metadata metadata;
                if ((metadata = cloudFiles.Find((d) =>
                {
                    return LocalPathtoCloudPath(file).Equals(d.FullPath);
                })) == null)
                {
                    if (SqliteHelper.Select(file) == null)
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientCreate, file);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                    else
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ServerDelete, file);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                }
                else
                {
                    DateTime LocalTime = new FileInfo(file).LastWriteTimeUtc;
                    DateTime CloudTime = metadata.ModifiedTime;
                    Models.SQLDataType sqlData = SqliteHelper.Select(file);
                    if (sqlData == null)
                    {
                        Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientCreate, file);
                        Util.Global.FileUpdateQueue.Add(fce);
                    }
                    else
                    {
                        DateTime LastCloudTime = sqlData.getModifiedTime();
                        if (LocalTime > LastCloudTime)
                        {
                            Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientChange, file);
                            Util.Global.FileUpdateQueue.Add(fce);
                        }
                        else
                        {
                            if (CloudTime > LastCloudTime)
                            {
                                Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ServerChange, file);
                                Util.Global.FileUpdateQueue.Add(fce);
                            }
                        }
                    }
                }
            }
        }

        public static void DownloadFolder(string folder)
        {
            List<Models.Metadata> cloudEntries = BackgroundWorks.NetworkManager.ListFolder(LocalPathtoCloudPath(folder));
            Util.Global.FileUpdateQueue.Add(new Models.FileChangeEvent(
                Models.FileChangeEvent.FileChangeType.ServerCreate, 
                folder));
            foreach (Models.Metadata metadata in cloudEntries)
            {
                if (metadata.Tag.Equals("Folder"))
                    DownloadFolder(CloudPathtoLocalPath(metadata.FullPath));
                else
                {
                    Util.Global.FileUpdateQueue.Add(new Models.FileChangeEvent(
                        Models.FileChangeEvent.FileChangeType.ServerCreate,
                        CloudPathtoLocalPath(metadata.FullPath)));
                }
            }
        }

        public static void UploadFolder(string folder)
        {
            Util.Global.FileUpdateQueue.Add(new Models.FileChangeEvent(
                Models.FileChangeEvent.FileChangeType.ClientCreate,
                folder));
            string[] dirs = Directory.GetDirectories(folder);
            string[] files = Directory.GetFiles(folder);
            foreach (string dir in dirs)
                UploadFolder(dir);
            foreach (string file in files)
                Util.Global.FileUpdateQueue.Add(new Models.FileChangeEvent(
                    Models.FileChangeEvent.FileChangeType.ClientCreate,
                    file));
        }
    } 
}
