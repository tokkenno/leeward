using System;
using System.IO;

namespace Leeward.Protocol.Packets
{
    /// <summary>
    /// Send zone name to player
    /// </summary>
    internal class ResponseLoadLevelPacket : ResponsePacket
    {
        public readonly String Name;
        
        public ResponseLoadLevelPacket(String name) : base(PacketType.ResponseLoadLevel)
        {
            this.Name = string.IsNullOrEmpty(name) ? "" : name;
        }

        public override string ToString()
        {
            return $"Packet(ResponseLoadLevel) => Name: {this.Name}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Name);
        }
    }
}