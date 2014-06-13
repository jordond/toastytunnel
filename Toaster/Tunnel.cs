using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Toaster
{
    public class Tunnel
    {
        public string Name { get; set; }
        public Process Instance { get; set; }
        public Session instanceSession { get; set; }
        public string ConnectionString { get; set; }
        public bool isStarted { get; set; }

        public ProcessStartInfo InstanceInfo
        {
            get { return setStartInfo(); }
        }

        public Tunnel(Session s)
        {
            Instance = new Process();
            ID = -1;
            instanceSession = s;
            createTunnelName();
        }
           
        public void start()
        {
            try
            {
                Instance.StartInfo = InstanceInfo;
                Toast._logWriter.addEntry(LogLevels.INFO, "Digging the " + Name + " tunnel, with these specs: " + tunnelSpecs());
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
                if (Instance.HasExited)
                {
                    Toast._logWriter.addEntry(LogLevels.WARNING, Name + " has already collapsed...");
                }
                else
                {
                    Toast._logWriter.addEntry(LogLevels.INFO, "Collapsing the " + Name + " tunnel.");
                    Instance.Kill();
                }
                isStarted = false;
            }
            catch(Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Could not collapse the " + Name + " tunnel: " + ex.Message);
                throw new Exception("Tunnel.cs - stop() - " + ex.Message);
            }
        }

        private void createTunnelName()
        {
            Name = instanceSession.identity.User + "@" + instanceSession.Host +
                (instanceSession.IsLocal == false ? " D:" + instanceSession.RemotePort :
                " L:" + instanceSession.LocalPort + ":" + instanceSession.RemoteAddress +
                ":" + instanceSession.RemotePort);
        }

        private ProcessStartInfo setStartInfo()
        {
            ProcessStartInfo temp = new ProcessStartInfo();
            temp.FileName = Toast.plinkLocation;
            temp.Arguments = instanceSession.ConnectionString;
            temp.WindowStyle = ProcessWindowStyle.Minimized;
#if !DEBUG
            temp.UseShellExecute = false;
            temp.CreateNoWindow = true;
#endif
            return temp;
        }

        private string tunnelSpecs()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("");
            s.AppendLine("ID #: " + ID);
            s.AppendLine("Name: " + Name);
            s.AppendLine("Identity: ");
            s.AppendLine("User: " + instanceSession.identity.User);
            s.AppendLine("Host: " + instanceSession.Host);
            s.AppendLine("Ports: " + (instanceSession.IsLocal == false ? " D:" + 
                        instanceSession.RemotePort : " L:" + instanceSession.LocalPort + ":" + 
                        instanceSession.RemoteAddress + ":" + instanceSession.RemotePort));
            s.AppendLine(instanceSession.identity.PrivateKey == "" ? "Key: none" 
                        : "Key: " + Path.GetFileName(instanceSession.identity.PrivateKey));
            s.AppendLine("Password: " + instanceSession.identity.Password == "" ? "Password: none" : "Password: yes");
            s.AppendLine("Quick Connect: " + instanceSession.identity.Save);
            return s.ToString();
        }
    }
}