using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EasyClick.BizWeb2;
using EasyClick.BizWeb2.Hit;
using EasyClick.Web.Mini2;
using EC5;
using EC5.DbCascade;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.IG2.Plugin;
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml;


namespace App.InfoGrid2.View.OneBuilder
{
    public partial class PreviewPage : WidgetControl, IView
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

        #region 主表

        /// <summary>
        /// 主表(数据结构)
        /// </summary>
        TableSet m_MainTs = null;

        Store m_MainStore = null;

        Table m_MainTable = null;

        Toolbar m_MainToolbar = null;

        #endregion

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
        /// 数据集, 按表名做索引
        /// </summary>
        SortedList<string, Store> m_StoreSet = new SortedList<string, Store>();

        /// <summary>
        /// 按照表 ID 作为索引
        /// </summary>
        SortedList<string, Store> m_StoreIdSet = new SortedList<string, Store>();

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
        /// <param name="isShareData">共享同一个数据表的数据</param>
        /// <param name="tableName"></param>
        /// <param name="instStoreId">特殊数据仓库, 独立存在的</param>
        /// <returns></returns>
        private Store GetStoreByTable(bool isShareData, string tableName, string instStoreId)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName", "表名不能为空,获取数据仓库 Store 失败.");
            }

            if(!isShareData && StringUtil.IsBlank(instStoreId))
            {
                throw new ArgumentNullException("不共享表数据, 必须提供实例 TableID 的id");
            }

               //单例的数据参数, 特殊类型...独立存在.
            string newStoreId;

            if (isShareData)
            {
                newStoreId = $"Store_{tableName}";
            }
            else
            {
                newStoreId = $"Store_{tableName}__{instStoreId}";
            }

            tableName = tableName.ToUpper();

            Store store = null;

            lock (m_StoreList)
            {
                if (isShareData && m_StoreSet.TryGetValue(tableName, out store))
                {
                    return store;
                }
                else if (!isShareData)
                {
                    if (m_StoreIdSet.TryGetValue(newStoreId, out store))
                    {
                        return store;
                    }
                }

                LModelElement modelElem = LModelDna.GetElementByName(tableName);

                store = new Store();
                store.ID = newStoreId;// "Store_" + tableName;
                store.IdField = modelElem.PrimaryKey;
                store.Model = tableName;
                store.StringFields = ToString(modelElem.Fields);

                store.Updating += store_Updating;


                store.DeleteRecycle = true;



                //添加进集合
                if (isShareData)
                {
                    m_StoreSet.Add(tableName, store);
                }
                else
                {
                    m_StoreIdSet.Add(newStoreId, store);
                }

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


            return store;
        }

        void store_Updating(object sender, ObjectCancelEventArgs e)
        {
            BizHelper.FullForUpdate(e.Object as LModel);
        }

        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            EC5.BizLogger.LogStepMgr.Insert(e.Steps[0], e.OpText,e.Remark);
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

            m_CustomPage.SetDefaultValue(this, m_MainSearch, m_MainStore,m_MainToolbar, m_MainTable);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";

             this.viewport1.Controls.Add(m_CustomPage);
        }
        

        private void InitUI()
        {

            int pageId = WebUtil.QueryInt("pageId");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = m_PageModel = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (model == null)
            {
                throw new Exception(string.Format("复杂表页面的数据 IG2_TABLE 不存在, IG2_TABLE_ID={0}", pageId));
            }


            HeadPanel.Visible = model.IS_BIG_TITLE_VISIBLE;

            string alias_title = WebUtil.Query("alias_title");

            this.headLab.Value = "<span class='page-head' >" + StringUtil.NoBlank(alias_title, model.DISPLAY) + "</span>";

            this.viewport1.MarginTop = model.IS_BIG_TITLE_VISIBLE ? 40 : 0;

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



            Panel panel = new Panel();
            panel.ID = "panelPage" + model.IG2_TABLE_ID;
            panel.Dock = DockStyle.Full;
            panel.Region = RegionType.Center;
            panel.Layout = LayoutStyle.Auto;
            panel.Scroll = ScrollBars.None;

            m_UserControls.Add(this.viewport1);
            m_UserControls.Add(panel);

            foreach (XmlNode cNode in bodyNode.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(cNode, "ec-type").ToUpper();
                string title = XmlUtil.GetAttrValue(cNode, "title");
                string id = XmlUtil.GetAttrValue(cNode, "id");

                string ecMainView = XmlUtil.GetAttrValue(cNode, "ec-main-view");


                if (ecType == "SEARCH")
                {
                    
                    InitType_Search(panel, id, ecMainView);

                }
                else if (ecType == "BOX")
                {
                    InitType_Table(panel, id, ecMainView, ecType);

                }
                else if (ecType == "TABS")
                {
                    InitType_Tabs(this.viewport1,cNode,  id, ecMainView);
                }

            }

            this.viewport1.Controls.Add(panel);

            InitCustomPage(model.SERVER_CLASS);
            
        }

        private void InitType_Tabs(Control parent,XmlNode node, string id, string ecMainView)
        {
            if (!node.HasChildNodes)
            {
                return;
            }

            int tabHeight = GlobelParam.GetValue<int>("BUID_PAGE/TAB_HEIGHT", 300, "自定义页面(下方)的 Tab 标签高度");

            if(tabHeight <= 0)
            {
                tabHeight = 300;
            }

            TabPanel tabPanel = new TabPanel();
            tabPanel.TabLeft = 8;
            tabPanel.Dock = DockStyle.Bottom;
            tabPanel.Region = RegionType.South;
            tabPanel.Height = tabHeight;
            tabPanel.ID = "tabs"+ (m_Identity++);
            tabPanel.Plain = true;
            tabPanel.UI = "win10";

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

            parent.Controls.Add(tabPanel);

            m_UserControls.Add(tabPanel);
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

        /// <summary>
        /// 初始化 Tab 控件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="ecMainView"></param>
        /// <param name="ecType"></param>
        private void InitType_Tab(Tab parent, string id, string ecMainView,string ecType)
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

            Store store;

            if (view.IS_SHARE_DATA)
            {
                store = GetStoreByTable(view.IS_SHARE_DATA, view.TABLE_NAME, null);
            }
            else
            {
                store = GetStoreByTable(view.IS_SHARE_DATA, view.TABLE_NAME, view.IG2_TABLE_ID.ToString());
            }


            store.Tag = view.IG2_TABLE_ID;

            if (!StringUtil.IsBlank(view.ME_COL_NAME, view.JOIN_TAB_NAME,view.JOIN_COL_NAME))
            {
                Store joinStore = GetStoreByTable(true, view.JOIN_TAB_NAME, null);  //一般获取主表

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

            //创建工具栏 + 表格
            CreateBoxUI(parent, store, tSet, tbSet, tableId, ecType, id, view.IS_SHARE_DATA, out Toolbar toolbar);
        }


        /// <summary>
        /// 主表
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="ecMainView"></param>
        /// <param name="ecType"></param>
        private void InitType_Table(Control parent, string id, string ecMainView,string ecType)
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

            Store store = GetStoreByTable(true, view.TABLE_NAME, null);

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

            Control con = CreateBoxUI(parent, store, tSet, tbSet, tableId, ecType, id,false, out toolbar);


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
        /// <param name="isShareData">共享同一个表的数据</param>
        /// <param name="ecId"></param>
        /// <returns></returns>
        private Control CreateBoxUI(Control parent, Store store, TableSet tSet, ToolbarSet tbSet,string tableId,
            string ecType,string ecId,bool isShareData, out Toolbar toolbar)
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
                toolbarFactory.SearchFormID = (m_MainSearch != null ? m_MainSearch.ID : "");
                toolbarFactory.CreateItems(toolbar1, tbSet);





                M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);
                tableFactory.CreateTableColumns(table1,store, tSet);

                

                m_SecUiFty.Filter(ecType, m_SecTag, ecId,toolbar1, table1, store);

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
        private void InitType_Search(Control parent, string id, string ecMainView)
        {
            int itemViewID = StringUtil.ToInt(ecMainView, -1);

            if (itemViewID == -1)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectByPk(decipher, itemViewID);

            

            IG2_TABLE view = tSet.Table;



            Store store = GetStoreByTable(true, view.TABLE_NAME,null);

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

            FormLayout search1 = new FormLayout();
            search1.ID = "serach" + view.IG2_TABLE_ID;

            search1.Dock = DockStyle.Top;
            search1.Region = RegionType.Center;
            search1.FlowDirection = FlowDirection.TopDown;
            search1.ItemWidth = 300;
            search1.ItemLabelAlign = TextAlign.Right;
            search1.ItemClass = "mi-box-item";
            search1.Layout = LayoutStyle.HBox;
            search1.StoreID = store.ID;
            search1.FormMode = FormLayoutMode.Filter;
            search1.Scroll = ScrollBars.None;


            if (!this.IsPostBack)
            {
                search1.Visible = false;
            }

            parent.Controls.Add(search1);



            M2SearchFormFactory searchFactory = new M2SearchFormFactory(this.IsPostBack,this.m_MainStore.ID);


            TableSet newTSet = m_SecUiFty.FilterTableSet("SEARCH", m_SecTag, "search1", tSet);

            if (newTSet != null)
            {
                tSet = newTSet;
            }

            searchFactory.CreateControls(search1, tSet);

            if (m_MainSearch == null)
            {
                m_MainSearch = search1;
            }


            if (!m_UserControls.Contains(store))
            {
                m_UserControls.Add(store);
            }
            m_UserControls.Add(search1);
        }


        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);


            //当前页面的 js 

            if (m_MainStore != null)
            {
                if (m_MainTable == null)
                {
                    throw new Exception("输出脚本错误 MainTable=null ，主表不存在。");
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("$(document).ready(function(){");
                sb.AppendLine("  window.curPage = {");
                sb.AppendLine("    mainStore : " + m_MainStore.ClientID + ",");
                sb.AppendLine("    mainTable : " + m_MainTable.ClientID + ",");
                sb.AppendLine("    mainModel : '" + m_MainStore.Model + "'");
                sb.AppendLine("  };");


                sb.AppendLine("});");

                EasyClick.Web.Mini2.ScriptManager script = EasyClick.Web.Mini2.ScriptManager.GetManager(this.Page);

                script.AddScript(sb);
                

            }


        }


        


        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        public void ChangeBizSID(string psStr)
        {
            string[] sp = StringUtil.Split(psStr, ",");
            

            Table tab = m_MainTable;
            Store store = m_MainStore;

            if (sp.Length == 4)
            {
                string tableId = sp[2];
                string storeId = sp[3];
                
                tab = FindControl(this.viewport1,tableId) as Table;
                store = FindControl(this.StoreSet, storeId) as Store;

                psStr = $"{sp[0]},{sp[1]}";
            }



            try
            {
                ResultBase result = BizHelper.ChangeBizSID(psStr, tab, store);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }

        }




        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="paramStr"></param>
        public void ChangeField(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeField(paramStr, m_MainTable, m_MainStore);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }
        }


        private SortedList<string, object> ConvertToResult(string jsonPs)
        {
            jsonPs = jsonPs.Trim();

            if (!StringUtil.StartsWith(jsonPs, "{") && !StringUtil.EndsWith(jsonPs, "}"))
            {
                jsonPs = "{" + jsonPs + "}";
            }

            JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonPs);

            SortedList<string, object> result = new SortedList<string, object>();

            foreach (var prop in jObj.Properties())
            {
                JValue jv = (JValue)prop.Value;

                //JTokenType jType = jv.Type;

                result[prop.Name] = jv.Value;               
            }

            return result;
        }


        /// <summary>
        /// 主表数据 输出到打印机
        /// </summary>
        /// <param name="paramJson"></param>
        public void ToPrintMain(string paramJson)
        {


            int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
            int pageId = WebUtil.QueryInt("pageId");

           
            string mainTable = m_MainStore.Model;
            string mainPk = m_MainStore.IdField;


            string url = "/App/InfoGrid2/View/PrintTemplate/PrintTemplateSingleTable.aspx";

            NameValueCollection nv = new NameValueCollection(){
                {"id",m_MainTs.Table.IG2_TABLE_ID.ToString()},
                {"mainTableID",pkValue.ToString()},  //主数据记录的ID
                {"pageID",m_MainTs.Table.IG2_TABLE_ID.ToString()},

                {"mainTable",mainTable},    //表名
                {"mainPK",mainPk},

            };

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nv.Count; i++)
            {
                string key = nv.Keys[i];
                string value = nv[i];

                if (i > 0) { sb.Append("&"); }

                sb.Append(key).Append("=").Append(value);
            }



            string urlStr = url + "?" + sb.ToString();

            Window win = new Window("打印");
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.ContentPath = urlStr;
            win.FormClosedForJS = string.Concat(
                "function(e)",
                "{",
                "if (e.result != 'ok'){ return; }",
                "widget1.submit('form:first', {action: 'btnPrint',actionPs:e.ids});",
                "}");


            win.ShowDialog();



        }

        /// <summary>
        /// 小渔夫写的 
        /// 根据选择的打印机，和打印模板生成打印文件输出到打印机中
        /// </summary>
        /// <param name="ids">打印机ID | 模板ID </param>
        public void btnPrint(string ids)
        {

            string[] idList = StringUtil.Split(ids, "|");

            if (idList.Length < 2)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            BIZ_PRINT bp = decipher.SelectModelByPk<BIZ_PRINT>(idList[0]);

            BIZ_PRINT_TEMPLATE bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(idList[1]);


            if (bp == null || bpt == null)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            string pathUrl = string.Empty;

            try
            {

                pathUrl = CreateExcelData(bpt.TEMPLATE_URL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("打印出错了！" + ex.Message);
                return;
            }

            try
            {

                BIZ_PRINT_FILE bpf = new BIZ_PRINT_FILE()
                {
                    FILE_URL = pathUrl,
                    PRINT_CODE = bp.PRINT_CODE,
                    PRINT_NAME = bp.PRINT_TEXT,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                decipher.InsertModel(bpf);

            }
            catch (Exception ex)
            {
                log.Error("插入打印数据出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }


        }

        /// <summary>
        ///  小渔夫写的   
        /// 生成打印Excel文件
        /// </summary>
        string CreateExcelData(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return url;
            }

            string path = Server.MapPath(url);

            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在。");
            }


            Excel_Template.DataSet ds = new Excel_Template.DataSet();

            try
            {
                // 拿到主表数据
                ds.Items = this.m_MainStore.GetList() as List<LModel>;


                if (ds.Head == null && ds.Items.Count > 0)
                {
                    ds.Head = ds.Items[0];
                }

            }
            catch (Exception ex)
            {

                throw new Exception("查询数据出错了！", ex);

            }

            try
            {

                //文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT", "P", 4);
                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", fileName + ".xls");
                

                NOPIHandlerEX handler = new NOPIHandlerEX();

                SheetPro sp = handler.ReadExcel(path);

                handler.InsertSubData(sp, ds);


                //保存Excel文件在服务器中
                handler.WriteExcel(sp, wFile.PhysicalPath);
                handler.Dispose();
                
                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }

        ///// <summary>
        ///// 输出到打印机
        ///// </summary>
        //public void ToPrint(string paramJson)
        //{

        //    SortedList<string, object> userData = null;

        //    if (!StringUtil.IsBlank(paramJson))
        //    {
        //        try
        //        {
        //            userData = ConvertToResult(paramJson);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("打印参数错误." + paramJson, ex);
        //        }
        //    }
           

        //    int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
        //    int pageId = WebUtil.QueryInt("pageId");

        //    Store subStore = m_StoreList[1];

        //    int mainTableId = (int)m_MainStore.Tag;
        //    string mainTable = m_MainStore.Model;
        //    string mainPk = m_MainStore.IdField;

        //    int subTableId =  (int)subStore.Tag;
        //    string subTable = subStore.Model;

            
        //    if(userData != null )
        //    {
        //        if (userData.ContainsKey("subTableID"))
        //        {
        //            subTableId = Convert.ToInt32(userData["subTableID"]);
        //        }

        //        if (userData.ContainsKey("subTable"))
        //        {
        //            subTable = Convert.ToString(userData["subTable"]);
        //        }
        //    }


        //    string joinMainField = null;

        //    if (m_JoinMainFields.ContainsKey(subTable))
        //    {
        //        joinMainField = m_JoinMainFields[subTable];
        //    }

        //    if (string.IsNullOrEmpty(joinMainField))
        //    {
        //        throw new Exception("子表必须有一个字段指向父表.");
        //    }


        //    string url = "/App/InfoGrid2/View/PrintTemplate/PrintTemplateTest.aspx";

        //    NameValueCollection nv = new NameValueCollection(){
        //        {"id",pageId.ToString()},
        //        {"mainId",pkValue.ToString()},  //主数据记录的ID
        //        {"pageID",pageId.ToString()},

        //        {"fFiled",joinMainField},

        //        {"mainTableID",mainTableId.ToString()},  //表定义的ID
        //        {"mainTable",mainTable},    //表名
        //        {"mainPK",mainPk},

        //        {"subTableID",subTableId.ToString()},
        //        {"subTable",subTable},
        //        {"pageText",m_PageModel.DISPLAY}
        //    };


        //    StringBuilder sb = new StringBuilder();

        //    for (int i = 0; i < nv.Count; i++)
        //    {
        //        string key = nv.Keys[i];
        //        string value = nv[i];

        //        if (i > 0) { sb.Append("&"); }

        //        sb.Append(key).Append("=").Append(value);
        //    }

            

        //    string urlStr = url + "?" + sb.ToString();

        //    Window win = new Window("打印");
        //    win.StartPosition = WindowStartPosition.CenterScreen;
        //    win.ContentPath = urlStr;
        //    win.ShowDialog();

        //}

        /// <summary>
        /// 显示打印机界面  新的写法了  新的打印Excel写法，好像还不能通用
        /// 小渔夫 
        /// 1206-12-09
        /// </summary>
        public void ToPrint(string paramJson)
        {

            SortedList<string, object> userData = null;

            if (!StringUtil.IsBlank(paramJson))
            {
                try
                {
                    userData = ConvertToResult(paramJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("打印参数错误." + paramJson, ex);
                }
            }


            int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
            int pageId = WebUtil.QueryInt("pageId");

            Store subStore = m_StoreList[1];

            int mainTableId = (int)m_MainStore.Tag;
            string mainTable = m_MainStore.Model;
            string mainPk = m_MainStore.IdField;

            int subTableId = (int)subStore.Tag;
            string subTable = subStore.Model;


            if (userData != null)
            {
                if (userData.ContainsKey("subTableID"))
                {
                    subTableId = Convert.ToInt32(userData["subTableID"]);
                }

                if (userData.ContainsKey("subTable"))
                {
                    subTable = Convert.ToString(userData["subTable"]);
                }
            }


            string joinMainField = null;

            if (m_JoinMainFields.ContainsKey(subTable))
            {
                joinMainField = m_JoinMainFields[subTable];
            }

            if (string.IsNullOrEmpty(joinMainField))
            {
                throw new Exception("子表必须有一个字段指向父表.");
            }


            string url = "/App/InfoGrid2/View/PrintTemplate/PrintTempBuilder.aspx";

            NameValueCollection nv = new NameValueCollection(){
                {"id",pageId.ToString()},
                {"mainId",pkValue.ToString()},  //主数据记录的ID
                {"pageID",pageId.ToString()},

                {"fFiled",joinMainField},

                {"mainTableID",mainTableId.ToString()},  //表定义的ID
                {"mainTable",mainTable},    //表名
                {"mainPK",mainPk},

                {"subTableID",subTableId.ToString()},
                {"subTable",subTable},
                {"pageText",m_PageModel.DISPLAY}
            };


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nv.Count; i++)
            {
                string key = nv.Keys[i];
                string value = nv[i];

                if (i > 0) { sb.Append("&"); }

                sb.Append(key).Append("=").Append(value);
            }



            string urlStr = url + "?" + sb.ToString();

            Window win = new Window("打印");
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.ContentPath = urlStr;
            win.WindowClosed += Win_WindowClosed;
            win.ShowDialog();

        }

        /// <summary>
        /// 新的选择打印界面确定按钮的回调函数
        /// 小渔夫
        /// 2016-12-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void Win_WindowClosed(object sender, string data)
        {

            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            SModel sm = SModel.ParseJson(data);

            if (sm.Get<string>("result") != "ok")
            {
                return;
            }


            string[] idList = StringUtil.Split(sm.Get<string>("ids"), "|");

            if (idList.Length < 2)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            BIZ_PRINT bp = decipher.SelectModelByPk<BIZ_PRINT>(idList[0]);

            BIZ_PRINT_TEMPLATE bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(idList[1]);


            if (bp == null || bpt == null)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            string pathUrl = string.Empty;

            try
            {

                pathUrl = CreateExcelData_2(bpt.TEMPLATE_URL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("打印出错了！" + ex.Message);
                return;
            }

            try
            {

                BIZ_PRINT_FILE bpf = new BIZ_PRINT_FILE()
                {
                    FILE_URL = pathUrl,
                    PRINT_CODE = bp.PRINT_CODE,
                    PRINT_NAME = bp.PRINT_TEXT,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                decipher.InsertModel(bpf);

            }
            catch (Exception ex)
            {
                log.Error("插入打印数据出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }

        }

        /// <summary>
        ///  小渔夫写的   
        /// 生成打印Excel文件
        /// 修改于 2017-03-13   可以打印多个子表
        /// </summary>
        string CreateExcelData_2(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return url;
            }

            string path = Server.MapPath(url);

            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在。");
            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            try
            {

                //文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT", "P", 4);
                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", fileName + ".xls");
                wFile.CreateDir();


                SheetParam sp = TemplateUtilV1.ReadTemp(path);


                MoreSubTableDataSet ds_v1 = new MoreSubTableDataSet();


                foreach(LModel item in m_MainStore.GetList())
                {
                    if(item.GetPk<string>()== m_MainStore.CurDataId)
                    {
                        ds_v1.Head = item;
                    }
                }


                

                foreach (var item in m_StoreSet)
                {

                    if (m_MainStore.Model == item.Key)
                    {
                        continue;

                    }

                    ds_v1.Items.Add(item.Key, item.Value.GetList() as LModelList<LModel>);

                }


                Store subStore = m_StoreList[1];

                // 拿到主表数据
                ds_v1.OneItems = subStore.GetList() as List<LModel>;

                if (ds_v1.Head == null && ds_v1.OneItems.Count > 0)
                {
                    ds_v1.Head = ds_v1.OneItems[0];
                }


                //这个可以打多个子表的  只能顺序打 打完一个子表再打下一个子表  不能合在一起打
                TemplateUtilV1.CreateExcel(sp, ds_v1, wFile.PhysicalPath);

        

                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }



        /// <summary>
        /// 导出 Excel 
        /// 小渔夫
        /// 2018-01-29
        /// </summary>
        /// <param name="paramJson"></param>
        public void ToExcel(string paramJson)
        {

            log.Info(paramJson+"--------------111111111111111");

            SModel result = new SModel();

            if (!StringUtil.IsBlank(paramJson))
            {
                paramJson = "{" + paramJson + "}";

                try
                {
                    result = SModel.ParseJson(paramJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("导出 Excel 参数错误. " + paramJson, ex);
                }
            }



            int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
            int pageId = WebUtil.QueryInt("pageId");

            Store subStore = m_StoreList[1];

            int mainTableId = (int)m_MainStore.Tag;
            string mainTable = m_MainStore.Model;
            string mainPk = m_MainStore.IdField;

            int subTableId = result.Get("subTableID", (int)subStore.Tag);
            string subTable = result.Get("subTable", subStore.Model);


            string joinMainField = null;

            if (m_JoinMainFields.ContainsKey(subTable))
            {
                joinMainField = m_JoinMainFields[subTable];
            }

            if (string.IsNullOrEmpty(joinMainField))
            {
                throw new Exception("子表必须有一个字段指向父表.");
            }



            string url = "/App/InfoGrid2/View/PrintTemplate/ExportExcel.aspx";

            NameValueCollection nv = new NameValueCollection(){
                {"mainId",pkValue.ToString()},  //主数据记录的ID
                {"pageID",pageId.ToString()},

                {"fFiled",joinMainField},

                {"mainTableID",mainTableId.ToString()},  //表定义的ID
                {"mainTable",mainTable},    //表名
                {"mainPK",mainPk},

                {"subTableID",subTableId.ToString()},
                {"subTable",subTable},
                {"pageText","销售订单"}
            };


            UriInfo excelUrl = new UriInfo(url);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nv.Count; i++)
            {
                string key = nv.Keys[i];
                string value = nv[i];

                excelUrl.Append(key, value);
            }



            string urlStr = excelUrl.ToString();

            log.Info($"{urlStr}=========================================");


            Window win = new Window("导出 Excel");

            win.ContentPath = urlStr;
            win.WindowClosed += Win_WindowClosed1;
            win.ShowDialog();

        }

        /// <summary>
        /// 这是导出excel界面关闭事件
        /// 小渔夫
        /// 2018-01-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">返回来数据</param>
        public void Win_WindowClosed1(object sender, string data)
        {

            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            SModel sm = SModel.ParseJson(data);

            if (sm.Get<string>("result") != "ok")
            {
                return;
            }


            string url = sm.Get<string>("url");


            try
            {

                string  pathUrl = CreateExcelData_2(url);

                DownloadWindow.Show("下载Excel文件",pathUrl);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("导出Excel出错了！" + ex.Message);
                return;
            }



        }




        public void EditPage()
        {
            int pageId = WebUtil.QueryInt("pageId");

            MiniPager.Redirect("PageBuilder.aspx?id=" + pageId);
        }


        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="cmdParam"></param>
        public void ExecPlugin(string cmdParam)
        {
            string[] ps = cmdParam.Split(';');

            if (ps.Length < 4)
            {
                MessageBox.Alert("命令已经失效..");
                return;
            }

            int toolbarItemId = StringUtil.ToInt( ps[0]);
            string storeId = ps[1];
            string tableId = ps[2];
            string searchId = ps[3];

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TOOLBAR_ITEM tItem = decipher.SelectModelByPk<IG2_TOOLBAR_ITEM>(toolbarItemId);

            if (tItem == null)
            {
                MessageBox.Alert("命令已经失效.");
                return;
            }

            if (tItem.EVENT_MODE_ID != "PLUG")
            {
                MessageBox.Alert("非插件命令。");
                return;
            }

            if (StringUtil.IsBlank(tItem.PLUG_CLASS))
            {
                MessageBox.Alert("插件设置有错误:没有插件路径名.");
                return;
            }

            if (StringUtil.IsBlank(tItem.PLUG_METHOD))
            {
                MessageBox.Alert("插件设置有错误:没有插件函数名.");
                return;
            }


            Store storeUi;
            Table tableUi;


            storeUi = this.FindControl(storeId) as Store;

            if (storeUi == null)
            {
                MessageBox.Alert("数据仓库 " + storeId + " 不存在.");
                return;
            }

            tableUi = this.FindControl(tableId) as Table;

            if (tableUi == null)
            {
                MessageBox.Alert("表格 " + tableId + " 不存在.");
                return;
            }


            Type plugT = PluginManager.Get(tItem.PLUG_CLASS);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + tItem.PLUG_CLASS);
                return;
            }

            MethodInfo mi = plugT.GetMethod(tItem.PLUG_METHOD);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + tItem.PLUG_CLASS + ", " + tItem.PLUG_METHOD);
                return;
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = tItem.PLUG_CLASS;
                inter.Method = tItem.PLUG_METHOD;
                inter.Params = tItem.PLUG_PARAMS;

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.m_MainStore;
                inter.MainTable = this.m_MainTable;

                inter.SrcStore = storeUi;
                inter.SrcTable = tableUi;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }

        /// <summary>
        /// 插件步骤第二步
        /// </summary>
        /// <param name="cmdParam"></param>
        public void ExecPlugin_NextStep(string plug,string user_ps)
        {
            string plugJson = Base64Util.FromString(plug, Base64Mode.Http);

            JObject ppm;

            string plugClass;
            string plugMethod;
            string src_url;

            string plugParam;

            string srcStoreId;

            string srcTableId;

            try
            {
                ppm = (JObject)JsonConvert.DeserializeObject(plugJson);

                plugClass = ppm.Value<string>("plug_class");
                plugMethod = ppm.Value<string>("plug_method");
                src_url = ppm.Value<string>("src_url");

                plugParam = ppm.Value<string>("ps");

                srcStoreId = ppm.Value<string>("src_store_id");
                srcTableId = ppm.Value<string>("src_table_id");

            }
            catch (Exception ex)
            {
                log.Error("解析命令参数出错", ex);
                MessageBox.Alert("解析命令参数出错.");
                return;
            }




            Store storeUi;
            Table tableUi;


            storeUi = this.FindControl(srcStoreId) as Store;

            if (storeUi == null)
            {
                MessageBox.Alert("数据仓库 " + srcStoreId + " 不存在.");
                return;
            }

            tableUi = this.FindControl(srcTableId) as Table;

            if (tableUi == null)
            {
                MessageBox.Alert("表格 " + srcTableId + " 不存在.");
                return;
            }




            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
                return;
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
                return;
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = plugParam;

                inter.SrcUrl = src_url;

                inter.Main = this;
                inter.MainStore = this.m_MainStore;
                inter.MainTable = this.m_MainTable;

                inter.SrcStore = storeUi;
                inter.SrcTable = tableUi;

                inter.UserParams = user_ps;

                mi.Invoke(inter, null);

            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }


        }

        /// <summary>
        /// 获取展示规则
        /// </summary>
        /// <returns></returns>
        public string GetDisplayRule()
        {
            return App.InfoGrid2.Bll.DisplayRuleMgr.GetJScript();
        }




        /// <summary>
        /// 快速插入数据
        /// </summary>
        /// <param name="storeId">数据仓库ID</param>
        /// <param name="curId">当前已经建立的记录id</param>
        /// <param name="selectTable">表格id</param>
        /// <param name="selectIds">新建记录选择的id</param>
        /// <param name="mapId">映射ID</param>
        public void GoQueryInsert(string storeId, string curId, string selectTable, string selectIds, int mapId)
        {
            Store store = FindControl(storeId) as Store;

            if (store == null)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();



            TableSet tSet = TableSet.Select(decipher, mapId);

            //初始化元素,怕没有实体元素.
            TableMgr.GetModelElem(tSet);


            InsertPosition insertPos = store.DefaultInsertPos;  //插入的位置

            if (insertPos == InsertPosition.Last)
            {
                LModel curModel = decipher.GetModelByPk(store.Model, curId);

                int[] idList = StringUtil.ToIntList(selectIds);

                List<LModel> newModels = new List<LModel>();

                foreach (int id in idList)
                {
                    LModel newModel = new LModel(store.Model);

                    curModel.CopyTo(newModel);

                    newModel["ROW_DATE_CREATE"] = DateTime.Now;
                    newModel["ROW_DATE_UPDATE"] = DateTime.Now;

                    EvetnAfterMap(decipher, tSet, selectTable, id, newModel);

                    newModels.Add(newModel);
                }

                decipher.InsertModels((IList)newModels);

                store.AddRange(newModels);
            }
            else if (insertPos == InsertPosition.FocusLast)
            {
                //放到焦点行后面
                string sortField = tSet.Table.USER_SEQ_FIELD;    //排序的字段

                store.SortField = sortField;

                LModelList<LModel> models = store.GetList() as LModelList<LModel>;

                int curIdInt = StringUtil.ToInt(curId);

                LModel curModel = models.FindByPk(curIdInt);

                int index = models.IndexOf(curModel);

                int newIndex = 1;

                LModel tmpModel = null;

                for (int i = 0; i <= index; i++)
                {
                    tmpModel = models[i];
                    tmpModel[store.SortField] = (newIndex++);
                    decipher.UpdateModelProps(models[i], sortField);
                }


                int[] idList = StringUtil.ToIntList(selectIds);

                List<LModel> newModels = new List<LModel>();

                foreach (int id in idList)
                {
                    LModel newModel = new LModel(store.Model);

                    curModel.CopyTo(newModel);

                    newModel["ROW_DATE_CREATE"] = DateTime.Now;
                    newModel["ROW_DATE_UPDATE"] = DateTime.Now;

                    EvetnAfterMap(decipher, tSet, selectTable, id, newModel);

                    newModel[store.SortField] = (newIndex++);

                    newModels.Add(newModel);
                }

                for (int i = index + 1; i < models.Count; i++)
                {
                    tmpModel = models[i];
                    tmpModel[store.SortField] = (newIndex++);
                    decipher.UpdateModelProps(models[i], sortField);
                }

                decipher.InsertModels((IList)newModels);

                store.InsertRange(index + 1, newModels);
            }

        }

        private void EvetnAfterMap(DbDecipher decipher, TableSet tSet, string selectTable, int id, LModel newModel)
        {
            LModel selectModel = decipher.GetModelByPk(selectTable, id);

            foreach (IG2_TABLE_COL item in tSet.Cols)
            {
                if (StringUtil.IsBlank(item.EVENT_AFTER_FIELD_ID))
                {
                    continue;
                }

                newModel[item.EVENT_AFTER_FIELD_ID] = selectModel[item.DB_FIELD];
            }

        }



    }
}