using System;

namespace Leeward.Protocol
{
    public class PacketMalformedException : Exception
    {
        public PacketMalformedException(string reason) : base(reason)
        {
        }
    }
}