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
using EC5;
using EC5.IG2.Core;
using App.InfoGrid2.Excel_Template.V1;

namespace App.InfoGrid2.View.OneView
{
    public partial class ViewPreview : WidgetControl, IView
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

            #region 设置更改行的参数

            if (m_ModelElem.HasField("ROW_DATE_UPDATE"))
            {
                this.store1.UpdateParams.Add(new ServerParam("ROW_DATE_UPDATE", "TIME_NOW"));
            }

            #endregion
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
                uri.Append("form_edit_pageID" , m_Table.FORM_EDIT_PAGEID);
                
                if (!StringUtil.IsBlank(title))
                {
                    uri.Append("alias_title", System.Uri.EscapeUriString(title));
                }

                string url = uri.ToString();

                EcView.Show( url , title + "-新建");
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
                    uri.Append("alias_title" , System.Uri.EscapeUriString(title));
                }

                EcView.Show( uri.ToString() , title + "-新建");
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

            if (!StringUtil.IsBlank(m_Table.SORT_TEXT))
            {
                this.store1.SortText = m_Table.SORT_TEXT;
            }

            this.store1.StringFields = GetFieldsArray(m_TableSet);


            LModelElement modelElem = LModelDna.GetElementByName(m_Table.TABLE_NAME);

            if (modelElem.HasField("BIZ_SID"))
            {
                this.store1.DeleteQuery.Add(new EasyClick.Web.Mini2.TSqlWhereParam(string.Format(
                    "(BIZ_SID is null OR BIZ_SID <= {0})", m_Table.DELETE_BY_BIZ_SID)));
            }



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

            }
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
            tableFty.CreateTableColumns(this.table1,this.store1, tableSet);

            M2SearchFormFactory searchFty = new M2SearchFormFactory(this.IsPostBack,this.store1.ID);
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



                SheetParam sp = TemplateUtilV1.ReadTemp(path);

                //保存Excel文件在服务器中
                TemplateUtilV1.CreateExcel(sp, ds, wFile.PhysicalPath);


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


            Table tab = this.table1;
            Store store = this.store1;

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
                ResultBase result = BizHelper.ChangeBizSID(psStr, this.table1, this.store1, m_TableSet);

                Toast.Show(result.Message);
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
        /// 改变所有记录的 Biz 业务状态
        /// </summary>
        /// <param name="psStr"></param>
        public void ChangeBizSIDALL(string psStr)
        {
            try
            {
                ResultBase result = BizHelper.ChangeBizSIDAll(psStr, this.store1);

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
                ResultBase result = BizHelper.ChangeField(paramStr, this.table1, store1);

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