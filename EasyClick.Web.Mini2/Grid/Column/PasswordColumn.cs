using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 密码列
    /// </summary>
    [Description("密码列")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class PasswordColumn:BoundField
    {
        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        public PasswordColumn()
        {
            this.MiType = "PasswordColumn";
            this.DefaultEditorType = "password";

            this.Width = 120;
        }

        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        public PasswordColumn(string dataField)
            : base(dataField)
        {
            this.MiType = "PasswordColumn";
            this.DefaultEditorType = "password";

            this.Width = 120;
        }

        /// <summary>
        /// (构造函数) 表格列
        /// </summary>
        /// <param name="dataField">字段名</param>
        /// <param name="headerText">表头</param>
        public PasswordColumn(string dataField, string headerText)
        {
            this.MiType = "PasswordColumn";
            this.DefaultEditorType = "password";

            this.Width = 120;
            this.DataField = dataField;
            this.HeaderText = headerText;
        }

        /// <summary>
        /// 密码字符框
        /// </summary>
        public string PasswordChar { get; set; }

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

                st.WriteParam("passwordChar", this.PasswordChar);

                st.WriteParam("dataIndex", this.DataField);
                st.WriteParam("headerText", this.HeaderText);

                st.WriteParam("width", this.Width);

                st.WriteParam("autoTrim", this.AutoTrim, true);

                st.WriteParam("required", this.Required);

                st.WriteParam("tag", this.Tag);

                st.WriteParam("sortable", this.Sortable, true);

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

                st.WriteParam("renderer", this.Renderer);


                CreateEditor(st);


            }
            st.RetractEnd("}");

            string json = st.ToString();

            st.Dispose();


            return json;
        }
    }
}
