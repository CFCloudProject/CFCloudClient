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
        public static Models.User user = new Models.User();
        public static BackgroundWorks.FileChangeMonitor FileMonitor;
        public static BackgroundWorks.FileChangeQueue FileUpdateQueue;
        public static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public static BackgroundWorks.FileUpdater updater;
    }
}
