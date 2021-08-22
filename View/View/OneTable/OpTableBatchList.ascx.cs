using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Xml;
using System;
using System.Collections.Generic;

namespace App.InfoGrid2.View.OneTable
{
    public partial class OpTableBatchList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.table1.Command += Table1_Command;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            if(e.CommandName == "GoEdit")
            {
                GoEdit(e);
            }
        }


        private void GoEdit(TableCommandEventArgs e)
        {
            DataRecord record = e.Record;

            EcView.Show($"/App/InfoGrid2/View/OneTable/OpTableBatch.aspx?id={e.Record.Id}","批量修改字段");


        }
    }
}