using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class DrawForm : Form
    {
        Graphics g;
        int x = -1;
        int y = -1;
        bool movieng = false;
        Pen pen;
        public DrawForm()
        {
          


            InitializeComponent();
            g = this.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias ;
            pen = new Pen(Color.Red , 2);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void DrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode  == Keys.Escape )// هنا في حالة ضعط زر ESC
            {
                Application .Exit ();
                
            }
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            button1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - button1.Width, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height);
            Bitmap B= new Bitmap (new Bitmap (Properties .Resources .Pen ),48,48);// كود تغير شكل الماوس
            this.Cursor =new Cursor (B.GetHicon ());
            button1.Cursor = Cursors.Hand; 
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //احداث الرسم
        //__________________________________________________________________________
        private void DrawForm_MouseUp(object sender, MouseEventArgs e)
        {
            
            //حدث عند رفع يدك من الماوس
            movieng = false;
            x = -1;
            y = -1;
              
        }

        private void DrawForm_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
               
                if (movieng && x != -1 && y != -1)
                {
                    
                    g.DrawLine(pen, new Point(x, y), e.Location);
                    x = e.X;
                    y = e.Y;
                }
            }
            catch (Exception h)
            {

               
            }
           
        }
        //*** مشكلة تناسق اللون مع حجم الخط
        private void DrawForm_MouseDown(object sender, MouseEventArgs e)
        {
            
            //حدث عند الضغط على زر الماوس
            movieng = true;
            x = e.X;
            y = e.Y;
              
        }
        //__________________________________________________________________________
        //لون الخط
        //__________________________________________________________________________
        ColorDialog cd = new ColorDialog();
        private void chToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (cd.ShowDialog() == DialogResult.OK)
            {

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pen = new Pen(cd.Color,2);
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            }
        }
        //__________________________________________________________________________
        //حجم الخط المرسوم
        //____________________________________________________________________________
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int size1 = int.Parse(toolStripMenuItem2.Text);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(cd.Color,size1 );
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int size2 = int.Parse(toolStripMenuItem3.Text);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(cd.Color, size2);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int size3 = int.Parse(toolStripMenuItem4.Text);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(cd.Color, size3);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            int size4 = int.Parse(toolStripMenuItem5.Text);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(cd.Color, size4);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        }
        //____________________________________________________________________________

        private void button1_Click_1(object sender, EventArgs e)
        {

            Application.Exit();
            
           
           
        }

      

      

      
        

       

    
      

       

        
    }
}
