using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.Models
{
    public class FileAndEditorMap
    {
        public string Name { get; set; }
        public User TokenHolder { get; set; }

        public string Message()
        {
            return Util.Utils.CloudPathtoLocalPath(Name) + " is editing by " + TokenHolder.getUserName() + ".";
        }
    }
}
