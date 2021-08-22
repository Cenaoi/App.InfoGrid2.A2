using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.IO;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 表格列
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class BoundField : MiniDataControlField,IAttributeAccessor
    {
        public BoundField()
        {
            this.Width = 120;
        }

        public BoundField(string dataField)
            : base(dataField)
        {
            this.Width = 120;
        }

        #region 字段

        internal Control m_Owner;

        CellAlign m_ItemAlign = CellAlign.Left;

        /// <summary>
        /// 允许输入
        /// </summary>
        internal bool m_AllowInput = false;

        bool m_ReadOnly = false;

        string m_DataFormatString = string.Empty;

        bool m_ToExcel = true;

        /// <summary>
        /// 标题的提示信息
        /// </summary>
        string m_HeaderTooltip = string.Empty;

        /// <summary>
        /// 单元格的提示信息
        /// </summary>
        string m_Tooltip = string.Empty;

        /// <summary>
        /// 不显示的值，空格显示为空状态
        /// </summary>
        string m_NotDisplayValue = string.Empty;

        /// <summary>
        /// 排序模式
        /// </summary>
        SortMode m_SortMode = SortMode.Default;

        /// <summary>
        /// 排序字段表达式。
        /// </summary>
        /// <example>Field1 asc,Field2 desc</example>
        string m_SortExpression = string.Empty;

        /// <summary>
        /// 升序字段表达式。
        /// </summary>
        string m_SortAscExpression = string.Empty;
        /// <summary>
        /// 降序字段表达式
        /// </summary>
        string m_SortDescExpression = string.Empty;

        int m_Index = 0;

        int m_ColSpan = 1;

        bool m_HeaderVisible = true;

        #endregion

        #region Sort

        /// <summary>
        /// 排序模式
        /// </summary>
        [DefaultValue(SortMode.Default),DisplayName("排序模式")]
        [Category("Sort")]
        public SortMode SortMode
        {
            get { return m_SortMode; }
            set { m_SortMode = value; }
        }

        /// <summary>
        /// 排序字段表达式
        /// </summary>
        [DefaultValue("")]
        [Category("Sort"), DisplayName("排序模式")]
        public new string SortExpression
        {
            get { return m_SortExpression; }
            set { m_SortExpression = value; }
        }

        /// <summary>
        /// 升序字段表达式
        /// </summary>
        [DefaultValue("")]
        [Category("Sort"),DisplayName("升序字段表达式")]
        public string SortAscExpression
        {
            get { return m_SortAscExpression; }
            set { m_SortAscExpression = value; }
        }

        /// <summary>
        /// 降序字段表达式
        /// </summary>
        [DefaultValue("")]
        [Category("Sort"), DisplayName("降序字段表达式")]
        public string SortDescExpression
        {
            get { return m_SortDescExpression; }
            set { m_SortDescExpression = value; }
        }

        #endregion

        /// <summary>
        /// 标题可见
        /// </summary>
        [DefaultValue(true),DisplayName("标题可见")]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
        }

        /// <summary>
        /// 合并行
        /// </summary>
        [DefaultValue(1),DisplayName("合并行")]
        public int ColSpan
        {
            get { return m_ColSpan; }
            set { m_ColSpan = value; }
        }

        /// <summary>
        /// 字段索引
        /// </summary>
        [Browsable(false)]
        [DefaultValue(0)]
        public int Index
        {
            get { return m_Index; }
        }

        internal void SetIndex(int index)
        {
            m_Index = index;
        }


        /// <summary>
        /// 不显示的值
        /// </summary>
        [DefaultValue(""),DisplayName("不显示的值")]
        public string NotDisplayValue
        {
            get { return m_NotDisplayValue; }
            set { m_NotDisplayValue = value; }
        }

        /// <summary>
        /// 标题提示
        /// </summary>
        [DefaultValue(""),DisplayName("标题提示")]
        public virtual string HeaderTooltip
        {
            get { return m_HeaderTooltip; }
            set { m_HeaderTooltip = value; }
        }

        /// <summary>
        /// 单元格的提示
        /// </summary>
        [DefaultValue(""),DisplayName("单元格的提示信息")]
        public virtual string Tooltip
        {
            get { return m_Tooltip; }
            set { m_Tooltip = value; }
        }



        /// <summary>
        /// 导出到 Excel。默认:true
        /// </summary>
        [DefaultValue(true),DisplayName("导出 Excel")]
        public virtual bool ToExcel
        {
            get { return m_ToExcel; }
            set { m_ToExcel = value; }
        }

        /// <summary>
        /// 允许输入
        /// </summary>
        /// <returns></returns>
        public bool AllowInput()
        {
            return m_AllowInput;
        }


        /// <summary>
        /// 单元格
        /// </summary>
        [DefaultValue(CellAlign.Left),DisplayName("单元格竖排序")]
        public virtual CellAlign ItemAlign
        {
            get { return m_ItemAlign; }
            set { m_ItemAlign = value; }
        }

        [Description("获取或设置数据格式化.例如:{0:#,##0}")]
        [DefaultValue(""),DisplayName("数据格式化")]
        public virtual string DataFormatString
        {
            get { return m_DataFormatString; }
            set { m_DataFormatString = value; }
        }

        /// <summary>
        /// 只读状态
        /// </summary>
        [DefaultValue(false),DisplayName("只读")]
        public virtual bool ReadOnly
        {
            get { return m_ReadOnly; }
            set { m_ReadOnly = value; }
        }


        protected internal Control Owner
        {
            get
            {
                return m_Owner;
            }
        }


        protected internal virtual void OnInit()
        {

        }

        protected internal virtual void OnLoad()
        {

        }

        public override string DebugModeCreateHtml(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (!this.Visible)
            {
                return string.Empty;
            }

            return base.DebugModeCreateHtml(cellType, rowState);
        }

        protected virtual void CreateHeaderHtmlTemplate(HtmlTextWriter writer,MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (!m_HeaderVisible)
            {
                return;
            }

            DataGrid grid = m_Owner as DataGrid;

            if (!string.IsNullOrEmpty(this.HeaderTooltip))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, this.HeaderTooltip);
            }

            if (m_ColSpan > 1)
            {
                writer.AddAttribute("colspan", m_ColSpan.ToString());
            }

            //if (!this.Visible)
            //{
            //    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            //}

            writer.AddAttribute("nowrap", "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Th);

            if ((grid != null && !grid.AllowSorting) || m_SortMode == Mini.SortMode.None)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Nobr);
                {
                    writer.Write(this.HeaderText);
                }
                writer.RenderEndTag();
            }
            else if (m_SortMode == Mini.SortMode.Default)
            {
                
                if (!string.IsNullOrEmpty(this.DataField) && string.IsNullOrEmpty(m_SortExpression))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Nobr);
                    {
                        writer.AddAttribute("href", "#" + this.DataField);
                        writer.AddAttribute("onclick", string.Format("{0}.sort(this,'{1}',{2})", grid.GetClientID(), this.DataField, m_Index));

                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(this.HeaderText);
                        writer.RenderEndTag();
                    }
                    writer.RenderEndTag();
                }
                else if (!string.IsNullOrEmpty(m_SortExpression))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Nobr);
                    {
                        writer.AddAttribute("href", "#" + m_SortExpression);
                        writer.AddAttribute("onclick", string.Format("{0}.sort(this,'{1}',{2})", grid.GetClientID(), m_SortExpression, m_Index));

                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(this.HeaderText);
                        writer.RenderEndTag();
                    }
                    writer.RenderEndTag();

                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Nobr);
                    {
                        writer.Write(this.HeaderText);
                    }
                    writer.RenderEndTag();
                }
            }
            else if (m_SortMode == Mini.SortMode.User)
            {
                
            }

            writer.RenderEndTag();
        }

        /// <summary>
        /// 创建 Html 模板
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (!this.Visible)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }

            if (cellType == MiniDataControlCellType.Header)
            {
                CreateHeaderHtmlTemplate(writer, MiniDataControlCellType.Header, rowState);
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {


                writer.AddAttribute("ColumnID", this.DataField );
                if (m_ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", m_ItemAlign.ToString().ToLower());
                }

                if (m_HtmlAttrs != null)
                {
                    foreach (MiniHtmlAttr attr in m_HtmlAttrs.Values)
                    {
                        writer.AddAttribute(attr.Key, attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(this.Tooltip))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, this.Tooltip);
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if (string.IsNullOrEmpty(m_NotDisplayValue))
                {
                    //writer.Write(string.Format("{{$T.{0}}}", this.DataField));
                    writer.Write(GetDataBindExp(this.DataField));
                }
                else
                {
                    writer.Write("{{#if $T.{0} == '{1}' }}&nbsp;{{#else}}{{$T.{0}}}{{#/if}}", this.DataField, m_NotDisplayValue);
                }


                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }
            
            string txt = tw.ToString();

            writer.Dispose();
            tw.Dispose();

            return txt;
        }

        /// <summary>
        /// 获取数据绑定的格式
        /// </summary>
        /// <param name="dataField"></param>
        /// <returns></returns>
        protected string GetDataBindExp(string dataField)
        {
            if (string.IsNullOrEmpty(dataField))
            {
                return string.Empty;
            }

            int n1 = dataField.IndexOf('{');

            if (n1 > -1)
            {
                int n2 = dataField.IndexOf('}', n1 + 1);

                if (n2 > -1)
                {
                    return dataField ;
                }
            }

            if (dataField == "#")
            {
                return dataField;
            }

            return "{$T." + dataField + "}";
        }


        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion

    }



}
