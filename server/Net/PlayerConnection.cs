using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Leeward.Protocol;

namespace Leeward.Net
{
    internal class PlayerConnection : InputConnection
    {
        public PlayerConnection(InputConnection conn) : this(conn.Socket)
        {
        }
        
        public PlayerConnection(Socket sock) : base(sock)
        {
            this.OnMessage += new MessageEventHandler(MessageHandler);
        }

        protected void MessageHandler(InputConnection conn, MemoryStream data)
        {
            try
            {
                List<Packet> messages = PacketHandler.Handle(data);

                foreach (Packet message in messages)
                {
                    switch (message.Type)
                    {
                        case PacketType.RequestID:
                            break;
                        case PacketType.Disconnect:
                            this.Disconnect();
                            break;
                    }
                }
            }
            catch (UnrecognizedPacketException upe) // TODO: Packet handler exception
            {
            }
        }
    }
}