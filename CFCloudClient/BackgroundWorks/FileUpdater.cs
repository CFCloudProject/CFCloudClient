using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileUpdater
    {
        private long ThreadCount;
        private const long MaxThreadCount = 20;
        private Thread UploaderThread;
        private bool stop = false;

        public FileUpdater()
        {
            ThreadCount = 0;
            UploaderThread = new Thread(new ThreadStart(() =>
            {
                while (!stop)
                {
                    if (ThreadCount < MaxThreadCount)
                    {
                        Models.FileChangeEvent e = Util.Global.FileUpdateQueue.GetAnEvent();
                        if (e != null)
                        {
                            FileProcessing.Add(e.ProcessingPath());
                            Interlocked.Increment(ref ThreadCount);
                            FileUpdaterProcesser processer = new FileUpdaterProcesser(e);
                            new Thread(new ThreadStart(processer.run)).Start();
                        }
                    }
                    Thread.Sleep(100);
                    Util.Global.manualResetEvent.WaitOne();
                }
            }));
            UploaderThread.Start();
        }

        public void WaitForAllThreadEnd()
        {
            while (Interlocked.Read(ref ThreadCount) != 0)
                Thread.Sleep(1000);
        }

        public void Destory()
        {
            Util.Global.manualResetEvent.Set();
            WaitForAllThreadEnd();
            stop = true;
        }

        public void ThreadEnd()
        {
            Interlocked.Decrement(ref ThreadCount);
        }
    }
}
