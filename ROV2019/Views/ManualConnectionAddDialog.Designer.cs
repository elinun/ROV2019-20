namespace ROV2019.Views
{
    partial class ManualConnectionAddDialog
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameField = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.IPField = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortField = new System.Windows.Forms.NumericUpDown();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.ShowPasswordButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PortField)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(1, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(41, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name: ";
            // 
            // nameField
            // 
            this.nameField.Location = new System.Drawing.Point(48, 12);
            this.nameField.Name = "nameField";
            this.nameField.Size = new System.Drawing.Size(100, 20);
            this.nameField.TabIndex = 1;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(1, 41);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(64, 13);
            this.IPLabel.TabIndex = 2;
            this.IPLabel.Text = "IP Address: ";
            // 
            // IPField
            // 
            this.IPField.Location = new System.Drawing.Point(71, 38);
            this.IPField.Name = "IPField";
            this.IPField.Size = new System.Drawing.Size(100, 20);
            this.IPField.TabIndex = 3;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(1, 68);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(32, 13);
            this.PortLabel.TabIndex = 4;
            this.PortLabel.Text = "Port: ";
            // 
            // PortField
            // 
            this.PortField.Location = new System.Drawing.Point(51, 64);
            this.PortField.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.PortField.Name = "PortField";
            this.PortField.Size = new System.Drawing.Size(120, 20);
            this.PortField.TabIndex = 5;
            this.PortField.Value = new decimal(new int[] {
            1741,
            0,
            0,
            0});
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(1, 94);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(59, 13);
            this.PasswordLabel.TabIndex = 5;
            this.PasswordLabel.Text = "Password: ";
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(66, 91);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(100, 20);
            this.PasswordField.TabIndex = 5;
            this.PasswordField.UseSystemPasswordChar = true;
            // 
            // ShowPasswordButton
            // 
            this.ShowPasswordButton.Location = new System.Drawing.Point(172, 88);
            this.ShowPasswordButton.Name = "ShowPasswordButton";
            this.ShowPasswordButton.Size = new System.Drawing.Size(75, 23);
            this.ShowPasswordButton.TabIndex = 5;
            this.ShowPasswordButton.Text = "Show";
            this.ShowPasswordButton.UseVisualStyleBackColor = true;
            this.ShowPasswordButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShowPasswordButton_MouseDown);
            this.ShowPasswordButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShowPasswordButton_MouseUp);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(95, 226);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ManualConnectionAddDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ShowPasswordButton);
            this.Controls.Add(this.PasswordField);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.PortField);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.IPField);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.nameLabel);
            this.Name = "ManualConnectionAddDialog";
            this.Text = "Add Connection";
            ((System.ComponentModel.ISupportInitialize)(this.PortField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameField;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox IPField;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.NumericUpDown PortField;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button ShowPasswordButton;
        private System.Windows.Forms.Button SaveButton;
    }
}