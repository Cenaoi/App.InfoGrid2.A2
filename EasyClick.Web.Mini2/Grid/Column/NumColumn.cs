using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数值列
    /// </summary>
    [Description("数值列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class NumColumn : BoundField
    {

        /// <summary>
        /// 最大值
        /// </summary>
        public const decimal MAX_VALUE = 1000000000;

        /// <summary>
        /// 最小值
        /// </summary>
        public const decimal MIN_VALUE = -1000000000;

        decimal m_Step = 1;

        int m_DecimalPrecision = 2;

        decimal m_MinValue = MIN_VALUE;

        decimal m_MaxValue = MAX_VALUE;


        /// <summary>
        /// 递增值
        /// </summary>
        [DefaultValue(1)]
        public decimal Step
        {
            get { return m_Step; }
            set { m_Step = value; }
        }

        /// <summary>
        /// 小数点位数
        /// </summary>
        [DefaultValue(2)]
        public int DecimalPrecision
        {
            get { return m_DecimalPrecision; }
            set { m_DecimalPrecision = value; }
        }
        

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MinValue
        {
            get { return m_MinValue; }
            set { m_MinValue = value; }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }


        /// <summary>
        /// (构造函数)数值列
        /// </summary>
        public NumColumn()
        {
            this.MiType = "numbercolumn";
            this.DefaultEditorType = "numberfield";

            this.Width = 80;

            this.ItemAlign = Mini.CellAlign.Right;
        }


        
        public NumColumn(string dataField)
        {

            this.MiType = "numbercolumn";
            this.DefaultEditorType = "numberfield";

            this.Width = 80;
            this.ItemAlign = Mini.CellAlign.Right;


            this.DataField = dataField;
        }

        public NumColumn(string dataField, string headerText)
        {
            this.MiType = "numbercolumn";
            this.DefaultEditorType = "numberfield";

            this.Width = 80;
            this.ItemAlign = Mini.CellAlign.Right;

            this.DataField = dataField;
            this.HeaderText = headerText;
        }

        string m_Format = "{0:0,000.00}";

        /// <summary>
        /// 格式化显示.默认 {0:0,000.00}
        /// </summary>
        [Description("格式化显示.默认 {0:0,000.00}")]
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            ScriptTextWriter st = new ScriptTextWriter(new StringBuilder(), QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");
            {
                st.WriteParam("miType", this.MiType);
                st.WriteParam("dataIndex", this.DataField);
                st.WriteParam("width", this.Width);
                st.WriteParam("headerText", this.HeaderText);
                st.WriteParam("sortable", this.SortMode, Mini.SortMode.Default, TextTransform.Lower);

                st.WriteParam("maxValue", this.MaxValue, MAX_VALUE);
                st.WriteParam("minValue", this.MinValue, MIN_VALUE);

                st.WriteParam("align", this.ItemAlign);

                st.WriteParam("tag", this.Tag);


                st.WriteParam("format", this.Format, "{0:0,000.00}");
                st.WriteParam("sortExpression", this.SortExpression);
                st.WriteParam("resizable", this.Resizable);
                st.WriteParam("notDisplayValue", this.NotDisplayValue);

                st.WriteParam("summaryType", this.SummaryType);
                st.WriteParam("summaryFormat", this.SummaryFormat);

                st.WriteParam("renderer", this.Renderer);

                st.WriteParam("editorMode", this.EditorMode);


                CreateStyleRule(st);    //创建样式规则
                CreateEditor(st);

            }
            st.RetractEnd("}");

            return st.ToString();

            //    StringBuilder sb = new StringBuilder();

            //sb.Append("{");

            //sb.AppendFormat("miType:'{0}', ", this.MiType);

            //sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            //sb.AppendFormat("width:{0}, ", this.Width);
            //sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            


            //if (this.SortMode != Mini.SortMode.Default)
            //{
            //    sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            //}


            //if (m_Format != "{0:0,000.00}")
            //{
            //    sb.AppendFormat("format:'{0}', ", m_Format);
            //}

            //if (!string.IsNullOrEmpty(this.SortExpression))
            //{
            //    sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            //}

            //if (!this.Resizable)
            //{
            //    sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            //}

            //if (!string.IsNullOrEmpty(this.NotDisplayValue))
            //{
            //    sb.AppendFormat("notDisplayValue:'{0}',", this.NotDisplayValue);
            //}

            //if (!string.IsNullOrEmpty(this.SummaryType))
            //{
            //    sb.AppendFormat("summaryType:'{0}',", this.SummaryType);
            //}

            //if (!string.IsNullOrEmpty(this.SummaryFormat))
            //{
            //    sb.AppendFormat("summaryFormat: '{0}',", this.SummaryFormat);
            //}

            //if (!string.IsNullOrEmpty(this.Renderer))
            //{
            //    sb.AppendFormat("renderer: {0},", this.Renderer);
            //}


            //sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());

            //sb.Append(CreateEditor());

            //sb.AppendFormat("align:'{0}'}}", this.ItemAlign.ToString().ToLower());

            //return sb.ToString();
        }
    }
}
