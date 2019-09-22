using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.Models
{
    public class ArduinoConnections
    {
        public List<ArduinoConnection> connections { get; set; }
    }

    public class ArduinoConnection
    {

        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string FriendlyName { get; set; }
        public int Latency { get; set; }
        //See ConnectionClass class
        public string ConnectionClass { get; set; }
        public Trim Trim { get; set; } = new Trim();
    }

    public class Trim
    {
        //How much it drifts left to right when moving forward
        public int LeftToRightCorrection { get; set; }
        //How much it drifts front to back when moving side to side
        public int FrontToBackCorrection { get; set; }
        public int RollCorrection { get; set; }
        public int PitchCorrection { get; set; }
        public Dictionary<Thrusters, bool> InvertedThrusters = new Dictionary<Thrusters, bool>();
    }
}
