using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.Biz.JLSLBZ.DLMX
{
    public partial class TableDlmx : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.StoreUT103.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(StoreUT103_CurrentChanged);

            if (!this.IsPostBack)
            {
                this.StoreUT103.DataBind();
            }
        }

        void StoreUT103_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }

            string billNo = model.Get<string>("COL_16");

            StoreUT102.FilterParams.Add(new Param("COL_17", billNo));

            StoreUT102.DataBind();

        }
    }
}