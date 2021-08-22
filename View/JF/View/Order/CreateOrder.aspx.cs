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
    public partial class CreateOrder : System.Web.UI.Page
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
                
                


                InitData();
            }
        }

        /// <summary>
        /// 选中商品总金额
        /// </summary>
        public decimal Total { get; set; } = 0;

        /// <summary>
        /// 选中商品数量
        /// </summary>
        public int Num { get; set; } = 0;


        /// <summary>
        /// 选中购物车商品数据
        /// </summary>
        public string CarsObj { get; set; }


        /// <summary>
        /// 获取已选中的购物车商品
        /// </summary>
        /// <returns></returns>
        void InitData()
        {

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_S_CAR));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", w_code);
            lmFilter.And("IS_CHECKED", true);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<ES_S_CAR> cars = decipher.SelectModels<ES_S_CAR>(lmFilter);

            Num = cars.Count;

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


                Total += car.PRICE * car.PROD_NUM;

            }


            CarsObj =  sm_cars.ToJson();



        }


        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public string GetUserObj()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_W_ACCOUNT account = decipher.SelectModelByPk<ES_W_ACCOUNT>(user.Identity);


            SModel sm = new SModel();

            sm["contacter_name"] = account.CONTACTER_NAME;
            sm["contacter_tel"] = account.CONTACTER_TEL;
            sm["address_t2"] = account.ADDRESS_T2;


            return sm.ToJson();



        }


    }
}