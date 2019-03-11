using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class NetworkManager
    {
        public static NetworkResults.LoginResult Login(Models.User user)
        {
            NetworkResults.LoginResult lr = new NetworkResults.LoginResult();
            lr.info = new Models.LoginInfo();
            lr.info.user = user;
            lr.info.user.FirstName = "Chen";
            lr.info.user.LastName = "Jian";
            lr.Succeed = true;
            return lr;
        }

        public static void Logout()
        {
            BackgroundWorks.HeartBeat.Stop();
        }

        public static List<Models.FileChangeEvent> HeartBeat()
        {
            return null;
        }

        public static NetworkResults.RegisterResult Register(Models.User user)
        {
            NetworkResults.RegisterResult rr = new NetworkResults.RegisterResult();
            rr.Succeed = true;
            return rr;
        }

        public static Models.Metadata Share(string path, string email)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static Models.Metadata Create(string path)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static Models.Metadata Rename(string path, string oldPath)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static Models.Metadata Delete(string path)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static Models.Metadata Upload(string path, string baseRev = null)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static bool Download(string path)
        {
            return true;
        }

        public static Models.Metadata GetMetadata(string path)
        {
            Models.Metadata metadata = new Models.Metadata();
            return metadata;
        }

        public static List<Models.Metadata> ListFolder(string path)
        {
            List<Models.Metadata> ListFolderResult = new List<Models.Metadata>();
            return ListFolderResult;
        }

        public static NetworkResults.GetTokenResult GetToken(string path)
        {
            NetworkResults.GetTokenResult gr = new NetworkResults.GetTokenResult();
            return null;
        }

        public static void ReturnToken(string path)
        {

        }

        public static NetworkResults.GetFolderTokenResult GetFolderToken(string folder)
        {
            NetworkResults.GetFolderTokenResult gfr = new NetworkResults.GetFolderTokenResult();
            return gfr;
        }
    }
}
