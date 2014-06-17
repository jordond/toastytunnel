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
using Logger;

namespace Toasty
{
    public partial class MainWindow : Window
    {
        public static Toast _toaster;
        public static NewTunnel newTunnel;
        private Log _log = Log.Instance;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _toaster = Toast.Instance;
                if (!_toaster.settings.plinkExists() )
                    findPlink();

                loadListView();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initializing: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void loadListView()
        {
            lstTunnels.Items.Clear();
            foreach (Tunnel t in _toaster.tunnels.All)
            {
                TunnelItem ti = new TunnelItem();
                ti.ID = t.ID;
                ti.Name = t.Name;
                ti.TunnelDesc = t.identity.User + "@" + t.Host;
                if (t.LocalPort != 0 || t.RemoteAddress != null)
                    ti.Port = t.LocalPort + "==" + t.RemoteAddress + ":" + t.RemotePort;
                else
                    ti.Port = "D" + t.RemotePort;
                ti.Active = t.isOpen;

                lstTunnels.Items.Add(ti);
            }
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TunnelItem item = b.CommandParameter as TunnelItem;
            
            if (!item.Active)
            {
                _log.Add(Levels.INFO, "Opening tunnel: " + item.Name + " - " + item.TunnelDesc);
                _toaster.tunnels.Start(item.ID);
            }
            loadListView();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TunnelItem item = b.CommandParameter as TunnelItem;

            if (item.Active)
            {
                _log.Add(Levels.INFO, "Collapsing tunnel: " + item.Name + " - " + item.TunnelDesc);
                _toaster.tunnels.Stop(item.ID);
            }
            loadListView();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            newTunnel = new NewTunnel();
            newTunnel.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _toaster.tunnels.Stop();
            _toaster.saveSettings();
            Environment.Exit(0);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            _toaster.saveSettings();
        }

        private void findPlink()
        {
            //Create the file open dialog
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            //Set the filter options
            openDialog.Title = "Please Locate the Plink executable.";
            openDialog.Filter = "Plink |*.exe";
            openDialog.FilterIndex = 1;

            //Show the dialog box to the user
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
                _toaster.settings.Plink = openDialog.FileName;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _toaster.tunnels.Stop();
                _toaster.saveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class TunnelItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TunnelDesc { get; set; }
        public string Port { get; set; }
        public bool Active { get; set; }
    }
}
