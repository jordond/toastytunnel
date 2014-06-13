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
        private Settings settings;
        public static LogWriter _logWriter;

        public const string plinkLocation = @"files\plink.exe"; 

        public Toast()
        {
            try
            {
                _logWriter = new LogWriter();
                loadSettings();
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
            _identities = settings.Identities;
            _sessions = settings.Sessions;

            Toast._logWriter.addEntry(LogLevels.INFO, "Found " + _identities.Count() + " identities, and " + _sessions.Count() + " sessions.");
            foreach (Identity i in _identities)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found identity: " + i.Name);
            foreach (Session s in _sessions)
                Toast._logWriter.addEntry(LogLevels.INFO, "Found session: " + s.Name);
        }
    }
}