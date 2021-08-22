using EasyClick.Web.ReportForms.Data;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EasyClick.Web.ReportForms.Export
{
    public class TableExport
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetBodyData(CrossReport report, RTable table)
        {

            DataTable dt = new DataTable();


            foreach (var itemGroup in report.RowGroupTags)
            {
                foreach (var rItem2 in itemGroup)
                {
                    DataColumn col = new DataColumn(rItem2.DBField);

                    dt.Columns.Add(col);
                }
            }



            CreateHeader(report.HeadTreeRoot, dt);

            dt.Columns.Add(new DataColumn("REP_ROW_TYPE", typeof(int)));


            dt.BeginLoadData();

            RBody body = table.Body;
            DataRow dr;

            for (int i = 0; i < body.Count - 1; i++)
            {
                dr = RenderRow(dt, report, body[i]);

                dr["REP_ROW_TYPE"] = 0x00;

                dt.Rows.Add(dr);
            }

            dr = RenderRow(dt, report, body[body.Count - 1]);
            dr["REP_ROW_TYPE"] =0xFF;

            dt.Rows.Add(dr);


            dt.EndLoadData();


            return dt;

        }


        private void CreateHeader(CrossHeadTreeNode parentNode, DataTable dt)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.HasChild())
                {
                    CreateHeader(node, dt);
                }
                else
                {
                    DataColumn col = new DataColumn("DATA_" + node.X,typeof(decimal));

                    dt.Columns.Add(col);

                }
            }


        }

        protected DataRow RenderRow(DataTable dt, CrossReport report, RRow row)
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



                string field;
                object value;

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


                value = cell.Value;

                dr[field] = cell.Value;


            }


            //dt.Rows.Add(dr);

            return dr;

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
