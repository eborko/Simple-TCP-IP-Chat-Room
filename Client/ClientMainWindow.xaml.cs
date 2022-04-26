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

            btnDisconnect.IsEnabled = false;
            btnSendMessage.IsEnabled = false;
        }

        private void AdmonteClient_ClientConnected(object? sender, EventArgs e)
        {
            txtMessage.AppendText("Connected to server.");
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            admonteClient = new AdmonteClient();
            admonteClient.ConnectTo(txtHost.Text, txtPort.Text);

            btnConnect.IsEnabled = false;
            btnSendMessage.IsEnabled = true;
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            admonteClient.Disconnect();

            txtMessage.AppendText("Disconnected from server.");

            btnDisconnect.IsEnabled = false;
            btnConnect.IsEnabled = true;
            btnSendMessage.IsEnabled = false;
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
