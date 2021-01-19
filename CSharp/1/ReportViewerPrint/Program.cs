using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportViewerPrint
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //获取欲启动进程名
            string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            ////获取版本号 
            //CommonData.VersionNumber = Application.ProductVersion; 
            //检查进程是否已经启动，已经启动则显示报错信息退出程序。 
            if (System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1)
            {
                //SendCopyDataMessage(p.MainWindowHandle, Args[0]); //发送消息                  
                
                //MessageBox.Show("打印客户端已启动！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Print(args.ToList()));
        }
    }
}
