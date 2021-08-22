using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.LightModels;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx2
{
    public partial class EditBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_101", id);

            string billType = (string)model["COL_5"];

            if ("胶盒成品" == billType)
            {
                this.Response.Redirect("/App/InfoGrid2/View/Biz/JLSLBZ/JHSCZL/FormJhsczl.aspx?id=" + id);
            }
            else if ("吸塑成品" == billType)
            {
                this.Response.Redirect("/App/InfoGrid2/View/Biz/JLSLBZ/XSSCZL/FormXssczl.aspx?id=" + id);
            }
        }
    }
}