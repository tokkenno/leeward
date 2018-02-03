using System;
using System.Net;
using System.Net.Sockets;

namespace Leeward.Net
{
    public class Connection
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(Connection));
        
        public readonly IPAddress Ip;
        public readonly int Port;
        public readonly Socket Socket;

        private bool _connected;
        
        public event EventHandler OnDisconnect;

        public Connection(Socket sock)
        {
            this.Socket = sock;
            this._connected = true;

            if (!(sock.RemoteEndPoint is IPEndPoint remoteEndPoint)) return;
            this.Ip = remoteEndPoint.Address;
            this.Port = remoteEndPoint.Port;
        }

        public void Disconnect()
        {
            if (this._connected)
            {
                this._connected = false;
                this.Socket.Shutdown(SocketShutdown.Both);
                this.Socket.Disconnect(false);
                this.OnDisconnect?.Invoke(this, EventArgs.Empty);
                _logger.Debug("Client " + this.Ip.ToString() + " disconnected.");
            }
        }

        public void Send(byte[] data)
        {
            if (this._connected)
            {
                this.Socket.Send(data);
            }
            else
            {
                throw new ConnectionClosedException(this);
            }
        }
        
        public IAsyncResult SendAsync(byte[] data) {
            if (this._connected)
            {
                return this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendHandler), this.Socket);
            }
            else
            {
                throw new ConnectionClosedException(this);
            }
        }

        protected void SendHandler(IAsyncResult ar) {
            try { 
                int bytesSent = this.Socket.EndSend(ar);
            } catch (Exception e) {  
                _logger.Warning("Socket send error: " + e.ToString());  
            }  
        }
    }
}