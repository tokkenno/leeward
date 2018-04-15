using System.IO;
using Leeward.Core;

namespace Leeward.Protocol.Packets
{
    /// <summary>
    /// Send zone host configuration to player
    /// </summary>
    internal class ResponseSetHostPacket : ResponsePacket
    {
        public readonly int PlayerId;
        
        public ResponseSetHostPacket(Player player) : base(PacketType.ResponseSetHost)
        {
            this.PlayerId = player.Id;
        }

        public override string ToString()
        {
            return $"Packet(ResponseSetHost) => PlayerId: {this.PlayerId}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.PlayerId);
        }
    }
}