using EasyClick.Web.ReportForms.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EasyClick.Web.ReportForms.Export
{
    /// <summary>
    /// 导出 DataTable 表
    /// </summary>
    public class DataTableExport
    {
        /// <summary>
        /// Json 数据导出
        /// </summary>
        public DataTableExport()
        {

        }


        protected void RenderRow(DataTable dt , CrossReport report, RRow row)
        {

            DataRow dr = dt.NewRow();


            int i = -1;

            int j = 0;

            decimal cc = 0;

            foreach (RCell cell in row)
            {
                i++;

                if (cell.Value == null || "0".Equals(cell.Value) || cc.Equals(cell.Value))
                {
                    continue;
                }

                if (!cell.Visible)
                {
                    continue;
                }

                if (j++ > 0) { sb.Append(","); }

                string field;

                if (i < report.RowGroupTags.Count)
                {
                    ReportItemGroup group = report.RowGroupTags[i];


                    ReportItem ri = group[0];

                    field = ri.DBField;
                }
                else
                {
                    field = "DATA_" + cell.X;
                }


                sb.Append("\"").Append(field).Append("\"");

                sb.Append(":");

                if (cell.Value == null)
                {
                    sb.Append("null");
                }
                else
                {
                    if (IsNumber(cell.Value))
                    {
                        decimal v = (decimal)cell.Value;

                        sb.Append(v.ToString("0.######"));
                    }
                    else
                    {
                        sb.Append("\"").Append(JsonUtil.ToJson(cell.Value)).Append("\"");
                    }
                }

            }


        }

        public DataTable GetBodyData(CrossReport report, RTable table)
        {

            DataTable dt = new DataTable();
            dt.BeginInit();

            CreateHeader(report.HeadTreeRoot, dt);

            dt.EndInit();

            dt.BeginLoadData();

            foreach (RRow row in table.Body)
            {
                RenderRow(dt, report, row);
            }

            dt.EndLoadData();

            return dt;

        }




        private void CreateHeader(CrossHeadTreeNode parentNode,DataTable dt)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.HasChild())
                {                    
                    CreateHeader(node,dt);
                }
                else
                {
                    DataColumn col = new DataColumn("DATA_" + node.X);

                    dt.Columns.Add(col);

                }
            }
        }



        private bool IsNumber(object value)
        {
            Type vType = value.GetType();

            bool isNum = false;

            if (vType == typeof(decimal) ||
                vType == typeof(Int16) ||
                vType == typeof(Int32) ||
                vType == typeof(Int64) ||

                vType == typeof(UInt16) ||
                vType == typeof(UInt32) ||
                vType == typeof(UInt64) ||

                vType == typeof(Double) ||
                vType == typeof(Single) ||
                vType == typeof(byte))

            {
                isNum = true;
            }

            return isNum;
        }

    }
}
