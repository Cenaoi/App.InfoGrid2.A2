using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5;
using EC5.BizLogger;
using EC5.DbCascade;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.IG2.Core.UI;
using EC5.IG2.Plugin;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
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

namespace App.InfoGrid2.View.OneTable
{
    public partial class TablePreview : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TableSet m_TableSet;
        
        LModelElement m_ModelElem = null;


        /// <summary>
        /// 表名
        /// </summary>
        string m_TableName;

        /// <summary>
        /// 
        /// </summary>
        int m_TableId = 0;

        IG2_TABLE m_Table;

        string m_SecTag;

        int m_MenuId = 0;


        Store m_MainStore;

        Table m_MainTable;

        Toolbar m_MainToolbar;

        string m_AliasTitle;

        /// <summary>
        /// 弹出编辑表单的别名
        /// </summary>
        public string FromEditAliasTitle { get; set; }

        public string AliasTitle
        {
            get { return m_AliasTitle; }
            set { m_AliasTitle = value; }
        }


        /// <summary>
        /// 修改的表单类型 ONE_FORM | TABLE_FORM
        /// </summary>
        public string FormEditType { get; set; } = "ONE_FORM";


        /// <summary>
        /// 表单页的 ID
        /// </summary>
        public int FormEditPageID { get; set; }



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

        protected override void OnInit(EventArgs e)
        {

            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            m_MainStore = this.store1;
            m_MainTable = this.table1;
            m_MainToolbar = this.Toolbar1;



            DbDecipher decipher = ModelAction.OpenDecipher();

            m_TableId = WebUtil.QueryInt("id");

            m_SecTag = WebUtil.Query("sec_tag");
            m_MenuId = WebUtil.QueryInt("menu_id");

            m_TableSet = TableSet.SelectByPk(decipher, m_TableId);

            if (m_TableSet == null)
            {
                throw new Exception(string.Format("找不到 TableSet ： TableID={0} 的记录集合。", m_TableId));
            }

            m_Table = m_TableSet.Table;
            m_TableName = m_Table.TABLE_NAME;

            this.FormEditType = m_Table.FORM_EDIT_TYPE;
            this.FormEditPageID = m_Table.FORM_EDIT_PAGEID;

            string alias_title = WebUtil.Query("alias_title");

            this.AliasTitle = StringUtil.NoBlank(alias_title, m_Table.DISPLAY);

            HeadPanel.Visible = m_Table.IS_BIG_TITLE_VISIBLE;
            this.headLab.Value = "<span class='page-head' >" + this.AliasTitle + "</span>";

            this.viewport1.MarginTop = m_Table.IS_BIG_TITLE_VISIBLE ? 40 : 0;



            m_ModelElem = TableMgr.GetModelElem(m_TableSet);


            //this.tableNameTB2.Text = m_Table.DISPLAY;

            //初始化数据仓库
            IntiStoreAttrs();

            if (m_ModelElem == null)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("数据表不存在");
                return;
            }

            //创建界面UI
            CreateControl(m_TableSet);

            if (!StringUtil.IsBlank(m_Table.LOCKED_FIELD) && StringUtil.IsBlank(m_MainStore.LockedField))
            {
                m_MainStore.LockedField = m_Table.LOCKED_FIELD;
            }

            M2SecurityUiFactory secUiFactory = new M2SecurityUiFactory();
            secUiFactory.InitSecUI(m_TableId, "TABLE", m_MenuId, m_TableSet);
            secUiFactory.Filter("", m_SecTag, "", this.Toolbar1, this.table1, this.store1);

            secUiFactory.FilterForForm("", m_SecTag, "", this.searchForm, this.store1);


            m_UserControls.AddRange(new List<Control>()
            {
                this.searchForm,this.Toolbar1,this.store1,this.table1
            });


