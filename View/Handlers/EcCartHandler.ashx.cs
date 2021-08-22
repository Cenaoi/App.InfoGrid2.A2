using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.Hairdressing;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// EcCartHandler 的摘要说明
    /// </summary>
    public class EcCartHandler : IHttpHandler, IRequiresSessionState
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {



            context.Response.ContentType = "text/plain";

            string action = WebUtil.FormTrimUpper("action");

            try
            {

                switch (action)
                {

                    case "CREATE_ORDER":
                        CreateOrder();
                        break;
                    case "DELETE_CARTS":
                        DeleteCarts();
                        break;
                    case "PUSH_IN_CART":
                        PushInCart();
                        break;

                }
            }catch(Exception ex)
            {
                log.Error(ex);
                ResponseHelper.Result_error("哦噢，出错了！");
            }

        }

        /// <summary>
        /// 根据传过来的商品ID建立订单
        /// </summary>
        void CreateOrder()
        {
            string ids = WebUtil.Form("cart_ids");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilterCart = new LightModelFilter(typeof(ES_SHOPPING_CART));
            lmFilterCart.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterCart.And("ES_SHOPPING_CART_ID", ids, Logic.In);

            List<ES_SHOPPING_CART> cartList = decipher.SelectModels<ES_SHOPPING_CART>(lmFilterCart);

            if (cartList.Count == 0)
            {
                ResponseHelper.Result_error("没有商品，不能下订单！");
                return;
            }


            EcUserState user = EcContext.Current.User;


            //订单金额
            decimal moneyTotal = 0;

            cartList.ForEach(c => moneyTotal += c.SUB_TOTAL_MONEY);

            ES_ORDER esOrder = new ES_ORDER()
            {
                ADDRESS_T1 = "广东省广州市",
                ADDRESS_T2 = "天河区软件园",
                CURRENCY_ID = "人名币",
                OP_TIME = DateTime.Now,
                USER_NAME = "小渔夫",
                USER_ID = 123456,
                USER_PHONE = "123456789",
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                USER_EMAIL = "13530383952@163.com",
                ORDER_NUM = BillIdentityMgr.NewCodeForDay("ES", "", 6),
                BIZ_SID = 0,
                ORDER_NAME = "订单名称",
                MONEY_TOTAL = moneyTotal
            };


            decipher.InsertModel(esOrder);

            List<ES_ORDER_ITEM> esOrderItemList = new List<ES_ORDER_ITEM>();


            foreach (ES_SHOPPING_CART cart in cartList)
            {
                ES_ORDER_ITEM item = new ES_ORDER_ITEM();

                cart.CopyTo(item, true);
                item.ROW_DATE_UPDATE = DateTime.Now;
                item.ORDER_ID = esOrder.ES_ORDER_ID;

                esOrderItemList.Add(item);
            }

            try
            {

                decipher.InsertModels(esOrderItemList);


                foreach(var cart in cartList)
                {

                    cart.ROW_SID = -3;

                    cart.ROW_DATE_DELETE = DateTime.Now;

                    decipher.UpdateModelProps(cart, "ROW_SID", "ROW_DATE_DELETE");


                }


            }
            catch (Exception ex)
            {
                log.Error("创建订单失败了！", ex);
                ResponseHelper.Result_error("哦噢，创建订单失败了！");
                return;
            }





            SModel vm = new SModel();
            vm["order_id"] = esOrder.ES_ORDER_ID;

            ResponseHelper.Result_ok(vm, "创建订单成功了!");

        }

        /// <summary>
        /// 删除购物车里面的产品
        /// </summary>
        void DeleteCarts()
        {

            string ids = WebUtil.Form("cart_ids");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_SHOPPING_CART));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ES_SHOPPING_CART_ID", ids,Logic.In);

            decipher.UpdateProps(lmFilter, new object[] {
                "ROW_SID", -3,
                "ROW_DATE_UPDATE",DateTime.Now,
                "ROW_DATE_DELETE",DateTime.Now});


            LightModelFilter lmFilterCart = new LightModelFilter(typeof(ES_SHOPPING_CART));
            lmFilterCart.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            List<ES_SHOPPING_CART> escList = decipher.SelectModels<ES_SHOPPING_CART>(lmFilterCart);

            SModel sm = new SModel();

            sm["esc_list"] = escList;

            ResponseHelper.Result_ok(sm);

        }


        /// <summary>
        /// 把产品数据放进购物车里面
        /// </summary>
        void PushInCart()
        {

            string data = WebUtil.Form("data");

            SModelList sms = SModelList.ParseJson(data);


            DbDecipher decipher = ModelAction.OpenDecipher();

            foreach(SModel sm in sms)
            {

                decimal num = Convert.ToDecimal(sm["spec_num"]);

                ES_PRODUCT_DATA prod_spec = decipher.SelectModelByPk<ES_PRODUCT_DATA>(sm["spec_id"]);

                ES_COMMON_PROD prod = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_spec.PROD_ID);


                ES_SHOPPING_CART cart = new ES_SHOPPING_CART();

                cart.PROD_ID = prod.ES_COMMON_PROD_ID;
                cart.PROD_NAME = prod.PROD_NAME;
                cart.PROD_CODE = prod.PROD_CODE;
                cart.PROD_DATA_ID = prod_spec.ES_PRODUCT_DATA_ID;
                cart.PROD_DATA_NAME = prod_spec.SPEC_NAME;
                cart.PROD_NUM = (int) num;
                cart.PRICE = prod.PRICE;
                cart.PRICE_AGENT = prod.PRICE_AGENT;
                cart.PRICE_MARKET = prod.PRICE_MARKET;
                cart.PRICE = prod.PRICE;
                cart.SUB_TOTAL_MONEY = num * prod.PRICE;
                cart.PROD_INTRO = prod.PROD_INTRO;
                cart.ROW_DATE_CREATE = cart.ROW_DATE_UPDATE = DateTime.Now;

                decipher.InsertModel(cart);
            }


            ResponseHelper.Result_ok("添加进购物车了！");


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