using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.GBZZZD.Task
{
    public class SyncSaleOrderItemsTask : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SyncSaleOrderItemsTask()
        {

            this.TaskSpan = new TimeSpan(0, 0, 60);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Now;

        }

        public bool IsExec { get; set; } = false;

        public override void Exec()
        {
            if (IsExec)
            {
                return;
            }

            IsExec = true;

            try
            {
                SyncOrderHelper.SyncSaleOrderItem();
            }
            catch (Exception ex)
            {
                log.Error($"SyncSaleOrderItemsTask出错", ex);
            }

            IsExec = false;
        }


    }
}