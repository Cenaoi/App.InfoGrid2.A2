using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using HWQ.Entity;
using System.Reflection;

namespace EasyClick.Web.Mini2.Data
{


    /// <summary>
    /// 记录排序构造工厂
    /// </summary>
    public class RecordSortFactory:IDisposable
    {
        DbDecipher m_Decipher;

        string m_TableName;
        string m_SortField;
        string m_IdField;

        MethodInfo m_FilterMethod;

        object m_Owner;

        public RecordSortFactory(object owner)
        {
            if (owner == null) { throw new ArgumentNullException("owner"); }

            m_Owner = owner;
        }

        //string m_TSqlWhere;

        /// <summary>
        /// 过滤函数
        /// </summary>
        public MethodInfo FilterMethod
        {
            get { return m_FilterMethod; }
            set { m_FilterMethod = value; }
        }


        public DbDecipher Decipher
        {
            get { return m_Decipher; }
            set { m_Decipher = value; }
        }


        ///// <summary>
        ///// T-SQL 的 Where 子语句
        ///// </summary>
        //public string TSqlWhere
        //{
        //    get { return m_TSqlWhere; }
        //    set { m_TSqlWhere = value; }
        //}

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
        public string SortField
        {
            get { return m_SortField; }
            set { m_SortField = value; }
        }



        /// <summary>
        /// 初始化索引
        /// </summary>
        public void SortReset()
        {
            DbDecipher decipher = m_Decipher;

            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.TSqlOrderBy = string.Concat(m_SortField, " ASC,", m_IdField, " ASC");

            m_FilterMethod.Invoke(m_Owner,new object[]{ filter});

            filter.Fields = new string[] { m_IdField, m_SortField };

            LModelElement modelElem = LModelDna.GetElementByName(m_TableName);

            LModelFieldElement seqFElem = modelElem.Fields[m_SortField];

            LModelList<LModel> models = decipher.GetModelList(filter);

            for (int i = 0; i < models.Count; i++)
            {
                LModel m = models[i];

                m.SetValue(seqFElem, i);

                decipher.UpdateModelProps(m, m_SortField);
            }
        }

        /// <summary>
        /// 判断是否已经建立排序索引
        /// </summary>
        private bool IsCreateSortTag()
        {
            LightModelFilter filter = new LightModelFilter(m_TableName);
            //filter.TSqlWhere = m_TSqlWhere;
            filter.Fields = new string[] { m_SortField };
            
            m_FilterMethod.Invoke(m_Owner, new object[] { filter });


            DbDecipher decipher = m_Decipher;

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
        public bool MoveUp(int[] ids)
        {

            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                SortReset();
            }

            //int[] ids = WebUtil.FormIntList("IG2_DEF_SVIEW_COL_ID");

            if (ids==null || ids.Length == 0)
            {
                return false;
            }

            LModel firstModel = GetLineByTableId(ids[0]);

            LModelList<LModel> models = new LModelList<LModel>();
            models.Add(firstModel);

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(m_TableName);
            //filter.TSqlWhere = m_TSqlWhere;


            m_FilterMethod.Invoke(m_Owner, new object[] { filter });

            filter.And(m_SortField, firstModel.Get<decimal>(m_SortField), Logic.LessThan);
            filter.Top = 2;
            filter.TSqlOrderBy = m_SortField + " DESC";
            filter.Fields = new string[] { m_IdField, m_SortField };

            DbDecipher decipher = m_Decipher;

            List<LModel> targetModels = decipher.GetModelList(filter);

            if (targetModels.Count == 0)
            {
                return false;
            }

            decimal newSeq;
            decimal dp = 1;

            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels[0].Get<decimal>(m_SortField);

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                decimal tarSeq1 = targetModels[1].Get<decimal>(m_SortField);
                decimal tarSeq2 = targetModels[0].Get<decimal>(m_SortField);

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
                line.SetValue(m_SortField, newSeq);

                decipher.UpdateModelProps(line, m_SortField);


                line.Dispose();
            }

            return true;
        }


        /// <summary>
        /// 向下移动
        /// </summary>
        public bool MoveDown(int[] ids)
        {
            bool isCreateSortTag = IsCreateSortTag();

            if (!isCreateSortTag)
            {
                SortReset();
            }

            //int[] ids = WebUtil.FormIntList("IG2_DEF_SVIEW_COL_ID");

            if (ids == null || ids.Length == 0)
            {
                return false;
            }

            LModel firstModel = GetLineByTableId(ids[0]);

            LModelList<LModel> models = new LModelList<LModel>();
            models.Add(firstModel);

            //查找排序小于第一个实体的

            LightModelFilter filter = new LightModelFilter(m_TableName);
            //filter.TSqlWhere = m_TSqlWhere;

            m_FilterMethod.Invoke(m_Owner, new object[] { filter });

            filter.And(m_SortField, firstModel.Get<decimal>(m_SortField), Logic.GreaterThan);
            filter.Top = 2;
            filter.TSqlOrderBy = m_SortField;
            filter.Fields = new string[] { m_IdField, m_SortField };

            DbDecipher decipher = m_Decipher;

            List<LModel> targetModels = decipher.GetModelList(filter);

            if (targetModels.Count == 0)
            {
                return false;
            }

            decimal newSeq;
            decimal dp = 1;

            LModel targetModels0 = targetModels[0],
                targetModels1 = null;


            if (targetModels.Count == 1)
            {
                decimal tarSeq = targetModels0.Get<decimal>(m_SortField);

                newSeq = tarSeq;
                dp = 1;
            }
            else
            {
                targetModels1 = targetModels[1];

                decimal tarSeq1 = targetModels0.Get<decimal>(m_SortField);
                decimal tarSeq2 = targetModels1.Get<decimal>(m_SortField);

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
                line.SetValue(m_SortField, newSeq);

                decipher.UpdateModelProps(line, m_SortField);

                line.Dispose();
            }

            return true;
        }




        private LModel GetLineByTableId(int id)
        {
            DbDecipher decipher = m_Decipher;

            LightModelFilter filter = new LightModelFilter(m_TableName);
            filter.And(m_IdField, id);
            filter.Fields = new string[] { m_IdField, m_SortField };

            LModel line = decipher.GetModel(filter);

            return line;
        }


        public void Dispose()
        {
            m_Owner = null;
            m_Decipher = null;
            m_FilterMethod = null;

            GC.SuppressFinalize(this);
        }
    }
}
