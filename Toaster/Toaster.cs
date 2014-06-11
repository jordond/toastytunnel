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

        public Toaster()
        {
            _identities = new Identity().loadIdentities();
            _sessions = new Session().loadSessions();
        }
    }
}
