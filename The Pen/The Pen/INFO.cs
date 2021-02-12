using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace The_Pen
{
    public partial class INFO : Form
    {
        public INFO()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.Font = new Font("Arial", 13, FontStyle.Bold);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/wilaiashield/");
        }


        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.Font = new Font("Arial", 15, FontStyle.Regular);
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.Font = new Font("Arial", 16, FontStyle.Regular);
        }
    }
}
