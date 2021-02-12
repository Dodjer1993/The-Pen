using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }

     
        //التقاط صورة
        Bitmap memoryImage;
        private void CaptureScreen()
        {
            int y = this.Top;
            this.Top = Screen.PrimaryScreen.Bounds.Height + 1000;
            // كود التقاط صورة
            Graphics myGraphics = this.CreateGraphics();
            Size s = new Size (Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            memoryImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height , myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.Left , Screen.PrimaryScreen.Bounds.X , 0, 0, s);
            //____________________
            this.Top = y;
        }
        //الفورم الخاص بالرسم
       public  DrawForm D = new DrawForm();
        private void SetDrawForm()
        {
   
            D.FormBorderStyle = FormBorderStyle.None;
            D.Bounds = Screen.PrimaryScreen.Bounds;
            D.TopMost = true;
            D.BackgroundImage = Image.FromFile("picture.jpg");
            Application.EnableVisualStyles();


 

            D.ShowDialog();
        }
        //زر الاحداث
        private void button3_Click(object sender, EventArgs e)
        {
           
           
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;

            CaptureScreen();
            if (File.Exists("picture.jpg"))
            {
                File.Delete("picture.jpg");
                memoryImage.Save("picture.jpg");
                SetDrawForm();
            }
            else
            {
                memoryImage.Save("picture.jpg");
                SetDrawForm();
            }
        }

       

       
       

      

      
       
    }
}
