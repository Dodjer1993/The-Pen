using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace Draw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Graphics g;
        Pen p = new Pen(Color.Black, 1);
        Point sp = new Point(0, 0);
        Point ep = new Point(0, 0);
        int k = 0;
        private int cX, cY, dX, dY;
        Pen pen;
        Color color;
        int size;

        int Case;
        private void Form1_Load(object sender, EventArgs e)
        {
            color = Color.Red;
            size = 2;
            Case = 1;
            //toolStrip1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - toolStrip1.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height);
            Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen11), 48, 48);// كود تغير شكل الماوس
            this.Cursor = new Cursor(B.GetHicon());

            
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.TopMost = true;
            g = pictureBox1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                if (k == 0)
                {
                    dX = e.X - cX;
                    dY = e.Y - cY;
                }
            }
            else if (Case == 3)
            {
                if (k == 0)
                {

                    dX = e.X - cX;
                    dY = e.Y - cY;
                }
            }
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                sp = e.Location;
                if (e.Button == MouseButtons.Left)
                {
                    k = 1;
                }
            }
            else if (Case == 3)
            {
                sp = e.Location;
                if (e.Button == MouseButtons.Left)
                {
                    k = 1;
                }
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                if (k == 1)
                {
                    ep = e.Location;
                    //حجم الخط هنا
                    //تنسيق نوع الخط ايضا
                    pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    g.DrawLine(pen, sp, ep);
                    sp = ep;
                }
            }
            else if (Case == 3)
            {
                if (k == 1)
                {
                    ep = e.Location;
                    //حجم الخط هنا
                    //تنسيق نوع الخط ايضا
                    Color baseColor = Color.White ;
                    //color = Color.FromArgb(30, baseColor);
                    pen = new Pen(baseColor, 20) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    g.DrawLine(pen, sp, ep);
                    sp = ep;
                }
            }
            
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                k = 0;
            }
            else if (Case == 3)
            {
                k = 0;
            }
        }
        // لون الخط
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            color = Color.Red;
            pen = new Pen(color , size ) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            color = Color.Blue ;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            color = Color.Yellow ;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            color = Color.Green ;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            color = Color.Black ;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        ColorDialog Cd = new ColorDialog();
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            
            if (Cd.ShowDialog() == DialogResult.OK)
            {
                color = Cd.Color;
            }
        }
        // لون الخط
        //حجم الخط

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            size = 4;

        }
        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            size = 6;
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            size = 2;
        }
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            size = 8;
        }

        //حجم الخط
        private void toolStripButton2_Click(object sender, EventArgs e)//حفظ الصورة
        {              
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
            g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
            g.Dispose();
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Png files|*.png|jpeg files|*jpg|bitmaps|*.bmp";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(s.FileName))
                {
                    File.Delete(s.FileName);
                }
                if (s.FileName.Contains(".jpg"))
                {
                    bmp.Save(s.FileName, ImageFormat.Jpeg);
                }
                else if (s.FileName.Contains(".png"))
                {
                    bmp.Save(s.FileName, ImageFormat.Png);
                }
                else if (s.FileName.Contains(".bmp"))
                {
                    bmp.Save(s.FileName, ImageFormat.Bmp);
                }
            }
            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void eraserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case = 3;
            if (Case == 3)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Eraser ), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
        }

        private void penToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case = 1;
            if (Case == 1)
            {
                color = Color.Red;
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen11 ), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            size = int.Parse(toolStripComboBox1.Text );
        }

    }
}
