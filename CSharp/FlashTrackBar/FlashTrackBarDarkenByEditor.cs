using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashTrackBar
{
    public class FlashTrackBarDarkenByEditor : FlashTrackBarValueEditor
    {
        protected override void SetEditorProps(FlashTrackerBar editingInstance, FlashTrackerBar editor)
        {
            editor.Min = 0;
            editor.Max = byte.MaxValue;
        }
    }
}
