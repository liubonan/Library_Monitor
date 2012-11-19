using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using System.Collections;

namespace LibAlarm
{
    public partial class Form1 : Form
    {
        private bool locked = false;
        private bool setpwd = false;
        private string filename = "config.inf";

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
        }

        private const int MM_MCINOTIFY = 0x3B9;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == MM_MCINOTIFY && locked)
            {
                mciSendString("close all", null, 0, IntPtr.Zero);
                mciSendString("open cn.mp3", null, 0, IntPtr.Zero);
                mciSendString("play cn.mp3 notify", null, 0, this.Handle);
            }
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if(locked)
            {
                mciSendString("close all", null, 0, IntPtr.Zero);
                mciSendString("open cn.mp3", null, 0, IntPtr.Zero);
                mciSendString("play cn.mp3 notify", null, 0, this.Handle);
            }
        }

        public String toMD5(String original_String)
        {
            string MD5Code = "";

            MD5CryptoServiceProvider aMD5CSP = new MD5CryptoServiceProvider();

            Byte[] aHashTable = aMD5CSP.ComputeHash(Encoding.ASCII.GetBytes(original_String));
            MD5Code = System.BitConverter.ToString(aHashTable).Replace("-", "");

            return MD5Code;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!locked)
            {
                locked = true;
                this.textBox1.Visible = true;
                this.label2.Visible = true;
                this.label2.Text = "解锁密码：";
                this.button2.Text = "解除锁定";
                this.button1.Enabled = false;
                this.button3.Enabled = false;
                this.textBox1.Text = null;
                this.textBox2.Text = null;
                this.textBox3.Text = null;

            }
            else 
            {
                StreamReader objReader = new StreamReader(filename);
                string sLine="";
                ArrayList LineList = new ArrayList();    
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null&&!sLine.Equals(""))
                    LineList.Add(sLine);
                }
                objReader.Close();

                string pwd = string.Join(",", (string[])LineList.ToArray(typeof(string)));

                if (!pwd.Equals(toMD5(textBox1.Text)))
                {
                    MessageBox.Show("密码输入错误，请重新输入！");
                    textBox1.Text = "";
                }
                else
                {
                    mciSendString("close all", null, 0, IntPtr.Zero);
                    locked = false;
                    this.textBox1.Visible = false;
                    this.button2.Text = "锁定电脑";
                    this.button1.Enabled = true;
                    this.label2.Visible = false;
                    this.button3.Enabled = true;
                }
                this.textBox1.Text = null;
                this.textBox2.Text = null;
                this.textBox3.Text = null;
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!setpwd)
            {
                setpwd = true;
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
                this.textBox3.Visible = true;
                this.label3.Text = "旧密码(初始密码admin)：";
                this.label2.Text = "新密码：";
                this.label4.Text = "重复输入：";
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label4.Visible = true;
                this.button1.Text = "提交修改";
                this.button2.Enabled = false;
                this.textBox1.Text = null;
                this.textBox2.Text = null;
                this.textBox3.Text = null;
                this.button3.Enabled = false;
                this.button4.Visible = true;
            }
            else
            {
                StreamReader objReader = new StreamReader(filename);
                string sLine="";
                ArrayList LineList = new ArrayList();    
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null&&!sLine.Equals(""))
                    LineList.Add(sLine);
                }
                objReader.Close();

                string pwd = string.Join(",", (string[])LineList.ToArray(typeof(string)));

                if (!pwd.Equals(toMD5(textBox2.Text)))
                {
                    MessageBox.Show("旧密码输入错误，请重新输入！");
                }
                else
                {
                    if (!textBox1.Text.Equals(textBox3.Text))
                    {
                        MessageBox.Show("两次新密码输入不一致，请重新输入！");
                    }
                    else
                    {
                        setpwd = false;
                        this.textBox1.Visible = false;
                        this.textBox2.Visible = false;
                        this.textBox3.Visible = false;
                        this.label2.Visible = false;
                        this.label3.Visible = false;
                        this.label4.Visible = false;
                        this.button1.Text = "设定密码";
                        this.button2.Enabled = true;
                        this.button3.Enabled = true;
                        this.button4.Visible = false;

                        FileStream fs = new FileStream(filename, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(toMD5(textBox3.Text));
                        sw.Flush();
                        sw.Close();
                        fs.Close();


                    }
                }

                this.textBox1.Text = null;
                this.textBox2.Text = null;
                this.textBox3.Text = null;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            setpwd = false;
            this.textBox1.Visible = false;
            this.textBox2.Visible = false;
            this.textBox3.Visible = false;
            this.label2.Visible = false;
            this.label3.Visible = false;
            this.label4.Visible = false;
            this.button1.Text = "设定密码";
            this.button2.Enabled = true;
            this.button3.Enabled = true;

            this.textBox1.Text = null;
            this.textBox2.Text = null;
            this.textBox3.Text = null;

            this.button4.Visible = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.liubonan.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:liubonan@liubonan.com");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.liubonan.info");
        }
    }
}
