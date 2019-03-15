using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class Chunk
    {
        public uint start;
        public uint length;
        public ulong cut_fingerprint;
    }
}
