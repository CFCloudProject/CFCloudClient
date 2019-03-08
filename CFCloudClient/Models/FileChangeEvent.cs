using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class FileChangeEvent
    {
        public enum FileChangeType
        {
            ClientCreate,
            ClientChange,
            ClientRename,
            ClientDelete,
            ServerCreate,
            ServerChange,
            ServerRename,
            ServerDelete,
            ClientRenameAndClientChange,
            ClientRenameAndServerChange,
            ServerRenameAndClientChange,
            ServerRenameAndServerChange
        }

        public FileChangeType Type { get; set; }
        public string LocalPath { get; set; }
        public string CloudPath { get; set; }
        public string OldLocalPath { get; set; }
        public string OldCloudPath { get; set; }

        public FileChangeEvent(FileChangeType type, string path, string oldPath = null)
        {
            Type = type;
            LocalPath = path;
            CloudPath = Util.Utils.LocalPathtoCloudPath(path);
            OldLocalPath = oldPath;
            OldCloudPath = Util.Utils.LocalPathtoCloudPath(oldPath);
        }

        public string ProcessingPath()
        {
            switch (Type)
            {
                case FileChangeType.ServerRename:
                case FileChangeType.ServerRenameAndClientChange:
                case FileChangeType.ServerRenameAndServerChange:
                    return OldLocalPath;
                default:
                    return LocalPath;
            }
        }
    }
}
