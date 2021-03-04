using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fillin.Common.SDK
{
    /// <summary>
    /// 空间拖动操作
    /// </summary>
    public class DragControl
    {
        private Control control;
        /// <summary>
        /// 待拖动控件
        /// </summary>
        private Control Control
        {
            get
            {
                return control;
            }
            set
            {
                control = value;
            }
        }

        private Control header;
        private Control Header
        {
            set
            {
                header = value;
                header.MouseDown += Control_MouseDown;
                header.MouseMove += Control_MouseMove;
            }
            get
            {
                return header;
            }
        }

        private int X;
        private int Y;

        public DragControl()
        {

        }

        public DragControl(Control control)
        {
            this.Control = control;
            this.Header = control;
        }

        /// <summary>
        /// 拖动标题栏移动窗体
        /// </summary>
        /// <param name="form"></param>
        /// <param name="header"></param>
        public DragControl(Control form, Control header)
        {
            Control = form;
            Header = header;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X - X;
                int y = e.Y - Y;
                this.Control.Left += x;
                this.Control.Top += y;
            }
        }



        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }

    }
}
