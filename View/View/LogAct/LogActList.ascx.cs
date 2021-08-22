using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;

namespace App.InfoGrid2.View.LogAct
{
    public partial class LogActList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {

            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.store1.DataBind();
            }
        }
    }
}