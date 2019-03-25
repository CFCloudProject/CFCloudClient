using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class Block
    {
        public byte[] data;
        public int index { get; set; }
        public string adler32 { get; set; }
        public string md5 { get; set; }
        public int start { get; set; }
        public int length { get; set; }

        public string Adler32()
        {
            int n;
            uint s1 = 1 & 0xFFFF;
            uint s2 = 1 >> 16;

            int pos = 0;
            int remain = data.Length;

            while (remain > 0)
            {
                n = (3800 > remain) ? remain : 3800;
                remain -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (uint)(data[pos++] & 0xFF);
                    s2 = s2 + s1;
                }
                s1 %= 65521;
                s2 %= 65521;
            }

            byte[] ret = new byte[4];
            ret[0] = (byte)(s2 >> 8);
            ret[1] = (byte)s2;
            ret[2] = (byte)(s1 >> 8);
            ret[3] = (byte)s1;

            StringBuilder str = new StringBuilder();
            foreach (byte b in ret)
            {
                str.Append(b.ToString("x2"));
            }
            return str.ToString();
        }

        public string MD5()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] ret = md5.ComputeHash(data);
            StringBuilder str = new StringBuilder();
            foreach (byte b in  ret)
            {
                str.Append(b.ToString("x2"));
            }
            return str.ToString();
        }
    }
}
