using App.BizCommon;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.JLSLBZ.Currency
{   /// <summary>
    /// 这是斌哥要新增的通用指令单界面   2016-08-08
    /// </summary>
    public partial class FormCreate : WidgetControl, IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = new LModel("UT_101");

            model["COL_5"] = "吸塑成品";// WebUtil.Query("ProdType"); //产品类型：吸塑、胶盒
            model["COL_4"] = DateTime.Now;
            model["ROW_SID"] = 0;
            model["BIZ_SID"] = 0;
            model["COL_10"] = "是";
            model["COL_15"] = 1;
            decipher.InsertModel(model);


            Response.Redirect(string.Format("FormXssczl.aspx?id={0}&menuId={1}", model["ROW_IDENTITY_ID"], this.Request.QueryString["menu_id"]));

        }
    }
}