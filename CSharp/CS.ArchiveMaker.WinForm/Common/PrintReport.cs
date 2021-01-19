using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Common
{
    public static class PrintReport
    {



        /// <summary>
        /// 创建条形码
        /// </summary>
        /// <param name="recordno"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateBarCode(string recordno, string fileName, string path)
        {
            try
            {
                Zen.Barcode.Code128BarcodeDraw zen = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                var scale = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeScale"] ?? "2");
                var height = int.Parse(System.Configuration.ConfigurationManager.AppSettings["BarCodeHeight"] ?? "60");
                var image = zen.Draw(recordno, height, scale);
                var file = Path.Combine(path, fileName + ".bmp");
                image.Save(file);
                image.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                XMessageBox.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="recordno"></param>
        /// <param name="fileName"></param>
        public static bool CreateQRCode(string recordno, string fileName, string path)
        {
            try
            {
                Zen.Barcode.CodeQrBarcodeDraw zen = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                var image = zen.Draw(recordno, 25);
                var file = Path.Combine(path, fileName + ".bmp");
                image.Save(file);
                image.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                XMessageBox.Error(ex.Message);
                return false;
            }
        }
    }
}
