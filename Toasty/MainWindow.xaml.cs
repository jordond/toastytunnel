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
using System.Windows.Threading;
using Toaster;
using Logger;

namespace Toasty
{
    public partial class MainWindow : Window
    {
        public static Toast _toaster;
        public static NewTunnel newTunnel;
        private DispatcherTimer timer;
        private Log log = Log.Instance;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _toaster = Toast.Instance;
                if (!_toaster.settings.plinkExists() )
                    findPlink();

                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += new EventHandler(timerTick);
                timer.Start();

                //autostart
                //_toaster.tunnels.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initializing: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            loadListView();
            if (_toaster.tunnels.Count() != 0)
                checkErrors();
        }

        public void loadListView()
        {
            //lstTunnels.Items.Clear();
            lstTunnels.ItemsSource = _toaster.tunnels.All;
            lstTunnels.Items.Refresh();
            //foreach (Tunnel t in _toaster.tunnels.All)
            //{
            //    TunnelItem ti = new TunnelItem();
            //    ti.ID = t.ID;
            //    ti.Name = t.Name;
            //    ti.TunnelDesc = t.identity.User + "@" + t.Host + " ";
            //    if (t.LocalPort != 0 || t.RemoteAddress != null)
            //        ti.TunnelDesc += t.LocalPort + ":" + t.RemoteAddress + ":" + t.RemotePort;
            //    else
            //        ti.TunnelDesc += "D" + t.RemotePort;
            //    ti.Active = t.isOpen;

            //    lstTunnels.Items.Add(ti);
            //}
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            if (!_toaster.settings.plinkExists())
            {
                log.Add(Levels.WARNING, "Can't build tunnel, plink not found.");
                findPlink();
            }
            else
            {
                Button b = sender as Button;
                Tunnel item = b.CommandParameter as Tunnel;

                if (!item.isOpen)
                {
                    log.Add(Levels.INFO, "Opening tunnel: " + item.Name + " - " + item.Description);
                    _toaster.tunnels.Start(item.ID);

                }
                loadListView();
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Tunnel item = b.CommandParameter as Tunnel;

            if (item.isOpen)
            {
                log.Add(Levels.INFO, "Collapsing tunnel: " + item.Name + " - " + item.Description);
                _toaster.tunnels.Stop(item.ID);
            }
            loadListView();
        }

        private void checkErrors()
        {
            List<Tunnel> all = _toaster.tunnels.All;
            foreach (Tunnel t in all.Where(tt => tt.sshErrors.Count() != 0))
            {
                MessageBoxButton choice = new MessageBoxButton();
                string title = "";
                if (t.sshErrors[0] == "The server's host key is not cached in the registry. You" ||
                    t.sshErrors[0] == "WARNING - POTENTIAL SECURITY BREACH!")
                {
                    choice = MessageBoxButton.YesNo;
                    title = "Accept or update " + t.Host + "'s Host key";
                }
                else
                {
                    choice = MessageBoxButton.OK;
                    title = "Server Error has Occurred";
                }

                StringBuilder s = new StringBuilder();
                for (int i = 0; i < t.sshErrors.Count(); i++)
                    s.Append(t.sshErrors[i].ToString() + "\n");

                MessageBoxResult r = MessageBox.Show("Message Recieved: \n\n" + s.ToString(), title, choice, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    t.acceptkey();
                    log.Add(Levels.INFO, "Accepting or updating the SSH Server's Host Key.");
                    t.sshErrors.Clear();
                }
                else if (r == MessageBoxResult.No)
                {
                    log.Add(Levels.WARNING, "Refusing the new Host key, terminating.");
                    _toaster.tunnels.Stop(t.ID);
                }
                else
                {
                    t.sshErrors.Clear();
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            newTunnel = new NewTunnel();
            newTunnel.Show();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            
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
            log.Add(Levels.INFO, "The search for plink begins!");

            if (result == true)
            {
                _toaster.settings.Plink = openDialog.FileName;
                log.Add(Levels.INFO, "Plink was found at '" + _toaster.settings.Plink + "'.");
            }
            else
                log.Add(Levels.WARNING, "You gave up on finding plink...");
        }
        #region Closing methods
        private void close()
        {
            timer.Stop();
            _toaster.tunnels.Stop();
            _toaster.saveSettings();
            System.Threading.Thread.Sleep(1000);
            log.Add(Levels.INFO, "Closing gracefully.");
            Environment.Exit(0);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            close();
        }
        #endregion
    }
}