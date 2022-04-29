using Server.Business;
using Server.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LogMessages = new ObservableCollection<string>();

            #region Init commands
            StartServerCommand = new UniversalCommand(executeMethod: ExecuteStartServer, canExecuteMethod: CanStartServer);
            StopServerCommand = new UniversalCommand(executeMethod: ExecuteStopServer, canExecuteMethod: CanStopServer);
            #endregion

        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                this._admonteServer?.Start();
            }
            catch (Exception ex)
            {
                Dispatcher?.Invoke(() => LogMessages.Add(ex.Message));
            }

        }

        #region All Command related methods
        private void ExecuteStartServer()
        {
            try
            {
                this._admonteServer = new AdmonteServer(this.ServerAddress, this.ServerPort);

                #region subscribe to events
                _admonteServer.OnMessageReceived += _admonteServer_OnMessageReceived;
                _admonteServer.OnStart += _admonteServer_OnStart;
                _admonteServer.OnStop += _admonteServer_OnStop;
                _admonteServer.OnClientConnected += _admonteServer_OnClientConnected;
                _admonteServer.OnWaitForClient += _admonteServer_OnWaitForClient;
                _admonteServer.OnError += _admonteServer_OnError;
                #endregion

                backgroundWorker.WorkerSupportsCancellation = true;
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher?.Invoke(() => LogMessages.Add(ex.Message));
            }
        }

        private bool CanStartServer()
        {
            if (_admonteServer == null || !_admonteServer.IsStarted)
                return true;

            return false;
        }

        private void ExecuteStopServer()
        {
            backgroundWorker.CancelAsync();
            this._admonteServer?.Stop();
        }

        private bool CanStopServer()
        {
            return !CanStartServer();
        }
        #endregion

        #region Server event handlers
        private void _admonteServer_OnError(object? sender, EventArgs e)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add("Internall server error."));
        }

        private void _admonteServer_OnWaitForClient(object? sender, EventArgs e)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add("Waiting for a client."));
        }

        private void _admonteServer_OnClientConnected(object? sender, EventArgs e)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add("Client connected."));
        }

        private void _admonteServer_OnStop(object? sender, EventArgs e)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add("Server stopped."));
        }

        private void _admonteServer_OnStart(object? sender, EventArgs e)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add("Server started."));
        }

        private void _admonteServer_OnMessageReceived(object? sender, AdmonteMessageEventArgs args)
        {
            this.Dispatcher?.Invoke(() => LogMessages.Add($"Message received:\n\tHost: {args.Host}, Port: {args.Port}\n\tMessage: {args.Message}\n"));
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
            propertyType: typeof(ObservableCollection<string>),
            ownerType: typeof(AdmonteServerViewModel));

        public IList<string> LogMessages
        { 
            get { return (ObservableCollection<string>)GetValue(LogMessagesProperty); } 
            set { SetValue(LogMessagesProperty, value); }
        }
        #endregion
    }
}
