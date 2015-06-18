using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spektral_Degistir
{
    public partial class Form1 : Form
    {
        Bitmap img;
        int k = 5;
        LockBitmap imgLBMP;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Jpeg files (*.jpg)|*.jpg|Tiff files (*.tif)|*.tif|All valid files (*.bmp,*.tif,*.jpg)|*.bmp*.tif*.jpg";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                img = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName, false);
                pictureBox1.Image = img;
                //pictureBox1.Width = img.Width;
                //pictureBox1.Height = img.Height;
                pictureBox1.Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //resol = Convert.ToInt16(textBox1.Text);
            Color renk;
            byte R, G, B;
            imgLBMP = new LockBitmap(img);
            imgLBMP.LockBits();

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                {
                    renk = imgLBMP.GetPixel(i, j);
                    R = renk.R; G = renk.G; B = renk.B;
                    //renk = Color.FromArgb(spektralTrans(R), spektralTrans(G), spektralTrans(B));
                    imgLBMP.SetPixel(i, j, gameGraphic(R, G, B));
                }
            if (int.TryParse(textBox1.Text, out k))
            {
                Pixelate(k);
            }
            imgLBMP.UnlockBits();

            pictureBox1.Image = img;
            pictureBox1.Invalidate();
        }

        //public int spektralTrans(int val)
        //{
        //    int newVal = (int)(resol * (val + 1) / 256);
        //    if (newVal == 0)
        //        return 0;
        //    else
        //    {
        //        newVal *= 256 / resol;
        //        return newVal - 1;
        //    }

        //}
        public Color gameGraphic(int R, int G, int B)
        {
            int red = 8 * (R + 1) / 256;
            if (red != 0)
            { red *= 256 / 8; red--; }
            int green = 8 * (G + 1) / 256;
            if (green != 0)
            { green *= 256 / 8; green--; }
            int blue = 4 * (B + 1) / 256;
            if (blue != 0)
            { blue *= 256 / 4; blue--; }

            Color r = Color.FromArgb(red, green, blue);
            return r;
        }
        private void Pixelate(int k)
        {
            Bitmap downSample = new Bitmap(img.Width / k, img.Height / k);
            Bitmap upSample = new Bitmap(downSample.Width * k, downSample.Height * k);
            LockBitmap downSampleLBMP = new LockBitmap(downSample);


            downSampleLBMP.LockBits();
            for (int i = 0; i < img.Width / k; i++)
            {
                for (int j = 0; j < img.Height / k; j++)
                {
                    Color r = imgLBMP.GetPixel(i * k, j * k);
                    downSampleLBMP.SetPixel(i, j, r);
                }
            }
            downSampleLBMP.UnlockBits();

            LockBitmap upSampleLBMP = new LockBitmap(upSample);
            upSampleLBMP.LockBits();
            for (int i = 0; i < upSample.Width; i++)
            {
                for (int j = 0; j < upSample.Height; j++)
                {
                    Color r = downSampleLBMP.GetPixel(i / k, j / k);
                    upSampleLBMP.SetPixel(i, j, r);
                }
            }
            upSampleLBMP.UnlockBits();
            img = upSample;
        }




    }
}
