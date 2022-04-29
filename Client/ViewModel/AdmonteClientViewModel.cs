using System.Collections.ObjectModel;
using System.Windows;

namespace Client.ViewModel
{
    public class AdmonteClientViewModel : DependencyObject
    {
        public AdmonteClientViewModel() 
        {
            LogMessages = new ObservableCollection<string>();
        }

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