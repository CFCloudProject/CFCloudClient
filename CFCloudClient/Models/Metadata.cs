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

        public static Metadata FromJson(string json)
        {
            JToken token = JToken.Parse(json);
            return FromJson(token);
        }
        
        public static Metadata FromJson(JToken json)
        {
            Metadata metadata = new Metadata();
            metadata.Tag = json["tag"].ToString();
            metadata.Name = json["name"].ToString();
            metadata.FullPath = json["fullpath"].ToString();
            metadata.size = long.Parse(json["size"].ToString());
            metadata.Rev = json["rev"].ToString();
            metadata.CreationTime = new DateTime(long.Parse(json["creation_time"].ToString()), DateTimeKind.Utc);
            metadata.ModifiedTime = new DateTime(long.Parse(json["modified_time"].ToString()), DateTimeKind.Utc);
            metadata.Modifier = User.FromJson(json["modifier"]);
            metadata.Owner = User.FromJson(json["owner"]);
            metadata.isShared = json["is_shared"].ToString().Equals("true");
            metadata.SharedUsers = new List<User>();
            JArray sharedUsers = (JArray)json["sharedusers"];
            foreach (var item in sharedUsers)
            {
                metadata.SharedUsers.Add(User.FromJson(item));
            }
            return metadata;
        }
    }
}
