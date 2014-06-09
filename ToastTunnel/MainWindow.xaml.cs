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
using System.Diagnostics;
using System.IO;
using ToastConfig;

namespace ToastTunnel
{    
    public partial class MainWindow : Window
    {
        private const string _client = "files\\plink.exe";
        private Host _host = new Host();
        private Process _plink;        
        public bool isInit = false;

        public MainWindow()
        {
            InitializeComponent();
            cmbHosts.ItemsSource = _host.Hosts;
            isInit = true;
        }
        private bool canStart()
        {
            if (txtUser.Text == ""  || cmbHosts.Text == "" || txtPrivateKey.Text == "")
                return false;
            if (chkTunnel.IsChecked == true)
            {
                if (txtTunnelPort.Text == "")
                    return false;
            }
            return true;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //Create the file open dialog
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            //Set the filter options
            openDialog.Title = "Select the private key you want to use.";
            openDialog.Filter = "Private Key |*.ppk| All files |*.*";
            openDialog.FilterIndex = 1;

            //Show the dialog box to the user
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
                txtPrivateKey.Text = openDialog.FileName;
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(txtPrivateKey.Text) && cmbHosts.Text != "")
            {
                _host.Hosts.Add(cmbHosts.Text);
                _host.saveHosts();
                cmdStop.IsEnabled = startSession("-ssh -i " + txtPrivateKey.Text + " -D 12344 " + txtUser.Text + "@" + cmbHosts.Text);
            }
            else
            {
                string error = cmbHosts.Text == "" ? "No host was entered, please select or type one in." : "The Private key could not be found";
                MessageBox.Show(error, "Error: cmdStart_Click", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool startSession(string sshCommand)
        {
            try
            {
                if (!File.Exists(_client))
                    return false;

                _plink = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.Arguments = sshCommand;
                info.FileName = _client;
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                _plink.StartInfo = info;
                _plink.Start();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create session: " + ex.Message, "Error: startSession", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            _plink.Kill();
            cmdStop.IsEnabled = false;
        }

        #region cmdStart Triggers
        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isInit)
                cmdStart.IsEnabled = canStart();
        }

        private void txtPrivateKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isInit)
                cmdStart.IsEnabled = canStart();
        }

        private void txtTunnelPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isInit)
                cmdStart.IsEnabled = canStart();
        }

        private void chkTunnel_Click(object sender, RoutedEventArgs e)
        {
            if (isInit)
                cmdStart.IsEnabled = canStart();
        }

        private void cmbHosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInit)
                cmdStart.IsEnabled = canStart();
        }
        #endregion
    }
}
