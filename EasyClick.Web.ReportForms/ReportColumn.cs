using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using EC5.Utility;

namespace EasyClick.Web.ReportForms
{


    /// <summary>
    /// 列
    /// </summary>
    [DebuggerDisplay("Text={Text}, Value={Value}, Name={Name}, Depth={Depth}")]
    public class ReportColumn
    {
        public ReportColumn(string text)
        {
            m_Text = text;
        }

        public ReportColumn()
        {
        }

        int m_Depth = 0;
        string m_Name;
        string m_Text;

        string m_Value;

        string m_Format;

        OperatorTypes m_Operator = OperatorTypes.Equals;

        /// <summary>
        /// 宽度,给 UI 使用
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 格式化显示，例如: {0:yyyy-MM-dd}
        /// </summary>
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        /// <summary>
        /// 值的格式化
        /// </summary>
        public string ValueFormat { get; set; }

        /// <summary>
        /// 操作符号
        /// </summary>
        public OperatorTypes Operator
        {
            get { return m_Operator; }
            set { m_Operator = value; }
        }

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
        /// 内部数据字段
        /// </summary>
        public string InnerDBField { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        public bool EnabledTotal { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string FormatType { get; set; }

        /// <summary>
        /// 自定义的代码
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// 值模式
        /// </summary>
        public RFieldValueMode ValueMode { get; set; } = RFieldValueMode.None;

        /// <summary>
        /// 排序类型
        /// </summary>
        public ReportOrderTypes OrderType { get; set; } = ReportOrderTypes.ASC;


        /// <summary>
        /// 处理值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ProValue(object value)
        {
            bool isOk = true;

            if(value == null)
            {
                
                isOk = false;
            }
            else if (value is string)
            {
                //按照字符串处理
                isOk = ProValue_ForStr((string)value);
            }
            else if (value is DateTime)
            {
                //按照日期处理

                //按照字符串处理
                isOk = ProValue_ForStr(value.ToString());
            }
            else
            {
                //按照数字处理

                isOk = ProValue_ForStr(value.ToString());

            }
            
            return isOk;
        }




        bool m_IsChangedToDecimal = false;
        decimal m_ValueDecimal = 0;

        decimal[] m_BufferValues_ForDecimal;
        string[] m_BufferValues_ForStr;

        private void InitBufferValue_ForDecimal()
        {
            if (m_BufferValues_ForDecimal != null) { return; }

            m_BufferValues_ForDecimal = StringUtil.ToDecimalList(m_Value);
        }

        private void InitBufferValue_ForStr()
        {
            if (m_BufferValues_ForStr != null) { return; }

            m_BufferValues_ForStr = StringUtil.Split(m_Value);
        }


        private bool ProValue_ForDecimal(decimal value)
        {
            if (!m_IsChangedToDecimal)
            {
                decimal.TryParse(m_Value,out m_ValueDecimal);
                m_IsChangedToDecimal = true;
            }


            bool isOk = false;

            switch (m_Operator)
            {
                case OperatorTypes.Equals:
                    isOk = (value == m_ValueDecimal);
                    break;
                case OperatorTypes.GreaterThan:
                    isOk = (value > m_ValueDecimal);
                    break;
                case OperatorTypes.GreaterThanOrEqual:
                    isOk = (value >= m_ValueDecimal);
                    break;
                case OperatorTypes.Inequality:
                    isOk = (value != m_ValueDecimal);
                    break;
                case OperatorTypes.LessThan:
                    isOk = (value < m_ValueDecimal);
                    break;
                case OperatorTypes.LessThanOrEqual:
                    isOk = (value <= m_ValueDecimal);
                    break;
                case OperatorTypes.In:
                    InitBufferValue_ForDecimal();
                    isOk = ArrayUtil.Exist<decimal>(m_BufferValues_ForDecimal, value);
                    break;
                case OperatorTypes.NotIn:
                    InitBufferValue_ForDecimal();
                    isOk = !ArrayUtil.Exist<decimal>(m_BufferValues_ForDecimal, value);
                    break;
            }

            return isOk;
        }


        private bool ProValue_ForDateTime(DateTime time)
        {
            throw new Exception("未实现");
        }


        /// <summary>
        /// 处理值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ProValue_ForStr(string value)
        {
            if (value == null)
            {
                return false;
            }

            bool isOk = false;

            switch (m_Operator)
            {
                case OperatorTypes.Equals:
                    isOk = (value == m_Value);
                    break;
                case OperatorTypes.Inequality: 
                    isOk = !(value == m_Value);
                    break;
                case OperatorTypes.Like:
                    isOk = value.Contains(m_Value);
                    break;
                case OperatorTypes.LeftLike:
                    isOk = value.StartsWith(m_Value);
                    break;
                case OperatorTypes.RightLike:
                    isOk = value.EndsWith(m_Value);
                    break;
                case OperatorTypes.In:
                    InitBufferValue_ForStr();

                    isOk = ArrayUtil.Exist(m_BufferValues_ForStr, value);

                    break;
                case OperatorTypes.NotIn: 
                    InitBufferValue_ForStr();
                    isOk = !ArrayUtil.Exist(m_BufferValues_ForStr, value);
                    break;
                case OperatorTypes.NotLike:
                    isOk = !value.Contains(m_Value);
                    break;
            }

            return isOk;
        }
    }


}
