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
        //no longer needed
        private const string savedData = "files\\data";        
        public Dictionary<int, Session> Sessions { get; set; }
        public Dictionary<int, Identity> Identities { get; set; }
        public bool? dataFileIsGood { get; set; }
        public List<string> _data { get; set; }

        public Information()
        {
            Sessions = new Dictionary<int, Session>();
            Identities = new Dictionary<int, Identity>();
            _data = new List<string>();

            if (File.Exists(savedData))
            {
                _data = loadDataFromFile();
                Toast._logWriter.addEntry(LogLevels.INFO, "Read in " + _data.Count() + " lines from the data file.");
                dataFileIsGood = sortData();

                if (dataFileIsGood == false)
                    Toast._logWriter.addEntry(LogLevels.WARNING, "Error reading in data file, using blank file instead");
                else
                    Toast._logWriter.addEntry(LogLevels.INFO, "Successfully read in " + Identities.Count() + " identities, and " + Sessions.Count() + " sessions.");
            }
            else
            {
                dataFileIsGood = null;
                File.Create(savedData).Dispose();
                Toast._logWriter.addEntry(LogLevels.WARNING, "Data file didn't exist and was created, missing or first run?");
            }
        }

        public void saveData()
        {
            try
            {
                if (File.Exists(savedData))
                {
                    File.Delete(savedData);
                    Toast._logWriter.addEntry(LogLevels.INFO, "Information.cs - SaveData(): Deleting previous data file before saving.");
                }

                loadDataFromLists();
                File.WriteAllLines(savedData, _data.ToArray());
                Toast._logWriter.addEntry(LogLevels.INFO, "Infomation.cs - SaveData(): Writing " + _data.Count() + " lines to data file.");

            }
            catch (Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Information.cs - SaveData() " + ex.Message);
                throw new Exception("Information.cs - saveData() " + ex.Message);
            }
        }       

        private List<string> loadDataFromFile()
        {
            try
            {
                string[] data = File.ReadAllLines(savedData);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Infomation.cs - loadData() " + ex.Message);
                throw new Exception("Information.cs - loadData() " + ex.Message);
            }
        }

        private void loadDataFromLists()
        {
            _data.Clear();

            foreach (Identity ident in Identities.Values)
            {
                if(ident.Save)
                {
                    _data.Add("%IDENTITY%");
                    _data.Add(ident.ID.ToString());
                    _data.Add(ident.User);
                    _data.Add(ident.Password);
                    _data.Add(ident.PrivateKey);
                }
            }

            foreach (Session s in Sessions.Values)
            {
                _data.Add("%SESSION%");
                _data.Add(s.ID.ToString());
                _data.Add(s.identity.ID.ToString());
                _data.Add(s.Host);
                _data.Add(s.IsLocal.ToString());
                _data.Add(s.LocalPort.ToString());
                _data.Add(s.RemoteAddress);
                _data.Add(s.RemotePort.ToString());
            }
        }

        private bool sortData()
        {
            try
            {
                for (int i = 0; i < _data.Count(); ++i)
                {
                    if (_data[i] == "%IDENTITY%")
                    {
                        Identity temp = new Identity();
                        temp.ID = int.Parse(_data[i+=1]);
                        temp.User = _data[i += 1];
                        temp.Password = _data[i += 1];
                        temp.PrivateKey = _data[i += 1];
                        Identities.Add(temp.ID, temp);
                        Toast._logWriter.addEntry(LogLevels.INFO, "Found identity: " + temp.User);

                    }
                    else if (_data[i] == "%SESSION%")
                    {
                        Session temp = new Session();
                        temp.ID = int.Parse(_data[i += 1]);
                        temp.identity = Identities[int.Parse(_data[i += 1])];
                        temp.Host = _data[i += 1];
                        temp.IsLocal = bool.Parse(_data[i += 1]);
                        temp.LocalPort = int.Parse(_data[i += 1]);
                        temp.RemoteAddress = _data[i += 1];
                        temp.RemotePort = int.Parse(_data[i += 1]);
                        Sessions.Add(temp.ID, temp);
                        Toast._logWriter.addEntry(LogLevels.INFO, "Found session: " + temp.identity.User + "@" + temp.Host);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Toast._logWriter.addEntry(LogLevels.ERROR, "Information.cs - SortData() - " + ex.Message); 
                return false;
            }
        }
    }
}
