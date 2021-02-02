using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCustomControls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 50; i++)
            {
                comboxMulti1.AddItems(i);
            }

        }
        /// <summary>
        /// 获取选中项的文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comCheckBoxList1_ItemClick(object sender, ItemCheckEventArgs e)
        {
            string text = comboxMulti1.GetItemText(comboxMulti1.Items[e.Index]);
            MessageBox.Show(text);
        }
    }
}
