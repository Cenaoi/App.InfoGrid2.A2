using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.WF.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// HomeHandler 的摘要说明
    /// </summary>
    public class HomeHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            EcUserState user = EcContext.Current.User;

            HttpResult result = null;


            try
            {


                string action = WebUtil.FormTrimUpper("action");

                switch (action)
                {

                    case "NEW_REIM":
                        result = NewReim();
                        break;

                    case "INIT_DATA_HOME":
                        result = InitDataHome();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result = HttpResult.Error(ex.Message);

            }


            context.Response.Write(result);


        }

        /// <summary>
        /// 新增一条
        /// </summary>
        HttpResult NewReim()
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

            LModel lm001 = decipher.GetModel(lmFilter);

            if (lm001 != null)
            {
                return HttpResult.Success(lm001);

            }


            LModel lm = new LModel("UT_346");

            lm["COL_1"] = DateTime.Now;
            lm["ROW_SID"] = 0;
            lm["ROW_DATE_CREATE"] = lm["ROW_DATE_UPDATE"] = lm["BIZ_CREATE_DATE"] = lm["BIZ_UPDATE_DATE"] = DateTime.Now;
            lm["ROW_AUTHOR_USER_CODE"] = user_code;
            lm["BIZ_CREATE_USER_CODE"] = user_code;
            lm["BIZ_CREATE_USER_TEXT"] = user.LoginName;
            lm["ROW_USER_SEQ"] = 100000000m;
            lm["COL_44"] = "草稿";
            lm["COL_35"] = "报销单";
            lm["COL_36"] = "150";
            lm["COL_38"] = "费用报销";
            lm["COL_39"] = "102";
            lm["COL_17"] = "是";
            lm["COL_5"] = user.LoginName;
            lm["COL_46"] = "等待核对";
            lm["COL_20"] = "草稿";
            lm["COL_21"] = "101";
            lm["COL_41"] = "制单中";
            lm["COL_51"] = user_code;
            lm["COL_3"] = DateTime.Now.Year;
            lm["COL_4"] = DateTime.Now.Month;


            LightModelFilter lmFilter116 = new LightModelFilter("UT_116");
            lmFilter116.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter116.And("COL_19", user_code);


            LModel lm116 = decipher.GetModel(lmFilter116);

            //不等于空的时候才处理
            if (lm116 != null)
            {

                lm["COL_27"] = lm116["COL_1"];

                lm["COL_8"] = lm116["COL_13"];
                lm["COL_9"] = lm116["COL_12"];

                lm["COL_10"] = lm116["COL_11"];
                lm["COL_11"] = lm116["COL_16"];
                lm["COL_7"] = lm116["COL_14"];

                lm["COL_6"] = lm116["COL_15"];

            }

            decipher.InsertModel(lm);

            return HttpResult.Success(lm);

        }



        HttpResult InitDataHome()
        {
            SModel result = new SModel();

            result["eaa_num"] = GetNoCheckNum();
            result["cc_num"] = GetCCNum();
            result["user_obj"] = GetUserInfo();
            result["draft_num"] = GetDraftNum();
            result["login_name"] = GetLoginNameStr();

            return HttpResult.Success(result);
        }



        /// <summary>
        /// 获取未审核数量
        /// </summary>
        /// <returns></returns>
        public int GetNoCheckNum()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserDocs(0, userCode, string.Empty);

            return docs.Count();

        }


        /// <summary>
        /// 获取没读的抄送记录
        /// </summary>
        /// <returns></returns>
        public int GetCCNum()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetInstCopys(0, userCode, "IS_OPEN = 0");

            return docs.Count();


        }

        /// <summary>
        /// 获取登录账号信息  从 dbo.UT_116  里面取
        /// </summary>
        /// <returns></returns>
        public SModel GetUserInfo()
        {

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_116");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_19", user_code);

            LModel lm116 = decipher.GetModel(lmFilter);

            if (lm116 == null)
            {
                return null;
            }

            SModel sm = new SModel();

            lm116.CopyTo(sm);

            return sm;
        }


        /// <summary>
        /// 获取制单中的数据
        /// </summary>
        /// <returns></returns>
        public int GetDraftNum()
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

            int num = decipher.SelectCount(lmFilter);


            LightModelFilter lmFilter371 = new LightModelFilter("UT_371");
            lmFilter371.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter371.And("BIZ_SID", 0);
            lmFilter371.And("BIZ_CREATE_USER_CODE", user_code);
            lmFilter371.TSqlOrderBy = "ROW_DATE_CREATE  desc";

            lmFilter371.Fields = new string[] { "ROW_IDENTITY_ID", "ROW_DATE_CREATE" };

            int num_1 = decipher.SelectCount(lmFilter371);


            return num + num_1;


        }


        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public string GetLoginNameStr()
        {

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            StringBuilder sb = new StringBuilder();

            if (DateTime.Now.Hour < 12)
            {
                sb.Append("上午好，");

            }

            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                sb.Append("下午好，");
            }

            if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 24)
            {
                sb.Append("晚上好，");
            }


            sb.Append(user.LoginName);


            return sb.ToString();

        }






        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}