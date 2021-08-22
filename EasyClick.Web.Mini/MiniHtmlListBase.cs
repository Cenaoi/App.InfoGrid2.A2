using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    
    [AspNetHostingPermission(SecurityAction.Demand,Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"),PersistChildren(false)]
    [DefaultProperty("Value")]
    public abstract class MiniHtmlListBase : Control, IAttributeAccessor, IClientIDMode
    {
        protected internal HtmlTextWriterTag HtmlTag = HtmlTextWriterTag.Select;

        ListItemCollection m_Items;


        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        [DefaultValue(ClientIDMode.AutoID)]
        public ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

        #region 空值

        /// <summary>
        /// 显示空选项
        /// </summary>
        bool m_ShowEmptyItem = false;
        string m_EmptyItemText = string.Empty;
        string m_EmptyItemValue = "N/A";

        /// <summary>
        /// 显示空选项
        /// </summary>
        [DefaultValue(false)]
        public bool ShowEmptyItem
        {
            get { return m_ShowEmptyItem; }
            set { m_ShowEmptyItem = value; }
        }

        [DefaultValue("N/A")]
        public string EmptyItemValue
        {
            get { return m_EmptyItemValue; }
            set { m_EmptyItemValue = value; }
        }

        [DefaultValue("")]
        public string EmptyItemText
        {
            get { return m_EmptyItemText; }
            set { m_EmptyItemText = value; }
        }


        #endregion

        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }


        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false) ]
        public virtual ListItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    if (this.DesignMode)
                    {
                        m_Items = new ListItemCollection();
                    }
                    else
                    {
                        m_Items = new ListItemCollection(this);
                    }
                }

                return m_Items;
            }

        }

        protected virtual void RenderItems(HtmlTextWriter writer)
        {

            foreach (ListItem item in this.Items)
            {
                if (string.IsNullOrEmpty(item.Value) && !string.IsNullOrEmpty(item.Text))
                {
                    item.Value = item.Text;
                }
                else if (!string.IsNullOrEmpty(item.Value) && string.IsNullOrEmpty(item.Text))
                {
                    item.Text = item.Value;
                }
                


                if (item.Value == this.Value)
                {
                    writer.Write("<option value='{0}' selected='selected'>{1}</option>", item.Value, item.Text);
                }
                else
                {
                    writer.Write("<option value='{0}'>{1}</option>", item.Value, item.Text);
                }

            }
        }

        protected virtual void RenderHtml(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", GetClientID());

            writer.AddAttribute("name", this.ClientID);

            if (!string.IsNullOrEmpty(this.Value))
            {
                writer.AddAttribute("value", this.Value);

                writer.AddAttribute("srcValue", this.Value);
            }

            foreach (string key in m_HtmlAttrs.Keys)
            {
                if (key == "id" || key == "type" || key == "name" || key == "value")
                {
                    continue;
                }

                MiniHtmlAttr attr = m_HtmlAttrs[key];

                writer.AddAttribute(attr.Key, attr.Value);
            }


            writer.RenderBeginTag(this.HtmlTag);

            if (this.ShowEmptyItem)
            {
                writer.AddAttribute("value", this.EmptyItemValue);

                if (this.EmptyItemValue == this.Value)
                {
                    writer.AddAttribute("selected", "selected");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.WriteEncodedText(this.EmptyItemText);
                writer.RenderEndTag();
            }

            RenderItems(writer);

            writer.RenderEndTag();
        }
        


        protected override void Render(HtmlTextWriter writer)
        {
            RenderHtml(writer);
        }

        string m_Value = null;

        string m_SetValueScript = "$('#{0}').val('{1}')";

        [DefaultValue("$('#{0}').val('{1}')")]
        public string SetValueScript
        {
            get { return m_SetValueScript; }
            set { m_SetValueScript = value; }
        }

        [DefaultValue("")]
        public virtual string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;

                if (!this.DesignMode && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    if (value == null)
                    {
                        MiniScript.Add(this.SetValueScript, this.ClientID, "");
                    }
                    else
                    {
                        string txt = value.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n").Replace("\t", @"\t");

                        MiniScript.Add(this.SetValueScript, this.ClientID, txt);
                    }
                }

            }
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
}
