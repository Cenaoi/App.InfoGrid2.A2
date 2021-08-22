using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini;
using EC5.Utility;
using App.InfoGrid2.Model.DataSet;

namespace App.InfoGrid2.View.OneBuilder
{
    public partial class ActionSample_Search : System.Web.UI.Page,IView
    {
        protected override void OnInit(EventArgs e)
        {

            int pageViewId = WebUtil.QueryInt("PageViewId");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, pageViewId);



            Define_Search(tSet.Cols);


            base.OnInit(e);
        }



        protected void Page_Load(object sender, EventArgs e)
        {

        }



        #region 定义查询框

        /// <summary>
        /// 定义查询框
        /// </summary>
        /// <param name="list"></param>
        private void Define_Search(List<IG2_TABLE_COL> list)
        {
            ControlCollection cons = this.Search1.Fields;

            foreach (IG2_TABLE_COL item in list)
            {
                if (!item.IS_VISIBLE || !item.IS_SEARCH_VISIBLE)
                {
                    continue;
                }

                Control con = CreateSearchControl(item);

                cons.Add(con);
            }

            Button btn = new Button();
            btn.Value = "查询";
            btn.Command = "Search";
            btn.SetAttribute("style", "width:80px;height:30px;");

            cons.Add(btn);
        }


        /// <summary>
        /// 创建查询子控件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Control CreateSearchControl(IG2_TABLE_COL item)
        {
            Control con = null;

            switch (item.DB_TYPE)
            {
                case "decimal":
                    {
                        NumberRangeBox tb = new NumberRangeBox();
                        con = tb;
                    }
                    break;
                case "datetime":
                    {
                        DateRangePicker drp = new DateRangePicker();
                        //drp.SetAttribute("ColSpan", "3");
                        con = drp;
                    }
                    break;
                case "boolean":
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.Items.Add("N/A", "");
                        ddl.Items.Add("True", "True");
                        ddl.Items.Add("False", "False");
                        con = ddl;
                    }
                    break;
                case "string":
                default:
                    {
                        TextBox tb = new TextBox();
                        con = tb;
                    }
                    break;
            }



            IAttributeAccessor attr = (IAttributeAccessor)con;

            string headerText = GetHeaderText(item.DISPLAY, item.F_NAME);


            con.ID = "Search" + item.DB_FIELD;

            attr.SetAttribute("HeaderText", headerText);
            attr.SetAttribute("DBField", item.DB_FIELD);
            attr.SetAttribute("DBLogic", "like");

            return con;
        }

        #endregion

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="display"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        public string GetHeaderText(string display, string fName)
        {
            if (!StringUtil.IsBlank(display))
            {
                return display;
            }

            return fName;
        }


    }
}