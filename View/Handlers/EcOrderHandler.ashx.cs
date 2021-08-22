using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.Hairdressing;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// EcOrderHandler 的摘要说明
    /// </summary>
    public class EcOrderHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        HttpRequest m_request;

        HttpResponse m_response;

        public void ProcessRequest(HttpContext context)
        {
            m_response = context.Response;

            m_request = context.Request;


            m_response.ContentType = "text/plain";


            string action = WebUtil.FormTrimUpper("action");

            switch (action)
            {


                case "ORDER_OK":
                    OrderOk();
                    break;

            }




            
        }


        /// <summary>
        /// 订单确认
        /// </summary>
        void OrderOk()
        {
            string cart_ids = WebUtil.Form("cart_ids");

            int order_id = WebUtil.FormInt("order_id");

            string address_t2 = WebUtil.Form("address_t2");

            string contacter_name = WebUtil.Form("contacter_name");


            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_ORDER esOrder = decipher.SelectModelByPk<ES_ORDER>(order_id);

            if (esOrder == null)
            {
                ResponseHelper.Result_error("提交的数据有问题了！");
                return;
            }

            esOrder.OP_TIME = DateTime.Now;
            esOrder.ADDRESS_T2 = address_t2;
            esOrder.CONTACTER_NAME = contacter_name;

            decipher.UpdateModelProps(esOrder, "OP_TIME", "ADDRESS_T2", "CONTACTER_NAME");

            LightModelFilter lmFilterCart = new LightModelFilter(typeof(ES_SHOPPING_CART));
            lmFilterCart.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterCart.And("ES_SHOPPING_CART_ID", cart_ids, Logic.In);

            decipher.DeleteModels(lmFilterCart);

            ResponseHelper.Result_ok("确认订单成功了！");

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}