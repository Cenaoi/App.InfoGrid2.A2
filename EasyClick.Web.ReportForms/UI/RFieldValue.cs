using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace EasyClick.Web.ReportForms.UI
{
    /// <summary>
    /// 字段值
    /// </summary>
    /// <summary>
    /// 列表条目
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("Value={Value}, Text={Text}, Operator={Operator}")]
    public class RFieldValue
    {
        string m_Value;
        string m_Text;
        string m_Type;
        OperatorTypes m_Operator = OperatorTypes.Equals;

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }


        /// <summary>
        /// 值的数据类型
        /// </summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public OperatorTypes Operator
        {
            get { return m_Operator; }
            set { m_Operator = value; }
        }
    }
}
