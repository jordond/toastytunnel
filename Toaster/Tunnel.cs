using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Logger;

namespace Toaster
{
    public class Tunnel
    {
        private Log logger = Log.Instance;
        public int ID { get; set; }
        public string Name { get; set; }
        public Identity identity { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int LocalPort { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }
        public string ConnectionString
        {
            get
            {
                StringBuilder cs = new StringBuilder();
                cs.Append("-ssh ");
                if (File.Exists(identity.PrivateKey))
                    cs.Append("-i " + identity.PrivateKey + " ");
                if (LocalPort != 0)
                    cs.Append("-L " + LocalPort + ":" + RemoteAddress + ":" + RemotePort + " ");
                else
                    cs.Append("-D " + RemotePort + " ");
                cs.Append(identity.User + "@" + Host + " ");
                if (Port != 22)
                    cs.Append("-p " + Port + " ");
                if (!File.Exists(identity.PrivateKey) || identity.Password != "")
                    cs.Append("-pw " + identity.Password);

                return cs.ToString();
            }
            set { }
        }        
        public bool autoStart { get; set; }
        public bool isOpen
        {
            get
            {
                if (Instance != null)
                    return Instance.HasExited;
                return false;
            }
            set { }
        }
        public Process Instance { get; set; }        
        public ProcessStartInfo InstanceInfo
        {
            get 
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = Toast.plinkLocation;
                info.Arguments = ConnectionString;
                info.WindowStyle = ProcessWindowStyle.Minimized;
                #if !DEBUG
                temp.UseShellExecute = false;
                temp.CreateNoWindow = true;
                #endif
                return info;
            }
        }

        public Tunnel()
        {
            Instance = new Process();
        }

        //per tunnel start
        public void Start()
        {
            try
            {
                Instance.StartInfo = InstanceInfo;
                logger.Add(Levels.INFO, "Digging the " + Name + " tunnel, with these specs: " + tunnelSpecs());
                isOpen = Instance.Start();
            }
            catch (Exception ex)
            {
                logger.Add(Levels.ERROR, "Digging the " + Name + " tunnel failed: " + ex.Message);
                throw new Exception("Tunnel.cs - start() - " + ex.Message);
            }
        }

        //per tunnel stop
        public void Stop()
        {
            try
            {
                if (Instance.HasExited)
                {
                    logger.Add(Levels.WARNING, Name + " has already collapsed...");
                }
                else
                {
                    logger.Add(Levels.INFO, "Collapsing the " + Name + " tunnel.");
                    Instance.Kill();
                }
                isOpen = false;
            }
            catch (Exception ex)
            {
                logger.Add(Levels.ERROR, "Could not collapse the " + Name + " tunnel: " + ex.Message);
                throw new Exception("Tunnel.cs - stop() - " + ex.Message);
            }
        }

        private string tunnelSpecs()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("");
            s.AppendLine("Name: " + Name);
            s.AppendLine("Identity: ");
            s.AppendLine("User: " + identity.User);
            s.AppendLine("Host: " + Host);
            s.AppendLine("Ports: " + (LocalPort == 0 ? " D:" +
                        RemotePort : " L:" + LocalPort + ":" +
                        RemoteAddress + ":" + RemotePort));
            s.AppendLine(identity.PrivateKey == "" ? "Key: none"
                        : "Key: " + Path.GetFileName(identity.PrivateKey));
            s.AppendLine("Password: " + identity.Password == "" ? "Password: none" : "Password: yes");
            s.AppendLine("Quick Connect: " + identity.Save);
            return s.ToString();
        }
    }
}