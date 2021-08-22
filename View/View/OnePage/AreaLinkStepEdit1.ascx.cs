using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using App.InfoGrid2.Model.DataSet;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaLinkStepEdit1 : WidgetControl, IView
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
                InitSelfCols();
                InitData();
            }
        }

        /// <summary>
        /// 初始化下拉表格的字段
        /// </summary>
        private void InitSelfCols()
        {
            string self_table = WebUtil.Query("self_table");

            if (StringUtil.IsBlank(self_table))
            {
                return;
            }
            

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = new TableSet();
            

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("TABLE_NAME", self_table);
            
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(filter);

            List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>("SEC_LEVEL < 6 AND IG2_TABLE_ID={0}", table.IG2_TABLE_ID);


            foreach (IG2_TABLE_COL col in cols)
            {
                comboBox1.Items.Add(col.DB_FIELD, col.F_NAME);
            }

        }

        private void InitData()
        {
            int viewId = WebUtil.QueryInt("view_Id");
            int pageId = WebUtil.QueryInt("page_id");

            if (viewId <= 0)
            {
                return;
            }

            string self_table = WebUtil.Query("self_table");

            string tables = WebUtil.Query("tables");

            string[] tableList = StringUtil.Split(tables, ",");

            List<string> tt = new List<string>(tableList);
            tt.Remove(self_table);

            tableList = tt.ToArray();


            DbDecipher decipher = ModelAction.OpenDecipher();


            IG2_TABLE pView = decipher.SelectModelByPk<IG2_TABLE>(viewId);

            if (pView == null)
            {
                return;
            }

            this.text1.Value = pView.TAB_TEXT;
            this.comboBox1.Value = pView.ME_COL_NAME;
            this.linkEnabledCB.Checked = pView.JOIN_ENABLED;
            this.tabShareDataCB.Checked = pView.IS_SHARE_DATA;


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("SEC_LEVEL", 6, Logic.LessThan);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("TABLE_NAME", tableList, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            var models = decipher.SelectModels<IG2_TABLE>(filter);

            this.store1.AddRange(models);



            //默认选中

            if (!StringUtil.IsBlank(pView.JOIN_TAB_NAME))
            {

                IG2_TABLE t1 = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' AND TABLE_NAME ='{0}'", pView.JOIN_TAB_NAME);

                if (t1 != null)
                {
                    this.table1.SetRecordCheck(t1.IG2_TABLE_ID.ToString(), true);
                    this.store1.SetCurrntForId(t1.IG2_TABLE_ID.ToString());
                }
            }



        }


        public void OnStore1_Refresh()
        {
            int viewId = WebUtil.QueryInt("view_Id");
            int pageId = WebUtil.QueryInt("page_id");

            if (viewId <= 0)
            {
                return;
            }

            string tables = WebUtil.Query("tables");

            string[] tableList = StringUtil.Split(tables, ",");

            DbDecipher decipher = ModelAction.OpenDecipher();


            IG2_TABLE pView = decipher.SelectModelByPk<IG2_TABLE>(viewId);

            if (pView == null)
            {
                return;
            }


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("SEC_LEVEL", 6, Logic.LessThan);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("TABLE_NAME", tableList, Logic.In);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            var models = decipher.SelectModels<IG2_TABLE>(filter);

            this.store1.RemoveAll();
            this.store1.AddRange(models);



            //默认选中

            if (!StringUtil.IsBlank(pView.JOIN_TAB_NAME))
            {

                IG2_TABLE t1 = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' AND TABLE_NAME ='{0}'", pView.JOIN_TAB_NAME);

                if (t1 != null)
                {
                    this.table1.SetRecordCheck(t1.IG2_TABLE_ID.ToString(), true);
                    this.store1.SetCurrntForId(t1.IG2_TABLE_ID.ToString());
                }
            }


        }


        protected void store1_CurrentChanged(object sender, ObjectEventArgs e)
        {
            DataRecord record = e.SrcRecord;

            if (record == null) { return; }

            int pk = StringUtil.ToInt(record.Id);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE_COL));
            filter.And("IG2_TABLE_ID", pk);
            filter.And("SEC_LEVEL", 6, Logic.LessThan);

            List<IG2_TABLE_COL> models = decipher.SelectModels<IG2_TABLE_COL>(filter);

            this.store2.RemoveAll();
            this.store2.AddRange(models);


            //默认选中
            int viewId = WebUtil.QueryInt("view_Id");

            IG2_TABLE pView = decipher.SelectModelByPk<IG2_TABLE>(viewId);

            if (!StringUtil.IsBlank(pView.JOIN_COL_NAME))
            {
                IG2_TABLE t1 = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' AND TABLE_NAME ='{0}'", pView.JOIN_TAB_NAME);

                if (t1 != null)
                {
                    IG2_TABLE_COL col1 = decipher.SelectToOneModel<IG2_TABLE_COL>("SEC_LEVEL < 6 AND IG2_TABLE_ID = {0} AND DB_FIELD ='{1}'",
                        t1.IG2_TABLE_ID, pView.JOIN_COL_NAME);

                    if (col1 != null)
                    {
                        string rectId =col1.IG2_TABLE_COL_ID.ToString();

                        this.table2.SetRecordCheck(rectId, true);
                        this.store2.SetCurrntForId(rectId);

                    }
                }
            }

        }


        public void GoLast()
        {

            int pageId = WebUtil.QueryInt("page_Id");
            int viewId = WebUtil.QueryInt("view_Id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE pageView = decipher.SelectModelByPk<IG2_TABLE>(viewId);


            string selfColName = this.comboBox1.Value;

            pageView.ME_COL_NAME = selfColName;
            pageView.TAB_TEXT = text1.Value;

            pageView.JOIN_ENABLED = this.linkEnabledCB.Checked;
            pageView.IS_SHARE_DATA = this.tabShareDataCB.Checked;


            if (!this.linkEnabledCB.Checked)
            {
                decipher.UpdateModelProps(pageView, "TAB_TEXT","ME_COL_NAME","JOIN_ENABLED", "IS_SHARE_DATA");

                var result2 = new
                {
                    result = "ok",
                    jon_enabled = false,
                    tab_text = text1.Value
                };

                this.Close(result2);

                return;
            }


            DataRecord tabRecord = this.store1.GetDataCurrent();

            DataRecord colRecord = this.store2.GetDataCurrent();

            if (tabRecord == null || colRecord == null)
            {
                EasyClick.Web.Mini.MiniHelper.Alert("必须选择关联，才能选择。");
                return;
            }

            int tabId = StringUtil.ToInt(tabRecord.Id);
            int colId = StringUtil.ToInt(colRecord.Id);

            string tabName = (string)tabRecord["TABLE_NAME"];

            string colName = (string)colRecord["DB_FIELD"];

            pageView.JOIN_ENABLED = true;
            pageView.JOIN_TAB_NAME = tabName;
            pageView.JOIN_COL_NAME = colName;

            decipher.UpdateModelProps(pageView, "JOIN_ENABLED","JOIN_TAB_NAME", "JOIN_COL_NAME", "ME_COL_NAME", "TAB_TEXT", "IS_SHARE_DATA");

            var result = new
            {
                result = "ok",
                tab = new
                {
                    id = tabId,
                    name = tabName
                },
                col = new
                {
                    id = colId,
                    name = colName
                },
                tab_text = text1.Value
            };

            this.Close(result);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="result"></param>
        private void Close(object result)
        {
            string resultStr = JsonConvert.SerializeObject(result);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close(" + resultStr + ")");
        }

    }
}