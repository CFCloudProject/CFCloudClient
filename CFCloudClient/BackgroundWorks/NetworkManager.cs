using Grpc.Core;
using GRPCServer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class NetworkManager
    {
        private static Channel channel = new Channel(Properties.Resources.Host, ChannelCredentials.Insecure);
        private static Channel heartBeatChannel = new Channel(Properties.Resources.HeartBeatHost, ChannelCredentials.Insecure);

        public static NetworkResults.RegisterResult Register(Models.User user)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            var response = client.Register(
                new User()
                {
                    Email = user.Email,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });

            NetworkResults.RegisterResult rr = new NetworkResults.RegisterResult();
            rr.Fail = NetworkResults.RegisterResult.FailType.Unknown;
            rr.Succeed = false;

            if (response != null)
            {
                rr.Succeed = response.Succeed;
                if (!response.Succeed && response.Error == 1)
                    rr.Fail = NetworkResults.RegisterResult.FailType.EmailExist;
            }

            return rr;
        }

        public static NetworkResults.LoginResult Login(Models.User user)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            var response = client.Login(
                new User()
                {
                    Email = user.Email,
                    Password = user.Password
                });

            NetworkResults.LoginResult lr = new NetworkResults.LoginResult();
            lr.Succeed = false;
            lr.Fail = NetworkResults.LoginResult.FailType.Unknown;
            lr.info = new Models.LoginInfo();

            if (response != null)
            {
                lr.Succeed = response.Succeed;
                if (response.Succeed)
                {
                    lr.info.user.Email = response.Email;
                    lr.info.user.Password = response.Password;
                    lr.info.user.FirstName = response.FirstName;
                    lr.info.user.LastName = response.LastName;
                    lr.info.SessionId = response.SessionId;
                }
                else
                {
                    if (response.Error == 1)
                        lr.Fail = NetworkResults.LoginResult.FailType.EmailNotExist;
                    if (response.Error == 2)
                        lr.Fail = NetworkResults.LoginResult.FailType.PwdError;
                }
            }
            
            return lr;
        }

        public static void Logout()
        {
            Util.Global.updater.WaitForAllThreadEnd();

            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            var response = client.Logout(
                new EmptyRequest()
                {
                    SessionId = Util.Global.info.SessionId
                });
            
            BackgroundWorks.HeartBeat.Stop();
            channel.ShutdownAsync().Wait();
            heartBeatChannel.ShutdownAsync().Wait();
        }

        public static List<Models.FileChangeEvent> HeartBeat()
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(heartBeatChannel);
            var response = client.HeartBeat(  //timeout setting
                new EmptyRequest()
                {
                    SessionId = Util.Global.info.SessionId
                });

            if (response == null)
                return null;
            if (response.PayLoad == null || response.PayLoad.Equals(""))
                return null;
            JObject obj = JObject.Parse(response.PayLoad);
            JArray eventsArray = (JArray)obj["Entries"];
            if (eventsArray.Count == 0)
                return null;

            List<Models.FileChangeEvent> eventsList = new List<Models.FileChangeEvent>();
            foreach (var item in eventsArray)
            {
                int type = int.Parse(item["Type"].ToString());
                string path = item["Path"].ToString();
                string oldPath = item["OldPath"].ToString();
                Models.FileChangeEvent e = new Models.FileChangeEvent(
                    new Func<Models.FileChangeEvent.FileChangeType>(() => {
                        switch (type)
                        {
                            case 1:
                                return Models.FileChangeEvent.FileChangeType.ServerCreate;
                            case 2:
                                return Models.FileChangeEvent.FileChangeType.ServerChange;
                            case 3:
                                return Models.FileChangeEvent.FileChangeType.ServerRename;
                            default:
                                return Models.FileChangeEvent.FileChangeType.ServerDelete;
                        }
                    }).Invoke(), Util.Utils.CloudPathtoLocalPath(path), Util.Utils.CloudPathtoLocalPath(oldPath));
                eventsList.Add(e);
            }
            return eventsList;
        }

        //to be continue
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
