using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class CDC
    {
        const ulong POLYNOMIAL = 0x3DA3358B4DC173;
        const int POLYNOMIAL_DEGREE = 53;
        const int WINSIZE = 64;
        const int AVERAGE_BITS = 13;
        const int MINSIZE = 1024;
        const int MAXSIZE = 64 * 1024;

        const int MASK = ((1 << AVERAGE_BITS) - 1);
        const int POL_SHIFT = POLYNOMIAL_DEGREE - 8;

        byte[] window = new byte[WINSIZE];
        uint wpos;
        uint count;
        uint pos;
        uint start;
        ulong digest;

        bool tables_initialized = false;
        ulong[] mod_table = new ulong[256];
        ulong[] out_table = new ulong[256];

        public Chunk last_chunk = new Chunk();

        public void rabin_init()
        {
            if (!tables_initialized)
            {
                calc_tables();
                tables_initialized = true;
            }

            rabin_reset();
        }

        private void rabin_reset()
        {
            for (int i = 0; i < WINSIZE; i++)
                window[i] = 0;
            digest = 0;
            wpos = 0;
            count = 0;
            digest = 0;

            rabin_slide(1);
        }

        private void rabin_slide(byte b)
        {
            byte o = window[wpos];
            window[wpos] = b;
            digest = (digest ^ out_table[o]);
            wpos = (wpos + 1) % WINSIZE;
            rabin_append(b);
        }

        private void rabin_append(byte b)
        {
            byte index = (byte)(digest >> POL_SHIFT);
            digest <<= 8;
            digest |= (ulong)b;
            digest ^= mod_table[index];
        }

        public int rabin_next_chunk(byte[] buf, int start_index, int len)
        {
            for (uint i = 0; i < len; i++)
            {
                byte b = buf[i + start_index];

                rabin_slide(b);

                count++;
                pos++;

                if ((count >= MINSIZE && ((digest & MASK) == 0)) || count >= MAXSIZE)
                {
                    last_chunk.start = start;
                    last_chunk.length = count;
                    last_chunk.cut_fingerprint = digest;

                    // keep position
                    uint position = pos;
                    rabin_reset();
                    start = position;
                    pos = position;

                    return (int)i + 1;
                }
            }

            return -1;
        }

        public Chunk rabin_finalize()
        {
            if (count == 0)
            {
                last_chunk.start = 0;
                last_chunk.length = 0;
                last_chunk.cut_fingerprint = 0;
                return null;
            }

            last_chunk.start = start;
            last_chunk.length = count;
            last_chunk.cut_fingerprint = digest;
            return last_chunk;
        }

        private int deg(ulong p)
        {
            ulong mask = 0x8000000000000000;

            for (int i = 0; i < 64; i++)
            {
                if ((mask & p) > 0)
                {
                    return 63 - i;
                }

                mask >>= 1;
            }

            return -1;
        }

        private ulong mod(ulong x, ulong p)
        {
            while (deg(x) >= deg(p))
            {
                int shift = deg(x) - deg(p);
                x = x ^ (p << shift);
            }

            return x;
        }

        private ulong append_byte(ulong hash, byte b, ulong pol)
        {
            hash <<= 8;
            hash |= (ulong)b;

            return mod(hash, pol);
        }

        private void calc_tables()
        {
            // calculate table for sliding out bytes. The byte to slide out is used as
            // the index for the table, the value contains the following:
            // out_table[b] = Hash(b || 0 ||        ...        || 0)
            //                          \ windowsize-1 zero bytes /
            // To slide out byte b_0 for window size w with known hash
            // H := H(b_0 || ... || b_w), it is sufficient to add out_table[b_0]:
            //    H(b_0 || ... || b_w) + H(b_0 || 0 || ... || 0)
            //  = H(b_0 + b_0 || b_1 + 0 || ... || b_w + 0)
            //  = H(    0     || b_1 || ...     || b_w)
            //
            // Afterwards a new byte can be shifted in.
            for (int b = 0; b < 256; b++)
            {
                ulong hash = 0;

                hash = append_byte(hash, (byte)b, POLYNOMIAL);
                for (int i = 0; i < WINSIZE - 1; i++)
                {
                    hash = append_byte(hash, 0, POLYNOMIAL);
                }
                out_table[b] = hash;
            }

            // calculate table for reduction mod Polynomial
            int k = deg(POLYNOMIAL);
            for (int b = 0; b < 256; b++)
            {
                // mod_table[b] = A | B, where A = (b(x) * x^k mod pol) and  B = b(x) * x^k
                //
                // The 8 bits above deg(Polynomial) determine what happens next and so
                // these bits are used as a lookup to this table. The value is split in
                // two parts: Part A contains the result of the modulus operation, part
                // B is used to cancel out the 8 top bits so that one XOR operation is
                // enough to reduce modulo Polynomial
                mod_table[b] = mod(((ulong)b) << k, POLYNOMIAL) | ((ulong)b) << k;
            }
        }
    }
}
