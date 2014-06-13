using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Toast
    {
        private List<Identity> _identities = new List<Identity>();
        private List<Session> _sessions = new List<Session>();
        private List<Tunnel> _tunnels = new List<Tunnel>();
        public Settings settings;
        public static LogWriter _logWriter;

        public const string plinkLocation = @"files\plink.exe"; 

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
            foreach (Session s in _sessions)
            {
                Tunnel temp = new Tunnel(s);
                temp.ID = _tunnels.Count() + 1;
                temp.start();
                _tunnels.Add(temp);
            }
        }
        public void debugKill()
        {
            foreach(Tunnel t in _tunnels)
            {
                t.stop();
            }
        }

        private void loadSettings()
        {
            settings = Settings.Load();

            Toast._logWriter.addEntry(LogLevels.INFO, "Found " + settings.Identities.Count() + " identities, and " + settings.Sessions.Count() + " sessions.");
            foreach (Identity i in settings.Identities)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found identity: " + i.Name);
            foreach (Session s in settings.Sessions)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found session: " + s.Name);
        }
    }
}