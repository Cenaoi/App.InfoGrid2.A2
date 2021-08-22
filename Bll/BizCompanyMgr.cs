using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public static class BizCompanyMgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 公司名称
        /// </summary>
        static string m_ShortName;

        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <returns></returns>
        public static string GetName()
        {
            if (!string.IsNullOrEmpty(m_ShortName))
            {
                return m_ShortName;
            }


            if (Transaction.Current == null)
            {
                return baesGetName();
            }
            else
            {
                TransactionOptions tOpt = new TransactionOptions();
                tOpt.IsolationLevel = IsolationLevel.ReadCommitted;
                tOpt.Timeout = new TimeSpan(0, 2, 0);

                using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Suppress, tOpt))
                {
                    return baesGetName();
                }
            }
        }

        private static string baesGetName()
        {
            BIZ_C_COMPANY item = null;

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                try
                {
                    decipher.Locks.Add(LockType.NoLock);
                    item = decipher.SelectToOneModel<BIZ_C_COMPANY>("ROW_SID >= 0");
                }
                catch (Exception ex)
                {
                    log.Error("获取公司信息错误", ex);
                }
            }


            if (item != null)
            {
                m_ShortName = item.SHORT_NAME;

                return m_ShortName;
            }

            return "EasyClick 软件开发公司";
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static BIZ_C_COMPANY GetInfo()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_C_COMPANY item = decipher.SelectToOneModel<BIZ_C_COMPANY>("ROW_SID >= 0");

            return item;
        }
    }
}
