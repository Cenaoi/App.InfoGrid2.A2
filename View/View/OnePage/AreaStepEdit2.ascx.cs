using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using App.InfoGrid2.View.OneTable;
using EasyClick.BizWeb2;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaStepEdit2 : WidgetControl, IView
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


            SelectColumn selectCol = table1.Columns.FindByDataField("LOCKED_FIELD") as SelectColumn;


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

            Guid opGuid = WebUtil.QueryGuid("TMP_GUID", Guid.NewGuid());
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");


            string urlStr = string.Format("AreaStepNew1.aspx?page_Id={0}&Area_Id={1}&Area_Type_Id={2}&TMP_GUID={3}",
                pageId, areaId, areaTypeId, opGuid);

            MiniPager.Redirect(urlStr);
        }


        public void GoNext()
        {

            int viewId = WebUtil.QueryInt("view_id");
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            string tableNames = WebUtil.Query("table_names");


            string urlStr = string.Format("AreaStepEdit3.aspx?page_Id={0}&Area_Id={1}&Area_Type_Id={2}&view_id={3}&table_names={4}",
                pageId, areaId, areaTypeId, viewId,tableNames);

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


        /// <summary>
        /// 工具栏设置
        /// </summary>
        public void ToolbarSetup()
        {
            #region 原地址

            int viewId = WebUtil.QueryInt("view_id");
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            string tableNames = WebUtil.Query("table_names");


            string urlStr = string.Format("/App/InfoGrid2/View/OnePage/AreaStepEdit2.aspx?page_Id={0}&Area_Id={1}&Area_Type_Id={2}&view_id={3}&table_names={4}",
                pageId, areaId, areaTypeId, viewId, tableNames);


            string urlEncode = Base64Util.ToString(urlStr, Base64Mode.Http);

            #endregion



            int view_id = WebUtil.QueryInt("view_id");

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TOOLBAR));
            filter.And("TABLE_ID", view_id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "IG2_TOOLBAR_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            int toolbarId = decipher.ExecuteScalar<int>(filter);


            string targetUrl ;

            if (toolbarId > 0)
            {
                targetUrl = string.Format("/app/infogrid2/view/OneToolbar/SetToolbar.aspx?id={0}&table_id={1}&src_url={2}", toolbarId, view_id, urlEncode);
            }
            else
            {
                targetUrl = string.Format("/app/infogrid2/view/OneToolbar/ToolbarStepNew1.aspx?table_id={0}&src_url={1}", view_id, urlEncode);
            }

            MiniPager.Redirect(targetUrl);

        }

        /// <summary>
        /// 流程定义
        /// </summary>
        public  void GoFlowSetup()
        {
            Console.WriteLine(this.Request.QueryString);

            int id = WebUtil.QueryInt("view_id");


            Window win = new Window("流程设置",600,400);

            win.ContentPath = $"/App/InfoGrid2/View/OneForm/FlowSetup.aspx?id={id}";
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.ShowDialog();

        }

    }
}