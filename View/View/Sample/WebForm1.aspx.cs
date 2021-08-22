using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Sample
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string file = this.Request.Files.Count.ToString();

            HttpPostedFile ff = this.Request.Files[0];

            ff.SaveAs("C:\\SSS\\" + ff.FileName);

        }


    }
}