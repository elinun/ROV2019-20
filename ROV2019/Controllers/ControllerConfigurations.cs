using ROV2019.Models;
using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.ControllerConfigurations
{
    public abstract class ControllerConfiguration
    {
        //It makes sense for a controller configuration to work with a certain thruster layout.
        //See ConnectionClass class.
        public string Layout;
        public abstract ConfiguredPollData Poll();
        //not sure if this will be necessary.
        public ControllerConfiguration(Controller controller)
        {

        }
    }

    public class Tank : ControllerConfiguration
    {
        public Tank(Controller c) : base(c)
        {

        }
        public override ConfiguredPollData Poll()
        {
            throw new NotImplementedException();
        }
    }

    public class Arcade : ControllerConfiguration
    {
        Controller controller;
        bool canVerticalMove = true;
        bool previousUp = false;
        bool previousDown = false;
        int rollSpeed = 0;
        public Arcade(Controller c) : base(c)
        {
            controller = c;
            Layout = ThrusterLayout.TL1;
        }
        public override ConfiguredPollData Poll()
        {
            //Poll controller, and interpret that into the vectors, etc.
            controller.Poll();

            //map ROV movements to controlls on controller

            //try to prevent swapping directions on the thrusters,
            int verticalSpeed = 0;
            if (canVerticalMove)
            {
                verticalSpeed = (controller.RotationX > 0 ? -controller.RotationX : controller.RotationY);
            }
            canVerticalMove = !(previousUp && previousDown);
            previousDown = controller.Buttons[6];
            previousUp = controller.Buttons[7];

            rollSpeed -= (controller.Buttons[4] && rollSpeed > -250 ? 1 : 0);
            rollSpeed += (controller.Buttons[5] && rollSpeed < 250 ? 1 : 0);

            Dictionary<Thrusters, int> thrusterSpeeds = new Dictionary<Thrusters, int>();

            /*The thrusters do not produce the same amount of thrust 
             *when they go forward as when they go backwards. This means that
             * when we move side to side we have to slow down the forward
             * thrusters. The variable below is how much to slow it down.
             * I currently have it set according to bluerobotic's documentation.
             * Max Backwards thrust/Max Forward thrust
             */
            double CorrectionFactor = 0.78846153846;
            //calculate the power to send to each thruster.
            int FLPwr = 1500;
            int FRPwr = 1500;
            int BLPwr = 1500;
            int BRPwr = 1500;

            //forward vector
            FLPwr += controller.X;
            FRPwr += controller.X;
            BLPwr += controller.X;
            BRPwr += controller.X;

            //lateral Vector
            if (controller.Y > 0)
            {
                //right
                FLPwr += (int)(controller.Y * CorrectionFactor);
                FRPwr -= controller.Y;
                BLPwr -= controller.Y;
                BRPwr += (int)(controller.Y * CorrectionFactor);
            }
            else if (controller.Y < 0)
            {
                //left
                //remember, adding a negative
                FLPwr += controller.Y;
                FRPwr -= (int)(controller.Y * CorrectionFactor);
                BLPwr -= (int)(controller.Y * CorrectionFactor);
                BRPwr += controller.Y;
            }

            //rotation
            if (controller.Z > 0)
            {
                //clockwise
                FLPwr += (int)(controller.Y * CorrectionFactor);
                FRPwr -= controller.Y;
                BLPwr += (int)(controller.Y * CorrectionFactor);
                BRPwr -= controller.Y;
            }
            else if (controller.Z < 0)
            {
                //counter-clockwise, or anti-clockwise if ur British
                FLPwr -= controller.Y;
                FRPwr += (int)(controller.Y * CorrectionFactor);
                BLPwr -= controller.Y;
                BRPwr += (int)(controller.Y * CorrectionFactor);
            }

            //vertical
            //calculate the power to send to each thruster.
            int leftPower = 1500 + verticalSpeed;
            int rightPower = 1500 + verticalSpeed;

            leftPower += rollSpeed;
            rightPower -= rollSpeed;

            thrusterSpeeds.Add(Thrusters.FrontLeft, FLPwr);
            thrusterSpeeds.Add(Thrusters.FrontRight, FRPwr);
            thrusterSpeeds.Add(Thrusters.BackRight, BRPwr);
            thrusterSpeeds.Add(Thrusters.BackLeft, BLPwr);
            thrusterSpeeds.Add(Thrusters.VerticalLeft, leftPower);
            thrusterSpeeds.Add(Thrusters.VerticalRight, rightPower);

            ConfiguredPollData data = new ConfiguredPollData()
            {
                ThrusterSpeeds = thrusterSpeeds,
                ServoSpeeds = new Dictionary<int, int?>()
            };
            return data;
        }
    }

    public class Helicopter : ControllerConfiguration
    {
        Dictionary<Thrusters, int> prevVals = new Dictionary<Thrusters, int>()
        {
            {Thrusters.Left, 1500 },
            {Thrusters.Right, 1500 },
            {Thrusters.VerticalBackLeft, 1500 },
            { Thrusters.VerticalBackRight, 1500 },
            {Thrusters.VerticalFrontLeft, 1500 },
            {Thrusters.VerticalFrontRight, 1500 }
        };
        long prevTime = DateTime.Now.Ticks;
        //Units of microseconds pulse/elapsed milliseconds
        readonly double maxRateOfChange = 0.25;

        int prevClawOpenSpeed = 0;
        int prevClawRotateSpeed = 0;

        bool goGoMotorOn = false;
        bool tetherWinderOn = false;

        Controller controller;
        public Helicopter(Controller c) : base(c)
        {
            controller = c;
            //Configuration for when we have 4 vertical thrusters.
            Layout = ThrusterLayout.TL2;
        }

        public override ConfiguredPollData Poll()
        {
            //TODO: Implement configuration
            controller.Poll();
            //TODO: Add logic to move straight forward when tilted (pitched).
            int L = 1500 + controller.X + controller.Y;
            int R = 1500 + controller.X - controller.Y;
            //int VFL = 1500 + controller.RotationZ + controller.Z;
            //int VFR = 1500 + controller.RotationZ - controller.Z;
            //int VBL = 1500 - controller.RotationZ + controller.Z;
            //int VBR = 1500 - controller.RotationZ - controller.Z;

            int VFL = 1500+controller.RotationY - controller.RotationX;
            int VFR = 1500+controller.RotationY - controller.RotationX;
            int VBL = 1500+controller.RotationY - controller.RotationX;
            int VBR = 1500+controller.RotationY - controller.RotationX;

            L = L > 0 ? Math.Min(L, 1650) : Math.Max(1350, L);
            R = R > 0 ? Math.Min(R, 1650) : Math.Max(1350, R);
            VFL = VFL > 0 ? Math.Min(VFL, 1650) : Math.Max(1350, VFL);
            VBL = VBL > 0 ? Math.Min(VBL, 1650) : Math.Max(1350, VBL);
            VBR = VBR > 0 ? Math.Min(VBR, 1650) : Math.Max(1350, VBR);
            VFR = VFR > 0 ? Math.Min(VFR, 1650) : Math.Max(1350, VFR);

            //Optional after we add the separate regulator for arduino
            //CheckRateOfChange(ref L, ref R, ref VFL, ref VFR, ref VBL, ref VBR);

            Dictionary<Thrusters, int> thrusterSpeeds = new Dictionary<Thrusters, int>();
            thrusterSpeeds.Add(Thrusters.Left, L);
            thrusterSpeeds.Add(Thrusters.Right, R);
            thrusterSpeeds.Add(Thrusters.VerticalFrontLeft, VFL);
            thrusterSpeeds.Add(Thrusters.VerticalFrontRight, VFR);
            thrusterSpeeds.Add(Thrusters.VerticalBackLeft, VBL);
            thrusterSpeeds.Add(Thrusters.VerticalBackRight, VBR);

            //Servos and Micro
            Dictionary<int, bool?> accessories = new Dictionary<int, bool?>();
            ///VERY VERY IMPORTANT!!!
            ///THE MOTOR AND WINDER CAN NOT BE ON AT THE SAME TIME!!!
            ///THE VOLTAGE REGULATOR WILL GET SHORTED IF THEY ARE!!!
            ///Oh, and there is nothing in the Arduino code to stop this
            if (!tetherWinderOn)
            {
                if (controller.Buttons[5])
                {
                    if (!goGoMotorOn)
                    {
                        accessories[(int)Accessories.MicroPropeller] = !true;
                        goGoMotorOn = true;
                    }

                }
                else if (goGoMotorOn)
                {
                    accessories[(int)Accessories.MicroPropeller] = !false;
                    goGoMotorOn = false;
                }
            }

            ///VERY VERY IMPORTANT!!!
            ///THE MOTOR AND WINDER CAN NOT BE ON AT THE SAME TIME!!!
            ///THE VOLTAGE REGULATOR WILL GET SHORTED IF THEY ARE!!!
            if (!goGoMotorOn)
            {

                if (controller.Buttons[4])
                {
                    if (!tetherWinderOn)
                    {
                        accessories[(int)Accessories.TetherWinder] = !true;
                        tetherWinderOn = true;
                    }
                }
                else if (tetherWinderOn)
                {
                    accessories[(int)Accessories.TetherWinder] = !false;
                    tetherWinderOn = false;
                }
            }

            Dictionary<int, int?> servoSpeeds = new Dictionary<int, int?>();
            if(controller.Buttons[1])
            {
                if (prevClawOpenSpeed >= 0)
                {
                    servoSpeeds[(int)Servos.ClawOpen] = -1;
                    prevClawOpenSpeed = -1;
                }
                    
            }
            else if(prevClawOpenSpeed<0)
            {
                servoSpeeds[(int)Servos.ClawOpen] = 0;
                prevClawOpenSpeed = 0;
            }

            if (controller.Buttons[3])
            {
                if (prevClawOpenSpeed <= 0)
                {
                    servoSpeeds[(int)Servos.ClawOpen] = 1;
                    prevClawOpenSpeed = 1;
                }
            }
            else if (prevClawOpenSpeed > 0)
            {
                servoSpeeds[(int)Servos.ClawOpen] = 0;
                prevClawOpenSpeed = 0;
            }

            if (controller.Buttons[0])
            {
                if (prevClawRotateSpeed >= 0)
                {
                    servoSpeeds[(int)Servos.ClawRotate] = -1;
                    prevClawRotateSpeed = -1;
                }
            }
            else if (prevClawRotateSpeed < 0)
            {
                servoSpeeds[(int)Servos.ClawRotate] = 0;
                prevClawRotateSpeed = 0;
            }

            if (controller.Buttons[2])
            {
                if (prevClawRotateSpeed <= 0)
                {
                    servoSpeeds[(int)Servos.ClawRotate] = 1;
                    prevClawRotateSpeed = 1;
                }
            }
            else if (prevClawRotateSpeed > 0)
            {
                servoSpeeds[(int)Servos.ClawRotate] = 0;
                prevClawRotateSpeed = 0;
            }

            ConfiguredPollData data = new ConfiguredPollData()
            {
                ThrusterSpeeds = thrusterSpeeds,
                Accessories = accessories,
                ServoSpeeds = servoSpeeds
            };
            return data;
        }

        //Makes sure that the thrusters are not speeding up too fast in order to prevent voltage drop.
        private void CheckRateOfChange(ref int Left, ref int Right, ref int vfl, ref int vfr, ref int vbl, ref int vbr)
        {
            int elapsedMs = (int)(DateTime.Now.Ticks - prevTime);
            int prevLeft = prevVals[Thrusters.Left];
            Left = (int)(((Left - prevLeft) / elapsedMs <= maxRateOfChange) ? Left : prevLeft + (maxRateOfChange*elapsedMs));
            prevVals[Thrusters.Left] = Left;

            int prevRight = prevVals[Thrusters.Right];
            Right = (int)(((Right - prevRight) / elapsedMs <= maxRateOfChange) ? Right : prevRight + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.Right] = Right;

            int prevvfl = prevVals[Thrusters.VerticalFrontLeft];
            vfl = (int)(((vfl - prevvfl) / elapsedMs <= maxRateOfChange) ? vfl : prevvfl + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalFrontLeft] = vfl;

            int prevvfr = prevVals[Thrusters.VerticalFrontRight];
            vfr = (int)(((vfr - prevvfr) / elapsedMs <= maxRateOfChange) ? vfr : prevvfr + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalFrontRight] = vfr;

            int prevvbl = prevVals[Thrusters.VerticalBackLeft];
            vbl = (int)(((vbl - prevvbl) / elapsedMs <= maxRateOfChange) ? vbl : prevvbl + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalBackLeft] = vbl;

            int prevvbr = prevVals[Thrusters.VerticalBackRight];
            vbr = (int)(((vbr - prevvbr) / elapsedMs <= maxRateOfChange) ? vbr : prevvbr + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalBackRight] = vbr;

            prevTime = DateTime.Now.Ticks;
        }
    }

    public class Amelie : ControllerConfiguration
    {
        Dictionary<Thrusters, int> prevVals = new Dictionary<Thrusters, int>()
        {
            {Thrusters.Left, 1500 },
            {Thrusters.Right, 1500 },
            {Thrusters.VerticalBackLeft, 1500 },
            { Thrusters.VerticalBackRight, 1500 },
            {Thrusters.VerticalFrontLeft, 1500 },
            {Thrusters.VerticalFrontRight, 1500 }
        };
        long prevTime = DateTime.Now.Ticks;
        //Units of microseconds pulse/elapsed milliseconds
        readonly double maxRateOfChange = 0.25;

        int prevClawOpenSpeed = 0;
        int prevClawRotateSpeed = 0;

        bool goGoMotorOn = false;
        bool tetherWinderOn = false;
        bool laserOn = !true;
        bool canChangeLaserState = false;

        Controller controller;
        public Amelie(Controller c) : base(c)
        {
            controller = c;
            //Configuration for when we have 4 vertical thrusters.
            Layout = ThrusterLayout.TL2;
        }

        public override ConfiguredPollData Poll()
        {
            //TODO: Implement configuration
            controller.Poll();
            //TODO: Add logic to move straight forward when tilted (pitched).
            int fwdBack = controller.RotationY > 10 ? controller.RotationY : -controller.RotationX;
            int L = 1500 + fwdBack + controller.X;
            int R = 1500 + fwdBack - controller.X;
            //int VFL = 1500 + controller.RotationZ + controller.Z;
            //int VFR = 1500 + controller.RotationZ - controller.Z;
            //int VBL = 1500 - controller.RotationZ + controller.Z;
            //int VBR = 1500 - controller.RotationZ - controller.Z;

            int VFL = 1500;// + controller.RotationZ;// - controller.RotationX;
            int VFR = 1500 + controller.RotationZ;// - controller.RotationX;
            int VBL = 1500 + controller.RotationZ;// - controller.RotationX;
            int VBR = 1500;// + controller.RotationZ;// - controller.RotationX;

            L = L > 0 ? Math.Min(L, 1600) : Math.Max(1400, L);
            R = R > 0 ? Math.Min(R, 1600) : Math.Max(1400, R);
            VFL = VFL > 0 ? Math.Min(VFL, 1600) : Math.Max(1400, VFL);
            VBL = VBL > 0 ? Math.Min(VBL, 1600) : Math.Max(1400, VBL);
            VBR = VBR > 0 ? Math.Min(VBR, 1600) : Math.Max(1400, VBR);
            VFR = VFR > 0 ? Math.Min(VFR, 1600) : Math.Max(1400, VFR);

            //Optional after we add the separate regulator for arduino
            //CheckRateOfChange(ref L, ref R, ref VFL, ref VFR, ref VBL, ref VBR);

            Dictionary<Thrusters, int> thrusterSpeeds = new Dictionary<Thrusters, int>();
            thrusterSpeeds.Add(Thrusters.Left, L);
            thrusterSpeeds.Add(Thrusters.Right, R);
            //thrusterSpeeds.Add(Thrusters.VerticalFrontLeft, VFL);
            thrusterSpeeds.Add(Thrusters.VerticalFrontRight, VFR);
            thrusterSpeeds.Add(Thrusters.VerticalBackLeft, VBL);
            //thrusterSpeeds.Add(Thrusters.VerticalBackRight, VBR);

            //Servos and Micro
            Dictionary<int, bool?> accessories = new Dictionary<int, bool?>();

            if(controller.Buttons[13])
            {
                if (canChangeLaserState)
                {
                    accessories[(int)Accessories.Laser] = !laserOn;
                    laserOn = !laserOn;
                }
                canChangeLaserState = false;
            }
            else
            {
                canChangeLaserState = true;
            }
            ///VERY VERY IMPORTANT!!!
            ///THE MOTOR AND WINDER CAN NOT BE ON AT THE SAME TIME!!!
            ///THE VOLTAGE REGULATOR WILL GET SHORTED IF THEY ARE!!!
            ///Oh, and there is nothing in the Arduino code to stop this
            if (!tetherWinderOn)
            {
                if (controller.Buttons[5])
                {
                    if (!goGoMotorOn)
                    {
                        accessories[(int)Accessories.MicroPropeller] = !true;
                        goGoMotorOn = true;
                    }

                }
                else if (goGoMotorOn)
                {
                    accessories[(int)Accessories.MicroPropeller] = !false;
                    goGoMotorOn = false;
                }
            }

            ///VERY VERY IMPORTANT!!!
            ///THE MOTOR AND WINDER CAN NOT BE ON AT THE SAME TIME!!!
            ///THE VOLTAGE REGULATOR WILL GET SHORTED IF THEY ARE!!!
            if (!goGoMotorOn)
            {

                if (controller.Buttons[4])
                {
                    if (!tetherWinderOn)
                    {
                        accessories[(int)Accessories.TetherWinder] = !true;
                        tetherWinderOn = true;
                    }
                }
                else if (tetherWinderOn)
                {
                    accessories[(int)Accessories.TetherWinder] = !false;
                    tetherWinderOn = false;
                }
            }

            Dictionary<int, int?> servoSpeeds = new Dictionary<int, int?>();
            if (controller.Buttons[1])
            {
                if (prevClawOpenSpeed >= 0)
                {
                    servoSpeeds[(int)Servos.ClawOpen] = -1;
                    prevClawOpenSpeed = -1;
                }

            }
            else if (prevClawOpenSpeed < 0)
            {
                servoSpeeds[(int)Servos.ClawOpen] = 0;
                prevClawOpenSpeed = 0;
            }

            if (controller.Buttons[3])
            {
                if (prevClawOpenSpeed <= 0)
                {
                    servoSpeeds[(int)Servos.ClawOpen] = 1;
                    prevClawOpenSpeed = 1;
                }
            }
            else if (prevClawOpenSpeed > 0)
            {
                servoSpeeds[(int)Servos.ClawOpen] = 0;
                prevClawOpenSpeed = 0;
            }

            if (controller.Buttons[0])
            {
                if (prevClawRotateSpeed >= 0)
                {
                    servoSpeeds[(int)Servos.ClawRotate] = -1;
                    prevClawRotateSpeed = -1;
                }
            }
            else if (prevClawRotateSpeed < 0)
            {
                servoSpeeds[(int)Servos.ClawRotate] = 0;
                prevClawRotateSpeed = 0;
            }

            if (controller.Buttons[2])
            {
                if (prevClawRotateSpeed <= 0)
                {
                    servoSpeeds[(int)Servos.ClawRotate] = 1;
                    prevClawRotateSpeed = 1;
                }
            }
            else if (prevClawRotateSpeed > 0)
            {
                servoSpeeds[(int)Servos.ClawRotate] = 0;
                prevClawRotateSpeed = 0;
            }

            ConfiguredPollData data = new ConfiguredPollData()
            {
                ThrusterSpeeds = thrusterSpeeds,
                Accessories = accessories,
                ServoSpeeds = servoSpeeds
            };
            return data;
        }

        //Makes sure that the thrusters are not speeding up too fast in order to prevent voltage drop.
        private void CheckRateOfChange(ref int Left, ref int Right, ref int vfl, ref int vfr, ref int vbl, ref int vbr)
        {
            int elapsedMs = (int)(DateTime.Now.Ticks - prevTime);
            int prevLeft = prevVals[Thrusters.Left];
            Left = (int)(((Left - prevLeft) / elapsedMs <= maxRateOfChange) ? Left : prevLeft + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.Left] = Left;

            int prevRight = prevVals[Thrusters.Right];
            Right = (int)(((Right - prevRight) / elapsedMs <= maxRateOfChange) ? Right : prevRight + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.Right] = Right;

            int prevvfl = prevVals[Thrusters.VerticalFrontLeft];
            vfl = (int)(((vfl - prevvfl) / elapsedMs <= maxRateOfChange) ? vfl : prevvfl + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalFrontLeft] = vfl;

            int prevvfr = prevVals[Thrusters.VerticalFrontRight];
            vfr = (int)(((vfr - prevvfr) / elapsedMs <= maxRateOfChange) ? vfr : prevvfr + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalFrontRight] = vfr;

            int prevvbl = prevVals[Thrusters.VerticalBackLeft];
            vbl = (int)(((vbl - prevvbl) / elapsedMs <= maxRateOfChange) ? vbl : prevvbl + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalBackLeft] = vbl;

            int prevvbr = prevVals[Thrusters.VerticalBackRight];
            vbr = (int)(((vbr - prevvbr) / elapsedMs <= maxRateOfChange) ? vbr : prevvbr + (maxRateOfChange * elapsedMs));
            prevVals[Thrusters.VerticalBackRight] = vbr;

            prevTime = DateTime.Now.Ticks;
        }
    }
}
