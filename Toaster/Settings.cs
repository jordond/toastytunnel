﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;

using Logger;

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

        public bool plinkExists()
        {
            return File.Exists(Plink);
        }
    }

    public class State<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.conf";

        public void Save(string filename = DEFAULT_FILENAME)
        {
            try
            {
                string temp = new JavaScriptSerializer().Serialize(this);
                byte[] bytes = new byte[temp.Length * sizeof(char)];
                System.Buffer.BlockCopy(temp.ToCharArray(), 0, bytes, 0, bytes.Length);

                File.WriteAllText(filename, Convert.ToBase64String(bytes));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Save(T pSettings, string filename = DEFAULT_FILENAME)
        {
            try
            {
                string temp = new JavaScriptSerializer().Serialize(pSettings);
                byte[] bytes = new byte[temp.Length * sizeof(char)];
                System.Buffer.BlockCopy(temp.ToCharArray(), 0, bytes, 0, bytes.Length);

                File.WriteAllText(filename, Convert.ToBase64String(bytes));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            if (File.Exists(fileName))
            {
                byte[] bytes = Convert.FromBase64String(File.ReadAllText(fileName));
                char[] chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);


                t = (new JavaScriptSerializer()).Deserialize<T>(new string(chars));
            }
            return t;
        }
    }
}
