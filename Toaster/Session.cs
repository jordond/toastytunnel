using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Session
    {
        public int ID { get; set; }
        public Identity identity { get; set; }
        public string Host { get; set; }
        public bool IsLocal { get; set; }
        public int LocalPort { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }

        public Session() { }

        public string ConnectionString
        {
            get
            {
                return "";
            }
        }

        public List<Session> loadSessions()
        {
            return null;
        }
    }
}
