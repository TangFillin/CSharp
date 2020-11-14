using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Client
{
    public partial class Form_Client : Form
    {
        public Form_Client()
        {
            InitializeComponent();
        }
        Socket socket;
        private void btn_connect_Click(object sender, EventArgs e)
        {
            string ip = txt_IPAddress.Text.Trim();
            string port = txt_port.Text.Trim();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(ipaddress, Convert.ToInt32(port));
            socket.Connect(point);
            ShowMessage("连接成功");

            Thread thread = new Thread(Recive);
            thread.IsBackground = true;
            thread.Start();
        }

        void ShowMessage(string message)
        {
            txt_message.Text += message + "\r\n";
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string message = txt_send.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer);
            ShowMessage("我：" + message);
            txt_send.Clear();
        }

        void Recive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int r = socket.Receive(buffer);
                if (r == 0)
                {
                    break;
                }
                string message = Encoding.UTF8.GetString(buffer, 0, r);
                ShowMessage(message);
            }
        }

        private void Form_Client_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
    }
}
