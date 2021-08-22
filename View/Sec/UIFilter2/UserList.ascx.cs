using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.View;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Sec.UIFilter2
{
    public partial class UserList : WidgetControl, IView
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

            int formId = WebUtil.QueryInt("form_id");

            string page_sub_type_id = WebUtil.Query("page_sub_type_id");


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

            string secPageTypeId = menu.SEC_PAGE_TYPE_ID;

            string secPageSubTypeId = string.Empty;

            if (page_sub_type_id == "ONE_FORM")
            {
                secPageTypeId = "PAGE";
            }


            LightModelFilter filter = new LightModelFilter(typeof(SEC_UI));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("SEC_MODE_ID", 2);
            filter.And("UI_TYPE_ID", secPageTypeId);

            if (page_sub_type_id == "ONE_FORM")
            {
                filter.And("UI_SUB_TYPE_ID", "ONE_FORM");
            }
            else
            {
                filter.And("UI_SUB_TYPE_ID", string.Empty);
            }

            filter.And("MENU_ID", menuId);
            filter.And("SEC_USER_CODE", userCode);
            filter.TSqlOrderBy = "SEC_UI_ID desc";

            SEC_UI secUI = decipher.SelectToOneModel<SEC_UI>(filter);

            if (secUI == null)
            {

                secUI = new SEC_UI();
                secUI.SEC_MODE_ID = 2;
                secUI.MENU_ID = menuId;
                secUI.SEC_USER_CODE = userCode;

                if (page_sub_type_id == "ONE_FORM")
                {
                    secUI.UI_PAGE_ID = formId;
                    secUI.UI_TYPE_ID = "PAGE";
                    secUI.UI_SUB_TYPE_ID = "ONE_FORM";
                }
                else
                {
                    secUI.UI_PAGE_ID = menu.SEC_PAGE_ID;
                    secUI.UI_TYPE_ID = menu.SEC_PAGE_TYPE_ID;
                }


                decipher.InsertModel(secUI);


                LightModelFilter filterDe = new LightModelFilter(typeof(SEC_UI));
                filterDe.And("MENU_ID", menuId);
                filterDe.And("SEC_USER_CODE", userCode);
                filterDe.And("ROW_SID", -3);
                filterDe.TSqlOrderBy = "ROW_DATE_CREATE desc";

                SEC_UI secUIDe = decipher.SelectToOneModel<SEC_UI>(filterDe);


                if (page_sub_type_id == "ONE_FORM")
                {
                    CreatePage(secUI, menu, formId, "ONE_FORM", secUIDe);
                }
                else
                {
                    if (menu.SEC_PAGE_TYPE_ID == "TABLE")
                    {
                        CreateTable(secUI, menu, secUIDe);
                    }
                    else if (menu.SEC_PAGE_TYPE_ID == "PAGE")
                    {
                        CreatePage(secUI, menu, 0, string.Empty, secUIDe);
                    }
                }
            }




            MiniPager.Redirect("iform1",
                string.Format("/App/InfoGrid2/Sec/UIFilter2/UITableColSetup.aspx?id={0}", secUI.SEC_UI_ID));


        }

        private void CreateTable(SEC_UI ui, BIZ_C_MENU menu, SEC_UI uiDe)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            TableSet tSet = TableSet.Select(decipher, menu.SEC_PAGE_ID);

            //拿重置之前的所有列的信息
            List<SEC_UI_TABLECOL> sutDe = new List<SEC_UI_TABLECOL>();
            if (uiDe != null)
            {
                sutDe = decipher.SelectModels<SEC_UI_TABLECOL>("SEC_UI_ID = {0} and ROW_SID = -3", uiDe.SEC_UI_ID);
            }


            SEC_UI_TABLE suTable = new SEC_UI_TABLE();

            suTable.SEC_UI_TABLE_ID = menu.SEC_PAGE_ID;

            suTable.SEC_UI_ID = ui.SEC_UI_ID;
            suTable.TABLE_NAME = tSet.Table.TABLE_NAME;
            suTable.TABLE_UID = tSet.Table.TABLE_UID;
            suTable.TABLE_TEXT = tSet.Table.DISPLAY;

            suTable.PAGE_ID = menu.SEC_PAGE_ID;


            List<SEC_UI_TABLECOL> suCols = new List<SEC_UI_TABLECOL>();

            SEC_UI_TABLECOL oldTabCol = null;

            foreach (IG2_TABLE_COL tCol in tSet.Cols)
            {
                SEC_UI_TABLECOL tc = new SEC_UI_TABLECOL();
                tc.DB_FIELD = tCol.DB_FIELD;
                tc.FIELD_TEXT = StringUtil.NoBlank(tCol.DISPLAY, tCol.F_NAME);

                tc.IS_VISIBLE = tCol.IS_VISIBLE;
                tc.IS_LIST_VISIBLE = tCol.IS_LIST_VISIBLE;
                tc.IS_SEARCH_VISIBLE = tCol.IS_SEARCH_VISIBLE;

                tc.IS_READONLY = tCol.IS_READONLY;

                oldTabCol = null;

                //这是拿之前已经设置好的排序，过滤条件
                if (sutDe != null && sutDe.Count != 0)
                {
                    oldTabCol = sutDe.Find(s => s.DB_FIELD == tc.DB_FIELD);

                    if (oldTabCol != null)
                    {
                        oldTabCol.CopyTo(tc, true);
                        tc.FIELD_TEXT = tCol.F_NAME;
                        tc.ROW_SID = 0;
                        tc.ROW_DATE_DELETE = null;

                    }

                }


                if (oldTabCol == null)
                {
                    tc.IS_LIST_VISIBLE_B = tc.IS_LIST_VISIBLE;
                    tc.IS_READONLY_B = tc.IS_READONLY;
                    tc.IS_SEARCH_VISIBLE_B = tc.IS_SEARCH_VISIBLE;
                    tc.IS_VISIBLE_B = tc.IS_VISIBLE;
                    tc.SORT_TYPE_B = tc.SORT_TYPE;
                    tc.SORT_ORDER_B = tc.SORT_ORDER;
                }


                suCols.Add(tc);
            }


            decipher.InsertModel(suTable);

            foreach (SEC_UI_TABLECOL uiTC in suCols)
            {
                uiTC.SEC_UI_ID = suTable.SEC_UI_ID;
                uiTC.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                decipher.InsertModel(uiTC);
            }


            #region 插入权限工具栏数据

            IG2_TOOLBAR tool = decipher.SelectToOneModel<IG2_TOOLBAR>("TABLE_ID={0}", tSet.Table.IG2_TABLE_ID);

            if (tool == null)
            { return; }
            SEC_UI_TOOLBAR sut = new SEC_UI_TOOLBAR();
            tool.CopyTo(sut, true);
            sut.SEC_UI_ID = ui.SEC_UI_ID;
            sut.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

            try
            {
                decipher.InsertModel(sut);
            }
            catch (Exception ex)
            {
                log.Error("插入权限工具栏数据出错了！", ex);
                throw new Exception("插入权限工具栏数据出错了！", ex);
            }

            List<IG2_TOOLBAR_ITEM> toolItems = decipher.SelectModels<IG2_TOOLBAR_ITEM>(
                "TABLE_ID={0} and IG2_TOOLBAR_ID={1} and ROW_SID >=0",
                tSet.Table.IG2_TABLE_ID, tool.IG2_TOOLBAR_ID);

            List<SEC_UI_TOOLBAR_ITEM> sutiList = new List<SEC_UI_TOOLBAR_ITEM>();

            foreach (var item in toolItems)
            {
                SEC_UI_TOOLBAR_ITEM suti = new SEC_UI_TOOLBAR_ITEM();
                item.CopyTo(suti, true);
                suti.SEC_UI_TOOLBAR_ID = sut.SEC_UI_TOOLBAR_ID;
                suti.SEC_UI_ID = ui.SEC_UI_ID;
                suti.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;
                sutiList.Add(suti);
            }


            try
            {
                decipher.InsertModels<SEC_UI_TOOLBAR_ITEM>(sutiList);

            }
            catch (Exception ex)
            {
                log.Error("插入权限工具栏条目出错了！", ex);
                throw new Exception("插入权限工具栏条目出错了！", ex);
            }


            #endregion




        }



        private void CreatePage(SEC_UI ui, BIZ_C_MENU menu, int formId, string subTypeId, SEC_UI uiDe)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            //拿到重置之前所有表信息
            List<SEC_UI_TABLE> sutList = new List<SEC_UI_TABLE>();

            if (uiDe != null)
            {
                sutList = decipher.SelectModels<SEC_UI_TABLE>("SEC_UI_ID = {0} and ROW_SID = -3", uiDe.SEC_UI_ID);
            }



            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.AddFilter("ROW_SID >= 0");

            if (subTypeId == "ONE_FORM")
            {
                filter.And("IG2_TABLE_ID", formId);
                filter.And("TABLE_TYPE_ID", "PAGE");
                filter.And("TABLE_SUB_TYPE_ID", "ONE_FORM");
            }
            else
            {
                filter.And("IG2_TABLE_ID", menu.SEC_PAGE_ID);
                filter.And("TABLE_TYPE_ID", "PAGE");
                filter.And("TABLE_SUB_TYPE_ID", "");
            }

            filter.Fields = new string[] { "PAGE_TEMPLATE" };

            string pageTemplate = decipher.ExecuteScalar<string>(filter);

            if (StringUtil.IsBlank(pageTemplate))
            {
                return;
            }

            List<PageTable> pTables = new List<PageTable>();

            ParseTemplate(pageTemplate, pTables);


            SEC_UI_TABLECOL oldTabCol;

            foreach (PageTable pTable in pTables)
            {

                //if (pTable.EcType == "SEARCH")
                //{
                //    continue;
                //}

                TableSet tSet = TableSet.Select(decipher, pTable.TableId);

                SEC_UI_TABLE suTable = new SEC_UI_TABLE();
                suTable.SEC_UI_ID = ui.SEC_UI_ID;

                suTable.SEC_UI_TABLE_ID = pTable.TableId;
                suTable.TABLE_NAME = pTable.TableName;
                suTable.TABLE_UID = tSet.Table.TABLE_UID;
                suTable.DISPALY_MODE_ID = pTable.EcType;
                suTable.PAGE_AREA_ID = pTable.ID;


                if (subTypeId == "ONE_FORM")
                {
                    suTable.PAGE_ID = formId;
                }
                else
                {
                    suTable.PAGE_ID = menu.SEC_PAGE_ID;
                }



                if (pTable.EcType == "SEARCH")
                {
                    suTable.TABLE_TEXT = "(查询)" + tSet.Table.DISPLAY;
                }
                else
                {
                    suTable.TABLE_TEXT = tSet.Table.DISPLAY;
                }

                //拿到重置前所有列信息
                List<SEC_UI_TABLECOL> sutcList = new List<SEC_UI_TABLECOL>();
                if (sutList != null && sutList.Count > 0)
                {
                    var item = sutList.Find(s => s.TABLE_NAME == suTable.TABLE_NAME && s.TABLE_TEXT == suTable.TABLE_TEXT);

                    if (item != null)
                    {
                        sutcList = decipher.SelectModels<SEC_UI_TABLECOL>("SEC_UI_TABLE_ID = {0} and SEC_UI_ID = {1} and ROW_SID = -3", item.SEC_UI_TABLE_ID, item.SEC_UI_ID);
                    }

                }

                List<SEC_UI_TABLECOL> suCols = new List<SEC_UI_TABLECOL>();

                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    SEC_UI_TABLECOL tc = new SEC_UI_TABLECOL();
                    tc.DB_FIELD = tCol.DB_FIELD;
                    tc.FIELD_TEXT = StringUtil.NoBlank(tCol.DISPLAY, tCol.F_NAME);

                    tc.IS_VISIBLE = tCol.IS_VISIBLE;
                    tc.IS_LIST_VISIBLE = tCol.IS_LIST_VISIBLE;
                    tc.IS_SEARCH_VISIBLE = tCol.IS_SEARCH_VISIBLE;

                    tc.IS_READONLY = tCol.IS_READONLY;

                    oldTabCol = null;

                    //这是拿之前已经设置好的排序，过滤条件
                    if (sutcList != null && sutcList.Count != 0)
                    {
                        oldTabCol = sutcList.Find(s => s.DB_FIELD == tc.DB_FIELD);

                        if (oldTabCol != null)
                        {
                            oldTabCol.CopyTo(tc, true);

                            tc.FIELD_TEXT = StringUtil.NoBlank(tCol.DISPLAY, tCol.F_NAME);
                            tc.ROW_SID = 0;
                            tc.ROW_DATE_DELETE = null;

                        }

                    }


                    if (oldTabCol == null)
                    {
                        tc.IS_LIST_VISIBLE_B = tc.IS_LIST_VISIBLE;
                        tc.IS_READONLY_B = tc.IS_READONLY;
                        tc.IS_SEARCH_VISIBLE_B = tc.IS_SEARCH_VISIBLE;
                        tc.IS_VISIBLE_B = tc.IS_VISIBLE;
                        tc.SORT_TYPE_B = tc.SORT_TYPE;
                        tc.SORT_ORDER_B = tc.SORT_ORDER;
                    }


                    suCols.Add(tc);
                }


                decipher.InsertModel(suTable);

                foreach (SEC_UI_TABLECOL uiTC in suCols)
                {
                    uiTC.SEC_UI_ID = suTable.SEC_UI_ID;
                    uiTC.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                    decipher.InsertModel(uiTC);
                }


                #region 插入权限工具栏数据

                IG2_TOOLBAR tool = decipher.SelectToOneModel<IG2_TOOLBAR>("TABLE_ID={0}", tSet.Table.IG2_TABLE_ID);

                if (tool != null)
                {


                    SEC_UI_TOOLBAR sut = new SEC_UI_TOOLBAR();
                    tool.CopyTo(sut, true);
                    sut.SEC_UI_ID = ui.SEC_UI_ID;
                    sut.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                    try
                    {
                        decipher.InsertModel(sut);
                    }
                    catch (Exception ex)
                    {
                        log.Error("插入权限工具栏数据出错了！", ex);
                        throw new Exception("插入权限工具栏数据出错了！", ex);
                    }

                    List<IG2_TOOLBAR_ITEM> toolItems = decipher.SelectModels<IG2_TOOLBAR_ITEM>(
                        "TABLE_ID={0} and IG2_TOOLBAR_ID={1} and ROW_SID >=0",
                        tSet.Table.IG2_TABLE_ID, tool.IG2_TOOLBAR_ID);


                    List<SEC_UI_TOOLBAR_ITEM> sutiList = new List<SEC_UI_TOOLBAR_ITEM>();

                    foreach (var item in toolItems)
                    {
                        SEC_UI_TOOLBAR_ITEM suti = new SEC_UI_TOOLBAR_ITEM();
                        item.CopyTo(suti, true);
                        suti.SEC_UI_TOOLBAR_ID = sut.SEC_UI_TOOLBAR_ID;
                        suti.SEC_UI_ID = ui.SEC_UI_ID;
                        suti.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;
                        sutiList.Add(suti);
                    }
                    try
                    {
                        decipher.InsertModels<SEC_UI_TOOLBAR_ITEM>(sutiList);

                    }
                    catch (Exception ex)
                    {
                        log.Error("插入权限工具栏条目出错了！", ex);
                        throw new Exception("插入权限工具栏条目出错了！", ex);
                    }

                }
                #endregion





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

        /// <summary>
        /// 同步
        /// </summary>
        public void Synchronous()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            List<SEC_LOGIN_ACCOUNT> slaList = decipher.SelectModels<SEC_LOGIN_ACCOUNT>("ROW_STATUS_ID >=0");
            int menuId = WebUtil.QueryInt("menuId");
            BIZ_C_MENU menu = decipher.SelectModelByPk<BIZ_C_MENU>(menuId);

            try
            {
                foreach (var sla in slaList)
                {
                    if (string.IsNullOrEmpty(sla.BIZ_USER_CODE) || string.IsNullOrEmpty(menu.SEC_PAGE_TYPE_ID))
                    {
                        continue;
                    }

                    LightModelFilter filter = new LightModelFilter(typeof(SEC_UI));
                    filter.And("SEC_MODE_ID", 2);
                    filter.And("MENU_ID", menuId);
                    filter.And("SEC_USER_CODE", sla.BIZ_USER_CODE);
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.TSqlOrderBy = "SEC_UI_ID desc";
                    SEC_UI secUI = decipher.SelectToOneModel<SEC_UI>(filter);
                    if (secUI == null)
                    {
                        continue;
                    }

                    if (menu.SEC_PAGE_TYPE_ID == "TABLE")
                    {
                        SynchronousTableTool(secUI, menu);
                    }
                    else if (menu.SEC_PAGE_TYPE_ID == "PAGE")
                    {
                        SynchronousPageTool(secUI, menu);

                    }


                    ///这是同步表的列数据
                    SynchronousPage(secUI, menu);

                }

                MessageBox.Alert("同步数据成功了！");

            }
            catch (Exception ex)
            {
                log.Error("同步失败了！", ex);
                MessageBox.Alert("同步失败了！");

            }


        }

        /// <summary>
        /// 同步一张表的工具栏数据
        /// </summary>
        void SynchronousTableTool(SEC_UI ui, BIZ_C_MENU menu)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, menu.SEC_PAGE_ID);

            IG2_TOOLBAR tool = decipher.SelectToOneModel<IG2_TOOLBAR>("TABLE_ID={0} and ROW_SID >=0", tSet.Table.IG2_TABLE_ID);
            SEC_UI_TOOLBAR sut = decipher.SelectToOneModel<SEC_UI_TOOLBAR>("SEC_UI_ID={0} and TABLE_ID={1} and ROW_SID >=0", ui.SEC_UI_ID, tSet.Table.IG2_TABLE_ID);
            if (tool == null || sut == null)
            {
                return;
            }

            List<IG2_TOOLBAR_ITEM> toolItems = decipher.SelectModels<IG2_TOOLBAR_ITEM>("TABLE_ID={0} and IG2_TOOLBAR_ID={1} and ROW_SID >=0", tSet.Table.IG2_TABLE_ID, tool.IG2_TOOLBAR_ID);

            List<SEC_UI_TOOLBAR_ITEM> sutiList = decipher.SelectModels<SEC_UI_TOOLBAR_ITEM>("SEC_UI_ID={0} and TABLE_ID={1} and ROW_SID >=0", ui.SEC_UI_ID, tSet.Table.IG2_TABLE_ID);

            ///这是新增的工具栏按钮
            List<IG2_TOOLBAR_ITEM> toolNew = new List<IG2_TOOLBAR_ITEM>();
            ///这是删除的工具栏按钮
            List<SEC_UI_TOOLBAR_ITEM> sutiDelete = new List<SEC_UI_TOOLBAR_ITEM>();


            foreach (var item in toolItems)
            {
                var suti = sutiList.Find(s => s.IG2_TOOLBAR_ITEM_ID == item.IG2_TOOLBAR_ITEM_ID);
                if (suti != null)
                {
                    continue;
                }
                toolNew.Add(item);

            }


            foreach (var item in sutiList)
            {
                var ti = toolItems.Find(t => t.IG2_TOOLBAR_ITEM_ID == item.IG2_TOOLBAR_ITEM_ID);
                if (ti != null)
                {

                    continue;
                }
                sutiDelete.Add(item);

            }

            try
            {

                ///这是删除工具栏
                foreach (var item in sutiDelete)
                {
                    item.ROW_SID = -3;
                    item.ROW_DATE_DELETE = DateTime.Now;
                    decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");
                }
            }
            catch (Exception ex)
            {
                log.Error("删除工具栏数据出错了！");
                throw new Exception("删除工具栏数据出错了！", ex);
            }

            try
            {

                ///这是新增工具栏
                foreach (var item in toolNew)
                {
                    SEC_UI_TOOLBAR_ITEM suti = new SEC_UI_TOOLBAR_ITEM();
                    item.CopyTo(suti, true);
                    suti.TABLE_ID = tSet.Table.IG2_TABLE_ID;
                    suti.SEC_UI_ID = ui.SEC_UI_ID;
                    suti.SEC_UI_TOOLBAR_ID = sut.SEC_UI_TOOLBAR_ID;
                    suti.SEC_UI_TABLE_ID = sut.SEC_UI_TABLE_ID;
                    decipher.InsertModel(suti);

                }


            }
            catch (Exception ex)
            {
                log.Error("新增工具栏数据出错了！", ex);
                throw new Exception("新增工具栏数据出错了！", ex);
            }


        }
        /// <summary>
        /// 这是同步多张表的工具栏数据
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="menu"></param>
        void SynchronousPageTool(SEC_UI ui, BIZ_C_MENU menu)
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


                IG2_TOOLBAR tool = decipher.SelectToOneModel<IG2_TOOLBAR>("TABLE_ID={0} and ROW_SID >=0", tSet.Table.IG2_TABLE_ID);
                SEC_UI_TOOLBAR sut = decipher.SelectToOneModel<SEC_UI_TOOLBAR>("SEC_UI_ID={0} and TABLE_ID={1} and ROW_SID >=0", ui.SEC_UI_ID, tSet.Table.IG2_TABLE_ID);
                if (tool == null || sut == null)
                {
                    return;
                }

                List<IG2_TOOLBAR_ITEM> toolItems = decipher.SelectModels<IG2_TOOLBAR_ITEM>("TABLE_ID={0} and IG2_TOOLBAR_ID={1} and ROW_SID >=0", tSet.Table.IG2_TABLE_ID, tool.IG2_TOOLBAR_ID);

                List<SEC_UI_TOOLBAR_ITEM> sutiList = decipher.SelectModels<SEC_UI_TOOLBAR_ITEM>("SEC_UI_ID={0} and TABLE_ID={1} and ROW_SID >=0", ui.SEC_UI_ID, tSet.Table.IG2_TABLE_ID);

                ///这是新增的工具栏按钮
                List<IG2_TOOLBAR_ITEM> toolNew = new List<IG2_TOOLBAR_ITEM>();
                ///这是删除的工具栏按钮
                List<SEC_UI_TOOLBAR_ITEM> sutiDelete = new List<SEC_UI_TOOLBAR_ITEM>();


                foreach (var item in toolItems)
                {
                    var suti = sutiList.Find(s => s.IG2_TOOLBAR_ITEM_ID == item.IG2_TOOLBAR_ITEM_ID);
                    if (suti != null)
                    {
                        continue;
                    }
                    toolNew.Add(item);

                }


                foreach (var item in sutiList)
                {
                    var ti = toolItems.Find(t => t.IG2_TOOLBAR_ITEM_ID == item.IG2_TOOLBAR_ITEM_ID);
                    if (ti != null)
                    {
                        continue;
                    }
                    sutiDelete.Add(item);

                }

                try
                {

                    ///这是删除工具栏
                    foreach (var item in sutiDelete)
                    {
                        item.ROW_SID = -3;
                        item.ROW_DATE_DELETE = DateTime.Now;
                        decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("删除工具栏数据出错了！");
                    throw new Exception("删除工具栏数据出错了！", ex);
                }

                try
                {

                    ///这是新增工具栏
                    foreach (var item in toolNew)
                    {
                        SEC_UI_TOOLBAR_ITEM suti = new SEC_UI_TOOLBAR_ITEM();
                        item.CopyTo(suti, true);
                        suti.TABLE_ID = tSet.Table.IG2_TABLE_ID;
                        suti.SEC_UI_ID = ui.SEC_UI_ID;
                        suti.SEC_UI_TOOLBAR_ID = sut.SEC_UI_TOOLBAR_ID;
                        suti.SEC_UI_TABLE_ID = sut.SEC_UI_TABLE_ID;

                        decipher.InsertModel(suti);

                    }


                }
                catch (Exception ex)
                {
                    log.Error("新增工具栏数据出错了！", ex);
                    throw new Exception("新增工具栏数据出错了！", ex);
                }


            }





        }

        /// <summary>
        /// 同步表的列数据
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="menu"></param>
        void SynchronousPage(SEC_UI ui, BIZ_C_MENU menu)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            List<SEC_UI_TABLE> sutList = decipher.SelectModels<SEC_UI_TABLE>("SEC_UI_ID={0} and ROW_SID >=0", ui.SEC_UI_ID);
            if (sutList.Count == 0)
            {
                return;
            }
            foreach (var sut in sutList)
            {
                IG2_TABLE it = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID = 'TABLE' and TABLE_NAME='{0}' and ROW_SID >=0", sut.TABLE_NAME);
                if (it == null)
                {
                    return;
                }

                List<SEC_UI_TABLECOL> sutcList = decipher.SelectModels<SEC_UI_TABLECOL>("SEC_UI_ID={0} and SEC_UI_TABLE_ID={1} and ROW_SID >=0", ui.SEC_UI_ID, sut.SEC_UI_TABLE_ID);
                List<IG2_TABLE_COL> itcList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID = {0} and ROW_SID >=0", it.IG2_TABLE_ID);

                ///这是表新增的列数据
                List<IG2_TABLE_COL> itcNew = new List<IG2_TABLE_COL>();
                ///这是表删除的列数据
                List<SEC_UI_TABLECOL> sutDelete = new List<SEC_UI_TABLECOL>();

                foreach (var item in itcList)
                {
                    var itcc = sutcList.Find(s => s.DB_FIELD == item.DB_FIELD);
                    if (itcc != null) { continue; }
                    itcNew.Add(item);

                }

                foreach (var item in sutcList)
                {
                    var sutcc = itcList.Find(i => i.DB_FIELD == item.DB_FIELD);
                    if (sutcc != null) { continue; }
                    sutDelete.Add(item);
                }

                try
                {

                    foreach (var item in sutDelete)
                    {
                        item.ROW_SID = -3;
                        item.ROW_DATE_DELETE = DateTime.Now;
                        decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("删除表的列数据出错了！", ex);
                    throw new Exception("删除表的列数据出错了！", ex);
                }

                try
                {

                    foreach (var item in itcNew)
                    {
                        SEC_UI_TABLECOL sutNew = new SEC_UI_TABLECOL();
                        item.CopyTo(sutNew, true);
                        sutNew.FIELD_TEXT = item.DISPLAY;
                        sutNew.SEC_UI_ID = ui.SEC_UI_ID;
                        sutNew.SEC_UI_TABLE_ID = sut.SEC_UI_TABLE_ID;
                        decipher.InsertModel(sutNew);
                    }



                }
                catch (Exception ex)
                {
                    log.Error("插入表的列数据出错了！", ex);
                    throw new Exception("插入表的列数据出错了！");
                }
            }
        }

    }
}