﻿using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.GBZZZD.Task
{
    public class SyncSaleOrderTask : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SyncSaleOrderTask()
        {

            this.TaskSpan = new TimeSpan(0, 0, 60);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Now;

        }


        public override void Exec()
        {
            try
            {
                SyncOrderHelper.SyncSaleOrder();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}