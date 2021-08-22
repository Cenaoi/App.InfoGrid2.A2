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
    public partial class ShowChartPie : WidgetControl, IView
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
        public string m_grapTitel = string.Empty;


        /// <summary>
        /// 系列数据
        /// </summary>
        public string m_grapData = string.Empty;


        /// <summary>
        /// 报表名称
        /// </summary>
        public string m_title = string.Empty;




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
            

            DateTime? begTime = DateUtil.StartByMonth(drpTime.StartDate);
            DateTime? endTime = DateUtil.EndByMonth(drpTime.EndDate);
            

            string timeFiled = m_TemplateItem.date_field;

            if (begTime != null)
            {
                this.store1.FilterParams.Add(new Param(timeFiled, DbType.DateTime, begTime) { Logic = ">=" });
            }

            if (endTime != null)
            {
                this.store1.FilterParams.Add(new Param(timeFiled, DbType.DateTime, endTime) { Logic = "<=" });
            }


            IList models = this.store1.GetList();


            m_Report.SetDataSource(models);

            //这个就是最终的表格数据了...
            RTable table = m_Report.ToRTable();


            m_title = m_Report.Title;


            CreateTitleData(table);


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

            sbFun.Append($"option.legend.data = {m_grapTitel};");

            sbFun.Append($"option.series[0].data = {m_grapData};");

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
        /// 根据字段名查找
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="field"></param>
        private CrossHeadTreeNode FindText( CrossHeadTreeNode pNode, string field)
        {
            if(pNode == null)
            {
                return null;
            }

            if(pNode.Column.Name == field)
            {
                return pNode;
            }

            foreach (CrossHeadTreeNode node in pNode.Childs)
            {
                if (node.NodeType != CrossHeadTreeNodeTypes.Value)
                {
                    continue;
                }

                if(node.Column.Name == field)
                {
                    return node;
                }

                CrossHeadTreeNode find = FindText(node, field);

                if(find != null)
                {
                    return find;
                }
            }

            return null;
        }


        /// <summary>
        /// 创建标题数据
        /// </summary>
        void CreateTitleData(RTable table)
        {

            string displayField = m_TemplateItem.row_display_field;   //显示的字段名


            Dictionary<int, string> level2 = new Dictionary<int, string>();

            if (string.IsNullOrEmpty(displayField))
            {
                foreach (var item in m_Report.HeadTreeRoot.Childs)
                {
                    if (item.NodeType != CrossHeadTreeNodeTypes.Value)
                    {
                        continue;
                    }

                    //值所在索引
                    int xIndex = item.X;

                    if (m_TemplateItem.chart_row_col == "col")
                    {
                        xIndex -= 1;    //特殊处理的。。。
                    }

                    level2.Add(xIndex, item.Column.Text);
                }
            }
            else
            {
                foreach (var item in m_Report.HeadTreeRoot.Childs)
                {
                    if (item.NodeType != CrossHeadTreeNodeTypes.Value)
                    {
                        continue;
                    }

                    CrossHeadTreeNode curNode = FindText(item, displayField);

                    //值所在索引
                    int xIndex = curNode.X;

                
                    xIndex += m_TemplateItem.temp_x_index;    //特殊处理的。。。
                    

                    level2.Add(xIndex, curNode.Column.Text);
                }
            }

            //获取到第一层的东西
            var body1 = table.Body[0];

            //前面没有用的数据行 数量
            var tagsNum = m_Report.RowGroupTags.Count;



            StringBuilder sb = new StringBuilder();

            StringBuilder sb1 = new StringBuilder();


            sb.Append("[");
            sb1.Append("[");


            int i = 0;

            foreach (var item in level2)
            {

                if (i++ > 0)
                {
                    sb.Append(",");
                    sb1.Append(",");
                }

                string name = item.Value;

                string value ;

                object valueDec = body1[item.Key + tagsNum].Value;

                if(valueDec != null)
                {
                    value = string.Format("{0:0.######}", valueDec);
                }
                else
                {
                    value = "null";
                }


                sb.Append($"'{name}  {value}'");

                sb1.Append($"{{value:'{value}',name:'{name}  {value}'}}");

            }

            sb.Append("]");

            sb1.Append("]");

            m_grapTitel = sb.ToString();

            m_grapData = sb1.ToString();



        }


    }
}