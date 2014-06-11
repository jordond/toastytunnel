using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Toaster
{
    public class Identity
    {
        public int ID { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Path PrivateKey { get; set; }
        public bool Save { get; set; }

        public Identity() { }

        public Identity(string user, string password, Path pKey, bool save)
        {
            User = user;
            Password = password;
            PrivateKey = pKey;
            Save = save;
        }

        public List<Identity> loadIdentities()
        {
            return null;
        }
    }
}
