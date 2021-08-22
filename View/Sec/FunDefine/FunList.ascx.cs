using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.BizWeb.UI;
using EasyClick.Web.Mini;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using HWQ.Entity;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.FunDefine
{
    public partial class FunList : System.Web.UI.UserControl
    {

        int m_ModuleId;

        DataGridViewAction<SEC_FUN_DEF> m_DataGridAct ;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_ModuleId = WebUtil.QueryInt("moduleId");

            m_DataGridAct = new DataGridViewAction<SEC_FUN_DEF>(this.DataGridView1);
            m_DataGridAct.AddFilterFixed("PARENT_ID", m_ModuleId);
            m_DataGridAct.AddFilterFixed("FUN_TYPE_ID", 4);

            m_DataGridAct.AddFilterSearch("ROW_STATUS_ID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            m_DataGridAct.AddFilterSearch("PARENT_ID", m_ModuleId);
            m_DataGridAct.AddFilterSearch("FUN_TYPE_ID", 4);

            m_DataGridAct.AddNewDefaultValues("SEQ", 999);

            if (!this.IsPostBack)
            {
                InitDropDownItems();

                m_DataGridAct.GoPage(0);
            }
        }


        /// <summary>
        /// 初始化下拉框列表
        /// </summary>
        private void InitDropDownItems()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<SEC_FUN_CODE> models = decipher.SelectModels<SEC_FUN_CODE>("ROW_STATUS_ID >= 0");

            EditorSelect2Cell col = this.DataGridView1.Columns.FindByDataField("CODE") as EditorSelect2Cell;

            col.Items.Clear();

            foreach (var m in models)
            {
                col.Items.Add(m.CODE);
            }


        }



        public void CreateFun()
        {
            m_DataGridAct.New();
        }

        public void CreateCommonFun()
        {
            InsertFun("NEW_ROW", "新建行");
            InsertFun("SAVE_ROWS", "保存");
            InsertFun("REFRESH", "刷新");
            InsertFun("DELETE_ROWS", "删除行");
            InsertFun("SEARCH", "查找");

            m_DataGridAct.Refresh();
        }

        private void InsertFun(string code, string text)
        {

            SEC_FUN_DEF fun = new SEC_FUN_DEF();
            fun.CODE = code;
            fun.TEXT = text;

            fun.FUN_TYPE_ID = 4;
            fun.PARENT_ID = m_ModuleId;
            fun.SEQ = 999;

            DbDecipher decipher = ModelAction.OpenDecipher();
            decipher.InsertModel(fun);
        }

        public void SaveFun()
        {
            m_DataGridAct.Save();
        }


        public void RefreshFun()
        {
            m_DataGridAct.Refresh();
        }

        public void DeleteFun()
        {
            m_DataGridAct.ChangeStatus("ROW_STATUS_ID", 0, -3);

        }

        /// <summary>
        /// 判断是否已经建立排序索引
        /// </summary>
        private bool IsCreateSortTag()
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("PARENT_ID", m_ModuleId);
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "SEQ" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelReader reader = decipher.GetModelReader(filter);

            decimal[] ids = ModelHelper.GetColumnData<decimal>(reader);

            if (ids.Length <= 1)
            {
                return true;
            }

            bool isSortTag = true;

            Array.Sort(ids);

            decimal id = ids[0];

            for (int i = 1; i < ids.Length; i++)
            {
                decimal id2 = ids[i];

                //只要有一个排序号是相同的,就证明不是系统产生的.
                if (id == id2)
                {
                    isSortTag = false;
                    break;
                }
            }

            return isSortTag;
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        public void MoveUp()
        {
            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                InitSeq(); 
            }


            LModelList<SEC_FUN_DEF> models = m_DataGridAct.GetModelsForCheckedInt();

            if (models.Count == 0)
            {
                return;
            }           


            SEC_FUN_DEF firstModel = models[0];

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("PARENT_ID", m_ModuleId);
            filter.And("SEQ", firstModel.SEQ, Logic.LessThan);
            filter.Top = 2;
            filter.TSqlOrderBy = "SEQ DESC";

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<SEC_FUN_DEF> targetModels = decipher.SelectModels<SEC_FUN_DEF>(filter);

            if (targetModels.Count == 0)
            {
                return;
            }

            decimal newSeq ;
            decimal dp = 1;

            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels[0].SEQ;

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                decimal tarSeq1 = targetModels[1].SEQ;
                decimal tarSeq2 = targetModels[0].SEQ;

                decimal ts = tarSeq2 - tarSeq1;

                decimal beiSu = 1;

                dp = 1;

                while (ts <= models.Count)
                {
                    beiSu *= 10;

                    ts = tarSeq2 * beiSu - tarSeq1 * beiSu;
                }

                dp = 1 / beiSu;

                newSeq = tarSeq2;
            }


            for (int i = models.Count - 1; i >= 0; i--)
            {
                newSeq -= dp;
                models[i].SEQ = newSeq;

                decipher.UpdateModelProps(models[i], "SEQ");
            }


            m_DataGridAct.Refresh();

            this.DataGridView1.SetItemFocus(this.DataGridView1.FocusedItem.Index-1);
        }



        /// <summary>
        /// 向下移动
        /// </summary>
        public void MoveDown()
        {
            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                InitSeq();
            }


            LModelList<SEC_FUN_DEF> models = m_DataGridAct.GetModelsForCheckedInt();

            if (models.Count == 0)
            {
                return;
            }


            SEC_FUN_DEF firstModel = models[models.Count - 1];

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("PARENT_ID", m_ModuleId);
            filter.And("SEQ", firstModel.SEQ, Logic.GreaterThan);
            filter.Top = 2;
            filter.TSqlOrderBy = "SEQ";

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<SEC_FUN_DEF> targetModels = decipher.SelectModels<SEC_FUN_DEF>(filter);

            if (targetModels.Count == 0)
            {
                return;
            }

            decimal newSeq;
            decimal dp = 1;

            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels[0].SEQ;

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                decimal tarSeq1 = targetModels[0].SEQ;
                decimal tarSeq2 = targetModels[1].SEQ;

                decimal ts = tarSeq2 - tarSeq1;

                decimal beiSu = 1;

                dp = 1;

                while (ts <= models.Count)
                {
                    beiSu *= 10;

                    ts = tarSeq2 * beiSu - tarSeq1 * beiSu;
                }

                dp = 1 / beiSu;

                newSeq = tarSeq1;
            }


            for (int i = 0; i < models.Count ; i++)
            {
                newSeq += dp;
                models[i].SEQ = newSeq;

                decipher.UpdateModelProps(models[i], "SEQ");
            }


            m_DataGridAct.Refresh();

            this.DataGridView1.SetItemFocus(this.DataGridView1.FocusedItem.Index + 1);
        }


        /// <summary>
        /// 初始化索引
        /// </summary>
        public void InitSeq()
        {
            //m_ModuleId = WebUtil.QueryInt("moduleId");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>(
                LOrder.By("SEQ,SEC_FUN_DEF_ID"), 
                "ROW_STATUS_ID >= 0 AND PARENT_ID={0}",m_ModuleId);

            for (int i = 0; i < models.Count; i++)
            {
                SEC_FUN_DEF m = models[i];

                m.SEQ = i;

                decipher.UpdateModelProps(m, "SEQ");
            }

            MiniHelper.Tooltip("初始化完成");

        }


    }
}