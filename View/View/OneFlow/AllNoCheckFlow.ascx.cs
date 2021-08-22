using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.OneFlow
{
    public partial class AllNoCheckFlow : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);

        }

        string m_TSqlWhere;

        int m_BizSid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            this.store1.PageLoading += Store1_PageLoading;
            this.table1.Command += Table1_Command;


        }

        public void Reload()
        {


            store1.DataBind();



        }


        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {

            e.Cancel = true;

            string tSqlWhere = "";

            if (!StringUtil.IsBlank(textBox1.Value))
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }
                tSqlWhere += $" EXTEND_BILL_CODE like '%{textBox1.Value.Trim()}%' ";
            }


            DateTime? startDate = this.dateRange1.StartDate;
            DateTime? endDate = this.dateRange1.EndDate;

            if (startDate != null)
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }

                tSqlWhere += $" FLOW_INST_PARTY.ROW_DATE_CREATE >= '{DateUtil.ToDateString(startDate.Value)}' ";
            }


            if (endDate != null)
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }
                tSqlWhere += $" FLOW_INST_PARTY.ROW_DATE_CREATE <= '{DateUtil.ToDateTimeString(endDate.Value)}' ";
            }

            
            

            m_TSqlWhere = tSqlWhere;



            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetAllUserDocs(0, tSqlWhere,e.TSqlSort);

            this.store1.RemoveAll();
            this.store1.AddRange(docs);
        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {

            if (e.CommandName == "GoViewDoc")
            {
                DataRecord record = e.Record;

                string text = (string)record["EXTEND_DOC_TEXT"];
                string url = (string)record["EXTEND_DOC_URL"];

                EcView.Show(url, text + " - 审核");
            }
        }

        public void GoViewDoc(string jsonStr)
        {
        }
    }
}