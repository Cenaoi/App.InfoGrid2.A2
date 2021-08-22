using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    public class Component:Control
    {

        string m_InReady = "Mini2.ui.form.field.Text";

        string m_JsNamaspace = "Mini2.ui.form.field.Text";

        DockStyle m_Dock = DockStyle.None;

        bool m_Visible = true;



        int m_MinWidth = 0;
        int m_MinHeight = 0;

        /// <summary>
        /// 最大高度
        /// </summary>
        int m_MaxHeight = 0;

        /// <summary>
        /// 最大宽度
        /// </summary>
        int m_MaxWidth = 0;

        /// <summary>
        /// 权限编码
        /// </summary>
        public string SecFunCode { get; set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        [Description("最小宽度")]
        [DefaultValue(0)]
        public int MinWidth
        {
            get { return m_MinWidth; }
            set { m_MinWidth = value; }
        }

        /// <summary>
        /// 最小高度
        /// </summary>
        [Description("最小高度")]
        [DefaultValue(0)]
        public int MinHeight
        {
            get { return m_MinHeight; }
            set { m_MinHeight = value; }
        }



        /// <summary>
        /// 最大高度
        /// </summary>
        [Description("最大高度")]
        [DefaultValue(0)]
        public int MaxHeight
        {
            get { return m_MaxHeight; }
            set { m_MaxHeight = value; }
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [Description("最大宽度")]
        [DefaultValue(0)]
        public int MaxWidth
        {
            get { return m_MaxWidth; }
            set { m_MaxWidth = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        [DefaultValue(DockStyle.Top)]
        public DockStyle Dock
        {
            get { return m_Dock; }
            set { m_Dock = value; }
        }

        protected string JsNamespace
        {
            get { return m_JsNamaspace; }
            set { m_JsNamaspace = value; }
        }

        protected string InReady
        {
            get { return m_InReady; }
            set { m_InReady = value; }
        }

        /// <summary>
        /// 客户端脚本模式
        /// </summary>
        [Browsable(false)]
        [DefaultValue(JavascriptMode.Auto)]
        [Description("客户端脚本模式")]
        public JavascriptMode JavascriptMode
        {
            get
            {
                JavascriptMode jsMode = JavascriptMode.Auto;

                HttpContext context = HttpContext.Current;

                if (context != null && context.Items.Contains("JS_MODE"))
                {
                    string jsModeStr = (string)context.Items["JS_MODE"];

                    jsMode = EnumUtil.Parse<JavascriptMode>(jsModeStr, true);
                }

                return jsMode;
            }
        }

        protected void BeginScript(StringBuilder sb)
        {
            sb.AppendLine("<script type=\"text/javascript\">");
        }

        protected void BeginReady(StringBuilder sb)
        {

            JavascriptMode jsMode = this.JavascriptMode;

            //if (jsMode == JavascriptMode.MInJs2)
            //{
            //    sb.Append("In.ready('" + m_InReady + "', function () {").AppendLine();
            //}
            //else
            //{
                sb.AppendLine("$(document).ready(function(){");
            //}
        }

        protected void EndReady(StringBuilder sb)
        {
            sb.AppendLine("});");
        }


        protected void EndScript(StringBuilder sb)
        {
            sb.AppendLine("</script>");
        }

        public override bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                m_Visible = value;

                EasyClick.Web.Mini.MiniHelper.EvalFormat("if({0} && {0}.setVisible){{ {0}.setVisible({1}); }}", this.ClientID, value.ToString().ToLower());
            }
        }

        #region Render_JsParam

        protected void JsParam(StringBuilder sb, string mame, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }


        protected void JsParam(StringBuilder sb, string mame, string[] value)
        {
            if(value == null)
            {
                sb.AppendFormat("    {0}: {1},", mame, "null").AppendLine();
            }
            else
            {
                StringBuilder itemSB = new StringBuilder();
                itemSB.Append("[");

                for (int i = 0; i < value.Length; i++)
                {
                    if(i > 0) { itemSB.Append(", "); }

                    itemSB.Append("'");
                    itemSB.Append(JsonUtil.ToJson(value[i], JsonQuotationMark.SingleQuotes));
                    itemSB.Append("'");
                }

                itemSB.Append("]");

                sb.AppendFormat("    {0}: {1},", mame, itemSB.ToString()).AppendLine();
            }

        }

        protected void JsParam(StringBuilder sb, string mame, int value)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string mame, decimal value, decimal defaultValue)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string mame, decimal value)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }


        protected void JsParam(StringBuilder sb, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            sb.AppendFormat("    {0}: '{1}',", name, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, string value, string defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: '{1}',", name, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, bool value, bool defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", name, value.ToString().ToLower()).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, Enum value, Enum defaultValue)
        {
            JsParam(sb, name, value, defaultValue, TextTransform.None);
        }

        protected void JsParam(StringBuilder sb, string name, Enum value, TextTransform textTransform)
        {
            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            sb.AppendFormat("    {0}: '{1}',", name, valueStr).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, Enum value, Enum defaultValue, TextTransform textTransform)
        {
            if (value == defaultValue)
            {
                return;
            }

            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            sb.AppendFormat("    {0}: '{1}',", name, valueStr).AppendLine();
        }

        #endregion


    }
}
