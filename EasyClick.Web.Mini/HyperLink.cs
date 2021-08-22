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
    /// 导航链接
    /// </summary>
    [DefaultProperty("Text")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true,"Text"), PersistChildren(true)]
    public class HyperLink : Control, IAttributeAccessor, IClientIDMode
    {
        #region ClientIDMode 

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        [DefaultValue(ClientIDMode.AutoID)]
        public ClientIDMode ClientIDMode
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

        #endregion

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

        string m_Text;
        string m_Url;

        string m_Target;

        string m_SetValueScript = "$(\"#{0}\").attr(\"href\",\"{1}\")";

        /// <summary>
        /// (JScript) 值
        /// </summary>
        [DefaultValue("")]
        [MergableProperty(false)]
        public virtual string Target
        {
            get { return m_Target; }
            set
            {
                if (!this.DesignMode && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    if (value == null)
                    {
                        MiniScript.Add(m_SetValueScript, this.ClientID, "");
                    }
                    else
                    {
                        string txt = value.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n").Replace("\t", @"\t");

                        MiniScript.Add(m_SetValueScript, this.ClientID, txt);
                    }
                }

                m_Target = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Browsable(false)]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 导航地址
        /// </summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderMiniControl(writer);
        }

        protected virtual void RenderMiniControl(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", GetClientID());
            //writer.AddAttribute("name", this.ClientID);
            writer.AddAttribute("href", this.Url);

            if (!string.IsNullOrEmpty(m_Target))
            {
                writer.AddAttribute("target", m_Target);
            }
            
            foreach (string key in m_HtmlAttrs.Keys)
            {
                if (key == "id" )
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

                MiniHtmlAttr attr = m_HtmlAttrs[key];
                string v = attr.Value;

                writer.AddAttribute(attr.Key, v);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(m_Text);
            writer.RenderEndTag();
        }


    }
}
