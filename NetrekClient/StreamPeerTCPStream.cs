using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetrekGodot.NetrekClient
{
    class StreamPeerTCPStream : Stream
    {
        private StreamPeerTCP tcp;
        public StreamPeerTCPStream(StreamPeerTCP tcp)
        {
            this.tcp = tcp;
        }
        public int AvailableBytes { get => tcp.GetAvailableBytes(); }

        private long position = 0;
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => -1;

        public override long Position { get => position; set => throw new NotImplementedException(); }

        public override void Flush()
        {
            // Don't do anything
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] data = GodotError.Assert<byte[]>(tcp.GetData(count));
            data.CopyTo(buffer, offset);
            return data.Length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            GodotError.Assert(tcp.PutData(buffer.Skip(offset).Take(count).ToArray()));
        }
    }
}
