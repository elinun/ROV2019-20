using Microsoft.VisualStudio.TestTools.UnitTesting;
using ROV2019.Models;
using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ROV2019Tests
{
    [TestClass]
    public class Thrusters
    {
        static ConnectionContext connection;
        [ClassInitialize]
        public static void TestClassinitialize(TestContext context)
        {
            var manager = new ConnectionManager();
            var connectionInfo = new ArduinoConnection()
            {
                IpAddress = "192.168.1.146",
                Port = 1740,
                Password = "password",
                ConnectionClass = "ThrusterLayout2Connection"
            };
            connection = manager.GetConnectionContext(connectionInfo);
            connection.OpenConnection();
            connection.Authorize();
        }

        [TestMethod]
        public void RunLeft_Thruster()
        {
            connection.SetThruster(ROV2019.Models.Thrusters.Right, 1800);
            connection.SetThruster(ROV2019.Models.Thrusters.Left, 1800);
            Thread.Sleep(1000);
            connection.Stop();
            //System.Diagnostics.Debug.WriteLine(connection.GetAccelerations().Temp);
            connection.Close();
        }
    }
}
