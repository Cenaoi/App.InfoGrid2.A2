using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 用于在页面上显示文本。
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Literal : Control, IAttributeAccessor
    {

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }


        string m_Value;

        /// <summary>
        /// 规定要显示的文本。
        /// </summary>
        [DefaultValue("")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(m_Value);

        }
    }
}
