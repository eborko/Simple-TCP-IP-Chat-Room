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
        private IPEndPoint _endPoint;
        // Basic socket by Berkeley
        private Socket? _socket;

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

            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._endPoint = new IPEndPoint(_hostAddress, _portNumber);
            this._socket.Bind(this._endPoint);
        }

        #region Events
        public event EventHandler<AdmonteMessageEventArgs>? OnMessageReceived;
        public event EventHandler<EventArgs>? OnStart;
        public event EventHandler<EventArgs>? OnStop;
        #endregion

        public void Start()
        {
            Console.WriteLine("Starting ...");

            if (_socket == null)
                return;

            this._socket.Connect(this._endPoint);

            if (OnStart != null && this._socket.Connected)
                OnStart(this, new EventArgs());
        }

        public void Stop()
        {
            Console.WriteLine("Stopping ...");
            this._socket?.Disconnect(true);

            if (OnStop != null)
                OnStop(this, new EventArgs());
        }
    }
}
