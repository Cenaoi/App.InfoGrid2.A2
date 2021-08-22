using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using App.InfoGrid2.Bll;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneTable
{
    public partial class StepNew4 : WidgetControl, IView
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
                this.store1.DataBind();
            }
        }

        public void GoPre()
        {
            Guid op_guid = WebUtil.QueryGuid("tmp_id");

            MiniPager.Redirect("StepNew3.aspx?tmp_id=" + op_guid.ToString());
        }

        public void GoNext()
        {
            Guid op_guid = WebUtil.QueryGuid("tmp_id");

            MiniPager.Redirect("StepNew4.aspx?tmp_id=" + op_guid.ToString());
        }

        /// <summary>
        /// 完成，最后一步。创建数据表
        /// </summary>
        public void GoLast()
        {
            Guid op_guid = WebUtil.QueryGuid("tmp_id");

            int tableId = TmpTableMgr.TempTable2Table(op_guid);
            InserMenu(tableId);

            MiniPager.Redirect("TablePreview.aspx?id=" + tableId);

        }

        /// <summary>
        /// 创建表菜单
        /// </summary>
        /// <param name="id"></param>
        void InserMenu(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

            LModel lm = new LModel("BIZ_C_MENU");
            lm["URI"] = string.Format("/app/infogrid2/view/onetable/tablepreview.aspx?id={0}", id);
            lm["PARENT_ID"] = 80;
            lm["NAME"] = it.DISPLAY;
            lm["MENU_ENABLED"] = true;
            lm["SEC_PAGE_ID"] = id;
            lm["SEC_PAGE_TYPE_ID"] = "TABLE";

            decipher.InsertModel(lm);

        }




    }
}