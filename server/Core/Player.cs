using System;
using System.Collections.Generic;
using Leeward.Net;
using Leeward.Protocol;
using Leeward.Protocol.Packets;
using Leeward.Utils;

namespace Leeward.Core
{
    internal delegate void PlayerMessageEventHandler(Player player, System.IO.MemoryStream message);

    internal class Player
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(Player));
        private static readonly SequentialIdGenerator UserIdGenerator = new SequentialIdGenerator();
            
        public int Id { get; }

        public string Name { get; }

        private readonly List<string> _aliases = new List<string>();

        public DateTime ConnectedAt { get; }

        private byte[] _data = null;
        public byte[] Data
        {
            get => this._data;
        }

        private Zone _currentZone = null;
        public Zone CurrentZone
        {
            get => this._currentZone;
        }

        public readonly InputConnection Connection;

        public event PlayerMessageEventHandler OnMessage;

        public Player(string name, InputConnection connection)
        {
            this.Connection = connection;
            this.Id = Player.UserIdGenerator.NextInt();
            this.Name = string.IsNullOrWhiteSpace(name) ? "Guest" : name.Trim();
            this.ConnectedAt = DateTime.Now;

            connection.OnMessage +=
                new MessageEventHandler(((inputConnection, message) => OnMessage?.Invoke(this, message)));
        }

        ~Player()
        {
            Player.UserIdGenerator.FreeInt(this.Id);
        }

        public void SetAlias(string alias)
        {
            lock (_aliases)
            {
                if (alias != null && !_aliases.Contains(alias))
                    this._aliases.Add(alias); // TODO: Check user unique alias globally
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
                this.LeaveZone();
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
                this.Send(new ResponseLeaveZonePacket());
            }
        }

        public void Disconnect()
        {
            this.Send(new PlayerDisconnectedPacket((int) this.Id, this.Name));
            this.Connection.Disconnect();
        } 

        public void Send(ResponsePacket packet)
        {
            _logger.Trace($"Server => Player: {packet.ToString()}");
            this.Connection.Send(packet.ToBinary());
        }

        public void SendResponseId()
        {
            this.Send(new ResponseIdPacket((int) this.Id, this.ConnectedAt));
        }

        public void SendPlayerConnected()
        {
            this.Send(new PlayerConnectedPacket((int) this.Id, this.Name));
        }
    }
}