using App.BizCommon;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Explorer
{
    public partial class SysHeartbeat : System.Web.UI.Page,IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Context.Items["ResponseJS"] = true;
            this.Context.Items["EC_NoLog"] = true;

            Response.Clear();

            string tag = Request.QueryString["tag"];

            if (tag == "USER_INFO")
            {
                EcUserState user = EC5.SystemBoard.EcContext.Current.User;

                DbDecipher decipher = ModelAction.OpenDecipher();
                int loginOnly = GlobelParam.GetValue(decipher, "LoginOnly", 1, "登录唯一，0=关闭，1=打开");

                bool loginVaild = user.Identity != 0;
                bool loginState = false;

                if (loginVaild && loginOnly == 1)
                {
                    Hashtable onlineActive = (Hashtable)System.Web.HttpContext.Current.Application["onlineActive"];
                    if (onlineActive == null)
                    {
                        onlineActive = new Hashtable();
                    }
                    Hashtable online = (Hashtable)System.Web.HttpContext.Current.Application["online"];
                    if (online == null)
                    {
                        online = new Hashtable();
                    }

                    string sessionId = System.Web.HttpContext.Current.Session.SessionID;
                    if (onlineActive.ContainsKey(user.Identity) && online.ContainsKey(user.Identity))
                    {
                        if (sessionId == (string)online[user.Identity])
                        {
                            onlineActive[user.Identity] = DateTime.Now;

                            loginState = true;
                        }
                        else 
                        {
                            log.Debug($"sessionId变化，{onlineActive[user.Identity]} > {sessionId}");
                        }
                    }

                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["onlineActive"] = onlineActive;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                Response.Write("{");

                Response.Write(string.Format("time:'{0}',", DateTime.Now.ToString("HH:mm")));
                Response.Write(string.Format("loginName:'{0}',", user.LoginName));
                Response.Write(string.Format("roleName:'{0}',", user.FirstRoleName));
                
                Response.Write($"loign:{(user.Roles.Count > 0 ? 1:0)},");
                
                Response.Write($"isVirtual:{(user.IsVirtual ? "1" : "0")},");
                Response.Write($"loginState:{(loginState ? "1" : "0")},");
                Response.Write($"loginVaild:{(loginVaild ? "1" : "0")}");

                Response.Write("}");
            }
            else
            {
                Response.Write(DateTime.Now.ToString("HH:mm"));
            }


        }
    }
}