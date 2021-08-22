using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Filter;

namespace EC5.DbCascade.DbCascadeEngine
{
    /// <summary>
    /// 数据级联管理器
    /// </summary>
    public static class DbccManager
    {
        static DbccActRSorted m_Actions = new DbccActRSorted();

        /// <summary>
        /// 级联条目
        /// </summary>
        public static DbccActRSorted Acts
        {
            get { return m_Actions; }
        }


    }
















}
