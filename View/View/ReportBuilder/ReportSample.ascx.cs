using EasyClick.Web.Mini2;
using EasyClick.Web.ReportForms;
using EasyClick.Web.ReportForms.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class ReportSample : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            List<CONT_INFO> list = GetDataList();

            ReportItem rItem;
            CrossReport report = new CrossReport();

            rItem = new ReportItem("IO", RFieldValueMode.FixedValue);
            rItem.FixedValues.Add("I", "进场");
            rItem.FixedValues.Add("O", "出场");

            report.ColGroupTags.Add(rItem);


            rItem = new ReportItem("CONT_SIZE", RFieldValueMode.FixedValue);
            rItem.FixedValues.Add("20", "20尺");
            rItem.FixedValues.Add("40", "40尺");
            rItem.FixedValues.Add("45", "45尺");
            rItem.FixedValues.Add("20,40,45", "其它", OperatorTypes.NotIn);

            report.ColGroupTags.Add(rItem);


            report.RowGroupTags.Add(new ReportItem("ORG_TYPE") { Title = "类型", EnabledTotal = false });

            report.RowGroupTags.Add(new ReportItem("ORG") { Title = "组织机构", EnabledTotal = false });


            report.DataGroupTags.Add(new ReportItem("ID", RFieldValueMode.DBValue, "COUNT"));

            report.SetDataSource(list);



            RTable table = report.ToRTable();

            foreach (var itemGroup in report.RowGroupTags)
            {
                foreach (var item in itemGroup)
                {
                    BoundField field = new BoundField(item.DBField, item.Title);

                    this.table1.Columns.Add(field);
                }
            }

            CreateHeader(report.HeadTreeRoot, this.table1.Columns);

            foreach (var row in table.Body)
            {

                StringBuilder sb = new StringBuilder();
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

                    if (i < report.RowGroupTags.Count)
                    {
                        ReportItemGroup group = report.RowGroupTags[i];


                        ReportItem ri = group[0];

                        sb.Append("\"").Append(ri.DBField).Append("\"");

                        sb.Append(":");

                        sb.Append("\"").Append(cell.Value).Append("\"");

                    }
                    else
                    {
                        sb.Append("\"").Append("DATA_" + cell.X).Append("\"");

                        sb.Append(":");

                        if (cell.Value == null)
                        {

                            sb.Append("null");
                        }
                        else
                        {
                            //sb.Append(cell.Value);
                            sb.Append("\"").Append(cell.Value).Append("\"");
                        }

                        //sb.Append("\"").Append(cell.Value).Append("\"");

                    }

                }


                sb.Append("}");

                this.store1.Add(sb.ToString());
            }



        }

        private void CreateHeader(CrossHeadTreeNode parentNode, TableColumnCollection cols)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.HasChild())
                {
                    GroupColumn field = new GroupColumn(node.Column.Text);

                    cols.Add(field);

                    CreateHeader(node, field.Columns);
                }
                else
                {
                    BoundField field = new BoundField("DATA_" + node.X, node.Column.Text);
                    field.ItemAlign = EasyClick.Web.Mini.CellAlign.Right;
                    field.Width = 80;
                    field.NotDisplayValue = "0";

                    cols.Add(field);
                }
            }
        }


        [DBTable("CONT_INFO", DBTableMode.Virtual)]
        public class CONT_INFO
        {
            [DBField]
            public int ID { get; set; }
            [DBField]
            public string IO { get; set; }
            [DBField]
            public string CONT_SIZE { get; set; }
            [DBField]
            public string CONT_TYPE { get; set; }

            /// <summary>
            /// 组织机构
            /// </summary>
            public string ORG { get; set; }

            /// <summary>
            /// 组织机构
            /// </summary>
            public string ORG_TYPE { get; set; }
        }


        public List<CONT_INFO> GetDataList()
        {


            List<CONT_INFO> list = new List<CONT_INFO>();

            CONT_INFO cont = new CONT_INFO();
            cont.ID = 1;
            cont.CONT_SIZE = "20";
            cont.CONT_TYPE = "GP";
            cont.IO = "I";
            cont.ORG = "ADMIN";
            cont.ORG_TYPE = "A";

            list.Add(cont);

            cont = new CONT_INFO();
            cont.ID = 2;
            cont.CONT_SIZE = "40";
            cont.CONT_TYPE = "GP";
            cont.IO = "I";
            cont.ORG = "ADMIN";
            cont.ORG_TYPE = "A";

            list.Add(cont);

            cont = new CONT_INFO();
            cont.ID = 3;
            cont.CONT_SIZE = "45";
            cont.CONT_TYPE = "GP";
            cont.IO = "O";
            cont.ORG = "USER";
            cont.ORG_TYPE = "B";

            list.Add(cont);

            cont = new CONT_INFO();
            cont.ID = 4;
            cont.CONT_SIZE = "20";
            cont.CONT_TYPE = "HQ";
            cont.IO = "O";
            cont.ORG = "USER";
            cont.ORG_TYPE = "A";

            list.Add(cont);


            cont = new CONT_INFO();
            cont.ID = 4;
            cont.CONT_SIZE = "48";
            cont.CONT_TYPE = "HQ";
            cont.IO = "O";
            cont.ORG = "USER";
            cont.ORG_TYPE = "A";

            list.Add(cont);


            return list;
        }


    }
}