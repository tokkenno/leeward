using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Leeward.Net
{
    delegate void MessageEventHandler(InputConnection connection, MemoryStream message);
        
    internal class InputConnection : Connection
    {
        private DateTime _lastReception;
        public DateTime LastSeen => _lastReception;

        private const int InputBufferSize = 8192;
        private byte[] _inputBuffer = new byte[InputBufferSize];

        public event MessageEventHandler OnMessage;
        
        public InputConnection(Socket sock) : base(sock)
        {
            sock.BeginReceive(this._inputBuffer, 0, this._inputBuffer.Length, SocketFlags.None,
                new AsyncCallback(this.ReceiveHandler), (object) sock);
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
        }

        protected void SendHandler(IAsyncResult ar) {
            try { 
                int bytesSent = this.Socket.EndSend(ar);
            } catch (Exception e) {  
                Console.WriteLine("Socket send error: " + e.ToString());  
            }  
        }

        public void Disconnect()
        {
            this.Socket.Shutdown(SocketShutdown.Both);
            this.Socket.Disconnect(false);
            Console.WriteLine("Client " + this.Ip.ToString() + " disconnected.");
            // TODO: Handle well
        }

        public void Send(byte[] data)
        {
            this.Socket.Send(data);
        }
        
        public IAsyncResult SendAsync(byte[] data) {
            return this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendHandler), this.Socket);
        }
    }
}