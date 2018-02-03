using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Leeward.Net
{
    internal delegate void MessageEventHandler(InputConnection connection, MemoryStream message);
        
    internal class InputConnection : Connection
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(InputConnection));

        private DateTime _lastReception;
        public DateTime LastSeen => _lastReception;

        private const int InputBufferSize = 8192;
        private byte[] _inputBuffer = new byte[InputBufferSize];

        public event MessageEventHandler OnMessage;
        
        public InputConnection(Socket sock) : base(sock)
        {
            try
            {
                sock.BeginReceive(this._inputBuffer, 0, this._inputBuffer.Length, SocketFlags.None,
                    new AsyncCallback(this.ReceiveHandler), (object) sock);
            }
            catch (SocketException se)
            {
                this.Disconnect();
            }
        }
        
        protected void ReceiveHandler(IAsyncResult ar)
        {
            int bytesRead = this.Socket.EndReceive(ar);
            if (bytesRead > 0)
            {
                this._lastReception = DateTime.Now;

                if (OnMessage != null)
                {
                    lock (OnMessage)
                    {
                        using (MemoryStream msgData = new MemoryStream(this._inputBuffer, 0, bytesRead))
                        {
                            this.OnMessage.Invoke(this, msgData);
                        }
                    }
                }
            }

            try
            {
                Socket.BeginReceive(this._inputBuffer, 0, this._inputBuffer.Length, SocketFlags.None,
                    new AsyncCallback(this.ReceiveHandler), (object) Socket);
            }
            catch (SocketException se)
            {
                this.Disconnect();
            }
        }
    }
}