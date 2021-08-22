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
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity;
using EC5.LCodeEngine;


namespace App.InfoGrid2.View.OneTable
{
    public partial class StepEdit4 : WidgetControl, IView
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

        private void InitUI()
        {
            int id = WebUtil.QueryInt("id");


            SelectColumn selectCol = table2.Columns.FindByDataField("LOCKED_FIELD") as SelectColumn;


            DbDecipher decipher = ModelAction.OpenDecipher();

            if (selectCol != null)
            {
                TableSet tSet = TableSet.Select(decipher, id);

                foreach (IG2_TABLE_COL col in tSet.Cols)
                {
                    selectCol.Items.Add(col.DB_FIELD, col.F_NAME);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.storeTab.DataBind();
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

            DbDecipher decipher = ModelAction.OpenDecipher();
            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);

            Reset_LModelElem(tSet);
            Reset_LcModel(tSet);

            TableBufferMgr.Remove(id);
            EC5.IG2.Core.UI.M2VFieldHelper.VModels.Remove(id);

            MiniPager.Redirect("TablePreview.aspx?id=" + id);
        }


        /// <summary>
        /// 重新设置实体元素
        /// </summary>
        private void Reset_LModelElem(TableSet tSet)
        {
            IG2_TABLE tab = tSet.Table;

            LModelElement modelElem = LightModel.GetLModelElement(tab.TABLE_NAME);

            LModelFieldElement fieldElem;

            LModelDna.BeginEdit();

            try
            {
                foreach (IG2_TABLE_COL tCol in tSet.Cols)
                {
                    if (!modelElem.TryGetField(tCol.DB_FIELD, out fieldElem))
                    {
                        continue;
                    }
                    
                    if (!fieldElem.IsNumber)
                    {
                        continue;
                    }

                    fieldElem.DecimalDigits = tCol.DB_DOT;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            LModelDna.EndEdit();
        }


        /// <summary>
        /// 重新设置字段监控
        /// </summary>
        /// <param name="tableId"></param>
        private void Reset_LcModel(TableSet tSet)
        {


            string tableName = tSet.Table.TABLE_NAME;

            LcModel cModel = null;

            foreach (IG2_TABLE_COL col in tSet.Cols)
            {
                if (StringUtil.IsBlank(col.L_CODE))
                {
                    continue;
                }

                if (cModel == null)
                {
                    cModel = new LcModel();
                    cModel.TableName = tableName;
                }

                cModel.Add(col.DB_FIELD, col.L_CODE);

            }

            if (cModel == null)
            {
                return;
            }

            cModel.Reset();

            if (LcModelManager.Models.ContainsKey(tableName))
            {
                LcModelManager.Models.Remove(tableName);
            }

            LcModelManager.Models.Add(tableName, cModel);

        }


        /// <summary>
        /// 应用到全部视图
        /// </summary>
        public void GoApplyToViewAll()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectSID_0_5(decipher, id);


            //创建字段索引
            Dictionary<string,IG2_TABLE_COL> colDict = ModelHelper.ToDictionary<string, IG2_TABLE_COL>(tSet.Cols, "DB_FIELD");



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


        static string[] m_ApplyFields = new string[]{
            "DISPLAY",
            "DISPLAY_TYPE",
            "DISPLAY_LEN",
            "FORMAT",
            "GROUP_ID",
            "V_LIST_MODE_ID",
            "V_TRIGGER_MODE",
            "TOOLTIP",
            "ANGLE",
            "DB_LEN",
            "DB_DOT",
            "ACT_MODE",
            "ACT_FIXED_ITEMS",
            "ACT_TABLE_ITEMS"
        };

        /// <summary>
        /// 应用到视图表
        /// </summary>
        /// <param name="tableId">视图表ID</param>
        /// <param name="tableUid">视图表UID</param>
        /// <param name="srcTSet"></param>
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