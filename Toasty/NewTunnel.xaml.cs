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

using Toaster;
namespace Toasty
{
    /// <summary>
    /// Interaction logic for New.xaml
    /// </summary>
    public partial class NewTunnel : Window
    {
        private Toast _toaster = Toast.Instance;

        public NewTunnel()
        {
            InitializeComponent();
        }

        private void loadListView()
        {
            foreach (Identity i in _toaster.settings.Identities)
            {

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Tunnel n = new Tunnel();
            n.Name = txtTName.Text;
            n.Host = txtHost.Text;
            n.Port = int.Parse(txtSshPort.Text);
            n.RemotePort = int.Parse(txtRemotePort.Text);
            n.autoStart = (bool)chkAuto.IsChecked;

            Identity i = new Identity();
            i.Name = txtIName.Text;
            i.User = txtUsername.Text;
            i.PrivateKey = txtPrivateKey.Text;
            i.Save = (bool)chkSave.IsChecked;

            n.identity = i;
            _toaster.settings.Tunnels.Add(n);
            _toaster.settings.Identities.Add(i);
            
            this.Close();
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
    }
}
