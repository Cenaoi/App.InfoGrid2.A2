using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility.Web;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaStepEdit3 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";



            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            if (!IsPostBack)
            {
                InitUI();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.DataBind();
            }
        }

        private void InitUI()
        {
            int id = WebUtil.QueryInt("view_id");


            SelectColumn selectCol = table2.Columns.FindByDataField("LOCKED_FIELD") as SelectColumn;


            DbDecipher decipher = ModelAction.OpenDecipher();

            if (selectCol != null)
            {
                LightModelFilter colsFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                colsFilter.And("IG2_TABLE_ID", id);
                colsFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                colsFilter.And("SEC_LEVEL", 6, Logic.LessThanOrEqual);

                colsFilter.TSqlOrderBy = "FIELD_SEQ ASC,IG2_TABLE_COL_ID ASC";

                List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>(colsFilter);


                foreach (IG2_TABLE_COL col in cols)
                {
                    selectCol.Items.Add(col.DB_FIELD, col.F_NAME);
                }
            }
        }


        /// <summary>
        /// 上一步
        /// </summary>
        public void GoPre()
        {

            int view_id = WebUtil.QueryInt("view_id");
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            string tableNames = WebUtil.Query("table_names");

            string urlStr = string.Format("AreaStepEdit2.aspx?page_Id={0}&Area_Id={1}&Area_Type_Id={2}&view_Id={3}&table_names={4}",
                pageId, areaId, areaTypeId, view_id,tableNames);

            MiniPager.Redirect(urlStr);
        }


        /// <summary>
        /// 完成
        /// </summary>
        public void GoLast()
        {
            int view_id = WebUtil.QueryInt("view_id");

            string tableNames = WebUtil.Query("table_names");

            Guid opGuid = WebUtil.QueryGuid("TMP_GUID", Guid.NewGuid());
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            DbDecipher decipher = ModelAction.OpenDecipher();



            TableSet tSet = TableSet.Select(decipher, view_id);

            IG2_TABLE tab = tSet.Table;


            string ps = string.Format("viewId:{0}, viewType:'{1}', viewName:'{2}'",
                view_id, "VIEW", tab.TABLE_NAME);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', " + ps + "})");

            //EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', table_id:" + tSet.Table.IG2_TABLE_ID + ",table_name: '" + tSet.Table.TABLE_NAME + "'});");

        }
    }
}