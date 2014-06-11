using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Toaster
{
    public class LogWriter
    {
        private string logFilePath = @"logs\";
        private string logFileName = "toastytunnel.log";
        private string logFileFull;

        public enum LogLevels
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2,
            DEBUG = 3
        };
 
        public LogWriter()
        {
            logFileFull = logFilePath + logFileName;
            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);
            if (!File.Exists(logFileFull))
                File.Create(logFileFull);
            else
            {
                File.Move(logFileFull, logFileFull + File.GetLastWriteTime(logFileFull));
                File.Create(logFileFull);
            }

            

        }

        public void writeLog(int logLevel, string date, string message)
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }

        private string logHeader()
        {
            StringBuilder h = new StringBuilder();
            h.AppendLine("#########################################################");
            h.AppendLine(" Toasty Tunnel - Easy SSH Tunnel using PuTTY and Plink");
            h.AppendLine("           Jordon de Hoog - Hooger 2014");
            h.AppendLine("#########################################################");
            h.AppendLine("Library Version: " + FileVersionInfo.GetVersionInfo(logFileFull).FileVersion);
            h.AppendLine("Tunneler Version: ");
            h.AppendLine("Date: " + DateTime.Now.ToString());
            h.AppendLine("Working Directory: " + Directory.GetCurrentDirectory());
            h.AppendLine("Data File Location: ");
            h.AppendLine("Log File Location: " + Path.GetFullPath(logFilePath) + logFileName);
            h.AppendLine("Plink Location:" + Path.GetFullPath("File\\"));
            h.AppendLine("Local IP: " + getIPAddress());
            h.AppendLine("#########################################################");
            h.AppendLine("");
            return h.ToString();
        }

        private string getLogLevel(int errorCode)
        {
            switch (errorCode)
            {
                case 0: return "INFO: ";
                case 1: return "WARNING:  ";
                case 2: return "ERROR: ";
                case 3: return "DEBUG: ";
                default: return "NONE: ";
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
