using Client.Business;
using Client.ViewModel;
using System;
using System.Windows;
namespace Client.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientMainWindow : Window
    {
        public ClientMainWindow()
        {
            InitializeComponent();
            this.DataContext = new AdmonteClientViewModel();
        }
    }
}
