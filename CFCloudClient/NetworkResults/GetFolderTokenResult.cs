using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.NetworkResults
{
    public class GetFolderTokenResult
    {
        public bool Succeed { get; set; }
        public List<Models.FileAndEditorMap> TokenHolders { get; set; }

        public string FailMessage()
        {
            string message = "Some users are editing some files:";
            foreach (Models.FileAndEditorMap map in TokenHolders)
            {
                message += "\n" + map.Message();
            }
            return message;
        }
    }
}
