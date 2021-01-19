using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CS.ArchiveMaker.WinForm.Common
{
    public static class Extension
    {
        public static DataTable ToDataTable(this Dictionary<string, object> dic)
        {
            DataTable dt = new DataTable();

            foreach (var key in dic.Keys)
            {
                dt.Columns.Add(new DataColumn()
                {
                    ColumnName = key,
                    DataType = typeof(System.String)
                });
            }
            DataRow dr = dt.NewRow();

            foreach (var key in dic.Keys)
            {
                dr[key] = dic[key];
            }

            dt.Rows.Add(dr);
            return dt;
        }
    }
}
