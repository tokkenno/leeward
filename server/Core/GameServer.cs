using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Leeward.Entity;
using Leeward.Net;
using Leeward.Protocol;

namespace Leeward.Core
{
    internal class GameServer : TcpServer
    {
        private MessageEventHandler _newConnectionEventHandler;

        private int _httpServerPort = -1;

        public GameServer(int port) : base(new IPEndPoint(IPAddress.Any, port))
        {
        }

        private void AddPlayer(Player player)
        {
            Console.WriteLine("New player: " + player.Name);
        }

        #region Network methods

        protected override void OnConnection(InputConnection connection)
        {
            // TODO: Save active connection list

            // Set new connection handler
            if (this._newConnectionEventHandler == null)
                this._newConnectionEventHandler = new MessageEventHandler(NewConnectionHandler);

            connection.OnMessage += this._newConnectionEventHandler;
        }

        private void NewConnectionHandler(InputConnection connection, MemoryStream data)
        {
            // Remove new connection handler. This isn't new already.
            connection.OnMessage -= this._newConnectionEventHandler;
            
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
        }

        #endregion
    }
}