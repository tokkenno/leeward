using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using Leeward.Protocol;

namespace Leeward.Net
{
    internal abstract class InputConnection : Connection
    {
        private DateTime _lastReception;
        
        private const int InputBufferSize = 8192;
        private byte[] _inputBuffer = new byte[InputBufferSize];
        
        public InputConnection(Socket sock) : base(sock)
        {
            sock.BeginReceive(this._inputBuffer, 0, this._inputBuffer.Length, SocketFlags.None,
                new AsyncCallback(this.OnReceive), (object) sock);
        }
        
        protected void OnReceive(IAsyncResult ar)
        {
            int bytesRead = this._socket.EndReceive(ar);
            if (bytesRead > 0)
            {
                this._lastReception = DateTime.Now;
                using (MemoryStream msgData = new MemoryStream(this._inputBuffer, 0, bytesRead))
                {
                    this.OnMessage(msgData);
                }
                Console.WriteLine(BitConverter.ToString(this._inputBuffer,0,bytesRead));
                Console.WriteLine(Encoding.UTF8.GetString(this._inputBuffer, 0, bytesRead));
            }
        }

        protected abstract void OnMessage(MemoryStream data);

        protected void OnSend(IAsyncResult ar) {  
            try { 
                int bytesSent = this._socket.EndSend(ar);
            } catch (Exception e) {  
                Console.WriteLine("Socket send error: " + e.ToString());  
            }  
        }

        public void Disconnect()
        {
            
        }

        protected void Send(byte[] data)
        {
            this._socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSend), this._socket);
        }
    }
}