using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 内容菜单栏
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items")]
    [Description("内容菜单栏")]
    public class ContextMenuStrip : Control,IAttributeAccessor
    {
        MenuItemCollection m_Items;

        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public MenuItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new MenuItemCollection();
                }

                return m_Items;
            }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);

            writer.AddAttribute("id", this.ID);
            writer.AddAttribute("class", "Mini_menu");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                RenderItems(writer, m_Items);                
            }
            writer.RenderEndTag();
        }

        private void RenderItems(HtmlTextWriter writer, MenuItemCollection items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (MenuItem item in items)
            {
                RenderItem(writer, item);
            }

            writer.RenderEndTag();
        }

        private void RenderItem_Href(HtmlTextWriter writer, MenuItem item)
        {
            writer.AddAttribute("href", "#");

            if (!string.IsNullOrEmpty(item.Command))
            {
                writer.AddAttribute("command", item.Command);
            }

            if (!string.IsNullOrEmpty(item.OnClick))
            {
                writer.AddAttribute("onclick", item.OnClick);
            }


            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(item.Text);
            writer.RenderEndTag();
        }

        private void RenderItem_Separator(HtmlTextWriter writer, MenuItem item)
        {
            writer.AddAttribute("class", "Separator");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }

        private void RenderItem(HtmlTextWriter writer, MenuItem item)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            {
                if ("-" == item.Text)
                {
                    RenderItem_Separator(writer, item);
                }
                else
                {
                    RenderItem_Href(writer, item);

                    if (item.HasItems())
                    {
                        RenderItems(writer, item.Items);
                    }
                }

            }
            writer.RenderEndTag();
        }

        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion
    }


    /// <summary>
    /// 菜单条目
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("ID={ID}, Text={Text}")]
    public class MenuItem
    {
        string m_ID;
        string m_Text;
        string m_Icon;
        string m_OnClick;
        string m_Command;

        MenuItemCollection m_Items;

        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public MenuItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new MenuItemCollection();
                }

                return m_Items;
            }
        }

        public bool HasItems()
        {
            return !(m_Items == null || m_Items.Count == 0);
        }

        [DefaultValue("")]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        [DefaultValue("")]
        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        [DefaultValue("")]
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        [DefaultValue("")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }
    }

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class MenuItemCollection:List<MenuItem>
    {

    }
}
