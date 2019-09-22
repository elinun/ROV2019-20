using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace ROV2019
{
    public partial class FormShapeDetection : Form
    {
        Image<Bgr, byte> imgInput;
        public FormShapeDetection()
        {
            InitializeComponent();
        }

        private void FormShapeDetection_Load(object sender, EventArgs e)
        {
            panel1.AutoScroll = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(dialog.FileName)
   .Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                    pictureBox1.Image = imgInput.Bitmap;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("File format is not image.");
            }
        }
        bool playing = false;
        VideoCapture capture = new VideoCapture();
        Mat mCap = new Mat();
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!playing) //just to initialize only once
            {
                playing = true;
                pictureBox1.Width = capture.Width;
                pictureBox1.Height = capture.Height;
            }
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 540);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playing = false;
            capture.Stop();
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            capture.Read(mCap);
            try
            {
                Image<Bgr, byte> capImage = new Image<Bgr, byte>(mCap.Bitmap);
                CvInvoke.Flip(capImage, capImage, Emgu.CV.CvEnum.FlipType.Horizontal);
                findShapes(capImage);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong...\nTry starting the webcam when an image is not already loaded.");
                Application.Exit();
            }
        }

        private void detectShapesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (imgInput == null)
            {
                MessageBox.Show("No image detected.");
                return;
            }
            findShapes(imgInput);
        }

        private void findShapes(Image<Bgr, byte> img)
        {
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);
            
            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            //Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrWDown().PyrUp();
            UMat blurred = new UMat();
            CvInvoke.GaussianBlur(uimage, blurred, new Size(21, 21), 1.0);

            #region circle detection
            Stopwatch watch = Stopwatch.StartNew();
            double cannyThreshold = 70.0;
            //double circleAccumulatorThreshold = 2*(GetContrast(uimage))-50;
            double circleAccumulatorThreshold = 60;
            CircleF[] circles = CvInvoke.HoughCircles(blurred, HoughType.Gradient, 2.0, 5.0, cannyThreshold, circleAccumulatorThreshold, 5, 100);
            //outputLabel.Text += "C: " + circles.Length;

            watch.Stop();
            //msgBuilder.Append(String.Format("Hough circles - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            #region Canny and edge detection
            watch.Reset(); watch.Start();
            double cannyThresholdLinking = 125.0;
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);

            LineSegment2D[] lines = CvInvoke.HoughLinesP(
               cannyEdges,
               1, //Distance resolution in pixel-related units
               Math.PI / 45.0, //Angle resolution measured in radians.
               20, //threshold
               30, //min Line width
               10); //gap between lines

            watch.Stop();
            //outputLabel.Text += "\nL: " + lines.Length;
            //msgBuilder.Append(String.Format("Canny & Hough lines - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            //#region Find triangles and rectangles
            watch.Reset(); watch.Start();
            List<Triangle2DF> triangleList = new List<Triangle2DF>();
            List<RotatedRect> rectangleList = new List<RotatedRect>();
            List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle
            List<LineSegment2D> lineBoxes = new List<LineSegment2D>(); //for the species that looks like a line
            List<CircleF> circleList = new List<CircleF>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                            {
                                Point[] pts = approxContour.ToArray();
                                triangleList.Add(new Triangle2DF(
                                   pts[0],
                                   pts[1],
                                   pts[2]
                                   ));
                            }
                            else if (approxContour.Size == 4) //The contour has 4 vertices.
                            {
                                #region determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                       edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                //rectangleList.Add(CvInvoke.MinAreaRect(approxContour));
                                #endregion

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }

                            pictureBox1.Image = imgInput.Bitmap;
                        }
                    }
                    textBox8.Text = (triangleList.Count/2).ToString();
                    textBox6.Text = (boxList.Count/2).ToString();
                    textBox4.Text = rectangleList.Count.ToString();
                    textBox2.Text = circles.Length.ToString();
                }
            }

            #region draw triangles and rectangles
            Image<Bgr, Byte> triangleRectangleImage = img.CopyBlank();
            foreach (Triangle2DF triangle in triangleList)
                img.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            foreach (RotatedRect box in boxList)
                img.Draw(box, new Bgr(Color.DarkOrange), 2);
            #endregion

            #region draw circles
            foreach (CircleF circle in circles)
                img.Draw(circle, new Bgr(Color.Cyan), 2);
            #endregion

            #region draw lines
            Image<Bgr, Byte> lineImage = img.CopyBlank();
            foreach (LineSegment2D line in lines)
                img.Draw(line, new Bgr(Color.Green), 2);
            #endregion

            pictureBox1.Image = img.ToBitmap();
        }

        private int GetContrast(UMat img)
        {
            double averageIntensity = img.Bytes[0];
            double MAD = 0.0;//Mean Absolute Deviation
            double N = 1.0;
            Bitmap bm = img.Bitmap;
            for(int x = 0;x<bm.Width;x++)
            {
                for(int y = 0;y<bm.Height;y++)
                {
                    N += 1.0;
                    byte intensity = bm.GetPixel(x, y).B;
                    averageIntensity = approxRollingAverage(averageIntensity, intensity, N);
                    MAD = approxRollingAverage(MAD, Math.Abs(intensity - averageIntensity), N);
                }

            }
            return (int)MAD;
        }

        double approxRollingAverage(double avg, double new_sample, double N)
        {

            avg -= avg / N;
            avg += new_sample / N;

            return avg;
        }
    }
}


