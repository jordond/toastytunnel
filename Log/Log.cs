using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Logger
{
    public enum Levels
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2,
        DEBUG = 3
    };

    public class Log
    {
        private static Log instance;
        public string Filename { get; set; }
        public string Path { get; set; }
        public string fullPath
        {
            get
            {
                return Path + Filename;
            }
        }

        public static Log Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new Log();
                }
                return instance;
            }
        }

        public Log()
        {
            Path = @"logs\";
            Filename = DateTime.Now.ToString("dd_MM_yyyy") + ".log";
            Create();
            Write(Header());
        }

        public void Add(Levels level, string message)
        {
            StringBuilder wl = new StringBuilder();
            wl.Append(DateTime.Now.ToString() + "- ");
            wl.Append(getLevel(level));
            wl.Append(message);

            Write(wl.ToString());
        }

        private void Write(string line)
        {
            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                sw.WriteLine(line);
            }
        }

        private void Create()
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                if (!File.Exists(fullPath))
                    File.Create(fullPath).Dispose();
                else
                {
                    if (File.Exists(fullPath + ".old"))
                        File.Delete(fullPath + ".old");
                    File.Move(fullPath, fullPath + ".old");
                    File.Create(fullPath).Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string Header()
        {
            StringBuilder h = new StringBuilder();
            h.AppendLine("#########################################################");
            h.AppendLine(" Toasty Tunnel - Easy SSH Tunnel using PuTTY and Plink");
            h.AppendLine("           Jordon de Hoog - Hooger 2014");
            h.AppendLine("#########################################################");
            h.AppendLine("Tunneler Version: ");
            h.AppendLine("Date: " + DateTime.Now.ToString());
            h.AppendLine("Working Directory: " + Directory.GetCurrentDirectory());
            h.AppendLine("Data File Location: ");
            h.AppendLine("Log File Location: " + fullPath);
            h.AppendLine("Found Plink: " + File.Exists("files\\plink.exe"));
            h.AppendLine("Local IP: " + getIPAddress());
            h.AppendLine("#########################################################");
            return h.ToString();
        }

        private string getLevel(Levels errorCode)
        {
            switch (errorCode)
            {
                case Levels.INFO:       return "INFO: ";
                case Levels.WARNING:    return "WARNING: ";
                case Levels.ERROR:      return "ERROR: ";
                case Levels.DEBUG:      return "DEBUG: ";
                default:                return "";
            }
        }

        private string getIPAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }
            return "Unvailable";
        }
    }
}
