using Client.Business;
using SharedCodeLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client.ViewModel
{
    public class AdmonteClientViewModel : DependencyObject
    {
        private readonly ClientEngine _engine;

        public AdmonteClientViewModel() 
        {
            _engine = new ClientEngine();
            LogMessages = new ObservableCollection<string>();

            #region Init commands
            ConnectCommand = new UniversalCommand(executeMethod: ExecuteConnect, canExecuteMethod: CanExecuteConnect);
            DisconnectCommand = new UniversalCommand(executeMethod: ExecuteDisconnect, canExecuteMethod: CanExecuteDisconnect);
            SendMessageCommand = new UniversalCommand(executeMethod: ExecuteSendMessage, canExecuteMethod: CanExecuteSendMessage);
            #endregion

            #region Event subscribtions
            _engine.OnError += _engine_OnError;
            _engine.OnClientConnected += _engine_OnClientConnected;
            _engine.OnClientDisconnected += _engine_OnClientDisconnected;
            _engine.OnServerConnectionFailed += _engine_OnServerConnectionFailed;
            #endregion

            // Testing
            ServerAddress = "127.0.0.1";
            ServerPort = "5444";
        }

        #region Client engine event handlers
        private void _engine_OnServerConnectionFailed(object? sender, EventArgs e)
        {
            Dispatcher?.Invoke(() => LogMessages.Add("Server is not responding.\nPlease check your network connection."));
        }

        private void _engine_OnClientDisconnected(object? sender, EventArgs e)
        {
            Dispatcher?.Invoke(() => LogMessages.Add("Client disconnected from the server."));
        }

        private void _engine_OnClientConnected(object? sender, System.EventArgs e)
        {
            Dispatcher?.Invoke(() => LogMessages.Add("Client connected to the server."));
        }

        private void _engine_OnError(object? sender, System.EventArgs e)
        {
            Dispatcher?.Invoke(() => LogMessages.Add("Internal client error."));
        }
        #endregion

        #region Command methods
        public void ExecuteSendMessage()
        {
            _engine.SendMessage(Message);
        }

        public bool CanExecuteSendMessage()
        {
            return _engine.IsConnectedToServer;
        }

        public void ExecuteConnect()
        {
            try
            {
                _engine.ConnectTo(ServerAddress, ServerPort);
            }
            catch (Exception ex)
            {
                Dispatcher?.Invoke(() =>LogMessages.Add(ex.Message));
            }
        }

        public bool CanExecuteConnect()
        {
            return !_engine.IsConnectedToServer;
        }

        public void ExecuteDisconnect()
        {
            _engine.Disconnect();
        }

        public bool CanExecuteDisconnect()
        {
            return _engine.IsConnectedToServer;
        }
        #endregion

        #region Commands
        public UniversalCommand ConnectCommand { get; set; }
        public UniversalCommand DisconnectCommand { get; set; }
        public UniversalCommand SendMessageCommand { get; set; }
        #endregion

        #region Properties
        public static readonly DependencyProperty ServerAddressProperty = DependencyProperty.Register(
            name: "ServerAddress",
            propertyType: typeof(string),
            ownerType: typeof(AdmonteClientViewModel));

        public string ServerAddress
        {
            get { return (string)GetValue(ServerAddressProperty); }
            set { SetValue(ServerAddressProperty, value); }
        }

        public static readonly DependencyProperty ServerPortProperty = DependencyProperty.Register(
            name: "ServerPort",
            propertyType: typeof(string),
            ownerType: typeof(AdmonteClientViewModel));

        public string ServerPort
        {
            get { return (string)GetValue(ServerPortProperty); }
            set { SetValue(ServerPortProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            name: "Message",
            propertyType: typeof(string),
            ownerType: typeof(AdmonteClientViewModel));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty LogMessagesProperty = DependencyProperty.Register(
            name: "LogMessages",
            propertyType: typeof(ObservableCollection<string>),
            ownerType: typeof(AdmonteClientViewModel));

        public ObservableCollection<string> LogMessages
        {
            get { return (ObservableCollection<string>)GetValue(LogMessagesProperty); }
            set { SetValue(LogMessagesProperty, value); }
        }
        #endregion
    }
}