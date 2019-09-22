using Microsoft.VisualStudio.TestTools.UnitTesting;
using ROV2019.Models;
using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019Tests
{
    [TestClass]
    public class ConnectionPresenters
    {

        [TestMethod]
        public void VectorCalculation()
        {
           // ConnectionContext.MoveVectors(400, -400, 0);
        }

        /*[TestMethod]
        public void ConnectionManager_Scan()
        {
            ConnectionManager manager = new ConnectionManager();
            int previousProgress = 0;
            List<ArduinoConnection> results = manager.Scan(new Progress<(ArduinoConnection, int)>(progress =>
            {
                Assert.IsTrue(progress >= previousProgress);
                previousProgress = progress;
            })).Result;
            Assert.AreEqual(results.Count, 1);
            Debug.WriteLine(results[0].IpAddress);
        }*/

    }
}
