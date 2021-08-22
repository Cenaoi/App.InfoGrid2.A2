using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EC5.Utility;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneValid
{
    public partial class ValidStepEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.store1.DataBind();
                this.storeTab.DataBind();
            }
        }

        public void GoLast()
        {
            string src_url = WebUtil.QueryBase64("src_url");

            if (!StringUtil.IsBlank(src_url))
            {
                MiniPager.Redirect(src_url);
            }
        }

    }
}