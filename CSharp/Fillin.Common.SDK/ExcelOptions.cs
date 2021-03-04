using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fillin.Common.SDK
{
    /// <summary>
    /// Excel的选项
    /// </summary>
    public class ExcelOptions
    {
        public ExcelOptions()
        {
            this.MergedRegions = new List<MergedRegion>();
        }
        /// <summary>
        /// 需要合并的区域
        /// </summary>
        public List<MergedRegion> MergedRegions { get; private set; }

        /// <summary>
        /// 合并区域无效的消息
        /// </summary>
        public string InvalidMergedRegionsMessage { get; private set; }

        /// <summary>
        /// 多个合并区域是否有效
        /// </summary>
        public bool IsValidMergedRegions
        {
            get
            {
                if (MergedRegions != null)
                {
                    if (MergedRegions.Count(x => !x.IsValidMergedRegion) != 0)
                    {
                        InvalidMergedRegionsMessage = "存在一个或多个无效的合并区域";
                        return false;
                    }
                    var rows = MergedRegions.OrderBy(x => x.FirstRow).ToArray();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (i != rows.Length - 1)
                        {
                            //if (rows[i].FirstRow == rows[i + 1].FirstRow && rows[i].LastRow == rows[i + 1].LastRow)
                            //{
                            if (rows[i].LastRow >= rows[i + 1].FirstRow)
                            {
                                var mgs = new MergedRegion[2] { rows[i], rows[i + 1] }.OrderBy(x => x.FirstCol).ToArray();
                                if (mgs[0].LastCol >= mgs[1].FirstCol)
                                {
                                    InvalidMergedRegionsMessage = string.Format("合并区域[{0},{1},{2},{3}]和合并区域[{4},{5},{6},{7}]行相交叉", rows[i].FirstRow, rows[i].LastRow, rows[i].FirstCol, rows[i].LastCol, rows[i + 1].FirstRow, rows[i + 1].LastRow, rows[i + 1].FirstCol, rows[i + 1].LastCol);
                                    return false;
                                }
                            }
                            //}
                            //else if (rows[i].LastRow >= rows[i + 1].FirstRow)
                            //{
                            //    InvalidMergedRegionsMessage = string.Format("合并区域[{0},{1},{2},{3}]和合并区域[{4},{5},{6},{7}]行相交叉", rows[i].FirstRow, rows[i].LastRow, rows[i].FirstCol, rows[i].LastCol, rows[i + 1].FirstRow, rows[i + 1].LastRow, rows[i + 1].FirstCol, rows[i + 1].LastCol);
                            //    return false;
                            //}
                        }
                    }
                    //var cols = MergedRegions.OrderBy(x => x.FirstCol).ToArray();
                    //for (int i = 0; i < cols.Length; i++)
                    //{
                    //    if (i != cols.Length - 1)
                    //    {
                    //        if (cols[i].LastCol >= cols[i + 1].FirstCol)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //}
                }

                return true;
            }
        }

        /// <summary>
        /// 增加合并区域
        /// </summary>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCol">超始列</param>
        /// <param name="lastCol">结束列</param>
        /// <param name="excelTotalRows">excel的总行数,不含标题</param>
        /// <param name="excelTotalColums">excel的总列数</param>
        public ExcelOptions AddMergedRegion(int firstRow, int lastRow, int firstCol, int lastCol, int? excelTotalRows = 0, int? excelTotalColums = 0)
        {
            this.MergedRegions.Add(new MergedRegion(firstRow, lastRow, firstCol, lastCol, excelTotalRows, excelTotalColums));
            return this;
        }

        /// <summary>
        /// 移除合并区域
        /// </summary>
        /// <param name="firstRow"></param>
        /// <returns></returns>
        public ExcelOptions RemoveMergedRegion(int firstRow)
        {
            var mg = this.MergedRegions.Where(x => x.FirstRow == firstRow);
            foreach (var v in mg)
            {
                this.MergedRegions.Remove(v);
            }

            return this;
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        public CellStyle CellStyle
        {
            get;
            set;
        }

        /// <summary>
        /// 自动调整列宽以适应内容(此属性很慢,如果不是特殊要求,请不要调置)
        /// </summary>
        public bool AutoSizeColumn
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 单元格样式
    /// </summary>
    public class CellStyle
    {
        public CellStyle(bool useBorder, bool useSkipColor, double? fontHeight)
        {
            UseBorder = useBorder;
            UseSkipColor = useSkipColor;
            FontHeight = fontHeight;
        }
        /// <summary>
        /// 是否设置单元格边框
        /// </summary>
        public bool UseBorder { get; set; }

        /// <summary>
        /// 是否设置间隔行的颜色(有BUG,不要使用)
        /// </summary>
        public bool UseSkipColor { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public double? FontHeight { get; set; }
    }
}
