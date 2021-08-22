using EC5.SystemBoard;
using System;
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

            user.Clear();

            Response.Redirect("/App/InfoGrid2/Login/Index2.aspx", false);
        }
    }
}