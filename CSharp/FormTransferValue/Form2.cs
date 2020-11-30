using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FormTransferValue
{
    public delegate void Transfer(string value);
    public partial class Form2 : Form
    {
        public Transfer Transfer;
        public Form2(Transfer transfer)
        {
            InitializeComponent();
            Transfer = transfer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text.Trim();
            Transfer(str);
            this.Close();
        }
    }
}
