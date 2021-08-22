using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System;
using System.Text;
using System.IO;

namespace EasyClick.Web.Mini
{
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class MiniHtmlBase : Control, IAttributeAccessor, IClientIDMode
    {
        protected internal HtmlTextWriterTag HtmlTag = HtmlTextWriterTag.Input;

        string m_Type = "text";

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

        [Browsable(false)]
        protected internal string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                if (!App.Register.RegHelp.IsRegister()) {writer.Write("调用限制：未注册"); return;}
            }
            
            RenderMiniControl(writer);
        }

        public void FullScript(StringBuilder sb)
        {
            StringWriter sw = new StringWriter(sb);

            HtmlTextWriter writer = new HtmlTextWriter(sw);

            RenderMiniControl(writer);

            writer.Dispose();
            sw.Dispose();
        }

        protected virtual void RenderMiniControl(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", GetClientID());


            if (!string.IsNullOrEmpty(this.Type))
            {
                writer.AddAttribute("type", this.Type);
            }

            writer.AddAttribute("name", this.ClientID);

            if (!string.IsNullOrEmpty(this.Value))
            {
                writer.AddAttribute("value", this.Value);
            }

            foreach (string key in m_HtmlAttrs.Keys)
            {
                if (key == "id" || key == "type" || key == "name" || key == "value")
                {
                    continue;
                }


                MiniHtmlAttr attr = m_HtmlAttrs[key];
                string v = attr.Value;
                
                writer.AddAttribute(attr.Key, v);
            }

            writer.RenderBeginTag(this.HtmlTag);
            writer.RenderEndTag();
        }

        string m_SetValueScript = "$('#{0}').val('{1}')";

        [DefaultValue("$('#{0}').val('{1}')")]
        public string SetValueScript
        {
            get { return m_SetValueScript; }
            set { m_SetValueScript = value; }
        }

        string m_Value = string.Empty;

        /// <summary>
        /// (JScript) 值
        /// </summary>
        [DefaultValue("")]
        public virtual string Value
        {
            get { return m_Value; }
            set 
            {
                if (!this.DesignMode && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    if (value == null)
                    {
                        MiniScript.Add(m_SetValueScript, GetClientID(), string.Empty);
                    }
                    else
                    {
                        string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(value);

                        MiniScript.Add(m_SetValueScript, GetClientID(), txt);
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
