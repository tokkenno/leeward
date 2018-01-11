using System;
using System.CodeDom;
using System.IO;

namespace Leeward.Protocol
{
    internal static class PacketHandler
    {
        public static Packet Handle(MemoryStream data)
        {
            using (BinaryReader dataReader = new BinaryReader(data))
            {
                switch (dataReader.ReadUInt32())
                {
                    case (uint) PacketType.RequestID: return HandleRequestId(dataReader);
                    default: throw new Exception("Incorrect packet"); // TODO: Custom exception
                }
            }
        }

        public static RequestIdPacket HandleRequestId(BinaryReader dr)
        {
            String name = dr.ReadString();
            Object data = (Object) dr.Read(); // TODO: What the hell is this?
            
            return new RequestIdPacket(
                name
            );
        }
    }
}