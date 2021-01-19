using CS.ArchiveMaker.WinForm.Common;
using Newtonsoft.Json;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CS.ArchiveMaker.WinForm
{
    public partial class Form_PrintSetting : Form
    {
        public readonly Configuration Config = null;
        public Form_PrintSetting(string templatename)
        {
            InitializeComponent();


            Config = new Configuration(templatename);

            if (!Directory.Exists(Config.SettingFileLocation))
            {
                Directory.CreateDirectory(Config.SettingFileLocation);
            }
            Init();

        }
        /// <summary>
        /// 加载设置
        /// </summary>
        private void Init()
        {
            cmbPrinter.DisplayMember = "PrinterName";
            cmbPaper.DisplayMember = "PaperName";
            // 纸张选择改变
            cmbPaper.SelectedIndexChanged += PaperSelectChange;
            // 打印机选择改变事件
            cmbPrinter.SelectedIndexChanged += PrinterSelectChange;


            InitPrinter();

            this.Load += PrintSetting_Load;
        }

        /// <summary>
        /// 加载用户打印设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrintSetting_Load(object sender, EventArgs e)
        {
            try
            {
                var fileName = Config.FilePath;

                if (File.Exists(fileName))
                {
                    RawPrintSetting rps = Config.GetRawPrintSettings();
                    if (rps != null)
                    {
                        //打印机
                        foreach (PrinterSettings ps in this.cmbPrinter.Items)
                        {
                            if (ps.PrinterName.Equals(rps.PrinterName, StringComparison.OrdinalIgnoreCase))
                            {
                                ps.DefaultPageSettings.Landscape = rps.LandScape;
                                ps.DefaultPageSettings.Margins = rps.PageMargins;
                                this.cmbPrinter.SelectedItem = ps;
                                PrinterSelectChange(this.cmbPrinter, new EventArgs());
                                break;
                            }
                        }
                        //纸张
                        foreach (PaperSize ps in this.cmbPaper.Items)
                        {
                            if (ps.PaperName.Equals(rps.PaperName, StringComparison.OrdinalIgnoreCase))
                            {
                                if (ps.PaperName.Equals("自定义纸张"))
                                {
                                    ps.Height = rps.PaperSize.Height;
                                    ps.Width = rps.PaperSize.Width;
                                }
                                this.cmbPaper.SelectedItem = ps;
                                PaperSelectChange(this.cmbPaper, new EventArgs());
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                XMessageBox.Warning(string.Format("加载打印设置出错,您需要重新设置并保存"));
            }
        }

        /// <summary>
        /// 初始化打印机
        /// </summary>
        private void InitPrinter()
        {
            try
            {
                // 加载打印机
                var printers = PrinterSettings.InstalledPrinters;
                foreach (var prt in printers)
                {
                    var prtSetting = new PrinterSettings();
                    prtSetting.PrinterName = prt as string;
                    cmbPrinter.Items.Add(prtSetting);
                    if (prtSetting.IsDefaultPrinter)
                    {
                        cmbPrinter.SelectedItem = prtSetting;
                    }
                }
            }
            catch { }
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                #region 参数校验

                if (this.cmbPrinter.SelectedItem == null)
                {
                    XMessageBox.Warning("请选择打印机");
                    return;
                }
                if (this.cmbPaper.SelectedItem == null)
                {
                    XMessageBox.Warning("请选择打印纸张");
                    return;
                }

                if (!double.TryParse(this.tbPaperWidth.Text, out double nw) || !double.TryParse(this.tbPaperHeight.Text, out double nh) || nw <= 0 || nh <= 0)
                {
                    XMessageBox.Warning("请输入正确的纸张高度和宽度，宽度和高度必须大于零");
                    return;
                }

                if (!double.TryParse(this.tbMarginBottom.Text, out double mb) || !double.TryParse(this.tbMarginRight.Text, out double mr) || !double.TryParse(this.tbMarginLeft.Text, out double ml) || !double.TryParse(this.tbMarginTop.Text, out double mt) || mt < 0 || mb < 0 || ml < 0 || mr < 0)
                {
                    XMessageBox.Warning("请输入正确的页边距，上下左右边距必须大于等于零");
                    return;
                }

                if (nh - mt - mb <= 0 || Math.Abs(nh - mt - mb) / (double)nh < 0.2)
                {
                    XMessageBox.Warning("上下边距太大，可打印区域太小，请重新设置上下边距");
                    return;
                }

                if (nw - mr - ml <= 0 || Math.Abs(nw - mr - ml) / (double)nw < 0.2)
                {
                    XMessageBox.Warning("左右边距太大，可打印区域太小，请重新设置左右边距");
                    return;
                }

                #endregion

                PrinterSettings ps = cmbPrinter.SelectedItem as PrinterSettings;
                var pz = cmbPaper.SelectedItem as PaperSize;
                ps.DefaultPageSettings.PaperSize = pz;
                if (pz.PaperName.Equals("自定义纸张"))
                {
                    pz.Width = (int)Config.Mm2Inch(Convert.ToDouble(tbPaperWidth.Text));
                    pz.Height = (int)Config.Mm2Inch(Convert.ToDouble(tbPaperHeight.Text));
                }
                ps.DefaultPageSettings.Margins = new Margins
                {
                    Top = (int)Config.Mm2Inch(Convert.ToDouble(tbMarginTop.Text)),
                    Bottom = (int)Config.Mm2Inch(Convert.ToDouble(tbMarginBottom.Text)),
                    Left = (int)Config.Mm2Inch(Convert.ToDouble(tbMarginLeft.Text)),
                    Right = (int)Config.Mm2Inch(Convert.ToDouble(tbMarginRight.Text))
                };
                ps.DefaultPageSettings.Landscape = this.rbLanscape.Checked;

                RawPrintSetting rps = new RawPrintSetting();
                rps.LandScape = this.rbLanscape.Checked;
                rps.PageMargins = ps.DefaultPageSettings.Margins;
                rps.PaperName = ps.DefaultPageSettings.PaperSize.PaperName;
                rps.PaperSize = pz;
                rps.PrinterName = ps.PrinterName;



                XMessageBox.Info(Config.SaveSetting(rps));
            }
            catch (Exception exp)
            {
                XMessageBox.Error(string.Format("保存打印设置失败:{0}", exp.Message));
            }


        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 打印机选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PrinterSelectChange(object sender, EventArgs args)
        {
            var setting = cmbPrinter.SelectedItem as PrinterSettings;
            if (setting != null)
            {
                cmbPaper.Items.Clear();
                cmbPaper.Items.Add(Config.CustomPaperSize);
                var defPaperName = setting.DefaultPageSettings.PaperSize.PaperName;
                foreach (PaperSize pz in setting.PaperSizes)
                {
                    cmbPaper.Items.Add(pz);
                    if (pz.PaperName.Equals(defPaperName, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbPaper.SelectedItem = pz;
                    }
                }
                tbMarginBottom.Text = Config.Inch2Mm(setting.DefaultPageSettings.Margins.Bottom).ToString();
                tbMarginLeft.Text = Config.Inch2Mm(setting.DefaultPageSettings.Margins.Left).ToString();
                tbMarginRight.Text = Config.Inch2Mm(setting.DefaultPageSettings.Margins.Right).ToString();
                tbMarginTop.Text = Config.Inch2Mm(setting.DefaultPageSettings.Margins.Top).ToString();
                rbLanscape.Checked = setting.DefaultPageSettings.Landscape;
                rbPortrait.Checked = !rbLanscape.Checked;
            }
        }

        /// <summary>
        /// 纸张选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PaperSelectChange(object sender, EventArgs args)
        {
            var paper = cmbPaper.SelectedItem as PaperSize;
            if (paper != null)
            {
                tbPaperHeight.Enabled = tbPaperWidth.Enabled = paper == Config.CustomPaperSize;
                tbPaperHeight.Text = Config.Inch2Mm(paper.Height).ToString();
                tbPaperWidth.Text = Config.Inch2Mm(paper.Width).ToString();
            }
        }

       
    }
}
