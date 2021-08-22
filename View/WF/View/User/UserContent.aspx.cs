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

namespace App.InfoGrid2.WF.View.User
{
    public partial class UserContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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

            if (lm116 == null)
            {
                return "{}";

            }

            SModel sm = new SModel();

            lm116.CopyTo(sm);

            return sm.ToJson();




        }


    }
}