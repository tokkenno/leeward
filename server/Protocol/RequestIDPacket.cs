namespace Leeward.Protocol
{
    internal class RequestIdPacket : Packet
    {
        public RequestIdPacket() : base(PacketType.RequestID)
        {
        }
    }
}