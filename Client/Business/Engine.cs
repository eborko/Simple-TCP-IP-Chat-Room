using System;
using System.Net.Sockets;
using System.Net;

namespace Client.Business
{
    /// <summary>
    /// This class provides main logic for client app
    /// </summary>
    public class Engine
    {
        private IPAddress? _serverAddress;
        private int _portNumber;
        private TcpClient? _client;

        public Engine() 
        {
            
        }

        public bool ConnectTo(string serverAddress, string portNumber)
        {
            if (!IPAddress.TryParse(serverAddress, out _serverAddress))
                throw new ArgumentNullException(nameof(_serverAddress));

            if (!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentNullException(nameof(_portNumber));

            _client = new TcpClient();
            try
            {
                _client.Connect(_serverAddress, _portNumber);
            }
            catch (Exception ex)
            {
                ServerConnectionFailed?.Invoke(this, new EventArgs());
                return false;
            }

            if (_client.Connected)
                ClientConnected?.Invoke(this, new EventArgs());
            return true;
        }

        public void Disconnect()
        {
            _client?.Close();
            ClientDisconnected?.Invoke(this, new EventArgs());
        }

        public bool SendMessage(string message)
        {
            if (!_client.Connected)
            {
                ServerConnectionFailed?.Invoke(this, new EventArgs());
                return false;
            }
                
            try
            {
                NetworkStream stream = _client.GetStream();
                if (stream == null) return false;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                MessageSent?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                // TODOO: Log error
            }

            return true;
        }

        #region Events
        public event EventHandler<EventArgs>? MessageSent;
        public event EventHandler<EventArgs>? ClientConnected;
        public event EventHandler<EventArgs>? ClientDisconnected;
        public event EventHandler<EventArgs>? ServerConnectionFailed;
        #endregion
    }
}