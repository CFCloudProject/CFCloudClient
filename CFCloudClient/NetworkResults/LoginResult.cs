using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.NetworkResults
{
    public class LoginResult
    {
        public bool Succeed { get; set; }
        public Models.LoginInfo info { get; set; }
        public enum FailType { EmailNotExist, PwdError, Unknown }
        public FailType Fail { get; set; }
    }
}
