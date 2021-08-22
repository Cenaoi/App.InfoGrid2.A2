using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 日期列
    /// </summary>
    [Description("日期列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DateColumn : BoundField
    {
        /// <summary>
        /// (构造函数)日期列
        /// </summary>
        public DateColumn()
        {

            this.MiType = "datecolumn";

            this.DefaultEditorType = "datefield";
        }



        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        public DateColumn(string dataField)
            : base(dataField)
        {
            this.MiType = "datecolumn";

            this.DefaultEditorType = "datefield";
        }

        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        /// <param name="headerText">表头</param>
        public DateColumn(string dataField, string headerText) : base(dataField, headerText)
        {
            this.MiType = "datecolumn";

            this.DefaultEditorType = "datefield";
        }

        /// <summary>
        /// 默认格式
        /// </summary>
        const string DEFAULT_FORMAT = "Y-m-d";

        string m_Format ;

        /// <summary>
        /// 格式化显示
        /// </summary>
        [Description("格式化显示.默认:Y-m-d")]
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            sb.AppendFormat("format:'{0}', ", StringUtil.NoBlank(this.DataFormatString, this.Format, DEFAULT_FORMAT));


            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }

            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SummaryType))
            {
                sb.AppendFormat("summaryType:'{0}',", this.SummaryType);
            }

            CreateStyleRule(sb);

            sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());


            sb.Append(CreateEditor());

            if (this.ItemAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("align:'{0}',", this.ItemAlign.ToString().ToLower());
            }
            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }
            sb.AppendFormat("width:{0} }}", this.Width);

            return sb.ToString();
        }

    }
}
