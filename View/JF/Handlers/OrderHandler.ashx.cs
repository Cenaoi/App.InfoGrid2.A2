using App.BizCommon;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
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

namespace App.InfoGrid2.JF.Handlers
{
    /// <summary>
    /// OrderHandler 的摘要说明
    /// </summary>
    public class OrderHandler : IHttpHandler, IRequiresSessionState
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
                    case "DELETE_ORDER":
                        DeleteOrder();
                        break;
                    case "PAY_ORDER":
                        PayOrder();
                        break;

                    default:
                        ResponseHelper.Result_error("写错了吧！");
                        break;

                }


            }
            catch (Exception ex)
            {

                log.Debug(ex);

                ResponseHelper.Result_error("哦噢，出错了喔！");
            }



        }

        /// <summary>
        /// 创建订单了
        /// </summary>
        void CreateOrder()
        {

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            string text = WebUtil.Form("text");

            string tel = WebUtil.Form("tel");

            string address = WebUtil.Form("address");


            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_S_CAR));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", w_code);
            lmFilter.And("IS_CHECKED", true);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<ES_S_CAR> cars = decipher.SelectModels<ES_S_CAR>(lmFilter);

            if(cars.Count == 0)
            {
                ResponseHelper.Result_error("请选中一些购物车的商品！");
                return;
            }


            List<ES_ORDER_DETA> order_detas = new List<ES_ORDER_DETA>();

            //订单总金额
            decimal total = 0;

            //商品数量
            int good_num = 0;


            string order_intro = string.Empty;


            foreach (var item in cars)
            {
                LightModelFilter lmFilterProd = new LightModelFilter(typeof(ES_COMMON_PROD));
                lmFilterProd.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilterProd.And("PK_P_CODE", item.FK_P_CODE);
                //获取商品信息
                ES_COMMON_PROD prod = decipher.SelectToOneModel<ES_COMMON_PROD>(lmFilterProd);

                ES_ORDER_DETA order_deta = new ES_ORDER_DETA();

                prod.CopyTo(order_deta, true);

                order_deta.ROW_DATE_CREATE = order_deta.ROW_DATE_UPDATE = DateTime.Now;
                order_deta.ROW_SID = 0;
                order_deta.FK_P_CODE = item.FK_P_CODE;
                order_deta.SALE_NUM = item.PROD_NUM;
                good_num += item.PROD_NUM;
                order_deta.SUB_TOTAL = prod.PRICE * item.PROD_NUM;
                total += order_deta.SUB_TOTAL;
                order_detas.Add(order_deta);
                

                order_intro +="("+ prod.PROD_NAME +"-"+ prod.SPEC_TEXT +")";

            }


            ES_ORDER order = new ES_ORDER();

            order.ADDRESS_T2 = address;
            order.USER_PHONE = tel;
            order.CONTACTER_NAME = text;
            order.FK_W_CODE = w_code;
            order.PK_O_CODE = BillIdentityMgr.NewCodeForDay("ORDER_CODE", "O", 5);
            order.ORDER_CODE = BillIdentityMgr.NewCodeForDay("ORDER_BIZ_CODE", "",5);
            order.PAYMENT_TYPE_TEXT = "微信";
            order.CURRENCY_ID = "RMB";
            order.MONEY_TOTAL = total;
            //产品金额合计
            order.MONEY_GOODS = total;
            order.ORDER_INTRO = order_intro;
            order.ROW_DATE_CREATE = order.ROW_DATE_UPDATE = DateTime.Now;
            order.W_OPENID = user.LoginID;
            //商品数量
            order.GOOD_NUM = good_num;
            //默认显示
            order.IS_VISIBLE = true;
            //微信支付订单流水号
            order.W_P_SERIAL_CODE = BillIdentityMgr.NewCodeForDay("W_P_SERIAL_CODE", "", 10);



            decipher.BeginTransaction();

            try
            {

                //插入订单头
                decipher.InsertModel(order);

                //插入订单明细
                foreach (var order_deta in order_detas)
                {
                    order_deta.PK_OD_CODE = BillIdentityMgr.NewCodeForDay("ORDER_DETA_CODE", "D", 5);
                    order_deta.FK_O_CODE = order.PK_O_CODE;

                    decipher.InsertModel(order_deta);

                }

                //删除购物车里面的一些商品
                cars.ForEach(c => decipher.DeleteModel(c));


                #region   这里要写调用微信支付功能

                string ip = HttpContext.Current.Request.UserHostAddress;

                //拿简单数据
                string domain_name = GlobelParam.GetValue<string>("DOMAIN_NAME", "wshop.gzbeih.com", "域名加端口");

                SModel sm_result = WeChatUtil.Receivables(cars[0].PROD_TEXT, order.W_P_SERIAL_CODE, total * 100, ip, $"http://{domain_name}/JF/Handlers/WxCallbackHandler.ashx", user.LoginID);

                #endregion


                order.W_UNIFIED_ORDER_JSON = sm_result.ToJson();

                order.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModelProps(order, "W_UNIFIED_ORDER_JSON", "ROW_DATE_UPDATE");



                //事务提交
                decipher.TransactionCommit();


                ResponseHelper.Result_ok(sm_result);



            }catch(Exception ex)
            {
                decipher.TransactionRollback();
                log.Error(ex);




            }

        }

        /// <summary>
        /// 删除订单
        /// </summary>
        void DeleteOrder()
        {

            int order_id = WebUtil.FormInt("order_id");



            DbDecipher decipher = ModelAction.OpenDecipher();


            ES_ORDER order = decipher.SelectModelByPk<ES_ORDER>(order_id);

            if(order.BIZ_SID != 999)
            {
                ResponseHelper.Result_error("未完成订单不能删除！");
                return;
            }



            order.IS_VISIBLE = false;

            order.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(order, "IS_VISIBLE", "ROW_DATE_UPDATE");

            ResponseHelper.Result_ok("删除订单成功了！");


        }

        /// <summary>
        /// 重新付款
        /// </summary>
        void PayOrder()
        {

            EcUserState user = EcContext.Current.User;

            int order_id = WebUtil.FormInt("order_id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            ES_ORDER order = decipher.SelectModelByPk<ES_ORDER>(order_id);

            if(order.PAY_SID == 999)
            {

                ResponseHelper.Result_error("已经付款过了！");

                return;

            }


            if (string.IsNullOrWhiteSpace(order.W_UNIFIED_ORDER_JSON))
            {
                ResponseHelper.Result_error("哦噢，出错了！");
                return;
            }


            //微信支付订单流水号
            order.W_P_SERIAL_CODE = BillIdentityMgr.NewCodeForDay("W_P_SERIAL_CODE", "", 10);

            #region   这里要写调用微信支付功能

            string ip = HttpContext.Current.Request.UserHostAddress;

            //拿简单数据
            string domain_name = GlobelParam.GetValue<string>("DOMAIN_NAME", "wshop.gzbeih.com", "域名加端口");

            SModel sm_result = WeChatUtil.Receivables(order.ORDER_INTRO, order.W_P_SERIAL_CODE, order.MONEY_TOTAL * 100, ip, $"http://{domain_name}/JF/Handlers/WxCallbackHandler.ashx", user.LoginID);

            #endregion

            order.W_UNIFIED_ORDER_JSON = sm_result.ToJson();

            order.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(order, "W_P_SERIAL_CODE", "W_UNIFIED_ORDER_JSON", "ROW_DATE_UPDATE");


            ResponseHelper.Result_ok(sm_result);


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