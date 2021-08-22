using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Model.XmlModel;
using EasyClick.Web.Mini2;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;

namespace App.InfoGrid2.View.SysBugRepair
{
    public partial class SecTableRepart : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 修复权限 bug
        /// </summary>
        public void GoSecTableRepart()
        {
            WebFileInfo wFile = new WebFileInfo("/_Temporary/log_Repart", "SEC_UI_TABLE数据表修复日志_" + FileUtil.NewFielname(".txt"));
            StringBuilder rLog = new StringBuilder();   //修复日志.
            rLog.AppendLine($"[开始]  {DateTime.Now}");

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<SEC_UI> uiList = decipher.SelectModels<SEC_UI>("ROW_SID >= 0");

            int count = 0;

            try
            {
                foreach (var ui in uiList)
                {
                    List<SEC_UI_TABLE> uiTables = decipher.SelectModels<SEC_UI_TABLE>($"ROW_SID >= 0 and SEC_UI_ID = {ui.SEC_UI_ID} and PAGE_ID <> {ui.UI_PAGE_ID}");

                    if (uiTables.Count == 0)
                    {
                        continue;
                    }

                    rLog.AppendLine();
                    rLog.AppendLine();
                    rLog.AppendLine($"UI_SEC_ID={ui.SEC_UI_ID}");
                    rLog.AppendLine("--------------------------------------------------------");

                    foreach (var uiTable in uiTables)
                    {
                        rLog.AppendLine($"ROW_ID={uiTable.SEC_UI_TABLE_ID}, PAGE_ID: {uiTable.PAGE_ID} -> {ui.UI_PAGE_ID}");

                        uiTable.PAGE_ID = ui.UI_PAGE_ID;

                        decipher.UpdateModelProps(uiTable, "PAGE_ID");

                        count++;
                    }

                }

                MessageBox.Alert($"修复完成...共{count}条记录");
            }
            catch(Exception ex)
            {
                log.Error("修复失败");

                MessageBox.Alert("修复失败.");
            }


            rLog.AppendLine($"[结束]  {DateTime.Now}");

            FileUtil.WriteAllText(wFile.PhysicalPath, rLog.ToString());
        }
    }
}