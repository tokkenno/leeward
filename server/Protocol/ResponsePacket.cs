using System.IO;

namespace Leeward.Protocol
{
    internal abstract class ResponsePacket : Packet
    {
        public ResponsePacket(PacketType type) : base(type)
        {
        }

        public byte[] ToBinary()
        {
            using (MemoryStream data = new MemoryStream())
            using (BinaryWriter bwData = new BinaryWriter(data))
            {
                // Size empty
                bwData.Write(0);
                
                // Packet type
                bwData.Write((byte) this.Type);
                
                // Packet body
                this.Write(bwData);
                
                // Rewind and set size
                data.Position = 0;
                bwData.Write((uint)data.Length);
                
                return data.ToArray();
            }
        }

        protected abstract void Write(BinaryWriter outWriter);
    }
}