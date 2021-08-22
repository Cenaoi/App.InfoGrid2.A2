using App.BizCommon;
using App.InfoGrid2.Model.CMS;
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

namespace App.InfoGrid2.DJK
{
    /// <summary>
    /// 文章界面啦
    /// </summary>
    public partial class ArticlePage : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取菜单名称
        /// </summary>
        /// <returns></returns>
        public string GetMenuTextStr()
        {


            int menu_id = WebUtil.QueryInt("menu_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            CMS_MENU cms_menu = decipher.SelectModelByPk<CMS_MENU>(menu_id);


            if(cms_menu == null)
            {
                return "";
            }

            return cms_menu.MENU_TEXT;


        }


        /// <summary>
        /// 获取目录数组对象
        /// </summary>
        public string GetCatasObj()
        {

            int menu_id = WebUtil.QueryInt("menu_id");
            
            DbDecipher decipher = ModelAction.OpenDecipher();

            CMS_MENU cms_menu = decipher.SelectModelByPk<CMS_MENU>(menu_id);

            SModelList sm_catas = new SModelList();

            if(cms_menu == null)
            {
                return sm_catas.ToJson();
            }

            string catalog_table_name = cms_menu.TABLE_NAME+ "_CATALOG";

            string item_table_name =  cms_menu.TABLE_NAME + "_ITEM";

            //如果找不到目录表和明细表就不用处理
            if (!decipher.DatabaseBuilder.ExistTable(catalog_table_name) || !decipher.DatabaseBuilder.ExistTable(item_table_name))
            {
                return sm_catas.ToJson();


            }


            LightModelFilter lmFilterCata = new LightModelFilter(catalog_table_name);
            lmFilterCata.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            sm_catas = decipher.GetSModelList(lmFilterCata);
            
            foreach(SModel sm_cata in sm_catas)
            {

                string pk_catalog_code = sm_cata.Get<string>("PK_CATALOG_CODE");

                if (string.IsNullOrWhiteSpace(pk_catalog_code))
                {
                    continue;
                }

                LightModelFilter lmFilterItem = new LightModelFilter(item_table_name);
                lmFilterItem.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilterItem.And("FK_CATALOG_CODE", pk_catalog_code);

                SModelList sm_items = decipher.GetSModelList(lmFilterItem);

                sm_cata["sub_data"] = sm_items;

            }

           return  sm_catas.ToJson();


        }


    }
}