using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 日期时间列
    /// </summary>
    [Description("日期时间列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DateTimeColumn : BoundField
    {
        /// <summary>
        /// (构造函数)日期时间列
        /// </summary>
        public DateTimeColumn()
        {

            this.MiType = "datecolumn";

            this.DefaultEditorType = "datefield";

            this.Width = 160;
        }

        /// <summary>
        /// 默认格式化
        /// </summary>
        const string DEFAULT_FORMAT = "Y-m-d H:i:s";

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

        protected virtual string CreateEditor()
        {
            StringBuilder sb = new StringBuilder();


            if (this.EditorMode == Mini2.EditorMode.Auto)
            {
                sb.Append("editor:{");

                sb.Append("showTime: true, ");
                sb.Append("format: 'Y-m-d H:i:s', ");

                sb.AppendFormat("tag:'{0}',", MiniHelper.GetItemJson(this.Tag));
                sb.AppendFormat("xtype:'{0}'", this.DefaultEditorType);
                sb.Append("}, ");
            }

            return sb.ToString();
        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);
            
            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            sb.Append("showTime:true, ");

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

            sb.Append(CreateEditor());

            if (this.ItemAlign != Mini.CellAlign.Left)
            {
                sb.AppendFormat("align:'{0}',", this.ItemAlign.ToString().ToLower());
            }

            sb.AppendFormat("width:{0} }}", this.Width);

            return sb.ToString();
        }

    }
}
