using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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



                textBox1.Text = $"{args[0].Length}-{args[0]}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

    }
}
