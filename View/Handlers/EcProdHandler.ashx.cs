using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.Hairdressing;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// EcProdHandler 的摘要说明
    /// </summary>
    public class EcProdHandler : IHttpHandler
    {


        /// <summary>
        /// 当前界面提交的json对象
        /// </summary>
        public class cur_json
        {
            /// <summary>
            /// 规格ID
            /// </summary>
            public int spec_id { get; set; }

            /// <summary>
            /// 产品ID
            /// </summary>
            public int prod_id { get; set; }

            /// <summary>
            /// 规格数量
            /// </summary>
            public int spec_num { get; set; }


        }

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

                case "GET_PRODSPEC_BY_ID":
                    GetProdSpecById();
                    break;
                case "PUSH_IN_CART":
                    PushInCart();
                    break;

            }

        }



        /// <summary>
        /// 根据ID获取规格信息
        /// </summary>
        void GetProdSpecById()
        {

            int prod_id = WebUtil.FormInt("prod_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilterEpd = new LightModelFilter(typeof(ES_PRODUCT_DATA));
            lmFilterEpd.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterEpd.And("PROD_ID", prod_id);
            lmFilterEpd.And("IS_VALID", true);


            List<ES_PRODUCT_DATA> epdList = decipher.SelectModels<ES_PRODUCT_DATA>(lmFilterEpd);


            Dictionary<string, List<ES_PRODUCT_DATA>> epdDic = new Dictionary<string, List<ES_PRODUCT_DATA>>();

            //把规格分类
            foreach (ES_PRODUCT_DATA item in epdList)
            {
                //分类名称
                string keyName = item.CATEGORY_TEXT.Trim();

                if (epdDic.ContainsKey(keyName))
                {
                    epdDic[keyName].Add(item);
                    continue;
                }

                List<ES_PRODUCT_DATA> epdListNew = new List<ES_PRODUCT_DATA>() { item };

                epdDic.Add(keyName, epdListNew);

            }


            StringBuilder sb = new StringBuilder();

            sb.Append("{\"result\":\"ok\",\"msg\":\"获取规格成功了！\",\"spec_list\":[");

            int i = 0;
            foreach (var item in epdDic.OrderBy(e => e.Key))
            {
                if (i++ > 0)
                {
                    sb.Append(",");
                }

                sb.Append("{\"data\":[");

                int j = 0;
                foreach (var subItem in item.Value)
                {
                    if (j++ > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(ModelHelper.ToJson(subItem));
                }

                sb.Append("]}");

            }

            sb.Append("]}");

            m_response.Write(sb.ToString());


        }

        /// <summary>
        /// 添加数据到购物车里面
        /// </summary>
        void PushInCart()
        {

            string postJson = WebUtil.Form("data");



            List<cur_json> curJsons = null;

            try
            {

                //获取到界面上提交上来的数据，准备放入到购物车里面
                curJsons = JsonConvert.DeserializeObject<List<cur_json>>(postJson);

            }
            catch (Exception ex)
            {
                log.Error("出错了！插入购物车失败了！", ex);
                ResponseHelper.Result_error("提交的数据有问题了！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {

                List<ES_SHOPPING_CART> escList = new List<ES_SHOPPING_CART>();


                foreach (var item in curJsons)
                {

                    ES_COMMON_PROD ecp = decipher.SelectModelByPk<ES_COMMON_PROD>(item.prod_id);

                    ES_PRODUCT_DATA epd = decipher.SelectModelByPk<ES_PRODUCT_DATA>(item.spec_id);

                    if (ecp == null || epd == null)
                    {
                        ResponseHelper.Result_error("提交的数据有问题了！");
                        return;
                    }

                    ES_SHOPPING_CART esc = new ES_SHOPPING_CART();
                    ecp.CopyTo(esc, true);

                    esc.ROW_DATE_CREATE = DateTime.Now;
                    esc.ROW_DATE_UPDATE = DateTime.Now;
                    esc.PROD_DATA_ID = epd.ES_PRODUCT_DATA_ID;
                    esc.PROD_DATA_NAME = epd.SPEC_NAME;
                    esc.CATEGORY_TEXT = epd.CATEGORY_TEXT;
                    esc.PROD_NUM = item.spec_num;
                    esc.PROD_ID = ecp.ES_COMMON_PROD_ID;

                    esc.SUB_TOTAL_MONEY = esc.PRICE_MEMBER * esc.PROD_NUM;

                    escList.Add(esc);

                }


                decipher.InsertModels(escList);


                ResponseHelper.Result_ok("加入购物车了！");

            }
            catch (Exception ex)
            {
                log.Error("插入购物车失败了！", ex);
                ResponseHelper.Result_error("哦噢，有错误哦！");
                return;
            }
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