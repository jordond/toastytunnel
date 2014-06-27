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
using System.Windows.Shapes;
using System.IO;

using Toaster;
using Logger;

namespace Toasty
{
    /// <summary>
    /// Interaction logic for New.xaml
    /// </summary>
    public partial class NewTunnel : Window
    {
        private Toast _toaster = Toast.Instance;
        private Log _log = Log.Instance;
        private Identity _identity;

        public NewTunnel()
        {
            InitializeComponent();
            lstIdentities.ItemsSource = _toaster.settings.Identities;
        }
        
        private void Delete(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Identity item = b.CommandParameter as Identity;

            MessageBoxResult result = MessageBox.Show("Are you sure you would like to delete the " + item.Name + " identity", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (result == MessageBoxResult.Yes)
            {
                _log.Add(Levels.INFO, "Deleting saved identity: " + item.Name + " - Username: " + item.User);
                _toaster.settings.Identities.RemoveAll(i => i.ID == item.ID);
            }
            lstIdentities.Items.Refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Identity i = new Identity();
            i.Name = txtIName.Text;
            i.User = txtUsername.Text;
            if (txtPrivateKey.Text != "")
                i.PrivateKey = txtPrivateKey.Text;
            if (txtPassword.Password != "")
                i.Password = txtPassword.Password;
            i.Save = (bool)chkSave.IsChecked;

            Tunnel n = new Tunnel();
            n.ID = _toaster.settings.Tunnels.Count() + 1;
            n.Name = txtTName.Text;
            n.identity = i;
            n.Host = txtHost.Text;
            n.Port = int.Parse(txtSshPort.Text);
            if (rdLocal.IsChecked == true )
            {
                n.LocalPort = int.Parse(txtLocalPort.Text);
                n.RemoteAddress = txtRemoteHost.Text;
            }
            n.RemotePort = int.Parse(txtRemotePort.Text);
            n.autoStart = (bool)chkAuto.IsChecked;

            _toaster.tunnels.Add(n);

            if (_identity == null)
            {
                i.ID = _toaster.settings.Identities.Count() + 1;
                _toaster.settings.Identities.Add(i);
            }
            else
            {
                int oldID = _identity.ID;
                _identity = i;
                _identity.ID = oldID;
            }
            

            this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //Create the file open dialog
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Title = "Select the private key you want to use.";
            openDialog.Filter = "Private Key |*.ppk";
            openDialog.FilterIndex = 1;
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
                txtPrivateKey.Text = openDialog.FileName;
        }

        private void clearTextBoxes()
        {
            txtIName.Text = "";
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtPrivateKey.Text = "";
        }

        private void lstIdentities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearTextBoxes();
            if (lstIdentities.SelectedItem != null)
            {
                Identity i = lstIdentities.SelectedItem as Identity;
                if (i != null)
                {
                    _identity = i;
                    txtIName.Text = i.Name;
                    txtUsername.Text = i.User;
                    txtPassword.Password = i.Password;
                    txtPrivateKey.Text = i.PrivateKey;
                }
            }
        }
    }
}