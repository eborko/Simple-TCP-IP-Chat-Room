using System;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    public class AdmonteClient
    {
        private IPAddress? _hostAddress;
        private int _portNumber;
        private readonly Socket _socket;
        private IPEndPoint? _endPoint;

        public AdmonteClient() 
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectTo(string hostAddress, string portNumber)
        {
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentException("Given argument HostAddress is not valid.");

            if (!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentException("Given argument PortNumber is not valid.");

            this._endPoint = new IPEndPoint(_hostAddress, _portNumber);
            this._socket.Bind(this._endPoint); // THERE IS THTOWN EXCEPTION -> Only one usage of each socket address (protocol/network address/port) is normally permitted.
            _socket.Connect(_endPoint);

            if (_socket.Connected)
                ClientConnected?.Invoke(this, new EventArgs());
        }

        public void Disconnect()
        {
            _socket.Disconnect(true);

            ClientDisconnected?.Invoke(this, new EventArgs());
        }

        public void SendMessage(string message)
        {
            this._socket.SendTo(System.Text.Encoding.ASCII.GetBytes(message), 0, this._endPoint);

            MessageSent?.Invoke(this, new EventArgs());
        }

        #region Events
        public event EventHandler<EventArgs>? MessageSent;
        public event EventHandler<EventArgs>? ClientConnected;
        public event EventHandler<EventArgs>? ClientDisconnected;
        #endregion
    }
}