using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Leeward.Entity;
using Leeward.Net;
using Leeward.Protocol;
using Leeward.Protocol.Packets;

namespace Leeward.Core
{
    internal class GameServer : TcpServer
    {
        private MessageEventHandler _newConnectionEventHandler;

        private int _httpServerPort = -1;
        
        private List<Player> _players = new List<Player>();

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
            
            Console.WriteLine("New player: " + player.Name);
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
            try
            {
                List<Packet> messages = PacketHandler.Handle(data);

                switch (messages.First().Type) // FIX: RequestID is always an only packet?
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
                        // TODO: Check bans. connection.Send(ResponseIdPacket.RejectPlayer().ToBinary());
                        this.AddPlayer(new Player((messages.First() as RequestIdPacket)?.Name,
                            new PlayerConnection(connection)));
                        break;
                    default: break;
                }
            }
            catch (UnrecognizedPacketException ex)
            {
                Console.Error.WriteLine(ex.Message + " Client: " + connection.Ip.ToString() + ".");
                connection.Disconnect();
            }
            catch (VersionNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message + " Client: " + connection.Ip.ToString() + ".");
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
            try
            {
                List<Packet> messages = PacketHandler.Handle(data);
                messages.ForEach((packet) => Console.WriteLine(packet.ToString()));
                throw new NotImplementedException();
            }
            catch (UnrecognizedPacketException ex)
            {
                Console.Error.WriteLine(ex.Message + " Client: " + player.Connection.Ip.ToString() + ".");
                player.Connection.Disconnect();
            }
        }

        #endregion

        #region Network send methods

        #endregion
    }
}