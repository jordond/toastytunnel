using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Toaster
{
    public class Information
    {
        private const string savedData = "files\\config";        
        public List<Session> Sessions { get; set; }
        public List<Identity> Identities { get; set; }
        private List<string> _data;

        public Information()
        {
            if (!File.Exists(savedData))
                File.Create(savedData);

            _data = loadData();

            if (_data != null)
                sortData();
        }

        public void saveData()
        {
            try
            {
                if (File.Exists(savedData))
                    File.Delete(savedData);

                File.WriteAllLines(savedData, _data.ToArray());

            }
            catch (Exception ex)
            {
                throw new Exception("Information.cs - saveData() " + ex.Message);
            }
        }

        private List<string> loadData()
        {
            try
            {
                string[] data = File.ReadAllLines(savedData);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Information.cs - loadData() " + ex.Message);
            }
        }

        private void sortData()
        {

        }
    }
}
