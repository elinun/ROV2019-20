namespace ROV2019.Views
{
    partial class AddControllerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.ConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ControllerComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FriendlyNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Configuration: ";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(98, 122);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Items.AddRange(new object[] {
            "SlimDX"});
            this.TypeComboBox.Location = new System.Drawing.Point(98, 1);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.TypeComboBox.TabIndex = 3;
            // 
            // ConfigurationComboBox
            // 
            this.ConfigurationComboBox.FormattingEnabled = true;
            this.ConfigurationComboBox.Items.AddRange(new object[] {
            "Arcade",
            "Helicopter",
            "Amelie"});
            this.ConfigurationComboBox.Location = new System.Drawing.Point(98, 27);
            this.ConfigurationComboBox.Name = "ConfigurationComboBox";
            this.ConfigurationComboBox.Size = new System.Drawing.Size(121, 21);
            this.ConfigurationComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Controller: ";
            // 
            // ControllerComboBox
            // 
            this.ControllerComboBox.FormattingEnabled = true;
            this.ControllerComboBox.Items.AddRange(new object[] {
            "PlayStation"});
            this.ControllerComboBox.Location = new System.Drawing.Point(98, 55);
            this.ControllerComboBox.Name = "ControllerComboBox";
            this.ControllerComboBox.Size = new System.Drawing.Size(121, 21);
            this.ControllerComboBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Name: ";
            // 
            // FriendlyNameTextBox
            // 
            this.FriendlyNameTextBox.Location = new System.Drawing.Point(98, 80);
            this.FriendlyNameTextBox.Name = "FriendlyNameTextBox";
            this.FriendlyNameTextBox.Size = new System.Drawing.Size(121, 20);
            this.FriendlyNameTextBox.TabIndex = 8;
            // 
            // AddControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.FriendlyNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ControllerComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConfigurationComboBox);
            this.Controls.Add(this.TypeComboBox);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddControllerForm";
            this.Text = "AddControllerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.ComboBox ConfigurationComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ControllerComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FriendlyNameTextBox;
    }
}