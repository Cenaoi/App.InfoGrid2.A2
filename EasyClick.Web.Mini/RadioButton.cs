using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 单选项
    /// </summary>
    [ToolboxData("<{0}:RadioButton  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class RadioButton:MiniHtmlBase
    {
        /// <summary>
        /// 单选项
        /// </summary>
        public RadioButton()
        {
            this.Type = "radio";
        }

        string m_Group;

        bool m_Checked = false;

        [DefaultValue(false)]
        public bool Checked
        {
            get { return m_Checked; }
            set 
            {
                if (m_Checked != value && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    MiniScript.Add("$('#{0}').val('{1}')", this.GetClientID(), value);

                }

                m_Checked = value;
            }
        }

        /// <summary>
        /// 组名
        /// </summary>
        [DefaultValue("")]
        public string Group
        {
            get { return m_Group; }
            set { m_Group = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", this.GetClientID());

            if (!string.IsNullOrEmpty(this.Type))
            {
                writer.AddAttribute("type", this.Type);
            }

            if (this.Checked)
            {
                writer.AddAttribute("checked", "checked"); 
            }



            writer.AddAttribute("name", m_Group);

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

                writer.AddAttribute(attr.Key, attr.Value);
            }


            writer.RenderBeginTag(this.HtmlTag);
            writer.RenderEndTag();
        }
    }
}
