using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CS.ArchiveMaker.WinForm.Common
{
    public class XMessageBox
    {
        public static DialogResult Info(string message)
        {
            return MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Warning(string message)
        {
            return MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult Error(string message)
        {
            return MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult YesNo(string message)
        {
            return MessageBox.Show(message, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult OKCancel(string message)
        {
            return MessageBox.Show(message, "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult YesNoCancel(string message)
        {
            return MessageBox.Show(message, "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        }

        public static DialogResult RetryCancel(string message)
        {
            return MessageBox.Show(message, "询问", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult AbortRetryIgnore(string message)
        {
            return MessageBox.Show(message, "询问", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        }
    }
}
