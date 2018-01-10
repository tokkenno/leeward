using System.Net;
using System.Net.Sockets;

namespace Leeward.Net
{
    public class Connection
    {
        public readonly IPAddress Ip;
        public readonly int Port;
        protected readonly Socket _socket;

        public Connection(Socket sock)
        {
            this._socket = sock;

            if (!(sock.RemoteEndPoint is IPEndPoint remoteEndPoint)) return;
            this.Ip = remoteEndPoint.Address;
            this.Port = remoteEndPoint.Port;
        }
    }
}