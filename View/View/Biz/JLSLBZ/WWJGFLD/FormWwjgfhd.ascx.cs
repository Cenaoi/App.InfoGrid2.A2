using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;

namespace App.InfoGrid2.View.Biz.JLSLBZ.WWJGFLD
{
    public partial class FormWwjgfhd : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                this.StoreUT137.DataBind();
            }
        }


        public void GoLast() 
        {
            DataRecordCollection drc = this.table1.CheckedRows;

            if (drc.Count == 0)
            {
                MessageBox.Alert("请选择记录！");
                return;
            }

            string ids = "";
            for (int i = 0; i < drc.Count; i++)
            {
                if (i > 0)
                {
                    ids += ",";
                }

                ids += drc[i].Id;
            }


            EasyClick.Web.Mini.MiniHelper.EvalFormat("ownerWindow.close({{result:'ok',ids:'{0}'}});", ids);
        }

    }
}