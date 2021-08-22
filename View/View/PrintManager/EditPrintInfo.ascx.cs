using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.PrintManager
{
    public partial class EditPrintInfo : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(store1_CurrentChanged);

            if(!IsPostBack)
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


            LModel lm = (LModel)e.Object;

            string guid = lm["PCLIENT_GUID"].ToString();

            DbDecipher decipher = ModelAction.OpenDecipher();
            List<BIZ_PRINT_NAME> ItemList = decipher.SelectModels<BIZ_PRINT_NAME>("PCLIENT_GUID='{0}'", guid);

            this.store2.RemoveAll();
            this.store2.AddRange(ItemList); 

        }
    }
}