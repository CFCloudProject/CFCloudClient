using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class Polynomials
    {
        public static int Deg(ulong x)
        {
            if (x == 0)
                return -1;
            int r = 0;
            if ((x & 0xffffffff00000000) > 0)
            {
                x >>= 32;
                r |= 32;
            }
            if ((x & 0xffff0000) > 0)
            {
                x >>= 16;
                r |= 16;
            }
            if ((x & 0xff00) > 0)
            {
                x >>= 8;
                r |= 8;
            }
            if ((x & 0xf0) > 0)
            {
                x >>= 4;
                r |= 4;
            }
            if ((x & 0xc) > 0)
            {
                x >>= 2;
                r |= 2;
            }
            if ((x & 0x2) > 0)
            {
                x >>= 1;
                r |= 1;
            }
            return r;
        }

        public static ulong Mod(ulong x, ulong y)
        {
            if (x == 0)
                return 0;
            while (x >= y)
            {
                x -= y;
            }
            return x;
        }
    }
}
