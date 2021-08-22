using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Transform
{
    public partial class CustList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += store1_CurrentChanged;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            if (e.SrcRecord == null)
            {
                return;
            }

            int cust_id = StringUtil.ToInt(e.SrcRecord.Id);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm005 = decipher.GetModelByPk("UT_005", cust_id);


            string url = $"/App/InfoGrid2/View/Biz/Rosin/Transform/WarehouseList.aspx?cust_text={lm005.Get<string>("CLIENT_TEXT")}";

            MiniPager.Redirect("iform1", url);

        }
    }
}