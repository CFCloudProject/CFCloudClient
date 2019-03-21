using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileUpdaterProcesser
    {
        private Models.FileChangeEvent e;

        public FileUpdaterProcesser(Models.FileChangeEvent e)
        {
            this.e = e;
        }

        public void run()
        {
            switch (e.Type)
            {
                case Models.FileChangeEvent.FileChangeType.ClientCreate:
                    ClientCreate(); break;
                case Models.FileChangeEvent.FileChangeType.ClientChange:
                    ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRename:
                    ClientRename(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                    ClientRename(); ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                    ClientRename(); ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientDelete:
                    ClientDelete(); break;
                case Models.FileChangeEvent.FileChangeType.ServerCreate:
                    ServerCreate(); break;
                case Models.FileChangeEvent.FileChangeType.ServerChange:
                    ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRename:
                    ServerRename(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange:
                    ServerRename(); ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange:
                    ServerRename(); ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerDelete:
                    ServerDelete(); break;
            }

            FileProcessing.Remove(e.ProcessingPath());
            Util.Global.updater.ThreadEnd();
        }

        private void ClientCreate()
        {
            if (Util.SqliteHelper.Select(e.LocalPath) != null)
                return;
            if (!Directory.Exists(e.LocalPath) && !File.Exists(e.LocalPath))
                return;
            Models.Metadata createResult;
            if (!File.Exists(e.LocalPath))
                createResult = NetworkManager.CreateFolder(e.CloudPath);
            else
            {
                while ((createResult = NetworkManager.Upload(e.CloudPath)) == null)
                    Thread.Sleep(5000);
            }
            Models.SQLDataType sdt = new Models.SQLDataType(e.LocalPath, 
                createResult.ModifiedTime, 
                createResult.Rev, 
                createResult.Modifier.Email, 
                createResult.isShared ? "true" : "false");
            Util.SqliteHelper.Insert(sdt);
            if (createResult.isShared && createResult.Tag.Equals("File"))
                Util.Utils.LockFile(e.LocalPath);
            if (createResult.TokenHolder.Equal(Util.Global.user))
                NetworkManager.ReturnToken(e.CloudPath);
        }

        private void ClientChange()
        {
            if (!Directory.Exists(e.LocalPath) && !File.Exists(e.LocalPath))
                return;
            Models.SQLDataType sdt = Util.SqliteHelper.Select(e.LocalPath);
            if (sdt != null && sdt.getModifiedTime() == File.GetLastWriteTimeUtc(e.LocalPath))
                return;
            if (sdt == null)
            {
                ClientCreate();
            }
            else
            {
                if (Directory.Exists(e.LocalPath))
                    return;
                if (sdt.IsShared.Equals("true"))
                    Util.Utils.UnLockFile(e.LocalPath);
                Models.Metadata uploadResult;
                while ((uploadResult = NetworkManager.Upload(e.CloudPath)) == null)
                    Thread.Sleep(5000);
                Models.SQLDataType sdt2 = new Models.SQLDataType(e.LocalPath,
                    uploadResult.ModifiedTime,
                    uploadResult.Rev,
                    uploadResult.Modifier.Email,
                    uploadResult.isShared ? "true" : "false");
                Util.SqliteHelper.Update(sdt2);
                if (uploadResult.isShared)
                    Util.Utils.LockFile(e.LocalPath);
                if (uploadResult.TokenHolder.Equal(Util.Global.user))
                    NetworkManager.ReturnToken(e.CloudPath);
            }
        }

        private void ClientRename()
        {
            if (!Directory.Exists(e.LocalPath) && !File.Exists(e.LocalPath))
                return;
            Models.SQLDataType sdt = Util.SqliteHelper.Select(e.OldLocalPath);
            sdt.Path = e.LocalPath;
            Util.SqliteHelper.Delete(e.OldLocalPath);
            Util.SqliteHelper.Insert(sdt);
            Models.Metadata renameResult = NetworkManager.Rename(e.CloudPath, e.OldCloudPath);
            if (renameResult.isShared && renameResult.Tag.Equals("File"))
                Util.Utils.LockFile(e.LocalPath);
            if (renameResult.TokenHolder.Equal(Util.Global.user))
                NetworkManager.ReturnToken(e.CloudPath);
        }

        private void ClientDelete()
        {
            NetworkManager.Delete(e.CloudPath);
            Util.SqliteHelper.Delete(e.LocalPath);
        }

        private void ServerCreate()
        {
            Models.Metadata metadata = NetworkManager.GetMetadata(e.CloudPath);
            if (metadata.Tag.Equals("File"))
            {
                while (!NetworkManager.Download(e.CloudPath))
                    Thread.Sleep(5000);
                metadata = NetworkManager.GetMetadata(e.CloudPath);
                File.SetLastWriteTimeUtc(e.LocalPath, metadata.ModifiedTime);
                if (metadata.isShared)
                    Util.Utils.LockFile(e.LocalPath);
            }
            else
            {
                Directory.CreateDirectory(e.LocalPath);
                Directory.SetLastWriteTimeUtc(e.LocalPath, metadata.ModifiedTime);
            }
            Models.SQLDataType sdt = new Models.SQLDataType(e.LocalPath,
                metadata.ModifiedTime,
                metadata.Rev,
                metadata.Modifier.Email,
                metadata.isShared ? "true" : "false");
            Util.SqliteHelper.Insert(sdt);
        }

        private void ServerChange()
        {
            Models.Metadata metadata = NetworkManager.GetMetadata(e.CloudPath);
            if (metadata.Tag.Equals("File"))
            {
                Util.Utils.UnLockFile(e.LocalPath);
                while (!NetworkManager.Download(e.CloudPath))
                    Thread.Sleep(5000);
                metadata = NetworkManager.GetMetadata(e.CloudPath);
                File.SetLastWriteTimeUtc(e.LocalPath, metadata.ModifiedTime);
                if (metadata.isShared)
                    Util.Utils.LockFile(e.LocalPath);
                Models.SQLDataType sdt = new Models.SQLDataType(e.LocalPath,
                    metadata.ModifiedTime,
                    metadata.Rev,
                    metadata.Modifier.Email,
                    metadata.isShared ? "true" : "false");
                Util.SqliteHelper.Update(sdt);
            }
        }

        private void ServerRename()
        {
            Models.Metadata metadata = NetworkManager.GetMetadata(e.CloudPath);
            if (metadata.Tag.Equals("File"))
            {
                Util.Utils.UnLockFile(e.LocalPath);
                bool retry = true;
                while (retry)
                {
                    try
                    {
                        new FileInfo(e.OldLocalPath).MoveTo(e.LocalPath);
                        retry = false;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5000);
                    }
                }
                if (metadata.isShared)
                    Util.Utils.LockFile(e.LocalPath);
            }
            else
            {
                bool retry = true;
                while (retry)
                {
                    try
                    {
                        new DirectoryInfo(e.OldLocalPath).MoveTo(e.LocalPath);
                        retry = false;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            Models.SQLDataType sdt = Util.SqliteHelper.Select(e.OldLocalPath);
            sdt.Path = e.LocalPath;
            Util.SqliteHelper.Delete(e.OldLocalPath);
            Util.SqliteHelper.Insert(sdt);
        }

        private void ServerDelete()
        {
            if (File.Exists(e.LocalPath))
            {
                Util.Utils.UnLockFile(e.LocalPath);
                bool retry = true;
                while (retry)
                {
                    try
                    {
                        new FileInfo(e.LocalPath).Attributes = FileAttributes.Normal;
                        File.Delete(e.LocalPath);
                        retry = false;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            else
            {
                Util.Utils.DeleteFolder(e.LocalPath);
            }
            Util.SqliteHelper.Delete(e.LocalPath);
        }
    }
}
