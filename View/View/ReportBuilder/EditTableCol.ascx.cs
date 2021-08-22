using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class EditTableCol : WidgetControl, IView
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

                this.StoreTableCol.DataBind();
            }
        }


        /// <summary>
        /// 确定按钮
        /// </summary>
        public void Center()
        {
            MessageBox.Alert("我也不知道这个按钮是干什么的！");
        }

    }
}