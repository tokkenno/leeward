using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leeward.Protocol.Packets;
using Leeward.Utils;

namespace Leeward.Core
{
    internal class Zone
    {
        private const int MaxPlayersGlobal = 64;
        
        private static readonly RandomIdGenerator ZoneIdGenerator = new RandomIdGenerator(gap: 10000);

        public readonly int Id;

        private string _name;
        private string _password;
        private string _data;
        private int _maxPlayers;
        private bool _persistent;
        private bool _locked;
        
        /// <summary>
        /// Player who host the zone
        /// </summary>
        private Player _host;

        private List<Player> _players;
        
        public String Name
        {
            get => this._name;
        }
        
        public String Password
        {
            get => this._password;
        }
        
        public Boolean HasPassword
        {
            get => string.IsNullOrWhiteSpace(this._password);
        }
        
        public bool IsOpen {
            get { throw new NotImplementedException(); }
        }

        public List<Player> Players => this._players;
        public int PlayersCount => this._players.Count;

        public Zone(String name, String password = "", int maxPlayers = 1, bool persistent = false, int id = -1)
        {
            this.Id = id == -1 ? ZoneIdGenerator.NextInt() : id;
            this._name = name;
            this._password = password;
            this._maxPlayers = maxPlayers > 0 ? (maxPlayers > MaxPlayersGlobal ? MaxPlayersGlobal : maxPlayers) : 1;
            this._persistent = persistent;
        }
        
        ~Zone()
        {
            Zone.ZoneIdGenerator.FreeInt(this.Id);
        }

        public void JoinPlayer(Player player)
        {
            // Send zone join info to player
            player.JoinZone(this);
            player.Send(new ResponseJoiningZonePacket(this));

            // Send zone host config
            lock (this._host)
            {
                if (this._host == null) this._host = player;
            }
            player.Send(new ResponseSetHostPacket(this._host));

            // Send zone data
            if (!String.IsNullOrEmpty(this._data))
            {
                player.Send(new ResponseSetZoneDataPacket(this._data));
            }

            // Load zone name
            player.Send(new ResponseLoadLevelPacket(this._name));
            
            // TODO: Send created objects
            // TODO: Send destroyed objects
            // TODO: Send rpc
            
            // Send locked zone config
            if (this._locked)
                player.Send(new ResponseLockZonePacket(this._locked));
            
            // this.SendMaster((new ResponsePlayerJoinedPacket(player)).ToBinary());
        }

        public void LeavePlayer(Player p)
        {
            throw new NotImplementedException();
            
            // Update user if not updated. Caution with loops.
            // If zone has 0 players, stop and save to disk (or destroy if not persistent). Look GameServer:613
            // If the player is the hoster, set other
            // If zone has other players, notify the leave. ResponsePlayerLeft
            // Send player not notified object destroyed
        }

        /// <summary>
        /// Send packet to zone master
        /// </summary>
        /// <param name="data"></param>
        protected void SendMaster(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}