using System;
using System.Collections.Generic;
using System.Web;
using HWQ.Entity.LightModels;
using System.ComponentModel;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 普通报表处理行
    /// </summary>
    /// <param name="target">目标行</param>
    /// <param name="srcData">明细数据</param>
    [Obsolete]
    public delegate void ProcessReportTableRow(LModel target, LModel srcData);

    /// <summary>
    /// 普通表格统计报表
    /// </summary>
    [Obsolete]
    public class ReportTable
    {
        string m_GroupBy;

        /// <summary>
        /// 按组排序。例: "字段1, 字段2, 字段3,..."
        /// </summary>
        public string GroupBy
        {
            get { return m_GroupBy; }
            set { m_GroupBy = value; }
        }

        /// <summary>
        /// 处理报表的行数据
        /// </summary>
        public ProcessReportTableRow Process;

        /// <summary>
        /// 触发处理报表的行数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="srcData"></param>
        protected void OnProcess(LModel target, LModel srcData)
        {
            if (Process == null) { return; }

            Process.Invoke(target, srcData);
        }

        /// <summary>
        /// 目标列表
        /// </summary>
        SortedDictionary<string, LModel> m_TargetList = new SortedDictionary<string, LModel>();

        List<LModel> m_List = new List<LModel>();

        LModelElement m_TargetModelElem;

        /// <summary>
        /// 固定行，不自动增加
        /// </summary>
        bool m_AuotCreateRows = true;

        /// <summary>
        /// 根据GroupBy 字段，自动创建行。默认：true
        /// </summary>
        public bool AutoCreateRows
        {
            get { return m_AuotCreateRows; }
            set { m_AuotCreateRows = value; }
        }

        public LModelElement TargetModelElem
        {
            get { return m_TargetModelElem; }
            set { m_TargetModelElem = value; }
        }

        /// <summary>
        /// 根据 GroupBy 字段，获取统计数据的 唯一标记。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public string GetCID(LModel model, string[] groupBy)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < groupBy.Length; i++)
            {
                string field = groupBy[i];

                string v = model[field].ToString();

                sb.Append(v);
                sb.Append("_");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取最终处理结果
        /// </summary>
        /// <returns></returns>
        public LModel[] GetTargetList()
        {
            //LModel[] ms = new LModel[ m_TargetList.Count];
            //m_TargetList.Values.CopyTo(ms,0);

            return m_List.ToArray();
        }

        /// <summary>
        /// 获取最终处理结果
        /// </summary>
        /// <returns></returns>
        public LModel[] GetOrderList()
        {
            LModel[] ms = new LModel[m_TargetList.Count];
            m_TargetList.Values.CopyTo(ms, 0);

            return ms;
        }



        /// <summary>
        /// 初始化数据行
        /// </summary>
        /// <param name="models"></param>
        public void SetInitRows(LModelList<LModel> models)
        {
            string[] groupBy = GetGroupBy();

            foreach (LModel m in models)
            {
                string cid = GetCID(m, groupBy);

                if (m_TargetList.ContainsKey(cid))
                {
                    continue;
                }

                m_List.Add(m);
                m_TargetList.Add(cid, m);
            }
        }

        private string[] GetGroupBy()
        {
            string[] groups = StringUtil.Split( m_GroupBy);

            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = groups[i].Trim();
            }

            return groups;
        }

        private void InitAuotCreateRow(string[] groupBy, LModelList<LModel> models)
        {

            if (!m_AuotCreateRows)
            {
                return;
            }

            foreach (LModel m in models)
            {
                string cid = GetCID(m, groupBy);

                if (m_TargetList.ContainsKey(cid))
                {
                    continue;
                }

                LModel tModel = new LModel(m_TargetModelElem);

                for (int i = 0; i < groupBy.Length; i++)
                {
                    string field = groupBy[i];
                    tModel[field] = m[field];
                }

                m_List.Add(tModel);
                m_TargetList.Add(cid, tModel);
            }

        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="models"></param>
        public void SetSourceData(LModelList<LModel> models)
        {
            string[] groupBy = GetGroupBy();

            InitAuotCreateRow(groupBy, models);

            foreach (LModel m in models)
            {
                string cid = GetCID(m, groupBy);

                if (!m_TargetList.ContainsKey(cid))
                {
                    continue;
                }

                LModel target = m_TargetList[cid];

                OnProcess(target, m);
            }
        }



    }


}