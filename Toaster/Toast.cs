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

                debug();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void debug()
        {
            //_logWriter.addEntry(LogLevels.ERROR, "This is a test");
            //_data.saveData();
            Tunnel temp = new Tunnel(_sessions[1].ConnectionString);
            temp.ID = _tunnels.Count() + 1;
            temp.createTunnelName(_sessions[1]);
            temp.start();
            _tunnels.Add(temp);
        }
    }
}