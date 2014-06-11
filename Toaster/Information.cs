﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Toaster
{
    public class Information
    {
        private const string savedData = "files\\data";        
        public List<Session> Sessions { get; set; }
        public List<Identity> Identities { get; set; }
        private List<string> _data;

        public Information()
        {
            if (File.Exists(savedData))
            {
                _data = loadData();
                sortData();
            }
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

        private bool sortData()
        {
            try
            {
                for (int i = 0; i < _data.Count(); ++i)
                {
                    if (_data[i] == "%IDENTITY%")
                    {
                        Identity temp = new Identity();
                        temp.ID = int.Parse(_data[i + 1]);
                        temp.User = _data[i + 2];
                        temp.Password = _data[i + 3];
                        temp.PrivateKey = _data[i + 4];
                        Identities.Add(temp);

                    }
                    else if (_data[i] == "%SESSION%")
                    {
                        Session temp = new Session();
                        temp.ID = int.Parse(_data[i + 1]);
                        temp.identity = Identities.Where(t => t.ID == int.Parse(_data[i + 2])).First();
                        temp.Host = _data[i + 3];
                        temp.IsLocal = bool.Parse(_data[i + 4]);
                        temp.LocalPort = temp.IsLocal == false ? 0 : int.Parse(_data[i + 5]);
                        temp.RemoteAddress = temp.IsLocal == false ? "" : _data[i + 6];
                        temp.RemotePort = int.Parse(_data[i + 7]);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
