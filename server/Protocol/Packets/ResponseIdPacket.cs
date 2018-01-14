using System;
using System.ComponentModel;
using System.IO;

namespace Leeward.Protocol.Packets
{
    internal class ResponseIdPacket : ResponsePacket
    {
        public readonly int Id;
        public readonly DateTime ConnectedAt;
        
        public ResponseIdPacket(int id, DateTime connectedAt) : base(PacketType.ResponseID)
        {
            this.Id = id;
            this.ConnectedAt = connectedAt;
        }

        protected override void Write(BinaryWriter outWriter)
        {
            if (this.Id >= 0)
            {
                outWriter.Write(12);
                outWriter.Write(this.Id);
                outWriter.Write(this.ConnectedAt.Ticks / 10000L);
            }
            else
            {
                outWriter.Write(0);
            }
        }

        public override string ToString()
        {
            return $"Packet(ResponseId) <= Id: {this.Id}, Connected: {this.ConnectedAt}"; 
        }

        public static ResponsePacket RejectPlayer()
        {
            return new ResponseIdPacket(-1, default(DateTime));
        }
    }
}