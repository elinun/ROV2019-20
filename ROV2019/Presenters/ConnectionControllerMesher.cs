using ROV2019.ControllerConfigurations;
using ROV2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ROV2019.Presenters
{
    public class ConnectionControllerMesher
    {
        ConnectionContext conn;
        ControllerConfiguration config;
        Thread pollThread;
        int PollInterval;
        public bool IsMeshing = false;
        public bool IsUsingPID = true;

        public ConnectionControllerMesher(ConnectionContext connection, ControllerConfiguration configuration, int PollRate = 100)
        {
            conn = connection;
            config = configuration;
            PollInterval = PollRate;
        }

        public void StartMesh()
        {
            if (!IsMeshing)
            {
                IsMeshing = true;
                pollThread = new Thread(Poll);
                pollThread.Start();
            }
        }

        private void Poll()
        {
            while (IsMeshing)
            {
                ConfiguredPollData data = config.Poll();
                if (!IsUsingPID)
                    conn.MoveAndAddTrim(data.ThrusterSpeeds);
                else
                    conn.VerticalStabilize(data.ThrusterSpeeds);

                //Servos
                int? openSpeed;
                int? rotateSpeed;
                if((openSpeed = Utilities.TryGet((int)Servos.ClawOpen, data.ServoSpeeds)) != null)
                {
                    conn.SetServoSpeed(Servos.ClawOpen, (int)openSpeed);
                }

                if ((rotateSpeed = Utilities.TryGet((int)Servos.ClawRotate, data.ServoSpeeds)) != null)
                    conn.SetServoSpeed(Servos.ClawRotate, (int)rotateSpeed);

                //Accessories
                bool? GoGoOn;
                bool? WinderOn;
                bool? LaserOn;
                if ((GoGoOn = Utilities.TryGet((int)Accessories.MicroPropeller, data.Accessories)) != null)
                    conn.DigitalWrite((int)Accessories.MicroPropeller, (bool)GoGoOn);

                if ((WinderOn = Utilities.TryGet((int)Accessories.TetherWinder, data.Accessories)) != null)
                    conn.DigitalWrite((int)Accessories.TetherWinder, (bool)WinderOn);
                if ((LaserOn = Utilities.TryGet((int)Accessories.Laser, data.Accessories)) != null)
                    conn.DigitalWrite((int)Accessories.Laser, (bool)LaserOn);

                Thread.Sleep(PollInterval);
            }
        }

        public void StopMesh()
        {
            IsMeshing = false;
            if(pollThread != null)
                pollThread.Abort() ;
            pollThread = null;
        }
    }
}
