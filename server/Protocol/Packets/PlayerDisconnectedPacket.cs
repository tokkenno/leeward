using System.IO;
using System.Text;

namespace Leeward.Protocol.Packets
{
    internal class PlayerDisconnectedPacket : ResponsePacket
    {
        public readonly int Id;
        public readonly string Name;
        
        public PlayerDisconnectedPacket(int id, string name) : base(PacketType.PlayerDisconnected)
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
            return $"Packet(PlayerDisconnected) <= Id: {this.Id}, Name: {this.Name}"; 
        }
    }
}