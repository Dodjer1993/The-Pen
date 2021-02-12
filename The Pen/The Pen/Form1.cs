using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using WindowAPITutorial;

namespace The_Pen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool close = false;
        //مفتاح الاختصار
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        private IntPtr thisWindow;
        private Hotkey hotkey;
        //مفتاح الاختصار
        public void ShowPen()
        {
            int x = this.Top;
            this.Top = Screen.PrimaryScreen.Bounds.Height + 1000;

            string red = System.IO.Path.GetTempPath() + "Pen.exe"; // استدعاء ملف تنفيذي من الريسروس
            System.IO.File.WriteAllBytes(red, Properties.Resources.Pen);
            System.Diagnostics.Process.Start(red);
            System.Threading.Thread.Sleep(1000);
            this.Top = x;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ShowPen();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon1.ShowBalloonTip(100, "The Pen", "Your program is still running", ToolTipIcon.Info);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            close = true;
            Application.Exit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            INFO I = new INFO();
            I.ShowDialog();
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon1, null);
            }
           







        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources.pen1; 
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties .Resources .pen0  ;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackgroundImage = Properties.Resources.info1 ;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackgroundImage = Properties.Resources.info0  ;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.shutdown_96px ;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.Exit0 ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            string red = System.IO.Path.GetTempPath() + "Draw1.exe"; // استدعاء ملف تنفيذي من الريسروس
            System.IO.File.WriteAllBytes(red, Properties.Resources.Draw1 );
            System.Diagnostics.Process.Start(red);
            
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackgroundImage = Properties.Resources.Draw11;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackgroundImage = Properties.Resources.Draw;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Magnifying M = new Magnifying();
            M.Owner = this;
            M.Show();
            this.Hide();
            
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.BackgroundImage = Properties.Resources.Zoom2;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackgroundImage = Properties.Resources.Zoom1;
        }
        //مفتاح الاختصار
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            hotkey.UnRegisterHotKeys();
        }

        protected override void WndProc(ref Message keyPressed)
        {
            if (keyPressed.Msg == 0x0312)
            {
                IntPtr hotkeyID = keyPressed.WParam;
                ShowPen();
            }
            base.WndProc(ref keyPressed);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            thisWindow = FindWindow(null, "Window tool");
            hotkey = new Hotkey(thisWindow);
            hotkey.RegisterHotKeys();
            
        }
        //مفتاح الاختصار
     }
}
