using System.IO;

namespace Leeward.Protocol.Packets
{
    internal class ResponseLeaveZonePacket : ResponsePacket
    {
        public ResponseLeaveZonePacket() : base(PacketType.ResponseLeaveZone)
        {
        }

        public override string ToString()
        {
            return $"Packet(ResponseJoinZone)"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
        }
    }
}