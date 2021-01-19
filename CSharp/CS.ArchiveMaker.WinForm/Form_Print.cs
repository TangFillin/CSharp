using CS.ArchiveMaker.WinForm.Common;
using CS.ArchiveMaker.WinForm.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CS.ArchiveMaker.WinForm
{
    public partial class Form_Print : Form
    {
        public string Message { get; set; }

        public PrintDataModel PrintData { get; set; }

        public Configuration Config { get; set; }

        public ReportViewer ReportViewer1 { get; set; }

        // <summary>
        /// 保存reportviewer打印流
        /// </summary>
        private List<Stream> m_streams = null;

        /// <summary>
        /// 当前打印页(每本）
        /// </summary>
        int m_currentPageIndex;

        private int Index { get; set; }
        public Form_Print(PrintDataModel data, Configuration config, int index)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;

            PrintData = data;
            Config = config;
            Index = index;

            this.Hide();
            ReportViewer1 = new ReportViewer();

            //this.Controls.Add(ReportViewer1);
            //ReportViewer1.Visible = true;
            //ReportViewer1.BringToFront();
            //ReportViewer1.Dock = DockStyle.Fill;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.RenderingComplete += ReportView_RenderingComplete;


        }

        private void Form_PrintCatelog_Shown(object sender, EventArgs e)
        {
            //base.OnShown(e);
            Print();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private void Print()
        {
            try
            {
                var item = PrintData.Data[Index];

                m_currentPageIndex = 0;
                m_streams = null;

                PageSettings ps = Config.GetPageSettings();
                ReportViewer1.SetPageSettings(ps);
                ReportViewer1.LocalReport.ReportPath = Config.TemplateLocation(PrintData.PrintTemplate.FileName);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(PrintData.PrintTemplate.DataSetName, item.ToDataTable()));
                var printParams = PrintData.PrintTemplate.PrintParams;
                if (printParams.Count > 0)
                {
                    int j = 0;
                    foreach (var parameter in ReportViewer1.LocalReport.GetParameters())
                    {
                        var db_param = PrintData.PrintTemplate.PrintParams.Find(p => p.PrintParamName == parameter.Name);
                        string fileName = $"{PrintData.PrintTemplate.PrintTemplateID}_{db_param.Value}{Index}{j++}";

                        switch (db_param.ParamType)
                        {
                            case ParamType.BarCode:
                                //创建条形码文件
                                fileName += "1D";
                                string value = CanCreateCode(db_param, item, out bool isCreate);

                                if (isCreate == false)
                                {
                                    Message = $"值({value})不能生成条形码";
                                    return;
                                }

                                PrintReport.CreateBarCode(value, fileName, Config.TempleAddress);
                                break;
                            case ParamType.QRCode:
                                //创建二维码
                                fileName += "2D";
                                value = CanCreateCode(db_param, item, out isCreate);

                                if (isCreate == false)
                                {
                                    Message = $"值({value})不能生成条形码";
                                }
                                PrintReport.CreateQRCode(value, fileName, Config.TempleAddress);
                                break;
                            case ParamType.Other:

                                ReportViewer1.LocalReport.SetParameters(new ReportParameter
                                    (parameter.Name, Convert.ToString(item[db_param.Value])));
                                continue;
                            case ParamType.SQL:
                                value = "0";
                                if (item.ContainsKey(db_param.Value))
                                {
                                    value = Convert.ToString(item[db_param.Value]);
                                }
                                ReportViewer1.LocalReport.SetParameters(new ReportParameter
                                    (parameter.Name, value));
                                continue;
                        }
                        var file = Path.Combine(Config.TempleAddress, fileName + ".bmp");
                        ReportViewer1.LocalReport.SetParameters(new ReportParameter(parameter.Name, "file://" + file));
                    }

                    ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

                    ReportViewer1.ZoomMode = ZoomMode.Percent;
                    ReportViewer1.ZoomPercent = 100;

                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }


        /// <summary>
        /// 判断是否可以生成条码
        /// </summary>
        /// <param name="db_param"></param>
        /// <param name="item"></param>
        /// <param name="IsCreate"></param>
        /// <returns></returns>
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

        private bool? CanPrint = null;
        private int? LastPageCount = null;
        /// <summary>
        /// ReportViewer渲染完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportView_RenderingComplete(object sender, Microsoft.Reporting.WinForms.RenderingCompleteEventArgs e)
        {
            if (e.Exception != null)
            {
                var exp = e.Exception;
                while (exp.InnerException != null)
                {
                    exp = exp.InnerException;
                }
                Message = string.Format("无法打印:{0}", exp.Message);
                return;
            }

            ReportViewer rv = sender as ReportViewer;
            //rv.SetPageSettings(Config.GetPageSettings());
            #region 如果能直接打印了

            if (!PrintData.PrintTemplate.IsAppend || CanPrint != null && CanPrint.Value)
            {
                if (e.Exception == null && rv.LocalReport.DataSources.Count > 0)
                {
                    try
                    {
                        Export(rv.LocalReport);
                        printDocument1.DefaultPageSettings = Config.GetPageSettings();
                        printDocument1.PrinterSettings = Config.GetPrintSettings();
                        printDocument1.Print();
                        StreamDispose();
                    }
                    catch (Exception exp)
                    {
                        Message = string.Format("无法打印:{0}", exp.Message);
                        //XMessageBox.Warning(string.Format("无法打印:{0}", exp.Message));
                    }
                }

                return;
            }

            #endregion

            #region 往页后面追加行
            if (PrintData.PrintTemplate.IsAppend)
            {
                var totalPages = rv.GetTotalPages();

                if (LastPageCount == null)
                {
                    LastPageCount = totalPages;
                }
                var dt = rv.LocalReport.DataSources[0].Value as DataTable;
                if (LastPageCount == totalPages)
                {
                    dt.Rows.Add(dt.NewRow());
                }
                else
                {
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
                    CanPrint = true;

                }
                rv.RefreshReport();
                return;
            }


            #endregion
        }

        /// <summary>
        /// 自动打印
        /// </summary>
        /// <param name="report"></param>
        private void Export(LocalReport report)
        {
            var pageset = new PageSettings(Config.GetPrintSettings());
            string deviceInfo =
             "<DeviceInfo>" +
             "  <OutputFormat>EMF</OutputFormat>" +
             "  <PageWidth>" + Config.Inch2Mm(pageset.PaperSize.Width) + "mm</PageWidth>" +
             "  <PageHeight>" + Config.Inch2Mm(pageset.PaperSize.Height) + "mm</PageHeight>" +
             "  <MarginTop>" + Math.Round((decimal)pageset.Margins.Top / 10) + "mm</MarginTop>" +
             "  <MarginLeft>" + Math.Round((decimal)pageset.Margins.Left / 10) + "mm</MarginLeft>" +
             "  <MarginRight>" + Math.Round((decimal)pageset.Margins.Right / 10) + "mm</MarginRight>" +
             "  <MarginBottom>" + Math.Round((decimal)pageset.Margins.Bottom / 10) + "mm</MarginBottom>" +
             "</DeviceInfo>";
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out Warning[] warnings);
            foreach (MemoryStream stream in m_streams)
            {
                stream.Position = 0;
            }
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            MemoryStream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public void StreamDispose()
        {
            if (m_streams != null)
            {
                foreach (MemoryStream stream in m_streams)
                    stream.Close();
                m_streams = null;
                m_currentPageIndex = 0;
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Image pageImage = Image.FromStream(m_streams[m_currentPageIndex]);
                e.Graphics.DrawImage(pageImage, e.PageBounds);
                m_currentPageIndex++;
                //e.HasMorePages = m_currentPageIndex < m_streams.Count;
                e.HasMorePages = m_currentPageIndex < m_streams.Count;
                if (e.HasMorePages == false)
                {
                    Message = "已打印";
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

        }
    }
}
