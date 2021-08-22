using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini;
using EasyClick.Web.Mini2.Data;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneMap
{
    public partial class OneMapList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.store1.Deleting += new EasyClick.Web.Mini2.ObjectCancelEventHandler(store1_Deleting);

            if(!IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        /// <summary>
        /// 应用新规则
        /// </summary>
        public void GoApplyRuleAll()
        {
            App.InfoGrid2.Bll.MapMgr.ClearBufferAll();
        }


        /// <summary>
        /// 删除之前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void store1_Deleting(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            int id = (int)lm["IG2_MAP_ID"];


            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.BeginTransaction();

            try
            {
                decipher.UpdateProps<IG2_MAP_COL>(string.Format("IG2_MAP_ID={0}", id), new object[] { "ROW_SID",-3,"ROW_DATE_DELETE", DateTime.Now });

                decipher.TransactionCommit();
            }
            catch (Exception ex) 
            {
                decipher.TransactionRollback();

                log.Error("删除数据出错！",ex);

                e.Cancel = true;
            }
        }



        /// <summary>
        /// 新增复制信息
        /// </summary>
        public void NewRow() 
        {
            MiniPager.Redirect("MapStepNew1.aspx");

            
        }


    }
}