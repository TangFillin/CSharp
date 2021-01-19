using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Models
{
    /// <summary>
    /// 打印数据
    /// </summary>
    public class PrintDataModel
    {
        /// <summary>
        /// 打印数据，来源SQL
        /// </summary>
        public List<Dictionary<string, object>> Data { get; set; }

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplateModel PrintTemplate { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public string FileContent { get; set; }


        /// <summary>
        /// 数据列标题
        /// </summary>
        public List<string> Columns { get; set; }

        /// <summary>
        /// 保存值来源于函数的参数值(包括常量)
        /// </summary>
        public Dictionary<string, string> FunValue { get; set; }

        /// <summary>
        /// 总共记录条数
        /// </summary>
        public int Total { get; set; }
    }

    public class PrintTemplateModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int PrintTemplateID { get; set; }

        /// <summary>
        /// 数据集名称
        /// </summary>
        public string DataSetName { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>
        public List<PrintParamModel> PrintParams { get; set; } = new List<PrintParamModel>();

        /// <summary>
        /// 模板文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 是否追加空白行
        /// </summary>
        public bool IsAppend { get; set; }
    }
    public class PrintParamModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int PrintParamID { get; set; }

        /// <summary>
        /// 参数名字(模板)
        /// </summary>
        public string PrintParamName { get; set; }

        /// <summary>
        /// 显示类型，0：一维码，1：二维码，2：SQL, 3:其他
        /// </summary>
        public ParamType ParamType { get; set; }


        /// <summary>
        /// 模板
        /// </summary>
        public int PrintTemplateID { get; set; }

        public PrintTemplateModel PrintTemplate { get; set; }

        /// <summary>
        /// 参数数据来源：0：DataSet数据源，1：Function,2:数据库主表，3：docfile
        /// </summary>
        public ValueSource ValueSource { get; set; }

        /// <summary>
        /// 参数值（仅在数据来源为Function)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public string DataSetName { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int UserID { get; set; }


        /// <summary>
        /// 创建修改时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ParamType
    {
        /// <summary>
        /// 条形码
        /// </summary>
        BarCode = 0,
        /// <summary>
        /// 二维码
        /// </summary>
        QRCode = 1,
        /// <summary>
        /// SQL
        /// </summary>
        SQL = 2,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 3
    }
    /// <summary>
    /// 值来源
    /// </summary>
    public enum ValueSource
    {
        /// <summary>
        /// 数据集
        /// </summary>
        DataSet = 0,
        /// <summary>
        /// 函数
        /// </summary>
        Function = 1,
        /// <summary>
        /// 主表
        /// </summary>
        MainTable = 2,
        /// <summary>
        /// 数据库表格
        /// </summary>
        DocFile = 3,
    }
}