            try
            {
                InitCustomPage(m_Table.SERVER_CLASS);
            }
            catch (Exception ex)
            {
                log.Error("加载自定义类错误:" + m_Table.SERVER_CLASS, ex);
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
                log.ErrorFormat("调用自定义类 OnInit 错误", ex);
            }

        }


        ExPage m_CustomPage;

        List<Control> m_UserControls = new List<Control>();

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

            m_CustomPage.SetDefaultValue(this, this.searchForm,this.store1,this.Toolbar1, this.table1);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";

            viewport1.Controls.Add(m_CustomPage);
        }


        /// <summary>
        /// 初始化数据库仓库的属性
        /// </summary>
        private void IntiStoreAttrs()
        {
            LModelElement modelElem = LightModel.GetLModelElement(m_TableName);

            this.store1.Model = m_TableName;
            this.store1.IdField = m_Table.ID_FIELD;
            this.store1.SortField = m_Table.USER_SEQ_FIELD;
            this.store1.SortText = m_Table.SORT_TEXT;


            this.store1.StringFields = GetFieldsArray(m_TableSet);
            
            this.store1.DeleteQuery.Add(new ControlParam(m_Table.ID_FIELD, this.table1.ID, "CheckedRows"));

            this.store1.Inserting += Store1_Inserting;
            this.store1.Inserted += Store1_Inserted;

            //针对 BIZ_SID 这个字段特殊处理
            if (modelElem.Fields.ContainsField("BIZ_SID"))
            {
                this.store1.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(
                    $"(BIZ_SID is null OR BIZ_SID <= {m_Table.DELETE_BY_BIZ_SID})"));
            }


            #region 设置创建行的默认值

            foreach (IG2_TABLE_COL col in m_TableSet.Cols)
            {
                if (col.ROW_SID == -3 || StringUtil.IsBlank(col.DEFAULT_VALUE) || col.SEC_LEVEL > 6)
                {
                    continue;
                }

                Param pa = new Param(col.DB_FIELD, col.DEFAULT_VALUE);

                this.store1.InsertParams.Add(pa);
            }

            #endregion

            #region 设置更改行的参数

            if (modelElem.HasField("ROW_DATE_UPDATE"))
            {
                this.store1.UpdateParams.Add(new ServerParam("ROW_DATE_UPDATE", "TIME_NOW"));
            }

            #endregion


            #region 删除进回收站模式

            this.store1.DeleteRecycle = m_Table.DELETE_RECYCLE;

            if (m_Table.DELETE_RECYCLE)
            {
                store1.FilterParams.Add(new Param("ROW_SID", "-1", DbType.Int32) { Logic = ">=" }); ;

                Param drP1 = new Param("ROW_SID", "-3", System.Data.DbType.Int32);
                ServerParam drP2 = new ServerParam("ROW_DATE_DELETE", "TIME_NOW");

                store1.DeleteRecycleParams.Add(drP1);
                store1.DeleteRecycleParams.Add(drP2);
            }

            #endregion


            #region 权限-结构过滤

            IG2_TABLE srcTab = m_Table;   //最原始的数据表

            if ( m_Table.TABLE_TYPE_ID != "TABLE")
            {
                srcTab = TableMgr.GetTableForName(m_Table.TABLE_NAME);
            }

            if (srcTab.SEC_STRUCT_ENABLED)
            {
                UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                if (uSec != null)
                {
                    Param cataPs = new Param("BIZ_CATA_CODE");
                    cataPs.SetInnerValue(uSec.ArrCatalogCode);
                    cataPs.Logic = "in";

                    this.store1.FilterParams.Add(cataPs);
                }
            }

            #endregion


            M2SecurityDataFactory secDataFactory = new M2SecurityDataFactory();
            secDataFactory.BindStore(this.store1);


            //M2ValidateFactory validFactory = new M2ValidateFactory();
            //validFactory.BindStore(this.store1);

           
            DbCascadeRule.Bind(this.store1);

        }


        #region 临时测试代码, 后面正式使用的时候取消，改为另外一种模式

        private void Store1_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            if (m_Table.FORM_NEW_TYPE == "ONE_FORM")
            {
                LModel model = e.Object as LModel;

                object pk = model.GetPk();
                int pageId = m_Table.FORM_NEW_PAGEID;



                string url = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={pageId}&edit_pageid={m_Table.FORM_EDIT_PAGEID}";

                string title = "";
                
                title = StringUtil.NoBlank(m_Table.FORM_NEW_ALIAS_TITLE, m_Table.DISPLAY);

                
                if (!StringUtil.IsBlank(title))
                {
                    url += "&alias_title=" + System.Uri.EscapeUriString(title);
                }

                url += "&form_edit_pageID=" + m_Table.FORM_EDIT_PAGEID;

                EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','"+ title + "-新建');");
            }


        }

        private void Store1_Inserting(object sender, ObjectCancelEventArgs e)
        {
            if(m_Table.FORM_NEW_TYPE == "ONE_FORM")
            {
                LModel model = e.Object as LModel;
                
                if(model.GetModelName() == "UT_001" || model.GetModelName() == "UT_008")
                {
                    model["IO_TAG"] = WebUtil.Query("io_tag").ToUpper();
                }
            }
        }

        #endregion


        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            LogStepMgr.Insert(e.Steps[0],e.OpText, e.Remark);
        }


        /// <summary>
        /// 输出到打印机
        /// 小渔夫
        /// 修改于 2016-12-12
        /// </summary>
        public void ToPrint()
        {
            //string plugClass = "EC5.IG2.Plugin.Custom.PrintPlugin";
            //string plugMethod = "PrintExcel";

            //Type plugT = PluginManager.Get(plugClass);

            //if (plugT == null)
            //{
            //    MessageBox.Alert("插件不存在 . " + plugClass);
            //    return;
            //}

            //MethodInfo mi = plugT.GetMethod(plugMethod);

            //if (mi == null)
            //{
            //    MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
            //    return;
            //}


            //try
            //{
            //    PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

            //    inter.ClassName = plugClass;
            //    inter.Method = plugMethod;
            //    inter.Params = "{}";

            //    inter.SrcUrl = this.Request.Url.ToString();

            //    inter.Main = this;
            //    inter.MainStore = this.store1;// this.m_MainStore;
            //    inter.MainTable = this.table1;// this.m_MainTable;

            //    inter.SrcStore = this.store1;
            //    inter.SrcTable = this.table1;

            //    mi.Invoke(inter, null);
            //}
            //catch (Exception ex)
            //{
            //    log.Error("执行插件函数错误。", ex);

            //    MessageBox.Alert("执行插件函数错误:" + ex.Message);
            //}




            int pkValue = StringUtil.ToInt(this.m_MainStore.CurDataId);
            int pageId = WebUtil.QueryInt("Id");


            string mainTable = m_MainStore.Model;
            string mainPk = m_MainStore.IdField;


            UriInfo uri = new UriInfo("/App/InfoGrid2/View/PrintTemplate/PrintTemplateSingleTable.aspx");
            uri.Append("id", pageId);
            uri.Append("mainTableID", pkValue);  //主数据记录的ID
            uri.Append("pageID", pageId);

            uri.Append("mainTable", mainTable);    //表名
            uri.Append("mainPK", mainPk);

            string urlStr = uri.ToString();

            Window win = new Window("打印");
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.ContentPath = urlStr;

            win.WindowClosed += Win_WindowClosed;


            win.ShowDialog();


        }

        /// <summary>
        /// 选择打印机和模板界面关闭事件
        /// 小渔夫 
        /// 创建于 2016-12-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">关闭界面传回来的数据</param>
        public void Win_WindowClosed(object sender, string data)
        {

            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            SModel sm = SModel.ParseJson(data);

            if(sm.Get<string>("result") != "ok")
            {
                return;
            }


            string ids = sm.Get<string>("ids");



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
        /// 修改于 2016-12-12
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
                ds.Items = store1.GetList() as List<LModel>;


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

                TemplateUtilV1.CreateExcel(sp, ds, wFile.PhysicalPath);


                //NOPIHandlerEX handler = new NOPIHandlerEX();

                //SheetPro sp = handler.ReadExcel(path);

                //handler.InsertSubData(sp, ds);

                ////保存Excel文件在服务器中
                //handler.WriteExcel(sp, wFile.PhysicalPath);
                //handler.Dispose();


                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
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

                tab = FindControl(this.viewport1, tableId) as Table;
                store = FindControl(this.viewport1, storeId) as Store;

                psStr = $"{sp[0]},{sp[1]}";
            }

            try
            {
                ResultBase result = BizHelper.ChangeBizSID(psStr, m_MainTable, m_MainStore, m_TableSet );

                Toast.Show(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.",ex);
                MessageBox.Alert("错误:" + ex.Message);
            }

            
        }

        /// <summary>
        /// 改变所有记录的 Biz 业务状态
        /// </summary>
        /// <param name="psStr"></param>
        public void ChangeBizSIDALL(string psStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeBizSIDAll(psStr, m_MainStore);

                MessageBox.Alert(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段错误", ex);

                MessageBox.Alert("错误:" + ex.Message);
            }
        }



        /// <summary>
        /// 改变某个字段的值
        /// 小渔夫 
        /// 新增于2018-03-23
        /// </summary>
        /// <param name="paramStr"></param>
        public void ChangeFieldNow(string paramStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeFieldNow(paramStr, table1, store1, null);

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



        /// <summary>
        /// 导出 Excel 
        /// </summary>
        public void ToExcel()
        {
            string plugClass = "EC5.IG2.Plugin.Custom.InputOutExcelPlugin";
            string plugMethod = "InputOut";

            Type plugT = PluginManager.Get(plugClass);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + plugClass);
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
            }


            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                PropertyInfo pi = plugT.GetProperty("IncludeFieldName");

                pi.SetValue(inter, true, null);


                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = "{}";

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = this.store1;
                inter.SrcTable = this.table1;

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }

        }

        private string GetFieldsArray(TableSet tableSet)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            

            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {

                if (col.ROW_SID == -3 && col.DB_FIELD != m_Table.ID_FIELD )
                {
                    continue;
                }

                if (i++ > 0)
                {
                    sb.Append(",");
                }

                sb.Append(col.DB_FIELD);
                
            }

            return sb.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
                log.ErrorFormat("调用自定义类 OnInit 错误", ex);
            }

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }



        /// <summary>
        /// 创建界面控件
        /// </summary>
        private void CreateControl( TableSet tableSet)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            ToolbarSet toolbarSet = new ToolbarSet();
            toolbarSet.SelectForTable(decipher, tableSet.Table.IG2_TABLE_ID);


            M2ToolbarFactory toolbarFty = new M2ToolbarFactory();
            toolbarFty.TableId = m_MainTable.ID;
            toolbarFty.StoreId = m_MainStore.ID;
            toolbarFty.CreateItems(this.Toolbar1, toolbarSet);


            M2TableFactory tableFty = new M2TableFactory(this.IsPostBack);
            tableFty.CreateTableColumns(this.table1,this.store1, tableSet);


            M2SearchFormFactory searchFty = new M2SearchFormFactory(this.IsPostBack,this.store1.ID);
            searchFty.CreateControls(this.searchForm, tableSet);
        }




        /// <summary>
        /// 修改列.
        /// 需要存到临时表,然后才开始修改
        /// </summary>
        public void StepEdit2()
        {
            string sessionId = this.Session.SessionID;

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tableSet = TableSet.Select(decipher, m_TableId);


            Guid tmpId = Guid.NewGuid();

            IList<IG2_TMP_TABLECOL> tmpCols = new List<IG2_TMP_TABLECOL>();

            IG2_TMP_TABLE tmpTable = new IG2_TMP_TABLE();
            tableSet.Table.CopyTo(tmpTable, true);

            tmpTable.TMP_GUID = tmpId;
            tmpTable.TMP_OP_ID = "";
            tmpTable.TMP_SESSION_ID = sessionId;

            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {
                if (col.ROW_SID == -3)
                {
                    continue;
                }

                IG2_TMP_TABLECOL tmpCol = new IG2_TMP_TABLECOL();
                col.CopyTo(tmpCol, true);

                tmpCol.TMP_GUID = tmpId;
                tmpCol.TMP_OP_ID = string.Empty;
                tmpCol.TMP_SESSION_ID = sessionId;

                tmpCols.Add(tmpCol);
            }



            try
            {
                decipher.InsertModel(tmpTable);
                decipher.InsertModels(tmpCols);


                string urlStr = $"/App/InfoGrid2/View/OneTable/StepEdit2.aspx?id={m_TableId}&tmp_id={tmpId}";

                EcView.Show(urlStr, $"修改列 - Table={m_TableId}");

            }
            catch (Exception ex)
            {
                log.Error(ex);

                MessageBox.Alert("无法修改。");
            }
        }

        public void StepEdit3()
        {
            string urlStr = $"/App/InfoGrid2/View/OneTable/StepEdit3.aspx?id={m_TableId}";

            EcView.Show(urlStr, $"列设置 - Table={m_TableId}");
            
        }

        public void StepEdit4()
        {
            string urlStr = $"/App/InfoGrid2/View/OneTable/StepEdit4.aspx?id={m_TableId}";

            EcView.Show(urlStr, $"高级设置 - Table={m_TableId}");
        }

        public void Search()
        {
            this.store1.LoadPage(0);

        }

        /// <summary>
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("TABLE_ID", m_TableId);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);


            string urlStr;

            if (toolbarId > 0)
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={0}&table_id={1}", toolbarId, m_TableId);
            }
            else
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={0}", m_TableId);
            }

            EcView.Show(urlStr, $"工具栏设置 - Table={m_TableId}");
        }


        /// <summary>
        /// 联动设置
        /// </summary>
        public void GoActList()
        {
            string src_url = string.Format("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id={0}", m_TableId);
            string urlCode = Base64Util.ToString(src_url, Base64Mode.Http);


            string urlStr = string.Format("/app/infogrid2/view/OneAction/ActList.aspx?l_table={0}&src_url={1}", m_TableName, urlCode);

            EcView.Show(urlStr, $"联动设置 - Table={m_TableId}");

        }

        /// <summary>
        /// 验证设置
        /// </summary>
        public void GoValidSetup()
        {
            string src_url = string.Format("/App/InfoGrid2/View/OneTable/TablePreview.aspx?id={0}", m_TableId);
            string urlCode = Base64Util.ToString(src_url, Base64Mode.Http);
            
            string urlStr = string.Format("/App/InfoGrid2/View/OneValid/ValidStepEdit2.aspx?id={0}&src_url={1}", m_TableId, urlCode);

            EcView.Show(urlStr, $"验证设置 - Table={m_TableId}");
        }







        #region 插件



        /// <summary>
        /// 执行插件
        /// </summary>
        /// <param name="id"></param>
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


            storeUi = this.store1;// this.FindControl(storeId) as Store;

            //if (storeUi == null)
            //{
            //    MessageBox.Alert("数据仓库 " + storeId + " 不存在.");
            //    return;
            //}

            tableUi = this.table1;// this.FindControl(tableId) as Table;

            //if (tableUi == null)
            //{
            //    MessageBox.Alert("表格 " + tableId + " 不存在.");
            //    return;
            //}


            Type plugT = PluginManager.Get(tItem.PLUG_CLASS);

            if (plugT == null)
            {
                MessageBox.Alert("插件不存在 . " + tItem.PLUG_CLASS);
            }

            MethodInfo mi = plugT.GetMethod(tItem.PLUG_METHOD);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + tItem.PLUG_CLASS + ", " + tItem.PLUG_METHOD);
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = tItem.PLUG_CLASS;
                inter.Method = tItem.PLUG_METHOD;
                inter.Params = tItem.PLUG_PARAMS;

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

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

            string plugJson = Base64Util.FromString( plug, Base64Mode.Http);

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
            }

            MethodInfo mi = plugT.GetMethod(plugMethod);

            if (mi == null)
            {
                MessageBox.Alert("插件不存在此函数名." + plugClass + ", " + plugMethod);
            }



            try
            {
                PagePlugin inter = Activator.CreateInstance(plugT) as PagePlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = plugParam;

                inter.SrcUrl = src_url;

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

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

        #endregion

        /// <summary>
        /// 获取展示规则
        /// </summary>
        /// <returns></returns>
        public string GetDisplayRule()
        {
            return App.InfoGrid2.Bll.DisplayRuleMgr.GetJScript();
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

                cols.Add(new ColConfig() { Index=index, Field = fieldProp, Width = value });
            }

            return cols;

        }


        /// <summary>
        /// 提交当前表格列的尺寸
        /// </summary>
        public void PostTableColWidth()
        {
            string jsonText = WebUtil.Form("table_cols_data");

            List<ColConfig> cols = JsonToColCfgs(jsonText);

            IG2_TABLE tab = m_Table;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_UID", m_Table.TABLE_UID);
            filter.Fields = new string[] { "IG2_TABLE_ID"};

            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);


            foreach (var col in cols)
            {
                LightModelFilter filter2 = new LightModelFilter(typeof(IG2_TABLE_COL));
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter2.And("IG2_TABLE_ID", ids, Logic.In);
                filter2.And("DB_FIELD", col.Field);


                decipher.UpdateProps(filter2, new object[] {
                    "DISPLAY_LEN", col.Width
                });
            }

            Toast.Show("保存宽度成功!");
        }


        /// <summary>
        /// 提交当前表格列的顺序
        /// </summary>
        public void PostTableColSeq()
        {
            string jsonText = WebUtil.Form("table_cols_data");

            List<ColConfig> cols = JsonToColCfgs(jsonText);

            IG2_TABLE tab = m_Table;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_UID", m_Table.TABLE_UID);
            filter.Fields = new string[] { "IG2_TABLE_ID" };

            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);


            
            foreach (var col in cols)
            {
                LightModelFilter filter2 = new LightModelFilter(typeof(IG2_TABLE_COL));
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter2.And("IG2_TABLE_ID", ids, Logic.In);
                filter2.And("DB_FIELD", col.Field);


                decipher.UpdateProps(filter2, new object[] {
                    "FIELD_SEQ", 0.1m + ((decimal)(col.Index ) / 100m) 
                });
            }

            Toast.Show("保存排列顺序成功!");
        }
    }
}