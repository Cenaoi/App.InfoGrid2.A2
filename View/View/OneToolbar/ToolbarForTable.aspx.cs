using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.OneToolbar
{
    public partial class ToolbarForTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int table_id = WebUtil.QueryInt("table_id");

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("TABLE_ID", table_id);

            IG2_TOOLBAR model = decipher.SelectToOneModel<IG2_TOOLBAR>(filter);

            
            SModel result = new SModel();
            result["result"] = "ok";

            SModel data = new SModel();
            data["toolbar_id"] = model.IG2_TOOLBAR_ID;

            result["data"] = data;

            Response.Clear();

            Response.Write(result.ToJson());

            Response.End();

        }
    }
}