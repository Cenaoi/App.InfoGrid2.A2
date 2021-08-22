using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini2.Data;
using EasyClick.Web.Mini;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class ShowTableAll : WidgetControl, IView
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


            if (this.table1.CheckedRows.Count <= 0)
            {
                MiniHelper.Alert("没有选择数据表！");
                return;
            }


            List<string> idList = new List<string>();

            DataRecordCollection  drc  = this.table1.CheckedRows;
            
            foreach(DataRecord dr in drc)
            {
                idList.Add(dr.Id);
            }


            string ids = string.Join(",", idList.ToArray());



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'" + ids + "'});");
        }


    }
}