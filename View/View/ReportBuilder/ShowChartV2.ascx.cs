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
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.ReportBuilder
{
    /// <summary>
    /// 这是显示一直多列数据的结构的
    /// </summary>
    public partial class ShowChartV2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected override void OnInitCustomControls(EventArgs e)
        {

            if (!IsPostBack)
            {
                this.drpTime.StartDate = WebUtil.QueryDateTime("begTime", DateUtil.StartByYear());
                this.drpTime.EndDate = WebUtil.QueryDateTime("endTime", DateUtil.EndByYear());

                InitData();


                BindStore();
            }
            else
            {

                InitData();


            }
            

        }

        /// <summary>
        /// 标题数组
        /// </summary>
        public string m_titleList = string.Empty;

        /// <summary>
        /// 时间数据数组
        /// </summary>
        public string m_dateList = string.Empty;

        /// <summary>
        /// 系列数据
        /// </summary>
        public string m_seriesData = string.Empty;

        /// <summary>
        /// 报表名称
        /// </summary>
        public string m_title = string.Empty;

        /// <summary>
        /// 标题集合
        /// </summary>
        public List<string> level2;



        void InitData()
        {

            int id = WebUtil.QueryInt("id");

                
            m_CrossTSet = CrossTableSet(id);


            m_TemplateItem = GetTemplateItem(m_CrossTSet.Table);

            OnInit_SQL(m_TemplateItem);


        }

        #region 构建数据


        int m_TableId;


        ReportTemplateItem m_TemplateItem;

        /// <summary>
        /// 报表的原始定义数据...例如模板,字段集合...
        /// </summary>
        TableSet m_CrossTSet;

        IG2_TABLE m_Table;

        /// <summary>
        /// [数据] 表名
        /// </summary>
        string m_TableName;

        /// <summary>
        /// [数据] 数据源
        /// </summary>
        TableSet m_TableSet;
        /// <summary>
        /// [数据] 视图数据源(多表关联)
        /// </summary>
        ViewSet m_ViewSet;

        /// <summary>
        /// [数据] 数据源实体
        /// </summary>
        LModelElement m_ModelElem = null;

        CrossReport m_Report;

        private void OnInit_SQL(ReportTemplateItem item)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_NAME", item.table_name);
            filter.And("TABLE_TYPE_ID", new string[] { "TABLE", "MORE_VIEW" }, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Top = 1;
            filter.Fields = new string[] { "IG2_TABLE_ID" };

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


            string alias_title = WebUtil.Query("alias_title");  //别名

            ReportItem rItem;
            m_Report = new CrossReport();
            m_Report.Title = StringUtil.NoBlank(alias_title, m_CrossTSet.Table.DISPLAY);


            if (m_TemplateItem.chart_row_col == "row")
            {
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
            }
            else
            {
                foreach (ReportCol c in item.row_value)
                {
                    rItem = GetReportItem(c);
                    m_Report.ColGroupTags.Add(rItem);
                }

                foreach (ReportCol c in item.col_value)
                {
                    rItem = GetReportItem(c);
                    rItem.EnabledTotal = false;
                    m_Report.RowGroupTags.Add(rItem);
                }
            }

            foreach (ReportCol c in item.values)
            {
                rItem = GetReportItem(c);
                m_Report.DataGroupTags.Add(rItem);
            }

        }


        /// <summary>
        /// 添加列、行、值标签
        /// </summary>
        public ReportItem GetReportItem(ReportCol excelCol)
        {
            ReportItem item = new ReportItem();

            item.DBField = excelCol.field;
            item.FunName = excelCol.fun_name;
            item.DBValue = excelCol.db_value;
            item.EnabledTotal = excelCol.total;
            item.Format = excelCol.format;

            if (!StringUtil.IsBlank(excelCol.format_type))
            {
                item.FormatType = excelCol.format_type.ToUpper();
            }
            else
            {
                item.FormatType = "DEFAULT";
            }

            item.Width = StringUtil.ToInt(excelCol.width);
            item.Title = StringUtil.NoBlank(excelCol.title, excelCol.desc);
            item.Style = excelCol.style;
            item.ValueMode = EnumUtil.Parse<RFieldValueMode>(excelCol.value_mode, RFieldValueMode.DBValue, false);

            item.OneChild = excelCol.one_child;

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
                else if (!StringUtil.IsBlank(col.FILTER_TSQL_WHERE))
                {
                    EasyClick.BizWeb2.TSqlWhereParam p = new EasyClick.BizWeb2.TSqlWhereParam(col.FILTER_TSQL_WHERE);
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

                if (!col.IS_VISIBLE)
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


        #endregion


        private void BindStore()
        {
            int id = WebUtil.QueryInt("id");

            DateTime begTime = this.drpTime.StartMonth ?? DateUtil.StartByMonth();

            DateTime endTime = this.drpTime.EndMonth ?? DateUtil.EndByMonth();
            

            string timeFiled = m_TemplateItem.date_field;

            this.store1.FilterParams.Add(new Param(timeFiled, DbType.DateTime, begTime) { Logic = ">=" });
            this.store1.FilterParams.Add(new Param(timeFiled, DbType.DateTime, endTime) { Logic = "<=" });



            IList models = this.store1.GetList();


            m_Report.SetDataSource(models);

            //这个就是最终的表格数据了...
            RTable table = m_Report.ToRTable();


            m_title = m_Report.Title;

            CreateTitleData();


            ConstructionJavascript(table);


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


        /// <summary>
        /// 拿到报表设置xml信息
        /// </summary>
        /// <returns></returns>
        public ReportTemplateItem GetTemplateItem(IG2_TABLE ts)
        {
            if (ts == null) { throw new ArgumentNullException("ts"); }

            if (string.IsNullOrEmpty(ts.PAGE_TEMPLATE)) throw new ArgumentNullException("模板内容不能为空.");

            try
            {
                ReportTemplateItem item = XmlUtil.Deserialize<ReportTemplateItem>(ts.PAGE_TEMPLATE);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("反序列化报表模板失败.", ex);
            }
        }


        /// <summary>
        /// 根据时间查询数据
        /// </summary>
        public void btnSelect()
        {

            BindStore();

            StringBuilder sbFun = new StringBuilder();


            sbFun.Append("myChart.clear();");

            sbFun.Append("myChart.hideLoading();");


            sbFun.Append($"option.series = {m_seriesData};");

            sbFun.Append($"option.xAxis[0].data = {m_dateList};");

            sbFun.Append("myChart.setOption(option);");


            string json = sbFun.ToString();


            EasyClick.Web.Mini.MiniHelper.Eval(json);


        }



        /// <summary>
        /// 查询本月的数据
        /// </summary>
        public void btnMonth()
        {
            this.drpTime.StartDate = DateUtil.StartByMonth();
            this.drpTime.EndDate = DateUtil.EndByMonth();

            btnSelect();

        }

        /// <summary>
        /// 获取今年的数据
        /// </summary>
        public void btnYear()
        {
            this.drpTime.StartDate = DateUtil.StartByYear();
            this.drpTime.EndDate = DateUtil.EndByYear();

            btnSelect();

        }


        /// <summary>
        /// 查询本季的数据
        /// </summary>
        public void btnQuarter()
        {
            this.drpTime.StartDate = DateUtil.StartByQuarter();
            this.drpTime.EndDate = DateUtil.EndByQuarter();

            btnSelect();

        }


        /// <summary>
        /// 第二层数据
        /// </summary>
        class LevelData2
        {
            public string Text { get; set; }

            public object Value { get; set; }
        }




        /// <summary>
        /// 构建脚本数据
        /// </summary>
        /// <param name="table">数据源</param>
        void ConstructionJavascript(RTable table)
        {

            CrossReport cr = m_Report;


            m_dateList = CreateDateList(table);


            List<string> level1 = GetLevel1(table);


            SortedList<string, SortedList<string, LevelData2>> data = new SortedList<string, SortedList<string, LevelData2>>();


            RRow row = table.Body[0];

            foreach (var item in cr.HeadTreeRoot.Childs)
            {
                if (item.NodeType != CrossHeadTreeNodeTypes.Value)
                {
                    continue;
                }

                string date = item.Column.Text;

                foreach (var item2 in item.Childs)
                {

                    int x = item2.X;

                    string l2 = item2.Column.Text;

                    SortedList<string,LevelData2> ld2 = null;

                    if (!data.TryGetValue(l2,out ld2))
                    {
                        ld2 = new SortedList<string, LevelData2>();

                        foreach (var level1Text in level1)
                        {
                            ld2.Add(level1Text, null);
                        }


                        data.Add(l2,ld2);
                    }

                    LevelData2 d2 = new LevelData2()
                    {
                        Text = date,
                        Value = row[cr.RowGroupTags.Count + x].Value
                    };

                    ld2[date] = d2;

                }

            }



            StringBuilder sb = new StringBuilder();


            sb.Append("[");


            int i = 0;

            foreach (var item in data)
            {
                if (i++ > 0)
                {
                    sb.Append(",");
                }

                string titleName = item.Key;

                sb.Append("{");

                sb.Append($"name:'{titleName}',");
                sb.Append("type: 'bar',");
                sb.Append("barWidth:50,");
                sb.Append("data:");

                sb.Append("[");

                int j = 0;

                
                foreach(var item2 in item.Value.Values)
                {
                    if(j++> 0)
                    {
                        sb.Append(",");
                    }

                    if(item2 == null)
                    {
                        continue;
                    }

                    if (item2.Value == null)
                    {
                        sb.Append($"{{name:'{item2.Text}',value:0}}");
                    }
                    else
                    {
                        sb.Append(string.Format("{{name:'{0}',value:{1:0.######}}}",item2.Text,item2.Value));
                    }

                }

                sb.Append("]}");


            }





            m_seriesData = sb.Append("]").ToString();










            //for (int i = 0; i < level2.Count; i++)
            //{
            //    if (i > 0)
            //    {
            //        sb.Append(",");
            //    }

            //    string titleName = level2[i];



            //    sb.Append("{");

            //    sb.Append($"name:'{titleName}',");
            //    sb.Append("type: 'bar',");
            //    sb.Append("barWidth:50,");
            //    sb.Append("data:");

            //    sb.Append("[");

            //    int j = 0;


            //    foreach(var row in table.Body[0])
            //    {


            //        if(j++ < m_Report.RowGroupTags.Count)
            //        {
            //            continue;
            //        }

            //        if (j > m_Report.RowGroupTags.Count + 1)
            //        {
            //            sb.Append(",");
            //        }

            //        string text = row?.TreeNode?.Column?.Text;

            //       if(text != titleName)
            //       {
            //            continue;
            //       }



            //        sb.Append($"{row.Value}");

            //    }

            //    sb.Append("]");


            //    sb.Append("}");
            //}



            //m_seriesData = sb.ToString();

        }



        /// <summary>
        /// 创建标题数据
        /// </summary>
        void CreateTitleData()
        {


            #region 获取第二层

            level2 = new List<string>();

            foreach (var item in m_Report.HeadTreeRoot.Childs)
            {
                if (item.NodeType != CrossHeadTreeNodeTypes.Value)
                {
                    continue;
                }

                foreach (var item2 in item.Childs)
                {
                    if (string.IsNullOrEmpty(item2.Column.Text))
                    {
                        continue;
                    }

                    if (!level2.Contains(item2.Column.Text))
                    {
                        level2.Add(item2.Column.Text);
                    }
                }
            }


            #endregion


            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            for (int i = 0; i < level2.Count; i++)
            {

                if (i > 0)
                {
                    sb.Append(",");
                }

                string titel = level2[i];


                sb.Append($"'{titel}'");

            }

            sb.Append("]");

            m_titleList = sb.ToString();

        }


        List<string> GetLevel1(RTable table)
        {
            #region 获取第一层

            List<string> level1 = new List<string>();

            foreach (var item in m_Report.HeadTreeRoot.Childs)
            {
                if (item.NodeType != CrossHeadTreeNodeTypes.Value)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(item.Column.Text))
                {
                    continue;
                }

                if (!level1.Contains(item.Column.Text))
                {
                    level1.Add(item.Column.Text);
                }

            }


            #endregion

            return level1;
        }

        /// <summary>
        /// 创建时间数组
        /// </summary>
        ///<param name="table">数据源</param>
        string CreateDateList(RTable table)
        {

            
            List<string> level1 = GetLevel1(table);


            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            int i = 0;


            foreach (var item in level1)
            {
               

                if (i++ > 0)
                {
                    sb.Append(",");

                }

                sb.Append($"'{item}'");


            }


            sb.Append("]");

            return sb.ToString();




        }

    }
}