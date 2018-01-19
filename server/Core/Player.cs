using System;
using System.Collections.Generic;
using Leeward.Net;
using Leeward.Protocol.Packets;
using Leeward.Utils;

namespace Leeward.Core
{
    internal delegate void PlayerMessageEventHandler(Player player, System.IO.MemoryStream message);

    internal class Player
    {
        private static readonly SequentialIdGenerator UserIdGenerator = new SequentialIdGenerator();
            
        public int Id { get; }

        public string Name { get; }

        private readonly List<string> _aliases = new List<string>();

        public DateTime ConnectedAt { get; }

        private Zone _currentZone = null;
        public Zone CurrentZone
        {
            get => this._currentZone;
        }

        public readonly PlayerConnection Connection;

        public event PlayerMessageEventHandler OnMessage;

        public Player(string name, PlayerConnection connection)
        {
            this.Connection = connection;
            this.Id = Player.UserIdGenerator.Next();
            this.Name = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();
            this.ConnectedAt = DateTime.Now;

            connection.OnMessage +=
                new MessageEventHandler(((inputConnection, message) => OnMessage?.Invoke(this, message)));
        }

        ~Player()
        {
            Player.UserIdGenerator.Free(this.Id);
        }

        public void SetAlias(string alias)
        {
            lock (_aliases)
            {
                if (!_aliases.Contains(alias))
                    this._aliases.Add(alias); // TODO: Check user unique alias globally?
            }
        }

        public bool Is(string alias)
        {
            lock (_aliases)
            {
                return _aliases.Contains(alias);
            }
        }

        public void JoinZone(Zone newZone)
        {
            if (this.CurrentZone != newZone)
            {
                this._currentZone = newZone;
                newZone.JoinPlayer(this);
            }
        }

        public void LeaveZone()
        {
            if (this.CurrentZone != null)
            {
                this.CurrentZone.LeavePlayer(this);
                this._currentZone = null;
                this.Send((new ResponseLeaveZonePacket()).ToBinary());
            }
        }

        public void Send(byte[] data) => this.Connection.Send(data);

        public void SendResponseId()
        {
            this.Connection.Send((new ResponseIdPacket(this.Id, this.ConnectedAt)).ToBinary());
        }

        public void SendPlayerConnected()
        {
            this.Connection.Send((new PlayerConnectedPacket(this.Id, this.Name)).ToBinary());
        }
    }
}