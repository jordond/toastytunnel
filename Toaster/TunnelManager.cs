using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toaster
{
    public class TunnelManager
    {
        public List<Tunnel> Tunnels { get; set; }

        public TunnelManager(List<Tunnel> t)
        {
            Tunnels = t;
        }

        public List<Tunnel> All
        {
            get
            {
                return Tunnels;
            }
        }

        public List<Tunnel> Open
        {
            get
            {
                var o = new List<Tunnel>();
                foreach (Tunnel t in Tunnels.Where(i => i.isOpen == true))
                    o.Add(t);
                return o;
            }
        }

        public void Add(Tunnel t)
        {
            Tunnels.Add(t);
        }

        
        #region Tunnel Start Methods
        //autostart 
        public void Start()
        {
            try
            {
                foreach (Tunnel t in Tunnels.Where(a => a.autoStart == true))
                    t.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Start() - " + ex.Message);
            }
        }

        //start single
        public void Start(int tunnelID)
        {
            try
            {
                Tunnels.First(t => t.ID == tunnelID).Start();
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Start(int) - " + ex.Message);
            }
        }

        //start multiple
        public void Start(List<int> tunnelIDs)
        {
            try
            {
                foreach (int i in tunnelIDs)
                {
                    Tunnels.First(t => t.ID == i).Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Start(List<int>) - " + ex.Message);
            }
        }
        #endregion

        #region Tunnel Stop Methods
        //stop all
        public void Stop()
        {
            try
            {
                foreach (Tunnel t in Tunnels)
                    t.Stop();
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Stop() - " + ex.Message);
            }
        }

        //stop single
        public void Stop(int tunnelID)
        {
            try
            {
                Tunnels.First(t => t.ID == tunnelID).Stop();
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Stop(int) - " + ex.Message);
            }
        }

        //stop multiple
        public void Stop(List<int> tunnelIDs)
        {
            try
            {
                foreach (int i in tunnelIDs)
                {
                    Tunnels.First(t => t.ID == i).Stop();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TunnelManager.cs Stop(List<int>) - " + ex.Message);
            }
        }
        #endregion
    }
}
