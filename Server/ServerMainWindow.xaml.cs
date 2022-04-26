using System;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window
    {
        private AdmonteServer? server;

        public ServerMainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            server = new AdmonteServer(txtHost.Text, txtPort.Text);

            #region subscribe to events
            if (server != null)
            {
                server.OnMessageReceived += ServerMessageReceived_EventHandler;
                server.OnStart += Server_OnStart;
                server.OnStop += Server_OnStop;
            }
            #endregion

            server?.Start();

            btnStart.IsEnabled = false;
        }

        private void Server_OnStart(object? sender, EventArgs e)
        {
            rtbMessages.AppendText("Server started.\n");
        }

        private void Server_OnStop(object? sender, EventArgs e)
        {
            rtbMessages.AppendText("Server stopped.\n");
        }

        private void ServerMessageReceived_EventHandler(object? sender, AdmonteMessageEventArgs? args)
        {
            ArgumentNullException.ThrowIfNull(args, null);
            rtbMessages.AppendText($"Message received:\n\tHost: {args.Host}, Port: {args.Port}\n\tMessage: {args.Message}\n");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            server?.Stop();

            btnStart.IsEnabled = true;
        }
    }
}
