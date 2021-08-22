using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.JF.View
{
    public partial class Home1 : System.Web.UI.Page
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

        public string GetProdsObj()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            //这里的过滤还不够，正式的话要有目录编码过来
            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_COMMON_PROD));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("CAN_SALE", true);
            lmFilter.And("IS_VISIBLE", true);
            lmFilter.Top = 20;
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<ES_COMMON_PROD> prods = decipher.SelectModels<ES_COMMON_PROD>(lmFilter);


            SModelList sm_prods = new SModelList();

            foreach (ES_COMMON_PROD prod in prods)
            {


                SModel sm = new SModel();


                sm["id"] = prod.ES_COMMON_PROD_ID;
                sm["prod_name"] = prod.PROD_NAME;
                sm["prod_intro"] = prod.PROD_INTRO;
                sm["price"] = prod.PRICE;
                sm["prod_thumb"] = prod.PROD_THUMB;

                sm_prods.Add(sm);

            }


            return sm_prods.ToJson();


        }

    }
}