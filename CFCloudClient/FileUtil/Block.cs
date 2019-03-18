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
        public string sha256 { get; set; }
        public string md5 { get; set; }
        public int start { get; set; }
        public int length { get; set; }

        public string SHA256()
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] ret = sha256.ComputeHash(data);
            return (Encoding.UTF8.GetString(ret));
        }

        public string MD5()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] ret = md5.ComputeHash(data);
            return (Encoding.UTF8.GetString(ret));
        }
    }
}
