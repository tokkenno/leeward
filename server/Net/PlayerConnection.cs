using System;
using System.IO;
using System.Net.Sockets;
using Leeward.Protocol;

namespace Leeward.Net
{
    internal class PlayerConnection : InputConnection
    {
        public PlayerConnection(Socket sock) : base(sock)
        {
        }

        protected override void OnMessage(MemoryStream data)
        {
            try
            {
                Packet message = PacketHandler.Handle(data);

                switch (message.Type)
                {
                    case PacketType.RequestID:
                        break;
                    case PacketType.Disconnect:
                        this.Disconnect();
                        break;
                }
            }
            catch (Exception) // TODO: Packet handler exception
            {
            }
        }
    }
}