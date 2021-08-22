using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5;
using EC5.AppDomainPlugin;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace App.InfoGrid2.View.Biz.Rosin2.Plan
{
    public partial class PlanListBizF3 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

            if (!this.IsPostBack)
            {
                OnInitData();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void OnInitData()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            string title = io_tag == "I" ? "入库计划管理-回收站" : "出库计划管理-回收站";
            
            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }

        
        public void GoReset()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            DataRecordCollection records = this.table1.CheckedRows;

            if(records.Count == 0)
            {
                return;
            }

            foreach (var record in records)
            {
                LModel model = decipher.GetModelByPk("UT_015_PLAN", record.Id);
                model.SetTakeChange(true);

                model["BIZ_SID"] = 0;
                model["BIZ_DELETE_USER_CODE"] = BizServer.LoginName;

                decipher.UpdateModel(model, true);
            }

            this.store1.Refresh();

            Toast.Show("还原成功");
        }

    }
}