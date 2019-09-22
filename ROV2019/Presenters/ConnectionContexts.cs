using ROV2019.ControllerConfigurations;
using ROV2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ROV2019.Presenters
{
    public class Accelerations
    {
        //This will minimize how often we poll the MPU chip.
        public static (int AcX, int AcY, int AcZ, int Temp, int GyX, int GyY, int GyZ) AccelerationValues;
        private static ConnectionContext connection;
        private static int pollDelay = 50;
        private static bool IsPolling;

        private static void Poll()
        {
            while(IsPolling)
            {
                AccelerationValues = connection.GetAccelerations();
                Thread.Sleep(pollDelay);
            }
        }

        public static void StartPolling(ConnectionContext ctx)
        {
            connection = ctx;
            if(!IsPolling)
            {
                IsPolling = true;
                Thread pollThread = new Thread(Poll);
                pollThread.IsBackground = true;
                pollThread.Start();
            }
        }

        public static void StopPolling()
        {
            IsPolling = false;
        }
    }


    //see Thruster Layout google drawing files in Documentation folder
    //https://drive.google.com/drive/u/1/folders/1kAhB2g80rjM9Hb7dXbg3HS3KL6_p3RJn
    //actual class name of Layout Classes.
    public class ThrusterLayout
    {
        public static readonly string TL1 = "ThrusterLayout1Connection";
        public static readonly string TL2 = "ThrusterLayout2Connection";
    }

    public abstract class ConnectionContext
    {
        public ArduinoConnection connection;
        protected TcpClient client;
        protected NetworkStream stream;

        protected ConnectionContext(ArduinoConnection connection)
        {
            this.connection = connection;
        }
        public void Close()
        {
            try
            {
                stream.Close();
                client.Close();
            }
            catch (Exception) { }
        }

        public bool OpenConnection(int timeout = 1000)
        {
            try
            {
                client = new TcpClient();
                var result = client.BeginConnect(connection.IpAddress, connection.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));

                if (!success)
                {
                    return false;
                }

                stream = client.GetStream();
                //stream.ReadTimeout = timeout;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool isConnected()
        {
            try
            {
                //stream.Write(ArduinoCommand.GetBytes(" "), 0, 1);
                return true;
            }
            catch (Exception) { return false; }
        }

        public (int AcX, int AcY, int AcZ, int Temp, int GyX, int GyY, int GyZ) GetAccelerations()
        {
            try
            {
                ArduinoCommand cmd = new ArduinoCommand()
                {
                    Command = Command.GetAccelerations
                };
                byte[] toWrite = cmd.GetCommandBytes();
                stream.Write(toWrite, 0, toWrite.Length);
                char c;
                StringBuilder sb = new StringBuilder();
                while ((c = (char)stream.ReadByte()) != '}')
                {
                    if (c != '{')
                        sb.Append(c);
                }
                string str = sb.ToString();
                string[] vals = str.Split(';');
                int X = int.Parse(vals[0].Substring(2));
                int Y = int.Parse(vals[1].Substring(2));
                int Z = int.Parse(vals[2].Substring(2));
                int temp = int.Parse(vals[3].Substring(2));
                int GyX = int.Parse(vals[4].Substring(2));
                int GyY = int.Parse(vals[5].Substring(2));
                int GyZ = int.Parse(vals[6].Substring(2));
                return (X, Y, Z, temp, GyX, GyY, GyZ);
            }
            catch (Exception) { return (0, 0, 0, 0, 0, 0, 0); }
        }

        public bool SetThruster(Thrusters thruster, int value)
        {
            try
            {
                ArduinoCommand command = new ArduinoCommand()
                {
                    Command = Command.SetThruster,
                    NumberOfReturnedBytes = 0
                };
                command.AddParameter((int)thruster);
                command.AddParameter(value);
                {
                    byte[] toWrite = command.GetCommandBytes();
                    stream.Write(toWrite, 0, toWrite.Length);
                    return true;
                }
            }
            catch (Exception) { return false; }
        }

        public string GetName()
        {
            if (isConnected())
            {
                try
                {
                    ArduinoCommand command = new ArduinoCommand()
                    {
                        Command = Command.GetName,
                        NumberOfReturnedBytes = 16
                    };
                    byte[] toWrite = command.GetCommandBytes();
                    stream.Write(toWrite, 0, toWrite.Length);
                    byte[] toRead = new byte[command.NumberOfReturnedBytes];
                    stream.Read(toRead, 0, toRead.Length);
                    return ArduinoCommand.GetString(toRead);
                }
                catch (Exception)
                {
                    return "Failed To Get Name";
                }
            }
            return "Failed to Get Name";
        }

        //Sends the authorize command to arduino. Should be the first command sent, besides GetName.
        public bool Authorize()
        {
            try
            {
                ArduinoCommand command = new ArduinoCommand()
                {
                    Command = Command.Authorize,
                    NumberOfReturnedBytes = 1
                };
                command.Parameters.Add(ArduinoCommand.GetBytes(connection.Password));
                byte[] toWrite = command.GetCommandBytes();
                stream.Write(toWrite, 0, toWrite.Length);
                byte[] toRead = new byte[1];
                stream.Read(toRead, 0, toRead.Length);
                return (toRead[0] == 0x01);
            }
            catch (Exception) { return false; }
        }

        public bool SetServoSpeed(Servos servo, int speed)
        {
            ArduinoCommand cmd = new ArduinoCommand()
            {
                Command = Command.SetServoSpeed,
                NumberOfReturnedBytes = 0
            };
            cmd.AddParameter((int)servo);
            cmd.AddParameter(speed);
            try
            {
                byte[] toWrite = cmd.GetCommandBytes();
                stream.Write(toWrite, 0, toWrite.Length);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //pin will not correspond to pin number for analog input pins being used
        //for digitalWrite
        public bool DigitalWrite(int pin, bool on)
        {
            try
            {
                ArduinoCommand cmd = new ArduinoCommand()
                {
                    Command = Command.DigitalWrite,
                    NumberOfReturnedBytes = 0
                };
                cmd.AddParameter(pin);
                cmd.AddParameter(on ? 1: 0);
                byte[] toWrite = cmd.GetCommandBytes();
                stream.Write(toWrite, 0, toWrite.Length);
                return true;
            }
            catch (Exception) { return false; }
        }

        public abstract void Stop();
        public abstract void MoveAndAddTrim(Dictionary<Thrusters, int> speeds);
        //Add logic for PID here. Is Same as MoveAndAddTrim, but auto adjusts with PID
        public abstract void VerticalStabilize(Dictionary<Thrusters, int> speeds);
    }

    //See thruster layout google drawings in the documentation folder. 
    //https://drive.google.com/drive/u/1/folders/1kAhB2g80rjM9Hb7dXbg3HS3KL6_p3RJn
    public class ThrusterLayout1Connection : ConnectionContext
    {

        public ThrusterLayout1Connection(ArduinoConnection connection) : base(connection)
        { }

        public override void Stop()
        {
            if (isConnected())
            {
                for (int i = 0; i < 6; i++)
                {
                    SetThruster((Thrusters)Enum.Parse(typeof(Thrusters), i.ToString()), 1500);

                }    
            }
        }

        public void MoveAndAddTrim(int VL, int VR, int FL, int FR, int BL, int BR)
        {


            //Trim
            VR += connection.Trim.RollCorrection;
            VL -= connection.Trim.RollCorrection;
                                                                     
            VL = (VL > 1500 ? Math.Min(VL, 1900) : Math.Max(VL, 1100));
            VR = (VR > 1500 ? Math.Min(VR, 1900) : Math.Max(VR, 1100));

            SetThruster(Thrusters.VerticalLeft, VL);
            SetThruster(Thrusters.VerticalRight, VR);
            MoveAndAddTrim(FR, FL, BL, BR);
        }

        //Adds the trim associated with this connection and sends command to turn on the thrusters
        //Parameters are the microsecond pulse lengths generated from ControllerConfig.
        public void MoveAndAddTrim(int FLPwr, int FRPwr, int BLPwr, int BRPwr)
        {        

            //Add trim. May need to add correction factor
            FLPwr -= connection.Trim.LeftToRightCorrection;
            FLPwr -= connection.Trim.FrontToBackCorrection;

            FRPwr += connection.Trim.LeftToRightCorrection;
            FRPwr += connection.Trim.FrontToBackCorrection;

            BLPwr -= connection.Trim.LeftToRightCorrection;
            BLPwr -= connection.Trim.FrontToBackCorrection;

            BRPwr += connection.Trim.LeftToRightCorrection;
            BRPwr += connection.Trim.FrontToBackCorrection;

            FLPwr = (FLPwr > 1500 ? Math.Min(FLPwr, 1900) : Math.Max(FLPwr, 1100));
            FRPwr = (FRPwr > 1500 ? Math.Min(FRPwr, 1900) : Math.Max(FRPwr, 1100));
            BLPwr = (BLPwr > 1500 ? Math.Min(BLPwr, 1900) : Math.Max(BLPwr, 1100));
            BRPwr = (BRPwr > 1500 ? Math.Min(BRPwr, 1900) : Math.Max(BRPwr, 1100));

            //send commands
            SetThruster(Thrusters.FrontLeft, FLPwr);
            SetThruster(Thrusters.FrontRight, FRPwr);
            SetThruster(Thrusters.BackLeft, BLPwr);
            SetThruster(Thrusters.BackRight, BRPwr);
        }

        [Obsolete("PID logic is no longer ran OnBoard")]
        public bool VerticalStabilize(int verticalLeftPwr, int verticalRightPwr)
        {
            try
            {
                ArduinoCommand command = new ArduinoCommand()
                {
                    Command = Command.VerticalStabilize,
                    NumberOfReturnedBytes = 0
                };
                int verticalSpeed = ((verticalLeftPwr + verticalRightPwr) / 2) - 1500;
                int rollPos = (verticalLeftPwr - verticalRightPwr) / 2;
                command.AddParameter(verticalSpeed);
                command.AddParameter(rollPos);
                byte[] toWrite = command.GetCommandBytes();
                stream.Write(toWrite, 0, toWrite.Length);
                return true;
            }
            catch (Exception) { return false; }
        }

        public override void MoveAndAddTrim(Dictionary<Thrusters, int> speeds)
        {
            throw new NotImplementedException();
        }

        public override void VerticalStabilize(Dictionary<Thrusters, int> speeds)
        {
            throw new NotImplementedException();
        }

    }

    public class ThrusterLayout2Connection : ConnectionContext
    {

        public ThrusterLayout2Connection(ArduinoConnection connection) : base(connection)
        { }

        public override void MoveAndAddTrim(Dictionary<Thrusters, int> speeds)
        {
            try
            {
                int L = speeds.FirstOrDefault(x => x.Key == Thrusters.Left).Value;
                int R = speeds.FirstOrDefault(x => x.Key == Thrusters.Right).Value;
                int VFL = speeds.FirstOrDefault(x => x.Key == Thrusters.VerticalFrontLeft).Value;
                int VFR = speeds.FirstOrDefault(x => x.Key == Thrusters.VerticalFrontRight).Value;
                int VBL = speeds.FirstOrDefault(x => x.Key == Thrusters.VerticalBackLeft).Value;
                int VBR = speeds.FirstOrDefault(x => x.Key == Thrusters.VerticalBackRight).Value;

                L -= connection.Trim.LeftToRightCorrection;
                R += connection.Trim.FrontToBackCorrection;

                //Check for inversions
                Dictionary<Thrusters, bool> inversions = connection.Trim.InvertedThrusters;
                L = Utilities.TryGet(Thrusters.Left, inversions) ? 3000 - L : L;
                R = Utilities.TryGet(Thrusters.Right, inversions) ? 3000 - R : R;
                VFL = Utilities.TryGet(Thrusters.VerticalFrontLeft, inversions) ? 3000 - VFL : VFL;
                VBL = Utilities.TryGet(Thrusters.VerticalBackLeft, inversions) ? 3000 - VBL : VBL;
                VBR = Utilities.TryGet(Thrusters.VerticalBackRight, inversions) ? 3000 - VBR : VBR;
                VFR = Utilities.TryGet(Thrusters.VerticalFrontRight, inversions) ? 3000 - VFR : VFR;

                //Make sure we don't go over or under the signal range
                L = (L > 1500 ? Math.Min(L, 1900) : Math.Max(1100, L));
                R = (R > 1500 ? Math.Min(R, 1900) : Math.Max(1100, R));
                VFL = (VFL > 1500 ? Math.Min(VFL, 1900) : Math.Max(1100, VFL));
                VFR = (VFR > 1500 ? Math.Min(VFR, 1900) : Math.Max(1100, VFR));
                VBL = (VBL > 1500 ? Math.Min(VBL, 1900) : Math.Max(1100, VBL));
                VBR = (VBR > 1500 ? Math.Min(VBR, 1900) : Math.Max(1100, VBR));

                SetThruster(Thrusters.Left, L);
                SetThruster(Thrusters.Right, R);
                SetThruster(Thrusters.VerticalFrontLeft, VFL);
                SetThruster(Thrusters.VerticalFrontRight, VFR);
                SetThruster(Thrusters.VerticalBackLeft, VBL);
                SetThruster(Thrusters.VerticalBackRight, VBR);

            }
            catch(Exception e)
            {
                //throw new Exception("Thruster Speed Is (most likely) Missing.", e);
            }

        }

        
        public override void Stop()
        {
            SetThruster(Thrusters.Left, 1500);
            SetThruster(Thrusters.Right, 1500);
            SetThruster(Thrusters.VerticalFrontLeft, 1500);
            SetThruster(Thrusters.VerticalFrontRight, 1500);
            SetThruster(Thrusters.VerticalBackLeft, 1500);
            SetThruster(Thrusters.VerticalBackRight, 1500);
        }

        public override void VerticalStabilize(Dictionary<Thrusters, int> speeds)
        {
            //throw new NotImplementedException();
            MoveAndAddTrim(speeds);
        }

    }
}
