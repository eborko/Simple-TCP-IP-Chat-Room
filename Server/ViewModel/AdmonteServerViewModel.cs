using Server.Business;
using Server.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Server.ViewModel
{
    public class AdmonteServerViewModel : DependencyObject
    {
        #region Private fields
        private AdmonteServer? _admonteServer;
        private BackgroundWorker backgroundWorker;
        #endregion

        #region Commands
        public UniversalCommand StartServerCommand { get; set; }
        public UniversalCommand StopServerCommand { get; set; }
        #endregion

        public AdmonteServerViewModel()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            LogMessages = new List<string>();

            #region Init commands
            StartServerCommand = new UniversalCommand(executeMethod: ExecuteStartServer, canExecuteMethod: CanStartServer);
            StopServerCommand = new UniversalCommand(executeMethod: ExecuteStopServer, canExecuteMethod: CanStopServer);
            #endregion

        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            this._admonteServer?.Start();
        }

        #region All Command related methods
        private void ExecuteStartServer()
        {
            try
            {
                this._admonteServer = new AdmonteServer(this.ServerAddress, this.ServerPort);

                #region subscribe to events
                _admonteServer.OnMessageReceived += ServerMessageReceived;
                _admonteServer.OnStart += Server_OnStart;
                _admonteServer.OnStop += Server_OnStop;
                _admonteServer.OnClientConnected += Server_OnClientConnected;
                _admonteServer.OnWaitForClient += Server_OnWaitForClient;
                #endregion

                backgroundWorker.WorkerSupportsCancellation = true;
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            catch (InvalidOperationException ex)
            {
                LogMessages.Add(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                LogMessages.Add(ex.Message);
            }
        }

        private bool CanStartServer()
        {
            return this._admonteServer == null;
        }

        private void ExecuteStopServer()
        {
            backgroundWorker.CancelAsync();
            this._admonteServer?.Stop();
        }

        private bool CanStopServer()
        {
            if (this._admonteServer == null) return false;

            return this._admonteServer.IsServerStarted;
        }
        #endregion

        #region Server event handlers
        private void Server_OnWaitForClient(object? sender, EventArgs e)
        {
            LogMessages.Add("Waiting for a client.");
        }

        private void Server_OnClientConnected(object? sender, EventArgs e)
        {
            LogMessages.Add("Client connected.");
        }

        private void Server_OnStop(object? sender, EventArgs e)
        {
            LogMessages.Add("Server stopped.");
        }

        private void Server_OnStart(object? sender, EventArgs e)
        {
            LogMessages.Add("Server started.");
        }

        private void ServerMessageReceived(object? sender, AdmonteMessageEventArgs args)
        {
            LogMessages.Add($"Message received:\n\tHost: {args.Host}, Port: {args.Port}\n\tMessage: {args.Message}\n");
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty ServerAddressProperty = DependencyProperty.Register(
                name: "ServerAddress",
                propertyType: typeof(string),
                ownerType: typeof(AdmonteServerViewModel));

        public string ServerAddress
        {
            get { return (string)GetValue(ServerAddressProperty); }
            set { SetValue(ServerAddressProperty, value); }
        }

        public static readonly DependencyProperty ServerPortProperty = DependencyProperty.Register(
            name: "ServerPort",
            propertyType: typeof(string),
            ownerType: typeof(AdmonteServerViewModel));

        public string ServerPort
        {
            get { return (string)GetValue(ServerPortProperty); }
            set { SetValue(ServerPortProperty, value); }
        }

        public static readonly DependencyProperty LogMessagesProperty = DependencyProperty.Register(
            name: "LogMessages",
            propertyType: typeof(IList<string>),
            ownerType: typeof(AdmonteServerViewModel));

        public IList<string> LogMessages
        { 
            get { return (IList<string>)GetValue(LogMessagesProperty); } 
            set { SetValue(LogMessagesProperty, value); }
        }
        #endregion
    }
}
