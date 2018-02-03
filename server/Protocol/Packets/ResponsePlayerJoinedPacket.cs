using System;
using System.IO;
using System.Text;
using Leeward.Core;

namespace Leeward.Protocol.Packets
{
    internal class ResponsePlayerJoinedPacket : ResponsePacket
    {
        public readonly int Id;
        public readonly String Name;
        public readonly byte[] Payload;
        
        public ResponsePlayerJoinedPacket(Player p) : base(PacketType.ResponsePlayerJoined)
        {
            this.Id = p.Id;
            this.Name = p.Name;
            this.Payload = new byte[] {}; // TODO: Check if neccesary
        }

        public override string ToString()
        {
            return $"Packet(ResponsePlayerJoined) => Id: {this.Id}, Name: {this.Name}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Id);
            outWriter.Write(StringToBytes(this.Name));
            
            if (this.Payload.Length == 0)
                outWriter.Write((byte) 0);
            else
                outWriter.Write(this.Payload);
        }
    }
}