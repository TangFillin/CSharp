using ADOX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Fillin.Common.SDK
{
    public static class AccessHelper
    {
        //private static readonly string constr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source =";
        private static readonly string constr = @"Provider = Microsoft.Ace.OLEDB.12.0; Data Source =";

        public static bool CreateAccessDb(string filePath)
        {
            Catalog catalog = new Catalog();
            if (!File.Exists(filePath))
            {
                try
                {
                    catalog.Create(constr + filePath);
                }
                catch (System.Exception e)
                {
                    Trace.TraceWarning("创建Access数据库出错,不能保存");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public static void ExecuteSqlTran(DataSet data, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            CreateAccessDb(filePath);
            string str = constr + filePath;
            using (OleDbConnection conn = new OleDbConnection(str))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
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
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
            /// 执行SQL语句，返回影响的记录数
            /// </summary>
            /// <param name="SQLString">SQL语句</param>
            /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(constr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                      (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}
