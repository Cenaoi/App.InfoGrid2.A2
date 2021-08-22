using System;
using System.Collections.Generic;
using System.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.LightModels;
using HWQ.Entity;
using HWQ.Entity.Filter;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 行排序，工厂
    /// </summary>
    public class RowSortFactory
    {
        string m_TableName;
        string m_SeqField;
        string m_IdField;

        string m_TSqlWhere;

        /// <summary>
        /// T-SQL 的 Where 子语句
        /// </summary>
        public string TSqlWhere
        {
            get { return m_TSqlWhere; }
            set { m_TSqlWhere = value; }
        }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName
        {
            get { return m_TableName; }
            set { m_TableName = value; }
        }

        /// <summary>
        /// 数据表主键名
        /// </summary>
        public string IdField
        {
            get { return m_IdField; }
            set { m_IdField = value; }
        }

        /// <summary>
        /// 排序的字段名
        /// </summary>
        public string SeqField
        {
            get { return m_SeqField; }
            set { m_SeqField = value; }
        }



        /// <summary>
        /// 初始化索引
        /// </summary>
        private void InitSeq()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.TSqlWhere = m_TSqlWhere;
            filter.TSqlOrderBy = m_SeqField + "," + m_IdField;


            LModelElement modelElem = LModelDna.GetElementByName(m_TableName);

            LModelFieldElement seqFElem = modelElem.Fields[m_SeqField];

            LModelList<LModel> models = decipher.GetModelList(filter);

            for (int i = 0; i < models.Count; i++)
            {
                LModel m = models[i];

                m.SetValue(seqFElem, i);

                decipher.UpdateModelProps(m, m_SeqField);
            }


        }

        /// <summary>
        /// 判断是否已经建立排序索引
        /// </summary>
        private bool IsCreateSortTag()
        {
            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.TSqlWhere = m_TSqlWhere;
            filter.Fields = new string[] { m_SeqField };

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
        public void MoveUp(int[] ids)
        {
            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                InitSeq();
            }

            //int[] ids = WebUtil.FormIntList("IG2_DEF_SVIEW_COL_ID");

            if (ids.Length == 0)
            {
                return;
            }

            LModel firstModel = GetLineByTableId(ids[0]);

            LModelList<LModel> models = new LModelList<LModel>();
            models.Add(firstModel);

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.TSqlWhere = m_TSqlWhere;
            filter.And(m_SeqField, firstModel.Get<decimal>(m_SeqField) , Logic.LessThan);
            filter.Top = 2;
            filter.TSqlOrderBy = m_SeqField +" DESC";

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<LModel> targetModels = decipher.GetModelList(filter);

            if (targetModels.Count == 0)
            {
                return;
            }

            decimal newSeq;
            decimal dp = 1;

            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels[0].Get<decimal>(m_SeqField);

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                decimal tarSeq1 = targetModels[1].Get<decimal>(m_SeqField);
                decimal tarSeq2 = targetModels[0].Get<decimal>(m_SeqField);

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
                LModel line = models[i];
                line.SetValue(m_SeqField, newSeq);

                decipher.UpdateModelProps(line, m_SeqField);
            }

            //ResetRows();
            //this.DataGridView2.SetItemFocus(this.DataGridView2.FocusedItem.Index - 1);
        }


        /// <summary>
        /// 向下移动
        /// </summary>
        public void MoveDown(int[] ids)
        {
            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                InitSeq();
            }

            //int[] ids = WebUtil.FormIntList("IG2_DEF_SVIEW_COL_ID");

            if (ids.Length == 0)
            {
                return;
            }

            LModel firstModel = GetLineByTableId(ids[0]);

            LModelList<LModel> models = new LModelList<LModel>();
            models.Add(firstModel);

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.TSqlWhere = m_TSqlWhere;
            filter.And(m_SeqField, firstModel.Get<decimal>(m_SeqField) , Logic.GreaterThan);
            filter.Top = 2;
            filter.TSqlOrderBy = m_SeqField;

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<LModel> targetModels = decipher.GetModelList(filter);

            if (targetModels.Count == 0)
            {
                return;
            }

            decimal newSeq;
            decimal dp = 1;

            LModel targetModels0 = targetModels[0], 
                targetModels1 = null;


            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels0.Get<decimal>(m_SeqField);

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                targetModels1 = targetModels[1];

                decimal tarSeq1 = targetModels0.Get<decimal>(m_SeqField);
                decimal tarSeq2 = targetModels1.Get<decimal>(m_SeqField);

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


            for (int i = 0; i < models.Count; i++)
            {
                newSeq += dp;
                LModel line = models[i];
                line.SetValue(m_SeqField, newSeq);

                decipher.UpdateModelProps(line,m_SeqField);
            }

        }




        private LModel GetLineByTableId(int id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel line = decipher.GetModelByPk(m_TableName, id);

            return line;
        }


    }
}