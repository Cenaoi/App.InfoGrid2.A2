using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:HtmlElement  runat=\"server\" />")]
    [ParseChildren(true, "Value"), PersistChildren(false)]
    public class HtmlElement:Control, IAttributeAccessor
    {
        

        
        string m_SetValueScript = "$(\"#{0}\").html(\"{1}\")";
        string m_Value;

        string m_HtmlTag = "div";

        public HtmlElement()
        {

        }

        public string HtmlTag
        {
            get { return m_HtmlTag; }
            set { m_HtmlTag = value; }
        }

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
        
        protected override void Render(HtmlTextWriter writer)
        {
            RenderMiniControl(writer);
        }

        protected virtual void RenderMiniControl(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", GetClientID() );

            writer.AddAttribute("name", this.ClientID);


            foreach (string key in m_HtmlAttrs.Keys)
            {
                if (key == "id" || key == "name")
                {
                    continue;
                }

                MiniHtmlAttr attr = m_HtmlAttrs[key];
                string v = attr.Value;

                if (string.IsNullOrEmpty(v))
                {
                    continue;
                }

                writer.AddAttribute(attr.Key, v);
            }

            writer.RenderBeginTag(this.HtmlTag);

            if (!string.IsNullOrEmpty(this.Value))
            {
                writer.Write(this.Value);
            }

            writer.RenderEndTag();
        }


        [DefaultValue("$(\"#{0}\").val(\"{1}\")")]
        public virtual string SetValueScript
        {
            get { return m_SetValueScript; }
            set { m_SetValueScript = value; }
        }



        /// <summary>
        /// (JScript) 值
        /// </summary>
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty), MergableProperty(false)]
        public virtual string Value
        {
            get { return m_Value; }
            set
            {
                if (!this.DesignMode && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    //MiniScript.Add("KE.util.setFullHtml('{0}', '{1}')", this.ClientID, value);
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

                m_Value = value;
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
