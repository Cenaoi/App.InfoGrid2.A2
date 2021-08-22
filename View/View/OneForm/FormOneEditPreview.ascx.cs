using App.BizCommon;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5;
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Xml;
using HWQ.Entity;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll;

namespace App.InfoGrid2.View.OneForm
{

    public partial class FormOneEditPreview : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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


                //初始化流程按钮
                InitFlowButton();
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

            m_table_naem_json = m_MainTs.Table.TABLE_NAME;

            m_row_id_json = rowPk.ToString();

            m_table_id_json = pageId.ToString();


            SModelList smList = decipher.GetSModelList($"select * from BIZ_FILE where ROW_SID >= 0 and TABLE_NAME = '{m_table_naem_json}' and TABLE_ID = {pageId} and ROW_ID = {rowPk} and TABLE_TYPE = 'table' and TAG_CODE = 'form_annex'");

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
        /// 大标题
        /// </summary>
        string m_alias_title;

        public string GetAliasTitle()
        {
            return m_alias_title;
        }


        /// <summary>
        /// 编辑表单的id，作为准备跳转的
        /// </summary>
        public int FormEditPageID
        {
            get;
            set;
        }

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
                    store.Inserting += Store_Inserting;
                    store.Inserted += Store_Inserted;

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



                string url = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={pageId}&edit_pageid={tab.FORM_EDIT_PAGEID}";

                if (!StringUtil.IsBlank(tab.FORM_NEW_ALIAS_TITLE))
                {
                    url += "&alias_title=" + System.Uri.EscapeUriString(tab.FORM_NEW_ALIAS_TITLE);
                }

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
            //if (m_MainTs.Table.FORM_NEW_TYPE == "ONE_FORM")
            //{
            //    LModel model = e.Object as LModel;

            //    if (model.GetModelName() == "UT_002" || model.GetModelName() == "UT_009")
            //    {
            //        model["IO_TAG"] = WebUtil.Query("io_tag").ToUpper();
            //    }
            //}

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

