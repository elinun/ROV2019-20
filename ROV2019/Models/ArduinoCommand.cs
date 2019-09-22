using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.Models
{
    public class ArduinoCommand
    {
        public string Command { get; set; }
        public List<byte[]> Parameters = new List<byte[]>();
        public int NumberOfReturnedBytes { get; set; }

        public void AddParameter(string value)
        {
            Parameters.Add(GetBytes(value));
        }

        public void AddParameter(int value)
        {
            /*
            byte[] intBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] result = intBytes;
            Parameters.Add(result);
            */
            Parameters.Add(GetBytes(value.ToString()));
        }
        public static byte[] GetBytes(string s)
        {
            return Encoding.ASCII.GetBytes(s?? string.Empty);
        }
        public static string GetString(byte[] bytes)
        {
            string str = Encoding.ASCII.GetString(bytes);
            return str.Replace("\0", "");
        }
        
        private List<byte> addAllBytes(List<byte> byteList, byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                byteList.Add(b);
            }
            return byteList;
        }
        public byte[] GetCommandBytes()
        {
            List<byte> command = new List<byte>();
            //add first part of the command
            string cmd = ("{" + Command + ":");
            byte[] stringPart = GetBytes(cmd);
            command = addAllBytes(command, stringPart);
            //add parameters
            foreach(byte[] bytes in Parameters)
            {
                command = addAllBytes(command, bytes);
                command = addAllBytes(command, GetBytes(","));
            }
            /*if(Parameters.Count>0)
                command.RemoveAt(command.Count - 1);*/
            //add bytes to return
            //cmd = ":" + NumberOfReturnedBytes + "}";
            cmd = "}";
            command = addAllBytes(command, GetBytes(cmd));
            return command.ToArray();
        }
    }
    
    public class Command
    {
        public static readonly string Authorize = "authorize";
        public static readonly string SetThruster = "SetThruster";
        public static readonly string AnalogRead = "analogRead";
        public static readonly string GetName = "GetName";
        [Obsolete]
        public static readonly string VerticalStabilize = "VerticalStabilize";
        public static readonly string GetAccelerations = "GetAccelerations";
        internal static readonly string SetServoSpeed = "setServoSpeed";
        internal static readonly string DigitalWrite = "digitalWrite";
    }

    public enum Thrusters
    {
        Left = 0,
        Right = 1,
        [Obsolete]
        BackLeft = 2,
        [Obsolete]
        BackRight = 3,
        //these will only be used when we have two vertical thrusters
        [Obsolete]
        VerticalLeft = 4,
        [Obsolete]
        VerticalRight = 5,
        //these four will only be used in thruster configurations with for vertical thrusters
        VerticalFrontLeft = 6,
        VerticalFrontRight = 7,
        VerticalBackLeft = 8,
        VerticalBackRight = 9,
        FrontLeft = 10,
        FrontRight = 11
    }

    public enum Servos
    {
        ClawOpen = 0,
        ClawRotate = 1
    }

    public enum Accessories
    {
        //correspond to pin numbers
        Laser = 15,
        MicroPropeller = 14,
        TetherWinder = 16
    }
}
