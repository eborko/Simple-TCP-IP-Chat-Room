using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window
    {
        private AdmonteServer? server;
        private BackgroundWorker _backgroundWorker;

        public ServerMainWindow()
        {
            InitializeComponent();
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
        }

        private void _backgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            server?.Start();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            server = new AdmonteServer(txtHost.Text, txtPort.Text);
            if (server == null) return;

            #region subscribe to events
            server.OnMessageReceived += ServerMessageReceived_EventHandler;
            server.OnStart += Server_OnStart;
            server.OnStop += Server_OnStop;
            server.OnClientConnected += Server_OnClientConnected;
            server.OnWaitForClient += Server_OnWaitForClient;
            #endregion

            // start thread
            _backgroundWorker.WorkerSupportsCancellation = true;
            if (!_backgroundWorker.IsBusy)
            {
                _backgroundWorker.RunWorkerAsync();
            }

            btnStart.IsEnabled = false;
        }

        private void Server_OnWaitForClient(object? sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => rtbMessages.AppendText("Waiting for client.\n"));
        }

        private void Server_OnClientConnected(object? sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => rtbMessages.AppendText("Client connected.\n"));
        }

        private void Server_OnStart(object? sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => rtbMessages.AppendText("Server started.\n"));
        }

        private void Server_OnStop(object? sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => rtbMessages.AppendText("Server stopped.\n"));
        }

        private void ServerMessageReceived_EventHandler(object? sender, AdmonteMessageEventArgs? args)
        {
            ArgumentNullException.ThrowIfNull(args, null);
            this.Dispatcher.Invoke(() => rtbMessages.AppendText($"Message received:\n\tHost: {args.Host}, Port: {args.Port}\n\tMessage: {args.Message}\n"));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            server?.Stop();
            _backgroundWorker.CancelAsync();

            btnStart.IsEnabled = true;
        }
    }
}
