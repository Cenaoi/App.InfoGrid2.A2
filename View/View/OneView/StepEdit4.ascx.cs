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


namespace App.InfoGrid2.View.OneView
{
    public partial class StepEdit4 : WidgetControl, IView
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
                this.storeTab.DataBind();
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

            TableBufferMgr.Remove(id);
            EC5.IG2.Core.UI.M2VFieldHelper.VModels.Remove(id);

            MiniPager.Redirect("ViewPreview.aspx?id=" + id);
        }


        /// <summary>
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {
            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("TABLE_ID", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);


            string urlStr;

            if (toolbarId > 0)
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={0}&table_id={1}", toolbarId, id);
            }
            else
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={0}", id);
            }

            MiniPager.Redirect(urlStr);
        }

    }
}