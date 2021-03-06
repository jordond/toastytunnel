﻿using System;
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
    public enum LogLevels
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2,
        DEBUG = 3
    };

    public class LogWriter
    {
        private string logFilePath = "logs\\";
        private string logFileName = "toastytunnel.log";
        private string logFileFull;
         
        public LogWriter()
        {
            createLogfile();
            writeToLog(logHeader());
        }

        public void addEntry(LogLevels logLevel, string message)
        {
            StringBuilder wl = new StringBuilder();            
            wl.Append(DateTime.Now.ToString() + "- ");
            wl.Append(getLogLevel(logLevel));
            wl.Append(message);

            writeToLog(wl.ToString());
        }

        private void createLogfile()
        {
            try
            {
                logFileFull = logFilePath + logFileName;
                if (!Directory.Exists(logFilePath))
                    Directory.CreateDirectory(logFilePath);
                if (!File.Exists(logFileFull))
                    File.Create(logFileFull).Dispose();
                else
                {
                    if (File.Exists(logFileFull + ".old"))
                        File.Delete(logFileFull + ".old");
                    File.Move(logFileFull, logFileFull + ".old");
                    File.Create(logFileFull).Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }          
        }

        private void writeToLog(string line)
        {
            using (StreamWriter sw = new StreamWriter(logFileFull, true))
            {
                sw.WriteLine(line);
            }            
        }

        #region Log writing helpers
        private string logHeader()
        {
            StringBuilder h = new StringBuilder();
            h.AppendLine("#########################################################");
            h.AppendLine(" Toasty Tunnel - Easy SSH Tunnel using PuTTY and Plink");
            h.AppendLine("           Jordon de Hoog - Hooger 2014");
            h.AppendLine("#########################################################");
            h.AppendLine("Library Version: " + FileVersionInfo.GetVersionInfo(logFileFull).ProductVersion);
            h.AppendLine("Tunneler Version: ");
            h.AppendLine("Date: " + DateTime.Now.ToString());
            h.AppendLine("Working Directory: " + Directory.GetCurrentDirectory());
            h.AppendLine("Data File Location: ");
            h.AppendLine("Log File Location: " + logFilePath + logFileName);
            h.AppendLine("Found Plink: " + File.Exists("files\\plink.exe"));
            h.AppendLine("Local IP: " + getIPAddress());
            h.AppendLine("#########################################################");
            return h.ToString();
        }

        private string getLogLevel(LogLevels errorCode)
        {
            switch (errorCode)
            {
                case LogLevels.INFO:    return "INFO: ";
                case LogLevels.WARNING: return "WARNING: ";
                case LogLevels.ERROR:   return "ERROR: ";
                case LogLevels.DEBUG:   return "DEBUG: ";
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
        #endregion
    }
}
