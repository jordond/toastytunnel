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
            checkErrors();
        }

        public void loadListView()
        {
            lstTunnels.Items.Clear();
            foreach (Tunnel t in _toaster.tunnels.All)
            {
                TunnelItem ti = new TunnelItem();
                ti.ID = t.ID;
                ti.Name = t.Name;
                ti.TunnelDesc = t.identity.User + "@" + t.Host + " ";
                if (t.LocalPort != 0 || t.RemoteAddress != null)
                    ti.TunnelDesc += t.LocalPort + ":" + t.RemoteAddress + ":" + t.RemotePort;
                else
                    ti.TunnelDesc += "D" + t.RemotePort;
                ti.Active = t.isOpen;

                lstTunnels.Items.Add(ti);
            }
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
                TunnelItem item = b.CommandParameter as TunnelItem;

                if (!item.Active)
                {
                    log.Add(Levels.INFO, "Opening tunnel: " + item.Name + " - " + item.TunnelDesc);
                    _toaster.tunnels.Start(item.ID);

                }
                loadListView();
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TunnelItem item = b.CommandParameter as TunnelItem;

            if (item.Active)
            {
                log.Add(Levels.INFO, "Collapsing tunnel: " + item.Name + " - " + item.TunnelDesc);
                _toaster.tunnels.Stop(item.ID);
            }
            loadListView();
        }

        private void checkErrors()
        {
            List<Tunnel> open = _toaster.tunnels.Open;
            foreach (Tunnel t in open.Where(tt => tt.sshErrors.Count() != 0))
            {
                MessageBoxResult r = MessageBox.Show("Server Message: \n\n" + string.Join(" ", t.sshErrors.ToArray()), 
                    "Interaction Required", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    t.acceptkey();
                    log.Add(Levels.INFO, "Accepting or updating the SSH Server's Host Key.");
                    t.sshErrors.Clear();
                }
                else
                {
                    log.Add(Levels.WARNING, "Refusing the new Host key, terminating.");
                    _toaster.tunnels.Stop(t.ID);
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

    public class TunnelItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TunnelDesc { get; set; }
        public string Port { get; set; }
        public bool Active { get; set; }
    }
}