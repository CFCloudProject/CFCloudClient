using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.NetworkResults
{
    public class GetTokenResult
    {
        public bool Succeed { get; set; }
        public Models.FileAndEditorMap TokenHolder { get; set; }
        public enum FailType { OtherHolding, UnConsistent, Unknown };
        public FailType Fail;
    }
}
