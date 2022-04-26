using System;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    public class AdmonteClient
    {
        private IPAddress? _hostAddress;
        private int _portNumber;
        private TcpClient? _client;

        public AdmonteClient() 
        {
            
        }

        public void ConnectTo(string hostAddress, string portNumber)
        {
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentException("Given argument HostAddress is not valid.");

            if (!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentException("Given argument PortNumber is not valid.");

            _client = new TcpClient();
            try
            {
                _client.Connect(_hostAddress, _portNumber);
            }
            catch (Exception ex)
            {
                // TODOO: Log error
                return;
            }

            if (_client.Connected)
                ClientConnected?.Invoke(this, new EventArgs());
        }

        public void Disconnect()
        {
            _client.Close();
            ClientDisconnected?.Invoke(this, new EventArgs());
        }

        public void SendMessage(string message)
        {
            NetworkStream stream = _client.GetStream();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);

            MessageSent?.Invoke(this, new EventArgs());
        }

        #region Events
        public event EventHandler<EventArgs>? MessageSent;
        public event EventHandler<EventArgs>? ClientConnected;
        public event EventHandler<EventArgs>? ClientDisconnected;
        public event EventHandler<EventArgs>? ServerConnectionFailed;
        #endregion
    }
}