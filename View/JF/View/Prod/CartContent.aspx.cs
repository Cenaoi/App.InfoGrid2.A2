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

namespace App.InfoGrid2.JF.View.Prod
{
    public partial class CartContent : System.Web.UI.Page
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
        /// 获取购物车里面的信息
        /// </summary>
        /// <returns></returns>
        public string GetCarsObj()
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

            foreach(ES_S_CAR car in cars)
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


            return sm_cars.ToJson();






        }

    }
}