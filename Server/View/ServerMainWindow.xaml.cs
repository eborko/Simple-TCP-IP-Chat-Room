using System.Windows;
using Server.ViewModel;

namespace AdmonteServer.View
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
