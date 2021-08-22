using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace EC5.WScript
{
    partial class VBA
    {
        /// <summary>
        /// 计算数据库中满足指定条件的非空单元格个数。
        /// </summary>
        /// <param name="database">Database 构成列表或数据库的单元格区域。
        /// 数据库是包含一组相关数据的列表，其中包含相关信息的行为记录，而包含数据的列为字段。
        /// 列表的第一行包含着每一列的标志项。 
        /// </param>
        /// <param name="field">指定函数所使用的数据列。列表中的数据列必须在第一行具有标志项。
        /// Field 可以是文本，即两端带引号的标志项，如“树龄”或“产量”；
        /// 此外，Field 也可以是代表列表中数据列位置的数字：1 表示第一列，2 表示第二列，等等。 
        /// </param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DCOUNTA(object database, object field, object criteria)
        {
            return -1;
        }

        /// <summary>
        /// 计算数据库中包含指定条件数字的单元格个数。 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="field"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DCOUNT(object database, object field, object criteria)
        {
            return -1;
        }


        /// <summary>
        /// 从数据库提取符合指定条件的单个记录。
        /// </summary>
        /// <param name="database"></param>
        /// <param name="field"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DGET(DbDecipher decipher, object database, object field, object criteria)
        {
            DbDecipher decipherX = decipher;

            object result = null;

            if (database.GetType() == typeof(string))
            {
                string table = Convert.ToString(database);

                string tWhere = Convert.ToString(criteria);


                string tSql = $"SELECT TOP(1) {field} FROM {table} ";

                if (!string.IsNullOrEmpty(tWhere))
                {
                    tSql += "WHERE " + tWhere;
                }
                
                result = decipherX.ExecuteScalar(tSql);
                
            }

            return result;
        }



        private static object DB_ExecuteScalar(string tSql)
        {
            object result = null;

            if (Transaction.Current != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        result = decipher.ExecuteScalar(tSql);
                    }
                }
            }
            else
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    result = decipher.ExecuteScalar(tSql);
                }
            }

            return result;
        }

        private static int DB_ExecuteNonQuery(string tSql)
        {
            int result = 0;

            if (Transaction.Current != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        result = decipher.ExecuteNonQuery(tSql);
                    }
                }
            }
            else
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    result = decipher.ExecuteNonQuery(tSql);
                }
            }

            return result;
        }




        /// <summary>
        /// 从数据库提取符合指定条件的单个记录。
        /// </summary>
        /// <param name="database"></param>
        /// <param name="field"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DGET(object database, object field, object criteria)
        {
            
            object result = null;

            if (database.GetType() == typeof(string))
            {
                string table = Convert.ToString(database);

                string tWhere = Convert.ToString(criteria);


                string tSql = $"SELECT TOP(1) {field} FROM {table} ";

                if (!string.IsNullOrEmpty(tWhere))
                {
                    tSql += "WHERE " + tWhere;
                }

                result = DB_ExecuteScalar(tSql);

            }

            return result;
        }

        public static dynamic DMAX(object database, object field, object criteria)
        {
            return -1;
        }

        public static dynamic DMIN(object database, object field, object criteria)
        {
            return -1;
        }


        public static dynamic DSUM(object database, object field, object criteria)
        {
            return -1;
        }

        public static dynamic DVAR(object database, object field, object criteria)
        {
            return -1;
        }


        public static dynamic DVARP(object database, object field, object criteria)
        {
            return -1;
        }

        public static dynamic DPRODUCT(object database, object field, object criteria)
        {
            return -1;
        }


        public static dynamic DSTDEV(object database, object field, object criteria)
        {
            return -1;
        }


        public static dynamic DSTDEVP(object database, object field, object criteria)
        {
            return -1;
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="database"></param>
        /// <param name="field"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DAVERAGE(object database, object field, object criteria)
        {
            if(database == null)
            {
                return null;
            }

            string fieldStr = Convert.ToString(field);

            decimal total = 0;

            int count = 0;

            if(!(database is Array))
            {
                database = new object[] { database };
            }

            if(database is Array)
            {
                Array list = (Array)database;

                foreach (var item in list)
                {
                    if(item is LightModel || item is SModel)
                    {
                        object v = _GetModelValue(item, fieldStr);

                        if (v == null)
                        {
                            continue;
                        }

                        total += Convert.ToDecimal(v);
                    }
                }

                count = list.Length;
            }

            return total / count;
        }


        private static IList _ToList(object value)
        {

            Type valueT = value.GetType();

            IList list = null;

            if (value is IList)
            {
                list = value as IList;

                if (list == null || list.Count == 0)
                {
                    return null;
                }

            }
            else if (valueT.IsValueType || valueT == typeof(string))
            {
                list = new object[] { value };
            }

            return list;

        }


        /// <summary>
        /// 把条件对象转换为 T-SQL Where 语句
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private static string _ToWhere(object criteria)
        {
            if(criteria == null)
            {
                throw new ArgumentNullException("criteria", "条件参数不能为空.");
            }

            if (criteria.GetType() == typeof(string))
            {
                return (string)criteria;
            }
            else if (criteria is SModel)
            {
                SModel sm = (SModel)criteria;

                if(sm.GetFieldCount() == 0)
                {
                    return null;
                }


                StringBuilder sb = new StringBuilder();

                int n = 0;
                
                sb.Append("(");
                

                foreach (var item in sm.GetFields())
                {
                    if(n ++ > 0)
                    {
                        sb.Append(" AND ");
                    }

                    sb.Append("(");

                    sb.Append(item);
                    sb.Append(" = ");
                    
                    object value = sm[item];
                    Type valueT = value.GetType();

                    if (value == null)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        if (TypeUtil.IsNumberType(valueT))
                        {
                            sb.Append(value);
                        }
                        else if(valueT == typeof(bool))
                        {
                            sb.Append(((bool)value) ? "1" : "0");
                        }
                        else
                        {
                            sb.Append("'");
                            sb.Append(value.ToString().Replace("'", "''"));
                            sb.Append("'");
                        }
                    }

                    sb.Append(")");
                }

                sb.Append(")");

                return sb.ToString();
            }
            else
            {
                throw new Exception("错误类型: criteria Type = " + criteria.GetType());
            }
        }


        /// <summary>
        /// 循环查找值
        /// </summary>
        /// <param name="lookup_value"></param>
        /// <param name="table"></param>
        /// <param name="col_name"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DLOOKUP(DbDecipher decipher, object lookup_value, string table, object col_name, object criteria)
        {
            if(lookup_value == null)
            {
                return null;
            }

            IList list = _ToList(lookup_value);

            if(list == null) { return null; }


            List<object> finds = null;
            
            foreach (var item in list)
            {
                string itemStr = item.ToString().Replace("'","''");

                string tSql = $"select top(1) {col_name} from {table} where {col_name} = '{itemStr}' ";

                if (criteria != null)
                {
                    string tWhere = _ToWhere(criteria);

                    tSql += "AND (" + tWhere + ") ";
                }

                object itemResult = decipher.ExecuteScalar(tSql);

                if(!StringUtil.IsBlank(itemResult))
                {
                    if(finds == null) { finds = new List<object>(); }

                    finds.Add(item);
                }
            }
            
            return finds;
        }
        

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="delete_value"></param>
        /// <param name="table"></param>
        /// <param name="col_name"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DDELETE(DbDecipher decipher, object delete_value, string table, object col_name, object criteria)
        {
            int deleteCount = 0;    //删除的记录数

            IList list = null;

            if (delete_value != null)
            {
                list = _ToList(delete_value);
            }

            string deleteValueTWhere = null;

            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                int n = 0;
                foreach (var item in list)
                {
                    if (n++ > 0) { sb.Append(", "); }
                    sb.Append(item.ToString().Replace("'", "''"));
                }

                deleteValueTWhere = sb.ToString();
            }

            string tWhere = _ToWhere(criteria);

            if (!StringUtil.IsBlank(deleteValueTWhere))
            {
                tWhere = $"{col_name} in ({deleteValueTWhere}) ";
            }

            string tSql = $"delete {table} ";
                        
            if (!StringUtil.IsBlank(tWhere))
            {
                tSql += " where " + tWhere;
            }

            deleteCount = decipher.ExecuteNonQuery(tSql);


            return deleteCount;
        }




        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="update_value"></param>
        /// <param name="table"></param>
        /// <param name="col_name"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static dynamic DINSERT(DbDecipher decipher, object update_value, string table, object col_name, object criteria)
        {
            string colStr = Convert.ToString(col_name);

            int deleteCount = 0;    //删除的记录数

            IList list = _ToList(update_value);

            if (list == null) { return 0; }

            List<LModel> newList = new List<LModel>();


            SModel criteriaSModel = null;

            if (criteria is SModel)
            {
                criteriaSModel = (SModel)criteria;
            }

            foreach (var item in list)
            {
                LModel model = new LModel(table);

                if (criteriaSModel != null)
                {
                    _CopyFields(criteriaSModel, model);
                }

                model[colStr] = item;

                newList.Add(model);
            }

            decipher.InsertModels(newList);


            return newList;
        }
        

        private static void _CopyFields(SModel from, LModel toModel)
        {
            foreach (var smField in from.GetFields())
            {
                toModel[smField] = from[smField];
            }
        }


        /// <summary>
        /// 获取实体值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static object _GetModelValue(object item, string field)
        {
            object value = null;

            if (item is LightModel)
            {
                value = ((LightModel)item).GetValue(field);
            }
            else if (item is SModel)
            {
                value = ((SModel)item).Get(field);
            }

            return value;
        }
    }
}
