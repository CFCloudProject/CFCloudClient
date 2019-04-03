using Grpc.Core;
using GRPCServer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class NetworkManager
    {
        private static string _SessionId;
        private static Channel channel = new Channel(Properties.Resources.Host, ChannelCredentials.Insecure);
        //private static Channel heartBeatChannel = new Channel(Properties.Resources.HeartBeatHost, ChannelCredentials.Insecure);
        //private static bool Online = true;

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
            lr.user = new Models.User();

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
                    lr.user.Email = user.Email;
                    lr.user.Password = user.Password;
                    lr.user.FirstName = response.FirstName;
                    lr.user.LastName = response.LastName;
                    _SessionId = response.SessionId;
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
                    SessionId = _SessionId
                });
            }
            catch (RpcException)
            {

            }
            
            //BackgroundWorks.HeartBeat.Stop();
            channel.ShutdownAsync().Wait();
            //heartBeatChannel.ShutdownAsync().Wait();
        }

        /*public static List<Models.FileChangeEvent> HeartBeat()
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(heartBeatChannel);
            StringResponse response = null;
            try
            {
                response = client.HeartBeat(new EmptyRequest
                {
                    SessionId = _SessionId
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
            JArray eventsArray = (JArray)obj["entries"];
            if (eventsArray.Count == 0)
                return null;

            List<Models.FileChangeEvent> eventsList = new List<Models.FileChangeEvent>();
            Func<int, Models.FileChangeEvent.FileChangeType> getType = delegate(int type)
            {
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
            };
            foreach (var item in eventsArray)
            {
                int type = int.Parse(item["type"].ToString());
                string path = item["path"].ToString();
                string oldPath = item["oldpath"].ToString();
                Models.FileChangeEvent e = new Models.FileChangeEvent(
                    getType(type), Util.Utils.CloudPathtoLocalPath(path), Util.Utils.CloudPathtoLocalPath(oldPath));
                eventsList.Add(e);
            }
            return eventsList;
        }*/
        
        public static Models.Metadata Share(string path, string email)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            try
            {
                response = client.Share(new ShareRequest
                {
                    SessionId = _SessionId,
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
                        SessionId = _SessionId,
                        Path = path,
                        ModifiedTime = (new DirectoryInfo(Util.Utils.CloudPathtoLocalPath(path)).LastWriteTimeUtc.Ticks - 621355968000000000) / 10000
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
                        SessionId = _SessionId,
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
                        SessionId = _SessionId,
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
        
        public static Models.Metadata Upload(string path, string baseRev = null, string localpath = null)
        {
            if (localpath == null)
                localpath = Util.Utils.CloudPathtoLocalPath(path);
            long modified_time = (new FileInfo(localpath).LastWriteTimeUtc.Ticks - 621355968000000000) / 10000;
            FileUtil.File UploadFile = new FileUtil.File();
            UploadFile.Path = localpath;
            if (!UploadFile.OpenRead())
                return null;
            string ottype = UploadFile.isBinary() ? "b" : "s";
            UploadFile.CDC_Chunking();
            string hashs = "";
            while (UploadFile.hasNextBlock())
            {
                FileUtil.Block block = UploadFile.getNextBlock();
                hashs += block.adler32 + block.md5;
            }
            UploadFile.reset();
            
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            try
            {
                response = client.Upload(new UploadRequest
                {
                    SessionId = _SessionId,
                    Path = path,
                    BaseRev = baseRev,
                    Hashs = hashs
                });
            }
            catch (RpcException)
            {
                return null;
            }

            if (response == null)
                return null;
            string _hashs = response.PayLoad;
            int hashcount = _hashs.Length / 20;
            List<string> cloudhashs = new List<string>();
            for (int i = 0; i < hashcount; i++)
            {
                cloudhashs.Add(_hashs.Substring(20 * i, 20));
            }

            try { 
                var reqres = client.UploadBlock();
                var requests = reqres.RequestStream;
                while (UploadFile.hasNextBlock())
                {
                    FileUtil.Block block = UploadFile.getNextBlock();
                    string hash = block.adler32 + block.md5;
                    Google.Protobuf.ByteString content;
                    if (cloudhashs.Contains(hash))
                        content = Google.Protobuf.ByteString.CopyFrom(block.data);
                    else
                        content = null;
                    BlockRequest blockRequest = new BlockRequest
                    {
                        SessionId = _SessionId,
                        Path = path,
                        ModifiedTime = modified_time,
                        BaseRev = baseRev,
                        OtType = ottype,
                        Index = block.index,
                        Hash = hash,
                        Size = block.length,
                        Content = content
                    };
                    requests.WriteAsync(blockRequest).Wait();
                }
                requests.CompleteAsync().Wait();
                UploadFile.Close();
                return Models.Metadata.FromJson(reqres.ResponseAsync.Result.PayLoad);
            }
            catch (RpcException)
            {
                UploadFile.Close();
                return null;
            }
        }
        
        public static bool Download(string path)
        {
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            try
            {
                response = client.Download(new PathRequest
                {
                    SessionId = _SessionId,
                    Path = path
                });
            }
            catch (RpcException)
            {
                return false;
            }

            if (response == null)
                return false;
            JObject obj = JObject.Parse(response.PayLoad);
            string rev = obj["rev"].ToString();
            JArray hashs = (JArray)obj["hashs"];
            List<FileUtil.CloudBlock> cloudBlocks = new List<FileUtil.CloudBlock>();
            for (int i = 0; i < hashs.Count; i++)
            {
                cloudBlocks.Add(new FileUtil.CloudBlock
                {
                    index = i,
                    adler32 = hashs[i].ToString().Substring(0, 4),
                    md5 = hashs[i].ToString().Substring(4, 16)
                });
            }

            FileUtil.File DownloadFile = new FileUtil.File();
            DownloadFile.Path = Util.Utils.CloudPathtoLocalPath(path);
            DownloadFile.TempPath = "temp\\" + path.Replace('/', '_') + ".temp";
            DownloadFile.Rev = rev;
            if (!DownloadFile.OpenWrite())
                return false;
            DownloadFile.CDC_Chunking();
            Dictionary<string, FileUtil.Block> localBlocks = new Dictionary<string, FileUtil.Block>();
            while (DownloadFile.hasNextBlock())
            {
                var block = DownloadFile.getNextBlock();
                block.data = null;
                localBlocks.Add(block.adler32 + block.md5, block);
            }

            var reqres = client.DownloadBlock();
            var requests = reqres.RequestStream;
            foreach (var block in cloudBlocks)
            {
                if (!localBlocks.ContainsKey(block.adler32 + block.md5))
                {
                    try
                    {
                        requests.WriteAsync(new BlockRequest
                        {
                            SessionId = _SessionId,
                            Path = path,
                            BaseRev = rev,
                            Hash = block.adler32 + block.md5,
                        }).Wait();
                    }
                    catch (RpcException)
                    {
                        DownloadFile.Close();
                        return false;
                    }
                }
            }
            requests.CompleteAsync().Wait();

            string resp = reqres.ResponseAsync.Result.PayLoad;
            JObject robj = JObject.Parse(resp);
            JArray blocks = (JArray)robj["blocks"];
            Dictionary<string, string> clouddatas = new Dictionary<string, string>();
            foreach (var item in blocks)
            {
                clouddatas.Add(item["hash"].ToString(), item["data"].ToString());
            }
            foreach (var block in cloudBlocks)
            {
                if (localBlocks.ContainsKey(block.adler32 + block.md5))
                {
                    var lb = localBlocks[block.adler32 + block.md5];
                    DownloadFile.WriteTemp(DownloadFile.ReadBlock(lb.start, lb.length));
                }
                else
                {
                    if (clouddatas.ContainsKey(block.adler32 + block.md5))
                    {
                        char[] clouddata = clouddatas[block.adler32 + block.md5].ToCharArray();
                        byte[] data = new byte[clouddata.Length / 2];
                        for (int i = 0; i < clouddata.Length / 2; i++)
                        {
                            char b1 = clouddata[2 * i];
                            char b2 = clouddata[2 * i + 1];
                            if (b1 >= '0' && b1 <= '9')
                                data[i] = (byte)(b1 - '0');
                            else
                                data[i] = (byte)(b1 - 'a' + 10);
                            data[i] *= 16;
                            if (b2 >= '0' && b2 <= '9')
                                data[i] += (byte)(b2 - '0');
                            else
                                data[i] += (byte)(b2 - 'a' + 10);
                        }
                        DownloadFile.WriteTemp(data);
                    }
                    else
                        return false;
                }
            }
            DownloadFile.WriteFile();
            DownloadFile.Close();

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
                        SessionId = _SessionId,
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
                        SessionId = _SessionId,
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
            JArray MetadataArray = (JArray)obj["entries"];
            foreach (var item in MetadataArray)
            {
                metadataList.Add(Models.Metadata.FromJson(item));
            }
            return metadataList;
        }
    }
}
