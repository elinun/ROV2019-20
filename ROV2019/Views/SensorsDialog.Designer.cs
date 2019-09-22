namespace ROV2019.Views
{
    partial class SensorsDialog
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
            this.InternalTemperatureLabel = new System.Windows.Forms.Label();
            this.attitudeIndicatorInstrumentControl1 = new ROV2019.Views.AttitudeIndicatorInstrumentControl();
            this.TareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Internal Temperature (Celcius): ";
            // 
            // InternalTemperatureLabel
            // 
            this.InternalTemperatureLabel.AutoSize = true;
            this.InternalTemperatureLabel.Location = new System.Drawing.Point(172, 9);
            this.InternalTemperatureLabel.Name = "InternalTemperatureLabel";
            this.InternalTemperatureLabel.Size = new System.Drawing.Size(13, 13);
            this.InternalTemperatureLabel.TabIndex = 2;
            this.InternalTemperatureLabel.Text = "--";
            // 
            // attitudeIndicatorInstrumentControl1
            // 
            this.attitudeIndicatorInstrumentControl1.Location = new System.Drawing.Point(44, 34);
            this.attitudeIndicatorInstrumentControl1.Name = "attitudeIndicatorInstrumentControl1";
            this.attitudeIndicatorInstrumentControl1.Size = new System.Drawing.Size(188, 193);
            this.attitudeIndicatorInstrumentControl1.TabIndex = 0;
            this.attitudeIndicatorInstrumentControl1.Text = "attitudeIndicatorInstrumentControl1";
            // 
            // TareButton
            // 
            this.TareButton.Location = new System.Drawing.Point(110, 226);
            this.TareButton.Name = "TareButton";
            this.TareButton.Size = new System.Drawing.Size(75, 23);
            this.TareButton.TabIndex = 3;
            this.TareButton.Text = "Tare";
            this.TareButton.UseVisualStyleBackColor = true;
            this.TareButton.Click += new System.EventHandler(this.TareButton_Click);
            // 
            // SensorsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TareButton);
            this.Controls.Add(this.InternalTemperatureLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.attitudeIndicatorInstrumentControl1);
            this.Name = "SensorsDialog";
            this.Text = "Sensors";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorsDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AttitudeIndicatorInstrumentControl attitudeIndicatorInstrumentControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label InternalTemperatureLabel;
        private System.Windows.Forms.Button TareButton;
    }
}