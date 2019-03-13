using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class Metadata
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public long size { get; set; }
        public string Rev { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public User Modifier { get; set; }
        public User Owner { get; set; }
        public bool isShared { get; set; }
        public List<User> SharedUsers { get; set; }
        public User TokenHolder { get; set; }

        public static Metadata FromJson(string json)
        {
            JToken token = JToken.Parse(json);
            return FromJson(token);
        }

        //to be continue
        public static Metadata FromJson(JToken json)
        {
            Metadata metadata = new Metadata();
            return metadata;
        }
    }
}
