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


namespace Print1111
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //____________________الرسم على الصورة
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
        //المربع_____________
        Rectangle Rec;
        Point XY;
        Point X1Y1;
        bool B = false;
        //المربع________________
        //الرسم على الصورة__________________
        private void Form1_Load(object sender, EventArgs e)
        {   
                color = Color.Red;
                size = 2;
                Case = 1;
                button1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - button1.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height);

                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());

                //this.BackColor = Color.Crimson; // هذا الكود يجعل الفورم بمثابة زجاج تشاهد ما خلفه
                //this.TransparencyKey = Color.Crimson;
                //pictureBox1.BackColor = this.BackColor;

                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                this.TopMost = true;
                CaptureScreen();
                if (File.Exists("picture.jpg"))
                {
                    File.Delete("picture.jpg");
                    memoryImage.Save("picture.jpg");
                    pictureBox1.BackgroundImage = Image.FromFile("picture.jpg");
                }
                else
                {
                    memoryImage.Save("picture.jpg");
                    pictureBox1.BackgroundImage = Image.FromFile("picture.jpg");
                }

                

                g = pictureBox1.CreateGraphics();
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed ;
            

                
        }
        //الرسم على الصورة_____________________
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
            else if (Case == 2)
            {
                pen = new Pen(color, 3) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                g.DrawRectangle(pen , GetRec());

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
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
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
            else if (Case == 2)
            {
                if (B == true)
                {
                    X1Y1 = e.Location;
                }
            }
            else if (Case == 3)
            {
                if (k == 1)
                {
                    ep = e.Location;
                    //حجم الخط هنا
                    //تنسيق نوع الخط ايضا
                    Color baseColor = Color.Yellow;
                    color = Color.FromArgb(30, baseColor);
                    pen = new Pen(color, 18) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    g.DrawLine(pen, sp, ep);
                    sp = ep;
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                sp = e.Location;
                if (e.Button == MouseButtons.Left)
                {
                    k = 1;
                }
            }
            else if (Case == 2)
            {
                B = true;
                XY = e.Location;
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
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Case == 1)
            {
                k = 0;
            }
            else if (Case == 2)
            {
                if (B == true)
                {
                    X1Y1 = e.Location;
                    B = false;
                }
            }
            else if (Case == 3)
            {
                k = 0;
            }
        }
        // الرسم على الصور _____________________
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // لون الخط___________________________
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            color = Color.Red;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            color = Color.Blue;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            color = Color.Yellow;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            color = Color.Green;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            color = Color.Black;
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
        // لون الخط________________________
        //حجم الخط_____________________
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            size = 2;
        }
        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            size = 4;
        }
        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            size = 6;
        }
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            size = 8;
        }
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            size = int.Parse(toolStripComboBox1.Text);
        }
        //حجم الخط_______________________
        private void toolStripMenuItem4_Click(object sender, EventArgs e)//حفظ الصورة
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Case = 2;
            if (Case == 2)
            {
                
                color = Color.Red;
                this.Cursor = Cursors.Cross;

            }
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Case = 1;
            if (Case == 1)
            {
                color = Color.Red;
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            
        }



        private Rectangle GetRec()
        {
            Rec = new Rectangle();
            Rec.X = Math.Min(XY.X, X1Y1.X);
            Rec.Y = Math.Min(XY.Y, X1Y1.Y);
            Rec.Width = Math.Abs(XY.X - X1Y1.X);
            Rec.Height = Math.Abs(XY.Y - X1Y1.Y);
            return Rec;
        }
        //التقاط صورة HD
        Bitmap memoryImage;
        private void CaptureScreen()
        {
            int y = this.Top;
            this.Top = Screen.PrimaryScreen.Bounds.Height + 1000;
            // كود التقاط صورة
            Graphics myGraphics = this.CreateGraphics();
            Size s = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            memoryImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Left, Screen.PrimaryScreen.Bounds.X, 0, 0, s);
            //____________________
            this.Top = y;
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            Case = 3;
            if (Case == 3)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.highlighter), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
        }


        //التقاط صورة HD
        
    }
}
