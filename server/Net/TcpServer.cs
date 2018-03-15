using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Leeward.Net
{
    delegate void NewConnectionEventHandler(InputConnection connection);
    
    internal abstract class TcpServer
    {
        private static readonly Utils.Logger _logger = Utils.Logger.Get(typeof(TcpServer));

        private readonly ManualResetEvent _connAccepted = new ManualResetEvent(false);
        private Socket _socket;
        protected IPEndPoint _localEndPoint;

        public event NewConnectionEventHandler NewConnection;

        public TcpServer(IPEndPoint local)
        {
            _localEndPoint = local;
        }
        
        // Blocker call
        public void Listen()
        {
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                this._socket.Bind(this._localEndPoint);
                this._socket.Listen(100);
                _logger.Info($"TCP port listening on: {this._localEndPoint}");

                while (true)
                {
                    _connAccepted.Reset();
                    this._socket.BeginAccept(new AsyncCallback(AcceptHandler), this._socket); 
                    _connAccepted.WaitOne();
                }
            }
            catch (Exception e) {
                _logger.Fatal("Server socket error: " + e.ToString());  
            }  
        }  

        protected void AcceptHandler(IAsyncResult ar) {  
            // Signal the main thread to continue.  
            _connAccepted.Set();  

            // Get the socket that handles the client request.  
            Socket clientSock = this._socket.EndAccept(ar);
            _logger.Debug($"Client connected from {clientSock.RemoteEndPoint}");

            // Create client
            InputConnection client = new InputConnection(clientSock);

            // Emit events and run default class handler
            this.OnConnection(client);
            NewConnection?.Invoke(client);
        }

        protected abstract void OnConnection(InputConnection sock);
    }
}