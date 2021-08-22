using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using App.BizCommon.Models;
using HWQ.Entity.Decipher.LightDecipher;
using System.Transactions;

namespace App.BizCommon
{
    /// <summary>
    /// 规则代码类型
    /// </summary>
    public enum BillCodeType
    {
        /// <summary>
        /// 数字
        /// </summary>
        Num,
        /// <summary>
        /// 年份
        /// </summary>
        Year,
        /// <summary>
        /// 年份 + 月份
        /// </summary>
        Month,
        /// <summary>
        /// 年份 + 月份 + 日
        /// </summary>
        Day
    }

    /// <summary>
    /// 单据编号管理
    /// </summary>
    public static class BillIdentityMgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取新订单编号
        /// </summary>
        /// <param name="billType">业务类型代码</param>
        /// <param name="prefix">单号前缀</param>
        /// <param name="seqLen">顺序号长度</param>
        /// <param name="codeType">代码类型</param>
        /// <returns></returns>
        public static string NewCode(BillCodeType codeType, string billType, string prefix, int seqLen)
        {
            string codeStr;

            switch (codeType)
            {
                case BillCodeType.Year:
                    codeStr = NewCodeForYear(billType, prefix, seqLen);
                    break;
                case BillCodeType.Month:
                    codeStr = NewCodeForMonth(billType, prefix, seqLen);
                    break;
                case BillCodeType.Day:
                    codeStr = NewCodeForDay(billType, prefix, seqLen);
                    break;
                case BillCodeType.Num:
                    codeStr = NewCodeForNum(billType, prefix, seqLen);
                    break;
                default:
                    codeStr = string.Empty;
                    break;
            }

            return codeStr;
        }

        /// <summary>
        /// 获取新订单编号
        /// </summary>
        /// <param name="billType">业务类型代码</param>
        /// <param name="prefix">单号前缀</param>
        /// <param name="seqLen">顺序号长度</param>
        /// <param name="codeType">代码类型</param>
        /// <returns></returns>
        public static string NewCode(BillCodeType codeType, string billType, string prefix, string separatorString, int seqLen)
        {
            string codeStr;

            switch (codeType)
            {
                case BillCodeType.Year:
                    codeStr = NewCodeForYear(billType, prefix, separatorString, seqLen);
                    break;
                case BillCodeType.Month:
                    codeStr = NewCodeForMonth(billType, prefix, separatorString, seqLen);
                    break;
                case BillCodeType.Day:
                    codeStr = NewCodeForDay(billType, prefix, separatorString, seqLen);
                    break;
                case BillCodeType.Num:
                    codeStr = NewCodeForNum(billType, prefix, seqLen);
                    break;
                default:
                    codeStr = string.Empty;
                    break;
            }

            return codeStr;
        }





