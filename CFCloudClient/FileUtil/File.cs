using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class File
    {
        public string Path { get; set; }
        public string Rev { get; set; }
        public List<FileBlock> blocks { get; set; }
        private FileStream stream;

        public bool OpenRead()
        {
            try
            {
                stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public bool OpenWrite()
        {
            try
            {
                stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            stream.Close();
        }


    }
}
