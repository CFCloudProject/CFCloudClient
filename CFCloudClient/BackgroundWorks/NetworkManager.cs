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
        private static bool Online = true;

        public static NetworkResults.RegisterResult Register(Models.User user)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            RegisterResult response = null;

            NetworkResults.RegisterResult rr = new NetworkResults.RegisterResult();
            rr.Fail = NetworkResults.RegisterResult.FailType.Unknown;
            rr.Succeed = false;

            try
            {
                response = client.Register(
                    new User
                    {
                        Email = user.Email,
                        Password = user.Password,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    });
            }
            catch (RpcException)
            {
                rr.Succeed = false;
                rr.Fail = NetworkResults.RegisterResult.FailType.Unknown;
                return rr;
            }
            
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
            LoginResult response = null;

            NetworkResults.LoginResult lr = new NetworkResults.LoginResult();
            lr.Succeed = false;
            lr.Fail = NetworkResults.LoginResult.FailType.Unknown;
            lr.info = new Models.LoginInfo();

            try
            {
                response = client.Login(
                    new User
                    {
                        Email = user.Email,
                        Password = user.Password
                    });
            }
            catch (RpcException)
            {
                return lr;
            }

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
            StringRespone response = null;
            try
            {
                response = client.Logout(
                new EmptyRequest
                {
                    SessionId = Util.Global.info.SessionId
                });
            }
            catch (RpcException)
            {

            }
            
            BackgroundWorks.HeartBeat.Stop();
            channel.ShutdownAsync().Wait();
            heartBeatChannel.ShutdownAsync().Wait();
        }

        public static List<Models.FileChangeEvent> HeartBeat()
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(heartBeatChannel);
            StringRespone response = null;
            try
            {
                response = client.HeartBeat(
                    new EmptyRequest
                    {
                        SessionId = Util.Global.info.SessionId
                    });
            }
            catch (RpcException)
            {
                if (Online)
                    Online = false;
                Util.Global.manualResetEvent.Reset();
                return null;
            }

            if (!Online)
            {
                Online = true;
                Util.Global.manualResetEvent.Set();
            }

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
        
        public static Models.Metadata Share(string path, string email)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringRespone response = null;
            try
            {
                response = client.Share(
                    new ShareRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path,
                        Dst = email
                    });
            }
            catch (RpcException)
            {
                return null;
            }

            return Models.Metadata.FromJson(response.PayLoad);
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
