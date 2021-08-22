using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5;
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
using App.InfoGrid2.Model.XmlModel;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.View.OneForm
{
    public partial class FormEditPreview : WidgetControl, IView
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
            m_SecUiFty.InitSecUI(pageId, "PAGE","ONE_FORM", m_MenuId, null);

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

                //初始化流程按钮
                InitFlowButton();
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
        /// 附件json数据
        /// </summary>
        public string m_biz_file_json;

        /// <summary>
        /// 行ID
        /// </summary>
        public string m_row_id_json;

        /// <summary>
        /// 表ID
        /// </summary>
        public string m_table_id_json;

        /// <summary>
        /// 表名
        /// </summary>
        public string m_table_naem_json;



        /// <summary>
        /// 大标题
        /// </summary>
        string m_alias_title;

        public string GetAliasTitle()
        {
            return m_alias_title;
        }

        /// <summary>
        /// 获取单据类型,默认采用标题
        /// </summary>
        /// <returns></returns>
        public string GetBillType()
        {
            return StringUtil.NoBlank(this.BillType, m_alias_title);
        }

        /// <summary>
        /// （注：临时变量，以后整理后撤销）
        /// 关联主表的字段名
        /// </summary>
        string m_JoinMainField;

        /// <summary>
        /// 
        /// </summary>
        SortedList<string, string> m_JoinMainFields = new SortedList<string, string>();

        SortedList<string, TableJoinConfig> m_JoinMainFieldsV2 = new SortedList<string, TableJoinConfig>();


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
        private Store GetStoreByTable(bool isShareData, string tableName, string instStoreId)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName", "表名不能为空,获取数据仓库 Store 失败.");
            }

            if (!isShareData && StringUtil.IsBlank(instStoreId))
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

            lock (m_StoreSet)
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
                store.ID = newStoreId;
                store.IdField = modelElem.PrimaryKey;
                store.Model = tableName;
                store.StringFields = ToString(modelElem.Fields);

                store.Updating += store_Updating;
                store.Inserting += Store_Inserting;
                store.Deleting += Store_Deleting;
                store.Inserted += Store_Inserted;

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

        /// <summary>
        /// 需要处理编辑状态?
        /// </summary>
        /// <param name="store"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool CanCancelEditor(Store store, string tableName)
        {
            bool cancel = false;

            int rowPk = WebUtil.QueryInt("row_pk");

            IG2_TABLE mainTable = m_MainTs.Table;

            LModelElement modelElem = LightModel.GetLModelElement(mainTable.TABLE_NAME);

            if (modelElem.HasField("BIZ_SID"))
            {

                LightModelFilter filterFLOW = new LightModelFilter(mainTable.TABLE_NAME);
                filterFLOW.AddFilter("ROW_SID >= 0");
                filterFLOW.And(mainTable.ID_FIELD, rowPk);
                filterFLOW.Fields = new string[] { "BIZ_SID" };

                DbDecipher decipher = ModelAction.OpenDecipher();
                SModel flowSM = decipher.GetSModel(filterFLOW);

                if (flowSM != null )
                {
                    int bizSid = flowSM.Get<int>("BIZ_SID");

                    if (bizSid > 0)
                    {

                        bool secEditor = CanEditor(store.Model);

                        if (!secEditor)
                        {
                            cancel = true;

                        }
                    }
                }
            }

            return cancel;
        }

        private void Store_Deleting(object sender, ObjectCancelEventArgs e)
        {
            Store store = (Store)sender;

            bool secEditor = CanCancelEditor(store, store.Model);

            if (secEditor)
            {
                e.Cancel = true;

                Toast.Show("此记录已经'提交',无法再删除记录!");
            }

        }

        private void Store_Inserted(object sender, ObjectEventArgs e)
        {
            Store store = (Store)sender;

            int tableId = (int)store.Tag;


            TableSet ts = m_TableSet_Dict[tableId];  //索引
            IG2_TABLE tab = ts.Table;

            if (tab.FORM_NEW_TYPE == "ONE_FORM")
            {
                LModel model = e.Object as LModel;

                object pk = model.GetPk();
                int pageId = tab.FORM_NEW_PAGEID;




                string title = StringUtil.NoBlank(tab.FORM_NEW_ALIAS_TITLE, tab.DISPLAY);


                string urlStr = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={pageId}&edit_pageid={tab.FORM_EDIT_PAGEID}";


                Window win = new Window("货物明细");
                win.StartPosition = WindowStartPosition.CenterParent;
                win.ContentPath = urlStr;
                win.State = WindowState.Max;
                win.Mode = true;
                //win.FormClosedForJS = string.Concat(
                //    "function(e)",
                //    "{",
                //    "if (e.result != 'ok'){ return; }",
                //    "widget1.submit('form:first', {action: 'btnPrint',actionPs:e.ids});",
                //    "}");

                win.ShowDialog();
            }
        }

        private void Store_Inserting(object sender, ObjectCancelEventArgs e)
        {
            Store store = (Store)sender;
            bool secEditor = CanCancelEditor(store, store.Model);

            if (secEditor)
            {
                e.Cancel = true;

                Toast.Show("此记录已经'提交',无法再新建记录!");
            }

        }

        private bool CanEditor(string tableName)
        {
            int pageId = WebUtil.QueryInt("pageid");
            int rowPk = WebUtil.QueryInt("row_pk");

            IG2_TABLE mainTable = m_MainTs.Table;

            LightModelFilter filterFLOW = new LightModelFilter(mainTable.TABLE_NAME);
            filterFLOW.AddFilter("ROW_SID >= 0");
            filterFLOW.And(mainTable.ID_FIELD, rowPk);
            filterFLOW.Fields = new string[] { "BIZ_FLOW_INST_CODE", "BIZ_FLOW_STEP_CODE", "BIZ_FLOW_DEF_CODE", "BIZ_FLOW_CUR_NODE_CODE" };

            DbDecipher decipher = ModelAction.OpenDecipher();
            SModel flowSM = decipher.GetSModel(filterFLOW);

            if (flowSM == null)
            {
                return false;
            }



            string userCode = this.EcUser.ExpandPropertys["USER_CODE"];
            string instCode = (string)flowSM["BIZ_FLOW_INST_CODE"];
            string stepCodde = (string)flowSM["BIZ_FLOW_STEP_CODE"];

            bool isCurNode = FlowInstMgr.IsFlowNodeParty(decipher, userCode, instCode, stepCodde);

            if (!isCurNode)
            {
                return false;
            }


            string flowCode = (string)flowSM["BIZ_FLOW_DEF_CODE"];
            string flowNodeCode = (string)flowSM["BIZ_FLOW_CUR_NODE_CODE"];

            bool secEditor = false;

            string[] secVFields = GetSecTFN_Fields(pageId, mainTable.TABLE_NAME, tableName, flowCode, flowNodeCode, out secEditor);

            return secEditor;
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

            int pageId = WebUtil.QueryInt("pageId", 33); //表单结构的 id



            int rowPk = WebUtil.QueryInt("row_pk");  //行主键





            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = m_PageModel = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (model == null)
            {
                throw new Exception(string.Format("复杂表页面的数据 IG2_TABLE 不存在, IG2_TABLE_ID={0}", pageId));
            }






            HeadPanel.Visible = model.IS_BIG_TITLE_VISIBLE;

            string alias_title = WebUtil.Query("alias_title");

            m_alias_title = StringUtil.NoBlank(alias_title, model.DISPLAY);
            
            #region 特殊处理

            //if (m_TableName == "UT_001")
            //{
            //    string ioTag = model.Get<string>("IO_TAG");

            //    if (ioTag == "I")
            //    {
            //        title = "入库单";
            //    }
            //    else if (ioTag == "O")
            //    {
            //        title = "出库单";
            //    }
            //}

            #endregion


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


            

            #region   小渔夫写的  判断是否显示附件
            
            IG2_TABLE mainTab = m_MainTs.Table;
            this.Enclosure1.Visible = mainTab.ATTACH_FILE_VISIBLE;

            bizSID_ci.Visible = mainTab.FORM_BIG_BIZSID_VISIBLE;
            FormLayoutTop.Visible = mainTab.FORM_RT_VISIBLE;

            #endregion

            //this.viewport1.Controls.Add(panel);

            this.bizSID_ci.DataSource = m_MainStore.ID;
            this.FormLayoutTop.StoreID = m_MainStore.ID;

            bool designMode = WebUtil.QueryBool("design_mode");
            Button1.Text = designMode ? "切换为-正常模式" : "切换为-设计模式";

            this.Button5.Visible = designMode;
            this.Button2.Visible = designMode;


            InitCustomPage(model.SERVER_CLASS);
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
            else
            {
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

        private void StoreJoinStore(Store mainStore, Store subStore, TableJoinConfig cfg)
        {
            foreach (var item in cfg.items)
            {
                StoreCurrentParam param = new StoreCurrentParam(item.field, mainStore.ID, item.join_field);

                StoreCurrentParam param2 = new StoreCurrentParam(item.field, mainStore.ID, item.join_field);

                subStore.FilterParams.Add(param);
                subStore.InsertParams.Add(param2);
            }
        }


        /// <summary>
        /// 数据集
        /// </summary>
        SortedList<int, TableSet> m_TableSet_Dict = new SortedList<int, TableSet>();


        /// <summary>
        /// 数据集(某个页面控件 ID  绑定的 TableSet );
        /// </summary>
        SortedList<string, TableSet> m_TableSet_ControlDict = new SortedList<string, TableSet>();

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


            m_TableSet_Dict[view.IG2_TABLE_ID] = tSet;  //索引

            if (view.JOIN_ENABLED)
            {
                if (view.JOIN_VERSION == 2)
                {
                    TableJoinConfig cfg = XmlUtil.Deserialize<TableJoinConfig>(view.JOIN_V2_CONFIG);

                    Store joinStore = GetStoreByTable(true, cfg.join_table,null);

                    StoreJoinStore(joinStore, store, cfg);

                    if (!m_JoinMainFieldsV2.ContainsKey(view.TABLE_NAME))
                    {
                        m_JoinMainFieldsV2.Add(view.TABLE_NAME, cfg);
                    }
                }
                else
                {
                    if (!StringUtil.IsBlank(view.ME_COL_NAME, view.JOIN_TAB_NAME, view.JOIN_COL_NAME))
                    {
                        Store joinStore = GetStoreByTable(true, view.JOIN_TAB_NAME,null);

                        StoreJoinStore(joinStore, store, view.JOIN_COL_NAME, view.ME_COL_NAME);

                        if (!m_JoinMainFields.ContainsKey(view.TABLE_NAME))
                        {
                            m_JoinMainFields.Add(view.TABLE_NAME, view.ME_COL_NAME);
                        }
                    }
                }
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

            Store store = GetStoreByTable(true,view.TABLE_NAME,null);

            store.Tag = tSet.Table.IG2_TABLE_ID;


            if (m_MainStore == null)
            {
                m_MainStore = store;
                m_MainStore.CurrentChanged += m_MainStore_CurrentChanged;
                
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
                m_MainTable = (Table)con ;
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

                if (modelElem.HasField("BIZ_SID"))
                {
                    //store.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(string.Format(
                    //    "(BIZ_SID is null OR BIZ_SID <= {0})", view.DELETE_BY_BIZ_SID)));

                    Param pm = new Param("BIZ_SID");
                    pm.SetInnerValue( view.DELETE_BY_BIZ_SID);
                    pm.Logic = "<=";

                    store.DeleteQuery.Add(pm);
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

                if(!StringUtil.IsBlank(view.LOCKED_RULE))
                {
                    store.LockedRule = view.LOCKED_RULE;
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


                #region 权限,二次叠加

                ProSec2(view, table1, store);

                #endregion


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


        #region 权限, 二次叠加


        private void ProSec2(IG2_TABLE view, FormLayout fromLayout1, Store store)
        {
            int pageId = WebUtil.QueryInt("pageid");
            int rowPk = WebUtil.QueryInt("row_pk");

            LModelElement mainLModelElem = LightModel.GetLModelElement(m_MainTs.Table.TABLE_NAME);

            if (!mainLModelElem.HasField("BIZ_FLOW_INST_CODE"))
            {
                return;
            }

            IG2_TABLE mainTable = m_MainTs.Table;
            
        

            LightModelFilter filterFLOW = new LightModelFilter(mainTable.TABLE_NAME);
            filterFLOW.AddFilter("ROW_SID >= 0");
            filterFLOW.And(mainTable.ID_FIELD, rowPk);
            filterFLOW.Fields = new string[] { "BIZ_FLOW_INST_CODE", "BIZ_FLOW_STEP_CODE", "BIZ_FLOW_DEF_CODE", "BIZ_FLOW_CUR_NODE_CODE" };

            DbDecipher decipher = ModelAction.OpenDecipher();
            SModel flowSM = decipher.GetSModel(filterFLOW);

            if (flowSM == null)
            {
                return;
            }



            string userCode = this.EcUser.ExpandPropertys["USER_CODE"];
            string instCode = (string)flowSM["BIZ_FLOW_INST_CODE"];
            string stepCodde = (string)flowSM["BIZ_FLOW_STEP_CODE"];

            bool isCurNode = FlowInstMgr.IsFlowNodeParty(decipher, userCode, instCode, stepCodde);

            if (!isCurNode)
            {
                return;
            }


            string flowCode = (string)flowSM["BIZ_FLOW_DEF_CODE"];
            string flowNodeCode = (string)flowSM["BIZ_FLOW_CUR_NODE_CODE"];

            bool secEditor = false;
            string[] secVFields = GetSecTFN_Fields(pageId, mainTable.TABLE_NAME, view.TABLE_NAME, flowCode, flowNodeCode,out secEditor);

            if (secVFields != null && secVFields.Length > 0)
            {
                foreach (var secVField in secVFields)
                {
                    FieldBase fb = fromLayout1.FindByDbField(secVField) as FieldBase;

                    if(fb == null)
                    {
                        continue;
                    }

                    fb.ReadOnly = false;                    
                }

                store.LockedExclusionFields = secVFields;
            }

            if (secEditor)
            {
                int n = store.DeleteQuery.RemoveAll(p =>
                {
                    return (p.Name == "BIZ_SID");
                });


            }

        }

        /// <summary>
        /// 权限,二次叠加
        /// </summary>
        private void ProSec2(IG2_TABLE view, Table table1, Store store)
        {

            int pageId = WebUtil.QueryInt("pageid");
            int rowPk = WebUtil.QueryInt("row_pk");

            IG2_TABLE mainTable = m_MainTs.Table;

            LModelElement mainModelElem = LightModel.GetLModelElement(mainTable.TABLE_NAME);

            if (!mainModelElem.HasField("BIZ_FLOW_INST_CODE"))
            {
                return;
            }

            LightModelFilter filterFLOW = new LightModelFilter(mainTable.TABLE_NAME);
            filterFLOW.AddFilter("ROW_SID >= 0");
            filterFLOW.And(mainTable.ID_FIELD, rowPk);
            filterFLOW.Fields = new string[] { "BIZ_FLOW_INST_CODE", "BIZ_FLOW_STEP_CODE", "BIZ_FLOW_DEF_CODE", "BIZ_FLOW_CUR_NODE_CODE" };

            DbDecipher decipher = ModelAction.OpenDecipher();
            SModel flowSM = decipher.GetSModel(filterFLOW);

            if (flowSM == null)
            {
                return;
            }



            string userCode = this.EcUser.ExpandPropertys["USER_CODE"];
            string instCode = (string)flowSM["BIZ_FLOW_INST_CODE"];
            string stepCodde = (string)flowSM["BIZ_FLOW_STEP_CODE"];

            bool isCurNode = FlowInstMgr.IsFlowNodeParty(decipher, userCode, instCode, stepCodde);

            if (!isCurNode)
            {
                return;
            }


            string flowCode = (string)flowSM["BIZ_FLOW_DEF_CODE"];
            string flowNodeCode = (string)flowSM["BIZ_FLOW_CUR_NODE_CODE"];

            bool secEditor = false;
            string[] secVFields = GetSecTFN_Fields(pageId, mainTable.TABLE_NAME, view.TABLE_NAME, flowCode, flowNodeCode, out secEditor);

            if (secVFields != null && secVFields.Length > 0)
            {
                foreach (var secVField in secVFields)
                {
                    BoundField bf = table1.Columns.FindByDataField(secVField);

                    bf.EditorMode = EditorMode.Auto;
                }

                store.LockedExclusionFields = secVFields;
            }

            if (secEditor)
            {
                int n = store.DeleteQuery.RemoveAll(p =>
                {
                    return (p.Name == "BIZ_SID");
                });
            }
        }


        /// <summary>
        /// 二次权限, 控制表格和主表的只读状态.
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="dbTable"></param>
        /// <param name="secTable">需要控制权限的表名</param>
        /// <param name="flowCode"></param>
        /// <param name="flowNodeCode"></param>
        /// <returns></returns>
        private string[] GetSecTFN_Fields(int pageId, string dbTable, string secTable,  string flowCode, string flowNodeCode,out bool editor)
        {
            SEC_TABLE_FLOW_NODE tfn = GetSecTFN(pageId, dbTable, flowCode, flowNodeCode);

            editor = false;

            if (tfn == null)
            {
                return null;
            }

            string fields = null;

            //bool editor = false;

            if(secTable == tfn.T_HEADER)
            {
                fields = tfn.T_HEADER_V_FIELDS;
            }
            else if(secTable == tfn.T_FOOTER)
            {
                fields = tfn.T_FOOTER_V_FIELDS;
            }
            else
            {
                for (int i = 1; i <= 9; i++)
                {
                    string field = $"T_SUB_{i}";

                    if (StringUtil.IsBlank(tfn[field]))
                    {
                        break;
                    }

                    string value = (string)tfn[field];

                    if(value == secTable)
                    {  
                        fields = (string)tfn[$"T_SUB_{i}_V_FIELDS"];
                        editor = (bool)tfn[$"T_SUB_{i}_EDITOR"];

                        break;
                    }
                }

            }

            if(fields == null)
            {
                return null;
            }

            string[] sp = StringUtil.Split(fields, ",");

            return sp;            
        }

        /// <summary>
        /// 二次权限, 控制表格和主表的只读状态.
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="dbTable"></param>
        /// <param name="flowCode"></param>
        /// <param name="flowNodeCode"></param>
        /// <returns></returns>
        private SEC_TABLE_FLOW_NODE GetSecTFN(int pageId, string dbTable, string flowCode, string flowNodeCode)
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_TABLE_FLOW_NODE));
            filter.AddFilter("ROW_SID >= 0");
            filter.And("PAGE_ID", pageId);
            filter.And("DB_TABLE", dbTable);
            filter.And("FLOW_CODE", flowCode);
            filter.And("FLOW_NODE_CODE", flowNodeCode);

            DbDecipher decipher = ModelAction.OpenDecipher();

            var tfn = decipher.SelectToOneModel<SEC_TABLE_FLOW_NODE>(filter);

            return tfn;
        }


        #endregion


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

            m_TableSet_ControlDict[id] = tSet;

            IG2_TABLE view = tSet.Table;


            Store store = GetStoreByTable(true, view.TABLE_NAME, null);

            store.Tag = itemViewID;

            if (m_MainStore == null)
            {
                int rowPk = WebUtil.QueryInt("row_pk");

                string idField = StringUtil.NoBlank(view.ID_FIELD, "ROW_IDENTITY_ID");

                store.FilterParams.Add(new Param(idField, rowPk.ToString()));

                m_MainStore = store;

                m_MainStore.CurrentChanged += m_MainStore_CurrentChanged;

                m_MainTs = tSet;

            }




            if (!StringUtil.IsBlank(view.LOCKED_FIELD) && StringUtil.IsBlank(store.LockedField))
            {
                store.LockedField = view.LOCKED_FIELD;
            }

            if(!StringUtil.IsBlank( view.LOCKED_RULE) && StringUtil.IsBlank(store.LockedRule))
            {
                store.LockedRule = view.LOCKED_RULE;
            }


            //bool isLockSub =  store.ReadOnly;
            
            //if (!isLockSub && model != null && !StringUtil.IsBlank(store.LockedField))
            //{
            //    object value = model[store.LockedField];

            //    isLockSub = BoolUtil.ToBool(value);
            //}


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


            #region 临时写的锁表头权限, 整理代码的时候, 要移除..[2017-1-4 13:21] 


            TableSet newTSet = m_SecUiFty.FilterTableSet("FORM", m_SecTag, id, tSet);

            SEC_UI secUi = m_SecUiFty.SecUI;

            if (StringUtil.IsBlank(store.LockedRule) && secUi != null)
            {
                LightModelFilter secFilter = new LightModelFilter(typeof(SEC_UI_TABLE));
                secFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                secFilter.And("SEC_UI_ID", secUi.SEC_UI_ID);
                secFilter.And("PAGE_ID", secUi.UI_PAGE_ID);


                if (secUi.UI_TYPE_ID == "PAGE")
                {
                    if (secUi.UI_SUB_TYPE_ID == "TABLE_FORM")
                    {

                    }
                    else
                    {
                        secFilter.And("DISPALY_MODE_ID", "FORM");
                        secFilter.And("PAGE_AREA_ID", id);
                    }
                }

                SEC_UI_TABLE suTable = decipher.SelectToOneModel<SEC_UI_TABLE>(secFilter);

                if (suTable != null)
                {
                    store.LockedRule = suTable.LOCKED_RULE;
                }
                //store.LockedRule = suTable.
            }

            #endregion

            if (newTSet != null)
            {
                tSet = newTSet;
            }

            formFty.CreateFormControls(form1, tSet);

            #region  临时加进去.


            #endregion


            #region 权限,二次叠加

            ProSec2(view, form1, store);

            #endregion

            if (id == "HanderForm1")
            {
                int tableId = view.IG2_TABLE_ID;

                Toolbar toolbar1 = this.mainToolbar1;



                ToolbarSet tbSet = new ToolbarSet();
                tbSet.SelectForTable(decipher, tableId);



                M2ToolbarFactory toolbarFactory = new M2ToolbarFactory();
                toolbarFactory.StoreId = m_MainStore.ID;
                toolbarFactory.TableId = tableId.ToString();
                //toolbarFactory.SearchFormID = (m_MainSearch != null ? m_MainSearch.ID : "");
                toolbarFactory.CreateItems(toolbar1, tbSet);


                m_SecUiFty.FilterToolbar("FORM", id, toolbar1);



                if (m_MainToolbar == null && toolbar1 != null)
                {
                    m_MainToolbar = toolbar1;
                }

                //if (m_MainTable == null && (con is Table))
                //{
                //    m_MainTable = (Table)con;
                //}

            }


        }




        /// <summary>
        /// 业务状态 0 -> 2
        /// </summary>
        public void GoBizSID_0_2()
        {
            int rowPk = WebUtil.QueryInt("row_pk");

            IList models = this.m_MainStore.GetList();

            LModel model = models[0] as LModel;

            LModelElement modelElem = model.GetModelElement();

            // 原"松香"项目的代码
            //if (modelElem.DBTableName == "UT_001" && model.Get<int>("BIZ_SID") == 0)
            //{
            //    model.SetTakeChange(true);

            //    model["BIZ_SID"] = 2;
            //    model["BILL_NO"] = BillIdentityMgr.NewCodeForDay("IO", "I");

            //    model["COL_1"] = BizServer.LoginName;
            //    model["COL_3"] = DateTime.Now;

            //    DbDecipher decipher = ModelAction.OpenDecipher();

            //    decipher.UpdateModel(model, true);

            //    AutoAdd_UT001(model);

            //    this.m_MainStore.Refresh();
            //}

            //if (modelElem.DBTableName == "UT_008" && model.Get<int>("BIZ_SID") == 0)
            //{
            //    model.SetTakeChange(true);

            //    model["BIZ_SID"] = 2;
            //    model["BILL_NO"] = BillIdentityMgr.NewCodeForDay("IO", "I");

            //    model["COL_1"] = BizServer.LoginName;
            //    model["COL_3"] = DateTime.Now;

            //    DbDecipher decipher = ModelAction.OpenDecipher();

            //    decipher.UpdateModel(model, true);

            //    AutoAdd_UT001(model);

            //    this.m_MainStore.Refresh();
            //}

            //if (modelElem.DBTableName == "UT_002")
            //{
            //    AutoAdd_UT002(model);
            //}


            //pageId=3&edit_pageid=3&alias_title=%E5%85%A5%E5%BA%93%E5%8D%95&form_edit_pageID=3


            Toast.Show("保存成功!");
            


        }



        /// <summary>
        /// 自动插入数据
        /// </summary>
        private void AutoAdd_UT001(LModel ownerModel)
        {
            bool isRosinSystem = GlobelParam.GetValue<bool>("IS_ROSIN_SYSTEM", false);

            if (!isRosinSystem)
            {
                return;
            }

            LModelElement modelElem = ownerModel.GetModelElement();

            if (modelElem.DBTableName == "UT_001" && modelElem.Fields.ContainsField("CLIENT_NAME"))
            {
                string clientName = ownerModel.Get<string>("CLIENT_NAME").Trim();



                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter("UT_005");
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("CLIENT_TEXT", clientName);

                if (!decipher.ExistsModels(filter))
                {
                    LModel model = new LModel("UT_005");
                    model["CLIENT_TEXT"] = clientName;

                    decipher.InsertModel(model);
                }

            }
        }

        /// <summary>
        /// 自动插入数据
        /// </summary>
        private void AutoAdd_UT002(LModel ownerModel)
        {
            bool isRosinSystem = GlobelParam.GetValue<bool>("IS_ROSIN_SYSTEM", false);

            if (!isRosinSystem)
            {
                return;
            }

            LModelElement modelElem = ownerModel.GetModelElement();

            if (modelElem.DBTableName == "UT_002" && modelElem.Fields.ContainsField("COL_3"))
            {
                string clientName = ownerModel.Get<string>("COL_3").Trim();



                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter("UT_006");
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("PROD_TEXT", clientName);

                if (!decipher.ExistsModels(filter))
                {
                    LModel model = new LModel("UT_006");
                    model["PROD_TEXT"] = clientName;

                    decipher.InsertModel(model);
                }

            }
        }




        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        public void ChangeBizSID(string psStr)
        {

            //执行自定义页面的函数
            if (m_CustomPage != null)
            {
                Type custPageT = m_CustomPage.GetType();

                MethodInfo mi = custPageT.GetMethod("ChangeBizSID");

                if (mi != null && mi.GetParameters().Length == 1)
                {
                    mi.Invoke(m_CustomPage, new object[] { psStr });
                }

            }



            int row_pk = WebUtil.QueryInt("row_pk");

            string[] sp = StringUtil.Split(psStr, ",");


            Table tab = m_MainTable;
            Store store = m_MainStore;

            if (sp.Length == 4)
            {
                string tableId = sp[2];
                string storeId = sp[3];

                tab = FindControl(this.viewport1, tableId) as Table;
                store = FindControl(this.StoreSet, storeId) as Store;

                psStr = $"{sp[0]},{sp[1]}";
            }



            try
            {
                ResultBase result = BizHelper.ChangeBizSID(psStr, row_pk, store);

                MessageBox.Alert(result.Message);

                EasyClick.Web.Mini.MiniHelper.Eval("flow_BtnChagned()");
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }



            //执行自定义页面的函数
            if (m_CustomPage != null)
            {
                Type custPageT = m_CustomPage.GetType();

                MethodInfo mi = custPageT.GetMethod("ChangeBizSID_After");

                if (mi != null && mi.GetParameters().Length == 1)
                {
                    mi.Invoke(m_CustomPage, new object[] { psStr });
                }

            }


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

            int toolbarItemId = StringUtil.ToInt(ps[0]);
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
        public void ExecPlugin_NextStep(string plug, string user_ps)
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
        /// 流程激活, 0=没有激活, 2=激活
        /// </summary>
        public int FlowEnabled { get; set; }

        public int MainPageId { get; set; }

        public int MainTableId { get; set; }

        public int MainRowId { get; set; }

        public int MenuId { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string BillType { get; set; }

        public string DocUrl { get; set; }



        /// <summary>
        /// 初始化流程按钮
        /// </summary>
        private void InitFlowButton()
        {
            IG2_TABLE tab = m_MainTs.Table;

            this.FlowEnabled = tab.FLOW_ENABLED ? 2 : 0;

            if (!tab.FLOW_ENABLED)
            {
                return;
            }

            this.MainPageId = WebUtil.QueryInt("pageid");

            this.MainTableId = tab.IG2_TABLE_ID;
            this.MainRowId = WebUtil.QueryInt("row_pk");

            this.MenuId = WebUtil.QueryInt("menu_id");

           

            this.DocUrl = Base64Util.ToString($"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={this.MainRowId}&pageId={this.MainPageId}&menu_id={this.MenuId}&alias_title={this.GetAliasTitle()}");


            //return;

            //string flowParamsJson = tab.FLOW_PARAMS;

            //if(!StringUtil.IsBlank(flowParamsJson))
            //{
            //    log.Error("流程参数为空!");
            //}


            //SModelList smList;

            //try
            //{
            //    smList = SModelList.ParseJson(flowParamsJson);
            //}
            //catch(Exception ex)
            //{
            //    log.Error("流程参数定义错误: \r\n" + flowParamsJson, ex);
            //    return;
            //}



            //Toolbar tb = this.mainToolbar1;


            //tb.Items.Add("-");

            //ToolBarButton btn = new ToolBarButton("提交");
            //btn.Command = "GoFlowNode";
            //tb.Items.Add(btn);

        }

        /// <summary>
        /// 执行流程节点
        /// </summary>
        public void GoFlowNode()
        {
            Window flowWin = new Window("流程节点");
            flowWin.StartPosition = WindowStartPosition.CenterScreen;

            flowWin.ContentPath = $"/App/InfoGrid2/View/OneForm/FlowStep.aspx?";

            flowWin.ShowDialog();
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
        /// 导出 Excel  
        /// 修改与 2018-11-16 把导出excel功能，放在这里
        /// 
        /// </summary>
        /// <param name="paramJson"></param>
        public void ToExcel(string paramJson)
        {
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

            TableSet viewSet = m_TableSet_Dict[subTableId];

            IG2_TABLE view = viewSet.Table;

            string joinMainField = null;

            if (view.JOIN_VERSION == 2)
            {
                if (m_JoinMainFieldsV2.ContainsKey(subTable))
                {
                    TableJoinConfig cfg = m_JoinMainFieldsV2[subTable];

                    joinMainField = cfg.items[0].field;
                }
            }
            else
            {
                if (m_JoinMainFields.ContainsKey(subTable))
                {
                    joinMainField = m_JoinMainFields[subTable];
                }
            }

            if (string.IsNullOrEmpty(joinMainField))
            {
                throw new Exception("子表必须有一个字段指向父表.");
            }

            string url = "/App/InfoGrid2/View/PrintTemplate/DownloadTemplate.aspx";

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

            Window win = new Window("导出 Excel");

            win.ContentPath = urlStr;
            win.WindowClosed += Win_WindowClosed1;
            win.ShowDialog();

        }

        /// <summary>
        /// 导出Excel关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
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

            string url = sm.GetString("url");

            Excel_Template.DataSet ds = new Excel_Template.DataSet();

            try
            {


                //拿到主表数据
                ds.Head = m_MainStore.GetFirstData() as LModel;



                //拿到子表数据
                ds.Items = m_StoreList[1].GetList() as List<LModel>;

            }
            catch (Exception ex)
            {
                log.Error("查询数据出错了！", ex);
                MessageBox.Alert("查询数据出错了！");
                return;
            }

            try
            {
                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
                wFile.CreateDir();

                string srcPath = Server.MapPath(url);


                SheetParam sp = TemplateUtilV1.ReadTemp(srcPath);

                //保存Excel文件在服务器中
                TemplateUtilV1.CreateExcel(sp, ds, wFile.PhysicalPath);

                DownloadWindow.Show(wFile.Filename, wFile.RelativePath);

            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);
                MessageBox.Alert("导出Excel文件出错了！");
            }


        }


        /// <summary>
        /// 输出到打印机
        /// 小渔夫
        /// 修改于 2016-12-11
        /// </summary>
        public void ToPrint(string paramJson)
        {

            SortedList<string, object> result = null;

            if (!StringUtil.IsBlank(paramJson))
            {
                try
                {
                    result = ConvertToResult(paramJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("打印参数错误." + paramJson, ex);

                }
            }


            int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
            int pageId = WebUtil.QueryInt("pageId");

            Store subStore = m_StoreList[1];

            int mainTableId =  (int)m_MainStore.Tag;
            string mainTable = m_MainStore.Model;
            string mainPk = m_MainStore.IdField;

            int subTableId = (int)subStore.Tag;
            string subTable = subStore.Model;




            if (result != null && result.ContainsKey("subTableID"))
            {
                subTableId = Convert.ToInt32(result["subTableID"]);
            }

            if (result != null && result.ContainsKey("subTable"))
            {
                subTable = Convert.ToString(result["subTable"]);
            }

            TableSet viewSet = m_TableSet_Dict[subTableId];

            IG2_TABLE view = viewSet.Table;

            string joinMainField = null;

            if (view.JOIN_VERSION == 2)
            {
                if (m_JoinMainFieldsV2.ContainsKey(subTable))
                {
                    TableJoinConfig cfg = m_JoinMainFieldsV2[subTable];

                    joinMainField = cfg.items[0].field;
                }
            }
            else
            {
                if (m_JoinMainFields.ContainsKey(subTable))
                {
                    joinMainField = m_JoinMainFields[subTable];
                }
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
        /// 小渔夫写的 
        /// 2016-12-11
        /// 根据选择的打印机，和打印模板生成打印文件输出到打印机中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">关闭窗口返回来的数据</param>
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


            // ids = 打印机ID | 模板ID
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


            Store sub_store = GetStoreByTableName(sm.Get<string>("sub_table_name"));

            string pathUrl = string.Empty;

            try
            {

                pathUrl = CreateExcelData(bpt.TEMPLATE_URL,sub_store);
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
        /// 根据表名获取到相应的数据仓库
        /// 小渔夫
        /// 2016-12-11
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <returns></returns>
        public Store GetStoreByTableName(string table_name)
        {
            Store sub_store = m_StoreList[1];

            if (string.IsNullOrWhiteSpace(table_name))
            {
                return sub_store;
            }


            foreach(Store s in m_StoreList)
            {
                //找到表名对应的数据仓库
                if(s.Model.Equals(table_name, StringComparison.CurrentCultureIgnoreCase))
                {

                    sub_store = s;
                    break;
                }
            }

            return sub_store;

        }



        /// <summary>
        ///  小渔夫写的   
        /// 生成打印Excel文件
        /// 修改于 2017-03-13   可以打印多个子表
        /// </summary>
        /// <param name="url">模板路径</param>
        /// <param name="subStore">子表数据仓库</param>
        string CreateExcelData(string url, Store subStore)
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
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
                string mainTable = m_MainStore.Model;

                ds.Head = decipher.GetModelByPk(mainTable, pkValue);

                // 拿到主表数据
                ds.Items = subStore.GetList() as List<LModel>;


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

                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel/", fileName + ".xls");
                wFile.CreateDir();

                SheetParam sp = TemplateUtilV1.ReadTemp(path);

                //看看是不是用的模板是多子表的  
                if (sp.TempParam.DataAreaType == DataAreaType.MORE_SUB_TABLE)
                {

                    MoreSubTableDataSet ds_v1 = new MoreSubTableDataSet();

                    ds_v1.Head = m_MainStore.GetFirstData() as LModel;

                    foreach (var item in m_StoreSet)
                    {

                        if (m_MainStore.Model == item.Key)
                        {
                            continue;
                          
                        }

                        ds_v1.Items.Add(item.Key, item.Value.GetList() as LModelList<LModel>);

                    }


                    //这个可以打多个子表的  只能顺序打 打完一个子表再打下一个子表  不能合在一起打
                    TemplateUtilV1.CreateExcel(sp, ds_v1, wFile.PhysicalPath);

                }
                else
                {

                    //保存Excel文件在服务器中
                    TemplateUtilV1.CreateExcel(sp, ds, wFile.PhysicalPath);

                }
                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }



        /// <summary>
        /// 初始化附件json数据
        /// </summary>
        public string GetFileList()
        {

            int pageId = WebUtil.QueryInt("pageId", 33); //表单结构的 id

            int rowPk = WebUtil.QueryInt("row_pk");  //行主键

            int itemViewID = StringUtil.ToInt(pageId.ToString(), -1);

            if (itemViewID == -1)
            {
                return string.Empty;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();


            string tableName = m_MainTs.Table.TABLE_NAME;

            m_table_naem_json = tableName;

            m_row_id_json = rowPk.ToString();

            m_table_id_json = pageId.ToString();


            SModelList smList = decipher.GetSModelList($"select * from BIZ_FILE where ROW_SID >= 0 and TABLE_NAME = '{tableName}' and TABLE_ID = {pageId} and ROW_ID = {rowPk} and TABLE_TYPE = 'table' and TAG_CODE = 'form_annex'");

            foreach (var item in smList)
            {
                int sz = item.Get<int>("FILE_SIZE");

                item["FILE_SIZE_STR"] = NumberUtil.FormatFileSize(sz);

                string fex = item.Get<string>("FILE_EX").TrimStart('.');

                string path = $"/res/file_icon_256/{fex}.png";

                if (WebFile.Exists(path))
                {
                    item["EX_PATH"] = path;
                }
                else
                {
                    item["EX_PATH"] = "/res/file_icon_256/undefined.png";
                }

            }

            string txt = smList.ToJson();

            return txt;
        }




        class ColConfig
        {
            public int Index { get; set; }

            public string Field { get; set; }

            public int Width { get; set; }
        }


        private List<ColConfig> JsonToColCfgs(string jsonText)
        {

            JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);

            JArray jCols = (JArray)json["cols"];

            List<ColConfig> cols = new List<ColConfig>();

            foreach (JToken item in jCols)
            {
                int index = item.Value<int>("index");
                string fieldProp = item.Value<string>("field");

                int value = item.Value<int>("width");

                cols.Add(new ColConfig() { Index = index, Field = fieldProp, Width = value });
            }

            return cols;

        }
        /// <summary>
        /// 保存表单的控件位置(表单头)
        /// </summary>
        public void PostTableColSeq()
        {
            string jsonText = WebUtil.Form("table_cols_data");

            List<ColConfig> cols = JsonToColCfgs(jsonText);

            IG2_TABLE tab = m_MainTs.Table;

            DbDecipher decipher = ModelAction.OpenDecipher();

            //LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            //filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            ////filter.And("TABLE_UID", tab.TABLE_UID);
            //filter.And("IG2_TABLE_ID", tab.IG2_TABLE_ID);
            //filter.Fields = new string[] { "IG2_TABLE_ID" };

            //LModelReader reader = decipher.GetModelReader(filter);

            //int[] ids = ModelHelper.GetColumnData<int>(reader);

            int updateCount = 0;

            foreach (var col in cols)
            {
                LightModelFilter filter2 = new LightModelFilter(typeof(IG2_TABLE_COL));
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter2.And("IG2_TABLE_ID", tab.IG2_TABLE_ID);
                filter2.And("DB_FIELD", col.Field);

                decimal seq = 0.1m + ((decimal)(col.Index) / 1000m);

                updateCount += decipher.UpdateProps(filter2, new object[] {
                    "FORM_FIELD_SEQ",seq
                });
            }

            EasyClick.Web.Mini2.Toast.Show("保存排列顺序成功! " + updateCount);
        }


        /// <summary>
        /// 保存表单的控件位置(表单尾部)
        /// </summary>
        public void PostTableColSeqFooter()
        {
            string jsonText = WebUtil.Form("table_cols_data");

            List<ColConfig> cols = JsonToColCfgs(jsonText);

            IG2_TABLE tab = m_TableSet_ControlDict["footerForm1"].Table;


            DbDecipher decipher = ModelAction.OpenDecipher();

            //LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            //filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            ////filter.And("TABLE_UID", tab.TABLE_UID);
            //filter.And("IG2_TABLE_ID", tab.IG2_TABLE_ID);
            //filter.Fields = new string[] { "IG2_TABLE_ID" };

            //LModelReader reader = decipher.GetModelReader(filter);

            //int[] ids = ModelHelper.GetColumnData<int>(reader);

            int updateCount = 0;

            foreach (var col in cols)
            {
                LightModelFilter filter2 = new LightModelFilter(typeof(IG2_TABLE_COL));
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter2.And("IG2_TABLE_ID", tab.IG2_TABLE_ID);
                filter2.And("DB_FIELD", col.Field);

                decimal seq = 0.1m + ((decimal)(col.Index) / 1000m);

                updateCount += decipher.UpdateProps(filter2, new object[] {
                    "FORM_FIELD_SEQ",seq
                });
            }

            EasyClick.Web.Mini2.Toast.Show("保存排列顺序成功! " + updateCount);
        }


        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);


            //当前页面的 js 

            if (m_MainStore != null)
            {
                //if (m_MainTable == null)
                //{
                //    throw new Exception("输出脚本错误 MainTable=null ，主表不存在。");
                //}

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("$(document).ready(function(){");
                sb.AppendLine("  window.curPage = {");
                sb.AppendLine("    mainStore : " + m_MainStore.ClientID + ",");
                //sb.AppendLine("    mainTable : " + m_MainTable.ClientID + ",");
                sb.AppendLine("    mainModel : '" + m_MainStore.Model + "'");
                sb.AppendLine("  };");


                sb.AppendLine("});");

                EasyClick.Web.Mini2.ScriptManager script = EasyClick.Web.Mini2.ScriptManager.GetManager(this.Page);

                script.AddScript(sb);


            }
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

            if (insertPos == InsertPosition.FocusLast)
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
            else
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


        /// <summary>
        /// 流程设置
        /// </summary>
        public void GoFlowSetup()
        {
            int pageId = WebUtil.QueryInt("pageId");

            IG2_TABLE header = m_MainTs.Table;

            UriInfo ui = new UriInfo("/App/InfoGrid2/View/OneForm/FlowSetup.aspx");
            ui.Append("id", header.IG2_TABLE_ID );
            ui.Append("page_id", pageId);

            Window win = new Window("流程设置");
            win.ContentPath = ui.ToString();
            win.ShowDialog();
        }

        /// <summary>
        /// 流程表单权限设置
        /// </summary>
        public void GoFlowFormSetup()
        {
            int pageId = WebUtil.QueryInt("pageId");

            IG2_TABLE header = m_MainTs.Table;

            UriInfo ui = new UriInfo("/App/InfoGrid2/View/OneForm/SecFlowFormSetup.aspx");
            ui.Append("id", header.IG2_TABLE_ID);
            ui.Append("page_id", pageId);

            Window win = new Window("流程设置");
            win.ContentPath = ui.ToString();
            win.State = WindowState.Max;
            win.ShowDialog();
        }


        /// <summary>
        /// 改变某个字段的值
        /// </summary>
        /// <param name="paramStr"></param>
        public void ChangeField(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeField(paramStr, m_MainStore);

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
        public void ChangeFieldAll(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeField(paramStr, m_MainStore);

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
        public void ChangeFieldSubTable(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeField_SubTable(paramStr, m_StoreSet);

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
        public void ChangeFieldNow(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeFieldNow(paramStr, m_MainStore, m_StoreSet);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }
        }


        /// <summary>
        /// 关闭并新建
        /// </summary>
        public void CloseAndNew()
        {
            try
            {                
                EasyClick.Web.Mini2.ScriptManager.Eval($"Mini2.parentPage && Mini2.parentPage.mainStoreInsert();");
                EasyClick.Web.Mini2.ScriptManager.Eval($"ownerWindow.close()");
            }
            catch (Exception ex)
            {
                log.Error("关闭并新建错误",ex);

                MessageBox.Alert("错误: " + ex.Message);
            }
        }

    }
}