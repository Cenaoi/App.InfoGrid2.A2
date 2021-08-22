using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Security.Permissions;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// Boolean 字段显示
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class BoolColumn: BoundField
    {
        /// <summary>
        /// Boolean 字段显示(构造函数)
        /// </summary>
        public BoolColumn()
        {
            base.MiType = "booleancolumn";
            this.EditorMode = Mini2.EditorMode.None;
        }

        string m_TrueText = "true";

        string m_FalseText = "false";

        string m_UndefinedText = "&#160;";


        [DefaultValue("true")]
        public string TrueText
        {
            get { return m_TrueText; }
            set { m_TrueText = value; }
        }


        [DefaultValue("false")]
        public string FalseText
        {
            get { return m_FalseText; }
            set { m_FalseText = value; }
        }

        /// <summary>
        /// 不明确
        /// </summary>
        [DefaultValue("&#160;")]
        public string UndefinedText
        {
            get { return m_UndefinedText; }
            set { m_UndefinedText = value; }
        }

        /// <summary>
        /// 创建 Html 模板
        /// </summary>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            if (m_TrueText != "true")
            {
                sb.AppendFormat("trueText:'{0}', ", m_TrueText);
            }



            CreateStyleRule(sb);

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }

            if (m_FalseText != "false")
            {
                sb.AppendFormat("falseText:'{0}', ", m_FalseText);
            }

            if (m_UndefinedText != "&#160;")
            {
                sb.AppendFormat("undefinedText:'{0}', ", m_UndefinedText);
            }

            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SummaryType))
            {
                sb.AppendFormat("summaryType:'{0}',", this.SummaryType);
            }
            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }
            sb.Append(CreateEditor());

            sb.AppendFormat("align:'{0}'}}", this.ItemAlign.ToString().ToLower());

            return sb.ToString();
        }
    }
}
