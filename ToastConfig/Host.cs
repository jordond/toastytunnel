using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ToastConfig
{
    public class Host
    {
        private const string hostsFile = "files\\hosts.dat";
        public List<string> Hosts { get; set; }

        public Host()
        {
            if (File.Exists(hostsFile))
                Hosts = loadAllHosts();            
        }

        private List<string> loadAllHosts()
        {
            List<string> theHosts = new List<string>();
            try
            {
                string[] tempHosts = File.ReadAllLines(hostsFile);
                theHosts = tempHosts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return theHosts;
        }

        public void saveHosts()
        {
            string[] hostsToWrite = Hosts.ToArray();

            try
            {
                if (File.Exists(hostsFile))
                    File.Delete(hostsFile);

                File.WriteAllLines(hostsFile, hostsToWrite);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
