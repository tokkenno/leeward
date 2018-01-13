using System;

namespace Leeward.Protocol.Packets
{
    internal class RequestIdPacket : Packet
    {
        public readonly String Name;
        
        public RequestIdPacket(String name) : base(PacketType.RequestID)
        {
            this.Name = name.Trim(); // TODO: Check
        }
    }
}