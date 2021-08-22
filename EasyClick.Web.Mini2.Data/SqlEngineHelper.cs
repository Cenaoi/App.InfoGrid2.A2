using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    static class SqlEngineHelper
    {


        internal static void GetTSQLWhereForFilter_RangeControl(StringBuilder sb, Param filterItem)
        {
            if (sb == null) { throw new ArgumentNullException("sb"); }
            if (filterItem == null) { throw new ArgumentNullException("filterItem"); }

            IRangeControl rangeCon = (IRangeControl)filterItem;

            string startV = rangeCon.StartValue;
            string endV = rangeCon.EndValue;

            bool isEmptyStart = string.IsNullOrEmpty(startV);
            bool isEmptyEnd = string.IsNullOrEmpty(endV);

            if (isEmptyStart && isEmptyEnd)
            {
                return;
            }

            sb.Append("(");

            if (!isEmptyStart)
            {
                sb.AppendFormat("{0} {1} '{2}'",
                    filterItem.Name,
                    StringUtil.NoBlank(filterItem.Logic, ">="),
                    startV.Replace("'", "''"));
            }

            if (!isEmptyEnd)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }

                sb.AppendFormat("{0} {1} '{2}'",
                    filterItem.Name,
                    StringUtil.NoBlank(filterItem.Logic, "<="),
                    endV.Replace("'", "''"));
            }
            

            sb.Append(")");
        }


        /// <summary>
        /// 获取记录的总数量
        /// </summary>
        /// <param name="storeUI"></param>
        /// <returns></returns>
        internal static string GetCountTSql(Store storeUI)
        {
            if (storeUI == null) { throw new ArgumentNullException("storeUI"); }


            Store store = storeUI;

            StoreTSqlQuerty tSqlQuery = store.TSqlQuery;

            string filterWhere;

            try
            {
                filterWhere = GetTSQLWhereForFilter(storeUI);
            }
            catch (Exception ex)
            {
                throw new Exception("获取TSQL Where 过滤参数错误。", ex);
            }

            string tSqlWhere = tSqlQuery.Where;

            if (filterWhere.Length > 0)
            {
                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSqlWhere += " AND ";
                }

                tSqlWhere += filterWhere.ToString();
            }



            StringBuilder tSql = new StringBuilder();

            tSql.Append("SELECT ");

            tSql.AppendFormat("COUNT({0}) ", StringUtil.NoBlank(tSqlQuery.IdField, "*"));

            tSql.AppendFormat("\nFROM {0} ", tSqlQuery.Form);

            //tSql.Append("WITH(NOLOCK) ");

            IfUtil.NotBlank_AppendFormat(tSql, "WHERE {0}", tSqlWhere);


            return tSql.ToString();
        }


        internal static string GetTSQLWhereForFilter(Store storeUI)
        {
            if (storeUI == null) { throw new ArgumentNullException("storeUI"); }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < storeUI.FilterParams.Count; i++)
            {
                Param filterItem = storeUI.FilterParams[i];

                if (filterItem.ParamMode == StoreParamMode.TSql)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" AND ");
                    }

                    TSqlWhereParam tswp = filterItem as TSqlWhereParam;

                    if (tswp != null)
                    {
                        sb.Append(tswp.Where);
                    }
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" AND ");
                    }

                    if (filterItem is IRangeControl)
                    {
                        GetTSQLWhereForFilter_RangeControl(sb, filterItem);
                    }
                    else
                    {
                        GetTSQLWhereForFilter_Default(sb, filterItem);
                    }
                }
            }

            return sb.ToString();

        }



        internal static void GetTSQLWhereForFilter_Default(StringBuilder sb, Param filterItem)
        {


            if (sb == null) { throw new ArgumentNullException("sb"); }
            if (filterItem == null) { throw new ArgumentNullException("filterItem"); }

            string value;

            if (filterItem.IsInnerValue)
            {
                value = filterItem.InnerValue.ToString();
            }
            else
            {
                value = filterItem.DefaultValue;
            }

            sb.Append("(");

            if (filterItem.Logic == "like")
            {                
                if (!value.Contains("%"))
                {
                    value = "%" + value + "%";
                }

                sb.AppendFormat("{0} {1} '{2}'",
                    filterItem.Name,
                    StringUtil.NoBlank(filterItem.Logic, "="),
                    value.Replace("'", "''"));
            }
            else
            {
                sb.AppendFormat("{0} {1} '{2}'",
                    filterItem.Name,
                    StringUtil.NoBlank(filterItem.Logic, "="),
                    value.Replace("'", "''"));
            }

            sb.Append(")");
        }



        /// <summary>
        /// 获取 T-SQL 语句
        /// </summary>
        /// <param name="storeUI"></param>
        /// <param name="page">页码</param>
        /// <param name="isPager">是否分页</param>
        /// <returns></returns>
        internal static string GetTSQL(Store storeUI, int page, bool isPager)
        {
            if (storeUI == null) { throw new ArgumentNullException("storeUI"); }

            Store store = storeUI;

            StoreTSqlQuerty tSqlQuery = store.TSqlQuery;

            StringBuilder tSql = new StringBuilder();


            string orderBy = tSqlQuery.OrderBy;

            DataRequest dr = store.GetAction();


            if (!StringUtil.IsBlank(dr.TSqlSort))
            {
                orderBy = dr.TSqlSort;
            }

            string filterWhere;

            try
            {
                filterWhere = GetTSQLWhereForFilter(storeUI);
            }
            catch (Exception ex)
            {
                throw new Exception("获取TSQL Where 过滤参数错误。", ex);
            }

            string tSqlWhere = tSqlQuery.Where;

            if (filterWhere.Length > 0)
            {
                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSqlWhere += " AND ";
                }

                tSqlWhere += filterWhere;
            }

            if (isPager)
            {
                int startRow = page * store.PageSize + 1;
                int endRow = (page + 1) * store.PageSize;

                tSql.AppendLine("SELECT * FROM ( ");
                {
                    tSql.Append("SELECT ").Append(tSqlQuery.Select);

                    if (!StringUtil.IsBlank(orderBy))
                    {
                        tSql.AppendFormat(",row_number() over(order by {0}) as ROW_NUMBER ", orderBy);
                    }
                    else
                    {
                        tSql.Append(",row_number() as ROW_NUMBER ");
                    }

                    tSql.AppendFormat("\nFROM {0} ", tSqlQuery.Form);

                    //tSql.Append("WITH (NOLOCK) ");  //读取数据并不锁住

                    IfUtil.NotBlank_AppendFormat(tSql, "WHERE {0} ", tSqlWhere);
                }

                tSql.Append("\n) as T ");
                tSql.AppendFormat("WHERE (ROW_NUMBER between {0} and {1})", startRow, endRow);
            }
            else
            {
                tSql.Append("SELECT ").Append(tSqlQuery.Select);

                if (!StringUtil.IsBlank(orderBy))
                {
                    tSql.AppendFormat(",row_number() over(order by {0}) as ROW_NUMBER ", orderBy);
                }
                else
                {
                    tSql.Append(",row_number() as ROW_NUMBER ");
                }

                tSql.AppendFormat("\nFROM {0} ", tSqlQuery.Form);

                //tSql.Append("WITH (NOLOCK) ");  //读取数据并不锁住

                IfUtil.NotBlank_AppendFormat(tSql, "WHERE {0} ", tSqlWhere);
            }


            return tSql.ToString();
        }


        internal static string GetSqlSort(Store store, DataRequest dr)
        {
            if (store == null) { throw new ArgumentNullException("store"); }
            if (dr == null) { throw new ArgumentNullException("dr"); }

            if (!StringUtil.IsBlank(dr.TSqlSort))
            {
                return dr.TSqlSort;
            }

            if (string.IsNullOrEmpty(dr.TSqlSort) && !string.IsNullOrEmpty(store.SortText))
            {
                return store.SortText;
            }
            else if (!string.IsNullOrEmpty(store.SortField))
            {
                return string.Format("{0} ASC,{1} ASC", store.SortField, store.IdField);
            }

            return string.Empty;
        }
    }
}
