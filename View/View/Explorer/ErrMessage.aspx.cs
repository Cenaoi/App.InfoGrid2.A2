using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.Explorer
{
    public partial class ErrMessage : System.Web.UI.Page,EC5.SystemBoard.Interfaces.IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = WebUtil.Query("msg");

            Error404 err;

            if (!string.IsNullOrEmpty(msg))
            {
                err = new Error404();
                err.SetBase64(msg);
            }
            else
            {

                HttpContext context = HttpContext.Current;

                err = context.Items["ERROR_404"] as Error404;

                context.Items.Remove("ERROR_404");
            }

            if (err != null)
            {
                this.titleDIV.InnerHtml = err.Title;
                this.contentDIV.InnerHtml = err.Content;

            }
        }
    }
}