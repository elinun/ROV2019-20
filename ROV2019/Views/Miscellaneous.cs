using ROV2019.Presenters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROV2019.Views
{
    public static class Dialog
    {
        public static string ShowPrompt(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public static void ShowMessageDialog(string message)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Message",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = message, AutoSize = true };
            prompt.Controls.Add(textLabel);
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
        }
    }
    [Obsolete("Use AttitudeIndicatorInstrumentControl instead.")]
    public class AttitudeIndicator : Control
    {
        public int RollMax { get; set; } = 255;
        public int RollMin { get; set; } = -255;
        private int rollValue;
        public int RollValue { get { return rollValue; } set { rollValue = value; Invalidate(); } }

        public int PitchMax { get; set; } = 255;
        public int PitchMin { get; set; } = -255;
        private int pitchValue;
        public int PitchValue { get { return pitchValue; } set { pitchValue = value;  Invalidate(); } }

        private int pixelRange;
        private int cX;
        private int cY;

        private int circleCX;
        private int circleCY;

        protected override void OnPaint(PaintEventArgs e)
        {
            pixelRange = Math.Min(Size.Height, Size.Width);
            circleCX = pixelRange / 2;
            circleCY = pixelRange / 2;
            Graphics g = e.Graphics;
            Pen black = new Pen(Color.Black);
            g.Clear(Color.White);
            //draw overlaying circle
            g.DrawArc(new Pen(Color.Black), 0, 0, pixelRange, pixelRange, 0, 360);
            //draw upper part
            Pen topPart = new Pen(Color.Blue);
            //determine points from ROll and Pitch
            //mapped to pixelRange * [0, 1]
            double xPercent = (((RollValue - RollMin) / (double)(RollMax - RollMin)));
            double yPercent = (((PitchValue - PitchMin) / (double)(PitchMax - PitchMin)) - 0.0);
            cX = (int)(pixelRange * xPercent);
            cY = (int)(pixelRange * yPercent);

            Point pitchArcStart = new Point(circleCX + (int)(Math.Sin(Math.PI * (yPercent)) * circleCX), circleCY + (int)(Math.Cos(Math.PI * ( yPercent)) * circleCY));
            Point pitchArcEnd = new Point((int)(circleCX - (Math.Sin(Math.PI*(yPercent)) * circleCX)), (int)(circleCY + (Math.Cos(Math.PI * (yPercent)) * circleCY)));
            Point rollArcStart = new Point((int)circleCX + (int)(Math.Sin(Math.PI * xPercent) * (circleCX)), circleCY + (int)(Math.Cos(Math.PI * xPercent) * (circleCY)));
            Point rollArcEndt = new Point((int)circleCX - (int)(Math.Sin(Math.PI * xPercent) * (circleCX)), circleCY - (int)(Math.Cos(Math.PI * xPercent) * (circleCY)));

            double rollStartTheta = Math.Atan((rollArcStart.Y - circleCY) / (rollArcStart.X - (double)circleCX));
            double rollEndTheta = Math.Atan((rollArcEndt.Y - circleCY) / (rollArcEndt.X - (double)circleCX));
            //the final points are the pitch points rotated by the same theta the roll points are rotated.
            //This may or may not be able to be simplified.
            //See this: https://stackoverflow.com/questions/2259476/rotating-a-point-about-another-point-2d
            Point arcStart = RotatePoint(circleCX, circleCY, rollStartTheta, pitchArcStart);
            Point arcEnd = RotatePoint(circleCX, circleCY, rollEndTheta, pitchArcEnd);
            double startΘ = Math.Atan(((arcStart.Y - circleCY) / (arcStart.X - (double)circleCX)));
            double endΘ = Math.Atan(((arcEnd.Y - circleCY) / (arcEnd.X - (double)circleCX)));
            //Point pointOnCircle = RotatePoint(circleCX, circleCY, (startΘ-endΘ)*2, arcStart);

            /*g.FillRectangle(black.Brush, cX, cY, 10, 10);
            g.FillRectangle(new Pen(Color.Red).Brush, pitchArcEnd.X, pitchArcEnd.Y, 10, 10);
            g.FillRectangle(new Pen(Color.Red).Brush, pitchArcStart.X, pitchArcStart.Y, 10, 10);
            g.FillRectangle(new Pen(Color.Blue).Brush, rollArcStart.X, rollArcStart.Y, 10, 10);
            g.FillRectangle(new Pen(Color.Blue).Brush, rollArcEndt.X, rollArcEndt.Y, 10, 10);*/
            g.FillRectangle(black.Brush, arcStart.X, arcStart.Y, 10, 10);
            g.FillRectangle(black.Brush, arcEnd.X, arcEnd.Y, 10, 10);

            Rectangle topRect = new Rectangle(arcEnd.X, 0, arcStart.X - arcEnd.X, Math.Max(arcEnd.Y, arcStart.Y)*2);

            float Θ = (float)((Math.Atan((arcEnd.Y-arcStart.Y)/(arcEnd.X-(double)arcStart.X)))*(180/Math.PI));
            //g.RotateTransform(Θ);
            g.DrawRectangle(new Pen(Color.Orange), topRect);
            g.FillPie(topPart.Brush, topRect, 180+Θ, 180);
        }

        private Point RotatePoint(int cx, int cy, double angle, Point p)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            //Fourmula: https://academo.org/demos/rotation-about-point/
            double xnew = p.X * c - p.Y * s;
            double ynew = p.X * s + p.Y * c;

            // translate point back:
            p.X = (int)xnew + cx;
            p.Y = (int)ynew + cy;
            return p;
        }
    }
}
