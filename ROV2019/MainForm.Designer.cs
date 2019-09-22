using ROV2019.Views;

namespace ROV2019
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ConnectionsList = new System.Windows.Forms.TableLayoutPanel();
            this.manualAddButton = new System.Windows.Forms.Button();
            this.ScanButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ConfigureTrimButton = new System.Windows.Forms.Button();
            this.PIDAssistField = new System.Windows.Forms.CheckBox();
            this.SensorButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ControllersListTable = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.UseControllerButton = new System.Windows.Forms.Button();
            this.AddControllerButton = new System.Windows.Forms.Button();
            this.ConfigureControllerButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
            this.missionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TimerLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectionsList
            // 
            this.ConnectionsList.AutoScroll = true;
            this.ConnectionsList.AutoSize = true;
            this.ConnectionsList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ConnectionsList.ColumnCount = 1;
            this.ConnectionsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConnectionsList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConnectionsList.Location = new System.Drawing.Point(12, 49);
            this.ConnectionsList.Name = "ConnectionsList";
            this.ConnectionsList.RowCount = 1;
            this.ConnectionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConnectionsList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConnectionsList.Size = new System.Drawing.Size(240, 50);
            this.ConnectionsList.TabIndex = 1;
            // 
            // manualAddButton
            // 
            this.manualAddButton.AutoSize = true;
            this.manualAddButton.Location = new System.Drawing.Point(126, 376);
            this.manualAddButton.Name = "manualAddButton";
            this.manualAddButton.Size = new System.Drawing.Size(75, 23);
            this.manualAddButton.TabIndex = 1;
            this.manualAddButton.Text = "Manual Add";
            this.manualAddButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.manualAddButton.UseVisualStyleBackColor = true;
            this.manualAddButton.Click += new System.EventHandler(this.manualAddButton_Click);
            // 
            // ScanButton
            // 
            this.ScanButton.AutoSize = true;
            this.ScanButton.Location = new System.Drawing.Point(78, 376);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(42, 23);
            this.ScanButton.TabIndex = 2;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.AutoSize = true;
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(4, 376);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(57, 23);
            this.ConnectButton.TabIndex = 3;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ConfigureTrimButton
            // 
            this.ConfigureTrimButton.Enabled = false;
            this.ConfigureTrimButton.Location = new System.Drawing.Point(35, 347);
            this.ConfigureTrimButton.Name = "ConfigureTrimButton";
            this.ConfigureTrimButton.Size = new System.Drawing.Size(53, 23);
            this.ConfigureTrimButton.TabIndex = 4;
            this.ConfigureTrimButton.Text = "Trim";
            this.ConfigureTrimButton.UseVisualStyleBackColor = true;
            this.ConfigureTrimButton.Click += new System.EventHandler(this.ConfigureTrimButton_Click);
            // 
            // PIDAssistField
            // 
            this.PIDAssistField.AutoSize = true;
            this.PIDAssistField.BackColor = System.Drawing.SystemColors.Control;
            this.PIDAssistField.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.PIDAssistField.Checked = true;
            this.PIDAssistField.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PIDAssistField.Location = new System.Drawing.Point(308, 380);
            this.PIDAssistField.Name = "PIDAssistField";
            this.PIDAssistField.Size = new System.Drawing.Size(91, 31);
            this.PIDAssistField.TabIndex = 5;
            this.PIDAssistField.Text = "Vertical Stabilizer";
            this.PIDAssistField.UseVisualStyleBackColor = false;
            this.PIDAssistField.CheckedChanged += new System.EventHandler(this.PIDAssistField_CheckedChanged);
            // 
            // SensorButton
            // 
            this.SensorButton.Enabled = false;
            this.SensorButton.Location = new System.Drawing.Point(94, 347);
            this.SensorButton.Name = "SensorButton";
            this.SensorButton.Size = new System.Drawing.Size(75, 23);
            this.SensorButton.TabIndex = 6;
            this.SensorButton.Text = "Sensors";
            this.SensorButton.UseVisualStyleBackColor = true;
            this.SensorButton.Click += new System.EventHandler(this.SensorButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Saved ROV Connections:";
            // 
            // ControllersListTable
            // 
            this.ControllersListTable.AutoScroll = true;
            this.ControllersListTable.AutoSize = true;
            this.ControllersListTable.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ControllersListTable.ColumnCount = 1;
            this.ControllersListTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ControllersListTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ControllersListTable.Cursor = System.Windows.Forms.Cursors.Default;
            this.ControllersListTable.Location = new System.Drawing.Point(470, 49);
            this.ControllersListTable.Name = "ControllersListTable";
            this.ControllersListTable.RowCount = 1;
            this.ControllersListTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51F));
            this.ControllersListTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.ControllersListTable.Size = new System.Drawing.Size(240, 50);
            this.ControllersListTable.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(528, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Saved Controllers:";
            // 
            // UseControllerButton
            // 
            this.UseControllerButton.AutoSize = true;
            this.UseControllerButton.Enabled = false;
            this.UseControllerButton.Location = new System.Drawing.Point(470, 376);
            this.UseControllerButton.Name = "UseControllerButton";
            this.UseControllerButton.Size = new System.Drawing.Size(53, 23);
            this.UseControllerButton.TabIndex = 10;
            this.UseControllerButton.Text = "Use";
            this.UseControllerButton.UseVisualStyleBackColor = true;
            this.UseControllerButton.Click += new System.EventHandler(this.UseControllerButton_Click);
            // 
            // AddControllerButton
            // 
            this.AddControllerButton.AutoSize = true;
            this.AddControllerButton.Location = new System.Drawing.Point(568, 376);
            this.AddControllerButton.Name = "AddControllerButton";
            this.AddControllerButton.Size = new System.Drawing.Size(53, 23);
            this.AddControllerButton.TabIndex = 11;
            this.AddControllerButton.Text = "Add";
            this.AddControllerButton.UseVisualStyleBackColor = true;
            this.AddControllerButton.Click += new System.EventHandler(this.AddControllerButton_Click);
            // 
            // ConfigureControllerButton
            // 
            this.ConfigureControllerButton.AutoSize = true;
            this.ConfigureControllerButton.Location = new System.Drawing.Point(648, 376);
            this.ConfigureControllerButton.Name = "ConfigureControllerButton";
            this.ConfigureControllerButton.Size = new System.Drawing.Size(62, 23);
            this.ConfigureControllerButton.TabIndex = 12;
            this.ConfigureControllerButton.Text = "Configure";
            this.ConfigureControllerButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(61, 20);
            this.toolStripTextBox1.Text = "Settings";
            // 
            // missionsToolStripMenuItem
            // 
            this.missionsToolStripMenuItem.Name = "missionsToolStripMenuItem";
            this.missionsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.missionsToolStripMenuItem.Text = "Missions";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TimerLabel
            // 
            this.TimerLabel.AutoSize = true;
            this.TimerLabel.Location = new System.Drawing.Point(341, 49);
            this.TimerLabel.Name = "TimerLabel";
            this.TimerLabel.Size = new System.Drawing.Size(34, 13);
            this.TimerLabel.TabIndex = 14;
            this.TimerLabel.Text = "00:00";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(258, 76);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 15;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(389, 76);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 16;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(722, 411);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.TimerLabel);
            this.Controls.Add(this.ConfigureControllerButton);
            this.Controls.Add(this.AddControllerButton);
            this.Controls.Add(this.UseControllerButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ControllersListTable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SensorButton);
            this.Controls.Add(this.PIDAssistField);
            this.Controls.Add(this.ConfigureTrimButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ScanButton);
            this.Controls.Add(this.manualAddButton);
            this.Controls.Add(this.ConnectionsList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "CHS ROV 2K19";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel ConnectionsList;
        private System.Windows.Forms.Button manualAddButton;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button ConfigureTrimButton;
        private System.Windows.Forms.CheckBox PIDAssistField;
        private System.Windows.Forms.Button SensorButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel ControllersListTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button UseControllerButton;
        private System.Windows.Forms.Button AddControllerButton;
        private System.Windows.Forms.Button ConfigureControllerButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem missionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripTextBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label TimerLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button button1;
    }
}

