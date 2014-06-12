using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Toaster
{
    public class Tunnel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Process Instance { get; set; }        
        public string ConnectionString { get; set; }
        public bool isStarted { get; set; }

        public Tunnel()
        {
            ID = -1;
            Name = "";
            ConnectionString = "";
        }

        public ProcessStartInfo InstanceInfo 
        {
            get
            {
                InstanceInfo.FileName = Toast.plinkLocation;
                InstanceInfo.Arguments = ConnectionString;
                InstanceInfo.WindowStyle = ProcessWindowStyle.Minimized;
                InstanceInfo.UseShellExecute = false;
                InstanceInfo.CreateNoWindow = true;
                return InstanceInfo;
            }
            set { } 
        }
        
        public void createTunnelName(Session s)
        {
            Name = s.identity.User + "@" + s.Host + (s.IsLocal == false ? " D:" + s.RemotePort
                            : " L:" + s.LocalPort + ":" + s.RemoteAddress + ":" + s.RemotePort);
        }

        public void start()
        {
            try
            {
                Instance.StartInfo = InstanceInfo;
                Toast._logWriter.addEntry(LogLevels.INFO, "Digging the " + Name + " tunnel, with these specs: " + InstanceInfo.ToString());
                isStarted = Instance.Start();
            }
            catch(Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Digging the " + Name + " tunnel failed: " + ex.Message);
                throw new Exception("Tunnel.cs - start() - " + ex.Message);
            }
        }

        public void stop()
        {
            try
            {
                Toast._logWriter.addEntry(LogLevels.INFO, "Collapsing the " + Name + " tunnel.");
                Instance.Kill();
                isStarted = false;
            }
            catch(Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Could not collapse the " + Name + " tunnel: " + ex.Message);
                throw new Exception("Tunnel.cs - stop() - " + ex.Message);
            }
        }
    }
}