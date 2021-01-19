using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportViewerPrint.Model
{
    public class User
    {
        public string UserName { get; set; }

        public string LoginName { get; set; }
        public int Age { get; set; }

        public Sex Sex { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}
