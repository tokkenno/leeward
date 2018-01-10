namespace Leeward.Protocol
{
    internal abstract class Packet
    {
        public readonly PacketType Type;

        protected Packet(PacketType type)
        {
            this.Type = type;
        }
    }
}