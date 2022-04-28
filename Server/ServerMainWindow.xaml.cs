using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using Server.Business;
using Server.ViewModel;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainWindow : Window
    {
        public ServerMainWindow()
        {
            InitializeComponent();
            this.DataContext = new AdmonteServerViewModel();
        }
    }
}
