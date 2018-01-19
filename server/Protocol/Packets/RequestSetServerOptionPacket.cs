using System.IO;
using Leeward.IO;

namespace Leeward.Protocol.Packets
{
    internal class RequestSetServerOptionPacket : ResponsePacket
    {
        public readonly Configuration Config;

        public RequestSetServerOptionPacket(Configuration config) : base(PacketType.RequestSetServerOption)
        {
            this.Config = config;
        }

        public override string ToString()
        {
            return $"Packet(RequestSetServerOptionPacket) => Config: {this.Config.ToString()}";
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Config);
        }
    }
}