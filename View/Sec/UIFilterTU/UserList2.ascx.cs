using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.View;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.Sec.UIFilterTU
{
    public partial class UserList2 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += store1_CurrentChanged;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

       
      

        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            int pageId = WebUtil.QueryInt("pageId");
            int tableId = WebUtil.QueryInt("tableId");
            string tabId = WebUtil.Query("tabId");
            string tabName = WebUtil.Query("tabName");
            string fieldName = WebUtil.Query("fieldName");
            string viewType = WebUtil.Query("viewType");

            int menuId = WebUtil.QueryInt("menuId");

            DataRecord dr = this.store1.GetDataCurrent();

            if (dr == null)
            {
                return;
            }

            string userCode = (string)dr["BIZ_USER_CODE"];

            if (StringUtil.IsBlank(userCode))
            {
                Error404 err = new Error404("提示", "用户没有“用户编码”，无法设置权限。");

                MiniPager.Redirect("iform1", "/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + err.GetBase64());

                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_MENU menu = decipher.SelectModelByPk<BIZ_C_MENU>(menuId);

            if (StringUtil.IsBlank(menu.SEC_PAGE_TYPE_ID))
            {
                Error404 err = new Error404("提示", "这个页面无需权限，不用设置。");

                MiniPager.Redirect("iform1", "/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + err.GetBase64());

                return;
            }


            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI));
            filter.And("SEC_MODE_ID", 2);
            //filter.And("UI_TYPE_ID", menu.PAGE_TYPE_ID);

            filter.And("UI_PAGE_ID", pageId);


            filter.And("MENU_ID", menuId);
            filter.And("SEC_USER_CODE", userCode);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.TSqlOrderBy = "SEC_UI_ID desc";

            SEC_UI secUI = decipher.SelectToOneModel<SEC_UI>(filter);

            if (secUI == null)
            {
                return;
            }




            IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(tableId);
            List<IG2_TABLE_COL> itcList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0}", tableId);

            LightModelFilter filtersut = new LightModelFilter(typeof(SEC_UI_TABLE));
            filtersut.And("DIALOG_TABLE_ID", tableId);
            filtersut.And("TABLE_NAME", it.TABLE_NAME);
            filtersut.And("PAGE_ID", pageId);
            filtersut.And("PAGE_AREA_ID", tabId);
            filtersut.And("DISPALY_MODE_ID", tabName);
            filtersut.And("SEC_ITEM_TYPE", viewType);
            filtersut.And("DIALOG_FIELD", fieldName);
            filtersut.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            filtersut.And("SEC_UI_ID", secUI.SEC_UI_ID);

            SEC_UI_TABLE sutss = decipher.SelectToOneModel<SEC_UI_TABLE>(filtersut);


            if(sutss != null)
            {
                MiniPager.Redirect("iform1",
                string.Format("/App/InfoGrid2/Sec/UIFilter/UITableColSetup2.aspx?id={0}&secId={1}", sutss.SEC_UI_TABLE_ID, secUI.SEC_UI_ID));
                return;
            }

            SEC_UI_TABLE sut = new SEC_UI_TABLE() 
            {
                 SEC_UI_ID = secUI.SEC_UI_ID,
                 TABLE_NAME = it.TABLE_NAME,
                 TABLE_TEXT = it.DISPLAY,
                 PAGE_ID = pageId,
                 DISPALY_MODE_ID = tabName,
                 PAGE_AREA_ID = tabId,
                 DIALOG_FIELD = fieldName,
                 DIALOG_TABLE_ID = tableId,
                 SEC_ITEM_TYPE = viewType,
                 TABLE_UID = it.TABLE_UID
            };


            decipher.InsertModel(sut);

            List<SEC_UI_TABLECOL> sutcList = new List<SEC_UI_TABLECOL>();

            foreach (var item in itcList)
            {
                SEC_UI_TABLECOL sutc = new SEC_UI_TABLECOL() 
                {
                     SEC_UI_ID = secUI.SEC_UI_ID,
                     SEC_UI_TABLE_ID = sut.SEC_UI_TABLE_ID,
                     DB_FIELD = item.DB_FIELD,
                     FIELD_TEXT = item.DISPLAY,
                     IS_LIST_VISIBLE = item.IS_LIST_VISIBLE,
                     IS_READONLY = item.IS_READONLY,
                     IS_VISIBLE = item.IS_VISIBLE,
                     IS_SEARCH_VISIBLE = item.IS_SEARCH_VISIBLE,
                     IS_LIST_VISIBLE_B =item.IS_LIST_VISIBLE,
                     IS_READONLY_B = item.IS_READONLY,
                     IS_SEARCH_VISIBLE_B = item.IS_SEARCH_VISIBLE,
                     IS_VISIBLE_B =item.IS_VISIBLE

                };
                sutcList.Add(sutc);
            }


            decipher.InsertModels<SEC_UI_TABLECOL>(sutcList);

            MiniPager.Redirect("iform1",
                string.Format("/App/InfoGrid2/Sec/UIFilter/UITableColSetup2.aspx?id={0}&secId={1}", sut.SEC_UI_TABLE_ID, secUI.SEC_UI_ID));

        }
        
    }
}