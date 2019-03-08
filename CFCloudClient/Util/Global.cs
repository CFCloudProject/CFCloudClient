using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.Util
{
    public class Global
    {
        public static Models.LoginInfo info = new Models.LoginInfo();
        public static Dictionary<string, FileStream> LockedFiles = new Dictionary<string, FileStream>();
        public static BackgroundWorks.FileChangeMonitor FileMonitor;
        public static BackgroundWorks.FileChangeQueue FileUpdateQueue = new BackgroundWorks.FileChangeQueue();
        public static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public static BackgroundWorks.FileUpdater updater = new BackgroundWorks.FileUpdater();
    }
}
