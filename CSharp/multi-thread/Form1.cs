using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multi_thread
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool flag = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if(flag == false)
            {
                flag = true;
                button1.Text = "停止";
                Thread thread = new Thread(RandNumber);
                thread.IsBackground = true;
                thread.Start();
                //thread.Start(parameter);
            }
            else
            {
                flag = false;
                button1.Text = "停止";
            }
        }
        private void RandNumber()
        {
            Random random = new Random();
            while (flag)
            {
                this.label1.Text = random.Next(0, 10).ToString();
                this.label2.Text = random.Next(0, 10).ToString();
                this.label3.Text = random.Next(0, 10).ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //不检查跨线程操作
            CheckForIllegalCrossThreadCalls = false;
        }
    }
}
