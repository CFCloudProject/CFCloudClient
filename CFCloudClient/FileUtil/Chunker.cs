using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class Chunker
    {
        //Config:
        uint minSize = 1024;
        uint maxSize = 32 * 1024;
        ulong pol;
        uint polShift;

        ulong[] Out = new ulong[256];
        ulong[] Mod = new ulong[256];

        bool Initialized;
        ulong splitmask;

        FileStream stream;
        bool closed;

        //State:
        byte[] window = new byte[64];
        int wpos;
        byte[] buf;
        uint bpos;
        uint bmax;
        uint start;
        uint count;
        uint pos;
        uint pre;
        ulong digest;

        public Chunker(FileStream stream, ulong pol)
        {
            this.stream = stream;
            this.pol = pol;
            this.buf = new byte[1024];
            this.splitmask = (1 << 12) - 1;
            reset();
        }

        private void reset()
        {
            polShift = (uint)(Polynomials.Deg(pol) - 8);
            fillTables();

            for (int i = 0; i < 64; i++)
                window[i] = 0;
            closed = false;
            digest = 0;
            wpos = 0;
            count = 0;

        }

        private void fillTables()
        {
            if (pol == 0)
                return;
            Initialized = true;
            for (int b = 0; b < 256; b++)
            {
                ulong h = 0;
                h = appendByte(h, (byte)b, pol);
                for (int i = 0; i < 63; i++)
                    h = appendByte(h, 0, pol);
                Out[b] = h;
            }
            int k = Polynomials.Deg(pol);
            for (int b = 0; b < 256; b++)
            {
                Mod[b] = Polynomials.Mod((ulong)b << k, pol) | ((ulong)b << k);
            }
        }

        private ulong appendByte(ulong hash, byte b, ulong pol)
        {
            hash <<= 8;
            hash |= b;
            return Polynomials.Mod(hash, pol);
        }

        
    }
}