                if (flowSM == null)
                {
                    int bizSid = flowSM.Get<int>("BIZ_SID");

                    if(bizSid > 0)
                    {
                        e.Cancel = true;

                        Toast.Show("此记录已经'提交',无法再新建记录!");
                    }
                }
            }

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

            this.headLab.Value = "<span class='page-head' >" + m_alias_title + "</span>";

            //this.viewport1.MarginTop = model.IS_BIG_TITLE_VISIBLE ? 40 : 0;


            m_UserControls.Add(this.viewport1);



            Control con = FindControl(this.viewport1, "HanderForm1");

            InitType_Form(this.viewport1, "HanderForm1", pageId.ToString(), con);


            IG2_TABLE mainTab = m_MainTs.Table;
            this.Enclosure1.Visible = mainTab.ATTACH_FILE_VISIBLE;

            bizSID_ci.Visible = mainTab.FORM_BIG_BIZSID_VISIBLE;
            FormLayoutTop.Visible = mainTab.FORM_RT_VISIBLE;

            if (mainTab.FORM_ITEM_LABLE_WIDTH > 0)
            {
                this.HanderForm1.ItemLabelWidth = mainTab.FORM_ITEM_LABLE_WIDTH;
            }

            if(mainTab.FORM_WIDTH > 0)
            {

            }

            ProFileUpload(this.HanderForm1); //文件上传
            ProHtmlEditor(this.HanderForm1); //富文本框


            this.bizSID_ci.DataSource = m_MainStore.ID;
            this.FormLayoutTop.StoreID = m_MainStore.ID;


            Button2.Text = WebUtil.QueryBool("design_mode") ? "切换为-正常模式" : "切换为-设计模式";

            InitCustomPage(model.SERVER_CLASS);


        }



        public void StepEdit3()
        {
            int pageId = WebUtil.QueryInt("pageId");
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit3.aspx?id=" + pageId, $"列设置-Table={pageId}");
        }

        public void StepEdit4()
        {
            int pageId = WebUtil.QueryInt("pageId");
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit4.aspx?id=" + pageId, $"高级设置-Table={pageId}");
        }

        public void StepEdit5_DialogMode()
        {
            int pageId = WebUtil.QueryInt("pageId");
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit5_DialogMode.aspx?id=" + pageId, $"模式窗口设置-Table={pageId}");
        }


        /// <summary>
        /// 处理文件上传控件
        /// </summary>
        private void ProFileUpload(Panel parent)
        {
            List<FileUpload> fileUploads = parent.FindBy<FileUpload>();

            if(fileUploads.Count == 0)
            {
                return;
            }

            int row_pk = WebUtil.QueryInt("row_pk");

            IG2_TABLE tab = m_MainTs.Table;

            UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormHandle.aspx");
            uri.Append("method", "image_upload");
            uri.Append("table", tab.TABLE_NAME);
            uri.Append("item_id", row_pk);


            foreach (var item in fileUploads)
            {
                item.FileUrl = uri.ToString();
            }

        }
        

        /// <summary>
        /// 处理富文本框
        /// </summary>
        /// <param name="parent"></param>
        private void ProHtmlEditor(Panel parent)
        {
            List<HtmlEditor> htmlEditors = parent.FindBy<HtmlEditor>();

            if (htmlEditors.Count == 0)
            {
                return;
            }

            int row_pk = WebUtil.QueryInt("row_pk");

            IG2_TABLE tab = m_MainTs.Table;

            UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormHandle.aspx");
            uri.Append("method", "image_upload");
            uri.Append("table", tab.TABLE_NAME);
            uri.Append("item_id", row_pk);


            foreach (var item in htmlEditors)
            {
                item.ImageUrl = uri.ToString();
            }

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

        /// <summary>
        /// 数据集
        /// </summary>
        SortedList<int, TableSet> m_TableSet_Dict = new SortedList<int, TableSet>();

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


            m_TableSet_Dict[view.IG2_TABLE_ID] = tSet;  //索引

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
                    //store.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(string.Format(
                    //    "(BIZ_SID is null OR BIZ_SID <= {0})", view.DELETE_BY_BIZ_SID)));

                    Param pm = new Param("BIZ_SID");
                    pm.SetInnerValue(view.DELETE_BY_BIZ_SID);
                    pm.Logic = "<=";

                    store.DeleteQuery.Add(pm);
                }


                //store.FilterParams.Add(new Param("ROW_SID", "0", DbType.Int32) );

                //store.InsertParams.Add("ROW_SID", -1);


                #region [Store] 删除进回收站模式


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

                table1.Command += Table1_Command;

                return table1;
            }


        }






        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            Console.WriteLine("命令: " + e.CommandName);
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


            LModelElement modelElem = LModelDna.GetElementByName(view.TABLE_NAME);

            Store store = GetStoreByTable(view.TABLE_NAME);

            if (m_MainStore == null)
            {
                int rowPk = WebUtil.QueryInt("row_pk");

                if (modelElem.HasField("ROW_IDENTITY_ID"))
                {
                    store.FilterParams.Add(new Param("ROW_IDENTITY_ID", rowPk.ToString()));
                }
                else
                {
                    store.FilterParams.Add(new Param(modelElem.PrimaryKey, rowPk.ToString()));
                }

                m_MainStore = store;

                m_MainStore.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(m_MainStore_CurrentChanged);

                m_MainTs = tSet;

            }

            if (!StringUtil.IsBlank(view.LOCKED_FIELD) && StringUtil.IsBlank(store.LockedField))
            {
                store.LockedField = view.LOCKED_FIELD;
            }


            if (!StringUtil.IsBlank(view.LOCKED_RULE) && StringUtil.IsBlank(store.LockedRule))
            {
                store.LockedRule = view.LOCKED_RULE;
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



            TableSet newTSet = m_SecUiFty.FilterTableSet("FORM", m_SecTag, id, tSet);

            if (newTSet != null)
            {
                tSet = newTSet;
            }

            if (tSet != null && tSet.Table != null && !StringUtil.IsBlank(tSet.Table.LOCKED_RULE) && StringUtil.IsBlank(store.LockedRule))
            {
                store.LockedRule = tSet.Table.LOCKED_RULE;
            }

            formFty.CreateFormControls(form1, tSet);



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
            }


        }


        /// <summary>
        /// 保存
        /// </summary>
        public void GoSave()
        {
            int pk = WebUtil.QueryInt("row_pk");

            int edit_pageId = WebUtil.QueryInt("edit_pageid");

            string modelName = m_MainStore.Model;

            LModelElement modelElem = LightModel.GetLModelElement(modelName);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(modelName);
            filter.And(modelElem.PrimaryKey, pk);
            filter.And("ROW_SID", -1);
            filter.Fields = new string[] { modelElem.PrimaryKey, "ROW_SID" };

            LModel model = decipher.GetModel(filter);

            if (model == null)
            {
                return;
            }


            int count = 0;

            count = decipher.UpdateModelByPk(m_MainStore.Model, pk, new object[] { "ROW_SID", 0 });


            foreach (var item in m_JoinMainFields)
            {
                LightModelFilter subFilter = new LightModelFilter(item.Key);
                subFilter.And(item.Value, pk);
                subFilter.And("ROW_SID", -1);

                decipher.UpdateProps(subFilter, new object[] { "ROW_SID", 0 });

            }

            //string alias_title = this.tit ;

            string alasTitleStr = "";

            if (!StringUtil.IsBlank(m_alias_title))
            {
                alasTitleStr = "&alias_title=" + System.Uri.EscapeUriString(m_alias_title);
            }

            UriBuilder ub = new UriBuilder();



            string editUrl = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={edit_pageId}" + alasTitleStr;

            MiniPager.Redirect(editUrl);



        }


        /// <summary>
        /// 业务状态 0 -> 2
        /// </summary>
        public void GoBizSID_0_2()
        {
            int rowPk = WebUtil.QueryInt("row_pk");

            LModel model = this.m_MainStore.GetFirstData() as LModel;

            if (model != null)
            {
                LModelElement modelElem = model.GetModelElement();


                EasyClick.Web.Mini.MiniHelper.Eval("alert('提交成功');");

            }

        }


        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        public void ChangeBizSID(string psStr)
        {
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

        }



        /// <summary>
        /// 主表数据 输出到打印机
        /// </summary>
        /// <param name="paramJson"></param>
        public void ToPrint(string paramJson)
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
                ds.Items = m_MainStore.GetList() as List<LModel>;


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
        /// 保存表单的控件位置
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



        #region 流程的代码



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



            this.DocUrl = Base64Util.ToString($"/App/InfoGrid2/View/OneForm/FormOneEditPreview.aspx?row_pk={this.MainRowId}&pageId={this.MainPageId}&menu_id={this.MenuId}&alias_title={this.GetAliasTitle()}");

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



        #endregion


        #region 二次叠加过滤


        private void ProSec2(IG2_TABLE view, FormLayout fromLayout1, Store store)
        {

            int pageId = WebUtil.QueryInt("pageid");
            int rowPk = WebUtil.QueryInt("row_pk");

            IG2_TABLE mainTable = m_MainTs.Table;

            LModelElement modelElem = LModelDna.GetElementByName(mainTable.TABLE_NAME);

            if (!modelElem.HasField("BIZ_FLOW_DEF_CODE"))
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

            string[] secVFields = GetSecTFN_Fields(pageId, mainTable.TABLE_NAME, view.TABLE_NAME, flowCode, flowNodeCode);

            if (secVFields != null && secVFields.Length > 0)
            {
                foreach (var secVField in secVFields)
                {
                    FieldBase fb = fromLayout1.FindByDbField(secVField) as FieldBase;

                    if (fb == null)
                    {
                        continue;
                    }

                    fb.ReadOnly = false;
                }

                store.LockedExclusionFields = secVFields;
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

            LModelElement modelElem = LModelDna.GetElementByName(mainTable.TABLE_NAME);

            if (!modelElem.HasField("BIZ_FLOW_DEF_CODE"))
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

            string[] secVFields = GetSecTFN_Fields(pageId, mainTable.TABLE_NAME, view.TABLE_NAME, flowCode, flowNodeCode);

            if (secVFields != null && secVFields.Length > 0)
            {
                foreach (var secVField in secVFields)
                {
                    BoundField bf = table1.Columns.FindByDataField(secVField);

                    bf.EditorMode = EditorMode.Auto;
                }

                store.LockedExclusionFields = secVFields;
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
        private string[] GetSecTFN_Fields(int pageId, string dbTable, string secTable, string flowCode, string flowNodeCode)
        {
            SEC_TABLE_FLOW_NODE tfn = GetSecTFN(pageId, dbTable, flowCode, flowNodeCode);

            if (tfn == null)
            {
                return null;
            }

            string fields = null;

            if (secTable == tfn.T_HEADER)
            {
                fields = tfn.T_HEADER_V_FIELDS;
            }
            else if (secTable == tfn.T_FOOTER)
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

                    if (value == secTable)
                    {
                        fields = (string)tfn[$"T_SUB_{i}_V_FIELDS"];
                        break;
                    }
                }

            }

            if (fields == null)
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
        /// 流程设置
        /// </summary>
        public void GoFlowSetup()
        {
            int pageId = WebUtil.QueryInt("pageId");

            IG2_TABLE header = m_MainTs.Table;

            UriInfo ui = new UriInfo("/App/InfoGrid2/View/OneForm/FlowSetup.aspx");
            ui.Append("id", header.IG2_TABLE_ID);
            ui.Append("page_id", pageId);

            Window win = new Window("流程设置");
            win.ContentPath = ui.ToString();
            win.ShowDialog();
        }

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
                ResultBase result = BizHelper.ChangeFieldNow(paramStr, m_MainStore , m_StoreSet);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }
        }

    }
}