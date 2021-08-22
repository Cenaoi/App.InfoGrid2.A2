using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace App.InfoGrid2.View.OneFlowBuilder
{
    public partial class FlowDocList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.table1.Command += Table1_Command;
            if (!IsPostBack)
            {
                EcUserState userState = EcContext.Current.User;

                SModelList docs;
                

                docs = FlowMgr.GetUserDocsAll(0,string.Empty);

                this.store1.AddRange(docs);
            }
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