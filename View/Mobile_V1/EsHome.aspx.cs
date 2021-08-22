using App.BizCommon;
using App.InfoGrid2.Model.Hairdressing;
using App.InfoGrid2.View;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1
{
    public partial class EsHome : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 获取首页的数据
        /// </summary>
        /// <returns></returns>
        public string GetRedirectLink()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilterLink = new LightModelFilter(typeof(CMS_U_REDIRECT_LINK));
            lmFilterLink.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterLink.And("BIZ_CATA_CODE", "0401");

            LightModelFilter lmFilterProdCata = new LightModelFilter(typeof(CMS_U_REDIRECT_LINK));
            lmFilterProdCata.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterProdCata.And("BIZ_CATA_CODE", "0402");


            LightModelFilter lmFilterAdv = new LightModelFilter(typeof(CMS_U_REDIRECT_LINK));
            lmFilterAdv.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterAdv.And("BIZ_CATA_CODE", "0403");

            LightModelFilter lmFilterProd = new LightModelFilter(typeof(CMS_U_REDIRECT_LINK));
            lmFilterProd.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterProd.And("BIZ_CATA_CODE", "0404");

            //幻灯片数据
            List<LModel> lmLinkList = null;

            //商品分类
            List<LModel> lmProdCataList = null;

            //推广广告数据
            List<LModel> lmAdvList = null;

            //商品信息
            List<LModel> lmProdList = null;

            try
            {

                lmLinkList = decipher.GetModelList(lmFilterLink);

                lmProdCataList = decipher.GetModelList(lmFilterProdCata);
                lmAdvList = decipher.GetModelList(lmFilterAdv);

                lmProdList = decipher.GetModelList(lmFilterProd);



            }
            catch (Exception ex)
            {
                log.Error("获取数据报错了", ex);

                Error404.Send("出错了！");

                

            }

            SModel vm = new SModel();

            vm["TOP_LINK_LIST"] = lmLinkList;
            vm["PROD_CATA_LIST"] = lmProdCataList;
            vm["ADV_LIST"] = lmAdvList;
            vm["PROD_LIST"] = lmProdList;

            return vm.ToJson();


        }

    }
}