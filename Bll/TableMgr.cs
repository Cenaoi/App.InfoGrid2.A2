using System;
using System.Collections.Generic;
using System.Text;
using App.BizCommon;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity;
using HWQ.Entity.Xml;
using HWQ.Entity.Filter;
using EC5.Utility;
using Newtonsoft.Json;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.Bll
{



    public static class TableMgr 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// 获取实体元素
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static LModelElement GetModelElem(string tableName)
        {
            if (StringUtil.IsBlank(tableName)) { throw new ArgumentNullException("tableName"); }

            LModelElement modelElem = null;

            if (StringUtil.IsBlank(tableName))
            {
                return null;
            }

            if (LModelDna.ContainsByName(tableName))
            {
                modelElem = LModelDna.GetElementByName(tableName);

                return modelElem;
            }


            TableSet tableSet = GetTableSet(tableName);

            modelElem = GetModelElem(tableSet);

            return modelElem;
        }

        /// <summary>
        /// 获取实体元素
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public static LModelElement GetModelElem(int tableId,string tableName)
        {
            LModelElement modelElem = null;

            if (StringUtil.IsBlank(tableName))
            {
                return null;
            }

            if (LModelDna.ContainsByName(tableName))
            {
                modelElem = LModelDna.GetElementByName(tableName);

                return modelElem;
            }


            LModelDna.BeginEdit();

            modelElem = CreateLModelElem(tableId);

            LModelDna.Add(modelElem);

            LModelDna.EndEdit();

            return modelElem;
        }

        /// <summary>
        /// 获取实体元素
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public static LModelElement GetModelElem(TableSet tableSet)
        {
            if (tableSet == null) { throw new ArgumentNullException("tableSet"); }

            string tableName = tableSet.Table.TABLE_NAME;
            LModelElement modelElem = null;

            if (LModelDna.ContainsByName(tableName))
            {
                modelElem = LModelDna.GetElementByName(tableName);

                return modelElem;
            }



            LModelDna.BeginEdit();

            try
            {
                modelElem = CreateLModelElem(tableSet);

                LModelDna.Add(modelElem);

                LModelDna.EndEdit();
            }
            catch (Exception ex)
            {
                LModelDna.EndEdit();
                throw new Exception("转换数据错误.",ex);
            }


            return modelElem;
        }

        /// <summary>
        /// 根据 IG2_TABLE 表，创建实体元素
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public static LModelElement CreateLModelElem(int tableId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tableSet = TableSet.Select(decipher, tableId);

            XmlModelElem xModelElem = CreateXModelElem(tableSet);

            LModelElement modelElem = ModelConvert.ToModelElem(xModelElem);

            return modelElem;

        }

        public static LModelElement CreateLModelElem(TableSet tableSet)
        {
            if (tableSet == null) { throw new ArgumentNullException("tableSet"); }

            XmlModelElem xModelElem = CreateXModelElem(tableSet);

            LModelElement modelElem = ModelConvert.ToModelElem(xModelElem);
            modelElem.Identity.Rule = LModelIdentityRule.Auto;

            return modelElem;
        }


        /// <summary>
        /// 根据 IG2_TABLE 表，创建实体元素
        /// </summary>
        /// <param name="tableSet"></param>
        /// <returns></returns>
        public static XmlModelElem CreateXModelElem(TableSet tableSet)
        {
            if (tableSet == null) { throw new ArgumentNullException("tableSet"); }

            IG2_TABLE table = tableSet.Table;

            XmlModelElem xModelElem = new XmlModelElem(table.TABLE_NAME);
            xModelElem.Description = table.DISPLAY;
            xModelElem.DbIdentity = table.IDENTITY_FIELD;

            SortedDictionary<string, IG2_TABLE_COL> cols = new SortedDictionary<string, IG2_TABLE_COL>();
        
            foreach (IG2_TABLE_COL col in tableSet.Cols)
            {
                if (col.ROW_SID == -3)
                {
                    continue;
                }

                if (cols.ContainsKey(col.DB_FIELD))
                {
                    log.ErrorFormat("IG2 数据表异常，对应不上实际表。 表“{0}:{1}”字段“{2}:{3}”重复。",
                        table.TABLE_NAME, table.DISPLAY, col.DB_FIELD, col.F_NAME);

                    continue;
                }

                cols.Add(col.DB_FIELD, col);

                XmlFieldElem fieldElem = new XmlFieldElem();

                fieldElem.Type = col.IS_VIEW_FIELD ? LModelFieldTypes.ViewField : LModelFieldTypes.General;
                
                

                fieldElem.DBField = col.DB_FIELD;
                fieldElem.PropName = col.DB_FIELD;

                if (!StringUtil.IsBlank(col.DB_TYPE))
                {
                    fieldElem.DBType = ModelConvert.ToDbType(col.DB_TYPE);
                }

                fieldElem.DecimalDigits = col.DB_DOT;
                fieldElem.MaxLen = col.DB_LEN;
                fieldElem.Caption = fieldElem.Description = col.DISPLAY;

                if (col.DB_DOT > 0)
                {
                    fieldElem.DecimalDigits = col.DB_DOT;
                }


                fieldElem.DefaultValue = GetDefaultValue(col.DEFAULT_VALUE);
                fieldElem.Mandatory = col.IS_MANDATORY;



                if (fieldElem.DBField == table.ID_FIELD)
                {
                    fieldElem.IsKey = true;
                }


                if (fieldElem.Type == LModelFieldTypes.ViewField )
                {
                    if (!StringUtil.IsBlank(col.VIEW_SQL))
                    {
                        XmlFieldVAttr xfvAttr = fieldElem.View = new XmlFieldVAttr();
                        xfvAttr.Mode = "TSql";
                        xfvAttr.TSql = col.VIEW_SQL; 
                    }
                    else if (!string.IsNullOrEmpty(col.VIEW_FIELD_CONFIG))
                    {
                        XmlFieldVAttr xfvAttr = fieldElem.View = new XmlFieldVAttr();
                        xfvAttr.Mode = "Tag";
                    }

                }

                xModelElem.Fields.Add(fieldElem);
            }
            
            return xModelElem;
        }





        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetDefaultValue(string defaultValue)
        {
            if (!defaultValue.StartsWith("(") || !defaultValue.EndsWith(")"))
            {
                return defaultValue;
            }

            string funCode = defaultValue.Substring(1, defaultValue.Length - 2).Trim();


            if (!StringUtil.EndsWith(funCode, "()"))
            {
                return defaultValue;
            }

            string funName = funCode.Substring(0, funCode.Length - 2);

            string funUpperName = funName.ToUpper();

            if (funUpperName == "GETDATE" || funUpperName == "NEWID")
            {
                return defaultValue;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据 TableID 获取 TableName
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public static string GetTableNameForId(int tableId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("IG2_TABLE_ID", tableId);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.Fields = new string[] { "TABLE_NAME" };
            filter.Locks.Add(LockType.NoLock);

            DbDecipher decipher = ModelAction.OpenDecipher();

            string tableName = decipher.ExecuteScalar<string>(filter);

            return tableName;
        }


        /// <summary>
        /// 根据表名，获取表实体
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IG2_TABLE GetTableForName(string tableName)
        {
            if (StringUtil.IsBlank(tableName)) { throw new ArgumentNullException("tableName"); }
                    
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_NAME", tableName);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.Locks.Add(LockType.NoLock);


            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(filter);


            return table;
        }


        /// <summary>
        /// 获取表ID
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int GetTableId(string table)
        {
            if (StringUtil.IsBlank(table)) { throw new ArgumentNullException("table"); }

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_NAME", table);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.Fields = new string[] { "IG2_TABLE_ID" };
            filter.Locks.Add(LockType.NoLock);


            DbDecipher decipher = ModelAction.OpenDecipher();

            int tableId = decipher.ExecuteScalar<int>(filter);

            return tableId;

        }

        /// <summary>
        /// 按表名获取
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static LModel GetLTableForName(string table,string[] fields)
        {
            if (StringUtil.IsBlank(table)) { throw new ArgumentNullException("table"); }

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("TABLE_NAME", table);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.Fields = fields;// new string[] { "IG2_TABLE_ID", "TABLE_NAME", "DISPLAY" };
            filter.Locks.Add(LockType.NoLock);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel model = decipher.GetModel(filter);
            
            return model;

        }

        /// <summary>
        /// 根据表id，或字段集合
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<LModel> GetLCols(int tableId,string[] fields)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE_COL));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("IG2_TABLE_ID", tableId);
            filter.Fields = fields;
            filter.Locks.Add(LockType.NoLock);

            DbDecipher decipher = ModelAction.OpenDecipher();

            IList<LModel> models = decipher.GetModelList(filter);

            return models;

        }


        /// <summary>
        /// 获取表结构（以后下面的代码需要加入缓冲机制)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static TableSet GetTableSet(string table)
        {
            if (StringUtil.IsBlank(table)) { throw new ArgumentNullException("table"); }

            int tableId = GetTableId(table);

            if (tableId <= 0)
            {
                return null;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, tableId);

            return tSet;
        }



        /// <summary>
        /// 获取表结构（以后下面的代码需要加入缓冲机制)
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static TableSet GetTableSet_0_5(string table)
        {
            if (StringUtil.IsBlank(table)) { throw new ArgumentNullException("table"); }

            int tableId = GetTableId(table);

            if (tableId <= 0)
            {
                return null;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectSID_0_5(decipher, tableId);


            return tSet;
        }


        /// <summary>
        /// 获取数据的 T-SQL 语句
        /// </summary>
        /// <param name="tSet"></param>
        /// <returns></returns>
        public static string GetCountForTSql(TableSet tSet)
        {
            return GetCountForTSql(tSet, null);
        }

        /// <summary>
        /// 获取数据的 T-SQL 语句
        /// </summary>
        /// <param name="tSet"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public static string GetCountForTSql(TableSet tSet, string filter2)
        {
            if (tSet == null) { throw new ArgumentNullException("tSet"); }

            List<string> fields = new List<string>();

            string tSqlSelect = "COUNT(*) ";    // GetTSqlSelect(tSet, ref fields);

            string tSqlForm = GetTSqlForm(tSet);

            string tSqlWhere = GetTSqlWhere(tSet);

            //string tSqlOrder = GetTSqlOrder(tSet, fields);

            StringBuilder tSql = new StringBuilder();

            
            tSql.Append("SELECT ");

            tSql.Append(tSqlSelect);
            
            tSql.AppendLine().Append(" FROM ").Append(tSqlForm);


            if (!StringUtil.IsBlank(filter2))
            {
                if (StringUtil.IsBlank(tSqlWhere))
                {
                    tSqlWhere = filter2;
                }
                else
                {
                    tSqlWhere += " AND " + filter2;
                }
            }

            if (!StringUtil.IsBlank(tSqlWhere))
            {
                tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
            }

            //if (!StringUtil.IsBlank(tSqlOrder))
            //{
            //    tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
            //}

            return tSql.ToString();
        }


        public static string GetTSql(TableSet tSet, int startRow, int pageSize, string filter2)
        {
            if (tSet == null) { throw new ArgumentNullException("tSet"); }

            List<string> fields = new List<string>();

            string tSqlSelect = GetTSqlSelect(tSet, ref fields);

            string tSqlForm = GetTSqlForm(tSet);

            string tSqlWhere = GetTSqlWhere(tSet);

            string tSqlOrder = GetTSqlOrder(tSet, fields);

            StringBuilder tSql = new StringBuilder();


            tSql.AppendLine("SELECT * FROM (");
            {
                tSql.Append("SELECT ");

                tSql.Append(tSqlSelect);

                tSql.AppendFormat(",row_number()  over(order by {0}) as ROW_NUMBER ", tSet.Table.ID_FIELD);

                tSql.AppendLine().Append(" FROM ").Append(tSqlForm);


                if (!StringUtil.IsBlank(filter2))
                {
                    if (StringUtil.IsBlank(tSqlWhere))
                    {
                        tSqlWhere = filter2;
                    }
                    else
                    {
                        tSqlWhere += " AND " + filter2;
                    }
                }

                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
                }

                if (!StringUtil.IsBlank(tSqlOrder))
                {
                    tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
                }
            }

            tSql.AppendLine().Append(") as T where ");
            tSql.AppendFormat("ROW_NUMBER between {0} and {1}", startRow + 1, startRow + pageSize);



            return tSql.ToString();
        }



        public static string GetTSqlWhere(TableSet vSet)
        {
            StringBuilder sb = new StringBuilder();

            int n = 0;

            for (int i = 0; i < vSet.Cols.Count; i++)
            {
                IG2_TABLE_COL vField = vSet.Cols[i];

                if (StringUtil.IsBlank(vField.FILTER_LOGIC))
                {
                    continue;
                }

                string fieldTWhere = GetFieldFilter(vField);

                if (n++ > 0)
                {
                    sb.Append(" AND ");
                }

                sb.Append(fieldTWhere);

            }

            return sb.ToString();
        }

        private static string GetFieldFilter(IG2_TABLE_COL vField)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(vField.DB_FIELD).Append(" ");

            if (vField.FILTER_LOGIC == "==")
            {
                vField.FILTER_LOGIC = "=";
            }


            sb.Append(vField.FILTER_LOGIC).Append(" ");

            if (vField.DB_TYPE == "string")
            {
                sb.Append("'").Append(vField.FILTER_VALUE.Replace("'","''")).Append("'");
            }
            else
            {
                sb.Append(vField.FILTER_VALUE);
            }

            return sb.ToString();
        }


        public static string GetTSqlSelect(TableSet vSet, ref List<string> outFields)
        {
            if (vSet == null) { throw new ArgumentNullException("vSet"); }

            List<string> fields = outFields;    // new List<string>();

            StringBuilder sb = new StringBuilder();


            string field;

            IG2_TABLE table = vSet.Table;

            for (int i = 0; i < vSet.Cols.Count; i++)
            {
                IG2_TABLE_COL vField = vSet.Cols[i];

                if (vField.SEC_LEVEL <= 6 && vField.ROW_SID < 0)
                {
                    continue;
                }

                field = vField.DB_FIELD;

                if (fields.Contains(field))
                {
                    continue;
                }

                fields.Add(field);

                if (i > 0) { sb.Append(", "); }

                sb.Append(field);
            }

            return sb.ToString();
        }


        public static string GetTSqlForm(TableSet vSet)
        {

            if (vSet == null) { throw new ArgumentNullException("vSet"); }

            IG2_TABLE table = vSet.Table;

            return table.TABLE_NAME;
        }


        public static string GetTSqlOrder(TableSet vSet, List<string> fields)
        {
            return string.Empty;
        }



    }
}
