using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace App.InfoGrid2.Bll
{
    public static class BizCommonMgr
    {
        /// <summary>
        /// 新的自动id值
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="tSqlWhere"></param>
        /// <returns></returns>
        public static int NewIdentity(string table, string field, string tSqlWhere)
        {
            return NewIdentity(table, field, tSqlWhere,0);
        }

        /// <summary>
        /// 新的自动id值
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="tSqlWhere"></param>
        /// <returns></returns>
        public static int NewIdentity(string table, string field, string tSqlWhere, int defaultValue)
        {
            int newId = 0;



            if (Transaction.Current == null)
            {
                newId = NewId(table, field, tSqlWhere, defaultValue);
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {

                    newId = NewId(table, field, tSqlWhere, defaultValue);


                }
            }


            return newId;
        }

        private static int NewId(string table, string field, string tSqlWhere, int defaultValue)
        {
            int newId = 0;

            using (DbDecipher tmpDecipher = DbDecipherManager.GetDecipherOpen())
            {
                string sql = $"update {table} set {field} = {field} + 1 " +
                    $"OUTPUT Inserted.{field} as [After] ";

                if (!string.IsNullOrEmpty(tSqlWhere))
                {
                    sql += "WHERE " + tSqlWhere;
                }

                newId = tmpDecipher.ExecuteScalar<int>(sql);
            }


            if (newId == 0)
            {
                newId = defaultValue;
            }

            return newId;
        }






        /// <summary>
        /// 新的自动id值
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="tSqlWhere"></param>
        /// <returns></returns>
        public static int NewIdentity(DbDecipher decipher, string table, string field, string tSqlWhere, int defaultValue)
        {
            int newId = 0;



            if (Transaction.Current == null)
            {
                newId = NewId(decipher,table, field, tSqlWhere, defaultValue);
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {

                    newId = NewId(decipher,table, field, tSqlWhere, defaultValue);


                }
            }


            return newId;
        }

        private static int NewId(DbDecipher decipher, string table, string field, string tSqlWhere, int defaultValue)
        {
            int newId = 0;


            string sql = $"update {table} set {field} = {field} + 1 " +
                $"OUTPUT Inserted.{field} as [After] ";

            if (!string.IsNullOrEmpty(tSqlWhere))
            {
                sql += "WHERE " + tSqlWhere;
            }

            newId = decipher.ExecuteScalar<int>(sql);



            if (newId == 0)
            {
                newId = defaultValue;
            }

            return newId;
        }
    }
}
