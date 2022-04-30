using System;
using System.Net.Sockets;
using System.Net;

namespace Client.Business
{
    /// <summary>
    /// This class provides main logic for client app
    /// </summary>
    public class ClientEngine
    {
        private IPAddress? _serverAddress;
        private int _portNumber;
        private TcpClient? _client;
        public bool IsConnectedToServer
        {
            get
            {
                if (_client == null) return false;
                return _client.Connected;
            }
        }

        public ClientEngine()
        {

        }

        public void ConnectTo(string serverAddress, string portNumber)
        {
            if (!IPAddress.TryParse(serverAddress, out _serverAddress))
                throw new ArgumentNullException(nameof(_serverAddress));

            if (!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentNullException(nameof(_portNumber));

            _client = new TcpClient();
            try
            {
                _client.Connect(_serverAddress, _portNumber);
                if (_client.Connected)
                    OnClientConnected?.Invoke(this, new EventArgs());
            }
            catch (Exception)
            {
                OnServerConnectionFailed?.Invoke(this, new EventArgs());
            }
        }

        public void Disconnect()
        {
            _client?.Close();
            OnClientDisconnected?.Invoke(this, new EventArgs());
        }

        public bool SendMessage(string message)
        {
            if (_client == null)
            {
                OnError?.Invoke(this, new EventArgs());
                return false;
            }

            NetworkStream stream = _client.GetStream();
            if (stream == null || !_client.Connected)
            {
                ConnectTo(this._serverAddress.ToString(), this._portNumber.ToString());
                stream = _client.GetStream();
            }
                

            if (string.IsNullOrEmpty(message))
                return false;

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);

            OnMessageSent?.Invoke(this, new EventArgs());


            return true;
        }

        #region Events
        public event EventHandler<EventArgs>? OnMessageSent;
        public event EventHandler<EventArgs>? OnClientConnected;
        public event EventHandler<EventArgs>? OnClientDisconnected;
        public event EventHandler<EventArgs>? OnServerConnectionFailed;
        public event EventHandler<EventArgs>? OnError;
        #endregion
    }
}