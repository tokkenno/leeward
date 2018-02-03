using System;

namespace Leeward.Net
{
    internal class ConnectionClosedException : Exception
    {
        public readonly Connection Connection;
        
        public ConnectionClosedException(Connection conn) : base($"Can't use a closed connection.")
        {
            this.Connection = conn;
        }
    }
}