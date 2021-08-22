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
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.View.OneTable;
using EasyClick.BizWeb2;


namespace App.InfoGrid2.View.OneView
{
    public partial class StepEdit3 : WidgetControl, IView
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
                this.storeTable.DataBind();
                this.store1.DataBind();
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void GoCancel()
        {
            int id = WebUtil.QueryInt("id");

            MiniPager.Redirect("ViewPreview.aspx?id=" + id);
        }

        /// <summary>
        /// 跳转到最后
        /// </summary>
        public void GoLast()
        {
            int id = WebUtil.QueryInt("id");
            MiniPager.Redirect("ViewPreview.aspx?id=" + id);
        }


    }
}