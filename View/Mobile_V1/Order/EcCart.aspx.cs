using App.BizCommon;
using App.InfoGrid2.Model.Hairdressing;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1.Order
{
    public partial class EcCart : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取购物车里面的产品
        /// </summary>
        /// <returns></returns>
        public string GetShoppingCartObj()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilterCart = new LightModelFilter(typeof(ES_SHOPPING_CART));
            lmFilterCart.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            List<ES_SHOPPING_CART> escList = decipher.SelectModels<ES_SHOPPING_CART>(lmFilterCart);


            SModel sm = new SModel();

            sm["esc_list"] = escList;

            return sm.ToJson();


        }



    }
}