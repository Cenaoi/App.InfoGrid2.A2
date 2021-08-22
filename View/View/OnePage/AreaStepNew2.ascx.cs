using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.View.OneTable;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaStepNew2 : WidgetControl, IView
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
                this.DataBind();
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

            Guid opGuid = WebUtil.QueryGuid("TMP_GUID", Guid.NewGuid());
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            string urlStr = string.Format("AreaStepNew3.aspx?page_Id={0}&Area_Id={1}&Area_Type_Id={2}&TMP_GUID={3}",
                pageId, areaId, areaTypeId, opGuid);

            MiniPager.Redirect(urlStr);
        }


        /// <summary>
        /// 完成
        /// </summary>
        public void GoLast()
        {
            //int tableId = StringUtil.ToInt(tableIdStr);

            Guid opGuid = WebUtil.QueryGuid("TMP_GUID", Guid.NewGuid());
            string session = this.Session.SessionID;

            int pageId = WebUtil.QueryInt("page_id");
            string areaId = WebUtil.Query("area_id");
            string areaTypeId = WebUtil.Query("area_type_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet ttSet = TmpTableSet.Select(decipher, opGuid);

            TableSet tSet = ttSet.ToTableSet();

            IG2_TABLE tab = tSet.Table;

            tab.PAGE_ID = pageId;
            tab.PAGE_AREA_ID = areaId;
            tab.TABLE_TYPE_ID = "VIEW";
            tab.TABLE_SUB_TYPE_ID = "PAGE_AREA";
            tab.DISPLAY_MODE = areaTypeId.ToUpper();

            tSet.Insert(decipher);

            TmpTableSet.Delete(decipher, opGuid);

            string ps = string.Format("viewId:{0}, viewType:'{1}', viewName:'{2}'",
                tab.IG2_TABLE_ID , "VIEW", tab.TABLE_NAME);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', " + ps + "})");

            //EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok', table_id:" + tSet.Table.IG2_TABLE_ID + ",table_name: '" + tSet.Table.TABLE_NAME + "'});");

            

        }


    }
}