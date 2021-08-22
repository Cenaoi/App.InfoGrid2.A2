using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{

    public class DataSeletedItem
    {
        string m_Guid;
        string m_Pk;

        int m_Index;

        /// <summary>
        /// 记录索引值
        /// </summary>
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

        public string Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
        }

        /// <summary>
        /// 主键值
        /// </summary>
        public string Pk
        {
            get { return m_Pk; }
            set { m_Pk = value; }
        }
    }
}
