using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasyClick.Web.ReportForms
{

    //数据索引
    [DebuggerDisplay("Text={Text}, Value={Value}, InnerDBField={InnerDBField}")]
    class ValueIndexNode
    {
        bool m_IsInit = false;

        public string Value { get; set; }
        public string Text { get; set; }

        public int Depth { get; set; }

        /// <summary>
        /// 内部字段名..配合 InnerCode
        /// </summary>
        public string InnerDBField { get; internal set; }

        /// <summary>
        /// 只限制一个子节点
        /// </summary>
        public bool OneChild { get; internal set; }

        /// <summary>
        /// 列的宽度,给UI使用
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 已经初始化
        /// </summary>
        public bool IsInit
        {
            get { return m_IsInit; }
            set { m_IsInit = value; }
        }

        public ValueIndexNode()
        {
        }

        public ValueIndexNode(string value)
        {
            this.Value = value;
        }

        ValueIndexNodeList m_Childs;

        public ValueIndexNodeList Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new ValueIndexNodeList();
                }
                return m_Childs;
            }
        }

        public string Code { get; internal set; }
        public RFieldValueMode ValueMode { get; internal set; }
        public string Format { get; internal set; }

        /// <summary>
        /// 数据区的格式化
        /// </summary>
        public string ValueFormat { get; internal set; }

        public string FormatType { get; internal set; }

        public ValueIndexNode AddChild(string value,string text, bool oneChild = false)
        {
            if (this.Childs.ContainsKey(value))
            {
                return this.Childs[value];
            }

            ValueIndexNode vin = new ValueIndexNode(value);
            vin.Text = text;
            vin.Depth = this.Depth + 1;
            vin.OneChild = oneChild;

            this.Childs.Add(value, vin);

            return vin;
        }

        public ValueIndexNode AddChild(string value)
        {
            return AddChild(value, value);
        }

        public bool Exist(string value)
        {
            return this.Childs.ContainsKey(value);
        }

        public ValueIndexNode GetChild(string value)
        {
            return this.Childs[value];
        }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            return (m_Childs != null && m_Childs.Count > 0);
        }
    }
}
