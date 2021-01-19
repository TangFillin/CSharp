using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Common
{
    [Serializable]
    public class ComoboTextValueModel
    {
        public string Text { get; set; }

        public object Value { get; set; }

        public ComoboTextValueModel(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public ComoboTextValueModel()
        {

        }

        public override string ToString()
        {
            return Text;
        }
    }
}
