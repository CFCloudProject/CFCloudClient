using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class LoginInfo
    {
        public User user { get; set; }
        public string SessionId { get; set; }

        public LoginInfo()
        {
            user = new User();
        }
    }
}
