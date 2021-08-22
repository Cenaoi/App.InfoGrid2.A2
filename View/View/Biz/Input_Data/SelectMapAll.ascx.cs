using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.Biz.Input_Data
{
    public partial class SelectMapAll : WidgetControl, IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);

        }


        /// <summary>
        /// 选择表
        /// </summary>
        public void SelectTable()
        {


            if (this.table1.CheckedRows.Count == 0)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("没有选择数据表！");
                return;
            }


          
            DataRecordCollection drc = this.table1.CheckedRows;


            string id = drc[0].Id;



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',id:'" + id + "'});");
        }
    }
}