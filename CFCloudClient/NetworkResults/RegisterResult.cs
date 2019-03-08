using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.NetworkResults
{
    public class RegisterResult
    {
        public bool Succeed { get; set; }
        public enum FailType { EmailExist, Unknown }
        public FailType Fail { get; set; }
    }
}
