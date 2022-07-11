using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
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

                bool loginVaild = false;
                bool loginState = false;
                bool login = true;
                if (user.Identity != 0)
                {
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
                    else
                    {
                        login = false;

                        log.Debug($"登录无效，userId：{user.Identity}，onlineActive：{onlineActive.Count}，online：{online.Count}");
                    }

                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["onlineActive"] = onlineActive;
                    System.Web.HttpContext.Current.Application.UnLock();

                    loginVaild = true;
                }

                Response.Write("{");

                Response.Write(string.Format("time:'{0}',", DateTime.Now.ToString("HH:mm")));
                Response.Write(string.Format("loginName:'{0}',", user.LoginName));
                Response.Write(string.Format("roleName:'{0}',", user.FirstRoleName));
                
                Response.Write($"loign:{(user.Roles.Count > 0 && login ? 1:0)},");
                
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