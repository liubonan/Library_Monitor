using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lib_Monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "EmeraldColor1.ssk";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要退出本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            } 
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = true;
        }
    }
}
