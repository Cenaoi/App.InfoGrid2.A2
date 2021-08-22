using App.BizCommon;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.Model.CMS;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.DJK.Handlers
{
    /// <summary>
    /// CmsHandler 的摘要说明
    /// </summary>
    public class CmsHandler : IHttpHandler,IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            string action = WebUtil.Form("action")?.Trim()?.ToUpper();

            try
            {

                switch (action)
                {

                    case "GET_ITEMS":
                        GetItems();
                        break;
                    case "GET_ITEM":
                        GetItem();
                        break;
                    case "GET_CATALOGS":
                        GetCatalogs();
                        break;
                    case "GET_CATALOG":
                        GetCatalog();
                        break;
                    case "GET_TILE":
                        GetTile();
                        break;
                    default:
                        ResponseHelper.Result_error("没有这个接口喔！");
                        break;
                }

            }catch(Exception ex)
            {

                log.Error(ex);

                ResponseHelper.Result_error("哦噢，出错了！");

            }

            
        }


        /// <summary>
        /// 获取明细数据 根据 menu_id 和 item_id
        /// </summary>
        void GetItem()
        {


            int item_id = WebUtil.FormInt("item_id");

         

            CMS_MENU cms_menu = GetMenu();

            if (cms_menu == null)
            {
                ResponseHelper.Result_error("找不到菜单对象！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            string item_table_name = cms_menu.TABLE_NAME + "_ITEM";

            LightModelFilter lmFilter = new LightModelFilter(item_table_name);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", item_id);

            SModel sm_item = decipher.GetSModel(lmFilter);

            ResponseHelper.Result_ok(sm_item);
            
        }

        /// <summary>
        /// 获取item数据集合  根据 menu_id 和 catalog_id 
        /// </summary>
        void GetItems()
        {

            int cata_id = WebUtil.FormInt("cata_id");


            CMS_MENU cms_menu = GetMenu();

            if (cms_menu == null)
            {

                ResponseHelper.Result_error("哦噢，找不到菜单对象哦！");

                return;

            }

            DbDecipher decipher = ModelAction.OpenDecipher();


            string cata_table_name = cms_menu.TABLE_NAME + "_CATALOG";

            string item_table_name = cms_menu.TABLE_NAME+"_ITEM";

            LightModelFilter lmFilter = new LightModelFilter(cata_table_name);
            lmFilter.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", cata_id);


            SModel sm_cata = decipher.GetSModel(lmFilter);


            if(sm_cata == null)
            {
                ResponseHelper.Result_error("找不到目录对象！");
                return;
            }
            

            LightModelFilter lmFilterItem = new LightModelFilter(item_table_name);
            lmFilterItem.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterItem.And("FK_CATALOG_CODE", sm_cata["PK_CATALOG_CODE"]);


            SModelList sm_items = decipher.GetSModelList(lmFilterItem);

            ResponseHelper.Result_ok(sm_items);

        }

        /// <summary>
        /// 获取目录集合数据  根据 menu_id
        /// </summary>
        void GetCatalogs()
        {

            CMS_MENU cms_menu = GetMenu();

            if (cms_menu == null)
            {
                ResponseHelper.Result_error("找不到菜单对象！");
                return;
            }
            DbDecipher decipher = ModelAction.OpenDecipher();

            string cata_table_name = cms_menu.TABLE_NAME + "_CATALOG";


            SModelList sm_catas = decipher.GetSModelList($"select * from {cata_table_name} where ROW_SID >= 0");


            ResponseHelper.Result_ok(sm_catas);




        }

        /// <summary>
        /// 获取目录数据 根据 cata_id 
        /// </summary>
        void GetCatalog()
        {

            int cata_id = WebUtil.FormInt("cata_id");

            CMS_MENU cms_menu = GetMenu();

            if (cms_menu == null)
            {
                ResponseHelper.Result_error("找不到菜单对象！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            string cata_table_name = cms_menu.TABLE_NAME + "_CATALOG";

            LightModelFilter lmFilter = new LightModelFilter(cata_table_name);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", cata_id);

            SModel sm_cata = decipher.GetSModel(lmFilter);

            ResponseHelper.Result_ok(sm_cata);

        }

        /// <summary>
        /// 获取单页数据 根据 menu_id
        /// </summary>
        void GetTile()
        {

            CMS_MENU cms_menu = GetMenu();

            if (cms_menu == null)
            {
                ResponseHelper.Result_error("找不到菜单对象！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(CMS_TILE));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            lmFilter.And("FK_MENU_CODE", cms_menu.PK_MENU_CODE);

            SModel sm_tile = decipher.GetSModel(lmFilter);

            ResponseHelper.Result_ok(sm_tile);

        }

        /// <summary>
        /// 根据传过来的menu_id 获取菜单对象
        /// </summary>
        /// <returns></returns>
        CMS_MENU GetMenu()
        {

            int menu_id = WebUtil.FormInt("menu_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            CMS_MENU cms_menu = decipher.SelectModelByPk<CMS_MENU>(menu_id);

            return cms_menu;



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