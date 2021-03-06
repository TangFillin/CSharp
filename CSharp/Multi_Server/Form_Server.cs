﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Multi_Server
{
    public partial class Form_Server : Form
    {
        public Form_Server()
        {
            InitializeComponent();
        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            if (btn_listen.Text == "开始监听")
            {
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Any;
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txt_port.Text.Trim()));
                socketWatch.Bind(point);
                ShowMessage("开始监听");
                socketWatch.Listen(10);
                btn_listen.Text = "停止监听";
                Thread thread = new Thread(Listen)
                {
                    IsBackground = true
                };
                thread.Start(socketWatch);
            }
            else
            {
                btn_listen.Text = "开始监听";
            }

        }
        List<Socket> Sockets = new List<Socket>();
        /// <summary>
        /// 等待客户端连接，并创建与之通信的Socket
        /// </summary>
        /// <param name="o"></param>
        void Listen(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                Socket socketSend = socket.Accept();
                ShowMessage(socketSend.RemoteEndPoint.ToString() + "上线了！");
                Sockets.Add(socketSend);
                //开启线程接收客户端消息
                Thread thread = new Thread(RecieveMessage);
                thread.IsBackground = true;
                thread.Start(socketSend);
            }
        }
        void RecieveMessage(object o)
        {
            Socket socket = o as Socket;
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    //实际接收到的有效字节数长度
                    int r = socket.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMessage(socket.RemoteEndPoint + ":" + str);
                    foreach (var s in Sockets)
                    {
                        if (!s.RemoteEndPoint.Equals(socket.RemoteEndPoint))
                        {
                            str = socket.RemoteEndPoint + ":" + str;
                            
                            s.Send(Encoding.UTF8.GetBytes(str));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                }

            }
        }
        /// <summary>
        /// 显示消息到窗口
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message)
        {
            txt_message.Text += message + "\r\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btn_sendMessage_Click(object sender, EventArgs e)
        {
            string message = txt_sendMessage.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes("192.168.1.4:50000" + message);
            foreach (var socket in Sockets)
            {
                socket.Send(buffer);
            }
            txt_sendMessage.Clear();
        }
    }
}
