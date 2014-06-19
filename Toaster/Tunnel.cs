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
        public int ID               { get; set; }
        public string Name          { get; set; }
        public Identity identity    { get; set; }
        public string Host          { get; set; }
        public int Port             { get; set; }
        public int LocalPort        { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort       { get; set; }
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
                if (!File.Exists(identity.PrivateKey) || identity.Password != null)
                    cs.Append("-pw " + identity.Password);
                return cs.ToString();
            }
        }        
        public bool autoStart       { get; set; }
        public bool isOpen
        {
            get
            {
                if (Instance != null)
                    return !Instance.HasExited;
                return false;
            }
            set { }
        }
        public Process Instance     { get; set; }        
        public ProcessStartInfo InstanceInfo
        {
            get 
            {
                if (Instance != null)
                {
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = Toast.Instance.settings.Plink;
                    info.Arguments = ConnectionString;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    //info.RedirectStandardOutput = true;
                    //info.RedirectStandardError = true;
                    //info.UseShellExecute = false;
                    #if !DEBUG
                        info.UseShellExecute = false;
                        info.CreateNoWindow = true;
                    #endif
                    return info;
                }
                return null;
            }
        }

        public Tunnel()
        {
            //Instance = new Process();
        }

        public void acceptHostKey()
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                StringBuilder s = new StringBuilder();
                s.Append("/c echo y | ");
                s.Append(Toast.Instance.settings.Plink + " ");
                s.Append(ConnectionString);
                p.StartInfo.Arguments = s.ToString();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.Kill();
            }            
        }

        //per tunnel start
        public void Start()
        {
            try
            {
                acceptHostKey();
                Instance = new Process();
                Instance.StartInfo = InstanceInfo;
                Toast.Instance.logger.Add(Levels.INFO, "Digging the " + Name + " tunnel, with these specs: " + tunnelSpecs());
                //Instance.OutputDataReceived += new DataReceivedEventHandler(outputHandler);
                isOpen = Instance.Start();
                //Instance.BeginOutputReadLine();
                //Instance.WaitForExit();
            }
            catch (Exception ex)
            {
                Toast.Instance.logger.Add(Levels.ERROR, "Digging the " + Name + " tunnel failed: " + ex.Message);
                throw new Exception("Tunnel.cs - start() - " + ex.Message);
            }
        }

        private static void outputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                Toast.Instance.logger.Add(Levels.INFO, outLine.Data);
            }
        }

        //per tunnel stop
        public void Stop()
        {
            try
            {
                if (Instance == null)                
                {
                    Toast.Instance.logger.Add(Levels.INFO, "Skipping tunnel, already closed.");
                }
                else if (Instance.HasExited)
                {
                    Toast.Instance.logger.Add(Levels.INFO, Name + " has already collapsed");
                }
                else
                {
                    Toast.Instance.logger.Add(Levels.INFO, "Collapsing the " + Name + " tunnel.");                    
                    Instance.Kill();
                    Instance.Close();
                }
                Instance = null;
                isOpen = false;
            }
            catch (Exception ex)
            {
                Toast.Instance.logger.Add(Levels.ERROR, "Could not collapse the " + Name + " tunnel: " + ex.Message);
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
            return s.ToString();
        }
    }
}