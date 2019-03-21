using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileChangeMonitor
    {
        private FileSystemWatcher watcher = new FileSystemWatcher();

        public FileChangeMonitor()
        {
            watcher.Path = Properties.Settings.Default.Workspace;
            watcher.NotifyFilter = NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.Security
                | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
            watcher.Created += OnCreate;
            watcher.Changed += OnChange;
            watcher.Renamed += OnRename;
            watcher.Deleted += OnDelete;
        }

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientCreate, e.FullPath);
            Util.Global.FileUpdateQueue.Add(fce);
            OnFileChange(null, null);
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientChange, e.FullPath);
            Util.Global.FileUpdateQueue.Add(fce);
            OnFileChange(null, null);
        }

        private void OnRename(object sender, RenamedEventArgs e)
        {
            Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientRename, e.FullPath, e.OldFullPath);
            Util.Global.FileUpdateQueue.Add(fce);
            OnFileChange(null, null);
        }

        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            Models.FileChangeEvent fce = new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ClientDelete, e.FullPath);
            Util.Global.FileUpdateQueue.Add(fce);
            OnFileChange(null, null);
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public event EventHandler OnFileChange;
    }
}
