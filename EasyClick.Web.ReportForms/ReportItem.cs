using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 栏目属性
    /// </summary>
    public class ReportItem
    {
        #region 字段

        ItemFixedValueCollection m_FixedValues;

        RFieldValueMode m_ValueMode = RFieldValueMode.DBValue;

        /// <summary>
        /// 激活汇总
        /// </summary>
        bool m_EnabledTotal = true;

        string m_DBField;

        /// <summary>
        /// 格式化显示：￥{0:#,##0.00}..
        /// </summary>
        string m_Format;

        /// <summary>
        /// 格式类型
        /// </summary>
        string m_FormatType = "default";

        string m_Style;

        /// <summary>
        /// 标头样式
        /// </summary>
        string m_HeadClassName;

        /// <summary>
        /// 单元格样式
        /// </summary>
        string m_CellClassName;



        /// <summary>
        /// 数据值
        /// </summary>
        object m_DBValue;

        /// <summary>
        /// 数据值
        /// </summary>
        string m_Code;

        /// <summary>
        /// 函数名称
        /// </summary>
        string m_FunName = FunTypes.COUNT;

        /// <summary>
        /// 标题
        /// </summary>
        string m_Title;

        int m_Width = 0;



        #endregion

        /// <summary>
        /// (构造函数)栏目属性
        /// </summary>
        public ReportItem()
        {
        }

        /// <summary>
        /// (构造函数)栏目属性
        /// </summary>
        /// <param name="dbField"></param>
        public ReportItem(string dbField)
        {
            m_DBField = dbField;
        }

        /// <summary>
        /// (构造函数)栏目属性
        /// </summary>
        /// <param name="dbField"></param>
        /// <param name="valueMode"></param>
        public ReportItem(string dbField, RFieldValueMode valueMode)
        {
            m_DBField = dbField;
            m_ValueMode = valueMode;
        }

        /// <summary>
        /// (构造函数)栏目属性
        /// </summary>
        /// <param name="dbField"></param>
        /// <param name="valueMode"></param>
        /// <param name="funName"></param>
        public ReportItem(string dbField, RFieldValueMode valueMode, string funName)
        {
            m_DBField = dbField;
            m_ValueMode = valueMode;
            m_FunName = FunName;
        }

        /// <summary>
        /// 单元格宽度
        /// </summary>
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 固定值
        /// </summary>
        public ItemFixedValueCollection FixedValues
        {
            get
            {
                if (m_FixedValues == null)
                {
                    m_FixedValues = new ItemFixedValueCollection();
                }
                return m_FixedValues;
            }
            set
            {
                m_FixedValues = value;
            }
        }


        /// <summary>
        /// 激活汇总。默认值=true
        /// </summary>
        [DefaultValue(true)]
        public bool EnabledTotal
        {
            get { return m_EnabledTotal; }
            set { m_EnabledTotal = value; }
        }

        /// <summary>
        /// 函数名称
        /// </summary>
        public string FunName
        {
            get { return m_FunName; }
            set { m_FunName = value; }
        }

        /// <summary>
        /// 值格式。
        /// </summary>
        public RFieldValueMode ValueMode
        {
            get { return m_ValueMode; }
            set { m_ValueMode = value; }
        }

        /// <summary>
        /// 代码表达式
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object DBValue
        {
            get { return m_DBValue; }
            set { m_DBValue = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        /// <summary>
        /// 自定义样式格式
        /// </summary>
        public string Style
        {
            get { return m_Style; }
            set { m_Style = value; }
        }

        /// <summary>
        /// 样式名称
        /// </summary>
        public string HeadClassName
        {
            get { return m_HeadClassName; }
            set { m_HeadClassName = value; }
        }


        /// <summary>
        /// 样式名称
        /// </summary>
        public string CellClassName
        {
            get { return m_CellClassName; }
            set { m_CellClassName = value; }
        }

        /// <summary>
        /// 格式化显示。例如：￥{0:#,##0.00}..
        /// </summary>
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        /// <summary>
        /// 格式类型
        /// </summary>
        public string FormatType
        {
            get { return m_FormatType; }
            set { m_FormatType = value; }
        }


        /// <summary>
        /// 字段名
        /// </summary>
        public string DBField
        {
            get { return m_DBField; }
            set { m_DBField = value; }
        }

        /// <summary>
        /// 限制只有一个子节点
        /// </summary>
        public bool OneChild { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public ReportOrderTypes OrderType { get; set; } = ReportOrderTypes.ASC;
    }
}
