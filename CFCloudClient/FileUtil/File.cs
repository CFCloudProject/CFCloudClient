using System;
using System.Collections.Generic;
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
    }
}
