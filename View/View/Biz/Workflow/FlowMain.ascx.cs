using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Workflow
{
    public partial class FlowMain : WidgetControl, IView
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.StoreUT091.CurrentChanged += StoreUT091_CurrentChanged;


            if (!IsPostBack)
            {

                this.StoreUT091.DataBind();

            }

        }

        private void StoreUT091_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
             LModel  lm =  e.Object as LModel;


            if(lm == null)
            {
                return;
            }


            int id = (int) lm.GetPk() ;


            MiniPager.Redirect("iform1", "/App/InfoGrid2/View/Biz/Workflow/FlowShow.aspx?id="+id);

        }
    }
}