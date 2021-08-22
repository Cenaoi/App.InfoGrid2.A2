using App.BizCommon;
using App.InfoGrid2.Model.Hairdressing;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.Mobile_V1.Prod
{
    public partial class EcProdSpec : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取规格json数据
        /// </summary>
        /// <returns></returns>
        public string GetSpecsObj()
        {


            int prod_id = WebUtil.QueryInt("prod_id");

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


            SModel sm = new SModel();



            SModelList spec_list = new SModelList();

            sm["spec_list"] = spec_list;


            foreach (var item in epdDic.OrderBy(e => e.Key))
            {

                SModel sm_data = new SModel();

                sm_data["datas"] = item.Value;


                foreach (var a in item.Value)
                {
                    sm_data["text"] = a.CATEGORY_TEXT;
                }

                spec_list.Add(sm_data);

            }



            return sm.ToJson();



        }

    }
}