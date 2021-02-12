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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;


namespace Print1111
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Stack <string> Undo = new Stack <string >();
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
                strUndoPop = "picture.jpg";
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
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen), 48, 48);
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 1 && PenCase == 2)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.Pen12), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 3)
            {
                Bitmap B = new Bitmap(new Bitmap(Properties.Resources.highlighter), 48, 48);// كود تغير شكل الماوس
                this.Cursor = new Cursor(B.GetHicon());
            }
            else if (Case == 4)
            {
                this.Cursor = Cursors.NoMove2D;
            }
            else if (Case == 5|| Case ==2)
            {
                this.Cursor = Cursors.Cross;
            }
        }
        public void SaveData()//حفظ الداتا
        {
            try
            {
                    StreamWriter SW = new StreamWriter("Data");
                    SW.Write(size+";");
                    SW.Write(PenCase+";");
                    SW.Write(Case+";");
                    SW.Write(color.R+";");
                    SW.Write(color.G+";");
                    SW.Write(color.B+";");
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

                if (File.Exists("Data"))
                {
                    StreamReader SR = new StreamReader("Data");
                    string str = SR.ReadToEnd();
                    string[] Data = str.Split(';');
                    size = int.Parse(Data[0]);
                    PenCase = int.Parse(Data[1]);
                    Case = int.Parse(Data[2]);
                    color = Color.FromArgb(int.Parse(Data[3]), int.Parse(Data[4]), int.Parse(Data[5]));
                    color2 = Color.FromArgb(int.Parse(Data[6]), int.Parse(Data[7]), int.Parse(Data[8]));
                    SR.Close();
                    Mouse();
                }
                else
                {
                    color = Color.Red;
                    color2 = Color.GreenYellow;
                    size = 2;
                    PenCase = 1;
                    Case = 1;
                }
            }
            catch (Exception Exp)
            {
                color = Color.Red;
                color2 = Color.GreenYellow;
                size = 2;
                PenCase = 1;
                Case = 1;
            }
        }
        //================================هاي لايت
        [DllImport("gdi32.dll")]
        static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDst, int x1, int y1, int cx, int cy,
                                         IntPtr hdcSrc, int x2, int y2, int rop);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern bool DeleteDC(IntPtr hdc);

        private const int SRCCOPY = 0x00CC0020;

        private const int PS_SOLID = 0;
        private const int R2_MASKPEN = 9;
        private const int R2_COPYPEN = 13;
        List<Point> points = new List<Point>();

        private void DrawHighlight(Point[] usePoints, int brushSize, Color brushColor)
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            using (Graphics gBMP = Graphics.FromImage(bmp))
            {
                IntPtr hBMP = bmp.GetHbitmap();
                IntPtr bDC = gBMP.GetHdc();
                IntPtr mDC = CreateCompatibleDC(bDC);
                IntPtr oDC = SelectObject(mDC, hBMP);

                int useColor = System.Drawing.ColorTranslator.ToWin32(brushColor);
                IntPtr pen = CreatePen(PS_SOLID, brushSize, (uint)useColor);
                IntPtr xDC = SelectObject(mDC, pen);

                SetROP2(mDC, R2_MASKPEN);
                Point p1 = new Point(0, 0);
                Point p2 = new Point(0, 0);
                for (int i = 1; i <= usePoints.Length - 1; i++)
                {
                    p1 = usePoints[i - 1];
                    p2 = usePoints[i];

                }
                MoveToEx(mDC, p1.X, p1.Y, IntPtr.Zero);
                LineTo(mDC, p2.X, p2.Y);
                SetROP2(mDC, R2_COPYPEN);

                BitBlt(bDC, 0, 0, bmp.Width, bmp.Height, mDC, 0, 0, SRCCOPY);
                SelectObject(mDC, xDC);
                DeleteObject(pen);
                gBMP.ReleaseHdc(bDC);
                SelectObject(mDC, oDC);
                DeleteDC(mDC);
                DeleteObject(hBMP);
            }
        }
        //================================هاي لايت

        Graphics g;
        Point sp = new Point(0, 0);
        Point ep = new Point(0, 0);
        int k = 0;
        private int cX, cY, dX, dY;
        Pen pen;
        Color color;
        Color color2;
        int size;
        int Case;
        //================================
        List<Point> curPoints = new List<Point>();
        List<List<Point>> allPoints = new List<List<Point>>();
        Point PLine = new Point();
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            Mouse();
                button1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - button1.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height);
                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                this.TopMost = true;
                CaptureScreen();
                if (File.Exists("picture.jpg"))
                {
                    File.Delete("picture.jpg");
                    Bitmap b = new Bitmap(memoryImage, pictureBox1.Width, pictureBox1.Height);
                    b.Save("picture.jpg");
                    pictureBox1.BackgroundImage = Image.FromFile("picture.jpg");
                }
                else
                {
                    Bitmap b = new Bitmap(memoryImage, pictureBox1.Width, pictureBox1.Height);
                    b.Save("picture.jpg");
                    pictureBox1.BackgroundImage = Image.FromFile("picture.jpg");
                }

                pictureBox1.Image = Image.FromFile("picture.jpg");
                g = pictureBox1.CreateGraphics();
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//هذا الشرط لضمان عدم عمل الشروط الاسفل في حالة الضغط على الزر الايمن
            {
                
                if (Case == 3)
                {
                    if (k == 0)
                    {
                        dX = e.X - cX;
                        dY = e.Y - cY;
                    }
                }
                
            }
            
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
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
                    if (k == 1)
                    {
                        ep = e.Location;
                        points.Add(sp);
                        points.Add(ep);
                        g.DrawImage(pictureBox1.Image, Point.Empty);
                        DrawHighlight(points.ToArray(), 16, color2 );
                        sp = ep;

                    }
                }
                else if (Case == 5)
                {
                    if (e.Button != MouseButtons.Left)
                        return;
                    PLine = e.Location;
                    pictureBox1.Invalidate();
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
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
                else if (Case == 2||Case ==5)
                {
                    RectStartPoint = e.Location;
                }
                else if (Case == 3)
                {
                    sp = e.Location;

                    if (e.Button == MouseButtons.Left)
                    {
                        k = 1;
                    }
                }
                else if (Case == 4)
                {
                    
                    P1.X = e.X - 15;
                    P1.Y = e.Y - 15;

                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Case == 1 ||Case == 2 )
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
                else if (Case == 3||Case ==5)
                {
                    k = 0;
                    DrawOnImag();
                }
                else if (Case == 4)
                {
                    
                        stopdraw += 1;
                    
                    if (stopdraw % 2 != 0)
                    {
                        return;
                    }
                    else { DrawOnImag(); };
                }

            }
        }
        // الرسم على الصور _____________________
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // لون الخط___________________________
        public void ColorFunc(Color c)//دالة تغيير اللون
        {
            if (Case == 3)
            {
                color2 = c;
            }
            else color = c;
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
                if (Case == 3) color2 = Cd.Color;
                else color = Cd.Color;
                
            }
        }
        // لون الخط________________________
        //حجم الخط_____________________

        public void SizeFunc(string s)//دالة الحجم
        {
            size =int.Parse ( s);
            s = toolStripMenuItem13.Text;
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            SizeFunc(((ToolStripMenuItem)sender).Text);
        }

        //حجم الخط_______________________
        private void toolStripMenuItem4_Click(object sender, EventArgs e)//حفظ الصورة
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
                Case = 2;
                Mouse();
                
        }

        //المربع_____________
        private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        //المربع________________

        //التقاط صورة متجاوبة HD
        private static class Win32Native
        {
            public const int DESKTOPVERTRES = 0x75;
            public const int DESKTOPHORZRES = 0x76;

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hDC, int index);
        }
        Bitmap memoryImage;
        private void CaptureScreen()
        {
            int width, height;
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hDC = g.GetHdc();
                width = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPHORZRES);
                height = Win32Native.GetDeviceCaps(hDC, Win32Native.DESKTOPVERTRES);
                g.ReleaseHdc(hDC);
            }
            int y = this.Top;
            this.Top = Screen.PrimaryScreen.Bounds.Height + 1000;
            Graphics myGraphics = this.CreateGraphics();
            Size s = new Size(width, height);
            memoryImage = new Bitmap(width, height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Left, Screen.PrimaryScreen.Bounds.X, 0, 0, s);
            this.Top = y;
        }
        //التقاط صورة HD
        int PenCase ;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = int.Parse(toolStripComboBox1.Text);
        }


        static int numperpic = 0;// هذه العملية للرسم بدون ان يحذف بقية الرسومات
        Bitmap b;
        int stopdraw = 1;
        public void DrawOnImag()
        {
            
            Directory.CreateDirectory("picture");
            numperpic = numperpic + 1;
            b = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Width);
            pictureBox1.DrawToBitmap(b, pictureBox1.ClientRectangle);
            b.Save("picture\\picture" + numperpic.ToString() + ".jpg");
            pictureBox1.Image = Image.FromFile("picture\\picture" + numperpic.ToString() + ".jpg");
            if (Case == 4)
            {
                stopdraw += 1;
            }
            //===============================stack
            int numperpic2 = numperpic;
            if (Undo.Count<1)
            {
                Undo.Push("picture.jpg");
                if (numperpic2-1==0)
                {
                    numperpic2 += 1;
                }
                Undo.Push("picture\\picture" + (numperpic2 -1).ToString () + ".jpg");
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
                    if (curPoints.Count > 1) e.Graphics.DrawCurve(pen  , curPoints.ToArray());
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
                    // other lines or curves
                
                
                foreach (List<Point> points in allPoints)
                    if (points.Count > 1)
                    {
                        pen = new Pen(color , size) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                        if (PenCase == 1) e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        else if (PenCase == 2) e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.DrawCurve(pen, points.ToArray());
                        
                    }
                allPoints.Clear();
            }
            else if (Case == 2)
            {
                Pen p = new Pen(Color.Red , 3);
                e.Graphics.DrawRectangle (p, Rect);
            }
            else if (Case == 4)
            {
                if (NumEllipse < 100)
                {
                    if (stopdraw % 2 == 0)
                    {
                        
                        string str = NumEllipse.ToString();
                        Font f = new Font("arial", 15, FontStyle.Bold);
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.FillEllipse(Brushes.Blue, P1.X, P1.Y, 30, 30);
                        e.Graphics.DrawEllipse(Pens.White, P1.X, P1.Y, 28, 28);
                        if (str.Length == 1) P1.X = P1.X + 6;
                        else P1.X = P1.X + 1;
                        e.Graphics.DrawString(str, f, Brushes.White, P1.X, P1.Y + 3);
                        NumEllipse += 1;
                        
                    }
                    else return;
                    
                }
                else NumEllipse = 1;
            }
            else if (Case == 5)
            {
                pen = new Pen(Color .Red , 8) { StartCap = System.Drawing.Drawing2D.LineCap.Round };
                pen.EndCap  = LineCap.ArrowAnchor;
                pen.StartCap  = LineCap.RoundAnchor;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(pen, RectStartPoint, PLine);

            }
            
        }
        int NumEllipse = 1;
        Point P1 = new Point();
        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Case = 4;
            Mouse();
        }

        private void arrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case = 5;
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

        private void pencilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenCase = 1;
            Case = 1;
            Mouse();
        }

        private void penToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PenCase = 2;
            Case = 1;
            Mouse();
        }

        private void highlighterPenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case = 3;
            Mouse();
        }

   
    }
}
