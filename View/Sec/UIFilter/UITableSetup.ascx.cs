using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using App.BizCommon;
using HWQ.Entity.Filter;
using EC5.Utility;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity.LightModels;
using EC5.Utility.Web;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.View;
using App.InfoGrid2.Model.DataSet;
using System.Xml;

namespace App.InfoGrid2.Sec.UIFilter
{
    public partial class UITableSetup : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(store1_CurrentChanged);

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            int menuId = WebUtil.QueryInt("menuId");

            DataRecord dr = this.store1.GetDataCurrent();

            if (dr == null)
            {
                return;
            }

            string roleCode = (string)dr["ROLE_CODE"];

            if (StringUtil.IsBlank(roleCode))
            {
                Error404 err = new Error404("提示", "角色没有“编码”，无法设置权限。");

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
            filter.And("SEC_MODE_ID", 1);
            //filter.And("UI_TYPE_ID", menu.PAGE_TYPE_ID);

            filter.And("MENU_ID", menuId);
            filter.And("SEC_ROLE_CODE", roleCode);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            SEC_UI secUI = decipher.SelectToOneModel<SEC_UI>(filter);

            if (secUI == null)
            {

                secUI = new SEC_UI();
                secUI.SEC_MODE_ID = 1;
                secUI.MENU_ID = menuId;
                secUI.SEC_ROLE_CODE = roleCode;
                secUI.UI_PAGE_ID = menu.SEC_PAGE_ID;
                secUI.UI_TYPE_ID = menu.SEC_PAGE_TYPE_ID;

                decipher.InsertModel(secUI);

                if (menu.SEC_PAGE_TYPE_ID == "TABLE")
                {
                    CreateTable(secUI, menu);
                }
                else if (menu.SEC_PAGE_TYPE_ID == "PAGE")
                {
                    CreatePage(secUI, menu);
                }
            }




            MiniPager.Redirect("iform1",
                string.Format("/App/InfoGrid2/Sec/UIFilter/UITableColSetup.aspx?id={0}", secUI.SEC_UI_ID));


        }

        private void CreateTable(SEC_UI ui, BIZ_C_MENU menu)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            TableSet tSet = TableSet.Select(decipher, menu.SEC_PAGE_ID);

            SEC_UI_TABLE suTable = new SEC_UI_TABLE();

            suTable.SEC_UI_TABLE_ID = menu.SEC_PAGE_ID;

            suTable.SEC_UI_ID = ui.SEC_UI_ID;
            suTable.TABLE_NAME = tSet.Table.TABLE_NAME;
            suTable.TABLE_UID = tSet.Table.TABLE_UID;
            suTable.TABLE_TEXT = tSet.Table.DISPLAY;

            suTable.PAGE_ID = menu.SEC_PAGE_ID;


            List<SEC_UI_TABLECOL> suCols = new List<SEC_UI_TABLECOL>();

            foreach (IG2_TABLE_COL tCol in tSet.Cols)
            {
                SEC_UI_TABLECOL tc = new SEC_UI_TABLECOL();
                tc.DB_FIELD = tCol.DB_FIELD;
                tc.FIELD_TEXT = tCol.F_NAME;

                tc.IS_VISIBLE = tCol.IS_VISIBLE;
                tc.IS_LIST_VISIBLE = tCol.IS_LIST_VISIBLE;
                tc.IS_SEARCH_VISIBLE = tCol.IS_SEARCH_VISIBLE;

                tc.IS_READONLY = tCol.IS_READONLY;

                suCols.Add(tc);
            }


            decipher.InsertModel(suTable);

            foreach (SEC_UI_TABLECOL uiTC in suCols)
            {
                uiTC.SEC_UI_ID = suTable.SEC_UI_ID;
                uiTC.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                decipher.InsertModel(uiTC);
            }


        }

        private void CreatePage(SEC_UI ui, BIZ_C_MENU menu)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", menu.SEC_PAGE_ID);
            filter.And("TABLE_TYPE_ID", "PAGE");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "PAGE_TEMPLATE" };

            string pageTemplate = decipher.ExecuteScalar<string>(filter);

            if (StringUtil.IsBlank(pageTemplate))
            {
                return;
            }

            List<PageTable> pTables = new List<PageTable>();

            ParseTemplate(pageTemplate, pTables);


            foreach (PageTable pTable in pTables)
            {

                if (pTable.EcType == "SEARCH")
                {
                    continue;
                }


                TableSet tSet = TableSet.Select(decipher, pTable.TableId);

                SEC_UI_TABLE suTable = new SEC_UI_TABLE();
                suTable.SEC_UI_ID = ui.SEC_UI_ID;

                suTable.SEC_UI_TABLE_ID = pTable.TableId;
                suTable.TABLE_NAME = pTable.TableName;
                suTable.TABLE_UID = tSet.Table.TABLE_UID;
                suTable.DISPALY_MODE_ID = pTable.EcType;
                suTable.PAGE_AREA_ID = pTable.ID;
                suTable.PAGE_ID = menu.SEC_PAGE_ID;
                suTable.TABLE_TEXT = tSet.Table.DISPLAY;

                List<SEC_UI_TABLECOL> suCols = new List<SEC_UI_TABLECOL>();

                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    SEC_UI_TABLECOL tc = new SEC_UI_TABLECOL();
                    tc.DB_FIELD = tCol.DB_FIELD;
                    tc.FIELD_TEXT = tCol.F_NAME;

                    tc.IS_VISIBLE = tCol.IS_VISIBLE;
                    tc.IS_LIST_VISIBLE = tCol.IS_LIST_VISIBLE;
                    tc.IS_SEARCH_VISIBLE = tCol.IS_SEARCH_VISIBLE;

                    tc.IS_READONLY = tCol.IS_READONLY;

                    suCols.Add(tc);
                }


                decipher.InsertModel(suTable);

                foreach (SEC_UI_TABLECOL uiTC in suCols)
                {
                    uiTC.SEC_UI_ID = suTable.SEC_UI_ID;
                    uiTC.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                    decipher.InsertModel(uiTC);
                }


            }



        }

        class PageTable
        {
            /// <summary>
            /// 
            /// </summary>
            public string EcType { get; set; }

            public int TableId { get; set; }

            public string TableName { get; set; }

            public string ID { get; set; }
        }

        private void ParseTemplate(string pageTemplate, List<PageTable> pTables)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pageTemplate);

            XmlNode root = doc.DocumentElement;
            XmlNode xBody = root.SelectSingleNode("body");


            ParseTemplateNode(xBody, pTables);

        }


        private void ParseTemplateNode(XmlNode xParent, List<PageTable> pTables)
        {
            foreach (XmlNode xNode in xParent.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(xNode, "ec-type");
                string ecMainView = XmlUtil.GetAttrValue(xNode, "ec-main-view");
                string ecMainName = XmlUtil.GetAttrValue(xNode, "ec-main-name");
                string id = XmlUtil.GetAttrValue(xNode, "id");

                if (!StringUtil.IsBlank(ecMainView) && !StringUtil.IsBlank(ecMainName))
                {
                    PageTable pt = new PageTable();
                    pt.EcType = ecType;
                    pt.ID = id;
                    pt.TableName = ecMainName;
                    pt.TableId = StringUtil.ToInt(ecMainView);

                    pTables.Add(pt);
                }

                ParseTemplateNode(xNode, pTables);
            }

        }
    }
}