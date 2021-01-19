using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using CS.ArchiveMaker.WinForm.Common;
using System.Text;
using System.Diagnostics;

namespace CS.ArchiveMaker.WinForm
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //RegisterApp();

            if (!GenerateRegisterFile())
            {
                return;
            }

            if (args.Length == 0)
            {
                args = new string[1];
                args[0] = "";
                //args[0] = "CSCOOLDE://PrintSetting;2";//打印设置
                //args[0] = "cscoolde://Print;http://localhost:64782/;20210118182736190896;6";//打印
                args[0] = "cscoolde://PrintPreview;http://localhost:64782/;20210118182736190896;6";
                //         
                //XMessageBox.Error("未找到参数");

            }

            if (string.IsNullOrWhiteSpace(args[0]) || !args[0].Contains(";"))
            {
                XMessageBox.Error($"参数不对:{args[0]}");
                return;
            }

            //MessageBox.Show($"{args[0]},长度{args[0].Length}");
            var pars = args[0].Trim().Split(';');

            //多模板打印
            if (pars.Length > 2 && pars[2].Contains('&'))
            {
                string[] batchNumbers = pars[2].Split('&');
                foreach (var bn in batchNumbers)
                {
                    if (string.IsNullOrEmpty(bn))
                    {
                        continue;
                    }
                    string str = bn.Replace(',', ';');
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = Application.ExecutablePath; //启动的应用程序名称
                    startInfo.Arguments = $"{pars[0]};{pars[1]};{str}";
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    Process.Start(startInfo);
                }
                return;
            }
            Form form;

            switch (pars[0].ToLower())
            {
                //打印设置
                case @"cscoolde://printsetting":
                    form = new Form_PrintSetting(pars[1].TrimEnd('/'));
                    break;
                //打印
                case @"cscoolde://print":
                    form = new Form_Main(pars[1], pars[2], pars[3].TrimEnd('/'), false);
                    break;
                //打印预览
                case "cscoolde://printpreview":
                    form = new Form_Main(pars[1], pars[2], pars[3].TrimEnd('/'), true);
                    break;
                default:
                    XMessageBox.Error($"Type:({pars[0]})");
                    return;
            }
            if (form != null)
            {
                Application.Run(form);
            }

        }

        private static void RegisterApp()
        {
            var name = ConfigurationManager.AppSettings["protocolName"];
            string path = Path.Combine(Application.StartupPath, Application.ProductName + ".exe");
            //注册表操作
            var archivePrint = Registry.ClassesRoot.CreateSubKey(name);

            if (archivePrint.SubKeyCount == 0 || !archivePrint.GetValue("URL Protocol").Equals(path))
            {
                //注册项不存在或者应用程序地址改变
                archivePrint.SetValue("", name);
                archivePrint.SetValue("URL Protocol", path);
                //var icon = archivePrint.CreateSubKey("DefaultIcon");
                //icon.SetValue("", path + ",1");
                var command = archivePrint.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
                command.SetValue("", $"\"{path}\" \" %1\"");
            }


        }

        /// <summary>
        /// 产生注册脚本
        /// </summary>
        /// <returns></returns>
        private static bool GenerateRegisterFile()
        {
            try
            {
                var name = ConfigurationManager.AppSettings["ProtocolName"];
                var appPath = ConfigurationManager.AppSettings["AppPath"];
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                //判断协议是否已注册
                if (Registry.ClassesRoot.GetSubKeyNames().FirstOrDefault(n => n.Equals(name)) == null || string.IsNullOrEmpty(appPath) || !appPath.Equals(baseDir))
                {
                    var filePath = Path.Combine(baseDir, "注册应用.reg");
                    var appName = Application.ExecutablePath;
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        using (var st = new StreamWriter(stream, Encoding.GetEncoding("gb2312")))
                        {
                            StringBuilder content = new StringBuilder();
                            content.AppendLine("Windows Registry Editor Version 5.00");
                            content.AppendLine($"[HKEY_CLASSES_ROOT\\{name}]");
                            content.AppendLine($"@= \"{name}\"");
                            content.AppendLine($"\"URL Protocol\" = \"{appName.Replace("\\", "\\\\")}\"");
                            content.AppendLine($"[HKEY_CLASSES_ROOT\\{name}\\shell]");
                            content.AppendLine($"[HKEY_CLASSES_ROOT\\{name}\\shell\\open]");
                            content.AppendLine($"[HKEY_CLASSES_ROOT\\{name}\\shell\\open\\command]");
                            content.AppendLine($"@=\"\\\"{appName.Replace("\\", "\\\\")}\\\" \\\" %1\\\"\"");
                            st.Write(content.ToString());
                        }
                    }
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["AppPath"].Value = baseDir;
                    config.Save(ConfigurationSaveMode.Modified);

                    //System.Diagnostics.Process.Start(filePath);
                    if (File.Exists(filePath))
                    {
                        Process.Start("regedit", string.Format(" /s {0}", @"""" + filePath + @""""));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                XMessageBox.Error(ex.Message);
                return false;
            }
        }
    }
}
