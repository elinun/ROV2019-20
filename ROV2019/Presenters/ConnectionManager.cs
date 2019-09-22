using ROV2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.Presenters
{
    public class ConnectionManager
    {
        public List<ArduinoConnection> SavedConnections { get; private set; }

        public ConnectionManager()
        {
            //populate connections
            if(Properties.Settings.Default.SavedConnections == null)
            {
                Properties.Settings.Default.SavedConnections = new ArduinoConnections()
                {
                    connections = new List<ArduinoConnection>()
                };
            }
            SavedConnections = Properties.Settings.Default.SavedConnections.connections;
        }

        public ConnectionContext GetConnectionContext(ArduinoConnection connection)
        {
            return (ConnectionContext)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("ROV2019.Presenters." + connection.ConnectionClass, true, System.Reflection.BindingFlags.CreateInstance, null, new object[] { connection }, null, null);
        }

        public async Task<List<ArduinoConnection>> Scan(IProgress<(ArduinoConnection, int)> progress)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                string myIP = "";
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        myIP = ip.ToString();
                    }
                }
                string ipsToScan = myIP.Substring(0, myIP.LastIndexOf('.')+1);
                return await Scan(ipsToScan, progress);
            }
            else
            {
                return null;
            }
        }

        //firstThreeBytes: First three bytes of IP address. "192.168.1" will scan for arduinos
        //with IP from 192.168.1.0-192.168.1.255.
        //save: Wheather the discovered arduinos should be saved
        private async Task<List<ArduinoConnection>> Scan(string firstThreeBytes, IProgress<(ArduinoConnection, int)> progress, int port = 1740, bool save=true)
        {
            List<ArduinoConnection> discoveredConnections = new List<ArduinoConnection>();
            await Task.Run(() =>
            {
                for (int lastByte = 0; lastByte < 256; lastByte++)
                {
                    ArduinoConnection connectionProperties = new ArduinoConnection()
                    {
                        Port = port,
                        IpAddress = firstThreeBytes + lastByte,
                        ConnectionClass = ThrusterLayout.TL2
                    };

                    ConnectionContext connection = GetConnectionContext(connectionProperties);
                    if (connection.OpenConnection(100))
                    {
                        connectionProperties.FriendlyName = connection.GetName();
                        discoveredConnections.Add(connectionProperties);
                        if (save)
                            Add(connectionProperties);
                        progress.Report((connectionProperties, (int)(((double)lastByte / 255.0) * 100)));
                    }
                    connection.Close();
                    progress.Report((null, (int)(((double)lastByte / 255.0)*100)));
                }
            });
            
            return discoveredConnections;
        }
        public void Save(ArduinoConnection connection)
        {
            ArduinoConnection oldConToReplace = SavedConnections.FirstOrDefault(c => c.FriendlyName == connection.FriendlyName || c.IpAddress == connection.IpAddress);
            SavedConnections.Remove(oldConToReplace);
            SavedConnections.Add(connection);
            Properties.Settings.Default.SavedConnections.connections = SavedConnections;
            Properties.Settings.Default.Save();
        }
        public void Add(ArduinoConnection connection)
        {
            this.SavedConnections.Add(connection);
            Properties.Settings.Default.SavedConnections.connections = this.SavedConnections;
            Properties.Settings.Default.Save();
        }

        public void Remove(ArduinoConnection connection)
        {
            this.SavedConnections.Remove(connection);
            Properties.Settings.Default.SavedConnections.connections = this.SavedConnections;
            Properties.Settings.Default.Save();
        }
    }
}
