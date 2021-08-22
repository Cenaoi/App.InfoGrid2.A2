using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.JF.View.Order
{
    public partial class OrderContent : System.Web.UI.Page
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
        /// 获取订单信息
        /// </summary>
        public string GetOrdersObj()
        {


            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_ORDER));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", w_code);
            lmFilter.And("IS_VISIBLE", true);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";


            List<ES_ORDER> orders = decipher.SelectModels<ES_ORDER>(lmFilter);


            SModelList sm_orders = new SModelList();

            foreach(ES_ORDER order in orders)
            {

                SModel sm = new SModel();

                sm["money_total"] = order.MONEY_TOTAL;
                sm["order_code"] = order.ORDER_CODE;
                sm["biz_sid"] = order.BIZ_SID;
                sm["order_id"] = order.ES_ORDER_ID;
                sm["pay_sid"] = order.PAY_SID;
                sm["del_sid"] = order.DEL_SID;
                sm["rec_sid"] = order.REC_SID;
                sm["order_intro"] = order.ORDER_INTRO;


                sm_orders.Add(sm);


            }


            return sm_orders.ToJson();
            
            


        }



    }
}