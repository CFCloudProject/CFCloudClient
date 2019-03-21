using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Equal(User user)
        {
            return Email == user.Email;
        }

        public string getUserName()
        {
            return FirstName + " " + LastName;
        }

        public static User FromJson(string json)
        {
            JToken token = JToken.Parse(json);
            return FromJson(token);
        }

        public static User FromJson(JToken json)
        {
            User user = new User();
            user.Email = json["Email"].ToString();
            user.FirstName = json["FirstName"].ToString();
            user.LastName = json["LastName"].ToString();
            return user;
        }
    }
}
