using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class FileBlock
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public long BlockHash { get; set; }
        public string Content { get; set; }
    }
}
