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
    public partial class AddControllerForm : Form
    {
        ControllerManager manager;
        public AddControllerForm(ControllerManager m)
        {
            InitializeComponent();
            manager = m;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            manager.Add(new Models.ControllerInfo()
            {
                ConfigurationClass = ConfigurationComboBox.Text,
                Type = (Models.ControllerType)Enum.Parse(typeof(Models.ControllerType), TypeComboBox.Text),
                ControllerClass = ControllerComboBox.Text,
                FriendlyName = FriendlyNameTextBox.Text
            });
            Close();
        }
    }
}
