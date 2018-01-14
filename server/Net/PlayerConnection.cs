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
        }
    }
}