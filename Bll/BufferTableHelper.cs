using App.BizCommon;
using App.InfoGrid2.Model.Report;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Bll
{

    public class BufferDataColumn : DataColumn
    {
        /// <summary>
        /// 是否为大数据类型
        /// </summary>
        public bool IsRemark { get; set; }
    }

    /// <summary>
    /// 缓存表帮助类
    /// </summary>
    public class BufferTableHelper
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 填充缓冲报表数据
        /// </summary>
        /// <param name="decipher">数据库帮助类</param>
        /// <param name="dt">内存表</param>
        /// <param name="tableName">表名</param>
        public static void FillReportBuffer(DbDecipher decipher, DataTable dt, string tableName)
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
                throw new Exception("批量插入报表数据错误.", ex);
            }

        }

        /// <summary>
        /// 报表界面的界面加载事件要用的
        /// </summary>
        /// <param name="store">数据仓库</param>
        /// <param name="table_name">数据表名</param>
        /// <param name="page">页码</param>
        /// <param name="TSqlSort">数据仓库上的排序数据</param>
        public static void PageLoading(Store store, string table_name, int page, string TSqlSort)
        {

            string reportBuffer = GlobelParam.GetValue("REP_BUFFER_NAME", "ReportBuffer", "报表临时缓冲数据库");

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen(reportBuffer);

            LightModelFilter filter = new LightModelFilter(table_name);
            filter.Limit = Limit.ByPageIndex((store.PageSize > 200 ? 20 : store.PageSize), page);
            filter.TSqlOrderBy = TSqlSort;



            if (string.IsNullOrWhiteSpace(TSqlSort))
            {
                filter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            }
            string tSql = GetTSql(filter, store.SortText);

            string tSqlCount = GetTSqlCount(filter);

            int count = decipher.ExecuteScalar<int>(tSqlCount);

            LModelList<LModel> dataModels = decipher.GetModelList(tSql);


            store.RemoveAll();

            store.BeginLoadData();
            {
                store.AddRange(dataModels);

                store.SetTotalCount(count);
                store.SetCurrentPage(page);
            }
            store.EndLoadData();

        }

        /// <summary>
        /// 创建界面上的表格列
        /// </summary>
        /// <param name="table">界面上的表格对象</param>
        public static void CreateViewTableCell(Table table)
        {
            string headJson = table.GetJsonForColumns();

            //清除表格列
            string clearColAll = string.Format("widget1_I_table1.clearColumnAll();");

            EasyClick.Web.Mini.MiniHelper.Eval(clearColAll);

            //添加最新的表格列
            string addColumn = string.Format("widget1_I_table1.addColumn({0});", headJson);

            EasyClick.Web.Mini.MiniHelper.Eval(addColumn);
        }





        /// <summary>
        /// 把obj数据转成数值类型 费用用的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static decimal ConvertFee(object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);

            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return 0;
            }
        }

        /// <summary>
        /// 创建报表缓冲
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="dt">数据表</param>
        public static LModelElement CreateReportBuffer(DbDecipher decipher, DataTable dt)
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
                    bool isRemark = false;

                    if (col is BufferDataColumn)
                    {
                        isRemark = ((BufferDataColumn)col).IsRemark;
                    }

                    if (col.DataType == typeof(decimal))
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.Decimal);
                        fieldElem.Mandatory = false;
                        fieldElem.DecimalDigits = 6;

                        modelElem.Fields.Add(fieldElem);
                    }
                    else if (col.DataType == typeof(int))
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.Int);
                        fieldElem.Mandatory = true;

                        modelElem.Fields.Add(fieldElem);
                    }
                    else if (col.DataType == typeof(DateTime))
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.DateTime);
                        fieldElem.Mandatory = false;

                        modelElem.Fields.Add(fieldElem);
                    }
                    else
                    {
                        LModelFieldElement fieldElem = new LModelFieldElement(col.ColumnName, LMFieldDBTypes.String);
                        fieldElem.Mandatory = false;
                        fieldElem.MaxLen = 300;

                        if (isRemark)
                        {
                            fieldElem.IsRemakr = true;
                        }

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

                throw new Exception("创建报表缓冲失败了！", ex);

            }
            finally
            {
                LModelDna.EndEdit();
            }

            DatabaseBuilder db = decipher.DatabaseBuilder;


            try
            {
                db.CreateTable(modelElem);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                throw ex;
            }

            return modelElem;
        }


        /// <summary>
        /// 获取sql语句
        /// </summary>
        /// <param name="filter">过滤对象</param>
        /// <param name="sortText">数据仓库的排序文本</param>
        /// <returns></returns>
        public static string GetTSql(LightModelFilter filter, string sortText)
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

            if (!string.IsNullOrEmpty(sortText))
            {
                tSqlOrderBy += "," + sortText;
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
        /// 获取数据总数
        /// </summary>
        /// <param name="filter">过滤对象</param>
        /// <returns></returns>
        public static string GetTSqlCount(LightModelFilter filter)
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



     



        /// <summary>
        /// 设置内存表中的时间数据
        /// </summary>
        /// <param name="lm">实体</param>
        /// <param name="dr">内存表行</param>
        /// <param name="lmField">实体字段</param>
        /// <param name="drField">内存表字段</param>
        public static void SetDate(LModel lm, DataRow dr, string lmField, string drField)
        {
            if (lm.IsNull(lmField))
            {
                dr[drField] = DBNull.Value;

            }
            else
            {
                dr[drField] = lm.Get<DateTime>(lmField);
            }
        }


        /// <summary>
        /// 更新界面上表格列数据
        /// </summary>
        /// <param name="table">界面表格对象</param>
        /// <param name="view_fileds">表格列数据集</param>
        public static void UpdateTableCellByFields(Table table, Dictionary<string, ViewTableColumnTree> view_fileds)
        {

            table.Columns.Clear();
            table.Columns.Add(new RowNumberer());
            table.Columns.Add(new RowCheckColumn());
            foreach (var item in view_fileds.Values)
            {


                BoundField bf = CreateBoundField(item.ct);


                table.Columns.Add(bf);

                if (item.ct.Is_Group)
                {

                    GroupColumn gc = bf as GroupColumn;


                    CreateGroupColumn(gc, item);

                }
            }


            table.Columns.Add(new BoundField("", ""));


        }

        /// <summary>
        /// 根据自定义类来创建界面上的列控件
        /// </summary>
        /// <param name="ct">自定义结构类</param>
        /// <returns></returns>
        static BoundField CreateBoundField(ViewTableColumnType ct)
        {

            BoundField bf = null;

            if (ct.DbType == LMFieldDBTypes.String)
            {
                if (ct.Is_Group)
                {
                    bf = new GroupColumn(ct.Text);
                }
                else
                {

                    bf = new BoundField(ct.Field, ct.Text);
                }
            }
            else if (ct.DbType == LMFieldDBTypes.Decimal)
            {

                if (ct.Is_Total)
                {
                    bf = new NumColumn() { DataField = ct.Field, HeaderText = ct.Text, Format = "0.00", SummaryType = "SUM", SummaryFormat = "合计:{0}", Width = 200 };

                }
                else
                {
                    bf = new NumColumn() { DataField = ct.Field, HeaderText = ct.Text, Format = "0.00" };
                }


            }
            else if (ct.DbType == LMFieldDBTypes.Date)
            {

                bf = new DateColumn() { DataField = ct.Field, HeaderText = ct.Text, Format = ct.Format };

            }

            return bf;

        }

        static void CreateGroupColumn(GroupColumn gc, ViewTableColumnTree ct)
        {

            foreach (ViewTableColumnTree ct_sub in ct.childres)
            {

                ViewTableColumnType col_type = ct_sub.ct;


                BoundField bf = CreateBoundField(col_type);

                gc.Columns.Add(bf);

                if (ct_sub.is_has_children())
                {
                    GroupColumn gc_sub = bf as GroupColumn;

                    CreateGroupColumn(gc_sub, ct_sub);
                }
            }
        }





    }
}
