using App.BizCommon;
using App.InfoGrid2.Mall.Bll;
using App.InfoGrid2.Model.Mall;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Collections.Specialized;
using App.InfoGrid2.Model.WeChat;
using System.Net;
using EC5.Utility;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.Mall.Handlers
{
    /// <summary>
    /// 订单处理类
    /// </summary>
    public class Order : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            string action = WebUtil.FormTrim("action");

            HttpResult result = null;

            switch (action)
            {

                case "GET_CHECKED_CARTS":
                    result = GetCheckedCarts();
                    break;
                case "CREATE_ORDER":
                    result = CreateOrder();
                    break;
                case "GET_ORDERS":
                    result = GetOrders();
                    break;
                case "DELETE_ORDER":
                    result = DeleteOrder();
                    break;
                case "PAY_ORDER":
                    result = PayOrder();
                    break;
                case "GET_ORDER_EXPRESS":
                    result = GetOrderExpress();
                    break;
                case "GET_ORDER_BY_FILTER":
                    result = GetOrderByFilter();
                    break;
                default:
                    result = HttpResult.Error("哦噢，写错了");
                    break;

            }


            context.Response.Write(result.ToJson());
        }


        /// <summary>
        /// 获取选中的购物车的商品
        /// </summary>
        /// <returns></returns>
        HttpResult GetCheckedCarts()
        {
            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("FK_W_CODE", user.LoginID);
            filter.And("IS_CHECKED", true);

            SModelList sms = decipher.GetSModelList(filter);

            foreach(var item in sms)
            {

                item["PROD_THUMB"] = BusHelper.GetSingImgByField(item["PROD_THUMB"]);

            }

            return HttpResult.Success(sms);

        }



        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>

        HttpResult CreateOrder()
        {

            log.Debug("进来次数++++++++++++++++++++++++++++++++++");


            //地址
            string address = WebUtil.FormTrim("address");

            //联系人
            string text = WebUtil.FormTrim("text");

            //联系电话
            string tel = WebUtil.FormTrim("tel");
            log.Debug("进来次数1++++++++++++++++++++++++++++++++++");

            if (StringUtil.IsBlank(text))
            {
                return HttpResult.Error("请填写收货人");
            }


            if (StringUtil.IsBlank(tel))
            {
                return HttpResult.Error("请填写手机号");
            }

            if (StringUtil.IsBlank(address))
            {
                return HttpResult.Error("请填写收货地址");
            }
            log.Debug("进来次数2++++++++++++++++++++++++++++++++++");
            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("FK_W_CODE", user.LoginID);
            filter.And("IS_CHECKED", true);
            log.Debug("进来次数3++++++++++++++++++++++++++++++++++");
            List<MALL_S_CART> carts = decipher.SelectModels<MALL_S_CART>(filter);

            StringBuilder sb = new StringBuilder();

            MALL_ORDER order = new MALL_ORDER();
            order.ROW_DATE_CREATE = order.ROW_DATE_UPDATE = DateTime.Now;

            //这是临时用的  正式用的时候会删除掉的
            order.BIZ_SID = 2;
            log.Debug("进来次数4++++++++++++++++++++++++++++++++++");
            order.CONSIGNEE_TEL = tel;
            order.CONSIGNEE = text;
            order.ADDRESS = address;
            order.PAY_TYPE_TEXT = "wechat";
            order.PK_ORDER_CODE = BillIdentityMgr.NewCodeForDay("ORDER_CODE", "OC");
            order.ORDER_NO = BillIdentityMgr.NewCodeForDay("ORDER_NO", "NO");
            order.FK_W_CODE = user.LoginID;
            order.W_NICKNAME = user.LoginName;
            order.ORDER_SID = "101";
            order.ORDER_SID_TEXT = "生产中";

            List<MALL_ORDER_ITEM> orderItems = new List<MALL_ORDER_ITEM>();

            foreach (var item in carts)
            {
                log.Debug("进来次数5++++++++++++++++++++++++++++++++++");
                order.GOODS_NUM += item.PROD_NUM;
                order.MONEY_TOTAL += item.PROD_NUM * item.PROD_PRICE;

                sb.AppendLine($"{item.PROD_TEXT}-{item.SPEC_TEXT_1}-{item.SPEC_TEXT_2}-数量：{item.PROD_NUM}<br />");


                MALL_ORDER_ITEM orderItem = new MALL_ORDER_ITEM();

                item.CopyTo(orderItem, true);

                //这是临时用的  正式用的时候会删除掉的
                orderItem.BIZ_SID = 2;
                log.Debug("进来次数6++++++++++++++++++++++++++++++++++");
                orderItem.FK_ORDER_CODE = order.PK_ORDER_CODE;
                orderItem.FK_W_CODE = user.LoginID;
                orderItem.ROW_DATE_CREATE = orderItem.ROW_DATE_UPDATE = DateTime.Now;
                orderItem.PK_ORDER_ITEM_CODE = BillIdentityMgr.NewCodeForDay("ORDER_ITEM_CODE", "MX");
                log.Debug("进来次数7++++++++++++++++++++++++++++++++++");
                orderItems.Add(orderItem);

            }


            order.ORDER_INTRO = sb.ToString();


            decipher.BeginTransaction();

            try
            {
                log.Debug("进来次数8++++++++++++++++++++++++++++++++++");

                decipher.InsertModel(order);
                decipher.InsertModels(orderItems);

                foreach(var item in carts)
                {
                    item.ROW_SID = -3;
                    item.ROW_DATE_DELETE = DateTime.Now;
                    decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");
                }



                //这是同步数据到彬哥那边的
                BusHelper.SycnOrder(order, orderItems);
                

                decipher.TransactionCommit();

                log.Debug("进来次数9++++++++++++++++++++++++++++++++++");
            }
            catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

                return HttpResult.Error("创建订单时出错");
              

            }

            WX_ACCOUNT account = decipher.SelectModelByPk<WX_ACCOUNT>(user.Identity);

            //收货人电话
            account.COL_20 = tel;
            //收货人姓名
            account.COL_23 = text;
            //收货人地址
            account.COL_27 = address;
          
            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(account, "COL_20", "COL_23", "COL_27", "ROW_DATE_UPDATE");
            log.Debug("进来次数1111++++++++++++++++++++++++++++++++++");
            //return BusHelper.PayOrder(order, account.W_OPENID, HttpContext.Current.Request.UserHostAddress);

            return HttpResult.SuccessMsg("ok");

         

        }

        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetOrders()
        {

            int page_index = WebUtil.FormInt("page_index");

            int page_size = WebUtil.FormInt("page_size");

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();
            user.LoginID = "W1709040001";
            BizFilter filter = new BizFilter(typeof(MALL_ORDER));
            filter.And("FK_W_CODE", user.LoginID);

            filter.Limit = new Limit(page_size, page_index * page_size);

            List<MALL_ORDER> orders = decipher.SelectModels<MALL_ORDER>(filter);

            return HttpResult.Success(orders);


        }


        /// <summary>
        /// 删除订单数据
        /// </summary>
        /// <returns></returns>
        HttpResult DeleteOrder()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int order_id = WebUtil.FormInt("order_id");

            BizFilter filter = new BizFilter(typeof(MALL_ORDER));
            filter.And("MALL_ORDER_ID", order_id);

            decipher.UpdateProps(filter, new object[] {
                "ROW_SID", -3,
                "ROW_DATE_DELETE",DateTime.Now });

            return HttpResult.SuccessMsg("删除订单成功了");

        }

        /// <summary>
        /// 重新付款
        /// </summary>
        /// <returns></returns>
        HttpResult PayOrder()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;

            WX_ACCOUNT account = decipher.SelectModelByPk<WX_ACCOUNT>(user.Identity);

            int order_id = WebUtil.FormInt("order_id");


            MALL_ORDER order = decipher.SelectModelByPk<MALL_ORDER>(order_id);

            order.ORDER_NO = BillIdentityMgr.NewCodeForDay("ORDER_NO", "NO");
            order.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(order, "ORDER_NO", "ROW_DATE_UPDATE");


            return BusHelper.PayOrder(order, account.W_OPENID, HttpContext.Current.Request.UserHostAddress);


        }

        /// <summary>
        /// 获取订单发货数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetOrderExpress()
        {

            int order_id = WebUtil.FormInt("order_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            #region  这是获取未发货信息

            BizFilter filter = new BizFilter("UT_090");
            filter.And("COL_49", order_id);

            LModel lm090 = decipher.GetModel(filter);

            if(lm090 == null)
            {
                return HttpResult.Error("获取不到发货数据");
            }



            BizFilter filter091 = new BizFilter("UT_091");
            filter091.And("COL_12", lm090.GetPk());

            List<LModel> lm091s = decipher.GetModelList(filter091);

            log.Info("091数量："+lm091s.Count);


            SModelList sms = new SModelList();

            foreach(var item in lm091s)
            {

                string prod_code = item.Get<string>("COL_2");

                string color_code = item.Get<string>("COL_69");

                


                CreateSize(sms, prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_0s", "均码", item.Get<decimal>("SZ_0S"));
                CreateSize(sms, prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_1s", "M", item.Get<decimal>("SZ_1S"));
                CreateSize(sms, prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_2s", "L", item.Get<decimal>("SZ_2S"));
                CreateSize(sms,prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_3s", "XL", item.Get<decimal>("SZ_3S"));
                CreateSize(sms,prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_4s", "2XL", item.Get<decimal>("SZ_4S"));
                CreateSize(sms,prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_5s", "3XL", item.Get<decimal>("SZ_5S"));
                CreateSize(sms,prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_6s", "4XL", item.Get<decimal>("SZ_6S"));
                CreateSize(sms,prod_code, item["COL_3"], color_code, item["COLOUR"], "sz_7s", "5XL", item.Get<decimal>("SZ_7S"));

            }

            log.Info("下面是获取091表的数据");
            log.Info(sms.ToJson());

            #endregion


            #region  这是获取发货数据

            List<LModel> lm097s = new List<LModel>();


            foreach(var item in lm091s)
            {

                BizFilter filter097 = new BizFilter("UT_097");
                filter097.And("COL_2", item["COL_19"]);
                filter097.And("COL_21", item["COL_29"]);
                filter097.And("COL_54", item["COL_69"]);

                LModel lm097 = decipher.GetModel(filter097);

                if(lm097 != null)
                {
                    lm097s.Add(lm097);

                }

            }

            Dictionary<int, List<LModel>> key_value = new Dictionary<int, List<LModel>>();

            foreach(var item in lm097s)
            {
                //这是单头ID
                int col_1 = item.Get<int>("COL_1");

                if (key_value.ContainsKey(col_1))
                {
                    key_value[col_1].Add(item);
                }else
                {

                    List<LModel> lms = new List<LModel>();

                    lms.Add(item);

                    key_value.Add(col_1, lms);

                }

            }


            SModelList ut_097s = new SModelList();

            foreach(var item in key_value)
            {

                //拿第一个的主表信息
                LModel lm097 = item.Value[0];

                SModel ut_097  = new SModel();
                ut_097["express_no"] = lm097["COL_61"];
                ut_097["express_text"] = lm097["COL_63"];
                ut_097["express_date"] = lm097.Get<DateTime>("COL_62").ToString("yyyy-MM-dd");

                SModelList items = new SModelList();


                foreach(var sub_item in item.Value)
                {

                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_0c", "均码", sub_item.Get<decimal>("SZ_0C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_1c", "M", sub_item.Get<decimal>("SZ_1C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_2c", "L", sub_item.Get<decimal>("SZ_2C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_3c", "XL", sub_item.Get<decimal>("SZ_3C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_4c", "2XL", sub_item.Get<decimal>("SZ_4C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_5c", "3XL", sub_item.Get<decimal>("SZ_5C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_6c", "4XL", sub_item.Get<decimal>("SZ_6C"));
                    CreateSize(items, sub_item["COL_3"], sub_item["COL_4"], sub_item["COL_54"], sub_item["COLOUR"], "sz_7c", "5XL", sub_item.Get<decimal>("SZ_7C"));

                }


                ut_097["items"] = items;

                ut_097s.Add(ut_097);

            }

            #endregion

            SModel sm = new SModel();

            sm["ut_091s"] = sms;
            sm["ut_097s"] = ut_097s;

            return HttpResult.Success(sm);


        }


        /// <summary>
        /// 创建产品尺寸对象
        /// </summary>
        /// <param name="sms">数据集合</param>
        /// <param name="prod_code">产品编号</param>
        /// <param name="prod_text">产品名称</param>
        /// <param name="color_code">颜色编码</param>
        /// <param name="color_text">颜色名称</param>
        /// <param name="size_code">尺寸编码</param>
        /// <param name="size_text">尺寸名称</param>
        /// <param name="size_num">尺寸数量</param>
        /// <returns></returns>
        void CreateSize(SModelList sms, string prod_code, string prod_text, string color_code, string color_text, string size_code, string size_text, decimal size_num)
        {

            if (size_num <= 0)
            {
                return;
            }



            SModel sm = new SModel();

            sm["prod_code"] = prod_code;
            sm["prod_text"] = prod_text;
            sm["color_code"] = color_code;
            sm["color_text"] = color_text;
            sm["size_code"] = size_code;
            sm["size_text"] = size_text;
            sm["size_num"] = size_num;

            sms.Add(sm);

        }




        /// <summary>
        /// 根据订单状态过滤订单
        /// </summary>
        /// <returns></returns>
        HttpResult GetOrderByFilter()
        {

            string filter_param = WebUtil.Form("filter_param");

            int page_index = WebUtil.FormInt("page_index");

            int page_size = WebUtil.FormInt("page_size");

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_ORDER));
            filter.And("FK_W_CODE", user.LoginID);

            if (filter_param != "all")
            {
                filter.And("ORDER_SID", filter_param);
            }

            filter.Limit = new Limit(page_size, page_index * page_size);

            List<MALL_ORDER> orders = decipher.SelectModels<MALL_ORDER>(filter);

            return HttpResult.Success(orders);

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