using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Identity
    {
        public int ID { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Save { get; set; }

        public Identity() { }

        public Identity(int id, string user, string password, bool save)
        {
            ID = id;
            User = user;
            Password = password;
            Save = save;
        }

        public List<Identity> loadIdentities()
        {
            return null;
        }
    }
}
