using HWQ.Entity.Decipher.LightDecipher;
using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.GBZZZD.Task
{
    public class ImportOrderDataTask : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ImportOrderDataTask()
        {

            this.TaskSpan = new TimeSpan(0, 0, 15);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Now;

        }

        public override void Exec()
        {
            //log.Debug("[ImportOrderDataTask]任务，准备处理导入数据文件...");

            try
            {
                EC5.IG2.Plugin.PluginBll.BizHelper.SDbConn = DbDecipherManager.GetConnectionString("GUBO_2021");
                
                EC5.IG2.Plugin.PluginBll.ImportHelper.HandleImport();
            }
            catch (Exception ex)
            {
                log.Error($"导入固铂数据出错", ex);
            }
        }


    }
}