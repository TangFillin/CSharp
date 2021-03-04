using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fillin.Common.SDK
{
    /// <summary>
    /// 构造一个Excel的合并区域
    /// </summary>
    public class MergedRegion
    {
        /// <summary>
        /// 合并区域
        /// </summary>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCol">超始列</param>
        /// <param name="lastCol">结束列</param>
        /// <param name="excelTotalRows">excel的总行数,不含标题</param>
        /// <param name="excelTotalColums">excel的总列数</param>
        public MergedRegion(int firstRow, int lastRow, int firstCol, int lastCol, int? excelTotalRows = 0, int? excelTotalColums = 0)
        {
            this.FirstRow = firstRow;
            this.LastRow = lastRow;
            this.FirstCol = firstCol;
            this.LastCol = lastCol;
            this.ExcelTotalRows = excelTotalRows;
            this.ExcelTotalColums = excelTotalColums;
        }

        /// <summary>
        /// Excel的总行数
        /// </summary>
        public int? ExcelTotalRows { get; set; }

        /// <summary>
        /// Excel的总列数
        /// </summary>
        public int? ExcelTotalColums { get; set; }

        /// <summary>
        /// 起始行
        /// </summary>
        public int FirstRow { get; set; }

        /// <summary>
        /// 结束行
        /// </summary>
        public int LastRow { get; set; }

        /// <summary>
        /// 超始列
        /// </summary>
        public int FirstCol { get; set; }

        /// <summary>
        /// 结束列
        /// </summary>
        public int LastCol { get; set; }

        /// <summary>
        /// 无区合并区域消息
        /// </summary>
        public string InvalidMergedRegionMessage { get; set; }

        /// <summary>
        /// 合并区域是否有效
        /// </summary>
        public bool IsValidMergedRegion
        {
            get
            {
                if (this.FirstRow < 0 || this.LastRow < 0 || this.FirstCol < 0 || this.LastCol < 0)
                {
                    InvalidMergedRegionMessage = "开始行索引或结束行索引或开始列索引或结束列索引小于0";
                    return false;
                }
                if (this.FirstRow > this.LastRow || this.FirstCol > this.LastCol)
                {
                    InvalidMergedRegionMessage = "开始行索引大于结束行索引或开始列索引大于结束列索引";
                    return false;
                }
                if (this.ExcelTotalColums != null && this.ExcelTotalColums.Value > 0)
                {
                    if (this.LastCol + 1 > this.ExcelTotalColums.Value)
                    {
                        InvalidMergedRegionMessage = "结束列索引大于Excel的总列数";
                        return false;
                    }
                }
                if (this.ExcelTotalRows != null && this.ExcelTotalRows.Value > 0)
                {
                    if (this.LastRow + 1 > this.ExcelTotalRows.Value)
                    {
                        InvalidMergedRegionMessage = "结束行索引大于Excel的总行数";
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
