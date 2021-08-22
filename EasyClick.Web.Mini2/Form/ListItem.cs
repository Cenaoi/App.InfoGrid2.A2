using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// 列表条目
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("Value={Value},Text={Text}")]
    public sealed class ListItem
    {
        /// <summary>
        /// 列表条目
        /// </summary>
        public ListItem()
        {
            this.Value = string.Empty;
            this.Text = string.Empty;
        }

        /// <summary>
        /// 列表条目
        /// </summary>
        /// <param name="value"></param>
        public ListItem(string value)
        {
            this.Value = value;
            this.Text = value;
        }

        /// <summary>
        /// 列表条目
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public ListItem(string value, string text)
        {
            this.Value = value;
            this.Text = text;
        }

        /// <summary>
        /// 列表条目
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public ListItem(int value, string text)
        {
            this.Value = value.ToString();
            this.Text = text;
        }


        [DefaultValue("")]
        public string Value { get; set; }

        [DefaultValue("")]
        public string Text { get; set; }

        /// <summary>
        /// Text 属性的扩展
        /// </summary>
        [DefaultValue("")]
        public string TextEx { get; set; }


        /// <summary>
        /// 最小宽度
        /// </summary>
        public string MinWidth { get; set; }

        /// <summary>
        /// 最大宽度
        /// </summary>
        public string MaxWidth { get; set; }


        /// <summary>
        /// 获取 json 格式
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            string valueJson = StringUtil.NoBlank(this.Text, this.Value);

            StringBuilder sb = new StringBuilder();

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);
            st.RetractBengin("{");
            {
                st.WriteParam("value", this.Value);
                st.WriteParam("text", valueJson);

                st.WriteParam("textEx",this.TextEx);
                st.WriteParam("minWidth", this.MinWidth);
                st.WriteParam("maxWidth", this.MaxWidth);
            }
            st.RetractEnd("}");
                       

            return sb.ToString();
        }
    }
}
