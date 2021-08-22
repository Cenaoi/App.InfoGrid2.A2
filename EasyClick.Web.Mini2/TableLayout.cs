using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 表单版面
    /// </summary>
    public class TableLayout:Panel
    {
        /// <summary>
        /// (构造函数)表单版面
        /// </summary>
        public TableLayout()
            : base()
        {
            this.Layout = LayoutStyle.Table;

            //this.ItemClass = "mi-anchor-form-item";
        }

        TextAlign m_LabelAlign = TextAlign.Left;

        int m_LabelWidth = 0;



        /// <summary>
        /// 布局样式
        /// </summary>
        [DefaultValue(LayoutStyle.Table)]
        [Description("布局样式")]
        [Browsable(false)]
        public new LayoutStyle Layout
        {
            get { return base.Layout; }
            set { base.Layout = value; }
        }

        /// <summary>
        /// 标签水平对齐方向
        /// </summary>
        [DefaultValue(TextAlign.Left)]
        [Category("Labelabel")]
        [Description("标签水平对齐方向")]
        public TextAlign LabelAlign
        {
            get { return m_LabelAlign; }
            set { m_LabelAlign = value; }
        }

        /// <summary>
        /// 标签宽度
        /// </summary>
        [DefaultValue(0)]
        [Category("Labelabel")]
        [Description("标签宽度")]
        public int LabelWidth
        {
            get { return m_LabelWidth; }
            set { m_LabelWidth = value; }
        }

        /// <summary>
        /// 填充脚本
        /// </summary>
        /// <param name="sb"></param>
        protected override void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

            st.WriteLine("  var field = Mini2.create('" + this.JsNamespace + "', {");

            st.WriteParam("id", this.ID);
            st.WriteParam( "clientId", clientId);


            FullScript_Layout(st);
            FullScript_ItemMargin(st);
            FullScript_Padding(st);


            st.WriteParam("width", this.Width.ToString());
            st.WriteParam("height", this.Height.ToString()); ;

            st.WriteParam("scroll", this.Scroll, ScrollBars.Auto, TextTransform.Lower);
            st.WriteParam("ui", this.Ui, UiStyle.Default, TextTransform.Lower);
            st.WriteParam("dock", this.Dock, TextTransform.Lower);
            st.WriteParam("region", this.Region, TextTransform.Lower);

            st.WriteParam("contentEl", "#" + clientId);

            st.WriteLine();
            st.WriteLine("});");

            st.WriteLine("  field.render();");

            st.WriteLine("  window.{0} = field;\n", clientId);


        }
    }
}
