using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepEdit2 : WidgetControl, IView
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

            string[] idList = StringUtil.Split(ids,",");

            if (idList.Length == 0)
            {
                return;
            }

            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                List<IG2_TABLE> tableList = decipher.SelectModelsIn<IG2_TABLE>("IG2_TABLE_ID", idList);

                foreach (IG2_TABLE item in tableList)
                {
                    IG2_TMP_VIEW_TABLE viewTable = new IG2_TMP_VIEW_TABLE();
                    
                    viewTable.IG2_VIEW_ID = id;

                    viewTable.TABLE_TEXT = item.DISPLAY;
                    viewTable.TABLE_NAME = item.TABLE_NAME;
                    
                    viewTable.TABLE_ID = item.IG2_TABLE_ID;
                    viewTable.RELATION_TYPE = "full";

                    viewTable.ROW_USER_SEQ = 9999;
                    viewTable.TMP_GUID = uid;
                    viewTable.TMP_SESSION_ID = sessionId;

                    decipher.InsertModel(viewTable);
                }

                this.store2.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("选择数据表出错了！", ex);

                MessageBox.Alert("选择数据表出错了.");
            }

        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");
            string sessionId = this.Session.SessionID;

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter(typeof(IG2_TMP_VIEW_TABLE));
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("TMP_GUID", uid);
                filter.And("TMP_SESSION_ID", sessionId);

                int count = decipher.SelectCount(filter);

                if (count < 2)
                {
                    EasyClick.Web.Mini.MiniHelper.Alert("请选择数据表");
                    return;
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex);


            }


            MiniPager.Redirect(string.Format("MViewStepEdit3.aspx?id={0}&uid={1}", id, uid));
        }


        /// <summary>
        /// 上一步
        /// </summary>
        public void GoBack()
        {
            int id = WebUtil.QueryInt("id");
            Guid uid = WebUtil.QueryGuid("uid");


            string url = string.Format("MViewStepEdit1.aspx?id={0}&uid={1}", id,uid);

            MiniPager.Redirect(url);

        }

    }
}