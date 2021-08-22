using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.OneForm
{
    public partial class SecFlowFormSetup : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.CurrentChanged += Store1_CurrentChanged;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void Store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            this.store2.DataBind();
        }

        public void GoResetFlowList()
        {
            int pageId = WebUtil.QueryInt("page_id");
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE table = decipher.SelectModelByPk<IG2_TABLE>(id);

            SModelList smList = SModelList.ParseJson(table.FLOW_PARAMS);


            foreach (var item in smList)
            {
                string flowCode = (string)item["FLOW_CODE"];

                var filter = new LightModelFilter(typeof(SEC_TABLE_FLOW));
                filter.AddFilter("ROW_SID >= 0");
                filter.And("PAGE_ID", pageId);
                filter.And("FLOW_CODE", flowCode);
                filter.And("DB_TABLE", table.TABLE_NAME);

                SEC_TABLE_FLOW srcSTF = decipher.SelectToOneModel<SEC_TABLE_FLOW>(filter);

                if (srcSTF != null)
                {
                    

                    ResetItem(decipher, table, pageId, id, flowCode, srcSTF.PK_SEC_TF_CODE);

                    continue;
                }

                var flowDef = decipher.SelectToOneModel<FLOW_DEF>($"ROW_SID >=0 and DEF_CODE='{flowCode}'");
                var flowNodeDefs = decipher.SelectModels<FLOW_DEF_NODE>($"ROW_SID >=0 and DEF_CODE='{flowCode}' and NODE_TYPE='node'");

                SEC_TABLE_FLOW stf = new SEC_TABLE_FLOW();
                stf.PK_SEC_TF_CODE = BillIdentityMgr.NewCodeForMonth("SEC_TABLE_FLOW", "STF");
                stf.PAGE_ID = pageId;
                stf.FLOW_CODE = flowCode;
                stf.FLOW_TEXT = flowDef.DEF_TEXT;
                stf.DB_TABLE = table.TABLE_NAME;

                List<SEC_TABLE_FLOW_NODE> stfnList = new List<SEC_TABLE_FLOW_NODE>();

                foreach (var nodeDef in flowNodeDefs)
                {
                    SEC_TABLE_FLOW_NODE stfn = new SEC_TABLE_FLOW_NODE();
                    stfn.FK_SEC_TF_CODE = stf.PK_SEC_TF_CODE;
                    stfn.PAGE_ID = pageId;
                    stfn.FLOW_CODE = flowCode;
                    stfn.FLOW_NODE_CODE = nodeDef.NODE_CODE;
                    stfn.FLOW_NODE_TEXT = nodeDef.NODE_TEXT;
                    stfn.DB_TABLE = table.TABLE_NAME;

                    stfnList.Add(stfn);

                }

                decipher.BeginTransaction();

                try
                {
                    decipher.InsertModel(stf);
                    decipher.InsertModels(stfnList);

                    decipher.TransactionCommit();
                }
                catch(Exception ex)
                {
                    log.Error(ex);

                    decipher.TransactionRollback();

                    MessageBox.Alert("错误:" + ex.Message);

                    break;
                }
            }

        }


        private void ResetItem(DbDecipher decipher, IG2_TABLE table, int pageId, int id, string flowCode, string secTFCode)
        {


            var flowDef = decipher.SelectToOneModel<FLOW_DEF>($"ROW_SID >=0 and DEF_CODE='{flowCode}'");
            var flowNodeDefs = decipher.SelectModels<FLOW_DEF_NODE>($"ROW_SID >=0 and DEF_CODE='{flowCode}' and NODE_TYPE='node'");

            
            List<SEC_TABLE_FLOW_NODE> stfnList = new List<SEC_TABLE_FLOW_NODE>();

            foreach (var nodeDef in flowNodeDefs)
            {

                var flowNodeFilter = new LightModelFilter(typeof(SEC_TABLE_FLOW_NODE));
                flowNodeFilter.And("FK_SEC_TF_CODE", secTFCode);
                flowNodeFilter.And("PAGE_ID", pageId);
                flowNodeFilter.And("FLOW_CODE", flowCode);
                flowNodeFilter.And("FLOW_NODE_CODE", nodeDef.NODE_CODE);
                flowNodeFilter.And("DB_TABLE", table.TABLE_NAME);

                bool fnExist = decipher.ExistsModels(flowNodeFilter);

                if(fnExist)
                {
                   

                    continue;
                }

                SEC_TABLE_FLOW_NODE stfn = new SEC_TABLE_FLOW_NODE();
                stfn.FK_SEC_TF_CODE = secTFCode;
                stfn.PAGE_ID = pageId;
                stfn.FLOW_CODE = flowCode;
                stfn.FLOW_NODE_CODE = nodeDef.NODE_CODE;
                stfn.FLOW_NODE_TEXT = nodeDef.NODE_TEXT;
                stfn.DB_TABLE = table.TABLE_NAME;

                stfnList.Add(stfn);

            }


            if(stfnList.Count > 0)
            {
                decipher.InsertModels(stfnList);
            }

        }



    }
}