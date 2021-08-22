using App.BizCommon;
using App.InfoGrid2.Model.Hairdressing;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1.Prod
{
    public partial class EcProd : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 获取商品名称
        /// </summary>
        /// <returns></returns>
        public string GetProdNameStr()
        {


            int prod_id = WebUtil.QueryInt("prod_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD ecp = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            if (ecp == null)
            {

                log.Error("找不到产品信息");

                return "哦噢，找不到产品信息";

            }

            return  ecp.PROD_NAME;


        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <returns></returns>
        public string GetProdObj()
        {




            int prod_id = WebUtil.QueryInt("prod_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD ecp = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            if (ecp == null)
            {

                log.Error("找不到产品信息");

            }


            SModel vm = new SModel();

            vm["ECP"] = ecp;

            return vm.ToJson();



        }





    }
}