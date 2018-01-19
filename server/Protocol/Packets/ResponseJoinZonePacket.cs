using System;
using System.IO;

namespace Leeward.Protocol.Packets
{
    internal class ResponseJoinZonePacket : ResponsePacket
    {
        public readonly bool Joined; // ??
        public readonly string Message;
        
        public ResponseJoinZonePacket(bool joined, string message) : base(PacketType.ResponseJoinZone)
        {
            this.Joined = joined;
            this.Message = message.Trim();
        }

        public override string ToString()
        {
            return $"Packet(ResponseJoinZone) => Joined?: {this.Joined}, Message: {this.Message}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Joined);
            outWriter.Write(this.Message);
        }
    }
}