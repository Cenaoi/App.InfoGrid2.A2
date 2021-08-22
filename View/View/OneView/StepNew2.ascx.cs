using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EC5.Utility;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OneView
{
    public partial class StepNew2 : WidgetControl, IView
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
            int cataId = WebUtil.QueryInt("catalog_id", 102);

            int curId = StringUtil.ToInt32(this.store1.CurDataId, -1);

            string sessionId = this.Session.SessionID;
            Guid tmpId = Guid.NewGuid();


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, curId);

            
            

            TmpTableSet ttSet = tSet.ToTmpTableSet();

            IG2_TMP_TABLE tmpTab = ttSet.Table;
            tmpTab.TABLE_TYPE_ID = "VIEW";
            tmpTab.TABLE_SUB_TYPE_ID = "USER";
            tmpTab.IG2_CATALOG_ID = cataId;


            ttSet.Insert(decipher, tmpId, sessionId);

            string url = string.Format("StepNew3.aspx?tmp_id={0}", tmpId);

            MiniPager.Redirect(url);
        }

    }
}