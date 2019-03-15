using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.FileUtil
{
    public class File
    {
        public string Path { get; set; }
        public string Rev { get; set; }
        public List<Chunk> chunks { get; set; }
        private FileStream stream;
        private int index = 0;

        public bool OpenRead()
        {
            try
            {
                stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public bool OpenWrite()
        {
            try
            {
                stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            stream.Close();
        }

        public void CDC_Chunking()
        {
            chunks = new List<Chunk>();
            byte[] buf = new byte[1024 * 1024];
            CDC cdc = new CDC();
            cdc.rabin_init();
            while (stream.Position < stream.Length)
            {
                int len = stream.Read(buf, 0, 1024 * 1024);
                int start = 0;
                while (true)
                {
                    int remaining = cdc.rabin_next_chunk(buf, start, len);
                    if (remaining < 0)
                        break;
                    len -= remaining;
                    start += remaining;
                    chunks.Add(new Chunk
                    {
                        start = cdc.last_chunk.start,
                        length = cdc.last_chunk.length,
                        cut_fingerprint = cdc.last_chunk.cut_fingerprint
                    });
                }
            }

            if (cdc.rabin_finalize() != null)
            {
                chunks.Add(new Chunk
                {
                    start = cdc.last_chunk.start,
                    length = cdc.last_chunk.length,
                    cut_fingerprint = cdc.last_chunk.cut_fingerprint
                });
            }

            stream.Seek(0, SeekOrigin.Begin);
        }

        public bool hasNextBlock()
        {
            return index < chunks.Count;
        }

        public Block getNextBlock()
        {
            int len = (int)chunks[index].length;
            Block block = new Block();
            block.data = new byte[len];
            stream.Read(block.data, 0, len);
            block.index = index;
            block.sha256 = block.SHA256();
            block.md5 = block.MD5();
            return block;
        }
    }
}
