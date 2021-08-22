using EC5.SystemBoard;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.InfoGrid2.WF.Bll;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.WF.View
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            if(DateTime.Now.Hour < 12)
            {
                sb.Append("上午好，");

            }

            if(DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                sb.Append("下午好，");
            }

            if(DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 24)
            {
                sb.Append("晚上好，");
            }


            sb.Append(user.LoginName);


            return sb.ToString();
            
        }


        /// <summary>
        /// 获取未审核数量
        /// </summary>
        /// <returns></returns>
        public string GetNoCheckNum()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetUserDocs(0, userCode, string.Empty);

            return docs.Count().ToString();

        }


        /// <summary>
        /// 获取没读的抄送记录
        /// </summary>
        /// <returns></returns>
        public string GetCCNum()
        {

            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetInstCopys(0, userCode, "IS_OPEN = 0");

            return docs.Count().ToString();


        }

        /// <summary>
        /// 获取登录账号信息  从 dbo.UT_116  里面取
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo()
        {

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_116");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_19", user_code);


            LModel lm116 = decipher.GetModel(lmFilter);

            if(lm116 == null)
            {

                return "{}";

            }

            SModel sm = new SModel();

            lm116.CopyTo(sm);

            return sm.ToJson();




        }


        /// <summary>
        /// 获取制单中的数据
        /// </summary>
        /// <returns></returns>
        public string GetDraftNum()
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


            return (num + num_1).ToString();
            

        }



    }
}