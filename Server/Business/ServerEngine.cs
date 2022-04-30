using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AdmonteServer.Business
{
    /// <summary>
    /// Class AdmonteServer represents main logic for the server app
    /// </summary>
    public class ServerEngine
    {
        // IP address of server
        private readonly IPAddress? _hostAddress;
        // Port number to listen
        private readonly int _portNumber;
        private Socket _server;
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Constructor of Server
        /// </summary>
        /// <param name="hostAddress">Server address</param>
        /// <param name="portNumber">Port number to listen</param>
        /// <exception cref="ArgumentException">Throws <code>ArgumentException</code> if params are not valid</exception>
        public ServerEngine(string hostAddress, string portNumber)
        {
            // Try to parse address from string.
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentNullException("HostAddress");

            // Try to parse port number from string.
            if(!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentNullException("PortNumber");

            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, _portNumber));
        }

        #region Events
        public event EventHandler<AdmonteMessageEventArgs>? OnMessageReceived;
        public event EventHandler<EventArgs>? OnStart;
        public event EventHandler<EventArgs>? OnStop;
        public event EventHandler<EventArgs>? OnClientConnected;
        public event EventHandler<EventArgs>? OnWaitForClient;
        public event EventHandler<EventArgs>? OnError;
        #endregion

        public void Start()
        {
            try
            {
                IsStarted = true;
                _server.Listen(10000);
                OnStart?.Invoke(this, new EventArgs());

                while (true)
                {
                    OnWaitForClient?.Invoke(this, new EventArgs());
                    Socket client = _server.Accept();

                    OnClientConnected?.Invoke(this, new EventArgs());

                    Task task = new Task(() =>
                    {
                        // Use default buffer size 8192
                        byte[] buffer = new byte[8192];
                        string message = null;
                        int i = 0;
                        while ((i = client.Receive(buffer, 0, buffer.Length, SocketFlags.None)) > 0)
                        {
                            message = System.Text.ASCIIEncoding.ASCII.GetString(buffer, 0, i);
                            if (message != "")
                                OnMessageReceived?.Invoke(this, new AdmonteMessageEventArgs(message, client.RemoteEndPoint.ToString(), _portNumber));
                        }
                        client.Close();
                    });

                    task.Start();
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new EventArgs());
                Stop();
            }
        }

        public void Stop()
        {
            _server?.Close();
            IsStarted = false;
            OnStop?.Invoke(this, new EventArgs());
        }
    }
}
