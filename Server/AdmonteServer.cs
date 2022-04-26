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
        private readonly TcpListener _listener;

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
                throw new ArgumentException("Given argument HostAddress is not valid.");

            // Try to parse port number from string.
            if(!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentException("Given argument PortNumber is not valid.");

            this._listener = new TcpListener(_hostAddress, _portNumber);
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
                _listener.Start();
                OnStart?.Invoke(this, new EventArgs());

                // Use default buffer size 8192
                byte[] buffer = new byte[8192];
                string? message = null;

                while (true)
                {
                    OnWaitForClient?.Invoke(this, new EventArgs());

                    this._client = _listener.AcceptTcpClient();
                    OnClientConnected?.Invoke(this, new EventArgs());

                    message = null;

                    NetworkStream stream = _client.GetStream();

                    // uncomment if default buffer size is not used
                    //_client.ReceiveBufferSize = buffer.Length;

                    int i;

                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        message = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, i);
                        OnMessageReceived?.Invoke(this, new AdmonteMessageEventArgs(message, "localhost", _portNumber));
                    }
                    OnMessageReceived?.Invoke(this, new AdmonteMessageEventArgs(message + " <- FINAL_MESSAGE", "localhost", _portNumber));
                    _client.Close();
                    
                }
            }
            finally
            {
                _listener.Stop();
            }
        }

        public void Stop()
        {
            Console.WriteLine("Stopping ...");

            OnStop?.Invoke(this, new EventArgs());
        }
    }
}
