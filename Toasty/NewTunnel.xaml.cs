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

        public NewTunnel()
        {
            InitializeComponent();
            loadListView();
        }

        private void loadListView()
        {
            lstIdentities.Items.Clear();
            foreach (Identity i in _toaster.settings.Identities)
            {
                IdentityItem s = new IdentityItem();
                
                s.ID = i.ID;
                s.Name = i.Name;
                s.User = i.User;
                if (i.Password != "" || i.Password != null)
                    s.Password = "YES";
                else
                    s.Password = "NO";
                if (i.PrivateKey != "" || i.PrivateKey != null)
                    s.PrivateKey = System.IO.Path.GetFileName(i.PrivateKey);
                else
                    s.PrivateKey = "NO";
                
                lstIdentities.Items.Add(s);
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            IdentityItem item = b.CommandParameter as IdentityItem;

            MessageBoxResult result = MessageBox.Show("Are you sure you would like to delete the " + item.Name + " identity", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if (result == MessageBoxResult.Yes)
            {
                _log.Add(Levels.INFO, "Deleting saved identity: " + item.Name + " - Username: " + item.User);
                _toaster.settings.Identities.RemoveAll(i => i.ID == item.ID);
            }
            loadListView();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            Identity i = new Identity();
            i.ID = _toaster.settings.Identities.Count() + 1;
            i.Name = txtIName.Text;
            i.User = txtUsername.Text;
            i.PrivateKey = txtPrivateKey.Text;
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
            _toaster.settings.Identities.Add(i);
                        
            this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //Create the file open dialog
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            //Set the filter options
            openDialog.Title = "Select the private key you want to use.";
            openDialog.Filter = "Private Key |*.ppk";
            openDialog.FilterIndex = 1;

            //Show the dialog box to the user
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
                txtPrivateKey.Text = openDialog.FileName;
        }
    }

    public class IdentityItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PrivateKey { get; set; }
    }
}