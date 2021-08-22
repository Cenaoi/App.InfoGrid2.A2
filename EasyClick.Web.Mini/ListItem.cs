using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 列表条目
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("Value={Value},Text={Text},")]
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


    }

}
