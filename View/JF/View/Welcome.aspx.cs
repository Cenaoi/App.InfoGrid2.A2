using App.BizCommon;
using App.InfoGrid2.Model.JF;
using App.InfoGrid2.View;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.JF.View
{
    public partial class Welcome : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               InitData();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {


            string guid_str = WebUtil.Query("guid");

            if (string.IsNullOrWhiteSpace(guid_str))
            {
                Error404.Send("Guid不能为空！");

                return;
            }

            log.Debug("Guid:" + guid_str);

            Guid guid = new Guid(guid_str);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_W_ACCOUNT));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("W_GUID", guid);
            lmFilter.And("GUID_LIMIT_DATE", DateTime.Now, Logic.GreaterThan);


            ES_W_ACCOUNT account = decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilter);

            if (account == null)
            {
                Error404.Send("没有微信授权，不能进来系统！");
                return;
            }

            log.Debug("微信用户登录了！微信昵称：" + account.W_NICKNAME);

            #region  这里设置Cookie code 

            SetCookie(account);

            #endregion

            EcContext ec_context = EcContext.Current;

            EcUserState userState = ec_context.User;

            userState.Clear();

            userState.ExpandPropertys.Clear();

            userState.Identity = account.ES_W_ACCOUNT_ID;
            userState.LoginID = account.W_OPENID;
            userState.LoginName = account.W_NICKNAME;

            userState.Roles.Clear();
            userState.Roles.Add("WX");

            userState.ExpandPropertys["W_CODE"] = account.PK_W_CODE;



        }

        /// <summary>
        /// 设置Cookie USER_GUID 的值  用来自动登录的
        /// </summary>
        /// <param name="account">微信账户对象</param>
        void SetCookie(ES_W_ACCOUNT account)
        {


            HttpCookie cookie = Request.Cookies["USER_GUID"];

            if (cookie == null)
            {

                cookie = new HttpCookie("USER_GUID");

            }

            cookie.Value = account.W_GUID.ToString();
            cookie.Expires = account.GUID_LIMIT_DATE.Value;

           Response.SetCookie(cookie);

        }




    }
}