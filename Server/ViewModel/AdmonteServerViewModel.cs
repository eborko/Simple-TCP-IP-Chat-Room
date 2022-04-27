using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Server.ViewModel
{
    public class AdmonteServerViewModel : DependencyObject
    {
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
            propertyType: typeof(int),
            ownerType: typeof(AdmonteServerViewModel));

        public int ServerPort
        {
            get { return (int)GetValue(ServerPortProperty); }
            set { SetValue(ServerPortProperty, value); }
        }

        public static readonly DependencyProperty LogMessagesProperty = DependencyProperty.Register(
            name: "LogMessages",
            propertyType: typeof(IList<string>),
            ownerType: typeof(AdmonteServerViewModel));

        public IList<string> LogMessages
        { get { return (IList<string>)GetValue(LogMessagesProperty); } }
    }
}
