using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Leeward.Protocol.Packets;

namespace Leeward.Protocol
{
    internal static class PacketHandler
    {
        public static List<Packet> Handle(MemoryStream data)
        {
            List<Packet> packets = new List<Packet>();
            
            Console.WriteLine("BIN   ==> " + BitConverter.ToString(data.ToArray()));
            Console.WriteLine("ASCII ==> " + Encoding.ASCII.GetString(data.ToArray()));
            
            try
            {
                using (BinaryReader dataReader = new BinaryReader(data))
                {
                    uint expectedSize = dataReader.ReadUInt32();

                    if (expectedSize == 542393671) // TODO: Add other HTTP common headers
                    {
                        data.Position = 0;
                        packets.Add(new HttpRequest(data));
                    }
                    else
                    {
                        uint code = dataReader.ReadByte();

                        switch (code)
                        {
                            case (uint) PacketType.RequestID:
                                packets.Add(HandleRequestId(dataReader));
                                break;
                            case (uint) PacketType.RequestSetAlias:
                                packets.Add(HandleRequestSetAlias(dataReader));
                                break;
                            default: throw new UnrecognizedPacketException(code, data.Length);
                        }

                        if (expectedSize > data.Length)
                        {
                            packets.AddRange(Handle(data));
                        }
                    }
                }
            }
            catch (ArgumentException er)
            {
                Console.WriteLine("The packet can't be readed: " + data.CanRead); // TODO: DEBUG ONLY, DELETE
            }

            return packets;
        }

        public static RequestIdPacket HandleRequestId(BinaryReader dr)
        {
            if (dr.ReadInt32() != 12)
                throw new PacketMalformedException("Second parameter not expected");
            
            return new RequestIdPacket(
                dr.ReadString()
            );
        }

        public static RequestSetAliasPacket HandleRequestSetAlias(BinaryReader dr)
        {
            return new RequestSetAliasPacket(
                dr.ReadString()
            );
        }
    }
}