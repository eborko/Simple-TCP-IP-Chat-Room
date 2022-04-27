using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientMainWindow : Window
    {
        AdmonteClient admonteClient;

        public ClientMainWindow()
        {
            InitializeComponent();

            admonteClient = new AdmonteClient();
            admonteClient.ClientConnected += AdmonteClient_ClientConnected;
            admonteClient.ClientDisconnected += AdmonteClient_ClientDisconnected;
            admonteClient.MessageSent += AdmonteClient_MessageSent;
            admonteClient.ServerConnectionFailed += AdmonteClient_ServerConnectionFailed;

            btnDisconnect.IsEnabled = false;
            btnSendMessage.IsEnabled = false;
        }

        private void AdmonteClient_ServerConnectionFailed(object? sender, EventArgs e)
        {
            rtbStatus.AppendText("Server is not responding...\n");
        }

        private void AdmonteClient_MessageSent(object? sender, EventArgs e)
        {
            rtbStatus.AppendText("Message sent.\n");
        }

        private void AdmonteClient_ClientDisconnected(object? sender, EventArgs e)
        {
            rtbStatus.AppendText("Disconnected from the server\n");
        }

        private void AdmonteClient_ClientConnected(object? sender, EventArgs e)
        {
            rtbStatus.AppendText("Connected to the server.\n");
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!admonteClient.ConnectTo(txtHost.Text, txtPort.Text))
                return;

            btnConnect.IsEnabled = false;
            btnSendMessage.IsEnabled = true;
            btnDisconnect.IsEnabled = true;
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            admonteClient.Disconnect();

            btnDisconnect.IsEnabled = false;
            btnConnect.IsEnabled = true;
            btnSendMessage.IsEnabled = false;
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!admonteClient.SendMessage(txtMessage.Text))
                rtbStatus.AppendText("Unable to send a message to the server.\n");
        }
    }
}
