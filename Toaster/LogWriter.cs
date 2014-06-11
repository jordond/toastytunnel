using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;

namespace Toaster
{
    public class LogWriter
    {
        private string logFilePath = @"logs\";
        private string logFileName = "toastytunnel.log";
        private string logFileFull;
 
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

        public void writeLog(string logLevel, string date, string message)
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
            h.AppendLine("Local IP: ");
            h.AppendLine("#########################################################");
            h.AppendLine("");
            return h.ToString();
        }
    }
}
