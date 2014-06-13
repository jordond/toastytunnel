using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Toaster
{
    public class Session
    {
        public string Name { get; set; }
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
                StringBuilder cs = new StringBuilder();
                cs.Append("-ssh ");
                if (File.Exists(identity.PrivateKey))
                    cs.Append("-i " + identity.PrivateKey + " ");
                if (IsLocal)
                    cs.Append("-L " + LocalPort + ":" + RemoteAddress + ":" + RemotePort + " ");
                else
                    cs.Append("-D " + RemotePort + " ");
                cs.Append(identity.User + "@" + Host + " ");
                if (!File.Exists(identity.PrivateKey) || identity.Password != "")
                    cs.Append("-pw " + identity.Password);

                return cs.ToString();
            }
            set{}
        }
    }
}
