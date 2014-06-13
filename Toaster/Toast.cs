using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Toast
    {
        public Settings settings { get; set; }
        public List<Tunnel> _tunnels = new List<Tunnel>();        
        public static LogWriter _logWriter;
        public static string plinkLocation; 

        public Toast()
        {
            try
            {
                _logWriter = new LogWriter();
                loadSettings();
                settings.saveSettings();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void debugCreate()
        {
            
        }
        public void debugKill()
        {
            
        }

        private void loadSettings()
        {
            settings = Settings.Load();
            plinkLocation = settings.Plink;

            Toast._logWriter.addEntry(LogLevels.INFO, "Found " + settings.Identities.Count() + " identities, and " + settings.Sessions.Count() + " sessions.");
            foreach (Identity i in settings.Identities)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found identity: " + i.Name);
            foreach (Tunnel t in settings.Tunnels)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found tunnel: " + t.Name);
        }
    }
}