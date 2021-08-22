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
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.FlowModels;

namespace App.InfoGrid2.View.OneFlow
{
    public partial class InstCopyList : WidgetControl, IView
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


          
            if (!IsPostBack)
            {
                store1.DataBind();
            }
        }

        public void GoSearch()
        {

            store1.DataBind();
        }

        public void Reload()
        {
            this.store1.DataBind();
        }

        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;

            string state = WebUtil.Query("state");

            if (state == "check")
            {
                m_BizSid = 2;
                this.headText1.Text = "已审核";
            }
            else if (state == "no_check")
            {
                m_BizSid = 0;
                this.headText1.Text = "未审核";
            }


            string tSqlWhere = "";

            if (!StringUtil.IsBlank(textBox1.Value))
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }
                tSqlWhere += $" EXTEND_BILL_CODE like '%{textBox1.Value.Trim()}%' ";
            }

            DateTime? fromDate = this.dateRange1.StartDate;
            DateTime? toDate = this.dateRange1.EndDate;

            if (fromDate != null)
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }

                tSqlWhere += $" ROW_DATE_CREATE >= '{DateUtil.ToDateString(fromDate.Value)}' ";
            }


            if (toDate != null)
            {
                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }
                tSqlWhere += $" ROW_DATE_CREATE <= '{DateUtil.ToDateTimeString(toDate.Value)}' ";

            }

            string isOpenStr = WebUtil.Query("is_open");

            if (!StringUtil.IsBlank(isOpenStr))
            {
                bool isOpen = BoolUtil.ToBool(isOpenStr);

                if (!StringUtil.IsBlank(tSqlWhere)) { tSqlWhere += " AND "; }
                tSqlWhere += $" IS_OPEN = { (isOpen ? 1 : 0) } ";
            }

            m_TSqlWhere = tSqlWhere;


            EcUserState userState = EcContext.Current.User;

            string userCode = userState.ExpandPropertys["USER_CODE"];

            SModelList docs = FlowMgr.GetInstCopys(m_BizSid, userCode, tSqlWhere, e.TSqlSort);

            store1.RemoveAll();
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


                DbDecipher decipher = ModelAction.OpenDecipher();


                LightModelFilter filter = new LightModelFilter(typeof(FLOW_INST_COPY));
                filter.And("FLOW_INST_COPY_ID", record.Id);
                filter.And("IS_OPEN", 0);

                FLOW_INST_COPY fic = decipher.SelectToOneModel<FLOW_INST_COPY>(filter);
                

                if(fic != null)
                {
                    fic.IS_OPEN = true;
                    fic.OPEN_DATE = DateTime.Now;

                    decipher.UpdateModelProps(fic, "IS_OPEN", "OPEN_DATE");
                }
            }
        }

        public void GoViewDoc(string jsonStr)
        {
        }
    }
}