using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EC5.BizCoder
{
    public static class BizCodeMgr
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static object m_Locked = new object();

        static BizCodeDefineList m_CodeList = new BizCodeDefineList();

        /// <summary>
        /// 设置定义规则
        /// </summary>
        /// <param name="codeDefList"></param>
        public static void SetCdoeDefineList(BizCodeDefineList codeDefList)
        {
            lock (m_Locked)
            {
                m_CodeList = codeDefList;
            }
        }


        /// <summary>
        /// 根据代码获取编号
        /// </summary>
        /// <param name="tCode"></param>
        /// <returns></returns>
        public static string NewCode(string tCode)
        {
            string newCode = null;

            if (Transaction.Current == null)
            {
                newCode = baseNewCode(tCode);
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    newCode = baseNewCode(tCode);
                }
            }

            return newCode;
        }

        public static string baseNewCode(string tCode)
        {
            string newCode = null;

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                try
                {
                    newCode = NewCode(decipher, tCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return newCode;
        }


        /// <summary>
        /// 根据代码获取编号
        /// </summary>
        /// <param name="tCode"></param>
        /// <returns></returns>
        public static string NewCode(DbDecipher decipher, string tCode)
        {

            BizCodeDefine def = m_CodeList.GetItem(tCode);

            int newId ; 
            DateTime now = DateTime.Now;

            lock (def)
            {
                newId = GetCodeIdentity(decipher, tCode, def.CodeMode, 1, def.NumAdd, now);
            }

            StringBuilder sb = new StringBuilder(20);

            sb.Append(def.CodePrefix);

            sb.AppendFormat(def.TFormat, newId, now);

            sb.Append(def.CodeSuffix);

            return sb.ToString();
        }

        /// <summary>
        /// 获取递增值
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tCode"></param>
        /// <param name="codeMode"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numAdd"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        private static int GetCodeIdentity(DbDecipher decipher, string tCode, BizCodeMode codeMode,int defaultValue, int numAdd, DateTime curTime)
        {
            int codeModeId = (int)codeMode;

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CODE_IDENTITY));
            filter.And("T_CODE", tCode);
            filter.And("CODE_MODE_ID", (int)codeMode);
            filter.Locks.Add(LockType.RowLock);

            switch (codeMode)
            {
                case BizCodeMode.Year:
                    filter.And("C_YEAR", curTime.Year);
                    break;
                case BizCodeMode.Month:
                    filter.And("C_YEAR", curTime.Year);
                    filter.And("C_MONTH", curTime.Month);
                    break;
                case BizCodeMode.Day:
                    filter.And("C_YEAR", curTime.Year);
                    filter.And("C_MONTH", curTime.Month);
                    filter.And("C_DAY", curTime.Day);
                    break;
            }

            int newIdentity = -1;

            decipher.BeginTransaction();



            BIZ_CODE_IDENTITY cIdentity = decipher.SelectToOneModel<BIZ_CODE_IDENTITY>(filter);

            try
            {
                if (cIdentity == null)
                {
                    cIdentity = new BIZ_CODE_IDENTITY();
                    cIdentity.T_CODE = tCode;
                    cIdentity.CODE_MODE_ID = codeModeId;
                    cIdentity.NUM_CUR = defaultValue;

                    switch (codeMode)
                    {
                        case BizCodeMode.Year:
                            cIdentity.C_YEAR = curTime.Year;
                            break;
                        case BizCodeMode.Month:
                            cIdentity.C_YEAR = curTime.Year;
                            cIdentity.C_MONTH = curTime.Month;
                            break;
                        case BizCodeMode.Day:
                            cIdentity.C_YEAR = curTime.Year;
                            cIdentity.C_MONTH = curTime.Month;
                            cIdentity.C_DAY = curTime.Day;
                            break;
                    }

                    newIdentity = defaultValue;

                    decipher.InsertModel(cIdentity);
                }
                else
                {
                    cIdentity.NUM_CUR += numAdd;

                    newIdentity = cIdentity.NUM_CUR;

                    decipher.UpdateModelProps(cIdentity, "NUM_CUR");
                }


                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("实体操作错误", ex);
            }


            return newIdentity;
        
        }
        
        

    }
}
