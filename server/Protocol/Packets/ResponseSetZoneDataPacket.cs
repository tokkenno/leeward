using System;
using System.IO;
using Leeward.IO;

namespace Leeward.Protocol.Packets
{
    internal class ResponseSetZoneDataPacket : ResponsePacket
    {
        public readonly string ZoneData;
        
        public ResponseSetZoneDataPacket(String data) : base(PacketType.ResponseSetZoneData)
        {
            this.ZoneData = data;
        }

        public override string ToString()
        {
            return $"Packet(ResponseSetZoneData) => ZoneData?: {this.ZoneData}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.ZoneData);
        }
    }
}