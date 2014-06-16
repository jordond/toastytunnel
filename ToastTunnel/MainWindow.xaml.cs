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
using Toaster;

namespace ToastTunnel
{    
    public partial class MainWindow : Window
    {
        Toast temp;
        public MainWindow()
        {
            InitializeComponent();
            temp = new Toast();
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
            temp.debugCreate();
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            temp.debugKill();
        }
    }
}
