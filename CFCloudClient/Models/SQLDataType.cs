using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class SQLDataType
    {
        public string Path { get; set; }
        public string ModifiedTime { get; set; }
        public string Version { get; set; }
        public string Modifier { get; set; }
        public string IsShared { get; set; }

        public SQLDataType(string path, string modifiedTime, string version, string modifier, string isShared)
        {
            Path = path;
            ModifiedTime = modifiedTime;
            Version = version;
            Modifier = modifier;
            IsShared = isShared;
        }

        public DateTime getModifiedTime()
        {
            return new DateTime(long.Parse(ModifiedTime), DateTimeKind.Utc);
        }
    }
}
