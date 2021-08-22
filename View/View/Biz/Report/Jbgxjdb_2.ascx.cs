using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.JsonModel;
using App.InfoGrid2.View.OneTable;
using System.Data;
using EasyClick.BizWeb2;
using EC5.IG2.Core.UI;
using EC5.IG2.BizBase;
using HWQ.Entity.Filter;
using Newtonsoft.Json.Linq;
using EC5.IG2.Plugin;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using App.InfoGrid2.Excel_Template;
using EC5.IG2.Plugin.Custom;

namespace App.InfoGrid2.View.Biz.Report
{
    public partial class Jbgxjdb_2 : WidgetControl, IView
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

        ViewSet m_ViewSet;

        /// <summary>
        /// 修改的表单类型 ONE_FORM | TABLE_FORM
        /// </summary>
        public string FormEditType { get; set; } = "ONE_FORM";

        public int FormEditPageID { get; set; }

        public string FromEditAliasTitle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }




        protected override void OnInitCustomControls(EventArgs e)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            m_TableId = WebUtil.QueryInt("id");

            m_TableSet = TableSet.Select(decipher, m_TableId);

            m_Table = m_TableSet.Table;
            m_TableName = m_Table.TABLE_NAME;

            string title = StringUtil.NoBlank(WebUtil.Query("alias_title"), m_Table.DISPLAY);

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


            this.FormEditType = m_Table.FORM_EDIT_TYPE;

            this.FormEditPageID = m_Table.FORM_EDIT_PAGEID;

            this.FromEditAliasTitle = m_Table.FORM_EDIT_ALIAS_TITLE;

            //this.tableNameTB1.Text = m_Table.DISPLAY;

            if (m_TableName.StartsWith("UV_"))
            {
                InitStoreAttrs_ForMoreView();
            }
            else
            {
                m_ModelElem = TableMgr.GetModelElem(m_TableSet);
                InitStoreAttrs_ForOneTable();
            }

            IntiStoreAttrs();   //初始化数据库仓库的属性

            if (m_ModelElem == null)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("数据表不存在");
                return;
            }


            CreateControl(m_TableSet);


            try
            {
                InitCustomPage(m_Table.SERVER_CLASS);
            }
            catch (Exception ex)
            {
                log.Error("加载自定义类错误:" + m_Table.SERVER_CLASS, ex);
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

            m_CustomPage.SetDefaultValue(this, this.searchForm, this.store1, this.Toolbar1, this.table1);
            m_CustomPage.UserControls = m_UserControls;
            m_CustomPage.ID = "ExPage";

            viewport1.Controls.Add(m_CustomPage);
        }


        /// <summary>
        /// 初始化数据库仓库的属性
        /// </summary>
        private void IntiStoreAttrs()
        {
            this.store1.Inserted += Store1_Inserted;
        }

        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {
            if (m_Table.FORM_NEW_TYPE == "ONE_FORM")
            {
                LModel model = e.Object as LModel;

                object pk = model.GetPk();
                int pageId = m_Table.FORM_NEW_PAGEID;

                int menuId = WebUtil.QueryInt("menu_id");


                //string url = $"/App/InfoGrid2/View/OneForm/FormEditPreview.aspx?row_pk={pk}&pageId={pageId}&edit_pageid={m_Table.FORM_EDIT_PAGEID}&menu_Id={menuId}";

                string title = "";

                title = StringUtil.NoBlank(m_Table.FORM_NEW_ALIAS_TITLE, m_Table.DISPLAY);


                UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormEditPreview.aspx");
                uri.Append("row_pk", pk);
                uri.Append("pageId", pageId);
                uri.Append("edit_pageid", m_Table.FORM_EDIT_PAGEID);
                uri.Append("menu_Id", menuId);
                uri.Append("form_edit_pageID", m_Table.FORM_EDIT_PAGEID);

                if (!StringUtil.IsBlank(title))
                {
                    uri.Append("alias_title", System.Uri.EscapeUriString(title));
                }

                string url = uri.ToString();

                EcView.Show(url, title + "-新建");
            }
            else if (m_Table.FORM_NEW_TYPE == "TABLE_FORM")
            {
                LModel model = e.Object as LModel;

                object pk = model.GetPk();
                int pageId = m_Table.FORM_NEW_PAGEID;

                int menuId = WebUtil.QueryInt("menu_id");

                UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormOneEditPreview.aspx");
                uri.Append("row_pk", pk);
                uri.Append("pageId", pageId);
                uri.Append("edit_pageid", m_Table.FORM_EDIT_PAGEID);
                uri.Append("menu_Id", menuId);
                uri.Append("form_edit_pageID", m_Table.FORM_EDIT_PAGEID);

                string title = StringUtil.NoBlank(m_Table.FORM_NEW_ALIAS_TITLE, m_Table.DISPLAY);

                if (!StringUtil.IsBlank(title))
                {
                    uri.Append("alias_title", System.Uri.EscapeUriString(title));
                }

                EcView.Show(uri.ToString(), title + "-新建");
            }
        }


        /// <summary>
        /// 关联视图结构
        /// </summary>
        private void InitStoreAttrs_ForMoreView()
        {
            int id = WebUtil.QueryInt("id", 2);


            DbDecipher decipher = ModelAction.OpenDecipher();

            m_ViewSet = ViewSet.Select(decipher, id);


            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);


            LModelElement modelElem = ViewMgr.GetModelElem(m_ViewSet);

            this.store1.Model = modelElem.DBTableName;
            this.store1.IdField = m_ViewSet.View.MAIN_TABLE_NAME + "_" + m_ViewSet.View.MAIN_ID_FIELD;



            M2TableFactory tableFactory = new M2TableFactory(this.IsPostBack);

            tableFactory.CreateTableColumns(this.table1, this.store1, tSet);

        }

        /// <summary>
        /// 普通单表结构
        /// </summary>
        private void InitStoreAttrs_ForOneTable()
        {

            this.store1.Model = m_TableName;
            this.store1.IdField = m_Table.ID_FIELD;


            if (!StringUtil.IsBlank(m_Table.USER_SEQ_FIELD))
            {
                this.store1.SortField = m_Table.USER_SEQ_FIELD;
            }
            else
            {
                this.store1.SortText = m_Table.ID_FIELD + " ASC";
            }


            this.store1.StringFields = GetFieldsArray(m_TableSet);


            #region  二次过滤

            string filter2 = WebUtil.QueryBase64("filter2");
            List<Param> psFor_Filter2 = GetParmas_ForFilter2(filter2);

            this.store1.FilterParams.AddRange(psFor_Filter2);

            #endregion


            this.store1.DeleteQuery.Add(new ControlParam(m_Table.ID_FIELD, this.table1.ID, "CheckedRows"));

            #region 设置创建行的默认值

            foreach (IG2_TABLE_COL col in m_TableSet.Cols)
            {
                if (col.ROW_SID == -3 || StringUtil.IsBlank(col.DEFAULT_VALUE) || col.SEC_LEVEL >= 6)
                {
                    continue;
                }

                Param pa = new Param(col.DB_FIELD, col.DEFAULT_VALUE);

                this.store1.InsertParams.Add(pa);
            }

            #endregion


            #region 删除进回收站模式

            store1.DeleteRecycle = true;

            store1.FilterParams.Add(new Param("ROW_SID", "-1", DbType.Int32) { Logic = ">=" }); ;

            Param drP1 = new Param("ROW_SID", "-3", System.Data.DbType.Int32);
            ServerParam drP2 = new ServerParam("ROW_DATE_DELETE", "TIME_NOW");

            store1.DeleteRecycleParams.Add(drP1);
            store1.DeleteRecycleParams.Add(drP2);

            #endregion


            DbCascadeRule.Bind(this.store1);

        }



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
                InputOutExcelPlugin inter = Activator.CreateInstance(plugT) as InputOutExcelPlugin;

                inter.ClassName = plugClass;
                inter.Method = plugMethod;
                inter.Params = "{}";

                inter.SrcUrl = this.Request.Url.ToString();

                inter.Main = this;
                inter.MainStore = this.store1;// this.m_MainStore;
                inter.MainTable = this.table1;// this.m_MainTable;

                inter.SrcStore = this.store1;
                inter.SrcTable = this.table1;
                inter.Title = StringUtil.NoBlank(WebUtil.Query("alias_title"), m_Table.DISPLAY);

                mi.Invoke(inter, null);
            }
            catch (Exception ex)
            {
                log.Error("执行插件函数错误。", ex);

                MessageBox.Alert("执行插件函数错误:" + ex.Message);
            }
        }


        #region 二次过滤

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr, string defualtValue)
        {
            string value = obj.Value<string>(attr);

            return StringUtil.NoBlank(value, defualtValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr)
        {
            string value = obj.Value<string>(attr);


            return value;
        }


        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private string GetTSqlWhere_ForFilter2(string filter2)
        {
            if (StringUtil.IsBlank(filter2))
            {
                return string.Empty;
            }

            JArray items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filter2);

            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (JObject item in items)
            {
                //string field = item.Value<string>("field");
                //string logic = item.Value<string>("logic");
                //string value = item.Value<string>("value");

                //if (i++ > 0) { sb.Append(" AND "); }

                //sb.AppendFormat("({0} {1} '{2}')", field, logic, value);


                string p_type = GetJsonValue(item, "p_type", "DEFAULT");    //两种模式. Default | TSQL_WHERE 
                string field = GetJsonValue(item, "field");
                string logic = GetJsonValue(item, "logic", "=");
                string value = GetJsonValue(item, "value");

                if (i++ > 0) { sb.Append(" AND "); }

                p_type = p_type.ToUpper();

                if (p_type == "TSQL_WHERE")
                {
                    sb.Append("(").Append(value).Append(")");
                }
                else
                {
                    if (StringUtil.IsBlank(field))
                    {
                        throw new Exception("二次筛选的参数字段名不能为空。");
                    }

                    sb.AppendFormat("({0} {1} '{2}')", field, logic, value);
                }
            }


            return sb.ToString();
        }


        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private List<Param> GetParmas_ForFilter2(string filter2)
        {
            List<Param> ps = new List<Param>();

            if (StringUtil.IsBlank(filter2))
            {
                return ps;
            }

            JArray items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filter2);


            foreach (JObject item in items)
            {
                //string field = item.Value<string>("field");
                //string logic = item.Value<string>("logic");
                //string value = item.Value<string>("value");

                //Param p = new Param(field, value);
                //p.Logic = logic;

                //ps.Add(p);


                string p_type = GetJsonValue(item, "p_type", "DEFAULT");    //两种模式. Default | TSQL_WHERE 
                string field = GetJsonValue(item, "field");
                string logic = GetJsonValue(item, "logic", "=");
                string value = GetJsonValue(item, "value");

                p_type = p_type.ToUpper();

                if (p_type == "TSQL_WHERE")
                {
                    EasyClick.Web.Mini2.TSqlWhereParam sqlWhereParam = new EasyClick.Web.Mini2.TSqlWhereParam(value);

                    ps.Add(sqlWhereParam);
                }
                else
                {
                    if (StringUtil.IsBlank(field))
                    {
                        throw new Exception("二次筛选的参数字段名不能为空。");
                    }

                    Param p = new Param(field, value);
                    p.Logic = logic;

                    ps.Add(p);
                }
            }

            return ps;
        }


        #endregion


        private string GetFieldsArray(TableSet tableSet)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();



            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {
                if (col.ROW_SID == -3)
                {
                    continue;
                }

                if (!col.IS_VISIBLE && col.DB_FIELD != m_Table.ID_FIELD)
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

                InitChart();

            }




            SetSum();



        }



        /// <summary>
        /// 创建界面控件
        /// </summary>
        private void CreateControl(TableSet tableSet)
        {

            int viewId = WebUtil.QueryInt("id");    //当前查询窗体的 id
            int menuId = WebUtil.QueryInt("menu_Id");   //菜单ID
            string secTag = WebUtil.Query("sec_tag");

            DbDecipher decipher = ModelAction.OpenDecipher();

            ToolbarSet toolbarSet = new ToolbarSet();
            toolbarSet.SelectForTable(decipher, tableSet.Table.IG2_TABLE_ID);

            if (toolbarSet.Toolbar != null)
            {
                this.Toolbar1.Items.Clear();
            }

            M2ToolbarFactory toolbarFty = new M2ToolbarFactory();
            toolbarFty.StoreId = store1.ID;
            toolbarFty.TableId = table1.ID;
            toolbarFty.CreateItems(this.Toolbar1, toolbarSet);


            M2TableFactory tableFty = new M2TableFactory(this.IsPostBack);
            tableFty.CreateTableColumns(this.table1, this.store1, tableSet);

            M2SearchFormFactory searchFty = new M2SearchFormFactory(this.IsPostBack, this.store1.ID);
            searchFty.CreateControls(this.searchForm, tableSet);

            M2SecurityUiFactory secUiFty = new M2SecurityUiFactory();
            secUiFty.InitSecUI(viewId, "TABLE", menuId, tableSet);
            secUiFty.Filter("", secTag, "", this.Toolbar1, this.table1, this.store1);

            secUiFty.FilterForForm("", secTag, "", this.searchForm, this.store1);

            searchForm.Visible = tableSet.Table.SEARCH_PANEL_EXPAND;

        }





        public void StepEdit3()
        {
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit3.aspx?id=" + m_TableId, $"列设置-Table={m_TableId}");
        }

        public void StepEdit4()
        {
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit4.aspx?id=" + m_TableId, $"高级设置-Table={m_TableId}");
        }

        public void StepEdit5_DialogMode()
        {
            EcView.Show("/App/InfoGrid2/View/OneView/StepEdit5_DialogMode.aspx?id=" + m_TableId + "&view_id=" + m_TableId, $"模式窗口设置-Table={m_TableId}");
        }

        public void Search()
        {
            this.store1.LoadPage(0);

        }

        public void GoValidSetup()
        {
            MiniPager.Redirect("/App/InfoGrid2/View/OneValid/ValidStepEdit2.aspx?id=" + m_TableId);
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
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {
            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_ID", id);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);


            string urlStr;

            if (toolbarId > 0)
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={0}&table_id={1}", toolbarId, id);
            }
            else
            {
                urlStr = string.Format("/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={0}", id);
            }

            MiniPager.Redirect(urlStr);
        }



        /// <summary>
        /// 输出到打印机
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




            int pkValue = StringUtil.ToInt(this.store1.CurDataId);
            int pageId = WebUtil.QueryInt("Id");


            string mainTable = store1.Model;
            string mainPk = store1.IdField;


            string url = "/App/InfoGrid2/View/PrintTemplate/PrintTemplateSingleTable.aspx";

            NameValueCollection nv = new NameValueCollection(){
                {"id",pageId.ToString()},
                {"mainTableID",pkValue.ToString()},  //主数据记录的ID
                {"pageID",pageId.ToString()},

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


        /// <summary>
        /// 第一个图形报表的标题吧
        /// </summary>
        public string m_pie_legends_1 = string.Empty;


        /// <summary>
        /// 第一个图形报表的数据把
        /// </summary>
        public string m_pie_series_1 = string.Empty;


        /// <summary>
        /// 第二个图形报表的标题吧
        /// </summary>
        public string m_pie_legends_2 = string.Empty;


        /// <summary>
        /// 第二个图形报表的数据把
        /// </summary>
        public string m_pie_series_2 = string.Empty;


        /// <summary>
        /// 第三个图形报表的标题吧
        /// </summary>
        public string m_pie_legends_3 = string.Empty;


        /// <summary>
        /// 第三个图形报表的数据把
        /// </summary>
        public string m_pie_series_3 = string.Empty;


        /// <summary>
        /// 柱状图的所有数据
        /// </summary>
        public string m_bar_data = string.Empty;




        void InitChart()
        {



            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter lmFilter297 = new LightModelFilter("UT_297");
            lmFilter297.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter297.And("COL_86", "", Logic.Inequality);
            lmFilter297.TSqlOrderBy = "ROW_DATE_CREATE desc";
            lmFilter297.Top = 100;

            List<LModel> lm297s = decipher.GetModelList(lmFilter297);


            FillPieData(lm297s, "COL_86", "COL_96", ref m_pie_legends_1, ref m_pie_series_1);


            LightModelFilter lmFilter298 = new LightModelFilter("UT_298");
            lmFilter298.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter298.And("COL_2", "", Logic.Inequality);
            lmFilter298.TSqlOrderBy = "ROW_DATE_CREATE desc";
            lmFilter298.Top = 100;

            List<LModel> lm298s = decipher.GetModelList(lmFilter298);


            FillPieData(lm298s, "COL_2", "COL_96", ref m_pie_legends_2, ref m_pie_series_2);



            LightModelFilter lmFilter113 = new LightModelFilter("UT_113");
            lmFilter113.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter113.And("COL_5", "", Logic.Inequality);
            lmFilter113.TSqlOrderBy = "ROW_DATE_CREATE desc";
            lmFilter113.Top = 100;


            List<LModel> lm113s = decipher.GetModelList(lmFilter113);


            FillPieData(lm113s, "COL_5", "COL_10", ref m_pie_legends_3, ref m_pie_series_3);



            LightModelFilter lmFilter297_1 = new LightModelFilter("UT_297");
            lmFilter297_1.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter297_1.And("COL_70", "成型室");
            lmFilter297_1.TSqlOrderBy = "ROW_DATE_CREATE desc";
            lmFilter297_1.Top = 10;


            List<LModel> lm297_1s = decipher.GetModelList(lmFilter297_1);



            SModel sm = new SModel();


            List<string> x_datas = new List<string>();

            List<string> series_1s = new List<string>();

            List<string> series_2s = new List<string>();

            List<string> series_3s = new List<string>();




            foreach (var item in lm297_1s)
            {


                x_datas.Add($"{item.Get<DateTime>("COL_68").ToString("MM-dd")}{item["COL_71"]}{item["COL_82"]}");

                series_1s.Add($"{item.Get<decimal>("COL_76").ToString("N2")}");

                series_2s.Add($"{item.Get<decimal>("COL_96").ToString("N2")}");

                series_3s.Add($"{item.Get<decimal>("COL_97").ToString("N2")}");



            }


            sm["x_datas"] = x_datas.ToArray();
            sm["series_1s"] = series_1s.ToArray();
            sm["series_2s"] = series_2s.ToArray();
            sm["series_3s"] = series_3s.ToArray();


            m_bar_data = sm.ToJson();




        }


        void FillPieData(List<LModel> lm297s, string filed_1, string filed_2, ref string pie_legends_1, ref string pie_series_1)
        {
            Dictionary<string, decimal> key_value_1 = new Dictionary<string, decimal>();


            decimal total = 0;

            foreach (var item in lm297s)
            {

                string col_86 = item.Get<string>(filed_1);

                if (col_86 == null)
                {
                    continue;
                }


                decimal col_96 = item.Get<decimal>(filed_2);


                total += col_96;

                if (key_value_1.ContainsKey(col_86))
                {
                    key_value_1[col_86] += col_96;

                }
                else
                {

                    key_value_1.Add(col_86, col_96);
                }
            }



            List<string> legends = new List<string>();

            SModelList sm_series = new SModelList();

            foreach (var item in key_value_1)
            {

                string key = string.Empty;





                SModel sm_serie = new SModel();

                if (total > 0)
                {
                    sm_serie["value"] = item.Value.ToString("0.##");
                    sm_serie["name"] = key = item.Key + ":" + item.Value.ToString("0.##") + "(" + ((item.Value / total) * 100).ToString("0.##") + "%)";

                }
                else
                {

                    sm_serie["value"] = item.Value.ToString("0.##");
                    sm_serie["name"] = key = item.Key + ": 0 (0%)";

                }

                legends.Add(key);

                sm_series.Add(sm_serie);

            }


            SModel sm = new SModel();
            sm["orient"] = "vertical";
            sm["left"] = "left";
            sm["data"] = legends.ToArray();

            pie_legends_1 = sm.ToJson();

            pie_series_1 = sm_series.ToJson();


        }


        void SetSum()
        {


            List<LModel> lm297s = store1.GetList() as List<LModel>;

            if (lm297s.Count == 0)
            {
                return;
            }


            decimal total_col_91 = 0;
            decimal total_col_90 = 0;
            decimal total_col_77 = 0;
            decimal total_col_76 = 0;



            foreach (var item in lm297s)
            {

                decimal col_77 = item.Get<decimal>("COL_77");

                decimal col_76 = item.Get<decimal>("COL_76");

                decimal col_90 = item.Get<decimal>("COL_90");

                decimal col_91 = item.Get<decimal>("COL_91");

                total_col_76 += col_76;
                total_col_77 += col_77;

                total_col_90 += col_90;
                total_col_91 += col_91;



            }


            string sum_text_col_98 = "0%";

            string sum_text_col_93 = "0%";


            if (total_col_76 + total_col_77 != 0)
            {
                sum_text_col_98 = (total_col_76 / (total_col_76 + total_col_77) * 100).ToString("0.###") + "%";

            }


            if (total_col_90 != 0)
            {

                sum_text_col_93 = (total_col_91 / total_col_90 * 100).ToString("0.###") + "%";

            }




            BoundField bf = this.table1.Columns.FindByDataField("COL_98");
            bf.SummaryType = "user";


            BoundField bf_2 = this.table1.Columns.FindByDataField("COL_93");
            bf_2.SummaryType = "user";


            this.store1.SetSummary("COL_98", sum_text_col_98);

            this.store1.SetSummary("COL_93", sum_text_col_93);



        }



    }
}