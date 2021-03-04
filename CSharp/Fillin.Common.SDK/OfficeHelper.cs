using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Fillin.Common.SDK
{
    // <summary>
    /// 提供对Excel和Word(暂未实现)等Office支持
    /// </summary>
    public class OfficeHelper
    {
        /// <summary>
        /// 写入Excel字节流
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel的选项,包括合并,样式等</param>
        /// <returns></returns>
        public static byte[] WriteToBytes(DataTable dt, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            if (dt == null)
            {
                return null;
            }

            //Excel最多 255个sheet
            if (splitCount < 1 || splitCount > 65536)
            {
                splitCount = 65536;
            }

            int maxSheet = 255;
            var sheetCount = dt.Rows.Count / (splitCount - (useTitle ? 1 : 0)) + (dt.Rows.Count % (splitCount - (useTitle ? 1 : 0)) == 0 ? 0 : 1);
            while (sheetCount > maxSheet)
            {
                splitCount++;
                sheetCount = dt.Rows.Count / (splitCount - (useTitle ? 1 : 0)) + (dt.Rows.Count % (splitCount - (useTitle ? 1 : 0)) == 0 ? 0 : 1);
            }


            int totalSheet = (dt.Rows.Count / splitCount) + (dt.Rows.Count % splitCount == 0 ? 0 : 1);
            int currentSheet = 0;
            int titleCount = useTitle ? 1 : 0;
            var sheetName = (string.IsNullOrWhiteSpace(dt.TableName) ? "Sheet" : dt.TableName) + "_{0}";
            var sheetKey = string.Format(sheetName, currentSheet);
            int rowIndex = 0;

            HSSFWorkbook wk = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;

            //直接输出标题
            if (dt.Rows.Count == 0)
            {
                sheet = wk.CreateSheet(sheetKey);
                row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = row.CreateCell(i, CellType.String);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    wk.Write(ms);
                    return ms.ToArray();
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (sheet == null || rowIndex >= splitCount - titleCount)
                {
                    currentSheet++;
                    rowIndex = 0;
                    sheetKey = string.Format(sheetName, currentSheet);
                    sheet = wk.GetSheet(sheetKey) ?? wk.CreateSheet(sheetKey);
                    row = sheet.CreateRow(0);
                    int j = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        cell = row.CreateCell(j, CellType.String);
                        cell.SetCellValue(dc.ColumnName);
                        j++;
                    }
                }
                int k = 0;
                row = sheet.CreateRow(rowIndex + titleCount);
                foreach (DataColumn dc in dt.Columns)
                {
                    cell = row.CreateCell(k, CellType.String);
                    cell.SetCellValue(dt.Rows[i][k].ToString());
                    k++;
                }
                rowIndex++;
            }

            ApplyOptions(wk, useTitle, splitCount, option);

            using (MemoryStream ms = new MemoryStream())
            {
                wk.Write(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 写入Excel字节流，多个表分成多个Sheet输出,每个表数据不能超过65536
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel的选项,包括合并,样式等</param>
        /// <returns></returns>
        public static byte[] WriteToBytes(DataSet ds, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            if (ds == null)
            {
                return null;
            }

            int titleCount = useTitle ? 1 : 0;

            HSSFWorkbook wk = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;

            foreach (DataTable dt in ds.Tables)
            {
                int currentSheet = 0;
                var sheetName = string.IsNullOrWhiteSpace(dt.TableName) ? "Sheet_{0}" : dt.TableName;
                var sheetKey = string.Format(sheetName, currentSheet);
                int rowIndex = 0;
                sheet = null;

                //直接输出标题
                if (dt.Rows.Count == 0)
                {
                    sheet = wk.CreateSheet(sheetKey);
                    row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        cell = row.CreateCell(i, CellType.String);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }

                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    wk.Write(ms);
                    //    return ms.ToArray();
                    //}
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (sheet == null || rowIndex >= splitCount - titleCount)
                    {
                        currentSheet++;
                        rowIndex = 0;
                        sheetKey = string.Format(sheetName, currentSheet);
                        sheet = wk.GetSheet(sheetKey) ?? wk.CreateSheet(sheetKey);
                        row = sheet.CreateRow(0);
                        int j = 0;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            cell = row.CreateCell(j, CellType.String);
                            cell.SetCellValue(dc.ColumnName);
                            j++;
                        }
                    }
                    int k = 0;
                    row = sheet.CreateRow(rowIndex + titleCount);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        cell = row.CreateCell(k, CellType.String);
                        cell.SetCellValue(dt.Rows[i][k].ToString());
                        k++;
                    }
                    rowIndex++;
                }
            }

            ApplyOptions(wk, useTitle, splitCount, option);

            using (MemoryStream ms = new MemoryStream())
            {
                wk.Write(ms);
                return ms.ToArray();
            }
        }
        private static void ApplyOptions(HSSFWorkbook wk, bool useTitle, int splitCount, ExcelOptions option)
        {
            try
            {
                #region 单元格样式

                if (option != null && option.CellStyle != null && wk != null)
                {
                    var cellStyle = wk.CreateCellStyle();
                    var cellStylex = wk.CreateCellStyle();

                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    if (option.CellStyle.FontHeight.HasValue)
                    {
                        var font = wk.CreateFont();
                        font.FontHeight = option.CellStyle.FontHeight.Value * 100;
                        font.FontName = "宋体";
                        cellStyle.SetFont(font);
                    }
                    if (option.CellStyle.UseBorder)
                    {
                        cellStyle.BorderBottom = BorderStyle.Thin;
                        cellStyle.BorderTop = BorderStyle.Thin;
                        cellStyle.BorderRight = BorderStyle.Thin;
                    }

                    cellStylex.CloneStyleFrom(cellStyle);

                    if (option.CellStyle.UseSkipColor)
                    {
                        HSSFPalette palette = wk.GetCustomPalette(); //调色板实例
                        palette.SetColorAtIndex((short)8, (byte)184, (byte)204, (byte)228);
                        var color = palette.FindColor((byte)184, (byte)204, (byte)228);
                        cellStylex.FillBackgroundColor = color.Indexed;
                        cellStylex.FillPattern = FillPattern.SolidForeground;
                    }

                    for (int i = 0; i < wk.NumberOfSheets; i++)
                    {
                        var sheet = wk.GetSheetAt(i);
                        if (sheet != null)
                        {
                            for (int j = sheet.FirstRowNum; j <= sheet.LastRowNum; j++)
                            {
                                var row = sheet.GetRow(j);
                                if (row != null)
                                {
                                    for (int k = row.FirstCellNum; k <= row.LastCellNum; k++)
                                    {
                                        var cell = row.GetCell(k);
                                        if (cell != null)
                                        {
                                            if (j % 2 == 0)
                                            {
                                                cell.CellStyle = cellStyle;
                                            }
                                            else
                                            {
                                                cell.CellStyle = cellStylex;
                                            }
                                        }
                                    }
                                    if (option.AutoSizeColumn)
                                    {
                                        sheet.AutoSizeColumn(j);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 合并区域参数

                if (option != null && option.MergedRegions != null && wk != null)
                {
                    //不允许区域相交
                    if (option.IsValidMergedRegions)
                    {
                        foreach (var mg in option.MergedRegions)
                        {
                            //先确定属于那个Excel
                            var startSheetIndex = (mg.FirstRow + 1) / (splitCount - (useTitle ? 1 : 0)) + ((mg.FirstRow + 1) % (splitCount - (useTitle ? 1 : 0)) == 0 ? 0 : 1) - 1;
                            var endSheetIndex = (mg.LastRow + 1) / (splitCount - (useTitle ? 1 : 0)) + ((mg.LastRow + 1) % (splitCount - (useTitle ? 1 : 0)) == 0 ? 0 : 1) - 1;

                            var firstRow = 0;
                            var lastRow = 0;

                            //合并区域在同一Sheet
                            if (startSheetIndex == endSheetIndex)
                            {
                                firstRow = mg.FirstRow - (startSheetIndex * (splitCount - (useTitle ? 1 : 0))) + (useTitle ? 1 : 0);
                                lastRow = mg.LastRow - (endSheetIndex * (splitCount - (useTitle ? 1 : 0))) + (useTitle ? 1 : 0);
                                wk.GetSheetAt(startSheetIndex).AddMergedRegion(new CellRangeAddress(firstRow, lastRow, mg.FirstCol, mg.LastCol));
                            }
                            else //if (endSheetIndex - startSheetIndex > 1)
                            {
                                firstRow = mg.FirstRow - (startSheetIndex * (splitCount - (useTitle ? 1 : 0))) + (useTitle ? 1 : 0);
                                lastRow = wk.GetSheetAt(startSheetIndex).LastRowNum;
                                wk.GetSheetAt(startSheetIndex).AddMergedRegion(new CellRangeAddress(firstRow, lastRow, mg.FirstCol, mg.LastCol));

                                for (int i = startSheetIndex + 1; i <= endSheetIndex; i++)
                                {
                                    firstRow = wk.GetSheetAt(i).FirstRowNum + (useTitle ? 1 : 0);
                                    lastRow = i == endSheetIndex ? (mg.LastRow - (endSheetIndex * (splitCount - (useTitle ? 1 : 0))) + (useTitle ? 1 : 0)) : wk.GetSheetAt(i).LastRowNum;
                                    wk.GetSheetAt(i).AddMergedRegion(new CellRangeAddress(firstRow, lastRow, mg.FirstCol, mg.LastCol));
                                }
                            }
                        }
                    }
                }

                #endregion

            }
            catch
            {

            }
        }

        /// <summary>
        /// 写入Excel的Zip压缩字节流
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel的选项</param>
        /// <returns></returns>

        public static byte[] WriteToZipBytes(DataTable dt, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            if (dt == null)
            {
                return null;
            }
            var ds = new DataSet();
            ds.Tables.Add(dt);

            return WriteToZipBytes(ds, useTitle, splitCount, option);
        }

        /// <summary>
        /// 写入到Excel的Zip压缩文件
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="filePathAndName">文件路径</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel选项</param>
        /// <returns></returns>
        public static void WriteToZipFile(DataTable dt, string filePathAndName, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            var ds = new DataSet();
            ds.Tables.Add(dt);
            WriteToZipFile(ds, filePathAndName, useTitle, splitCount, option);
        }

        /// <summary>
        /// 写入到文件(重名文件将会被覆盖)
        /// </summary>
        /// <param name="dt">是否使用标题,默认使用</param>
        /// <param name="filePathAndName">文件路径(*.xls)</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel选项</param>
        public static void WriteToFile(DataTable dt, string filePathAndName, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            if (string.IsNullOrWhiteSpace(filePathAndName))
            {
                throw new ArgumentNullException("filePathAndName");
            }
            //if (!Path.GetExtension(filePathAndName).EndsWith("xls", StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ArgumentException("必需是后缀名为xls的文件");
            //}
            var dir = Path.GetDirectoryName(filePathAndName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (FileStream fs = new FileStream(filePathAndName, FileMode.Create))
            {
                var bytes = WriteToBytes(dt, useTitle, splitCount, option);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 写入到文件(重名文件将会被覆盖)
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="filePathAndName">文件路径(*.xls)</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="option">Excel选项</param>
        public static void WriteToFile(DataSet ds, string filePathAndName, bool useTitle = true, int splitCount = 65536, ExcelOptions option = null)
        {
            if (string.IsNullOrWhiteSpace(filePathAndName))
            {
                throw new ArgumentNullException("filePathAndName");
            }
            //if (!Path.GetExtension(filePathAndName).EndsWith("xls", StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ArgumentException("必需是后缀名为xls的文件");
            //}
            var dir = Path.GetDirectoryName(filePathAndName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (FileStream fs = new FileStream(filePathAndName, FileMode.Create))
            {
                var bytes = WriteToBytes(ds, useTitle, splitCount, option);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        /// <summary>
        /// 把DataSet加载到RAR文件流
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="options">ExcelOptions对应的选项</param>
        /// <returns></returns>
        public static byte[] WriteToZipBytes(DataSet ds, bool useTitle = true, int splitCount = 65536, params ExcelOptions[] options)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            using (MemoryStream mStream = new MemoryStream())
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(mStream))
                {
                    zipStream.SetLevel(8);
                    for (int i = 1; i <= ds.Tables.Count; i++)
                    {
                        ZipEntry entry = new ZipEntry((string.IsNullOrWhiteSpace(ds.Tables[i - 1].TableName) ? "Excel" : ds.Tables[i - 1].TableName) + i + ".xls");
                        entry.DateTime = DateTime.Now;
                        zipStream.PutNextEntry(entry);
                        var option = options.Length >= i ? options[i - 1] : null;
                        var bytes = WriteToBytes(ds.Tables[i - 1], useTitle, splitCount, option);
                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
                return mStream.ToArray();
            }
        }

        /// <summary>
        /// 写入到ZIP文件
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="filePathAndName">文件路径(*.zip)</param>
        /// <param name="useTitle">是否使用标题,默认使用</param>
        /// <param name="splitCount">每个Sheet最大行数(Excel 2003 最大限制 65536),最大不能超过 65536,注意该值不是一个精确值,可能在运行时被修改</param>
        /// <param name="options">Excel选项</param>
        public static void WriteToZipFile(DataSet ds, string filePathAndName, bool useTitle = true, int splitCount = 65536, params ExcelOptions[] options)
        {
            if (string.IsNullOrWhiteSpace(filePathAndName))
            {
                throw new ArgumentNullException("filePathAndName");
            }
            //if (!Path.GetExtension(filePathAndName).EndsWith("xls", StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ArgumentException("必需是后缀名为xls的文件");
            //}
            var dir = Path.GetDirectoryName(filePathAndName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (FileStream fs = new FileStream(filePathAndName, FileMode.Create))
            {
                var bytes = WriteToZipBytes(ds, useTitle, splitCount, options);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 读取XLS文件到DataTable中,支持Excel 2003或以上
        /// </summary>
        /// <param name="filePathAndName">Xls或Xlsx的文件路径</param>
        /// <param name="useTitle">第一行是否是标题</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FileNotFoundException"></exception>
        /// <returns></returns>
        public static DataSet ReadToDataSet(string filePathAndName, bool useTitle = true)
        {
            if (string.IsNullOrWhiteSpace(filePathAndName))
            {
                throw new ArgumentNullException("filePathAndName");
            }
            if (!File.Exists(filePathAndName))
            {
                throw new FileNotFoundException("文件不存在", filePathAndName);
            }

            DataSet ds = new DataSet();

            FileStream fs = new FileStream(filePathAndName, FileMode.Open, FileAccess.Read);
            var bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            IWorkbook wk = null;
            try
            {
                wk = new HSSFWorkbook(new MemoryStream(bytes));
            }
            catch (Exception exp)
            {
                try
                {
                    wk = new XSSFWorkbook(new MemoryStream(bytes));
                }
                catch
                {
                    return ds;
                }
            }
            for (int i = 0; i != wk.NumberOfSheets; i++)
            {
                ISheet sheet = wk.GetSheetAt(i);
                DataTable dt = new DataTable();
                dt.TableName = sheet.SheetName;

                for (int j = 0; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);
                    if (row == null)
                    {
                        continue;
                    }

                    row.Cells.ForEach(delegate (ICell cell)
                    {
                        if (cell.CellType != CellType.String)
                        {
                            cell.SetCellType(CellType.String);
                        }
                    });

                    string value = null;
                    //区域名，表头
                    if (j == 0 && useTitle)
                    {
                        for (int k = 0; k < row.Cells.Count; k++)
                        {
                            value = row.GetCell(k) == null ? ("column" + j) : row.GetCell(k).StringCellValue;
                            if (value != null)
                            {
                                dt.Columns.Add(value);
                            }
                        }
                    }
                    else
                    {
                        object[] datas = new object[dt.Columns.Count];
                        for (int k = 0; k < datas.Length; k++)
                        {
                            datas[k] = row.GetCell(k) == null ? null : row.GetCell(k).StringCellValue;
                        }

                        dt.Rows.Add(datas);
                    }
                }
                if (dt.Rows.Count != 0)
                {
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        /// <summary>
        /// 读取到LIST对象
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="filePathAndName">Xls或Xlsx的文件路径</param>
        /// <param name="useTitle">第一行是否是标题</param>
        /// <returns></returns>
        public static List<T> ReadToList<T>(string filePathAndName, bool useTitle = true) where T : new()
        {
            List<T> list = new List<T>();
            var ds = ReadToDataSet(filePathAndName, useTitle);
            //var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.GetProperty);
            var props = typeof(T).GetProperties();
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var t = Activator.CreateInstance(typeof(T));
                    foreach (var p in props)
                    {
                        try
                        {
                            if (dt.Columns.Contains(p.Name))
                            {
                                p.SetValue(t, Convert.ChangeType(dr[p.Name], p.PropertyType), null);
                            }
                        }
                        catch { }
                    }
                    list.Add((T)t);
                }
            }

            return list;
        }
    }
}
