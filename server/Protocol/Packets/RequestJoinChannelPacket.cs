using System;

namespace Leeward.Protocol.Packets
{
    internal class RequestJoinChannelPacket : Packet
    {
        public readonly int Id;
        public readonly string Name;
        public readonly string Password;
        public readonly int MaxPlayers;
        public readonly bool Persistent;
        
        public RequestJoinChannelPacket(int id, string name, string password, int maxPlayers, bool persistent) : base(PacketType.RequestJoinChannel)
        {
            this.Id = id;
            this.Name = name.Trim();
            this.Password = password;
            this.MaxPlayers = maxPlayers > 0 ? maxPlayers : 1;
            this.Persistent = persistent;
        }

        public override string ToString()
        {
            return $"Packet(RequestJoinChannel) => Id: {this.Id}, Name: {this.Name}, Password: {this.Password}, MaxPlayers: {this.MaxPlayers}, Persistent?: {this.Persistent}"; 
        }
    }
}