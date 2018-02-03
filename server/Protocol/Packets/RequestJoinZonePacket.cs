using System;

namespace Leeward.Protocol.Packets
{
    internal class RequestJoinZonePacket : Packet
    {
        public readonly int Id;
        public readonly string Name;
        public readonly string Password;
        public readonly int MaxPlayers;
        public readonly bool Persistent;
        
        public RequestJoinZonePacket(int id, string name, string password, int maxPlayers, bool persistent) : base(PacketType.RequestJoinZone)
        {
            this.Id = id;
            this.Name = name.Trim();
            this.Password = password;
            this.MaxPlayers = maxPlayers > 0 ? maxPlayers : 1;
            this.Persistent = persistent;
        }

        public bool IsLastVersion()
        {
            return this.Id == 10000 && this.Password.Equals("1508310"); // TODO: Find what its 1508310, 2015/08/31?
        }

        public override string ToString()
        {
            return $"Packet(RequestJoinZone) => Id: {this.Id}, Name: {this.Name}, Password: {this.Password}, MaxPlayers: {this.MaxPlayers}, Persistent?: {this.Persistent}"; 
        }
    }
}