using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fillin.Common.SDK
{
    public static class XMLHelper
    {
        private static string XMLfile { get; set; }

        /// <summary>
        /// 创建XML文件并写入数据
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="filePath">保存位置</param>
        public static void WriteData(DataSet data, string filePath)
        {

            try
            {
                //判断文件是否存在，如果存在就覆盖
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                XmlDocument document = new XmlDocument();
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", null);
                document.AppendChild(dec);

                XmlElement root = document.CreateElement("root");
                document.AppendChild(root);

                XmlElement datasets = document.CreateElement("datasets");
                foreach (DataTable dt in data.Tables)
                {
                    XmlElement dataset = document.CreateElement("dataset");
                    dataset.SetAttribute("TABLENAME", dt.TableName);

                    foreach (DataRow dr in dt.Rows)
                    {
                        XmlElement attributes = document.CreateElement("attributes");

                        foreach (DataColumn dc in dt.Columns)
                        {
                            XmlElement attribute = document.CreateElement("attribute");
                            attribute.SetAttribute("Code", dc.ColumnName);
                            attribute.SetAttribute("Value", dr[dc.ColumnName].ToString());
                            attributes.AppendChild(attribute);
                        }
                        dataset.AppendChild(attributes);
                    }
                    datasets.AppendChild(dataset);
                }
                root.AppendChild(datasets);

                document.Save(filePath);
            }
            catch (Exception ex)
            {
                throw;
            }

        }



    }
}
