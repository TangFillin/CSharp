using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Common
{
    public class Configuration
    {
        /// <summary>
        /// 模板和配置文件所在文件夹
        /// </summary>
        public readonly string SettingFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrintTemplate");




        public Configuration(string templateName)
        {
            this.templateName = templateName;
        }

        private string filePath { get; set; }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = Path.Combine(SettingFileLocation, templateName + ".bin");
                }
                return filePath;
            }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }


        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplateLocation(string fileName)
        {
            if (!Directory.Exists(SettingFileLocation))
            {
                Directory.CreateDirectory(SettingFileLocation);
            }
            return Path.Combine(SettingFileLocation, fileName);

        }

        private string templateName { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName
        {
            get
            {
                if (templateName.Contains("/"))
                {
                    templateName = templateName.Replace("/", "");
                }
                return templateName;
            }
        }

        private RawPrintSetting setting { get; set; }

        /// <summary>
        /// 打印设置
        /// </summary>
        public RawPrintSetting RawPrintSetting
        {
            get
            {
                if (setting == null)
                {
                    setting = GetRawPrintSettings();
                }
                return setting;
            }
        }

        /// <summary>
        /// 获取打印设置
        /// </summary>
        /// <param name="st"></param>
        /// <param name="customSettingFileLocation"></param>
        /// <returns></returns>
        public PrinterSettings GetPrintSettings()
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = RawPrintSetting.PrinterName;
            ps.DefaultPageSettings.Margins = RawPrintSetting.PageMargins;
            ps.DefaultPageSettings.PaperSize = RawPrintSetting.PaperSize;
            ps.DefaultPageSettings.Landscape = RawPrintSetting.LandScape;
            return ps;
        }



        public PageSettings GetPageSettings()
        {
            PageSettings ps = new PageSettings();
            ps.Margins = RawPrintSetting.PageMargins;
            ps.PaperSize = RawPrintSetting.PaperSize;
            ps.Landscape = RawPrintSetting.LandScape;
            ps.PrinterSettings = GetPrintSettings();
            return ps;
        }

        /// <summary>
        /// 读取打印设置
        /// </summary>
        /// <returns></returns>
        public RawPrintSetting GetRawPrintSettings()
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return null;
            }
            var rps = new RawPrintSetting()
            {
                LandScape = false
            };
            try
            {

                if (File.Exists(FilePath))
                {
                    using (var stream = new StreamReader(FilePath, Encoding.UTF8))
                    {
                        var json = stream.ReadToEnd();
                        rps = JsonConvert.DeserializeObject<RawPrintSetting>(json);
                    }
                }
                else
                {
                    rps = DefaultSetting();
                }

            }
            catch (Exception exp)
            {
                XMessageBox.Warning(string.Format("加载打印设置出错,您需要重新设置并保存"));

            }

            return rps;
        }

        public string SaveSetting(RawPrintSetting rps)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    using (var st = new StreamWriter(stream, Encoding.UTF8))
                    {
                        st.Write(JsonConvert.SerializeObject(rps, new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                            Formatting = Formatting.Indented
                        }));
                    }
                }
                return "保存成功";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }


        public RawPrintSetting DefaultSetting()
        {
            if (PrinterSettings.InstalledPrinters.Count < 0)
            {
                return null;
            }
            PrinterSettings ps = new PrinterSettings();

            ps.PrinterName = new PrintDocument().PrinterSettings.PrinterName;

            ps.DefaultPageSettings.PaperSize = ps.PaperSizes[0];
            ps.DefaultPageSettings.Margins = new Margins
            {
                Top = (int)Mm2Inch(Convert.ToDouble(25)),
                Bottom = (int)Mm2Inch(Convert.ToDouble(25)),
                Left = (int)Mm2Inch(Convert.ToDouble(15)),
                Right = (int)Mm2Inch(Convert.ToDouble(15))
            };
            ps.DefaultPageSettings.Landscape = false;

            RawPrintSetting rps = new RawPrintSetting();
            rps.LandScape = false;
            rps.PageMargins = ps.DefaultPageSettings.Margins;
            rps.PaperName = ps.DefaultPageSettings.PaperSize.PaperName;
            rps.PaperSize = ps.DefaultPageSettings.PaperSize;
            rps.PrinterName = ps.PrinterName;
            return rps;
        }

        /// <summary>
        /// 自定义纸张的尺寸
        /// </summary>
        public readonly PaperSize CustomPaperSize = new PaperSize
        {
            PaperName = "自定义纸张",
            Height = 0,
            Width = 0
        };


        /// <summary>
        /// 数据接口地址
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 完整地址，包含接口
        /// </summary>
        public string FullAddress
        {
            get
            {
                return Address.TrimEnd('/') + "/api/PrintApi/printdata/{batchNumber}";
            }
        }

        /// <summary>
        /// 临时地址
        /// </summary>
        public string TempleAddress
        {
            get
            {
                return Path.Combine(SettingFileLocation, BatchNumber);
            }
            
        }

        /// <summary>
        /// 1/100英寸转毫米
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public double Inch2Mm(double inch)
        {
            return Math.Round(inch / 100 * 25.4);
        }




        /// <summary>
        /// 毫米转1/100英寸
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public double Mm2Inch(double mm)
        {
            return Math.Round(mm / 25.4 * 100);
        }

        /// <summary>
        /// 每次加载数据条数
        /// </summary>
        public int PageSize { get; set; }
    }
}
