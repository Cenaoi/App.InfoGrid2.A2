using EC5.SystemBoard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Login
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EC5.SystemBoard.EcContext content = EcContext.Current;

            EcUserState user = content.User;

            Hashtable online = (Hashtable)System.Web.HttpContext.Current.Application["online"];
            if (online == null)
            {
                online = new Hashtable();
            }

            if (online.ContainsKey(user.Identity))
            {
                online.Remove(user.Identity);
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application["online"] = online;
                System.Web.HttpContext.Current.Application.UnLock();
            }

            user.Clear();

            Response.Redirect("/App/InfoGrid2/Login/Index2.aspx", false);
        }
    }
}