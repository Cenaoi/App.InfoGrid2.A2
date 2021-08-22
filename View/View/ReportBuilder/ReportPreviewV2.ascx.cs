using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.ReportForms;
using EasyClick.Web.ReportForms.Data;
using EasyClick.Web.ReportForms.Export;
using EC5.IG2.BizBase;
using EC5.IG2.Core.UI;
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
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class ReportPreviewV2 : WidgetControl, IView
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




        ReportTemplateItem m_TemplateItem;

        /// <summary>
        /// 报表的
        /// </summary>
        TableSet m_CrossTSet = null;

        CrossReport m_Report;


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }




        /// <summary>
        /// 关联视图结构
        /// </summary>
        private void InitStoreAttrs_ForMoreView()
        {
            //int id = WebUtil.QueryInt("id", 2);

            int id = m_TableId;

            DbDecipher decipher = ModelAction.OpenDecipher();

            m_ViewSet = ViewSet.Select(decipher, id);


            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);


            LModelElement modelElem = ViewMgr.GetModelElem(m_ViewSet);

            this.store1.Model = modelElem.DBTableName;
            this.store1.IdField = m_ViewSet.View.MAIN_TABLE_NAME + "_" + m_ViewSet.View.MAIN_ID_FIELD;



            List<string> fields = new List<string>();

            StoreTSqlQuerty tSql = this.store1.TSqlQuery;

            tSql.Enabeld = true;
            tSql.Select = ViewMgr.GetTSqlSelect(m_ViewSet, ref fields);
            tSql.Form = ViewMgr.GetTSqlForm(m_ViewSet);
            tSql.Where = ViewMgr.GetTSqlWhere(m_ViewSet);
            tSql.OrderBy = ViewMgr.GetTSqlOrder(m_ViewSet, fields);


            #region 权限-结构过滤

            foreach (IG2_VIEW_TABLE vTab in m_ViewSet.Tables)
            {
                IG2_TABLE srcTab = TableMgr.GetTableForName(vTab.TABLE_NAME);

                if (!srcTab.SEC_STRUCT_ENABLED)
                {
                    continue;
                }

                UserSecritySet uSec = SecFunMgr.GetUserSecuritySet();

                if (uSec == null)
                {
                    continue;
                }

                IList<LModel> cols = TableMgr.GetLCols(srcTab.IG2_TABLE_ID,
                    new string[] { "DB_FIELD", "FILTER_CATA_TABLE", "FILTER_CATA_FIELD" });

                foreach (LModel col in cols)
                {
                    string dbField = col.Get<string>("DB_FIELD");
                    string cataTable = col.Get<string>("FILTER_CATA_TABLE");
                    string cataField = col.Get<string>("FILTER_CATA_FIELD");

                    if (string.IsNullOrEmpty(cataTable) || string.IsNullOrEmpty(cataField))
                    {
                        continue;
                    }
                     

                    string tSqlWhere = string.Format("{0} IN (SELECT {1} FROM {2} WHERE ROW_SID >=0 AND BIZ_CATA_CODE in ({3}))",
                        vTab.TABLE_NAME + "." + dbField,
                        cataField,
                        cataTable, uSec.ArrCatalogCodeString());

                    if (!string.IsNullOrEmpty(tSql.Where))
                    {
                        tSql.Where += " AND ";
                    }

                    tSql.Where += tSqlWhere;
                }
            }

            #endregion


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
            List<Param> psFor_Filter2 = JFilterHelper.GetParmas(filter2);

            this.store1.FilterParams.AddRange(psFor_Filter2);

            #endregion



            foreach (IG2_TABLE_COL col in m_CrossTSet.Cols)
            {
                if (!StringUtil.IsBlank(col.FILTER_LOGIC) ||
                    !StringUtil.IsBlank(col.FILTER_VALUE))
                {
                    Param p = new Param(col.DB_FIELD, col.FILTER_VALUE);
                    p.Logic = col.FILTER_LOGIC;

                    this.store1.SelectQuery.Add(p);
                }
                else if (!StringUtil.IsBlank(col.FILTER_TSQL_WHERE) )
                {
                    EasyClick.Web.Mini2.TSqlWhereParam p = new EasyClick.Web.Mini2.TSqlWhereParam(col.FILTER_TSQL_WHERE);
                    p.Name = col.DB_FIELD;
                    
                    this.store1.SelectQuery.Add(p);
                }


            }

        }

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



        private TableSet CrossTableSet(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet ts = new TableSet();

            ts.Table = decipher.SelectModelByPk<IG2_TABLE>(id);

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE_COL));
            filter.And("IG2_TABLE_ID", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            //filter.And("IS_SEARCH_VISIBLE", true);

            ts.Cols = decipher.SelectModels<IG2_TABLE_COL>(filter);

            return ts;
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            this.store1.Searching += Store1_Searching;
            this.store1.PageLoading += Store1_PageLoading;

            this.searchForm.ItemFiltering += SearchForm_ItemFiltering;

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");

            
            m_CrossTSet = CrossTableSet(id);


            string alias_title = WebUtil.Query("alias_title");

            HeadPanel.Visible = m_CrossTSet.Table.IS_BIG_TITLE_VISIBLE;
            this.headLab.Value = "<span class='page-head' >" + StringUtil.NoBlank(alias_title, m_CrossTSet.Table.DISPLAY) + "</span>";

            this.viewport1.MarginTop = m_CrossTSet.Table.IS_BIG_TITLE_VISIBLE ? 40 : 0;



            ReportTemplateItem item = GetTemplateItem(m_CrossTSet.Table);

            if (StringUtil.IsBlank(item.row_display_field))
            {
                this.ShowChartBtn.Visible = false;
            }


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_NAME", item.table_name);
            filter.And("TABLE_TYPE_ID", new string[] { "TABLE", "MORE_VIEW" }, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Top = 1;
            filter.Fields = new string[] { "IG2_TABLE_ID"};

            m_TableId = decipher.ExecuteScalar<int>(filter);


            #region 数据源

            m_TableSet = TableSet.Select(decipher, m_TableId);

            m_Table = m_TableSet.Table;
            m_TableName = m_Table.TABLE_NAME;



            if (m_TableName.StartsWith("UV_"))  //UV_ 关联表特殊处理
            {
                InitStoreAttrs_ForMoreView();
            }
            else
            {
                m_ModelElem = TableMgr.GetModelElem(m_TableSet);
                InitStoreAttrs_ForOneTable();
            }

            #endregion



            //构造查询控件
            M2SearchFormFactory searchFty = new M2SearchFormFactory(this.IsPostBack, this.store1.ID);
            searchFty.CreateControls(this.searchForm, m_CrossTSet);

            searchForm.Visible = true;




            m_TemplateItem = item;

            if (item == null)
            {
                return;
            }


            ReportItem rItem;
            m_Report = new CrossReport();
            m_Report.Title = StringUtil.NoBlank(alias_title, m_CrossTSet.Table.DISPLAY);

            m_Report.EnabledBeginningBalance = item.bb_enabled;
            m_Report.EnabledEndingBalance = item.eb_enabled;

            m_Report.BeginningBalanceTime = item.bb_time;


            m_Report.DateField = item.date_field;

            foreach (ReportCol c in item.col_value)
            {
                rItem = GetReportItem(c);
                m_Report.ColGroupTags.Add(rItem);
            }

            foreach (ReportCol c in item.row_value)
            {
                rItem = GetReportItem(c);
                rItem.EnabledTotal = false;
                m_Report.RowGroupTags.Add(rItem);
            }


            foreach (ReportCol c in item.values)
            {
                rItem = GetReportItem(c);
                m_Report.DataGroupTags.Add(rItem);
            }


            if (m_TemplateItem.is_data_buffer)
            {
                this.table1.PagerVisible = true;
            }

        }

        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;

            string tableName = bufferTableHid.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }

            log.Debug(this.store1.SortText);

            string reportBuffer = GlobelParam.GetValue("REP_BUFFER_NAME", "ReportBuffer", "报表临时缓冲数据库");

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen(reportBuffer);

            LightModelFilter filter = new LightModelFilter(tableName);
            filter.Limit = Limit.ByPageIndex((this.store1.PageSize > 200?20:this.store1.PageSize), e.Page);
            filter.TSqlOrderBy = e.TSqlSort;

            string tSql = GetTSql(filter);

            string tSqlCount = GetTSqlCount(filter);

            int count = decipher.ExecuteScalar<int>(tSqlCount);

            LModelList<LModel> dataModels = decipher.GetModelList(tSql);


            this.store1.RemoveAll();

            this.store1.BeginLoadData();
            {
                this.store1.AddRange(dataModels);

                this.store1.SetTotalCount(count);
                this.store1.SetCurrentPage(e.Page);
            }
            this.store1.EndLoadData();

        }

        private void SearchForm_ItemFiltering(object sender, FormLayoutItemEventArgs e)
        {
            if (m_Report.EnabledBeginningBalance)
            {
                if (e.Item is DateRangePicker)
                {
                    DateRangePicker picker = (DateRangePicker)e.Item;

                    if (picker.DataField == m_Report.DateField)
                    {
                        e.Cancel = true;
                    }
                }
            }
            
        }

        private void Store1_Searching(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = true;

            BindStore();

        }


        /// <summary>
        /// 处理 "财务期初" 设置
        /// </summary>
        private bool ProBeginningBalance()
        {
            if (!m_Report.EnabledBeginningBalance)
            {
                return true;
            }


            string field = m_Report.DateField;

            //时间控件
            DateRangePicker rangeCon = FindControl($"Search{m_TemplateItem.date_field}") as DateRangePicker;

            if(rangeCon == null)
            {
                MessageBox.Alert($"时间控件设置错误, 字段 \"{m_TemplateItem.date_field}\" 没有找到对应的控件.");
                return false;
            }

            DateTime? startDate = rangeCon.StartDate;
            DateTime? endDate = rangeCon.EndDate;


            m_Report.DateFrom = startDate;
            
            

            if (endDate != null)
            {
                Param p = new Param(field);
                p.SetInnerValue(endDate);

                p.Logic = "<=";

                this.store1.FilterParams.Add(p);
            }

            return true;
        }


        private void BindStore()
        {
            int id = WebUtil.QueryInt("id");

            bool sucess = ProBeginningBalance();

            if (!sucess)
            {
                return;
            }

            IList models = this.store1.GetList();


            m_Report.SetDataSource(models);



            RTable table = m_Report.ToRTable();


            foreach (var itemGroup in m_Report.RowGroupTags)
            {
                foreach (var rItem2 in itemGroup)
                {
                    BoundField field = new BoundField(rItem2.DBField, rItem2.Title);

                    field.Width = rItem2.Width;

                    this.table1.Columns.Add(field);
                }
            }


            CreateHeader(m_Report.HeadTreeRoot, this.table1.Columns);

            string headJson = this.table1.GetJsonForColumns();


            string clearColAll = string.Format("widget1_I_table1.clearColumnAll();");

            EasyClick.Web.Mini.MiniHelper.Eval(clearColAll);


            string addColumn = string.Format("widget1_I_table1.addColumn({0});", headJson);

            EasyClick.Web.Mini.MiniHelper.Eval(addColumn);




            //创建缓冲数据
            if (m_TemplateItem.is_data_buffer)
            {
                TableExport tabEx = new TableExport();

                DataTable dt = tabEx.GetBodyData(m_Report, table);

                string reportBuffer = GlobelParam.GetValue("REP_BUFFER_NAME", "ReportBuffer", "报表临时缓冲数据库");

                DbDecipher decipher = DbDecipherManager.GetDecipherOpen(reportBuffer);


                LModelElement modelElem = CreateReportBuffer(decipher, dt);

                FillReportBuffer(decipher, dt, modelElem.DBTableName);



                LightModelFilter filter = new LightModelFilter(modelElem.DBTableName);
                filter.Limit = Limit.ByPageIndex((this.store1.PageSize == 0 ? 20: this.store1.PageSize), 0);
                string tSql = GetTSql(filter);

                LModelList<LModel> dataModels = decipher.GetModelList(tSql);

                this.store1.RemoveAll();

                this.store1.BeginLoadData();
                {
                    this.store1.AddRange(dataModels);

                    this.store1.SetTotalCount(dt.Rows.Count);
                    
                    this.store1.SetCurrentPage(0);
                }
                this.store1.EndLoadData();

                bufferTableHid.Value = modelElem.DBTableName;
            }
            else
            {
                JsonExport crJsonEx = new JsonExport();

                string bodyData = crJsonEx.GetBodyData(m_Report, table);

                this.store1.RemoveAll();
                this.store1.AddRange(bodyData);
            }

        }


        private string GetTSqlCount(LightModelFilter filter)
        {
            
            string tSqlForm = filter.ModelName;
            string tSqlWhere = string.Empty;
            string tSqlSelect = "Count(REP_ROW_IDENTITY)";


            StringBuilder tSql = new StringBuilder();

            tSql.Append("  SELECT ").Append(tSqlSelect);
            
            tSql.AppendFormat("\n  FROM {0} ", tSqlForm);

            if (!string.IsNullOrEmpty(tSqlWhere))
            {
                tSql.AppendFormat("\n  WHERE {0} ", tSqlWhere);
            }


            return tSql.ToString();
        }

        private string GetTSql(LightModelFilter filter)
        {

            Limit limit = filter.Limit;
            string tSqlForm = filter.ModelName;
            string tSqlWhere = string.Empty;
            string tSqlOrderBy = "REP_ROW_TYPE DESC";
            string tSqlSelect = "*";

            if (!string.IsNullOrEmpty(filter.TSqlOrderBy))
            {
                tSqlOrderBy += "," + filter.TSqlOrderBy;
            }

            StringBuilder tSql = new StringBuilder();

            tSql.AppendLine("SELECT * FROM ( ");
            {
                tSql.Append("  SELECT ").Append(tSqlSelect);

                tSql.AppendFormat(",row_number() over(order by {0}) as PAGE_ROW_NUMBER ", tSqlOrderBy);

                tSql.AppendFormat("\n  FROM {0} ", tSqlForm);

                if (!string.IsNullOrEmpty(tSqlWhere))
                {
                    tSql.AppendFormat("\n  WHERE {0} ", tSqlWhere);
                }
            }

            tSql.Append("\n) as T ");
            tSql.AppendFormat("WHERE (PAGE_ROW_NUMBER between {0} and {1})",
                limit.StartRowIndex + 1, limit.EndRowIndex + 1);



            return tSql.ToString();
        }



        /// <summary>
        /// 填充缓冲报表数据
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        protected void FillReportBuffer(DbDecipher decipher, DataTable dt,string tableName)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)decipher.Connection);


            bulkCopy.NotifyAfter = dt.Rows.Count;

            try
            {
                // sbc.DestinationTableName = "jobs";
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                log.Error("批量插入报表数据错误.", ex);
            }

        }


        /// <summary>
        /// 获取 数值的整形长度
        /// </summary>
        /// <param name="table"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int GetLenForNumber(DataTable table, DataColumn col)
        {
            if (table.Rows.Count == 0) { return 0; }

            decimal maxValue = 0;
            decimal minValue = 0;

            bool isInit = false;

            foreach (DataRow row in table.Rows)
            {
                if (row.IsNull(col)) { continue; }

                decimal value = (decimal)row[col];

                if (isInit)
                {
                    maxValue = Math.Max(maxValue, value);
                    minValue = Math.Min(minValue, value);
                }
                else
                {
                    isInit = true;
                    maxValue = value;
                    minValue = value;
                }

            }


            long maxVD = decimal.ToInt64(maxValue);
            long minVD = decimal.ToInt64(minValue);

            int maxLen = Math.Abs(maxVD).ToString().Length;
            int minLen = Math.Abs(minVD).ToString().Length;

            return Math.Max(maxLen, minLen);
        }


        /// <summary>
        /// 创建报表缓冲
        /// </summary>
        /// <param name="decipher"></param>
        private LModelElement CreateReportBuffer(DbDecipher decipher,DataTable dt)
        {

            LModelDna.BeginEdit();

            LModelElement modelElem = null;

            try
            {
                modelElem = new LModelElement("REP_" + DateTime.Now.ToString("yyMMdd_HHmmss_") + RandomUtil.Next(10000000).ToString("00000000"));
                modelElem.IdentityField = "REP_ROW_IDENTITY";
                modelElem.PrimaryKey = "REP_ROW_IDENTITY";
                modelElem.Identity = new DBIdentityAttribute(LModelIdentityRule.Auto);


                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == typeof(decimal))
                    {
                        int len = GetLenForNumber(dt, col);

                        if (len < 0) { len = 1; }

                        len += 6;


                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.Decimal);
                        fieldElem.Mandatory = false;
                        fieldElem.MaxLen = len;
                        fieldElem.DecimalDigits = 6;

                        modelElem.Fields.Add(fieldElem);
                    }
                    else if(col.DataType == typeof(int))
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.Int);
                        fieldElem.Mandatory = true;

                        modelElem.Fields.Add(fieldElem);
                    }
                    else
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.String);
                        fieldElem.Mandatory = false;
                        fieldElem.MaxLen = 300;
                        modelElem.Fields.Add(fieldElem);
                    }
                }


                LModelFieldElement idField = new LModelFieldElement("REP_ROW_IDENTITY", LMFieldDBTypes.Int);
                idField.IsKey = true;
                modelElem.Fields.Add(idField);
            }
            catch (Exception ex)
            {

                LModelDna.EndEdit();

                log.Error(ex);
            }
            finally
            {
                LModelDna.EndEdit();
            }

            DatabaseBuilder db = decipher.DatabaseBuilder;

            db.CreateTable(modelElem);

            return modelElem;
        }


        /// <summary>
        /// 转换为 Excel 文件
        /// </summary>
        public void GoToExcel()
        {

            bool sucess = ProBeginningBalance();

            if (!sucess)
            {
                return;
            }


            IList models = this.store1.GetList();


            m_Report.SetDataSource(models);

            RTable table = m_Report.ToRTable();

            ExcelExport excel = new ExcelExport();

            string dir = "/_Temporary/Excel_ReportCross/";
            string path = dir + DateTime.Now.ToString("yyMMdd_HHmmssff") + ".xls";

            if (!Directory.Exists( MapPath(dir)))
            {
                Directory.CreateDirectory(MapPath(dir));
            }

            excel.Save(MapPath("~" + path), m_Report);


            string downloa = "Mini2.create('Mini2.ui.extend.DownloadWindow', {fileName: '下载 Excel 文件',fielUrl:'" + path + "'}).show();";

            EasyClick.Web.Mini.MiniHelper.Eval(downloa);
        }

        protected override void OnLoad(EventArgs e)
        {
            //int id = WebUtil.QueryInt("id");

            //IList models = GetModels(m_item.table_name, id);


        }




        private void CreateHeader(CrossHeadTreeNode parentNode, TableColumnCollection cols)
        {
            foreach (var node in parentNode.Childs)
            {
                if (node.HasChild())
                {
                    GroupColumn field = new GroupColumn(node.Column.Text);

                    cols.Add(field);

                    CreateHeader(node, field.Columns);
                }
                else
                {
                    NumColumn field = new NumColumn("DATA_" + node.X, node.Column.Text);
                    field.ItemAlign = EasyClick.Web.Mini.CellAlign.Right;
                    field.Width = node.Column.Width;
                    field.NotDisplayValue = "0";
                    field.EditorMode = EditorMode.None;
                    field.Format = node.Column.Format;

                    cols.Add(field);
                }
            }
        }


        /// <summary>
        /// 清理列
        /// </summary>
        public void GoClearColumnAll()
        {
            string js = string.Format("widget1_I_table1.clearColumnAll();");

            EasyClick.Web.Mini.MiniHelper.Eval(js);
        }

        /// <summary>
        /// 重新加载列　
        /// </summary>
        public void GoResetColumn()
        {
            string tt = File.ReadAllText(@"c:\column.txt", Encoding.UTF8);

            string js = string.Format("widget1_I_table1.addColumn({0});",tt);

            EasyClick.Web.Mini.MiniHelper.Eval(js);
        }
    

        /// <summary>
        /// 添加列、行、值标签
        /// </summary>
        public ReportItem GetReportItem( ReportCol excelCol)
        {
            ReportItem item = new ReportItem();
            
            item.DBField = excelCol.field;
            item.FunName = excelCol.fun_name;
            item.DBValue = excelCol.db_value;
            item.EnabledTotal = excelCol.total;
            item.Format = excelCol.format;
            
            item.FormatType = StringUtil.NoBlank(excelCol.format_type, "DEFAULT").ToUpper();
            
            item.Width = StringUtil.ToInt(excelCol.width);
            item.Title = StringUtil.NoBlank( excelCol.title,excelCol.desc);
            item.Style = excelCol.style;
            item.ValueMode = EnumUtil.Parse<RFieldValueMode>(excelCol.value_mode, RFieldValueMode.DBValue, true) ;

            item.OrderType = EnumUtil.Parse<ReportOrderTypes>( excelCol.order_type, ReportOrderTypes.ASC, true);


            item.OneChild = excelCol.one_child;

            item.Code = excelCol.use_expr;

            foreach (ReportField eFiled in excelCol.fixed_values)
            {
                ItemFixedValue fixedValue = new ItemFixedValue();
                fixedValue.Text = eFiled.text;
                fixedValue.Type = eFiled.type;
                fixedValue.Value = eFiled.value;
                fixedValue.Operator = EnumUtil.Parse<OperatorTypes>(eFiled.operators, OperatorTypes.Equals);

                item.FixedValues.Add(fixedValue);
            }

            return item;
        }



        /// <summary>
        /// 拿到报表设置xml信息
        /// </summary>
        /// <returns></returns>
        public ReportTemplateItem GetTemplateItem(IG2_TABLE ts)
        {
            if (ts == null) { throw new ArgumentNullException("ts"); }

            if (string.IsNullOrEmpty(ts.PAGE_TEMPLATE)) throw new ArgumentNullException("模板内容不能为空.");

            //之前的xml数据有问题
            if (ts.PAGE_TEMPLATE.Contains("ExcelTemplateItem"))
            {
                ts.PAGE_TEMPLATE = ts.PAGE_TEMPLATE.Replace("ExcelCol", "ReportCol").Replace("ExcelTemplateItem", "ReportTemplateItem");
                DbDecipher decipher = ModelAction.OpenDecipher();

                decipher.UpdateModelProps(ts, "PAGE_TEMPLATE");
            }

           

            try
            {
                ReportTemplateItem item = XmlUtil.Deserialize<ReportTemplateItem>(ts.PAGE_TEMPLATE);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("反序列化报表模板失败.",ex);
            }
        }


        /// <summary>
        /// 显示图形报表
        /// </summary>
        public void ShowChart()
        {
            int id = WebUtil.QueryInt("id");

            //时间控件
            DateRangePicker drp = FindControl($"Search{m_TemplateItem.date_field}") as DateRangePicker;

            if(drp == null)
            {
                MessageBox.Alert("'时间字段' 设置错误！");
                return;
            }

            
            string filter2 = JFilterHelper.GetJson(this.searchForm,new string[] { m_TemplateItem.date_field });

            string filter2Base64 = Base64Util.ToString(filter2, Base64Mode.Http);

            //开始时间
            DateTime begTime = drp.StartYear ?? DateTime.Now;

            //结束时间
            DateTime endTime = drp.EndYear ?? DateTime.Now;


            string begTimeStr = DateUtil.ToDateString(begTime);
            string endTimeStr = DateUtil.ToDateString(endTime);

            //显示圆形图表
            if(m_TemplateItem.chart_type == "pie")
            {
                EasyClick.Web.Mini.EcView.show(
                $"/App/InfoGrid2/View/ReportBuilder/ShowChartPie.aspx?id={id}&begTime={begTimeStr}&endTime={endTimeStr}&filter2={filter2Base64}", "图形报表");
            }
            //显示柱状图表
            else if(m_TemplateItem.chart_type == "bar")
            {
                //是否要显示一个值多列的情况
                if (m_TemplateItem.multi_value)
                {
                    EasyClick.Web.Mini.EcView.show(
                      $"/App/InfoGrid2/View/ReportBuilder/ShowChartV2.aspx?id={id}&begTime={begTimeStr}&endTime={endTimeStr}&filter2={filter2Base64}", "图形报表");

                }
                else
                {

                    EasyClick.Web.Mini.EcView.show(
                        $"/App/InfoGrid2/View/ReportBuilder/ShowChart.aspx?id={id}&begTime={begTimeStr}&endTime={endTimeStr}&filter2={filter2Base64}", "图形报表");

                }
            }




        }


       
    }
}