using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using EasyClick.Web.Mini2.Data;
using EasyClick.BizWeb2;
using EC5.Utility;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneToolbar
{
    public partial class SetToolbar : WidgetControl, IView
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
                this.store2.DataBind();
            }
            
        }

        public void GoLast()
        {
            string src_url = WebUtil.QueryBase64("src_url");

            if (!StringUtil.IsBlank(src_url))
            {
                MiniPager.Redirect(src_url);
            }
            else
            {
                int tableId = WebUtil.QueryInt("table_id");

                MiniPager.Redirect("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id=" + tableId);
            }
        }




    }
}