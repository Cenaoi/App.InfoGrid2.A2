using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Explorer
{
    public partial class SysHeartbeat : System.Web.UI.Page,IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Context.Items["ResponseJS"] = true;
            this.Context.Items["EC_NoLog"] = true;

            Response.Clear();

            string tag = Request.QueryString["tag"];

            if (tag == "USER_INFO")
            {
                EcUserState user = EC5.SystemBoard.EcContext.Current.User;

                Response.Write("{");

                Response.Write(string.Format("time:'{0}',", DateTime.Now.ToString("HH:mm")));
                Response.Write(string.Format("loginName:'{0}',", user.LoginName));
                Response.Write(string.Format("roleName:'{0}',", user.FirstRoleName));
                
                Response.Write($"loign:{(user.Roles.Count > 0?1:0)},");
                
                Response.Write($"isVirtual:{(user.IsVirtual ? "1" : "0")}");
                

                Response.Write("}");
            }
            else
            {
                Response.Write(DateTime.Now.ToString("HH:mm"));
            }


        }
    }
}