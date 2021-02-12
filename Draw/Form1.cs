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
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Draw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Stack<string> Undo = new Stack<string>();
        Stack<string> Redo = new Stack<string>();
        public void UndoRection()
        {
            if (Undo.Count < 1)
            {
                return;
            }
            string strUndoPop = Undo.Pop();

            if (Undo.Count == 1)
            {
                strUndoPop = "picture\\picture.jpg";
                pictureBox1.Image = Image.FromFile(strUndoPop);
            }
            else
            {
                pictureBox1.Image = Image.FromFile(strUndoPop);
                if (Redo.Count == 0 || strUndoPop != "")
                {
                    Redo.Push(strUndoPop);
                }
            }
        }
        public void RedoRection()
        {
            if (Redo.Count < 1)
            {
                return;
            }
            string strRedoPop = Redo.Pop();
            if (Undo.Count == 0 || strRedoPop != "")
            {
                Undo.Push(strRedoPop);
            }
            pictureBox1.Image = Image.FromFile(strRedoPop);
        }
        public void Mouse()// كود دالة تغير شكل الماوس
        {
            if (Case == 1 && PenCase == 1)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen11), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 1 && PenCase == 2)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen12), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 1&& PenCase ==3)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Eraser), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 2 || Case == 3)
            {
                this.Cursor = Cursors.Cross;
            }
        }
        public void SaveData()//حفظ الداتا
        {
            try
            {
                StreamWriter SW = new StreamWriter("Data1");
                SW.Write(size + ";");
                SW.Write(PenCase + ";");
                SW.Write(Case + ";");
                SW.Write(figer + ";");
                SW.Write(color.R + ";");
                SW.Write(color.G + ";");
                SW.Write(color.B + ";");
                SW.Write(color2.R + ";");
                SW.Write(color2.G + ";");
                SW.Write(color2.B);
                SW.Close();
            }
            catch (Exception EXP)
            {
                MessageBox.Show(EXP.ToString());
            }
        }
        public void LoadData()//تحميل الداتا
        {
            try
            {

                if (File.Exists("Data1"))
                {
                    StreamReader SR = new StreamReader("Data1");
                    string str = SR.ReadToEnd();
                    string[] Data = str.Split(';');
                    size = int.Parse(Data[0]);
                    PenCase = int.Parse(Data[1]);
                    Case = int.Parse(Data[2]);
                    color = Color.FromArgb(int.Parse(Data[4]), int.Parse(Data[5]), int.Parse(Data[6]));
                    color2 = Color.FromArgb(int.Parse(Data[7]), int.Parse(Data[8]), int.Parse(Data[9]));
                    pictureBox1.BackColor = color2;
                    SR.Close();
                    if(Case ==2)
                    {
                        figer =int .Parse (Data[3]);
                    }
                    Mouse();
                }
                else
                {
                    color = Color.Red;
                    size = 2;
                    PenCase = 1;
                    Case = 1;
                }
            }
            catch (Exception Exp)
            {
                color = Color.Red;
                size = 2;
                PenCase = 1;
                Case = 1;
            }
        }
        Graphics g;
        Point sp = new Point(0, 0);
        Point ep = new Point(0, 0);
        int k = 0;
        Pen pen;
        Color color2;
        Color color;
        int size;
        Brush brush;
        int Case;
        Point PLine = new Point();
        //المربع_____________
        private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        //المربع________________
        List<Point> curPoints = new List<Point>();
        List<List<Point>> allPoints = new List<List<Point>>();
        int drawonImage = 1;
        private void Form1_Load(object sender, EventArgs e)
        {

            LoadData();
            Mouse();
            button1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - button1.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height);
            label1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - label1.Width, Screen.PrimaryScreen.Bounds.Height - label1.Height);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.TopMost = true;
            g = pictureBox1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            DrawOnImag();
            
            
        }


        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                label1.Visible = false;
                if (Case == 1)
                {
                    if (curPoints.Count > 1)
                    {
                        // begin fresh line or curve
                        curPoints.Clear();
                        // startpoint

                        curPoints.Add(e.Location);
                    }
                }
                else if ( Case==3 || Case ==2)
                {
                    RectStartPoint = e.Location;
                }
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Case == 1)
                {
                        if (e.Button != MouseButtons.Left) return;
                        // here we should check if the distance is more than a minimum!
                        curPoints.Add(e.Location);
                        // let it show
                        pictureBox1.Invalidate();
                }
                else if (Case == 2)
                {
                    if (e.Button != MouseButtons.Left)
                        return;
                    Point tempEndPoint = e.Location;
                    Rect.Location = new Point(
                        Math.Min(RectStartPoint.X, tempEndPoint.X),
                        Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                    Rect.Size = new Size(
                        Math.Abs(RectStartPoint.X - tempEndPoint.X),
                        Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
                    pictureBox1.Invalidate();
                }
                else if (Case == 3)
                {
                    if (e.Button != MouseButtons.Left)
                        return;
                    PLine = e.Location;
                    pictureBox1.Invalidate();
                }
            }
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Case == 1)
                {
                    if (curPoints.Count > 1)
                    {
                        // ToList creates a copy
                        allPoints.Add(curPoints.ToList());
                        curPoints.Clear();


                    }
                    k = 0;
                    DrawOnImag();
                }
                else if ( Case == 2 || Case == 3 )
                {
                    DrawOnImag();
                }
                
            }
        }
        // لون الخط
        public void ColorFunc(Color c)//دالة تغيير اللون
        {
            color = c;
            pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ColorFunc(((ToolStripMenuItem)sender).BackColor);
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
        public void SizeFunc(string s)//دالة الحجم
        {
            size = int.Parse(s);
            s = toolStripMenuItem13.Text;
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            SizeFunc(((ToolStripMenuItem)sender).Text);
        }
        

        //حجم الخط

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void eraserToolStripMenuItem_Click(object sender, EventArgs e)
        {
                Case = 1;
                PenCase = 3;
                Mouse();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            
        }

        private void bordColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Black;
        }

        public void ColorBoardFunc(Color c)//دالة تغيير اللون
        {
            pictureBox1.BackColor = c;
            color2 = c;
        }
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            ColorBoardFunc(((ToolStripMenuItem)sender).BackColor);
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            if (Cd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = null;
                pictureBox1 .BackColor  = Cd.Color;
                color2 = Cd.Color;
            }
        }
        //==================التقاط صورة Hd للفورم
        private static class Win32Native
        {
            public const int DESKTOPVERTRES = 0x75;
            public const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);
        }
        //==================
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//حفظ الصورة
        {
            
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Png files|*.png";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(s.FileName))
                {
                    File.Delete(s.FileName);
                }

                else if (s.FileName.Contains(".png"))
                {
                    b.Save(s.FileName, ImageFormat.Png);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
                figer = 1;
                Case = 2;
                Mouse();
        }

        private void straightLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case = 3;
            Mouse();
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figer = 3;
            Case = 2;
            Mouse();
        }
        int figer;
        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            figer = 2;
            Case = 2;
            Mouse();
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            figer = 4;
            Case = 2;
            Mouse();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = int.Parse(toolStripComboBox1.Text);
        }

        int numperpic = 0;// هذه العملية للرسم بدون ان يحذف بقية الرسومات
        Bitmap b;
        public void DrawOnImag()
        {
            Directory.CreateDirectory("picture");
            numperpic = numperpic + 1;
            b = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Width);
            pictureBox1.DrawToBitmap(b, pictureBox1.ClientRectangle);
            if (drawonImage == 1)
            {
                b.Save("picture\\picture.jpg");
                b.Save("picture\\picture" + numperpic.ToString() + ".jpg");
                drawonImage = 2;
            }
            else
            {
                b.Save("picture\\picture" + numperpic.ToString() + ".jpg");
                pictureBox1.Image = Image.FromFile("picture\\picture" + numperpic.ToString() + ".jpg");
            }
            //===============================stack
            int numperpic2 = numperpic;
            if (Undo.Count < 1)
            {
                Undo.Push("picture\\picture.jpg");
                if (numperpic2 - 1 == 0)
                {
                    numperpic2 += 1;
                }
                Undo.Push("picture\\picture" + (numperpic2 - 1).ToString() + ".jpg");
            }
            else
            {
                Undo.Push("picture\\picture" + (numperpic2 - 1).ToString() + ".jpg");
            }
            //===============================stack
            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Case == 1)
            {

                if (PenCase == 1)
                {

                    pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    if (curPoints.Count > 1) e.Graphics.DrawCurve(pen, curPoints.ToArray());
                }
                else if (PenCase == 2)
                {
                    pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    if (curPoints.Count > 1)
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.DrawCurve(pen, curPoints.ToArray());

                    }
                }
                else if (PenCase == 3)
                {
                    pen = new Pen(pictureBox1 .BackColor , 10) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                    if (curPoints.Count > 1)
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.DrawCurve(pen, curPoints.ToArray());

                    }
                }
                // other lines or curves


                foreach (List<Point> points in allPoints)
                    if (points.Count > 1)
                    {
                        pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                        if (PenCase == 1) e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        else if (PenCase == 2) e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        else if (PenCase == 3)
                        {
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            pen = new Pen(pictureBox1.BackColor, 10) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                        }

                        e.Graphics.DrawCurve(pen, points.ToArray());

                    }
                allPoints.Clear();
            }
            else if(Case ==2)
            {
             if (figer == 1)
            {
                pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                e.Graphics.DrawRectangle(pen , Rect);
            }
            else if (figer == 3)
            {
                pen = new Pen(color, size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse (pen, Rect);
            }
            else if (figer == 2)
            {
                brush = new SolidBrush(Color.FromArgb(color.ToArgb()));
                e.Graphics.FillRectangle (brush , Rect);
            }
            else if (figer == 4)
            {
                brush = new SolidBrush(Color.FromArgb(color.ToArgb()));
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse (brush, Rect);
            }
            }
            else if (Case == 3)
            {
                pen = new Pen(color, size ) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(pen , RectStartPoint, PLine);
            
            }
            
        }
        int PenCase;
        private void penToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PenCase = 2;
            Case = 1;
            Mouse();
        }

        private void pToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenCase = 1;
            Case = 1;
            Mouse();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoRection();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoRection();
        }


    }
}
