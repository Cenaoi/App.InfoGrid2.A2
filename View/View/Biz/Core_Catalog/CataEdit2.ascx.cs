using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Core_Catalog
{
    public partial class CataEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.SavedAll += store1_SavedAll;

            if (!IsPostBack)
            {
               
                this.store1.DataBind();
            }
        }

        void store1_SavedAll(object sender, ObjectListEventArgs e)
        {

            Toast.Show("保存成功");
        }
    }
}