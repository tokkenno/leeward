using System.Net.Sockets;
using Leeward.Net;

namespace Leeward.Core
{
    internal class GameServer : TcpServer
    {
        protected override void OnConnection(Socket clientSock)
        {
            // Create input connection. TODO: Save active connections
            PlayerConnection state = new PlayerConnection(clientSock);
        }
    }
}