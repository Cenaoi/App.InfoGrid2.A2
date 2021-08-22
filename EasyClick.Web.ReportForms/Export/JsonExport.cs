using EasyClick.Web.ReportForms.Data;
using EC5.Utility;
using EC5.Utility.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms.Export
{
    /// <summary>
    /// Json 数据导出
    /// </summary>
    public class JsonExport
    {
        /// <summary>
        /// Json 数据导出
        /// </summary>
        public JsonExport()
        {

        }


        protected void RenderRow(StringBuilder sb  ,CrossReport report, RRow row)
        {

            sb.Append("{");

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
                        decimal v = Convert.ToDecimal( cell.Value);

                        sb.Append(v.ToString("0.######"));
                    }
                    else
                    {
                        sb.Append("\"").Append(JsonUtil.ToJson(cell.Value)).Append("\"");
                    }
                }

            }


            sb.Append("}");
        }

        public string GetBodyData(CrossReport report, RTable table)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            int i = 0;

            foreach (RRow row in table.Body)
            {
                if(i++ > 0) { sb.Append(","); }

                RenderRow(sb, report, row);            
            }

            sb.Append("]");

            return sb.ToString();

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
