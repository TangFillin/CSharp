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
        public Form1(string[] args)
        {
            InitializeComponent();
            
            StringBuilder stringBuilder = new StringBuilder();
            if(args.Length==0)
            {
                MessageBox.Show("未找到参数");
            }
            args.ToList().ForEach(item =>
            {
                stringBuilder.AppendLine(item);
                MessageBox.Show(item);
            });
            textBox1.Text = stringBuilder.ToString();
        }
        
        private bool flag = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if(flag == false)
            {
                flag = true;
                button1.Text = "停止";
                Thread thread = new Thread(RandNumber);
                //设置为后台线程
                thread.IsBackground = true;
                thread.Start();
                //传参数给线程
                //thread.Start(parameter);
            }
            else
            {
                flag = false;
                button1.Text = "停止";
            }
        }
        /// <summary>
        /// 
        /// <paramref name="flag"/>
        /// </summary>
        //private void RandNumber(object flag)
        private void RandNumber()
        {
            //var b = (bool)flag;
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
