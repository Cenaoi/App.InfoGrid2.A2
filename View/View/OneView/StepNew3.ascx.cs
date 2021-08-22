using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using App.InfoGrid2.View.OneTable;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneView
{
    public partial class StepNew3 : WidgetControl, IView
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

            MiniPager.Redirect("StepNew2.aspx?tmp_id=" + op_guid.ToString());
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


            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet ttSet = TmpTableSet.Select(decipher, op_guid);


            TableSet tSet = ttSet.ToTableSet();

            IG2_TABLE tab = tSet.Table;
            tab.ROW_DATE_CREATE = tab.ROW_DATE_UPDATE = DateTime.Now;
            //tab.TABLE_NAME = BillIdentityMgr.NewCodeForNum("USER_VIEW", "UV_", 3);


            foreach (IG2_TABLE_COL col in tSet.Cols)
            {
                col.ROW_DATE_CREATE = col.ROW_DATE_UPDATE = tab.ROW_DATE_CREATE;
            }


            tSet.Insert(decipher);

            //删除临时表数据
            TmpTableSet.Delete(decipher, op_guid);

            int tableId = tSet.Table.IG2_TABLE_ID;

            MiniPager.Redirect("ViewPreview.aspx?id=" + tableId);
        }




    }
}