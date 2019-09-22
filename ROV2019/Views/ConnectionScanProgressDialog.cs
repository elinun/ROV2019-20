using ROV2019.Models;
using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROV2019.Views
{
    public partial class ConnectionScanProgressDialog : Form
    {
        Progress<(ArduinoConnection, int)> progressTracker;
        Task<List<ArduinoConnection>> scanTask;

        public ConnectionScanProgressDialog(ConnectionManager connectionManager, IProgress<ArduinoConnection> connectionFound)
        {
            InitializeComponent();

            progressTracker = new Progress<(ArduinoConnection, int)>(progress =>
            {
                scanProgress.Value = progress.Item2;
                if (progress.Item1 != null)
                    connectionFound.Report(progress.Item1);
                if (progress.Item2 == 100)
                    this.Close();
            });
            scanTask = connectionManager.Scan(progressTracker);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
