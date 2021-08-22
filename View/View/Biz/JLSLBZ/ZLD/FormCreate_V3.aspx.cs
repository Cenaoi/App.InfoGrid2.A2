using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.JLSLBZ.ZLD
{
    public partial class FormCreate_V3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = new LModel("UT_101");

            model["COL_5"] = "印刷成品";// WebUtil.Query("ProdType"); //产品类型：吸塑、胶盒
            model["COL_4"] = DateTime.Now;
            model["ROW_SID"] = 0;
            model["BIZ_SID"] = 0;
            model["COL_10"] = "是";
            decipher.InsertModel(model);

            Response.Redirect(string.Format("FormXssczl_V3.aspx?id={0}&menuId={1}", model["ROW_IDENTITY_ID"], this.Request.QueryString["menu_id"]));
        }
    }
}