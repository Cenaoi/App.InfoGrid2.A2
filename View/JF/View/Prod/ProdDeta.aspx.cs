using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
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
    public partial class ProdDeta : System.Web.UI.Page
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
        /// 商品名称
        /// </summary>
        public string ProdText { get; set; } = "空的";

        /// <summary>
        /// 商品信息数据
        /// </summary>
        public string ProdObj { get; set; } = "{}";


        /// <summary>
        /// 购物车上的那个徽章是否要显示
        /// </summary>
        public string CarBadgeShow { get; set; } = "none";

        /// <summary>
        /// 购物车上的那个徽章里面的数值
        /// </summary>
        public string CarBadgeText { get; set; } = "";


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            int prod_id = WebUtil.QueryInt("prod_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_COMMON_PROD prod = decipher.SelectModelByPk<ES_COMMON_PROD>(prod_id);

            if (prod == null)
            {
                return;
            }

            ProdText = prod.PROD_NAME;

            SModel sm = new SModel();

            sm["prod_name"] = prod.PROD_NAME;
            sm["price"] = prod.PRICE;
            sm["price_market"] = prod.PRICE_MARKET;
            sm["id"] = prod.ES_COMMON_PROD_ID;
            sm["prod_explain"] = prod.PROD_EXPLAIN;

            string[] imgs = StringUtil.Split(prod.MULTIPLE_PHOTO_LIST, "\n");

            SModelList sm_imgs = new SModelList();

            foreach(string img_url in imgs)
            {

                SModel sm_img = new SModel();

                sm_img["url"] = img_url;

                sm_imgs.Add(sm_img);

            }

            //没有多图，就用自身的缩略图
            if(imgs.Length == 0)
            {

               
                SModel sm_img = new SModel();

                sm_img["url"] = prod.PROD_THUMB;

                sm_imgs.Add(sm_img);


            }



            sm["imgs"] = sm_imgs;




            ProdObj = sm.ToJson();

            InitCar();

        }

        /// <summary>
        /// 初始化购物车徽章
        /// </summary>
        void InitCar()
        {

            EcUserState user = EcContext.Current.User;

            string w_code = user.ExpandPropertys["W_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(ES_S_CAR));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", w_code);

            int num = decipher.SelectCount(lmFilter);


            if(num > 0)
            {

                CarBadgeShow = "block";
                CarBadgeText = num.ToString();

            }



        }



    }
}