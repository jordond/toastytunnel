using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;

namespace Toaster
{
    public class Settings : State<Settings>
    {
        public List<Tunnel> Tunnels { get; set; }
        public List<Identity> Identities { get; set; }
        public string Plink { get; set; }

        public Settings()
        {
            Tunnels = new List<Tunnel>();
            Identities = new List<Identity>();
        }
        
        public void saveSettings()
        {
            foreach (Identity i in this.Identities)
                Toast._logWriter.addEntry(LogLevels.INFO, "Saving identity: " + i.Name);
            foreach (Tunnel t in this.Tunnels)
                Toast._logWriter.addEntry(LogLevels.INFO, "Saving tunnel: " + t.Name);

            this.Save();
        }
    }

    public class State<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.conf";

        public void Save(string filename = DEFAULT_FILENAME)
        {
            string temp = new JavaScriptSerializer().Serialize(this);
            byte[] bytes = new byte[temp.Length * sizeof(char)];
            System.Buffer.BlockCopy(temp.ToCharArray(), 0, bytes, 0, bytes.Length);
            
            File.WriteAllText(filename, Convert.ToBase64String(bytes));
        }

        public static void Save(T pSettings, string filename = DEFAULT_FILENAME)
        {
            File.WriteAllText(filename, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            byte[] bytes = Convert.FromBase64String(File.ReadAllText(fileName));
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            string temp = new string(chars);

            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(temp);
            return t;
        }
    }
}
