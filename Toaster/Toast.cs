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
        public TunnelManager tunnels;
        public Log logger = Log.Instance;

        private static Toast instance;
        public static Toast Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Toast();
                }
                return instance;
            }
        }

        public Toast()
        {
            try
            {
                loadSettings();
                tunnels = new TunnelManager(settings.Tunnels);
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

        public void saveSettings()
        {
            try
            {
                List<Identity> temp = new List<Identity>();
                foreach (Identity i in settings.Identities)
                {
                    if (i.Save == true)
                    {
                        temp.Add(i);
                        logger.Add(Levels.INFO, "Saving identity: " + i.Name);
                    }
                }
                settings.Identities = temp;
                foreach (Tunnel t in tunnels.All)
                    logger.Add(Levels.INFO, "Saving tunnel: " + t.Name);
                settings.Tunnels = tunnels.All;
                settings.Save();
            }
            catch (Exception ex)
            {
                logger.Add(Levels.ERROR, "Toast - saveSettings(): " + ex.Message);
            }
        }

        public void loadSettings()
        {
            settings = Settings.Load();

            logger.Add(Levels.INFO, "Found " + settings.Identities.Count() + " identities, and " + settings.Tunnels.Count() + " sessions.");
            foreach (Identity i in settings.Identities)
                logger.Add(Levels.INFO, "Found identity: " + i.Name);
            foreach (Tunnel t in settings.Tunnels)
                logger.Add(Levels.INFO, "Found tunnel: " + t.Name);
        }
    }
}