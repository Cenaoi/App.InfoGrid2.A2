using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    public partial class DwgFilterItemSetup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> aa = new List<string>()
            {
                "COL_1 = COL_2 //备注",
                "COL_3 = col_33 //备注"
            };
        }
    }
}