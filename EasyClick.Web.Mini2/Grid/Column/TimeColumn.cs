using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 时间列
    /// </summary>
    [Description("日期列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TimeColumn :BoundField
    {
        /// <summary>
        /// (构造函数)时间列
        /// </summary>
        public TimeColumn()
        {

            this.MiType = "timecolumn";

            this.DefaultEditorType = "timefield";

            this.Width = 160;
        }

        /// <summary>
        /// 默认格式化
        /// </summary>
        const string DEFAULT_FORMAT = "H:i";

        string m_Format;

        /// <summary>
        /// 格式化显示
        /// </summary>
        [Description("格式化显示.默认:H:i")]
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
                sb.Append("format: 'H:i', ");

                sb.AppendFormat("tag:'{0}',", Mini.MiniHelper.GetItemJson(this.Tag));
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
