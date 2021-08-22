using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
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

namespace App.InfoGrid2.JF.View.Prod
{
    public partial class ProdSpec : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!BusHelper.AutoLogin())
                {

                    Response.Redirect("/JF/WeChat/Index.ashx");


                }

            }

        }

        /// <summary>
        /// 获取规格信息
        /// </summary>
        /// <returns></returns>
        public string GetSpecsObj()
        {

            int prod_id = WebUtil.QueryInt("prod_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD prod = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            if (prod == null)
            {
                return "[]";
            }

            SModelList sm_specs = new SModelList();


            string group_code = prod.GROUP_CODE;

            //如果
            if (string.IsNullOrWhiteSpace(group_code))
            {
                sm_specs.Add(CreateSpec(prod));
            }
            else
            {
                LightModelFilter lmFilter = new LightModelFilter(typeof(ES_COMMON_PROD));
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("GROUP_CODE", group_code);
                lmFilter.And("CAN_SALE", true);


                List<ES_COMMON_PROD> prods = decipher.SelectModels<ES_COMMON_PROD>(lmFilter);
                //如果组编码没有找到数据，就用自身数据
                if(prods.Count == 0)
                {
                    sm_specs.Add(CreateSpec(prod));
                }

                foreach (ES_COMMON_PROD item in prods)
                {

                    SModel sm = new SModel();

                    sm["id"] = item.ES_COMMON_PROD_ID;
                    sm["spec_text"] = item.SPEC_TEXT;
                    sm["price"] = item.PRICE;

                    sm_specs.Add(sm);

                }
            }

          

            return sm_specs.ToJson();


        }

        /// <summary>
        /// 创建规格对象
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        SModel CreateSpec(ES_COMMON_PROD prod)
        {

            SModel sm = new SModel();

            sm["id"] = prod.ES_COMMON_PROD_ID;
            sm["spec_text"] = prod.SPEC_TEXT;
            sm["price"] = prod.PRICE;

            return sm;

        }


    }
}