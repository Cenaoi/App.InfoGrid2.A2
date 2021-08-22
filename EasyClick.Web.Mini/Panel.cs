using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(false), PersistChildren(true)]
    public class Panel:Control, IAttributeAccessor
    {
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public override ControlCollection Controls
        {
            get
            {
                return base.Controls;
            }
        }

        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs ;

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            if (m_HtmlAttrs == null) { return false; }

            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            if (m_HtmlAttrs == null) { return null; }

            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            if (m_HtmlAttrs == null) { m_HtmlAttrs = new MiniHtmlAttrCollection(); }

            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion
    }
}
