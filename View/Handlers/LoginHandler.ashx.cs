using App.BizCommon;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";


            string action = WebUtil.FormTrimUpper("action");


            try {

            
                switch (action)
                {

                    case "LOGIN":
                        Login(context);
                        break;

                }

            }catch(Exception ex)
            {
                log.Error(ex);

                SModel sm = new SModel();
                sm["result"] = "error";
                sm["msg"] = "哦噢，请联系系统管理员！";

                context.Response.Write(sm.ToJson());
                return;
            }
           

            context.Response.End();

        }
        

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="context">http对象</param>
        /// <param name="decipher">数据库帮助对象</param>
        void Login(HttpContext context)
        {
            string login_name = WebUtil.FormTrim("login_name");

            string login_pass = WebUtil.FormTrim("login_pass");

            string wei_xin_id = WebUtil.FormTrim("wei_xin_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("VIP_ACCOUNT");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("LOGIN_NAME", login_name);
            lmFilter.And("LOGIN_PASS", login_pass);

            LModel lmVip = decipher.GetModel(lmFilter);

            if (lmVip == null)
            {

                SModel sm1 = new SModel();

                sm1["result"] = "error";
                sm1["msg"] = "账号或密码不对，请重新输入！";
                context.Response.Write(sm1.ToJson());

                return;
            }


            lmVip["WE_XIN_ID"] = wei_xin_id;
            decipher.UpdateModelProps(lmVip, "WE_XIN_ID");


            EcContext my_context = EcContext.Current;

            EcUserState userState = my_context.User;
            userState.Clear();
            userState.ExpandPropertys.Clear();

            userState.Identity = Convert.ToInt32(lmVip.GetPk());
            userState.LoginID = lmVip.Get<string>("LOGIN_NAME") ;
            userState.LoginName = lmVip.Get<string>("NICKNAME");

            userState.Roles.Clear();
            userState.Roles.Add("VIP");

            SModel sm = new SModel();

            sm["result"] = "ok";
            sm["msg"] = "登录成功了！";

            context.Response.Write(sm.ToJson());

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