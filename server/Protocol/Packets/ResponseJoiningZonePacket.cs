using System;
using System.IO;
using System.Linq;
using Leeward.Core;
using Leeward.IO;

namespace Leeward.Protocol.Packets
{
    /// <summary>
    /// Send zone joining response to player
    /// </summary>
    internal class ResponseJoiningZonePacket : ResponsePacket
    {
        public readonly Zone Zone;
        
        public ResponseJoiningZonePacket(Zone zone) : base(PacketType.ResponseJoiningZone)
        {
            this.Zone = zone;
        }

        public override string ToString()
        {
            String players = string.Join(", ", this.Zone.Players.Select((player) => $"{player.Name} ({player.Id})"));
            return $"Packet(ResponseJoiningZone) => ZoneId: {this.Zone.Id}, Name: {this.Zone.Name}, Players: {players}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.Zone.Id);
            outWriter.Write(this.Zone.PlayersCount);

            foreach (Player player in this.Zone.Players)
            {
                outWriter.Write(player.Id);
                outWriter.Write(string.IsNullOrEmpty(player.Name) ? "Guest" : player.Name);
                outWriter.Write(player.Data ?? new byte[]{ 0x00 });
            }
        }
    }
}