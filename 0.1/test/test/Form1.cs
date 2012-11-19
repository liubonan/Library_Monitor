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


namespace test
{
    public partial class Form1 : Form
    {
        private string filename="config.inf";
        private bool lock_flag = false;

        public Form1()
        {
            InitializeComponent();
            //SystemEvents.PowerModeChanged += OnPowerModeChanged;
        }

        //[DllImport("kernel32.dll")]
        //private static extern int Beep(int dwFreq, int dwDuration);
        
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);




        private void button1_Click(object sender, EventArgs e)
        {           
           lock_flag = true;  

        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
           // while(lock_flag)
           // {
                lock_flag = false;
                //Beep(0X5FF, 1000);
               
            //}
        }

        public String toMD5(String original_String)
        {
            string MD5Code = "";

            MD5CryptoServiceProvider aMD5CSP = new MD5CryptoServiceProvider();

            Byte[] aHashTable = aMD5CSP.ComputeHash(Encoding.ASCII.GetBytes(original_String));
            MD5Code = System.BitConverter.ToString(aHashTable).Replace("-", "");

            return MD5Code;

        }

        private void button3_Click(object sender, EventArgs e)
        { 
             

            //FileStream fs = new FileStream(filename, FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.Write(toMD5(textBox3.Text));
            //sw.Flush();
            //sw.Close();
            //fs.Close();

            //if (File.Exists("cn.mp3"))
              //  MessageBox.Show("Yes");

            mciSendString("close all", null, 0, IntPtr.Zero);
            mciSendString("open cn.mp3", null, 0, IntPtr.Zero);
            mciSendString("play cn.mp3", null, 0, IntPtr.Zero);

            
        }

        private void button2_Click(object sender, EventArgs e)
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

            if (pwd.Equals(toMD5(textBox4.Text)))
                lock_flag = false;
            else
                MessageBox.Show("密码错误！");
        }
        

    }
}
