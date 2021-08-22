using App.BizCommon;
using App.InfoGrid2.Handlers;
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
    /// 商品的处理程序
    /// </summary>
    public class ProdHandler : IHttpHandler, IRequiresSessionState
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

                    //加入购物车按钮点击事件
                    case "PUSH_CAR_BTN":
                        PUSH_CAR_BTN();
                        break;
                    //这是规格那边确定按钮事件
                    case "PUSH_IN_CART":
                        PushInCart();
                        break;
                    case "CHECKED_CAR":
                        CheckedCar();
                        break;
                    case "DELETE_CAR":
                        DeleteCar();
                        break;
                    case "CHECKED_CAR_ALL":
                        CheckedCarAll();
                        break;
                    default:
                        ResponseHelper.Result_error("写错了吧！");
                        break;

                }


            }catch(Exception ex)
            {

                log.Debug(ex);

                ResponseHelper.Result_error("哦噢，出错了喔！");
            }

            
        }

        /// <summary>
        /// 加入购物车按钮点击事件 要判断有没有其它规格先 没有再直接添加进购物车中，有其它规格就要返回错误，让前端显示规格选择界面
        /// </summary>
        void PUSH_CAR_BTN()
        {

            int prod_id = WebUtil.FormInt("prod_id");

            int num = WebUtil.FormInt("num");

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            if (!user.Roles.Exist("WX"))
            {
                ResponseHelper.Result_error("请重新登录！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD prod = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            if(prod == null)
            {
                ResponseHelper.Result_error("找不到商品数据！");

                return;
            }

            SModel sm = new SModel();

            string group_code = prod.GROUP_CODE;

            if (string.IsNullOrEmpty(group_code))
            {
                sm["is_ok"] = false;
                ResponseHelper.Result_ok(sm);
                return;

            }

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_COMMON_PROD));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("GROUP_CODE", group_code);
            lmFilter.And("CAN_SALE", true);

            List<ES_COMMON_PROD> prods = decipher.SelectModels<ES_COMMON_PROD>(lmFilter);

            //找到其它规格数据，如果大于1就代表有其它规格数据，就要弹出选择规格选择界面了
            if (prods.Count > 1)
            {
                sm["is_ok"] = false;
                ResponseHelper.Result_ok(sm);
                return;

            }


            ES_S_CAR car = CreateCar(prod, num, w_code);

            decipher.InsertModel(car);

            sm["is_ok"] = true;

            ResponseHelper.Result_ok(sm);


        }


        /// <summary>
        /// 把商品放入购物车里面
        /// </summary>
        void PushInCart()
        {

            int prod_id = WebUtil.FormInt("prod_id");

            int num = WebUtil.FormInt("num");

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            if (!user.Roles.Exist("WX"))
            {
                ResponseHelper.Result_error("请重新登录！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD prod = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            ES_S_CAR car = CreateCar(prod, num, w_code);

            decipher.InsertModel(car);

            ResponseHelper.Result_ok("插入购物车成功了！");


        }

        void CheckedCar()
        {

            int car_id = WebUtil.FormInt("car_id");

            bool is_checked = WebUtil.FormBool("is_checked");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_S_CAR car = decipher.SelectModelByPk<ES_S_CAR>(car_id);


            car.IS_CHECKED = is_checked;
            car.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(car, "IS_CHECKED", "ROW_DATE_UPDATE");


            SModelList sm_cars = GetCars();
            


            ResponseHelper.Result_ok(sm_cars);


        }

        void DeleteCar()
        {

            //这个到时候还要加上登录账号的编码
            string delete_sql = "DELETE FROM ES_S_CAR WHERE ROW_SID >= 0 and IS_CHECKED = 1";

            DbDecipher decipher = ModelAction.OpenDecipher();

            //执行删除购物车里面的数据，不是假删除
            decipher.ExecuteNonQuery(delete_sql);


            SModelList sm_cars = GetCars();





            ResponseHelper.Result_ok(sm_cars);


        }

        /// <summary>
        /// 全选
        /// </summary>
        void CheckedCarAll()
        {

            bool is_checked = WebUtil.FormBool("is_checked");

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = $"update ES_S_CAR set IS_CHECKED = {(is_checked ? 1:0)} where ROW_SID >= 0";

            decipher.ExecuteNonQuery(sql);

            SModelList sm_cars = GetCars();

            ResponseHelper.Result_ok(sm_cars);
            

        }


        /// <summary>
        /// 创建购物车对象
        /// </summary>
        /// <param name="prod">商品对象</param>
        /// <param name="num">数量</param>
        /// <param name="w_code">微信用户编码</param>
        /// <returns></returns>
        ES_S_CAR CreateCar(ES_COMMON_PROD prod,int num,string w_code)
        {


            ES_S_CAR car = new ES_S_CAR();

            car.FK_P_CODE = prod.PK_P_CODE;
            car.PK_SC_CODE = BillIdentityMgr.NewCodeForDay("CAR_CODE", "C", 5);
            car.PRICE = prod.PRICE;
            car.PRICE = prod.PRICE;
            car.PRICE_AGENT = prod.PRICE_AGENT;
            car.PRICE_MARKET = prod.PRICE_MARKET;
            car.PROD_NUM = num;
            car.SUB_TOTA = num * prod.PRICE;
            car.PROD_TEXT = prod.PROD_NAME;
            car.PROD_INTRO = prod.PROD_INTRO;
            car.ROW_DATE_CREATE = car.ROW_DATE_UPDATE = DateTime.Now;
            car.FK_W_CODE = w_code;
            car.PROD_THUMB = prod.PROD_THUMB;
            car.IS_CHECKED = true;

            return car;


        }


        /// <summary>
        /// 获取购物车数据  统一从这里拿
        /// </summary>
        /// <returns></returns>
        SModelList GetCars()
        {

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_S_CAR));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", w_code);

            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<ES_S_CAR> cars = decipher.SelectModels<ES_S_CAR>(lmFilter);

            SModelList sm_cars = new SModelList();

            foreach (ES_S_CAR car in cars)
            {

                SModel sm = new SModel();

                sm["price"] = car.PRICE;
                sm["prod_text"] = car.PROD_TEXT;
                sm["prod_intro"] = car.PROD_INTRO;
                sm["prod_num"] = car.PROD_NUM;
                sm["is_checked"] = car.IS_CHECKED;
                sm["car_id"] = car.ES_S_CAR_ID;
                sm["prod_thumb"] = car.PROD_THUMB;


                sm_cars.Add(sm);

            }


            return sm_cars;



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