using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility.Web;
using EC5.Utility;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneSearch
{
    public partial class StepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        public void GoNext()
        {
            string owner_type_Id = WebUtil.Query("owner_type_id");
            int owner_Table_Id = WebUtil.QueryInt("owner_table_id");
            string owner_Col_Id = WebUtil.Query("owner_col_id");


            int curId = StringUtil.ToInt32(this.store1.CurDataId, -1);

            string sessionId = this.Session.SessionID;
            Guid tmpId = Guid.NewGuid();


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, curId);

            TmpTableSet ttSet = tSet.ToTmpTableSet();

            ttSet.Insert(decipher, tmpId, sessionId);

            string url = string.Format("StepNew2.aspx?owner_type_id={0}&owner_table_id={1}&owner_col_id={2}&TMP_GUID={3}",
                owner_type_Id, owner_Table_Id, owner_Col_Id, tmpId);

            MiniPager.Redirect(url);
        }

    }
}