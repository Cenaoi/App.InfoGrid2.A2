using App.BizCommon;
using App.InfoGrid2.WF.Bll;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View.Report
{
    public partial class ReimList : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 获取当前用户下的所有周报告
        /// </summary>
        /// <returns></returns>
        public string GetReports()
        {

            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_371");
            lmFilter.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_CREATE_USER_CODE", user_code);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE  desc";

            lmFilter.Fields = new string[] { "COl_1", "COL_52" , "COL_5" ,"COL_26", "ROW_IDENTITY_ID","BIZ_SID"};

            SModelList sm371s = decipher.GetSModelList(lmFilter);

            foreach(var item in sm371s)
            {

                item["COL_1"] = BusHelper.FormatDate(item.Get<string>("COL_1"),"yyyy-MM");

            }

            return sm371s.ToJson();

        }

    }
}