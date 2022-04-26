using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class AdmonteServer
    {
        private readonly IPAddress? _hostAddress;
        private readonly int _portNumber;
        private readonly TcpListener _listener;
        private Socket? _socket;

        public AdmonteServer(string hostAddress, string portNumber)
        {
            if (!IPAddress.TryParse(hostAddress, out _hostAddress))
                throw new ArgumentException("Given argument HostAddress is not valid.");

            if(!Int32.TryParse(portNumber, out _portNumber))
                throw new ArgumentException("Given argument PortNumber is not valid.");

            this._listener = new TcpListener(_hostAddress, this._portNumber);
        }

        #region Events
        public event EventHandler<AdmonteMessageEventArgs>? OnMessageReceived;
        public event EventHandler<EventArgs>? OnStart;
        public event EventHandler<EventArgs>? OnStop;
        #endregion

        public void Start()
        {
            Console.WriteLine("Starting ...");
            this._listener.Start();

            if (OnStart != null)
                OnStart(this, new EventArgs());
        }

        public void Stop()
        {
            Console.WriteLine("Stopping ...");
            this._listener.Stop();

            if (OnStop != null)
                OnStop(this, new EventArgs());
        }
    }
}
