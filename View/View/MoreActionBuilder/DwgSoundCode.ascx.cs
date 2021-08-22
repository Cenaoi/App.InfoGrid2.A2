using App.BizCommon;
using App.InfoGrid2.Model.AC3;
using EC5.Entity.Expanding.ExpandV1;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    public partial class DwgSoundCode : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                string xml = GetXmlSoundCode();

            }
        }


        /// <summary>
        /// 获取 XML 的源码
        /// </summary>
        /// <returns></returns>
        private string GetXmlSoundCode()
        {
            string dwg_code = WebUtil.FormTrim("dwg_code");

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(AC3_DWG));
            filter.And("PK_DWG_CODE", dwg_code);

            AC3_DWG model = decipher.SelectToOneModel<AC3_DWG>(filter);





            return string.Empty;
        }

    }
}