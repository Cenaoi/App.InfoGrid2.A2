using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.DbSetup
{
    public partial class ClearRubbish : WidgetControl, IView
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
                OnInitData();
            }
        }

        private void OnInitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int count1 = decipher.SelectCount<BIZ_PRINT_HEARTBEAT>("ROW_SID = -3");
        }

        public void GoNext1()
        {

        }


    }
}