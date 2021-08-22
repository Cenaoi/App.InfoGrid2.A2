using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml;


namespace App.InfoGrid2.View.OneForm
{
    public partial class FormPreview : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInitCustomControls(EventArgs e)
        {

            //#region 命中页面，避免自循环

            //HitPageCollection hitPages = this.Session["HIT_PAGES"] as HitPageCollection;

            //if (hitPages == null)
            //{
            //    hitPages = new HitPageCollection();

            //    this.Session["HIT_PAGES"] = hitPages;
            //}

            //if (this.Context.Items["EC_ViewUri"] != null)
            //{
            //    string viewUri = (string)this.Context.Items["EC_ViewUri"];

            //    hitPages.HitPage(viewUri);

            //    if (hitPages.IsEndlessLoop(viewUri))
            //    {
            //        this.Context.Items["EC_ENDLESS_LOOP"] = true;

            //        hitPages.Clear(viewUri);

            //        return;
            //    }
            //}

            //#endregion


            int pageId = WebUtil.QueryInt("pageid");

            m_SecTag = WebUtil.Query("sec_tag");
            m_MenuId = WebUtil.QueryInt("menu_id");

            #region 权限

            m_SecUiFty = new M2SecurityUiFactory();
            m_SecUiFty.InitSecUI(pageId, "PAGE", m_MenuId, null);

            m_SecDataFty = new M2SecurityDataFactory();

            #endregion

            try
            {
                InitUI();
            }
            catch (Exception ex)
            {
                throw new Exception("初始化界面失败。", ex);
            }

            try
            {
                if (m_CustomPage != null)
                {
                    m_CustomPage.OnProInit();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("执行自定义类的 OnInit 代码错误", ex);
            }


        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_CustomPage != null)
                {
                    m_CustomPage.OnProLoad();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("执行自定义类的 OnLoad 代码错误", ex);
            }



            if (!this.IsPostBack)
            {
                if (m_MainStore != null)
                {
                    m_MainStore.DataBind();
                }
            }
        }


        public void GoRefresh()
        {
            m_MainStore.Refresh();
        }

        /// <summary>
        /// 页面实体
        /// </summary>
        IG2_TABLE m_PageModel;

        /// <summary>
        /// 界面权限
        /// </summary>
        M2SecurityUiFactory m_SecUiFty;

        /// <summary>
        /// 数据权限
        /// </summary>
        M2SecurityDataFactory m_SecDataFty;

        /// <summary>
        /// 查询
        /// </summary>
        Control m_MainSearch = null;

        /// <summary>
        /// 主表
        /// </summary>
        TableSet m_MainTs = null;

        Store m_MainStore = null;

        Table m_MainTable = null;

        Toolbar m_MainToolbar = null;


        /// <summary>
        /// （注：临时变量，以后整理后撤销）
        /// 关联主表的字段名
        /// </summary>
        string m_JoinMainField;

        /// <summary>
        /// 
        /// </summary>
        SortedList<string, string> m_JoinMainFields = new SortedList<string, string>();


        /// <summary>
        /// 数据集
        /// </summary>
        SortedList<string, Store> m_StoreSet = new SortedList<string, Store>();

        List<Store> m_StoreList = new List<Store>();

        /// <summary>
        /// 用户创建的控件集合
        /// </summary>
        List<Control> m_UserControls = new List<Control>();

        string m_SecTag;

        int m_MenuId = 0;

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        private string ToString(LModelFieldElementCollection fields)
        {
            StringBuilder sb = new StringBuilder();

            int n = 0;

            foreach (LModelFieldElement item in fields)
            {
                if (n++ > 0) { sb.Append(","); }
                sb.Append(item.DBField);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 按数据表明获取数据仓库
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private Store GetStoreByTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName", "表名不能为空,获取数据仓库 Store 失败.");
            }



            tableName = tableName.ToUpper();

            Store store = null;

            lock (m_StoreSet)
            {
                if (!m_StoreSet.TryGetValue(tableName, out store))
                {
                    LModelElement modelElem = LModelDna.GetElementByName(tableName);

                    store = new Store();
                    store.ID = "Store_" + tableName;
                    store.IdField = modelElem.PrimaryKey;
                    store.Model = tableName;
                    store.StringFields = ToString(modelElem.Fields);

                    store.Updating += store_Updating;

                    store.DeleteRecycle = true;



                    //添加进集合
                    m_StoreSet.Add(tableName, store);
                    m_StoreList.Add(store);

                    this.StoreSet.Controls.Add(store);


                    m_SecDataFty.BindStore(store);  //数据权限绑定

                    //M2ValidateFactory validFty = new M2ValidateFactory();
                    //validFty.BindStore(store);


                    //LCodeFactory lcFty = new LCodeFactory();
                    //lcFty.BindStore(store);

                    //LCodeValueFactory lcvFty = new LCodeValueFactory();
                    //lcvFty.BindStore(store);

                    //DbCascadeFactory dbccFty = new DbCascadeFactory();
                    //dbccFty.ExecEnd += dbccFactory_ExecEnd;
                    //dbccFty.BindStore(store);

                    DbCascadeRule.Bind(store);
                }
            }


            return store;
        }

