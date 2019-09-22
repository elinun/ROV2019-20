using ROV2019.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROV2019.Views
{
    public class ConnectionListItem : GroupBox
    {
        public bool Selected = false;
        public ConnectionListItem(ArduinoConnection con, EventHandler OnRemove, EventHandler OnTest, EventHandler OnSelect)
        {
            this.Tag = con;
            Name = con.FriendlyName + con.IpAddress;
            this.Click += OnSelect;
            Cursor = Cursors.Hand;
            //Name Label
            Label name = new Label()
            {
                Text = con.FriendlyName,
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(name);
            //IP Label
            Label ip = new Label()
            {
                Text = con.IpAddress + ":" + con.Port,
                Location = new Point(10,30),
                AutoSize = true
            };
            Controls.Add(ip);
            //Remove Button
            Button removeButton = new Button()
            {
                Text = "Remove",
                Tag = con,
                Location = new Point(75, 45)
            };
            removeButton.Click += OnRemove;
            this.Controls.Add(removeButton);
        }

        public bool ToggleSelected()
        {
            if (Selected)
                this.BackColor = Color.Gray;
            else
                this.BackColor = Color.Yellow;
            return (Selected = !Selected);
        }

    }

    public class ControllerListItem : GroupBox
    {
        public bool Selected = false;
        public ControllerListItem(ControllerInfo con, EventHandler OnRemove, EventHandler OnTest, EventHandler OnSelect)
        {
            Tag = con;
            Click += OnSelect;
            Cursor = Cursors.Hand;
            //Name Label
            Label name = new Label()
            {
                Text = con.FriendlyName,
                Location = new Point(10, 10),
                AutoSize = true
            };
            Controls.Add(name);
            //ConfigurationClass Label
            Label ip = new Label()
            {
                Text = "ConfigurationClass: "+con.ConfigurationClass,
                Location = new Point(10, 30),
                AutoSize = true
            };
            Controls.Add(ip);
            //Remove Button
            Button removeButton = new Button()
            {
                Text = "Remove",
                Tag = con,
                Location = new Point(75, 45)
            };
            removeButton.Click += OnRemove;
            Controls.Add(removeButton);
        }

        public bool ToggleSelected()
        {
            if (Selected)
                BackColor = Color.Gray;
            else
                BackColor = Color.Yellow;
            return (Selected = !Selected);
        }
    }
}
