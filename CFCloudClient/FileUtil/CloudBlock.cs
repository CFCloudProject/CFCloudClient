using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class CloudBlock
    {
        public int index { get; set; }
        public string sha256 { get; set; }
        public string md5 { get; set; }
    }
}
