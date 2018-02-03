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
            outWriter.Write(StringToBytes(this.Name));
        }

        public override string ToString()
        {
            return $"Packet(PlayerConnected) <= Id: {this.Id}, Name: {this.Name}"; 
        }
    }
}