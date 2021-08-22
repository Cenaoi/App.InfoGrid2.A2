using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity;
using EasyClick.BizWeb2;


namespace App.InfoGrid2.View.OneTable
{
    public partial class StepEdit3 : WidgetControl, IView
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
                this.storeTable.DataBind();
                this.store1.DataBind();
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void GoCancel()
        {

            int id = WebUtil.QueryInt("id");

            MiniPager.Redirect("TablePreview.aspx?id=" + id);
        }

        /// <summary>
        /// 跳转到最后
        /// </summary>
        public void GoLast()
        {
            int id = WebUtil.QueryInt("id");


            MiniPager.Redirect("TablePreview.aspx?id=" + id);
        }





        static string[] m_ApplyFields = new string[]{
            "IS_MANDATORY",
            "IS_READONLY",
            "IS_VISIBLE",
            "IS_VIEW_FIELD",
            "IS_BIZ_MANDATORY",
            "IS_LIST_VISIBLE",
            "IS_SEARCH_VISIBLE",
            "DEFAULT_VALUE",
            "FIELD_SEQ"
        };


        /// <summary>
        /// 应用到全部视图
        /// </summary>
        public void GoApplyToViewAll()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);


            //创建字段索引
            Dictionary<string, IG2_TABLE_COL> colDict = ModelHelper.ToDictionary<string, IG2_TABLE_COL>(tSet.Cols, "DB_FIELD");



            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("IG2_TABLE_ID", id, Logic.Inequality);
            filter.And("TABLE_NAME", tSet.Table.TABLE_NAME);
            filter.And("TABLE_UID", tSet.Table.TABLE_UID);

            filter.Fields = new string[] { "IG2_TABLE_ID", "TABLE_UID" };

            List<LModel> models = decipher.GetModelList(filter);

            decipher.BeginTransaction();

            try
            {
                foreach (LModel model in models)
                {
                    int tabId = model.Get<int>("IG2_TABLE_ID");
                    Guid tabUid = model.Get<Guid>("TABLE_UID");

                    ApplyToView(tabId, tabUid, colDict);
                }

                decipher.TransactionCommit();

                Toast.Show("成功应用到其它视图.");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("应用到其它视图失败.", ex);

                MessageBox.Alert("应用到其它视图失败!");
            }

        }



        /// <summary>
        /// 应用到视图表
        /// </summary>
        /// <param name="tableId">视图表ID</param>
        /// <param name="tableUid">视图表UID</param>
        /// <param name="colDict"></param>
        private void ApplyToView(int tableId, Guid tableUid, Dictionary<string, IG2_TABLE_COL> colDict)
        {

            IG2_TABLE_COL srcCol;

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectSID_0_5(decipher, tableId);

            foreach (IG2_TABLE_COL tarCol in tSet.Cols)
            {
                if (!colDict.TryGetValue(tarCol.DB_FIELD, out srcCol))
                {
                    continue;
                }

                if (!srcCol.CAN_APPLY_VIEW)
                {
                    continue;
                }

                CopyField(srcCol, tarCol, m_ApplyFields);

                decipher.UpdateModelProps(tarCol, m_ApplyFields);

            }
        }

        private void CopyField(IG2_TABLE_COL srcCol, IG2_TABLE_COL tarCol, string[] fields)
        {
            foreach (string field in fields)
            {
                tarCol[field] = srcCol[field];
            }
        }

    }
}