using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Toaster
{
    public class Tunnel
    {
        public Process Instance { get; set; }
        public ProcessStartInfo InstanceInfo { get; set; }
        public string ConnectionString { get; set; }

        public static Information _data;

        public Tunnel()
        {
            _data = new Information();
        }
    }
}
