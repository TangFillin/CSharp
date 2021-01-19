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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CS.ArchiveMaker.WinForm
{
    public partial class Form_Main : Form
    {
        /// <summary>
        /// 打印报表
        /// </summary>
        private ReportViewer reportViewer1 = null;

        /// <summary>
        /// 本地配置
        /// </summary>
        private Configuration Config = null;

        /// <summary>
        /// 打印数据
        /// </summary>
        private PrintDataModel PrintData = null;

        /// <summary>
        /// 保存reportviewer打印流
        /// </summary>
        private List<Stream> m_streams = null;
        private bool? CanPrint = null;

        /// <summary>
        /// 打印预览
        /// </summary>
        private readonly bool IsPreview = false;

        /// <summary>
        /// 当前打印页(每本）
        /// </summary>
        int m_currentPageIndex;

        private int? LastPageCount = null;

        /// <summary>
        /// 是否停止打印
        /// </summary>
        private bool IsStop = false;

        /// <summary>
        /// 初始化数据
        /// </summary>
        private readonly BackgroundWorker Bgw_InitData;

        /// <summary>
        /// 打印
        /// </summary>
        private readonly BackgroundWorker Bgw_Print;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address"></param>
        /// <param name="batchnumber"></param>
        /// <param name="templateName"></param>
        /// <param name="isPreview">是否预览，true:打印预览；false:直接打印</param>
        public Form_Main(string address, string batchnumber, string templateName, bool isPreview)
        {
            InitializeComponent();

            this.Text += $"批次号：{batchnumber}";


            //本地配置
            Config = new Configuration(templateName)
            {
                Address = address,
                BatchNumber = batchnumber,
                PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"] ?? "20")
            };

            toolStripStatusLabel2.Text += Config.GetPrintSettings().PrinterName;

            //是否预览
            IsPreview = isPreview;

            //打印进程
            Bgw_Print = new BackgroundWorker();
            Bgw_Print.DoWork += Bgw_Print_DoWork;
            Bgw_Print.RunWorkerCompleted += Bgw_Print_RunWorkerCompleted;

            //初始化数据线程
            Bgw_InitData = new BackgroundWorker();
            Bgw_InitData.DoWork += Bgw_InitData_DoWork;
            Bgw_InitData.RunWorkerCompleted += Bgw_InitData_RunWorkerCompleted;
            Bgw_InitData.RunWorkerAsync();


        }

        #region BackgroundWorker
        private void Bgw_Print_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (PrintMessage.ContainsKey(i))
            //    {
            //        dataGridView1.Rows[i].Cells["打印状态"].Value = PrintMessage[i];
            //    }
            //}
            btn_print.Enabled = true;
            btn_stopPrint.Enabled = false;
            btn_print.Text = "打印";
            toolStripStatusLabel1.Text = "打印完毕";
        }
        private void Bgw_Print_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateStatusBar("开始打印");
            for (int i = 0; i < PrintData.Data.Count; i++)
            {
                if (IsStop)
                {
                    break;
                }
                string message;
                Form_Print formCatelog = new Form_Print(PrintData, Config, i);
                formCatelog.ShowDialog();
                message = formCatelog.Message;
                UpdatePrintStatus(message, i);
                UpdateStatusBar($"打印:{i + 1}/{PrintData.Data.Count}");
            }
        }

        private void Bgw_InitData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (PrintData == null)
            {
                toolStripStatusLabel1.Text = "未找到数据";
                return;
            }

            btn_print.Enabled = true;
            toolStripStatusLabel1.Text = "加载打印数据完毕";
            if (!IsPreview)
            {
                btn_print.Enabled = false;
                btn_stopPrint.Enabled = true;
                Bgw_Print.RunWorkerAsync();
            }
        }

        private void Bgw_InitData_DoWork(object sender, DoWorkEventArgs e)
        {
            InitData();
            if (PrintData != null && PrintData.Data.Count > 0)
            {
                Init();
            }

        }
        #endregion

        #region 委托实现更新UI
        private delegate void Del_UpdatePrintStatus(string message, int index);
        private delegate void Del_UpdateDataGridViewData(List<Dictionary<string, object>> dic);
        private delegate void Del_UpdateStatusBar(string message = "");

        /// <summary>
        /// 更新打印状态
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="index">索引</param>
        private void UpdatePrintStatus(string message, int index)
        {
            if (dataGridView1.InvokeRequired)
            {
                // 解决窗体关闭时出现“访问已释放句柄”异常
                while (dataGridView1.IsHandleCreated == false)
                {
                    if (dataGridView1.Disposing || dataGridView1.IsDisposed) return;
                }

                Del_UpdatePrintStatus d = new Del_UpdatePrintStatus(UpdatePrintStatus);
                dataGridView1.Invoke(d, new object[] { message, index });
            }
            else
            {
                dataGridView1.Rows[index].Cells["打印状态"].Value = message;
            }
        }

        /// <summary>
        /// 更新DataGridView数据
        /// </summary>
        /// <param name="list"></param>
        private void UpdateDataGridViewData(List<Dictionary<string, object>> list)
        {
            if (dataGridView1.InvokeRequired)
            {
                // 解决窗体关闭时出现“访问已释放句柄”异常
                while (dataGridView1.IsHandleCreated == false)
                {
                    if (dataGridView1.Disposing || dataGridView1.IsDisposed) return;
                }

                Del_UpdateDataGridViewData d = new Del_UpdateDataGridViewData(UpdateDataGridViewData);
                dataGridView1.Invoke(d, new object[] { list });
            }
            else
            {
                if (dataGridView1.Columns.Count == 0)
                {
                    dataGridView1.SuspendLayout();

                    dataGridView1.Columns.Add("序号", "序号");
                    //PrintData.Columns?.Add("打印状态");
                    PrintData.Columns?.ForEach(col =>
                    {
                        dataGridView1.Columns.Add(col, col);
                    });

                    dataGridView1.Columns.Add("打印状态", "打印状态");


                    DataGridViewButtonColumn column = new DataGridViewButtonColumn
                    {
                        HeaderText = "打印预览",
                        Name = "打印预览",
                        Text = "查看",
                        UseColumnTextForButtonValue = true
                    };

                    dataGridView1.Columns.Add(column);
                    //禁用排序
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }
                    dataGridView1.ResumeLayout();

                }

                dataGridView1.SuspendLayout();

                list.ForEach(r =>
                {
                    object[] row = new object[PrintData.Columns.Count + 2];
                    int i = 0;

                    PrintData.Columns.ForEach(col =>
                    {
                        row[++i] = r[col];
                    });

                    row[0] = dataGridView1.Rows.Count + 1;
                    row[i + 1] = "未打印";

                    dataGridView1.Rows.Add(row);
                });

                dataGridView1.ResumeLayout();
            }

        }

        /// <summary>
        /// 更新状态栏
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStatusBar(string message = "")
        {
            if (statusStrip1.InvokeRequired)
            {
                // 解决窗体关闭时出现“访问已释放句柄”异常
                while (statusStrip1.IsHandleCreated == false)
                {
                    if (statusStrip1.Disposing || statusStrip1.IsDisposed) return;
                }

                Del_UpdateStatusBar d = new Del_UpdateStatusBar(UpdateStatusBar);
                statusStrip1.Invoke(d, new object[] { message });
            }
            else
            {

                toolStripStatusLabel1.Text = message;
            }
        }

        #endregion




        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        public void Init()
        {
            reportViewer1 = new ReportViewer();
            //reportViewer1.Visible = true;
            reportViewer1.BringToFront();
            //reportViewer1.Dock = DockStyle.Fill;
            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.RenderingComplete += ReportView_RenderingComplete;
            //InitData(batchnumber);
            if (!Directory.Exists(Config.TempleAddress))
            {
                Directory.CreateDirectory(Config.TempleAddress);
            }

            //PageSettings ps = Config.GetPageSettings();
            //reportViewer1.SetPageSettings(ps);
            //reportViewer1.PrinterSettings = ps.PrinterSettings;
            //reportViewer1.LocalReport.ReportPath = Config.TemplateLocation(PrintData.PrintTemplate.FileName);

        }

        /// <summary>
        /// 初始化打印数据
        /// </summary>
        private void InitData()
        {
            //获取打印数据
            string address = Config.FullAddress.Replace("{batchNumber}", Config.BatchNumber);

            var data = GetPrintData(address);

            JObject obj = (JObject)JsonConvert.DeserializeObject(data);
            if (obj == null || obj["result"].ToString() == "False")
            {
                XMessageBox.Error("获取数据失败,请检查网络链接是否正常和模板参数是否设置正确。");
                return;
            }

            PrintData = JsonConvert.DeserializeObject<PrintDataModel>(obj["data"].ToString());

            using (var stream = new StreamWriter(Config.TemplateLocation(PrintData.PrintTemplate.FileName), false))
            {
                stream.Write(PrintData.FileContent);
            }

            if (PrintData != null && PrintData.Data != null && PrintData.Total > 0)
            {
                UpdateDataGridViewData(PrintData.Data);
                UpdateStatusBar($"加载数据:{PrintData.Data.Count}/{PrintData.Total}");
                int pageCount = PrintData.Total / 20;
                if (pageCount * 5 < PrintData.Total)
                {
                    pageCount += 1;
                }
                //从第二页开始获取数据
                for (int i = 2; i <= pageCount; i++)
                {
                    data = GetPrintData(address, i);
                    obj = (JObject)JsonConvert.DeserializeObject(data);
                    var pData = JsonConvert.DeserializeObject<PrintDataModel>(obj["data"].ToString());
                    if (pData != null)
                    {
                        PrintData.Data.AddRange(pData.Data);
                        UpdateDataGridViewData(pData.Data);
                        UpdateStatusBar($"加载数据:{PrintData.Data.Count}/{PrintData.Total}");
                    }
                }
            }



        }

        /// <summary>
        /// 通过API获取打印数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetPrintData(string address, int page = 1, int size = 20)
        {
            try
            {
                //?page=1&size=2222
                address = $"{address}?page={page}&size={size}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.Proxy = null;
                request.Timeout = 30 * 1000;
                request.ReadWriteTimeout = 30 * 1000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception e)
            {
                UpdateStatusBar(e.Message);
            }
            return "";
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private string Print(int startIndex = 0)
        {
            try
            {
                if (PrintData != null && PrintData.Data.Count > 0)
                {
                    var item = PrintData.Data[startIndex];

                    m_currentPageIndex = 0;
                    m_streams = null;

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(PrintData.PrintTemplate.DataSetName, item.ToDataTable()));
                    var printParams = PrintData.PrintTemplate.PrintParams;
                    if (printParams.Count > 0)
                    {
                        int j = 0;
                        foreach (var parameter in reportViewer1.LocalReport.GetParameters())
                        {
                            var db_param = PrintData.PrintTemplate.PrintParams.Find(p => p.PrintParamName == parameter.Name);
                            string fileName = $"{PrintData.PrintTemplate.PrintTemplateID}_{db_param.Value}{startIndex}{j++}";

                            switch (db_param.ParamType)
                            {
                                case ParamType.BarCode:
                                    //创建条形码文件
                                    fileName += "1D";
                                    string value = CanCreateCode(db_param, item, out bool isCreate);

                                    if (isCreate == false)
                                    {
                                        return $"值({value})不能生成条形码";
                                    }

                                    PrintReport.CreateBarCode(value, fileName, Config.TempleAddress);
                                    break;
                                case ParamType.QRCode:
                                    //创建二维码
                                    fileName += "2D";
                                    value = CanCreateCode(db_param, item, out isCreate);

                                    if (isCreate == false)
                                    {
                                        return $"值({value})不能生成条形码";
                                    }
                                    PrintReport.CreateQRCode(value, fileName, Config.TempleAddress);
                                    break;
                                case ParamType.Other:

                                    reportViewer1.LocalReport.SetParameters(new ReportParameter
                                        (parameter.Name, Convert.ToString(item[db_param.Value])));
                                    continue;
                                case ParamType.SQL:
                                    value = "0";
                                    if (item.ContainsKey(db_param.Value))
                                    {
                                        value = Convert.ToString(item[db_param.Value]);
                                    }
                                    reportViewer1.LocalReport.SetParameters(new ReportParameter
                                        (parameter.Name, value));
                                    continue;
                            }
                            var file = Path.Combine(Config.TempleAddress, fileName + ".bmp");
                            reportViewer1.LocalReport.SetParameters(new ReportParameter(parameter.Name, "file://" + file));
                        }

                        reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

                        reportViewer1.ZoomMode = ZoomMode.Percent;
                        reportViewer1.ZoomPercent = 100;

                    }

                }
                else
                {
                    return "未找到需要打印的数据,请检查SQL或是条件或是著录信息";
                }
                return "已打印";
            }
            catch (Exception ex)
            {
                return ex.Message;
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

        private void Form_Print_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
        }



        private void btn_print_Click(object sender, EventArgs e)
        {
            IsStop = false;
            btn_print.Enabled = false;
            btn_print.Text = "正在打印";
            btn_stopPrint.Enabled = true;
            toolStripStatusLabel1.Text = "开始打印...";

            if (dataGridView1.Rows.Count < 0)
            {
                XMessageBox.Warning("未找到任何数据");
                return;
            }
            try
            {

                Bgw_Print.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "错误:" + ex.Message;
            }


        }

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
                XMessageBox.Error(string.Format("无法打印:{0}", exp.Message));
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
                        XMessageBox.Warning(string.Format("无法打印:{0}", exp.Message));
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
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void btn_stopPrint_Click(object sender, EventArgs e)
        {
            IsStop = true;
            toolStripStatusLabel1.Text = "停止打印";
        }

        private void Form_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (Directory.Exists(Config.TempleAddress))
                {
                    Directory.Delete(Config.TempleAddress, true);
                }
                System.Environment.Exit(0);

            }
            catch (Exception ex)
            {
                XMessageBox.Error("删除临时文件失败:" + ex.Message);
            }

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridView1.Columns["打印预览"].Index)
            {
                return;
            }

            Form form = new Form_PrintPreview(PrintData, Config, e.RowIndex)
            {
                Owner = this
            };
            form.Show();


        }
    }
}
