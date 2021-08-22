using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using EC5.Utility.Web;
using EasyClick.BizWeb2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Filter;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepNew2 : WidgetControl, IView
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
                this.store2.DataBind();
            }
        }





        /// <summary>
        /// 插入选择的表名
        /// </summary>
        public void InsertTable(string ids)
        {
            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            string[] id = StringUtil.Split(ids, ",");

            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                List<IG2_TABLE> tableList = decipher.SelectModelsIn<IG2_TABLE>("IG2_TABLE_ID",id);

                foreach(IG2_TABLE item in tableList)
                {
                    IG2_TMP_VIEW_TABLE viewTable = new IG2_TMP_VIEW_TABLE();

                    viewTable.TABLE_ID = item.IG2_TABLE_ID;
                    viewTable.TABLE_NAME = item.TABLE_NAME;
                    viewTable.TABLE_TEXT = item.DISPLAY;

                    viewTable.RELATION_TYPE = "full";

                    viewTable.TMP_SESSION_ID = sessionId;
                    viewTable.TMP_GUID = uid;

                    decipher.InsertModel(viewTable);
                }

                this.store2.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("选择数据表出错了！",ex);
                MessageBox.Alert("选择数据表出错了!");
            }
           
        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            int cataId = WebUtil.QueryInt("catalog_id", 104);

            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW_TABLE));
                filter.And("TMP_GUID", uid);
                filter.And("TMP_SESSION_ID", sessionId);
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                int count = decipher.SelectCount(filter);

                if (count < 2)
                {
                    MessageBox.Alert("最少必须选择两个数据表。");
                    return;
                }


                string url = string.Format("MViewStepNew3.aspx?uid={0}&catalog_id={1}", uid, cataId);

                MiniPager.Redirect(url);
            }
            catch (Exception ex) 
            {
                log.Error(ex);

                MessageBox.Alert("跳转失败:" + ex.Message);
            }
            

        }


        /// <summary>
        /// 上一步
        /// </summary>
        public void GoBack()
        {
            Guid uid = WebUtil.QueryGuid("uid");
            string url = string.Format("MViewStepNew1.aspx?uid={0}", uid);

            MiniPager.Redirect(url);

        }



    }
}