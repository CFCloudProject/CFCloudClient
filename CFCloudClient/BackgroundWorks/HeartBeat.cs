using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class HeartBeat
    {
        private static bool stop = false;

        public static void Stop()
        {
            stop = true;
        }

        public static void Start()
        {
            /*new Thread(new ThreadStart(() =>
            {
                while (!stop)
                {
                    List<Models.FileChangeEvent> notifications = NetworkManager.HeartBeat();
                    if (notifications != null)
                    {
                        foreach (Models.FileChangeEvent e in notifications)
                        {
                            Util.Global.FileUpdateQueue.Add(e);
                        }
                    }
                }
            })).Start();*/
        }
    }
}
