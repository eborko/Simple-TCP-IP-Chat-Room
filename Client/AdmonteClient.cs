﻿using System;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    /// <summary>
    /// This class provides main logic for client app
    /// </summary>
    public class AdmonteClient
    {
        private IPAddress? _hostAddress;
        private int _portNumber;
        private TcpClient? _client;

        public AdmonteClient() 
        {
            
        }

        public bool ConnectTo(string hostAddress, string portNumber)
        {
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
            {
                ServerConnectionFailed?.Invoke(this, new EventArgs());
                return false;
            }

            if (!Int32.TryParse(portNumber, out _portNumber))
            {
                ServerConnectionFailed?.Invoke(this, new EventArgs());
                return false;
            }

            _client = new TcpClient();
            try
            {
                _client.Connect(_hostAddress, _portNumber);
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
            _client.Close();
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