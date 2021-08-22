using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 多行文本的列
    /// </summary>
    public class TextAreaColumn:BoundField
    {

        /// <summary>
        /// (构造函数)多行文本列
        /// </summary>
        public TextAreaColumn()
        {
            this.MiType = "textareacolumn";
            this.DefaultEditorType = "textarea";

            this.Width = 200;


        }


        /// <summary>
        /// (构造函数)多行文本列
        /// </summary>
        /// <param name="dataField"></param>
        public TextAreaColumn(string dataField)
        {

            this.MiType = "textareacolumn";
            this.DefaultEditorType = "textarea";

            this.Width = 200;

            this.DataField = dataField;
        }

        /// <summary>
        /// (构造函数)多行文本列
        /// </summary>
        /// <param name="dataField"></param>
        /// <param name="headerText"></param>
        public TextAreaColumn(string dataField, string headerText)
        {
            this.MiType = "textareacolumn";
            this.DefaultEditorType = "textarea";

            this.Width = 200;


            this.DataField = dataField;
            this.HeaderText = headerText;
        }




        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);


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

            if (!string.IsNullOrEmpty(this.NotDisplayValue))
            {
                sb.AppendFormat("notDisplayValue:'{0}',", this.NotDisplayValue);
            }

            if (!string.IsNullOrEmpty(this.SummaryType))
            {
                sb.AppendFormat("summaryType:'{0}',", this.SummaryType);
            }

            if (!string.IsNullOrEmpty(this.SummaryFormat))
            {
                sb.AppendFormat("summaryFormat: '{0}',", this.SummaryFormat);
            }

            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }


            sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());

            sb.Append(CreateEditor());

            sb.AppendFormat("align:'{0}'}}", this.ItemAlign.ToString().ToLower());

            return sb.ToString();
        }
    }
}
