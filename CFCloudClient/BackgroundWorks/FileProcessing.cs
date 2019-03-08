using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileProcessing
    {
        public static List<string> Processing = new List<string>();

        public static bool Add(string file)
        {
            lock (Processing)
            {
                if (Processing.Contains(file))
                    return false;
                else
                {
                    Processing.Add(file);
                    return true;
                }
            }
        }

        public static bool Remove(string file)
        {
            lock (Processing)
            {
                if (Processing.Contains(file))
                {
                    Processing.Remove(file);
                    return true;
                }
                else
                    return false;
            }
        }

        public static bool isInProcessing(string file)
        {
            lock (Processing)
            {
                return Processing.Contains(file);
            }
        }
    }
}