        /// <summary>
        /// 获取顺序号的
        /// </summary>
        /// <param name="billType">业务类型代码</param>
        /// <param name="prefix">代码前缀</param>
        /// <param name="seqLen">代码长度</param>
        /// <returns></returns>
        public static string NewCodeForNum(string billType, string prefix, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            //string billType = "DOC_CZTB";

            string newCode;

            if (Transaction.Current == null)
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    newCode = NewCodeForNum(decipher, billType, prefix, seqLen);
                }
            }
            else
            {
                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        newCode = NewCodeForNum(decipher, billType, prefix, seqLen);
                    }
                }
            }

            return newCode;
        }

        private static string NewCodeForNum(DbDecipher decipher, string billType, string prefix, int seqLen = 4)
        {

            DateTime now = DateTime.Now;
            string nowId = "";


            try
            {
                if (!decipher.ExistsModels<C_BILL_IDENTITY>("BILL_TYPE_ID='{0}' ", billType))
                {
                    C_BILL_IDENTITY m = new C_BILL_IDENTITY();
                    m.BILL_TYPE_ID = billType;
                    m.BILL_YEAR = 0;
                    m.BILL_MONTH = 0;
                    m.BILL_DAY = 0;

                    decipher.InsertModel(m);
                }

                string sqlFormat = "Update C_BILL_IDENTITY Set BILL_IDENTITY = (BILL_IDENTITY + 1)  WHERE BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";
                string sqlFormat2 = "Select top 1 BILL_IDENTITY From C_BILL_IDENTITY Where BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";

                string sql = string.Format(sqlFormat, 0, 0, 0, billType);
                string sql2 = string.Format(sqlFormat2, 0, 0, 0, billType);


                int n = decipher.ExecuteNonQuery(sql);
                int id = decipher.ExecuteScalar<int>(sql2);


                string yearStr = now.Year.ToString();

                string idStr = id.ToString();

                nowId = prefix + idStr.PadLeft(seqLen, '0');

            }
            catch (Exception ex)
            {
                log.Error(ex);

                nowId = "错误单号";
            }


            return nowId;
        }



        /// <summary>
        /// 获取新订单编号(按年份)
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="prefix"></param>
        /// <param name="separatorString"></param>
        /// <param name="seqLen"></param>
        /// <returns></returns>
        public static string NewCodeForYear(string billType, string prefix, string separatorString, int seqLen = 4)
        {
            string newCode;

            if (Transaction.Current == null)
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    newCode = NewCodeForYear(decipher, billType, prefix, separatorString, seqLen);
                }
            }
            else
            {
                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        newCode = NewCodeForYear(decipher, billType, prefix, separatorString, seqLen);
                    }
                }
            }

            return newCode;
        }


        private static string NewCodeForYear(DbDecipher decipher, string billType, string prefix, string separatorString, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            //string billType = "DOC_CZTB";

            DateTime now = DateTime.Now;
            string nowId = "";


            try
            {
                if (!decipher.ExistsModels<C_BILL_IDENTITY>("BILL_TYPE_ID='{0}' AND BILL_YEAR={1} ", billType, now.Year))
                {
                    C_BILL_IDENTITY m = new C_BILL_IDENTITY();
                    m.BILL_TYPE_ID = billType;
                    m.BILL_YEAR = now.Year;
                    m.BILL_MONTH = 0;
                    m.BILL_DAY = 0;

                    decipher.InsertModel(m);
                }

                string sqlFormat = "Update C_BILL_IDENTITY Set BILL_IDENTITY = (BILL_IDENTITY + 1)  WHERE BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";
                string sqlFormat2 = "Select top 1 BILL_IDENTITY From C_BILL_IDENTITY Where BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";

                string sql = string.Format(sqlFormat, now.Year, 0, 0, billType);
                string sql2 = string.Format(sqlFormat2, now.Year, 0, 0, billType);


                int n = decipher.ExecuteNonQuery(sql);
                int id = decipher.ExecuteScalar<int>(sql2);


                string yearStr = now.Year.ToString();

                string idStr = id.ToString();

                string year2 = yearStr.Substring(2, 2);

                string idStrX = idStr.PadLeft(seqLen, '0');

                nowId = string.Concat(prefix, year2, separatorString, idStrX);

            }
            catch (Exception ex)
            {
                log.Error(ex);

                nowId = "错误单号";
            }

            return nowId;
        }


        /// <summary>
        /// 获取新订单编号(按年份)
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="prefix"></param>
        /// <param name="seqLen"></param>
        /// <returns></returns>
        public static string NewCodeForYear(string billType, string prefix, int seqLen = 4)
        {
            return NewCodeForYear(billType, prefix, null, seqLen);
        }

        /// <summary>
        /// 获取新订单编号(按月份)
        /// </summary>
        /// <returns></returns>
        public static string NewCodeForMonth(DbDecipher decipher, string billType, string prefix, string separatorString, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            //string billType = "DOC_CZTB";

            DateTime now = DateTime.Now;
            string nowId = "";

            try
            {
                if (!decipher.ExistsModels<C_BILL_IDENTITY>("BILL_TYPE_ID='{0}' AND BILL_YEAR={1} AND BILL_MONTH={2}", billType, now.Year, now.Month))
                {
                    C_BILL_IDENTITY m = new C_BILL_IDENTITY();
                    m.BILL_TYPE_ID = billType;
                    m.BILL_YEAR = now.Year;
                    m.BILL_MONTH = now.Month;
                    m.BILL_DAY = 0;

                    decipher.InsertModel(m);
                }

                string sqlFormat = "Update C_BILL_IDENTITY Set BILL_IDENTITY = (BILL_IDENTITY + 1)  WHERE BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";
                string sqlFormat2 = "Select top 1 BILL_IDENTITY From C_BILL_IDENTITY Where BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";

                string sql = string.Format(sqlFormat, now.Year, now.Month, 0, billType);
                string sql2 = string.Format(sqlFormat2, now.Year, now.Month, 0, billType);

                //SqlConnection conn = decipher.Connection as SqlConnection;

                //SqlCommand command = conn.CreateCommand();


                int n = decipher.ExecuteNonQuery(sql);
                int id = decipher.ExecuteScalar<int>(sql2);


                string yearStr = now.Year.ToString();

                string idStr = id.ToString();

                string year2 = yearStr.Substring(2, 2);

                string idStrX = idStr.PadLeft(seqLen, '0');

                nowId = string.Concat(prefix, string.Format("{0}{1:00}", year2, now.Month), separatorString, idStrX);

            }
            catch (Exception ex)
            {
                log.Error(ex);

                nowId = "错误单号";
            }


            return nowId;
        }

        public static string NewCodeForMonth(string billType, string prefix, string separatorString, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            //string billType = "DOC_CZTB";

            string newCode;

            if (Transaction.Current == null)
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    newCode = NewCodeForMonth(decipher, billType, prefix, separatorString, seqLen);
                }
            }
            else
            {
                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        newCode = NewCodeForMonth(decipher, billType, prefix, separatorString, seqLen);
                    }
                }
            }

            return newCode;
        }

        /// <summary>
        /// 获取新订单编号(按月份)
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="prefix"></param>
        /// <param name="seqLen"></param>
        /// <returns></returns>
        public static string NewCodeForMonth(string billType, string prefix, int seqLen = 4)
        {
            return NewCodeForMonth(billType, prefix, null, seqLen);
        }


        public static string NewCodeForDay(string billType, string prefix, string separatorString, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            //string billType = "DOC_CZTB";

            string newCode;

            if (Transaction.Current == null)
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    newCode = NewCodeForDay(decipher, billType, prefix, separatorString, seqLen);
                }
            }
            else
            {
                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                    {
                        newCode = NewCodeForDay(decipher, billType, prefix, separatorString, seqLen);
                    }
                }
            }

            return newCode;
        }

        /// <summary>
        /// 获取新订单编号(按日期)
        /// </summary>
        /// <returns></returns>
        public static string NewCodeForDay(DbDecipher decipher, string billType, string prefix, string separatorString, int seqLen = 4)
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册"); 
            //string billType = "DOC_CZTB";

            DateTime now = DateTime.Now;
            string nowId = "";


            try
            {
                if (!decipher.ExistsModels<C_BILL_IDENTITY>("BILL_TYPE_ID='{0}' AND BILL_YEAR={1} AND BILL_MONTH={2} AND BILL_DAY={3}", billType, now.Year, now.Month, now.Day))
                {
                    C_BILL_IDENTITY m = new C_BILL_IDENTITY();
                    m.BILL_TYPE_ID = billType;
                    m.BILL_YEAR = now.Year;
                    m.BILL_MONTH = now.Month;
                    m.BILL_DAY = now.Day;

                    decipher.InsertModel(m);
                }

                string sqlFormat = "Update C_BILL_IDENTITY Set BILL_IDENTITY = (BILL_IDENTITY + 1)  WHERE BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";
                string sqlFormat2 = "Select top 1 BILL_IDENTITY From C_BILL_IDENTITY Where BILL_TYPE_ID='{3}' AND BILL_YEAR={0} AND BILL_MONTH={1} AND BILL_DAY={2}";

                string sql = string.Format(sqlFormat, now.Year, now.Month, now.Day, billType);
                string sql2 = string.Format(sqlFormat2, now.Year, now.Month, now.Day, billType);


                int n = decipher.ExecuteNonQuery(sql);

                int id = decipher.ExecuteScalar<int>(sql2);
                
                string yearStr = now.Year.ToString();

                string idStr = id.ToString();

                string idStrX = idStr.PadLeft(seqLen, '0');


                nowId = string.Concat( prefix , string.Format("{0}{1:00}{2:00}", yearStr.Substring(2, 2), now.Month, now.Day) , separatorString,idStrX);

            }
            catch (Exception ex)
            {
                log.Error(ex);

                nowId = "错误单号";
            }
            finally
            {
                decipher.Dispose();
            }


            return nowId;
        }

        /// <summary>
        ///  获取新订单编号(按日期)
        /// </summary>
        /// <param name="billType"></param>
        /// <param name="prefix"></param>
        /// <param name="seqLen"></param>
        /// <returns></returns>
        public static string NewCodeForDay(string billType, string prefix , int seqLen = 4)
        {
            return NewCodeForDay(billType, prefix, null, seqLen);
        }
    }
}
