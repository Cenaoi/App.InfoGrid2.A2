using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;



namespace App.InfoGrid2.View.OneForm
{
    public partial class FlowSetup : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";



            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }


    }
}