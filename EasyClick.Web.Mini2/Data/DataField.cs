using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 数据仓库单元格
    /// </summary>
    [Description("")]
    [DebuggerDisplay("Name={Name},Value={Value}")]
    public class DataField
    {
        string m_Name;
        string m_Value;

        int m_Index = -1;

        /// <summary>
        /// 字段名
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 字段值
        /// </summary>
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }
    }
}
