using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneTable
{
    public partial class StepCopy1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {
            int srcTableId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, srcTableId);

            TB1.Value = tSet.Table.DISPLAY;
            

        }

        public void GoNext()
        {
            string display = this.TB1.Value;

            int srcTableId = WebUtil.QueryInt("id");



            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, srcTableId);
                        

            TmpTableSet tmpTSet = tSet.ToTmpTableSet();

            string sessionId = this.Session.SessionID;
            Guid opGuid = Guid.NewGuid();

            IG2_TMP_TABLE tmpTable = tmpTSet.Table;

            tmpTable.TABLE_NAME = string.Empty;
            tmpTable.DISPLAY = display;
            tmpTable.TABLE_UID = Guid.NewGuid();

            tmpTable.TMP_GUID = opGuid;
            tmpTable.TMP_SESSION_ID = sessionId;
            tmpTable.TMP_OP_TIME = DateTime.Now;

            decipher.InsertModel(tmpTable);

            foreach (var item in tmpTSet.Cols)
            {
                item.TMP_GUID = opGuid;
                item.TMP_SESSION_ID = sessionId;
                item.TMP_OP_TIME = DateTime.Now;

                decipher.InsertModel(item);
            }

            MiniPager.Redirect("StepNew2.aspx?tmp_id=" + opGuid);

        }


    }
}