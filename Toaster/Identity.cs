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

        public string PasswordDesc
        {
            get
            {
                if (string.IsNullOrEmpty(Password))
                    return "NO";
                else
                    return "YES"; 
            }
        }
        public string PrivateKeyDesc
        {
            get
	        {
		        if (string.IsNullOrEmpty(PrivateKey))
                    return "NO";
                else
                    return System.IO.Path.GetFileName(PrivateKey); 
	        }
        }
    }
}