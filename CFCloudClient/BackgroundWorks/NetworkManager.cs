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
        }
        
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
        
        public static Models.Metadata Upload(string path, string baseRev = null)
        {
            var info = new FileInfo(Util.Utils.CloudPathtoLocalPath(path));
            var client = new GRPCServer.GRPCServer.GRPCServerClient(channel);
            StringResponse response = null;
            try
            {
                response = client.Upload(new UploadRequest
                {
                    SessionId = _SessionId,
                    Path = path,
                    BaseRev = baseRev,
                    ModifiedTime = info.LastWriteTimeUtc.Ticks,
                    Size = info.Length
                });
            }
            catch (RpcException)
            {
                return null;
            }

            if (response == null)
                return null;
            JObject obj = JObject.Parse(response.PayLoad);
            string rev = obj["rev"].ToString();
            JArray blocks = (JArray)obj["blocks"];
            Dictionary<string, Dictionary<string, FileUtil.CloudBlock>> cloudBlocks = new Dictionary<string, Dictionary<string, FileUtil.CloudBlock>>();
            for (int i = 0; i < blocks.Count; i++)
            {
                FileUtil.CloudBlock cb = new FileUtil.CloudBlock
                {
                    index = i,
                    adler32 = blocks[i]["adler32"].ToString(),
                    md5 = blocks[i]["md5"].ToString()
                };
                if (cloudBlocks.ContainsKey(cb.adler32))
                {
                    cloudBlocks[cb.adler32].Add(cb.md5, cb);
                }
                else
                {
                    cloudBlocks.Add(cb.adler32, new Dictionary<string, FileUtil.CloudBlock>());
                    cloudBlocks[cb.adler32].Add(cb.md5, cb);
                }
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
                List<BlockRequest> WaitRequests = new List<BlockRequest>();
                int lastIndex = 0;
                int maxIndex = blocks.Count;
                while (UploadFile.hasNextBlock())
                {
                    FileUtil.Block block = UploadFile.getNextBlock();
                    FileUtil.CloudBlock ret = null;
                    if (cloudBlocks.ContainsKey(block.adler32))
                    {
                        if (cloudBlocks[block.adler32].ContainsKey(block.md5))
                            ret = cloudBlocks[block.adler32][block.md5];
                    }
                    if (ret != null)
                    {
                        BlockRequest blockRequest = new BlockRequest
                        {
                            SessionId = _SessionId,
                            Path = path,
                            BaseRev = baseRev,
                            Rev = rev,
                            BaseIndex = ret.index.ToString(),
                            Index = block.index,
                            Confident = true,
                            Hash = block.adler32 + block.md5,
                            Content = null
                        };
                        if (WaitRequests.Count > 0 && ret.index > lastIndex)
                        {
                            string baseIndex;
                            if (ret.index - lastIndex == 1)
                                baseIndex = null;
                            else
                            {
                                baseIndex = (lastIndex + 1).ToString();
                                for (int i = lastIndex + 2; i < ret.index; ++i)
                                {
                                    baseIndex += "|" + i;
                                }
                            }
                            foreach (var request in WaitRequests)
                            {
                                request.BaseIndex = baseIndex;
                                request.Confident = false;
                                requests.WriteAsync(request).Wait();
                            }
                            WaitRequests.Clear();
                            lastIndex = ret.index;
                        }
                        requests.WriteAsync(blockRequest).Wait();
                    }
                    else
                    {
                        BlockRequest blockRequest = new BlockRequest
                        {
                            SessionId = _SessionId,
                            Path = path,
                            BaseRev = baseRev,
                            Rev = rev,
                            Index = block.index,
                            Hash = block.adler32 + block.md5,
                            Content = Google.Protobuf.ByteString.CopyFrom(block.data)
                        };
                        WaitRequests.Add(blockRequest);
                    }
                }
                if (WaitRequests.Count != 0)
                {
                    string baseIndex;
                    if (maxIndex - lastIndex <= 1)
                        baseIndex = null;
                    else
                    {
                        baseIndex = (lastIndex + 1).ToString();
                        for (int i = lastIndex + 2; i < maxIndex; ++i)
                        {
                            baseIndex += "|" + i;
                        }
                        foreach (var request in WaitRequests)
                        {
                            request.BaseIndex = baseIndex;
                            request.Confident = false;
                            requests.WriteAsync(request).Wait();
                        }
                        WaitRequests.Clear();
                    }
                }
                requests.CompleteAsync().Wait();
                UploadFile.Close();
            }
            catch (RpcException)
            {
                UploadFile.Close();
                return null;
            }

            Models.Metadata metadata = GetMetadata(path);
            return metadata;
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
            JArray blocks = (JArray)obj["blocks"];
            List<FileUtil.CloudBlock> cloudBlocks = new List<FileUtil.CloudBlock>();
            for (int i = 0; i < blocks.Count; i++)
            {
                cloudBlocks.Add(new FileUtil.CloudBlock
                {
                    index = i,
                    adler32 = blocks[i]["adler32"].ToString(),
                    md5 = blocks[i]["md5"].ToString()
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
            var responses = reqres.ResponseStream;

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
                            Rev = rev,
                            Index = block.index,
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
            foreach (var block in cloudBlocks)
            {
                if (localBlocks.ContainsKey(block.adler32 + block.md5))
                {
                    var lb = localBlocks[block.adler32 + block.md5];
                    DownloadFile.WriteTemp(DownloadFile.ReadBlock(lb.start, lb.length));
                }
                else
                {
                    if (responses.MoveNext().Result)
                        DownloadFile.WriteTemp(responses.Current.Content.ToByteArray());
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
