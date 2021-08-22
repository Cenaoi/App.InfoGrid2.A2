using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.ReportForms
{

    class ValueIndexNodeList : SortedList<string, ValueIndexNode>
    {
        /// <summary>
        /// 不排序的列表
        /// </summary>
        List<ValueIndexNode> m_List = new List<ValueIndexNode>();


        /// <summary>
        /// 排序
        /// </summary>
        public ReportOrderTypes OrderType { get; set; } = ReportOrderTypes.ASC;

        public new void Add(string key, ValueIndexNode value)
        {
            m_List.Add(value);
            base.Add(key, value);
        }

        public List<ValueIndexNode> ToList
        {
            get { return m_List; }
        }

    }
}
