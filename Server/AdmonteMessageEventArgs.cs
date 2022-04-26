using System;

namespace Server
{
    public  class AdmonteMessageEventArgs : EventArgs
    {
        public string  Message { get; set; }
        public string  Host { get; set; }
        public int Port { get; set; }

        public AdmonteMessageEventArgs(string message, string host, int port)
        {
            this.Message = message;
            this.Host = host;
            this.Port = port;
        }
    }
}