using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Toaster
    {
        private List<Identity> _identities;
        private List<Session> _sessions;
        private List<Tunnel> _tunnels;
        public static LogWriter _logWriter;
        public static Information _data;

        public Toaster()
        {
            _logWriter = new LogWriter();
            _data = new Information();
            _identities = _data.Identities;
            _sessions = _data.Sessions;
        }
    }
}