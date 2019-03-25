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
        public string TempPath { get; set; }
        public string Rev { get; set; }
        public List<Chunk> chunks { get; set; }
        private FileStream stream;
        private FileStream _tempstream;
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
                _tempstream = new FileStream(TempPath, FileMode.Create);
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
            if (_tempstream != null)
                _tempstream.Close();
            if (System.IO.File.Exists(TempPath))
                System.IO.File.Delete(TempPath);
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
            block.index = index + 1;
            ++index;
            block.adler32 = block.Adler32();
            block.md5 = block.MD5();
            block.start = (int)chunks[index].start;
            block.length = len;
            return block;
        }

        public byte[] ReadBlock(int start, int len)
        {
            stream.Seek(start, SeekOrigin.Begin);
            byte[] ret = new byte[len];
            stream.Read(ret, 0, len);
            return ret;
        }

        public void WriteTemp(byte[] data)
        {
            _tempstream.Write(data, 0, data.Length);
        }

        public void WriteFile()
        {
            stream.SetLength(0);
            _tempstream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[1024 * 1024];
            int count;
            while ((count = _tempstream.Read(buffer, 0, 1024 * 1024)) > 0)
            {
                stream.Write(buffer, 0, count);
            }
            stream.Flush();
        }
    }
}
