using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Leeward.IO;
using Leeward.Net;
using Leeward.Protocol;
using Leeward.Protocol.Packets;

namespace Leeward.Core
{
    internal class GameServer : TcpServer
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(GameServer));

        private MessageEventHandler _newConnectionEventHandler;

        private int _httpServerPort = -1;
        
        private List<Player> _players = new List<Player>();
        private List<Zone> _zones = new List<Zone>();

        private Configuration _gameConfiguration; // FIX: Load

        public GameServer(int port) : base(new IPEndPoint(IPAddress.Any, port))
        {
        }

        /// <summary>
        /// Add player to the server
        /// </summary>
        /// <param name="player">Player</param>
        private void AddPlayer(Player player)
        {
            // Add to player list
            this._players.Add(player);
            
            // Send to user his server Id
            player.SendResponseId();
            
            // TODO: If server data don't exists, send to user ResponseReloadServerConfig
            
            // TODO: Update data on lobby server
            
            // TODO: Emit internal event
            
            // Handle new player messages
            player.OnMessage += new PlayerMessageEventHandler(PlayerMessageHandler);
            
            // Send player connected event
            player.SendPlayerConnected();
            
            _logger.Info($"New player <{player.Name}> connected from {player.Connection.Ip}");
        }

        /// <summary>
        /// Remove a player from the server and disconnect it
        /// </summary>
        /// <param name="player">Player</param>
        private void RemovePlayer(Player player)
        {
        }

        #region Network handler methods

        /// <summary>
        /// Handle new connections
        /// </summary>
        /// <param name="connection">New connection</param>
        protected override void OnConnection(InputConnection connection)
        {
            // TODO: Save active connection list

            // Set new connection handler
            if (this._newConnectionEventHandler == null)
                this._newConnectionEventHandler = new MessageEventHandler(NewConnectionHandler);

            connection.OnMessage += this._newConnectionEventHandler;
        }

        /// <summary>
        /// Handle new connection messages
        /// </summary>
        /// <param name="connection">Connection that send the message</param>
        /// <param name="data">Message raw data</param>
        private void NewConnectionHandler(InputConnection connection, MemoryStream data)
        {
            _logger.Trace("C => S: " + BitConverter.ToString(data.ToArray()));
            
            try
            {
                List<Packet> messages = PacketHandler.Handle(data);
                Packet firstMessage = messages.FirstOrDefault();

                switch (firstMessage?.Type) // FIX: RequestID is always an only packet?
                {
                    case PacketType.HttpRequest:
                        if (this._httpServerPort > 0)
                            connection.Send(Encoding.ASCII.GetBytes(
                                "HTTP/1.1 301 Moved Permanently\nLocation: http://" +
                                this._localEndPoint.Address.ToString() +
                                (this._httpServerPort != 80 ? ":" + this._httpServerPort.ToString() : "") +
                                "\nConnection: close\n\n")); // TODO: Set public IP or domain
                        else
                            connection.Send(Encoding.ASCII.GetBytes(
                                "HTTP/1.1 503 Service Unavailable\nConnection: close\n\nThis is not a web server"));
                        connection.Disconnect();
                        break;
                    case PacketType.RequestID:
                        RequestIdPacket reqId = firstMessage as RequestIdPacket;
                        Player newPlayer = new Player(reqId?.Name, new PlayerConnection(connection));
                        
                        // TODO: Check bans. connection.Send(ResponseIdPacket.RejectPlayer().ToBinary());
                        this.AddPlayer(new Player((messages.First() as RequestIdPacket)?.Name,
                            new PlayerConnection(connection)));
                        break;
                    default:
                        _logger.Warning(
                            $"Packet not expected on new connection from {connection.Ip}: {Enum.GetName(typeof(PacketType), firstMessage?.Type)}");
                        break;
                }
            }
            catch (UnrecognizedPacketException ex)
            {
                _logger.Warning($"Unrecognized packet from client {connection.Ip}: {ex.Message}");
                connection.Disconnect();
            }
            catch (VersionNotFoundException ex)
            {
                _logger.Warning($"Protocol version mismatch from client {connection.Ip}: {ex.Message}");
                connection.Disconnect();
            }
            
            // Remove new connection handler. This isn't new already.
            connection.OnMessage -= this._newConnectionEventHandler;
        }

        /// <summary>
        /// Handle messages sended from a player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PlayerMessageHandler(Player player, MemoryStream data)
        {
            _logger.Trace("C => S: " + BitConverter.ToString(data.ToArray()));
            
            try
            {
                List<Packet> messages = PacketHandler.Handle(data);
                _logger.Debug($"Handling messages: {string.Join(", ", messages.ConvertAll((p) => Enum.GetName(typeof(PacketType), p.Type)))}");
                
                foreach (Packet message in messages)
                {
                    switch (message.Type)
                    {
                        /*case PacketType.RequestJoinZone:
                            if (!this.HandleJoinZoneRequest(player, (RequestJoinZonePacket) message)) return;
                            break;*/
                        case PacketType.RequestSetAlias:
                            player.SetAlias((message as RequestSetAliasPacket)?.Alias);
                            break;
                        default:
                            Console.WriteLine("Unrecognized message");
                            Console.WriteLine(message.ToString());
                            throw new NotImplementedException();
                    }
                }
            }
            catch (UnrecognizedPacketException ex)
            {
                _logger.Warning($"Unrecognized packet from client {player.Connection.Ip}: {ex.Message}");
                player.Connection.Disconnect();
            }
        }

        /// <summary>
        /// Handle a user message requesting join to zone
        /// </summary>
        /// <param name="player">Player who has request</param>
        /// <param name="message">Request to join zone</param>
        /// <returns></returns>
        private bool HandleJoinZoneRequest(Player player, RequestJoinZonePacket message)
        {
            if (!message.IsLastVersion())
            {
                // TODO: player.SendMessage("Your client is outdated");
                this.RemovePlayer(player);
                return false;
            }

            int idToJoin = message.Id;

            // If join by zone name, find id
            if (idToJoin == -2)
            {
                // Get id, if exists, or set command to new channel mode
                idToJoin = this._zones.First((zone) =>
                    zone.IsOpen &&
                    (string.IsNullOrEmpty(message.Name) || message.Name.Equals(zone.Name)) &&
                    (zone.HasPassword || message.Password.Equals(zone.Password))
                )?.Id ?? -1;
            }
            
            // If join to new zone mode
            if (idToJoin == -1)
            {
                // If zone name its not provided
                if (string.IsNullOrEmpty(message.Name))
                {
                    player.Send((new ResponseJoinZonePacket(false, "No suitable channels found")).ToBinary());
                    return true; // FIX: ?
                }
                else
                {
                    Zone newZone = new Zone(message.Name, message.Password, message.MaxPlayers, message.Persistent);
                    this._zones.Add(newZone); // FIX: Any user can create a zone?
                    idToJoin = newZone.Id;
                }
            }
            
            Zone joinZone = this._zones.First((zone) => zone.Id == idToJoin);

            if (joinZone == null)
            {
                player.Send((new ResponseJoinZonePacket(false, "No suitable channels found")).ToBinary());
                return true; // FIX: ?
            }
            else if (!joinZone.IsOpen)
            {
                player.Send((new ResponseJoinZonePacket(false, "The requested channel is closed")).ToBinary());
                return true; // FIX: ?
            }
            else if (!joinZone.Password.Equals(message.Password))
            {
                player.Send((new ResponseJoinZonePacket(false, "Wrong password")).ToBinary());
                return true; // FIX: ?
            }
            else
            {
                if (player.CurrentZone != joinZone)
                {
                    if (player.CurrentZone != null) player.LeaveZone();
                    if (this._gameConfiguration != null) 
                        player.Send((new RequestSetServerOptionPacket(this._gameConfiguration)).ToBinary());
                    player.JoinZone(joinZone);
                }
            }

            return true;
        }

        #endregion

        #region Network send methods

        #endregion
    }
}