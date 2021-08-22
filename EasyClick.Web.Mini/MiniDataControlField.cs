using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 轻量级单元格控件字段
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class MiniDataControlField
    {
        #region 构造方法

        public MiniDataControlField()
        {
        }

        public MiniDataControlField(string dataField)
        {
            m_DataField = dataField;
            m_HeaderText = dataField;
        }

        public MiniDataControlField(string dataField, string headerText, int width)
        {
            m_DataField = dataField;
            m_HeaderText = headerText;
            this.Width = width;
        }

        #endregion

        string m_HeaderText = string.Empty;
        string m_DataField = string.Empty;
        string m_FooterText = string.Empty;

        bool m_Visible = true;

        /// <summary>
        /// 标题文字
        /// </summary>
        [Description("标题文字")]
        public string HeaderText
        {
            get { return m_HeaderText; }
            set { m_HeaderText = value; }
        }

        /// <summary>
        /// 页脚文字
        /// </summary>
        [DefaultValue("")]
        public string FooterText
        {
            get { return m_FooterText; }
            set { m_FooterText = value; }
        }

        [Description("宽度.像素 px单位")]
        [DefaultValue(0)]
        public virtual int Width { get; set; }

        /// <summary>
        /// 数据绑定的字段
        /// </summary>
        [Description("数据绑定的字段")]
        [DefaultValue("")]
        public string DataField
        {
            get { return m_DataField; }
            set { m_DataField = value; }
        }

        [Description("")]
        [DefaultValue(true)]
        public virtual bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        [DefaultValue("")]
        public string SortExpression
        {
            get;
            set;
        }

        protected virtual void InitializeDataCell(MiniDataControlCellType cell, MiniDataControlRowState rowState)
        {

        }

        public virtual void InitializeCell(MiniDataControlField cell, MiniDataControlCellType cellType, MiniDataControlRowState rowState, int rowIndex)
        {

        }

        public virtual string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            return string.Empty;
        }

        public virtual string DebugModeCreateHtml(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            return CreateHtmlTemplate(cellType, rowState) ;
        }
    }

}
