using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCustomControls.ComboxMulti
{
    /// <summary>
    /// 下拉按钮
    /// </summary>
    public class DropdownButton : Button
    {

        public DropdownButton()
        {
            //防止重绘控件闪烁
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        ButtonState State { get; set; }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            State = ButtonState.Pushed;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            State = ButtonState.Normal;
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            ControlPaint.DrawComboButton(pevent.Graphics, 0, 0, this.Width, this.Height, State);
        }
    }
}
