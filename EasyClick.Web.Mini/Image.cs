using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [Description("图片框")]
    [DefaultProperty("Src")]
    [ToolboxData("<{0}:Image runat=\"server\" Src=\"\" />")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true), PersistChildren(false)]
    public class Image:Control,IMiniControl,IAttributeAccessor
    {
        public Image()
        {
            
        }

        string m_Src;

        string m_SetValueScript = "$(\"#{0}\").attr(\"src\",\"{1}\");";

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        [DefaultValue(ClientIDMode.AutoID)]
        public new ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

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

        [DefaultValue("")]
        public string Src
        {
            get { return m_Src; }
            set
            {
                m_Src = value;

                if (this.DesignMode) { return; }

                if ( !MiniScriptManager.ClientScript.ReadOnly)
                {
                    MiniScript.Add(m_SetValueScript, GetClientID(), value);
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", GetClientID());

            //writer.AddAttribute("name", this.ClientID);

            writer.AddAttribute("border", "0");

            if (!string.IsNullOrEmpty(this.Src))
            {
                writer.AddAttribute("src", this.Src);
            }

            RenderAttrs(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        public void LoadPostData()
        {
            
        }

        private void RenderAttrs(HtmlTextWriter writer)
        {

            if (m_HtmlAttrs == null)
            {
                return;
            }

            string key;

            foreach (MiniHtmlAttr attr in m_HtmlAttrs)
            {
                key = attr.Key;

                if (key == "id" || key == "type" || key == "name" || key == "value")
                {
                    continue;
                }


                //if (MiniConfiguration.ServerAttrTags != null)
                //{
                //    string[] serTags = MiniConfiguration.ServerAttrTags;

                //    bool exist = false;

                //    for (int i = 0; i < serTags.Length; i++)
                //    {
                //        if (key.Equals( serTags[i], StringComparison.OrdinalIgnoreCase))
                //        {
                //            exist = true;
                //            break;
                //        }
                //    }

                //    if (exist)
                //    {
                //        continue;
                //    }
                //}

                //MiniHtmlAttr attr = m_HtmlAttrs[key];
                string v = attr.Value;

                writer.AddAttribute(attr.Key, v);
            }
        }

        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs;

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
