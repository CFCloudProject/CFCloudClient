using Grpc.Core;
using GRPCServer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                response = client.Register(new User
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
                response = client.Login(new User
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
            StringResponse response = null;
            try
            {
                response = client.Logout(new EmptyRequest
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
            StringResponse response = null;
            try
            {
                response = client.HeartBeat(new EmptyRequest
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
            StringResponse response = null;
            try
            {
                response = client.Share(new ShareRequest
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

        public static Models.Metadata CreateFolder(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.CreateFolder(new PathRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }

            return Models.Metadata.FromJson(response.PayLoad);
        }

        public static Models.Metadata Rename(string path, string oldPath)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.Rename(new RenameRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path,
                        OldPath = oldPath
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }

            return Models.Metadata.FromJson(response.PayLoad);
        }

        public static Models.Metadata Delete(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.Delete(new PathRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }

            return Models.Metadata.FromJson(response.PayLoad);
        }

        //to be continue
        public static Models.Metadata Upload(string path, string baseRev = null)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            try
            {
                response = client.Upload(new PathRevRequest
                {
                    SessionId = Util.Global.info.SessionId,
                    Path = path,
                    BaseRev = baseRev
                });
            }
            catch (RpcException)
            {
                return null;
            }

            if (response == null)
                return null;
            JObject obj = JObject.Parse(response.PayLoad);
            string rev = obj["Rev"].ToString();
            JArray blocks = (JArray)obj["Blocks"];
            List<FileUtil.CloudBlock> cloudBlocks = new List<FileUtil.CloudBlock>();
            for (int i = 0; i < blocks.Count; i++)
            {
                FileUtil.CloudBlock cb = new FileUtil.CloudBlock
                {
                    index = i,
                    sha256 = blocks[i]["SHA256"].ToString(),
                    md5 = blocks[i]["MD5"].ToString()
                };
                cloudBlocks.Add(cb);
            }

            FileUtil.File UploadFile = new FileUtil.File();
            UploadFile.Path = Util.Utils.CloudPathtoLocalPath(path);
            UploadFile.Rev = rev;
            if (!UploadFile.OpenRead())
                return null;
            UploadFile.CDC_Chunking();
            try
            {
                var requests = client.UploadBlock().RequestStream;
                while (UploadFile.hasNextBlock())
                {
                    FileUtil.Block block = UploadFile.getNextBlock();
                    var ret = cloudBlocks.Find((b) =>
                    {
                        return b.sha256.Equals(block.sha256) && b.md5.Equals(block.md5);
                    });
                    if (ret != null)
                    {
                        BlockRequest blockRequest = new BlockRequest
                        {
                            SessionId = Util.Global.info.SessionId,
                            Path = path,
                            BaseRev = baseRev,
                            Rev = rev,
                            BaseIndex = ret.index,
                            Index = block.index,
                            SHA256 = block.sha256,
                            MD5 = block.md5,
                            Content = null
                        };
                    }
                }
                requests.CompleteAsync().Wait();
            }
            catch (RpcException)
            {
                UploadFile.Close();
                return null;
            }

            Models.Metadata metadata = GetMetadata(path);
            return metadata;
        }

        //to be continue
        public static bool Download(string path)
        {
            return true;
        }

        public static Models.Metadata GetMetadata(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.GetMetadata(new PathRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }

            return Models.Metadata.FromJson(response.PayLoad);
        }

        public static List<Models.Metadata> ListFolder(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.ListFolder(new PathRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }

            List<Models.Metadata> metadataList = new List<Models.Metadata>();

            JObject obj = JObject.Parse(response.PayLoad);
            JArray MetadataArray = (JArray)obj["Entries"];
            foreach (var item in MetadataArray)
            {
                metadataList.Add(Models.Metadata.FromJson(item));
            }
            return metadataList;
        }
        
        public static NetworkResults.GetTokenResult GetToken(string path, string Rev)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            GetTokenResult response = null;

            NetworkResults.GetTokenResult gr = new NetworkResults.GetTokenResult();
            gr.Succeed = false;
            gr.TokenHolder = new Models.FileAndEditorMap();
            gr.Fail = NetworkResults.GetTokenResult.FailType.Unknown;

            try
            {
                response = client.GetToken(new PathRevRequest
                {
                    SessionId = Util.Global.info.SessionId,
                    Path = path,
                    BaseRev = Rev
                });
            }
            catch (RpcException)
            {
                return null;
            }

            if (response != null)
            {
                gr.Succeed = response.Succeed;
                gr.TokenHolder.Name = response.FileName;
                gr.TokenHolder.TokenHolder = new Models.User
                {
                    Email = response.Email,
                    Password = response.Password,
                    FirstName = response.FirstName,
                    LastName = response.LastName
                };
                if (!gr.Succeed)
                {
                    if (response.Error == 1)
                        gr.Fail = NetworkResults.GetTokenResult.FailType.OtherHolding;
                    if (response.Error == 2)
                        gr.Fail = NetworkResults.GetTokenResult.FailType.UnConsistent;
                }
            }

            return gr;
        }

        public static void ReturnToken(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            bool retry = true;
            while (retry)
            {
                try
                {
                    response = client.ReturnToken(new PathRequest
                    {
                        SessionId = Util.Global.info.SessionId,
                        Path = path
                    });
                }
                catch (RpcException)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                retry = false;
            }
        }
        
        public static NetworkResults.GetFolderTokenResult GetFolderToken(string folder)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;

            try
            {
                response = client.CanModifyFolder(new PathRequest
                {
                    SessionId = Util.Global.info.SessionId,
                    Path = folder
                });
            }
            catch (RpcException)
            {
                return null;
            }

            if (response == null)
                return null;
            return NetworkResults.GetFolderTokenResult.FromJson(response.PayLoad);
        }
    }
}
