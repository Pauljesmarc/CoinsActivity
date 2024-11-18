using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;

namespace CoinsActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded= new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBox1.Image);

                Mat src = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);

                Mat gray = new Mat();
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                Mat blurred = new Mat();
                Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(15, 15), 0);

                Mat edges = new Mat();
                Cv2.Canny(blurred, edges, 100, 200);

                OpenCvSharp.Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                int coinCount = 0;
                foreach (var contour in contours)
                {
                    double area = Cv2.ContourArea(contour);
                    if (area > 100) 
                    {
                        coinCount++;
                    }
                }
                label3.Text = coinCount.ToString();
            }
            else
            {
                label3.Text = "0";
            }
        }
    }
}
