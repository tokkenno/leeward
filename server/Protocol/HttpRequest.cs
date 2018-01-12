using System.IO;

namespace Leeward.Protocol
{
    internal class HttpRequest : Packet
    {
        public HttpRequest(MemoryStream data) : base(PacketType.HttpRequest)
        {
        }
    }
}