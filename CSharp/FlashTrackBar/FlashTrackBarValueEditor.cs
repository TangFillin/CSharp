using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace FlashTrackBar
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class FlashTrackBarValueEditor : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService edSv = null;

        protected virtual void SetEditorProps(FlashTrackerBar editingInstance, FlashTrackerBar editor)
        {
            editor.ShowValue = true;
            editor.StartColor = Color.Navy;
            editor.EndColor = Color.White;
            editor.ForeColor = Color.White;
            editor.Min = editingInstance.Min;
            editor.Max = editingInstance.Max;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                edSv = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSv != null)
                {
                    FlashTrackerBar trackBar = new FlashTrackerBar();
                    trackBar.ValueChanged += new EventHandler(this.ValueChanged);
                    SetEditorProps((FlashTrackerBar)context.Instance, trackBar);
                    bool asInt = true;
                    if (value is int)
                    {
                        trackBar.Value = (int)value;
                    }
                    else if (value is byte)
                    {
                        asInt = false;
                        trackBar.Value = (byte)value;
                    }
                    edSv.DropDownControl(trackBar);
                    if (asInt)
                    {
                        value = trackBar.Value;
                    }
                    else
                    {
                        value = (byte)trackBar.Value;
                    }
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }
        private void ValueChanged(object sender, EventArgs e)
        {
            if (edSv != null)
            {
                edSv.CloseDropDown();
            }
        }
    }
}
