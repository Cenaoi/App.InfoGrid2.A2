using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    public partial class DwgList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.Inserting += Store1_Inserting;
            this.table1.Command += Table1_Command;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        private void Store1_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            string  newCode =BizCommon.BillIdentityMgr.NewCodeForDay("AC3_DWG_CODE", "DWG-", "-", 2);

            model["PK_DWG_CODE"] = newCode;
            model["DWG_TEXT"] = "联动图_" + newCode;
            model["V_VERSION"] = "3.0";
        }

        private void Table1_Command(object sender, EasyClick.Web.Mini2.TableCommandEventArgs e)
        {
            if (e.CommandName == "GoEditFlow")
            {
                GoEditFlow(e.Record);
            }
            else if (e.CommandName == "GoPreview")
            {
                //string ruiDir = "/App/InfoGrid2/View/OneFlowBuilder";

                //string url = ruiDir + $"/FlowInstPreview.aspx?flow_def_id={e.Record.Id}&flow_inst_code={}&_rum={Guid.NewGuid()}";

                //EcView.Show(url, "流程预览");
            }
        }

        private void GoEditFlow(DataRecord record)
        {
            string ruiDir = "/App/InfoGrid2/View/MoreActionBuilder";

            string url = ruiDir + $"/DwgBuilder.aspx?dwg_code={record["PK_DWG_CODE"]}&_rum={Guid.NewGuid()}";

            EcView.Show(url, "联动v3 - 编辑 ");
        }




    }
}