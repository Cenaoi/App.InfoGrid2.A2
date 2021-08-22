using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.OneFlowBuilder
{
    /// <summary>
    /// 流程修复界面.
    /// </summary>
    public partial class FlowHotfix :WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 修补漏网的空白单号
        /// </summary>
        public void GoFullBlankCode()
        {
            LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("EXTEND_BILL_CODE", "");


            DbDecipher decipher = ModelAction.OpenDecipher();

            List<FLOW_INST> models = decipher.SelectModels<FLOW_INST>(filter) ;

            int n = 0;

            LModelElement modelElem;

            foreach (var item in models)
            {
                try
                {
                    if(!LModelDna.TryGetElementByName(item.EXTEND_TABLE, out modelElem))
                    {
                        log.Info($"补空白单号失败,表名不存在: FLOW_INST_ID={item.FLOW_INST_ID}, 表名={item.EXTEND_TABLE}");
                        continue;
                    }


                    string tSql = $"select {item.EXTEND_BILL_CODE_FIELD} from {item.EXTEND_TABLE} where {modelElem.PrimaryKey}={item.EXTEND_ROW_ID}";

                    string billCode = decipher.ExecuteScalar<string>(tSql);

                    if (!string.IsNullOrEmpty(billCode))
                    {
                        item.EXTEND_BILL_CODE = billCode;

                        decipher.UpdateModelProps(item, "EXTEND_BILL_CODE");

                        n++;
                    }
                    else
                    {
                        log.Info($"补空白单号失败,单号本身记录不存在. FLOW_INST_ID={item.FLOW_INST_ID}, 表名={item.EXTEND_TABLE}");
                    }
                }
                catch(Exception ex)
                {
                    log.Error($"补空白单号错误: FLOW_INST_ID={item.FLOW_INST_ID}, 表名={item.EXTEND_TABLE}", ex);
                }
            }

            log.Info($"流程单号修复: 修补了 {n} 条记录.");

            EasyClick.Web.Mini2.Toast.Show($"修补了 {n} 条记录.");
        }
    }
}