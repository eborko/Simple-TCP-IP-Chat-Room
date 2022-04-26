using System;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    public class AdmonteClient
    {
        private IPAddress? _hostAddress;
        private int _portNumber;
        private TcpClient _tcpClient;
        private readonly Socket _socket;
        private IPEndPoint? _endPoint;

        public AdmonteClient() 
        { 
            _tcpClient = new TcpClient();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectTo(string hostAddress, string portNumber)
        {
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentException("Given argument HostAddress is not valid.");

            if (!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentException("Given argument PortNumber is not valid.");

            this._endPoint = new IPEndPoint(_hostAddress, _portNumber);
            //_tcpClient.Connect(_endPoint);
            _socket.Connect(_endPoint);

            if (ClientConnected != null)
                ClientConnected(this, new EventArgs());
        }

        public void Disconnect()
        {
            _tcpClient.Close();

            if(ClientDisconnected != null)
                ClientDisconnected(this, new EventArgs());
        }

        public void SendMessage(string message)
        {
            this._socket.SendTo(System.Text.Encoding.ASCII.GetBytes(message), 0, _endPoint);

            if (MessageSent != null)
                MessageSent(this, new EventArgs());
        }

        #region Events
        public event EventHandler<EventArgs>? MessageSent;
        public event EventHandler<EventArgs>? ClientConnected;
        public event EventHandler<EventArgs>? ClientDisconnected;
        #endregion
    }
}