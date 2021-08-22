using App.BizCommon;
using App.InfoGrid2.Model.Mall;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.WeChat;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IG2.BizBase;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace App.InfoGrid2.Mall.Bll
{
    /// <summary>
    /// 业务帮助类
    /// </summary>
    public class BusHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 从字段中获取文件集合数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetFilesByField(string field_value)
        {
            SModelList sm_imgs = new SModelList();

            if (string.IsNullOrWhiteSpace(field_value))
            {

                return sm_imgs;

            }

            string[] imgs = StringUtil.Split(field_value, "\n");

            foreach (string img in imgs)
            {
                if (string.IsNullOrEmpty(img))
                {
                    continue;
                }

                string[] pro = StringUtil.Split(img, "||");
                SModel sm = new SModel();
                sm["url"] = pro[0];
                sm["thumb_url"] = pro[1];
                sm["name"] = pro[2];
                sm["code"] = pro[3];
                sm_imgs.Add(sm);
            }

            return sm_imgs;

        }

        /// <summary>
        /// 获取单张图片路径
        /// </summary>
        /// <param name="field_value"></param>
        /// <returns></returns>
        public static string GetSingImgByField(string field_value)
        {
            if (string.IsNullOrWhiteSpace(field_value))
            {

                return "";
            }


            string[] imgs = StringUtil.Split(field_value, "\n");

            if(imgs.Length == 0)
            {
                return "";
            }


            string[] pro = StringUtil.Split(imgs[0], "||");

            return pro[0];




        }

        /// <summary>
        /// 根据客户获取产品的价格
        /// </summary>
        /// <param name="prod_id">产品ID</param>
        /// <returns></returns>
        public static decimal GetProdPriceByClient(int prod_id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState user = EcContext.Current.User;
            //客户ID
            string user_id = user.ExpandPropertys["CLIENT_ID"];
            SEC_LOGIN_ACCOUNT account = decipher.SelectModelByPk<SEC_LOGIN_ACCOUNT>(user_id);

            BizFilter filter = new BizFilter("UT_252");
            filter.And("COL_1", prod_id);
            filter.And("COL_5", user_id);

            filter.Fields = new string[] { "COL_8"};

            return decipher.ExecuteScalar<decimal>(filter);

        }

        /// <summary>
        /// 微信统一下单函数
        /// </summary>
        /// <param name="order">订单对象</param>
        /// <param name="openid">微信用户唯一标识</param>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public static HttpResult PayOrder(MALL_ORDER order,string openid,string ip)
        {


            NameValueCollection nv = new NameValueCollection();
            log.Debug("dddddddddddddddddddddddddd1");
            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");

            var key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "QLM", "在微信公共API中的关键字");

            var me_domain = GlobelParam.GetValue("ME_DOMAIN", "http://mall.51.yq-ic.com", "自己的域名");

            nv["key"] = key;
            nv["body"] = order.ORDER_INTRO;
            nv["order_no"] = order.ORDER_NO;
            nv["fee"] = (order.MONEY_TOTAL * 100).ToString();                                //这里要变成分单位
            nv["openid"] = openid;
            nv["notify_url"] = $"{me_domain}/Mall/View/Handlers/WxCallback.ashx";
            nv["ip"] = ip;

            string result = string.Empty;
            log.Debug("dddddddddddddddddddddddd2");
            using (WebClient wc = new WebClient())
            {
                byte[] json = wc.UploadValues(url + "/Pay.ashx", "POST", nv);
                result = Encoding.UTF8.GetString(json);

                log.Info("准备调用微信支付返回来的数据：" + result);

            }
            log.Debug("dddddddddddddddddddddddd2");
            HttpResult sm_result = HttpResult.ParseJson(result);
            log.Debug("dddddddddddddddddddddd打出:"+sm_result);
            return sm_result;



        }

        /// <summary>
        /// 根据每个客户来获取到不同的产品
        /// </summary>
        /// <param name="tag">产品标签 用来过滤数据的</param>
        /// <param name="home_visible">是否拿在首页显示的产品  默认 false </param>
        /// <param name="prod_text">产品名称 默认 null 用来查询的</param>
        /// <returns></returns>
        public static SModelList GetOtherProdByClient(string tag, bool home_visible = false,string prod_text = null)
        {
            SModelList sms = new SModelList();

            if (string.IsNullOrWhiteSpace(tag))
            {
                return sms;
            }


            string[] tags = StringUtil.Split(tag, ",");

            if (tags.Length == 0)
            {
                return sms;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_PROD));
            filter.And("HOME_VISIBLE", home_visible);
            filter.And("CAN_SALE", true);
            filter.And("IS_COMMON", false);
            filter.And("PROD_TAG", tags, Logic.In);
            filter.TSqlOrderBy = "ROW_USER_SEQ desc";

            if (!string.IsNullOrWhiteSpace(prod_text))
            {
                filter.And("PROD_TEXT", $"%{prod_text}%", Logic.Like);
            }

            sms = decipher.GetSModelList(filter);

            return sms;


        }


        /// <summary>
        /// 已经登录过的获取客户对象
        /// </summary>
        /// <returns></returns>
        public static LModel GetClient()
        {
            EcUserState user = EcContext.Current.User;

            string client_id = user.ExpandPropertys["CLIENT_ID"];

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm071 = decipher.GetModelByPk("UT_071",client_id);


            return lm071;


        }

        /// <summary>
        /// 同步订单数据给彬哥那边用
        /// </summary>
        /// <param name="order">订单单头对象</param>
        /// <param name="items">订单明细集合对象</param>
        public static void SycnOrder(MALL_ORDER order, List<MALL_ORDER_ITEM> items)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_090");

            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_49", order["MALL_ORDER_ID"]);



            LModel model = decipher.GetModel(filter);





            if (model == null)
            {

                model = new LModel("UT_090");
                decipher.InsertModel(model);

            }
            model.SetTakeChange(true);
            model["ROW_SID"] = order["ROW_SID"];//记录状态
            model["BIZ_SID"] = 0;//业务状态
            log.Debug("tou1111111111111111111111111111111");
            //model["COL_44"] = order["PK_ORDER_CODE"];//订单单头主键编码        
            model["COL_1"] = order["ORDER_NO"];//订单编号
            model["COL_9"] = order["MONEY_TOTAL"];//订单合计总金额 
            model["COL_48"] = order["PAY_TYPE_TEXT"]; //付款类型
            model["COL_13"] = order["GOODS_NUM"];//商品数量
            model["COL_4"] = order["CONSIGNEE"];//收货人
            model["COL_7"] = order["ADDRESS"];//收货地址
            model["COL_2"] = DateTime.Now;//订单时间
            model["COL_5"] = order["CONSIGNEE_TEL"];//收货人电话
            model["COL_10"] = order["REMART"];//备注
            model["COL_47"] = order["PAY_DATE"];//付款时间
            model["COL_42"] = order["EXPRESS_NO"];//快递单号



            model["COL_40"] = "101";//数据来源编号
            model["COL_41"] = "商城订单";//数据来源

            //model["COL_45"] = "101";//微信ID
            model["COL_46"] = order["W_NICKNAME"];//微信昵称

            LightModelFilter filter_account = new LightModelFilter(typeof(WX_ACCOUNT));
            filter_account.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter_account.And("PK_W_CODE", order.FK_W_CODE);

            WX_ACCOUNT account = decipher.SelectToOneModel<WX_ACCOUNT>(filter_account);

            if (account != null)
            {
                model["COL_53"] = account.LOGIN_NAME;
            }
            else
            {
                log.Debug($"创建订单：获取用户对象为空,获取登录名出错了!!!创建订单的用户编码:{order.FK_W_CODE},订单编码:{order.PK_ORDER_CODE}");
            }

            
            model["COL_49"] = order["MALL_ORDER_ID"];
            model["COL_50"] = DateTime.Now;
            model["COL_51"] = "生产中";
            model["COL_52"] = "101";

            model["COL_10"] = "商城订单";
            //  decipher.UpdateModel(model,true);
            DbCascadeRule.Update(model);



            if (items == null)
            { 
                return;

            }


            foreach (MALL_ORDER_ITEM item in items)
            {
                try
                {

                    int prod_id = Convert.ToInt32(item["PROD_ID"]);

                    LightModelFilter filter1 = new LightModelFilter("UT_091");

                    filter1.And("COL_29", prod_id);
                    filter1.And("COL_69", item["SPEC_NO_1"]);
                    filter1.And("COL_12", model["ROW_IDENTITY_ID"]);



                    LModel model1 = decipher.GetModel(filter1);

                    string dd = Convert.ToString(item["SPEC_NO_2"]);


                    if (model1 == null)
                    {

                        model1 = new LModel("UT_091");

                        decipher.InsertModel(model1);

                    }

                    model1.SetTakeChange(true);


                    if (!string.IsNullOrWhiteSpace(dd))
                    {
                        model1[dd] = Convert.ToInt32(item["PROD_NUM"]);
                    }

                    model1["ROW_SID"] = item["ROW_SID"];
                    model1["BIZ_SID"] = 0;
                    //model1["COL_2"] = item["PROD_CODE"];
                    model1["COL_2"] = item["PROD_NO"];
                    model1["COL_3"] = item["PROD_TEXT"];
                    model1["COL_6"] = item["PROD_PRICE"];
                    model1["COL_82"] = item["PROD_PRICE"];
                    model1["COLOUR"] = item["SPEC_TEXT_1"];
                    model1["COL_7"] = item["PROD_NUM"];
                    model1["COL_29"] = prod_id;
                    model1["COL_69"] = item["SPEC_NO_1"];
                    log.Debug("4444444444444444444444444444444");
                    model1["COL_11"] = "商城订单";
                    model1["COL_12"] = model["ROW_IDENTITY_ID"];
                    model1["COL_87"] = DateTime.Now;



                    //model1["COL_85"] = item["PK_ORDER_ITEM_CODE"];
                    //model1["COL_12"] = item["FK_ORDER_CODE"];


                    DbCascadeRule.Update(model1);


                    //decipher.UpdateModelProps(model1, "COL_85", "COL_12",dd, "ROW_SID", "BIZ_SID", "COL_2", "COL_25", "COL_3", "COL_6", "COL_82", "COL_7", "COL_29", "COL_69", "COL_11", "COL_87");           

                }
                catch (Exception ex)
                {

                    log.Debug("错误++++++++++++++++++++++++++++++" + ex.Message);
                    throw ex;
                }

            }
        }

        //删除方法
        public static void delsnOrder(MALL_ORDER order, List<MALL_ORDER_ITEM> items)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
               

                int col_49 = Convert.ToInt32(order["MALL_ORDER_ID"]);

                LightModelFilter filter = new LightModelFilter("UT_090");

                filter.And("COL_49", col_49);

                LModel model = decipher.GetModel(filter);

                model["ROW_SID"] = -3;

                decipher.UpdateModelProps(model, "ROW_SID");
            }
            catch (Exception ex)
            {
                log.Debug("删除失败" + ex.Message);
            }

            List<LModel> lists = new List<LModel>();

            foreach (MALL_ORDER_ITEM mall_order in items)
            {
                int row_indentity_id =Convert.ToInt32(mall_order["ROW_IDENTITY_ID"]);

                LightModelFilter filter1 = new LightModelFilter("UT_091");
                filter1.And("COL_12", row_indentity_id);



                LModel model =  decipher.GetModel(filter1);



                model.SetTakeChange(true);

                model["ROW_SID"] = -3;
                lists.Add(model);             
                

            }
            decipher.UpdateModels(lists, true);

        }


        /// <summary>
        /// 判断是否登录了
        /// </summary>
        public static bool IsLogin()
        {

            EcUserState user = EcContext.Current.User;

            return user.Roles.Exist("WX");

        }



    }
} 