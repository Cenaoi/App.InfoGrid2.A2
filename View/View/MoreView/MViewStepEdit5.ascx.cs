using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.MoreView
{
    public partial class MViewStepEdit5 : WidgetControl, IView
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
                //this.storeTab.DataBind();
                this.store1.DataBind();
            }
        }

        public void GoLast()
        {
            int id = WebUtil.QueryInt("id");

            string urlStr = string.Format("MoreViewPreview.aspx?id={0}", id);

            MiniPager.Redirect(urlStr);

        }


        public void GoCancel()
        {
            int id = WebUtil.QueryInt("id");

            string urlStr = string.Format("MoreViewPreview.aspx?id={0}", id);

            MiniPager.Redirect(urlStr);

        }
    }
}