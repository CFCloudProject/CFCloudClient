using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class FileSystemItem
    {
        public string Name { get; set; }
        public string Modified { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Operation { get; set; }
        public string EditButton { get; set; }

        public FileSystemItem(string path, int type)
        {
            Name = path.Substring(path.LastIndexOf('\\') + 1);
            Modified = Directory.GetLastWriteTime(path).ToString();
            if (type == 0)
            {
                Type = "Folder";
                Size = null;
                Operation = "Open";
                EditButton = "Hidden";
            }
            else
            {
                Type = "";
                int pos = Name.LastIndexOf('.');
                if (pos != -1)
                    Type = Name.Substring(pos + 1) + " ";
                Type += "File";
                long size = new FileInfo(path).Length;
                if (size < 1024)
                    Size = size + "Byte";
                else
                {
                    size /= 1024;
                    if (size < 1024)
                        Size = size + "KB";
                    else
                    {
                        size /= 1024;
                        if (size < 1024)
                            Size = size + "MB";
                        else
                        {
                            size /= 1024;
                            Size = size + "GB";
                        }
                    }
                }
                Operation = "View";
                EditButton = "Visible";
            }
        }
    }
}
