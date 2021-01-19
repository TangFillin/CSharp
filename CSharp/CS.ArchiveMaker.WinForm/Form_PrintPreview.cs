using CS.ArchiveMaker.WinForm.Common;
using CS.ArchiveMaker.WinForm.Models;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CS.ArchiveMaker.WinForm
{
    public partial class Form_PrintPreview : Form
    {
        private Configuration Config = null;
        private ReportViewer MyReportViewer = null;
        private PrintDataModel PrintData = null;

        public Form_PrintPreview(PrintDataModel printData, Configuration config, int index)
        {
            InitializeComponent();
            MyReportViewer = new ReportViewer
            {
                Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right,
                Margin = new Padding(10),
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(780, 600),

            };

            MyReportViewer.LocalReport.EnableExternalImages = true;
            this.Controls.Add(MyReportViewer);

            Config = config;
            PrintData = printData;

            InitData(index);

        }

        private void InitData(int index)
        {
            PageSettings ps = Config.GetPageSettings();
            MyReportViewer.SetPageSettings(ps);
            MyReportViewer.LocalReport.ReportPath = Config.TemplateLocation(PrintData.PrintTemplate.FileName);

            Dictionary<string, object> item = new Dictionary<string, object>();
            if (PrintData != null && PrintData.Data.Count > 0)
            {
                item = PrintData.Data[index];
            }
            else
            {
                XMessageBox.Warning("未加载到数据");
                return;
            }

            MyReportViewer.LocalReport.DataSources.Clear();
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource(PrintData.PrintTemplate.DataSetName, item.ToDataTable()));
            var printParams = PrintData.PrintTemplate.PrintParams;
            if (printParams.Count > 0)
            {
                int j = 0;
                foreach (var parameter in MyReportViewer.LocalReport.GetParameters())
                {
                    var db_param = PrintData.PrintTemplate.PrintParams.Find(p => p.PrintParamName == parameter.Name);
                    string fileName = $"{PrintData.PrintTemplate.PrintTemplateID}_{db_param.Value}{index}{j++}";

                    switch (db_param.ParamType)
                    {
                        case ParamType.BarCode:
                            //创建条形码文件
                            fileName += "1D";
                            string value = CanCreateCode(db_param, item, out bool isCreate);

                            if (isCreate == false)
                            {
                                XMessageBox.Error($"值({value})不能生成条形码");
                            }

                            PrintReport.CreateBarCode(value, fileName, Config.TempleAddress);
                            break;
                        case ParamType.QRCode:
                            //创建二维码
                            fileName += "2D";
                            value = CanCreateCode(db_param, item, out isCreate);

                            if (isCreate == false)
                            {
                                XMessageBox.Error($"值({value})不能生成条形码");
                            }
                            PrintReport.CreateQRCode(value, fileName, Config.TempleAddress);
                            break;
                        case ParamType.Other:

                            MyReportViewer.LocalReport.SetParameters(new ReportParameter
                                (parameter.Name, Convert.ToString(item[db_param.Value])));
                            continue;
                        case ParamType.SQL:
                            value = "0";
                            if (item.ContainsKey(db_param.Value))
                            {
                                value = Convert.ToString(item[db_param.Value]);
                            }
                            MyReportViewer.LocalReport.SetParameters(new ReportParameter
                                (parameter.Name, value));
                            continue;
                    }
                    var file = Path.Combine(Config.TempleAddress, fileName + ".bmp");
                    MyReportViewer.LocalReport.SetParameters(new ReportParameter(parameter.Name, "file://" + file));
                }
            }
            MyReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = ZoomMode.Percent;
            MyReportViewer.ZoomPercent = 100;
        }
        private string CanCreateCode(PrintParamModel db_param, Dictionary<string, object> item, out bool IsCreate)
        {
            string value = "";
            IsCreate = true;
            if (db_param.ValueSource.Equals(ValueSource.Function))
            {
                value = PrintData.FunValue[db_param.PrintParamName];
            }
            else
            {
                value = Convert.ToString(item[db_param.Value]);
            }
            //判断是否包含中文
            if (Regex.IsMatch(value, @"[\u4e00-\u9fa5]"))
            {
                IsCreate = false;
            }
            return value;
        }

    }
}
