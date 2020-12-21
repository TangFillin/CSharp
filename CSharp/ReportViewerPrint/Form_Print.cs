using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using ReportViewerPrint.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportViewerPrint
{
    public partial class Form_Print : Form
    {
        public Form_Print(List<string> args)
        {
            InitializeComponent();
            string param = args.Count == 0 ? "未传参" : args[0];
            string path = Path.Combine(Application.StartupPath, Application.ProductName + ".exe");
            //注册表操作
            var archivePrint = Registry.ClassesRoot.CreateSubKey("ArchivePrint");

            if (archivePrint.SubKeyCount == 0 || !archivePrint.GetValue("URL Protocol").Equals(path))
            {
                //注册项不存在或者应用程序地址改变
                archivePrint.SetValue("", "ArchivePrint");
                archivePrint.SetValue("URL Protocol", path);
                var icon = archivePrint.CreateSubKey("DefaultIcon");
                icon.SetValue("", path + ",1");
                var command = archivePrint.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
                command.SetValue("", $"\"{path}\" \" %1\"");
            }

            try
            {
                using (FileStream file = new FileStream("1.txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter strem = new StreamWriter(file))
                    {
                        args.ForEach(item =>
                        {
                            strem.WriteLine(item);
                        });
                    }
                }



                textBox1.Text = $"{param.Length}-{param}";


                //获取打印机
                foreach (var item in PrinterSettings.InstalledPrinters)
                {
                    comboBox1.Items.Add(item.ToString());
                }
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }


        public void Print()
        {
            

            

        }

        private void Form_Print_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = Path.Combine(Environment.CurrentDirectory, "PrintModel/Report1.rdlc");
            var data = new List<User>() { new User() { Age = 11, LoginName = "fillin", UserName = "tfl", Sex = Sex.Male } };
            //设置数据源
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", data));
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            Print();
        }
    }
}
