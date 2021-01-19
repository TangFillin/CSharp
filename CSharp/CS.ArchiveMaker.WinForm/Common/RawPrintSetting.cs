using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Common
{
    [Serializable]
    public class RawPrintSetting
    {
        /// <summary>
        /// 打印机名
        /// </summary>
        public string PrinterName
        {
            get;
            set;
        }

        /// <summary>
        /// 横向打印
        /// </summary>
        public bool LandScape
        {
            get;
            set;
        }

        /// <summary>
        /// 纸张名
        /// </summary>
        public string PaperName
        {
            get;
            set;
        }

        /// <summary>
        /// 纸张大小
        /// </summary>
        public PaperSize PaperSize
        {
            get;
            set;
        }

        /// <summary>
        /// 边距
        /// </summary>
        public Margins PageMargins
        {
            get;
            set;
        }


    }
}
