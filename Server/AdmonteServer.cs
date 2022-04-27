using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// Class AdmonteServer represents main logic for server app
    /// </summary>
    public class AdmonteServer
    {
        // IP address of server
        private readonly IPAddress? _hostAddress;
        // Port number to listen
        private readonly int _portNumber;
        // Endpoint contains IPAddress and port number
        private TcpClient? _client;
        //private TcpListener? _listener;
        private Socket _server;

        /// <summary>
        /// Constructor of Server
        /// </summary>
        /// <param name="hostAddress">Server address</param>
        /// <param name="portNumber">Port number to listen</param>
        /// <exception cref="ArgumentException">Throws <code>ArgumentException</code> if params are not valid</exception>
        public AdmonteServer(string hostAddress, string portNumber)
        {
            // Try to parse address from string.
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentNullException("HostAddress");

            // Try to parse port number from string.
            if(!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentNullException("PortNumber");
<<<<<<< HEAD
=======

            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, _portNumber));
>>>>>>> Sockets1
        }

        #region Events
        public event EventHandler<AdmonteMessageEventArgs>? OnMessageReceived;
        public event EventHandler<EventArgs>? OnStart;
        public event EventHandler<EventArgs>? OnStop;
        public event EventHandler<EventArgs>? OnClientConnected;
        public event EventHandler<EventArgs>? OnWaitForClient;
        #endregion

        public void Start()
        {
            try
            {
                _server.Listen(10000);
                OnStart?.Invoke(this, new EventArgs());

                // Use default buffer size 8192
                byte[] buffer = new byte[8192];
                string? message = null;
                Socket client;

                while (true)
                {
                    OnWaitForClient?.Invoke(this, new EventArgs());

                    client = _server.Accept();
                    OnClientConnected?.Invoke(this, new EventArgs());

                    message = null;

                    int i;
                    while ((i = client.Receive(buffer, 0, buffer.Length, SocketFlags.None)) > 0)
                    {
                        message = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, i);
                        OnMessageReceived?.Invoke(this, new AdmonteMessageEventArgs(message, client.RemoteEndPoint.ToString(), _portNumber));
                    }
                    _client?.Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                // TODOO: Log error
            }
            catch (Exception ex)
            {
                // TODOO: Log error
            }
        }

        public void Stop()
        {
            _client?.Close();
            _server?.Close();
            OnStop?.Invoke(this, new EventArgs());
        }
    }
}
