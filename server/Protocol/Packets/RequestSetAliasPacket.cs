using System;

namespace Leeward.Protocol.Packets
{
    internal class RequestSetAliasPacket : Packet
    {
        public readonly string Alias;
        
        public RequestSetAliasPacket(string alias) : base(PacketType.RequestSetAlias)
        {
            this.Alias = alias.Trim(); // TODO: Check
        }

        public override string ToString()
        {
            return $"Packet(RequestSetAlias) => Alias: {this.Alias}"; 
        }
    }
}