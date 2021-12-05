using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyClick.Web.Mini2;
using EC5.IG2.Plugin.PluginBll;
using HWQ.Entity.Decipher.LightDecipher;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 导入固铂数据插件
    /// </summary>
    public class ImportGbDataPlugin: PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 提交
        /// </summary>
        public void Submit()
        {
            try
            {
                BizHelper.SDbConn = DbDecipherManager.GetConnectionString("GUBO_2021");

                ImportHelper.HandleImport();

                MessageBox.Alert("处理导入文件完成");
            }
            catch (Exception ex)
            {
                log.Error($"导入固铂数据出错", ex);

                MessageBox.Alert("处理导入文件失败");
            }    
        }



    }
}
