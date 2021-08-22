using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 数据仓库单元格
    /// </summary>
    [Description("")]
    [DebuggerDisplay("Name={Name},Value={Value}")]
    public class DataStoreCell
    {
        string m_Name;
        string m_Value;

        int m_Index = -1;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }
    }
}
