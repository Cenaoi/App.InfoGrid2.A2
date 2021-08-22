using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using System.Web.UI;
using System.ComponentModel;
using System.IO;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 编辑模式
    /// </summary>
    public enum EditorMode
    {
        /// <summary>
        /// 不能编辑
        /// </summary>
        None,
        /// <summary>
        /// 自动编辑模式
        /// </summary>
        Auto,
        /// <summary>
        /// 自定义编辑模式
        /// </summary>
        Custom
    }

    /// <summary>
    /// 表格列
    /// </summary>
    [Description("表格列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class BoundField : MiniDataControlField
    {
        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        public BoundField()
        {
            this.Width = 120;
        }

        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        public BoundField(string dataField)
            : base(dataField)
        {
            this.Width = 120;
        }

        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        /// <param name="headerText">表头</param>
        public BoundField(string dataField, string headerText)
        {
            this.Width = 120;
            this.DataField = dataField;
            this.HeaderText = headerText;
        }


        /// <summary>
        /// 获取或设置分配给服务器控件的编程标识符
        /// </summary>
        [Filterable(false)]
        [MergableProperty(false)]
        [ParenthesizePropertyName(true)]
        [Themeable(false)]
        public virtual string ID { get; set; }

        public virtual string ClientID
        {
            get
            {
                if (string.IsNullOrEmpty(this.ID))
                {
                    return null;
                }

                return this.Owner.ClientID + "_" + this.ID;
            }
        }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string SecFunCode { get; set; }

        /// <summary>
        /// 列的只读权限
        /// </summary>
        public string SecReadonly { get; set; }

        #region 字段

        internal Control m_Owner;

        /// <summary>
        /// 自动去除录入过程中,出现的左右两边空格
        /// </summary>
        bool m_AutoTrim = true;

        CellAlign m_ItemAlign = CellAlign.Left;

        CellAlign m_HeaderAlign = CellAlign.Left;

        string m_miType = "col";

        string m_DefaultEditorType = "text";

        /// <summary>
        /// 允许输入
        /// </summary>
        EditorMode m_EditorMode = EditorMode.Auto;
        
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


        bool m_Sortable = true;

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

        /// <summary>
        /// 可调整大小
        /// </summary>
        bool m_Resizable = true;

        /// <summary>
        /// 单元格点击在事件
        /// </summary>
        string m_Click;

        /// <summary>
        /// 标签，给用户做扩展用
        /// </summary>
        string m_Tag;

        /// <summary>
        /// 底部概要类型. count ,sum, average...
        /// </summary>
        string m_SummaryType;

        /// <summary>
        /// 汇总格式化
        /// </summary>
        string m_SummaryFormat;

        /// <summary>
        /// 自定义显示的 JS 函数
        /// </summary>
        string  m_Renderer;

        #endregion

        #region Sort

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序"),Category("Sort")]
        [DefaultValue(true)]
        public bool Sortable
        {
            get { return m_Sortable; }
            set { m_Sortable = value; }
        }

        /// <summary>
        /// 排序模式
        /// </summary>
        [DefaultValue(SortMode.Default), DisplayName("排序模式")]
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
        [Category("Sort"), DisplayName("升序字段表达式")]
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


        #region 汇总

        /// <summary>
        /// 底部概要类型. count ,sum, avg...
        /// </summary>
        [Category("Summary"), DefaultValue("")]
        [Description("汇总. count ,sum, average...")]
        public string SummaryType
        {
            get { return m_SummaryType; }
            set { m_SummaryType = value; }
        }


        /// <summary>
        /// 汇总格式化显示
        /// </summary>
        [Category("Summary"), DefaultValue("")]
        [Description("汇总格式化显示")]
        public string SummaryFormat
        {
            get { return m_SummaryFormat; }
            set { m_SummaryFormat = value; }
        }


        #endregion

        Typeahead m_Typeahead;

        /// <summary>
        /// 输入提示的参数
        /// </summary>
        ParamCollection m_TypeaheadParams;


        /// <summary>
        /// 输入提示
        /// </summary>
        [Description("输入提示")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Typeahead Typeahead
        {
            get
            {
                if (m_Typeahead == null)
                {
                    m_Typeahead = new Typeahead();
                }
                return m_Typeahead;
            }
        }



        /// <summary>
        /// 输入提示的参数
        /// </summary>
        [Description("输入提示的参数")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParamCollection TypeaheadParams
        {
            get
            {
                if (m_TypeaheadParams == null)
                {
                    m_TypeaheadParams = new ParamCollection();
                }
                return m_TypeaheadParams;
            }
        }



        /// <summary>
        /// 规则集合
        /// </summary>
        StyleRuleCollection m_StyleRules;


        /// <summary>
        /// 样式的规则集合
        /// </summary>
        [Description("样式的规则集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public StyleRuleCollection StyleRules
        {
            get
            {
                if(m_StyleRules == null)
                {
                    m_StyleRules = new StyleRuleCollection();
                }
                return m_StyleRules;
            }
        }

        /// <summary>
        /// 存在样式规则集合
        /// </summary>
        /// <returns></returns>
        public bool HasStyleRule()
        {
            return ArrayUtil.HasItem(m_StyleRules);
        }

        MapItemCollection m_TypeaheadMapItems;

        /// <summary>
        /// 映射集合
        /// </summary>
        [Description("快速录入的映射映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public virtual MapItemCollection TypeaheadMapItems
        {
            get
            {
                if (m_TypeaheadMapItems == null)
                {
                    m_TypeaheadMapItems = new MapItemCollection();
                }
                return m_TypeaheadMapItems;
            }
        }


        /// <summary>
        /// 存在快速录入的映射
        /// </summary>
        /// <returns></returns>
        public bool HasTypeaheadMapItems()
        {
            return ArrayUtil.HasItem(m_TypeaheadMapItems);
        }


        MapItemCollection m_MapItems;

        /// <summary>
        /// 映射集合
        /// </summary>
        [Description("映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public virtual MapItemCollection MapItems
        {
            get
            {
                if (m_MapItems == null)
                {
                    m_MapItems = new MapItemCollection();
                }
                return m_MapItems;
            }
        }

        /// <summary>
        /// 存在映射规则
        /// </summary>
        /// <returns></returns>
        public bool HasMapItems()
        {
            return m_MapItems != null && m_MapItems.Count > 0;
        }


        /// <summary>
        /// 存在输入提示
        /// </summary>
        /// <returns></returns>
        public bool HasTypeahead()
        {
            return m_Typeahead != null && m_Typeahead.Enabled;
        }

        /// <summary>
        /// 自动去除录入过程中,左右两边的空格
        /// </summary>
        [Description("自动去除录入过程中,左右两边的空格")]
        [DefaultValue(true)]
        public  bool AutoTrim
        {
            get { return m_AutoTrim; }
            set { m_AutoTrim = value; }
        }

        /// <summary>
        /// 水印
        /// </summary>
        [Description("水印")]
        [DefaultValue("")]
        public string Placeholder { get; set; }


        /// <summary>
        /// 自定义显示的 JS 函数
        /// </summary>
        public string Renderer
        {
            get { return m_Renderer; }
            set { m_Renderer = value; }
        }

        /// <summary>
        /// 必填
        /// </summary>
        [DefaultValue("必填")]
        public bool Required { get; set; } = false;

        /// <summary>
        /// 标签,扩展用
        /// </summary>
        [Description("标签，扩展用")]
        [DefaultValue("")]
        public string Tag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }

        /// <summary>
        /// 单元格点击在事件。(填写函数名)
        /// </summary>
        [Description("单元格点击在事件。(填写函数名)")]
        [DefaultValue("")]
        public string Click
        {
            get { return m_Click; }
            set { m_Click = value; }
        }

        /// <summary>
        /// 可调整大小
        /// </summary>
        [Description("可调整大小")]
        [DefaultValue(true)]
        public bool Resizable
        {
            get { return m_Resizable; }
            set { m_Resizable = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("col")]
        public string MiType
        {
            get { return m_miType; }
            set { m_miType = value; }
        }

        /// <summary>
        /// 默认编辑类型
        /// </summary>
        [Description("默认编辑类型")]
        public string DefaultEditorType
        {
            get { return m_DefaultEditorType; }
            set { m_DefaultEditorType = value; }
        }
    

        /// <summary>
        /// 标题可见
        /// </summary>
        [DefaultValue(true), DisplayName("标题可见")]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
        }

        /// <summary>
        /// 合并行
        /// </summary>
        [DefaultValue(1), DisplayName("合并行")]
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
        [DefaultValue(""), DisplayName("不显示的值")]
        public string NotDisplayValue
        {
            get { return m_NotDisplayValue; }
            set { m_NotDisplayValue = value; }
        }

        /// <summary>
        /// 标题提示
        /// </summary>
        [DefaultValue(""), DisplayName("标题提示")]
        public virtual string HeaderTooltip
        {
            get { return m_HeaderTooltip; }
            set { m_HeaderTooltip = value; }
        }

        /// <summary>
        /// 单元格的提示
        /// </summary>
        [DefaultValue(""), DisplayName("单元格的提示信息")]
        public virtual string Tooltip
        {
            get { return m_Tooltip; }
            set { m_Tooltip = value; }
        }



        /// <summary>
        /// 导出到 Excel。默认:true
        /// </summary>
        [DefaultValue(true), DisplayName("导出 Excel")]
        public virtual bool ToExcel
        {
            get { return m_ToExcel; }
            set { m_ToExcel = value; }
        }


        /// <summary>
        /// 单元格水平对齐
        /// </summary>
        [DefaultValue(CellAlign.Left), DisplayName("单元格水平对齐")]
        public virtual CellAlign ItemAlign
        {
            get { return m_ItemAlign; }
            set { m_ItemAlign = value; }
        }

        /// <summary>
        /// 标题水平对齐
        /// </summary>
        [DefaultValue(CellAlign.Left), DisplayName("标题水平对齐")]
        public virtual CellAlign HeaderAlign
        {
            get { return m_HeaderAlign; }
            set { m_HeaderAlign = value; }
        }

        [Description("获取或设置数据格式化.例如:{0:#,##0}")]
        [DefaultValue(""), DisplayName("数据格式化")]
        public virtual string DataFormatString
        {
            get { return m_DataFormatString; }
            set { m_DataFormatString = value; }
        }

        /// <summary>
        /// 编辑状态
        /// </summary>
        [DefaultValue(EditorMode.Auto), DisplayName("编辑状态")]
        public virtual EditorMode EditorMode
        {
            get { return m_EditorMode; }
            set { m_EditorMode = value; }
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


        protected virtual void CreateHeaderHtmlTemplate(HtmlTextWriter writer, MiniDataControlCellType cellType, MiniDataControlRowState rowState)
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

            ScriptTextWriter st = new ScriptTextWriter(new StringBuilder(), QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");
            {
                st.WriteParam("miType", this.MiType, "col");

                st.WriteParam("dataIndex", this.DataField);
                st.WriteParam("headerText", this.HeaderText);

                st.WriteParam("width", this.Width);

                st.WriteParam("autoTrim", this.AutoTrim, true);

                st.WriteParam("required", this.Required);

                st.WriteParam("tag", this.Tag);

                st.WriteParam("sortable", this.Sortable,true);

                if (this.SortMode != Mini.SortMode.Default)
                {
                    st.WriteParam("sortable", this.SortMode, Mini.SortMode.Default, TextTransform.Lower);
                }

                st.WriteParam("sortExpression", this.SortExpression);

                st.WriteParam("resizable", this.Resizable, true);

                st.WriteParam("align", this.ItemAlign, CellAlign.Left, TextTransform.Lower);

                st.WriteParam("click", this.Click);

                st.WriteParam("summaryType", this.SummaryType);
                st.WriteParam("summaryFormat", this.SummaryFormat);

                st.WriteParam("placeholder", this.Placeholder);
                st.WriteParam("notDisplayValue", this.NotDisplayValue);

                st.WriteParam("secFunCode", this.SecFunCode);   //权限编码
                st.WriteParam("secReadonly", this.SecReadonly);   //权限编码

                st.WriteParam("renderer", this.Renderer);


                CreateStyleRule(st);    //创建样式规则


                CreateEditor(st);


            }
            st.RetractEnd("}");

            string json = st.ToString();

            st.Dispose();
            

            return json;
        }



        /// <summary>
        /// 创建样式规则
        /// </summary>
        /// <param name="st"></param>
        protected virtual void CreateStyleRule(StringBuilder st)
        {
            //包含样式规则
            if (!this.HasStyleRule())
            {
                return;
            }


            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            int n = 0;

            foreach (var ruleItem in this.StyleRules)
            {
                if (n++ > 0) { sb.Append(","); }

                sb.Append("{");

                if (ruleItem is StyleRuleScript)
                {
                    StyleRuleScript rs = (StyleRuleScript)ruleItem;

                    sb.Append("role:'script', ");

                    sb.Append("script: '").Append(JsonUtil.ToJson(rs.Script, JsonQuotationMark.SingleQuotes)).Append("'");
                }

                sb.Append("}");
            }


            sb.Append("]");


            st.AppendFormat("styleRules: {0},", sb.ToString());

        }

        /// <summary>
        /// 创建样式规则
        /// </summary>
        /// <param name="st"></param>
        protected virtual void CreateStyleRule(ScriptTextWriter st)
        {
            //包含样式规则
            if (!this.HasStyleRule())
            {
                return;
            }


            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            int n = 0;

            foreach (var ruleItem in this.StyleRules)
            {
                if (n++ > 0) { sb.Append(","); }

                sb.Append("{");

                if(ruleItem is StyleRuleScript)
                {
                    StyleRuleScript rs = (StyleRuleScript)ruleItem;

                    sb.Append("role:'script', ");

                    sb.Append("script: '").Append(JsonUtil.ToJson(rs.Script, JsonQuotationMark.SingleQuotes)).Append("'");
                }

                sb.Append("}");
            }


            sb.Append("]");


            st.WriteFunction("styleRules", sb.ToString());

        }


        protected virtual void CreateTypeahead(ScriptTextWriter st)
        {
            //输入自动提示
            if (this.HasTypeahead())
            {
                st.WriteFunction("typeahead", this.Typeahead.ToJson());

                if (this.TypeaheadParams.Count > 0)
                {
                    st.WriteFunction("typeaheadParams", this.TypeaheadParams.ToJson());
                }

                if (this.HasTypeaheadMapItems())
                {
                    st.WriteFunction("typeaheadMapItems", this.TypeaheadMapItems.ToJson());
                }
            }
        }

        protected virtual void CreateMapItems(ScriptTextWriter st)
        {

            //映射
            if (this.HasMapItems())
            {
                st.WriteFunction("mapItems", this.MapItems.ToJson());
            }
        }

        protected virtual void CreateEditor(ScriptTextWriter st)
        {
            if (this.EditorMode != Mini2.EditorMode.Auto)
            {
                st.WriteParam("editorMode", this.EditorMode, EditorMode.Auto, TextTransform.Lower);
            }

            st.RenderBengin("editor");
            {
                st.WriteParam("tag", this.Placeholder);
                st.WriteParam("xtype", this.DefaultEditorType);

                CreateTypeahead(st);

                CreateMapItems(st);

            }
            st.RenderEnd();

        }

        protected virtual string CreateEditor()
        {
            StringBuilder sb = new StringBuilder();


            if ( this.EditorMode == Mini2.EditorMode.Auto)
            {
                sb.Append("editor:{");
                sb.AppendFormat("tag:'{0}',", MiniHelper.GetItemJson(this.Tag));
                sb.AppendFormat("xtype:'{0}'", this.DefaultEditorType);
                sb.Append("}, ");
            }
            else
            {
                sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());
            }

            return sb.ToString();
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
                    return dataField;
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
