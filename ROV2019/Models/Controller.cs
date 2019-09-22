
using SlimDX.DirectInput;
using System.Collections.Generic;

namespace ROV2019.Models
{
    //combines controller and state
    public abstract class Controller
    {
        public abstract void Poll();

        //copied from SlimDX.DirectInput.JoyStickState
        //
        // Summary:
        //     Gets the Z-axis, often the throttle control.
        public int Z { get; set;}
        //
        // Summary:
        //     Gets the X-axis rotation.
        public int RotationX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis rotation.
        public int RotationY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis rotation.
        public int RotationZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis velocity.
        public int VelocityX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis velocity.
        public int VelocityY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis velocity.
        public int VelocityZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis angular velocity.
        public int AngularVelocityX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis angular velocity.
        public int AngularVelocityY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis angular velocity.
        public int AngularVelocityZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis acceleration.
        public int AccelerationX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis acceleration.
        public int AccelerationY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis acceleration.
        public int AccelerationZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis angular acceleration.
        public int AngularAccelerationX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis, usually the forward-backward movement of a stick.
        public int Y { get; set;}
        //
        // Summary:
        //     Gets the Y-axis angular acceleration.
        public int AngularAccelerationY { get; set;}
        //
        // Summary:
        //     Gets the X-axis force.
        public int ForceX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis force.
        public int ForceY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis force.
        public int ForceZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis torque.
        public int TorqueX { get; set;}
        //
        // Summary:
        //     Gets the Y-axis torque.
        public int TorqueY { get; set;}
        //
        // Summary:
        //     Gets the Z-axis torque.
        public int TorqueZ { get; set;}
        //
        // Summary:
        //     Gets the Z-axis angular acceleration.
        public int AngularAccelerationZ { get; set;}
        //
        // Summary:
        //     Gets the X-axis, usually the left-right movement of a stick.
        public int X { get; set;}

        //
        // Summary:
        //     Gets the acceleration of each slider on the joystick.
        public int[] AccelerationSliders { get; set; }
        //
        // Summary:
        //     Gets the state of each button on the joystick.
        public bool[] Buttons { get; set;}
        //
        // Summary:
        //     Gets the force of each slider on the joystick.
        public int[] ForceSliders { get; set;}
        //
        // Summary:
        //     Gets the state of each point-of-view controller on the joystick.
        public int[] PointOfViewControllers { get; set;}
        //
        // Summary:
        //     Gets the position of each slider on the joystick.
        public int[] Sliders { get; set;}
        //
        // Summary:
        //     Gets the velocity of each slider on the joystick.
        public int[] VelocitySliders { get; set;}
    }
    
    public class ConfiguredPollData
    {
        [System.Obsolete("Use ThrusterSpeeds instead")]
        public (int forwardSpeed, int lateralSpeed, int rotationalSpeed, int verticalSpeed, int rollSpeed) Vectors { get; set; }
        public Dictionary<Thrusters, int> ThrusterSpeeds { get; set; }
        //for ServoSpeeds and accessories, null will indicate no change
        public Dictionary<int, int?> ServoSpeeds { get; set; }
        public Dictionary<int, bool?> Accessories { get; set; }
    }
}
