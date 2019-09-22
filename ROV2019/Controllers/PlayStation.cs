using ROV2019.Models;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV2019.Controllers
{
    public class PlayStation : Controller
    {
        Joystick Joystick;
        public PlayStation(DirectInput input, Guid guid)
        {
            Joystick = new Joystick(input, guid);
            SlimDX.Result acquireSuccess = Joystick.Acquire();
            if (acquireSuccess.IsFailure)
                throw new Exception("Failed to Acquire PlayStation Controller.");
        }

        public override void Poll()
        {
            JoystickState state = Joystick.GetCurrentState();
            //Straight forward for PS Controllers
            //Just convert to -400 to 400 scale.
            X = (state.X/82)-400;
            Y = (state.Y/82)-400;
            Z = (state.Z/82)-400;
            RotationX = (state.RotationX / 163);
            RotationY = (state.RotationY / 163);
            RotationZ = (state.RotationZ / 82)-400;
            PointOfViewControllers = state.GetPointOfViewControllers();
            Buttons = state.GetButtons();
                        
            //add more as we need more
        }
    }
}
