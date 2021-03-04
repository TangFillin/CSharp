using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fillin.Common.SDK
{
    public static class SQLiteHelper
    {
        // 连接数据库    
        private static readonly string connectString = "data source = ";
        public static bool CreateDatabase(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                SQLiteConnection.CreateFile(filePath);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("创建SQLite数据库文件失败" + ex.Message);
                return false;
            }
        }

        public static bool InserData(DataSet data, string filePath)
        {
            // 连接数据库    
            string connectString = "data source = " + filePath;
            CreateDatabase(filePath);
            using (SQLiteConnection conn = new SQLiteConnection(connectString))
            {
                conn.Open();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = trans;
                    try
                    {
                        #region 创建表结构
                        string createTb = "create table {0}({1})";

                        foreach (DataTable dt in data.Tables)
                        {
                            string tablename = dt.TableName.Substring(dt.TableName.LastIndexOf("(") + 1).Replace(")", "");
                            dt.TableName = tablename;//去掉中文部分
                            string cols = "";
                            foreach (DataColumn dc in dt.Columns)
                            {
                                cols += $"{dc.ColumnName} text(200), ";
                            }
                            cmd.CommandText = string.Format(createTb, tablename, cols).Replace(", )", ")");
                            cmd.ExecuteNonQuery();
                        }
                        #endregion
                        #region 插入数据
                        string insertData = "INSERT INTO {0} VALUES({1})";
                        foreach (DataTable dt in data.Tables)
                        {
                            string tablename = dt.TableName;

                            foreach (DataRow dr in dt.Rows)
                            {
                                string rows = "";
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    rows += $"'{dr[dc.ColumnName]}', ";
                                }
                                cmd.CommandText = string.Format(insertData, tablename, rows).Replace(", )", ")");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        #endregion
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
