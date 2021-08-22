using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.Model.SecModels;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// 登录相关的处理类
    /// </summary>
    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = WebUtil.FormTrimUpper("action");


            HttpResult result = null;



            try
            {

                switch (action)
                {

                    case "LOGIN":
                        result = Login();
                        break;
                    case "LOGOUT":
                        result = LogOut();
                        break;


                }

            }catch(Exception ex)
            {
                log.Error(ex);

                result = HttpResult.Error(ex.Message);

            }

            context.Response.Write(result);

        }

        HttpResult Login()
        {

            string login_name = WebUtil.Form("login_name");

            string login_pass = WebUtil.Form("login_pass");


            if (string.IsNullOrWhiteSpace(login_name))
            {
                return HttpResult.Error("登录名称不能为空！");
            }


            if (string.IsNullOrWhiteSpace(login_pass))
            {

                return HttpResult.Error("登录密码不能为空！");

            }


            LoginMgr login_mrg = new LoginMgr();

            bool flag = login_mrg.Login(login_name, login_pass);

            if (!flag)
            {

                return HttpResult.Error("登录名或密码不正确！");

            }


            login_mrg.GetUserByLoginName(login_name);


            EcUserState user = EcContext.Current.User;

            string user_code = user.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT_EX));
            lmFilter.And("FK_ACCOUNT_CODE", user_code);


            SEC_LOGIN_ACCOUNT_EX account_ex = decipher.SelectToOneModel<SEC_LOGIN_ACCOUNT_EX>(lmFilter);

            if(account_ex == null)
            {
                account_ex = new SEC_LOGIN_ACCOUNT_EX();

                account_ex.FK_ACCOUNT_CODE = user_code;
                account_ex.PK_ACCOUNT_EX_CODE = BillIdentityMgr.NewCodeForNum("ACCOUNT_EX_CODE", "EX_",3);

                account_ex.LOGIN_GUID = Guid.NewGuid();

                account_ex.GUID_LIMIT_DATE = DateTime.Now.AddDays(7);
                account_ex.LOGIN_NAME = login_name;

                decipher.InsertModel(account_ex);

            }

            #region  每次登陆进来都赋值一个新的GUID给用户cookie中

            account_ex.LOGIN_GUID = Guid.NewGuid();

            account_ex.GUID_LIMIT_DATE = DateTime.Now.AddDays(7);

            decipher.UpdateModelProps(account_ex, "LOGIN_GUID", "GUID_LIMIT_DATE");

            SetCookie(account_ex);

            #endregion


            return HttpResult.SuccessMsg("登录成功了！");

        }

        /// <summary>
        /// 退出系统函数
        /// </summary>
        /// <returns></returns>
        HttpResult LogOut()
        {

            EcUserState user = EcContext.Current.User;





            user.Clear();

            
            return HttpResult.SuccessMsg("注销账户成功了！");

        }

        /// <summary>
        /// 设置Cookie USER_GUID 的值  用来自动登录的
        /// </summary>
        /// <param name="account">微信账户对象</param>
        void SetCookie(SEC_LOGIN_ACCOUNT_EX account)
        {

            HttpCookie cookie = HttpContext.Current.Request.Cookies["USER_GUID"];

            if (cookie == null)
            {

                cookie = new HttpCookie("USER_GUID");

            }

            cookie.Value = account.LOGIN_GUID.ToString();
            cookie.Expires = account.GUID_LIMIT_DATE;

            HttpContext.Current.Response.SetCookie(cookie);

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