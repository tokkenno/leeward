using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Leeward.Net
{
    internal abstract class TcpServer
    {
        private readonly ManualResetEvent _connAccepted = new ManualResetEvent(false);
        private Socket _socket;
        
        // Blocker call
        public void Listen()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 5127);
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                this._socket.Bind(localEndPoint);
                this._socket.Listen(100);
                Console.WriteLine("Listening on port :" + 5127);

                while (true)
                {
                    _connAccepted.Reset();
                    this._socket.BeginAccept(new AsyncCallback(OnAccept), this._socket); 
                    _connAccepted.WaitOne();
                    Console.WriteLine("Client connected");
                }
            }
            catch (Exception e) {  
                Console.WriteLine("Server socket error: " + e.ToString());  
            }  
        }  

        protected void OnAccept(IAsyncResult ar) {  
            // Signal the main thread to continue.  
            _connAccepted.Set();  

            // Get the socket that handles the client request.  
            Socket clientSock = this._socket.EndAccept(ar);
        }

        protected abstract void OnConnection(Socket sock);
    }
}