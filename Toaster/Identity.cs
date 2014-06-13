using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Identity
    {
        public int ID { get; set; } //delete
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string PrivateKey { get; set; }
        public bool Save { get; set; }

        public Identity() { }

        public Identity(string user, string password, string pKey, bool save)
        {
            User = user;
            Password = password;
            PrivateKey = pKey;
            Save = save;
        }
    }
}
