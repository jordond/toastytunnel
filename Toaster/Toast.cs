using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logger;

namespace Toaster
{
    public class Toast
    {
        public Settings settings { get; set; }
        public List<Tunnel> _tunnels = new List<Tunnel>();        
        public static LogWriter _logWriter;
        public static string plinkLocation;
        public static Log logger = Log.Instance;

        public Toast()
        {
            try
            {
                _logWriter = new LogWriter();
                logger.Filename = "toastytunnel.log";
                logger.Path = @"logs\";

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

            logger.Add(Levels.INFO, "Found " + settings.Identities.Count() + " identities, and " + settings.Tunnels.Count() + " sessions.");
            foreach (Identity i in settings.Identities)
                logger.Add(Levels.INFO, "Found identity: " + i.Name);
            foreach (Tunnel t in settings.Tunnels)
                logger.Add(Levels.INFO,, "Found tunnel: " + t.Name);
        }
    }
}