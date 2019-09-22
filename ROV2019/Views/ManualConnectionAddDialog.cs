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
    public partial class ManualConnectionAddDialog : Form
    {

        ConnectionManager connectionManager;
        bool isLocked = true;
        public ManualConnectionAddDialog(ConnectionManager connectionManager)
        {
            InitializeComponent();

            this.connectionManager = connectionManager;
            this.AcceptButton = SaveButton;
            ShowDialog();
        }

        private void ShowPasswordButton_MouseDown(object sender, MouseEventArgs e)
        {
            PasswordField.UseSystemPasswordChar = PasswordPropertyTextAttribute.No.Password;
        }

        private void ShowPasswordButton_MouseUp(object sender, MouseEventArgs e)
        {
            PasswordField.UseSystemPasswordChar = PasswordPropertyTextAttribute.Yes.Password;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ArduinoConnection conn = new ArduinoConnection()
            {
                FriendlyName = nameField.Text,
                IpAddress = IPField.Text,
                Port = (int)PortField.Value,
                Password = PasswordField.Text,
                ConnectionClass = ThrusterLayout.TL2
            };
            connectionManager.Add(conn);
            Close();
        }
    }
}
