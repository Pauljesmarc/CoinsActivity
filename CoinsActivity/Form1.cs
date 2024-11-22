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

        const double cent_5 = 0.05;  
        const double cent_10 = 0.1; 
        const double cent_25 = 0.25;
        const double one_peso = 1.0;
        const double five_peso = 5.0;

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
            loaded = new Bitmap(openFileDialog1.FileName);
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
                double totalValue = 0.0;
                int cent_5_ctr = 0, cent_10_ctr = 0, cent_25_ctr =0, cinco_ctr =0,piso_ctr = 0;

                foreach (var contour in contours)
                {
                    double area = Cv2.ContourArea(contour);
                    Console.WriteLine(area);
                    if (area > 100) 
                    {
                        coinCount++;

                        if (area >=2600 && area < 2800)
                        {
                            cent_5_ctr++;
                            totalValue += cent_5; 
                        }
                        else if (area >= 3100 && area < 3300)
                        {
                            cent_10_ctr++;
                            totalValue += cent_10;
                        }
                        else if (area >= 4300 && area < 4600)
                        {
                            cent_25_ctr++;
                            totalValue += cent_25;
                        }else if(area >= 6200 && area < 6400)
                        {
                            piso_ctr++;
                            totalValue += one_peso;
                        }else if(area >= 7800 && area < 8100)
                        {
                            cinco_ctr++;
                            totalValue += five_peso;
                        }
                    }
                }

                Console.WriteLine($"{cent_5_ctr}'\n'{cent_10_ctr}'\n'{cent_25_ctr}'\n'{piso_ctr}'\n'{cinco_ctr}");
                label3.Text = $"Coins: {coinCount}\nValue: {totalValue:C2}";
            }
            else
            {
                label3.Text = "Coins: 0\nValue: $0.00";
            }
        }
    }
}
