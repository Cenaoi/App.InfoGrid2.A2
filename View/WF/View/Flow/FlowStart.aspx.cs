using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View.Flow
{
    public partial class FlowStart : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 获取当前用户未审核数据集合
        /// </summary>
        /// <returns></returns>
        public string GetOnCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserStartDocs(0, userCode, string.Empty);


            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();

                int flow_sid = sm.Get<int>("FLOW_SID");

                if (flow_sid != 2)
                {
                    continue;
                }

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"];
                new_sm["text"] = "进行中";

                if (sm.HasField("EXTEND_DOC_URL") && sm.HasField("EXTEND_TABLE"))
                {
                    string tabName = sm["EXTEND_TABLE"];
                    int menuId = sm["EXTEND_MENU_ID"];

                    string newUrl = FlowUrlMgr.Get("mobile", 0, tabName, sm);

                    new_sm["url"] = StringUtil.NoBlank(newUrl, sm["EXTEND_DOC_URL"]);
                }

              

                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();


        }

        /// <summary>
        ///  获取当前用户已审核数据集合
        /// </summary>
        /// <returns></returns>
        public string GetYesCheckArray()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserStartDocs(2, userCode, string.Empty);


            SModelList new_docs = new SModelList();

            foreach (SModel sm in docs)
            {
                SModel new_sm = new SModel();

                int flow_sid = sm.Get<int>("FLOW_SID");

                if (flow_sid != 4)
                {
                    continue;
                }

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = sm["EXTEND_DOC_TEXT"];
                new_sm["text"] = "审核已通过";

                if (sm.HasField("EXTEND_DOC_URL") && sm.HasField("EXTEND_TABLE"))
                {
                    string tabName = sm["EXTEND_TABLE"];
                    int menuId = sm["EXTEND_MENU_ID"];

                    string newUrl = FlowUrlMgr.Get("mobile", 0, tabName, sm);

                    new_sm["url"] = StringUtil.NoBlank(newUrl, sm["EXTEND_DOC_URL"]);
                }

                new_docs.Add(new_sm);

            }

            return new_docs.ToJson();

        }

        /// <summary>
        /// 获取制单中数据
        /// </summary>
        /// <returns></returns>
        public string GetDraftArray()
        {
            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_346");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("ROW_AUTHOR_USER_CODE", user_code);
            lmFilter.And("BIZ_CREATE_USER_CODE", user_code);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<LModel> lm346s = decipher.GetModelList(lmFilter);


            LightModelFilter lmFilter371 = new LightModelFilter("UT_371");
            lmFilter371.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter371.And("BIZ_SID", 0);
            lmFilter371.And("BIZ_CREATE_USER_CODE", user_code);
            lmFilter371.TSqlOrderBy = "ROW_DATE_CREATE  desc";

            lmFilter371.Fields = new string[] { "ROW_IDENTITY_ID", "ROW_DATE_CREATE" };

            SModelList sm371s = decipher.GetSModelList(lmFilter371);

            SModelList sms = new SModelList();
            
            foreach(LModel lm in lm346s)
            {

                SModel new_sm = new SModel();

                new_sm["create_date"] = lm["ROW_DATE_CREATE"];
                new_sm["title"] = "月度报销单";
                new_sm["text"] = "制单中";
                new_sm["url"] = "/WF/View/Reim/Reimbur.aspx?id="+lm["ROW_IDENTITY_ID"];

                sms.Add(new_sm);

            }


            foreach(var sm in sm371s)
            {

                SModel new_sm = new SModel();

                new_sm["create_date"] = sm["ROW_DATE_CREATE"];
                new_sm["title"] = "周报告";
                new_sm["text"] = "制单中";
                new_sm["url"] = "/WF/View/Report/ReimDeta.aspx?id=" + sm["ROW_IDENTITY_ID"]+"&show_type=edit";

                sms.Add(new_sm);

            }


            return sms.ToJson();

        }

    }
}