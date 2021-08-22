using App.BizCommon;
using App.InfoGrid2.Mall.Bll;
using App.InfoGrid2.Model.Mall;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility.Web;
using EC5.Web.WebAPI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Mall.Handlers
{
    /// <summary>
    /// Prod 的摘要说明
    /// </summary>
    public class Prod : AjaxHandler, IHttpHandler, IRequiresSessionState
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public override void ProcessRequest(HttpContext context)
        {

            // 这是同意跨域获取数据
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:8080");
            context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            string action = WebUtil.FormTrim("action");

            HttpResult result = null;

            switch (action)
            {

                case "GET_PRODS_BY_HOME":
                    result = GetProdsByHome();
                    break;
                case "GET_PROD_BY_CODE":
                    result = GetProdByCode();
                    break;
                case "GET_PROD_SPECS":
                    result = GetProdSpecs();
                    break;
                case "PUSH_CAR_BTN":
                    result = PushCarBtn();
                    break;
                case "GET_CARTS":
                    result = GetCarts();
                    break;
                case "CHECKED_CART_ITEM":
                    result = CheckedCartItem();
                    break;
                case "CHECKED_CART_ALL":
                    result = CheckedCartAll();
                    break;
                case "DELETE_CART_ITEM":
                    result = DeleteCartItem();
                    break;
                case "GET_CART_NUM":
                    result = GetCartNum();
                    break;
                case "GET_PRODS_BY_PAGE":
                    result = GetProdsByPage();
                    break;
                default:
                    result = HttpResult.Error("哦噢，写错了");
                    break;

            }


            context.Response.Write(result.ToJson());


        }

        /// <summary>
        /// 把商品放进购物车里面
        /// </summary>
        /// <returns></returns>
        [Ajax("PUSH_CAR_BTN")]
        HttpResult PushCarBtn()
        {

            string prod_code = WebUtil.FormTrim("prod_code");

            string spec_json = WebUtil.FormTrim("spec_json");

            //把传上来的规格数据解析成一个集合对象
            SModelList specs = SModelList.ParseJson(spec_json);

            if(specs.Count == 0)
            {
                return HttpResult.Error("请选择规格数量");
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_PROD));
            filter.And("PK_PROD_CODE", prod_code);
            //获取商品对象
            MALL_PROD mp = decipher.SelectToOneModel<MALL_PROD>(filter);

            foreach (var item in specs)
            {

                BizFilter filterCart = new BizFilter(typeof(MALL_S_CART));
                filterCart.And("FK_PROD_CODE", prod_code);
                filterCart.And("SPEC_CODE_1", item["spec_code_1"]);
                filterCart.And("SPEC_CODE_2", item["spec_code_2"]);

                MALL_S_CART cart = decipher.SelectToOneModel<MALL_S_CART>(filterCart);

                if (cart != null)
                {
                    cart.PROD_NUM += item["spec_num"];
                    cart.ROW_DATE_UPDATE = DateTime.Now;

                    decipher.UpdateModelProps(cart, "PROD_NUM", "ROW_DATE_UPDATE");

                    continue;
                }

                EcUserState user = EcContext.Current.User;

                cart = new MALL_S_CART();
                cart.PK_S_CART_CODE = BillIdentityMgr.NewCodeForDay("S_CART_CODE", "C");
                cart.PROD_INTRO = item["spec_text_1"] +"     "+ item["spec_text_2"] ;
                cart.PROD_NO = mp.PROD_NO;
                cart.PROD_NUM = item.Get<int>("spec_num");
                cart.PROD_PRICE = mp.PRICE_MEMBER;
                cart.PROD_TEXT = mp.PROD_TEXT;
                cart.PROD_THUMB = mp.PROD_THUMB;
                cart.SPEC_CODE_1 = item["spec_code_1"];
                cart.SPEC_NO_1 = item["spec_no_1"];
                cart.SPEC_TEXT_1 = item["spec_text_1"];
                cart.SPEC_CODE_2 = item["spec_code_2"];
                cart.SPEC_NO_2 = item["spec_no_2"];
                cart.SPEC_TEXT_2 = item["spec_text_2"];
                cart.FK_W_CODE = user.LoginID;
                cart.PROD_ID = mp.PROD_ID;
                cart.ROW_DATE_UPDATE = cart.ROW_DATE_CREATE = DateTime.Now;

                decipher.InsertModel(cart);


            }

            return HttpResult.SuccessMsg("添加进购物车成功");

        }

        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_CARTS")]
        HttpResult GetCarts()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("FK_W_CODE", user.LoginID);

            SModelList  carts = decipher.GetSModelList(filter);

            foreach(var item in carts)
            {

                item["PROD_THUMB"] = BusHelper.GetSingImgByField(item["PROD_THUMB"]);

            }

            return HttpResult.Success(carts);
            

        }

        /// <summary>
        /// 获取可以显示在首页
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_PRODS_BY_HOME")]
        HttpResult GetProdsByHome()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter("MALL_PROD");
           
            filter.And("HOME_VISIBLE", true);
            filter.And("CAN_SALE", true);
            filter.And("IS_COMMON", true);
            filter.TSqlOrderBy = "ROW_USER_SEQ desc";

            //获取可以显示在首页的商品
            SModelList lms = decipher.GetSModelList(filter);

            //获取客户对象
            LModel lm071 = BusHelper.GetClient();

            if(lm071 == null)
            {
                return HttpResult.Error("获取客户数据出错");
            }

            //这是获取每个单独客户产品
            SModelList otherLms = BusHelper.GetOtherProdByClient(lm071.Get<string>("PROD_TAG"),true);

            lms.InsertRange(0, otherLms);

            foreach(var item in lms)
            {

                item["PROD_THUMB"] = BusHelper.GetSingImgByField(item["PROD_THUMB"]);

            }


            //这是排序
            lms.Sort((m1, m2) => {


                if (m1.Get<decimal>("ROW_USER_SEQ") == m2.Get<decimal>("ROW_USER_SEQ"))
                {
                    return 0;
                }


                if (m1.Get<decimal>("ROW_USER_SEQ") > m2.Get<decimal>("ROW_USER_SEQ"))
                {

                    return 1;

                }

                return -1;

            });


            return HttpResult.Success(lms);

        }

        /// <summary>
        /// 根据商品编码获取商品信息
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_PROD_BY_CODE")]
        HttpResult GetProdByCode()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string code = WebUtil.FormTrim("code");


            BizFilter filter = new BizFilter("MALL_PROD");
            filter.And("CAN_SALE", true);
            filter.And("PK_PROD_CODE", code);

            SModel sm = decipher.GetSModel(filter);

            sm["PRICE_MEMBER"] = BusHelper.GetProdPriceByClient(sm.Get<int>("PROD_ID"));

            sm["imgs"] = BusHelper.GetFilesByField(sm["MULTIPLE_PHOTO_LIST"]);


            return HttpResult.Success(sm);


        }

        /// <summary>
        /// 获取产品规格属性
        /// </summary>
        /// <returns></returns>
        [Ajax("GET_PROD_SPECS")]
        HttpResult GetProdSpecs()
        {

            string code = WebUtil.FormTrim("code");


            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filterProd = new BizFilter("MALL_PROD");
            filterProd.And("CAN_SALE", true);
            filterProd.And("PK_PROD_CODE", code);

            SModel sm = decipher.GetSModel(filterProd);

            sm["PRICE_MEMBER"] = BusHelper.GetProdPriceByClient(sm.Get<int>("PROD_ID"));

            BizFilter filter = new BizFilter("MALL_PROD_SPEC");
            filter.And("FK_PROD_CODE", code);

            SModelList sms = decipher.GetSModelList(filter);

            SModelList specs1 = new SModelList();

            SModelList specs2 = new SModelList();


            foreach(var item in sms)
            {

                if (string.IsNullOrWhiteSpace(item.Get<string>("SPEC_TEXT")))
                {
                    continue;
                }

                string spec_type = item.Get<string>("SPEC_TYPE");

                if(spec_type == "colour")
                {
                    specs1.Add(item);
                }else
                {
                    specs2.Add(item);
                }

                item["spec_num"] = 0;


            }



            sm["PROD_THUMB"] = BusHelper.GetSingImgByField(sm["PROD_THUMB"]);

            SModel smResult = new SModel();
            smResult["prod_obj"] = sm;
            smResult["specs1"] = specs1;
            smResult["specs2"] = specs2;

            return HttpResult.Success(smResult);

        }

        /// <summary>
        /// 选中购物车一个商品事件
        /// </summary>
        /// <returns></returns>
        [Ajax("CHECKED_CART_ITEM")]
        HttpResult CheckedCartItem()
        {
            //购物车对象ID
            int cart_id = WebUtil.FormInt("cart_id");

            //是否选中
            bool is_checked = WebUtil.FormBool("is_checked");

            DbDecipher decipher = ModelAction.OpenDecipher();


            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("MALL_S_CART_ID", cart_id);

            decipher.UpdateProps(filter, new object[] {
                "IS_CHECKED", is_checked,
                "ROW_DATE_UPDATE",DateTime.Now });



            return GetCarts();


        }

        /// <summary>
        /// 全选购物车事件
        /// </summary>
        /// <returns></returns>
        [Ajax("CHECKED_CART_ALL")]
        HttpResult CheckedCartAll()
        {
            //是否选中
            bool is_checked = WebUtil.FormBool("is_checked");

            DbDecipher decipher = ModelAction.OpenDecipher();


            BizFilter filter = new BizFilter(typeof(MALL_S_CART));

            decipher.UpdateProps(filter, new object[] {
                "IS_CHECKED", is_checked,
                "ROW_DATE_UPDATE",DateTime.Now });


            return GetCarts();


        }


        /// <summary>
        /// 删除购物车里面打钩的商品
        /// </summary>
        /// <returns></returns>
        [Ajax("DELETE_CART_ITEM")]
        HttpResult DeleteCartItem()
        {
            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("IS_CHECKED", true);
            filter.And("FK_W_CODE", user.LoginID);
            

            decipher.UpdateProps(filter, new object[] {
                "ROW_SID",-3,
                "ROW_DATE_UPDATE",DateTime.Now,
                "ROW_DATE_DELETE",DateTime.Now});


            return GetCarts();


        }


        /// <summary>
        /// 分页获取商品数据
        /// </summary>
        /// <returns></returns>
        HttpResult GetProdsByPage()
        {

            //页索引
            int page_index = WebUtil.FormInt("page_index");
            //页大小
            int page_size = WebUtil.FormInt("page_size");

            //搜索
            string s_prod_text = WebUtil.FormTrim("s_prod_text");

            //分类编码
            string cata_code = WebUtil.FormTrim("cata_code");

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_PROD));
            filter.Limit = new Limit(page_size, page_index * page_size);

            if (!string.IsNullOrWhiteSpace(s_prod_text))
            {
                filter.And("PROD_TEXT", $"%{s_prod_text}%", Logic.Like);
            }

            if (!string.IsNullOrWhiteSpace(cata_code))
            {
                filter.And("PROD_TYPE_CODE", cata_code);
            }
            filter.TSqlOrderBy = "ROW_USER_SEQ desc";

            SModelList sms = decipher.GetSModelList(filter);

            //获取客户对象
            LModel lm071 = BusHelper.GetClient();

            if (lm071 == null)
            {
                return HttpResult.Error("获取客户数据出错");
            }


            //这是属于自己的产品
            SModelList otherSms =  BusHelper.GetOtherProdByClient(lm071.Get<string>("PROD_TAG"), false, s_prod_text);

            sms.InsertRange(0, otherSms);


            foreach(var item in sms)
            {

                item["PROD_THUMB"] = BusHelper.GetSingImgByField(item["PROD_THUMB"]);

            }

            //这是排序
            sms.Sort((m1,m2) => {
        

                if(m1.Get<decimal>("ROW_USER_SEQ") == m2.Get<decimal>("ROW_USER_SEQ"))
                {
                    return 0;
                }


                if(m1.Get<decimal>("ROW_USER_SEQ") > m2.Get<decimal>("ROW_USER_SEQ")){

                    return 1;

                }

                return -1;

              });


            return HttpResult.Success(sms);
        }

        /// <summary>
        /// 获取购物车的产品数量
        /// </summary>
        /// <returns></returns>
        HttpResult GetCartNum()
        {


            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            BizFilter filter = new BizFilter(typeof(MALL_S_CART));
            filter.And("FK_W_CODE", user.LoginID);

            int num = decipher.SelectCount(filter);

            return HttpResult.Success(new { num = num });


        }



    }
}