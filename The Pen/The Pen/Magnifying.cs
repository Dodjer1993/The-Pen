using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace The_Pen
{
    public partial class Magnifying : Form
    {
        Bitmap bit;
        Graphics graf;
        int zoom = 2;
        int mov;
        int movX;
        int movY;
        public Magnifying()
        {
            InitializeComponent();
        }
        private static class Win32Native
        {
            public const int DESKTOPVERTRES = 0x75;
            public const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = true;
            int width, height;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hDC = g.GetHdc();
                width = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPHORZRES);
                height = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPVERTRES);
                g.ReleaseHdc(hDC);
            }
            bit = new Bitmap(pictureBox1.Width/zoom  , pictureBox1.Height /zoom );
            graf = Graphics.FromImage(bit);
            Size s = new Size (width ,height );
            graf.CopyFromScreen(MousePosition.X - pictureBox1.Width / (zoom * 2), MousePosition.Y - pictureBox1.Height / (zoom * 2), 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            pictureBox1.Image = bit;
        }
        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close ();
        }

        private void panel1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseUp_1(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            zoom += 2;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (zoom == 2) return;
            else zoom -= 2;
        }

        private void Magnifying_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Owner.Show();
        }







    }
}
