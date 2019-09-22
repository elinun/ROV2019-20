using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROV2019.Views
{
    public partial class SensorsDialog : Form
    {
        Thread updateThread;
        int centerRoll = 0;
        int centerPitch = 0;
        public SensorsDialog(ConnectionContext connection)
        {
            InitializeComponent();
            Accelerations.StartPolling(connection);
            updateThread = new Thread(UpdateInstruments);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        private void UpdateInstruments()
        {
            while (true)
            {
                Invoke(new MethodInvoker(delegate
                {
                    double pitch = ((Accelerations.AccelerationValues.AcZ-centerPitch) / 4096.0)*90.0;
                    double roll = (-(Accelerations.AccelerationValues.AcY-centerRoll) / 4096.0)*90.0;
                    attitudeIndicatorInstrumentControl1.SetAttitudeIndicatorParameters(pitch, roll);
                    InternalTemperatureLabel.Text = Accelerations.AccelerationValues.Temp.ToString();
                }));
                Thread.Sleep(50);
            }
        }

        private void SensorsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Accelerations.StopPolling();
            updateThread.Abort();
        }

        private void TareButton_Click(object sender, EventArgs e)
        {
            centerPitch = Accelerations.AccelerationValues.AcZ;
            centerRoll = Accelerations.AccelerationValues.AcY;
        }
    }
}
