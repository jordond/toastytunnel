﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class Toast
    {
        private Dictionary<int,Identity> _identities;
        private Dictionary<int,Session> _sessions;
        private List<Tunnel> _tunnels;
        public static LogWriter _logWriter;
        public static Information _data;

        public const string plinkLocation = @"files\plink.exe"; 

        public Toast()
        {
            try
            {
                _logWriter = new LogWriter();
                _data = new Information();
                _identities = _data.Identities;
                _sessions = _data.Sessions;

                debug();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public void debug()
        {
            //_logWriter.addEntry(LogLevels.ERROR, "This is a test");
            //_data.saveData();
            Tunnel temp = new Tunnel();
            temp.ID = _tunnels.Count() + 1;
            //temp.Name = _sessions.
        }
    }
}