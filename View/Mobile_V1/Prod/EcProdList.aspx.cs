using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.Hairdressing;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1.Prod
{
    public partial class EcProdList : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取分类文字
        /// </summary>
        /// <returns></returns>
        public string GetCataTextStr()
        {

            string cata_code = WebUtil.Query("cata_code");

            BIZ_CATALOG cata = BizCatalogMgr.GetBizCata(cata_code);

            if (cata == null)
            {
                return "找不到分类信息";
            }


            return  cata.CATA_TEXT;

        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        public string GetEcpsObj()
        {
            string cata_code = WebUtil.Query("cata_code");


            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilterEcp = new LightModelFilter(typeof(ES_COMMON_PROD));
            lmFilterEcp.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterEcp.And("BIZ_CATA_CODE", cata_code);

            List<ES_COMMON_PROD> ecpList = decipher.SelectModels<ES_COMMON_PROD>(lmFilterEcp);


            SModel vm = new SModel();

            vm["ECP_LIST"] = ecpList;

            return vm.ToJson();



        }

    }
}