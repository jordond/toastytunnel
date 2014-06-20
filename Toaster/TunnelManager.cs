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

        /// <summary>
        /// Default tunnel manager constructor 
        /// </summary>
        /// <param name="t">List of tunnels to be added</param>
        public TunnelManager(List<Tunnel> t)
        {
            Tunnels = t;
        }

        /// <summary>
        /// Returns a list of all of the tunnels contained in the tunnel manager
        /// </summary>
        public List<Tunnel> All
        {
            get
            {
                return Tunnels;
            }
        }

        /// <summary>
        /// Returns all tunnels that are currently open
        /// </summary>
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

        /// <summary>
        /// Add a tunnel to the tunnel manager.
        /// </summary>
        /// <param name="t">Tunnel object</param>
        public void Add(Tunnel t)
        {
            Tunnels.Add(t);
        }

        public Tunnel Find(int id)
        {
            return (Tunnel)Tunnels.Where(t => t.ID == id);
        }
                
        #region Tunnel Start Methods
        /// <summary>
        /// Default start method for tunnels, it will start all tunnels with 
        /// auto start flag set to true.
        /// </summary>
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

        /// <summary>
        /// Will start a single tunnel with the matching passed
        /// through ID.
        /// </summary>
        /// <param name="tunnelID">ID of tunnel</param>
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

        /// <summary>
        /// Attmepts to start multiple tunnels, based on the passed
        /// through list of tunnel ids.
        /// </summary>
        /// <param name="tunnelIDs">IDs of tunnels to be started.</param>
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
        /// <summary>
        /// Will attempt to stop all of the tunnels that have been stored.
        /// </summary>
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

        /// <summary>
        /// Stops a single tunnel
        /// </summary>
        /// <param name="tunnelID">ID of tunnel</param>
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

        /// <summary>
        /// Stops multiple tunnels, based on the passed in list of
        /// tunnels.
        /// </summary>
        /// <param name="tunnelIDs">ID's of tunnels to be stopped</param>
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
