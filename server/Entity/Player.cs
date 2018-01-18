using System;
using System.Collections.Generic;
using Leeward.Net;
using Leeward.Protocol.Packets;
using Leeward.Utils;

namespace Leeward.Entity
{
    internal delegate void PlayerMessageEventHandler(Player player, System.IO.MemoryStream message);

    internal class Player
    {
        private static readonly SequentialIdGenerator UserIdGenerator = new SequentialIdGenerator();
            
        public int Id { get; }

        public string Name { get; }

        private readonly List<string> _aliases = new List<string>();

        public DateTime ConnectedAt { get; }

        public readonly PlayerConnection Connection;

        public event PlayerMessageEventHandler OnMessage;

        public Player(string name, PlayerConnection connection)
        {
            this.Connection = connection;
            this.Id = Player.UserIdGenerator.Next();
            this.Name = name;
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