        void store_Updating(object sender, ObjectCancelEventArgs e)
        {
            BizHelper.FullForUpdate(e.Object as LModel);
        }

        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            EC5.BizLogger.LogStepMgr.Insert(e.Steps[0], e.OpText, e.Remark);
        }

        ExPage m_CustomPage;

        /// <summary>
        /// 初始化自定义服务器代码
        /// </summary>
        private void InitCustomPage(string serClass)
        {
            if (StringUtil.IsBlank(serClass))
            {
                return;
            }

            serClass = serClass.Trim();

            Type customPageT = Type.GetType(serClass);

            if (customPageT == null)
            {
                log.ErrorFormat("自定义类“{0}”没有找到", serClass);
                return;
            }

            m_CustomPage = Activator.CreateInstance(customPageT) as ExPage;

            if (m_CustomPage == null)
            {
                log.ErrorFormat("自定义类“{0}”没法实例化。", serClass);
                return;
            }

            m_CustomPage.SetDefaultValue(this, m_MainSearch, m_MainStore, m_MainToolbar, m_MainTable);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";
            m_CustomPage.PageType = "FORM";

            this.viewport1.Controls.Add(m_CustomPage);
        }


        private void InitUI()
        {

            int pageId = WebUtil.QueryInt("pageId",33); //表单结构的 id



            int rowPk = WebUtil.QueryInt("row_pk");  //行主键





            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = m_PageModel = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (model == null)
            {
                throw new Exception(string.Format("复杂表页面的数据 IG2_TABLE 不存在, IG2_TABLE_ID={0}", pageId));
            }


            HeadPanel.Visible = model.IS_BIG_TITLE_VISIBLE;

            string alias_title = WebUtil.Query("alias_title");

            this.headLab.Value = "<span class='page-head' >" + StringUtil.NoBlank(alias_title, model.DISPLAY) + "</span>";

            //this.viewport1.MarginTop = model.IS_BIG_TITLE_VISIBLE ? 40 : 0;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(model.PAGE_TEMPLATE);
            }
            catch
            {
                throw new Exception(string.Format("加载复杂表模板错误. IG2_TABLE_ID={0}", pageId));
            }

            XmlNode root = doc.DocumentElement;

            XmlNode headNode = root.SelectSingleNode("head");

            XmlNode bodyNode = root.SelectSingleNode("body");



            m_UserControls.Add(this.viewport1);


            //Panel panel = new Panel();
            //panel.ID = "panelPage" + model.IG2_TABLE_ID;
            //panel.Dock = DockStyle.Full;
            //panel.Region = RegionType.Center;
            //panel.Layout = LayoutStyle.Auto;
            //panel.Scroll = ScrollBars.None;

            //m_UserControls.Add(panel);

            foreach (XmlNode cNode in bodyNode.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(cNode, "ec-type").ToUpper();
                string title = XmlUtil.GetAttrValue(cNode, "title");
                string id = XmlUtil.GetAttrValue(cNode, "id");

                string ecMainView = XmlUtil.GetAttrValue(cNode, "ec-main-view");

                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("这个节点 id 不能为空。");
                }

                Control con = FindControl(this.viewport1, id);

                if (ecType == "FORM")
                {

                    InitType_Form(this.viewport1, id, ecMainView, con);

                }
                else if (ecType == "BOX")
                {
                    InitType_Table(this.viewport1, id, ecMainView, ecType);

                }
                else if (ecType == "TABS")
                {
                    InitType_Tabs(this.viewport1, cNode, id, ecMainView, con);
                }

            }

            //this.viewport1.Controls.Add(panel);

            //InitCustomPage(model.SERVER_CLASS);

        }

        private void InitType_Tabs(Control parent, XmlNode node, string id, string ecMainView, Control defaultControl)
        {
            if (!node.HasChildNodes)
            {
                return;
            }

            TabPanel tabPanel;

            if (defaultControl != null)
            {
                tabPanel = defaultControl as TabPanel;
            }
            else {
                tabPanel = new TabPanel();
                tabPanel.Dock = DockStyle.Bottom;
                tabPanel.Region = RegionType.South;
                tabPanel.Height = 300;
                tabPanel.ID = "tabs" + (m_Identity++);
                tabPanel.Plain = true;

                parent.Controls.Add(tabPanel);
            }

            m_UserControls.Add(tabPanel);

            foreach (XmlNode cNode in node.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(cNode, "ec-type").ToUpper();
                string title = XmlUtil.GetAttrValue(cNode, "title");
                string cId = XmlUtil.GetAttrValue(cNode, "id");

                string ecMainView2 = XmlUtil.GetAttrValue(cNode, "ec-main-view");

                Tab tab = new Tab();
                tab.ID = cId;
                tab.Text = title;
                tab.Scroll = ScrollBars.None;

                tabPanel.Items.Add(tab);

                m_UserControls.Add(tab);

                if (ecType == "TAB")
                {
                    InitType_Tab(tab, cId, ecMainView2, ecType);
                }

            }


        }



        int m_Identity = 0;

        /// <summary>
        /// 仓库关联仓库
        /// </summary>
        /// <param name="mainStore"></param>
        /// <param name="subStore"></param>
        /// <param name="mainField"></param>
        /// <param name="subField"></param>
        private void StoreJoinStore(Store mainStore, Store subStore, string mainField, string subField)
        {
            //mainStore.CurrentChanged += delegate(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
            //{
            //    subStore.DataBind();
            //};

            StoreCurrentParam param = new StoreCurrentParam(subField, mainStore.ID, mainField);

            subStore.FilterParams.Add(param);

            StoreCurrentParam param2 = new StoreCurrentParam(subField, mainStore.ID, mainField);
            subStore.InsertParams.Add(param2);


        }

        private void InitType_Tab(Tab parent, string id, string ecMainView, string ecType)
        {
            int itemViewID = StringUtil.ToInt(ecMainView, -1);

            if (itemViewID == -1)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, itemViewID);

            ToolbarSet tbSet = new ToolbarSet();
            tbSet.SelectForTable(decipher, itemViewID);

            IG2_TABLE view = tSet.Table;



            string tableId = "tableT" + view.IG2_TABLE_ID;

            if (!StringUtil.IsBlank(view.TAB_TEXT))
            {
                parent.Text = view.TAB_TEXT;
            }

            Store store = GetStoreByTable(view.TABLE_NAME);

            store.Tag = view.IG2_TABLE_ID;

            if (!StringUtil.IsBlank(view.ME_COL_NAME, view.JOIN_TAB_NAME, view.JOIN_COL_NAME))
            {
                Store joinStore = GetStoreByTable(view.JOIN_TAB_NAME);

                StoreJoinStore(joinStore, store, view.JOIN_COL_NAME, view.ME_COL_NAME);

                if (!m_JoinMainFields.ContainsKey(view.TABLE_NAME))
                {
                    m_JoinMainFields.Add(view.TABLE_NAME, view.ME_COL_NAME);
                }

                //if (StringUtil.IsBlank(m_JoinMainField))
                //{
                //    m_JoinMainField = view.ME_COL_NAME;
                //}
            }

            if (!m_UserControls.Contains(store))
            {
                m_UserControls.Add(store);
            }

            Toolbar toolbar;
            //创建工具栏 + 表格
            CreateBoxUI(parent, store, tSet, tbSet, tableId, ecType, id, out toolbar);
        }



        private void InitType_Table(Control parent, string id, string ecMainView, string ecType)
        {
            int itemViewID = StringUtil.ToInt(ecMainView, -1);

            if (itemViewID == -1)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, itemViewID);


            ToolbarSet tbSet = new ToolbarSet();
            tbSet.SelectForTable(decipher, itemViewID);

            IG2_TABLE view = tSet.Table;

            string tableId = "tableT" + view.IG2_TABLE_ID;

            Store store = GetStoreByTable(view.TABLE_NAME);

            store.Tag = tSet.Table.IG2_TABLE_ID;


            if (m_MainStore == null)
            {
                m_MainStore = store;
                m_MainStore.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(m_MainStore_CurrentChanged);


                m_MainTs = tSet;

            }

            if (!StringUtil.IsBlank(view.LOCKED_FIELD) && StringUtil.IsBlank(store.LockedField))
            {
                store.LockedField = view.LOCKED_FIELD;
            }

            Toolbar toolbar;

            Control con = CreateBoxUI(parent, store, tSet, tbSet, tableId, ecType, id, out toolbar);


            if (m_MainToolbar == null && toolbar != null)
            {
                m_MainToolbar = toolbar;
            }

            if (m_MainTable == null && (con is Table))
            {
                m_MainTable = con as Table;
            }


            if (!m_UserControls.Contains(store))
            {
                m_UserControls.Add(store);
            }

            m_UserControls.Add(con);
        }

        /// <summary>
        /// 创建工具栏 + 表格
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="store"></param>
        /// <param name="tSet"></param>
        /// <param name="tbSet"></param>
        /// <param name="tableId"></param>
        /// <param name="ecType"></param>
        /// <param name="ecId"></param>
        /// <returns></returns>
        private Control CreateBoxUI(Control parent, Store store, TableSet tSet, ToolbarSet tbSet, string tableId,
            string ecType, string ecId, out Toolbar toolbar)
        {
            IG2_TABLE view = tSet.Table;

            if (view.UI_TYPE_ID == "FORM")
            {

                FormLayout form = new FormLayout();
                form.ID = "formT" + view.IG2_TABLE_ID;
                form.Dock = DockStyle.Full;
                form.StoreID = store.ID;
                form.Padding = 10;
                form.ItemWidth = 300;
                form.ItemLabelAlign = TextAlign.Right;



                M2FormFactory fromFactory = new M2FormFactory(this.IsPostBack);
                fromFactory.CreateFormControls(form, tSet);

                parent.Controls.Add(form);

                m_UserControls.Add(form);

                toolbar = null;

                return form;
            }
            else
            {

                ControlParam conParam = new ControlParam(store.IdField, tableId, "CheckedRows");

                store.DeleteQuery.Add(conParam);


                LModelElement modelElem = LightModel.GetLModelElement(store.Model);

                if (modelElem.Fields.ContainsField("BIZ_SID"))
                {
                    store.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(string.Format(
                        "(BIZ_SID is null OR BIZ_SID <= {0})", view.DELETE_BY_BIZ_SID)));
                }



                #region [Store] 删除进回收站模式

                store.FilterParams.Add(new Param("ROW_SID", "0", DbType.Int32) { Logic = ">=" }); ;

                Param drP1 = new Param("ROW_SID", "-3", System.Data.DbType.Int32);
                ServerParam drP2 = new ServerParam("ROW_DATE_DELETE", "TIME_NOW");

                store.DeleteRecycleParams.Add(drP1);
                store.DeleteRecycleParams.Add(drP2);

                #endregion

                store.DefaultInsertPos = EnumUtil.Parse<InsertPosition>(view.INSERT_POS, InsertPosition.Auto, true);

                if (!StringUtil.IsBlank(view.SORT_TEXT))
                {
                    store.SortText = view.SORT_TEXT;
                }



                Toolbar toolbar1 = new Toolbar();
                toolbar1.ID = "toolbarT" + view.IG2_TABLE_ID;


                Table table1 = new Table();
                table1.ID = tableId;
                table1.StoreID = store.ID;
                table1.RowLines = true;
                table1.ColumnLines = true;
                table1.Dock = DockStyle.Full;
                table1.Region = RegionType.Center;
                table1.Sortable = true;
                table1.SummaryVisible = view.SUMMARY_VISIBLE;
                table1.RowStyleEnabled = view.VALID_MSG_ENABLED;

                M2ToolbarFactory toolbarFactory = new M2ToolbarFactory();
                toolbarFactory.StoreId = store.ID;
                toolbarFactory.TableId = tableId;
                toolbarFactory.SearchFormID = m_MainSearch?.ID;
                toolbarFactory.CreateItems(toolbar1, tbSet);





                M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);
                tableFactory.CreateTableColumns(table1, store, tSet);



                m_SecUiFty.Filter(ecType, m_SecTag, ecId, toolbar1, table1, store);

                m_SecDataFty.BindCatalog(store, tSet);


                parent.Controls.Add(toolbar1);
                parent.Controls.Add(table1);

                if (!m_UserControls.Contains(store))
                {
                    m_UserControls.Add(store);
                }
                m_UserControls.Add(toolbar1);
                m_UserControls.Add(table1);


                //设置数据仓库默认值
                SetStoreDefaultValue(store, tSet);

                toolbar = toolbar1;

                return table1;
            }


        }

        /// <summary>
        /// 设置数据仓库默认值
        /// </summary>
        /// <param name="storeUi"></param>
        /// <param name="tSet"></param>
        private void SetStoreDefaultValue(Store storeUi, TableSet tSet)
        {
            //LModelElement modelElem = LightModel.GetLModelElement(tSet.Table.TABLE_NAME);

            foreach (IG2_TABLE_COL col in tSet.Cols)
            {
                if (StringUtil.IsBlank(col.DEFAULT_VALUE) || col.ROW_SID < 0)
                {
                    continue;
                }

                //LModelFieldElement fieldElem = modelElem.Fields[col.DB_FIELD];


                Param param = new Param(col.DB_FIELD, col.DEFAULT_VALUE);

                storeUi.InsertParams.Add(param);

            }
        }



        void m_MainStore_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            Store store = (Store)sender;

            bool isLockSub = false;

            LModel model = e.Object as LModel;

            if (model != null && !StringUtil.IsBlank(store.LockedField))
            {
                object value = model[store.LockedField];

                isLockSub = BoolUtil.ToBool(value);
            }


            foreach (Store subStore in m_StoreList)
            {
                if (subStore.Equals(sender))
                {
                    continue;
                }

                subStore.DataBind();

                subStore.ReadOnly = isLockSub;
            }


        }

        /// <summary>
        /// 初始化查询类型
        /// </summary>
        private void InitType_Form(Control parent, string id, string ecMainView, Control defaultControl)
        {
            int itemViewID = StringUtil.ToInt(ecMainView, -1);

            if (itemViewID == -1)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectByPk(decipher, itemViewID);



            IG2_TABLE view = tSet.Table;



            Store store = GetStoreByTable(view.TABLE_NAME);

            if (m_MainStore == null)
            {
                int rowPk = WebUtil.QueryInt("row_pk");

                store.FilterParams.Add(new Param("ROW_IDENTITY_ID", rowPk.ToString()));

                m_MainStore = store;

                m_MainStore.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(m_MainStore_CurrentChanged);

                m_MainTs = tSet;

            }

            if (!StringUtil.IsBlank(view.LOCKED_FIELD) && StringUtil.IsBlank(store.LockedField))
            {
                store.LockedField = view.LOCKED_FIELD;
            }


            FormLayout form1;

            if (defaultControl != null)
            {
                form1 = defaultControl as FormLayout;
                form1.StoreID = store.ID;
            }
            else
            {
                form1 = new FormLayout();
                form1.ID = "form" + view.IG2_TABLE_ID;

                form1.Dock = DockStyle.Top;
                form1.Region = RegionType.North;
                form1.FlowDirection = FlowDirection.LeftToRight;
                form1.ItemWidth = 300;
                form1.ItemLabelAlign = TextAlign.Right;
                form1.ItemClass = "mi-box-item";
                form1.Layout = LayoutStyle.HBox;
                form1.StoreID = store.ID;
                form1.AutoSize = true;
                form1.Scroll = ScrollBars.None;


                if (!this.IsPostBack)
                {
                    form1.Visible = false;
                }

                parent.Controls.Add(form1);
            }


            if (m_MainSearch == null)
            {
                m_MainSearch = form1;
            }


            if (!m_UserControls.Contains(store))
            {
                m_UserControls.Add(store);
            }
            m_UserControls.Add(form1);


            M2FormFactory formFty = new M2FormFactory(this.IsPostBack);


            //TableSet newTSet = m_SecUiFty.FilterTableSet("FORM", m_SecTag, "HanderForm1", tSet);

            //if (newTSet != null)
            //{
            //    tSet = newTSet;
            //}

            formFty.CreateFormControls(form1, tSet);




        }

    }
}