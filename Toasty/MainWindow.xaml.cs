using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Toaster;

namespace Toasty
{
    public partial class MainWindow : Window
    {
        public static Toast _toaster;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _toaster = Toast.Instance;
                
                lstTunnels.ItemsSource = _toaster.settings.Tunnels;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initializing: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewTunnel newTunnel = new NewTunnel();
            newTunnel.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            lstTunnels.ItemsSource = _toaster.settings.Tunnels;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            _toaster.saveSettings();
        }
    }
}
