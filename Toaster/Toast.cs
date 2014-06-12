using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Toast
    {
        private Dictionary<int,Identity> _identities;
        private Dictionary<int,Session> _sessions;
        private List<Tunnel> _tunnels = new List<Tunnel>();
        public static LogWriter _logWriter;
        public static Information _data;

        public const string plinkLocation = @"files\plink.exe"; 

        public Toast()
        {
            try
            {
                _logWriter = new LogWriter();
                _data = new Information();
                _identities = _data.Identities;
                _sessions = _data.Sessions;
                _tunnels = new List<Tunnel>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void debugCreate()
        {
            foreach (Session s in _sessions.Values)
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
    }
}