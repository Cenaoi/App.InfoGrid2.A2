using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.IG2.Core.TempDataManager
{
    /// <summary>
    /// IG2 临时数据管理
    /// </summary>
    public class IG2TempMgr
    {




        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 清理临时数据表
        /// </summary>
        public static void ClearTemp()
        {
            int count = 0;

            log.Info("准备删除 12 小时前的临时数据...");

            DateTime time = DateTime.Now.AddHours(-12);  //删除12小时以前的临时数据


            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                try
                {
                    count += DeleteTemp<IG2_TMP_TABLE>(decipher, time);
                    count += DeleteTemp<IG2_TMP_TABLECOL>(decipher, time);
                    count += DeleteTemp<IG2_TMP_VIEW>(decipher, time);
                    count += DeleteTemp<IG2_TMP_VIEW_FIELD>(decipher, time);
                    count += DeleteTemp<IG2_TMP_VIEW_TABLE>(decipher, time);
                }
                catch (Exception ex)
                {
                    log.Error("删除临时数据错误。", ex);
                }
            }


            log.InfoFormat("完成删除临时数据. {0} 条记录", count);
        }

        private static int DeleteTemp<ModelT>(DbDecipher decipher, DateTime lastTime)
        {
            Type modelT = typeof(ModelT);

            LightModelFilter filter = new LightModelFilter(modelT);
            filter.And("TMP_OP_TIME", lastTime, Logic.LessThanOrEqual);

            int count = decipher.DeleteModels(filter);

            if (count > 0)
            {
                log.InfoFormat("删除 {0} 条记录,表“{1}”。", count,modelT.Name);
            }

            return count;
        }
    }
}
