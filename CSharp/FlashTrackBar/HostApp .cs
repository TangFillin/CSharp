using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashTrackBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.RunWorkerAsync();
           
        }

        BackgroundWorker worker = new BackgroundWorker();
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            flashTrackerBar1.Value += 1;
            if (flashTrackerBar1.Value > 100)
            {
                flashTrackerBar1.Value = 1;
            }
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            while (count++ < 10000 * 10000) ;
        }
    }
}
