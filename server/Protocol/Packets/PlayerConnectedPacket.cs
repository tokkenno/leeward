using System;
using System.IO;
using System.Text;

namespace Leeward.Protocol.Packets
{
    internal class PlayerConnectedPacket : ResponsePacket
    {
        public readonly int Id;
        public readonly string Name;
        
        public PlayerConnectedPacket(int id, string name) : base(PacketType.PlayerConnected)
        {
            this.Id = id;
            this.Name = name;
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Id);
            outWriter.Write(Encoding.ASCII.GetBytes(this.Name));
        }
    }
}