using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items")]
    [DefaultProperty("Value")]
    [Description("根据值内容显示的标签")]
    public class LabelSwitch : MiniHtmlListBase
    {

        public LabelSwitch()
            : base()
        {
            this.SetValueScript = "$(\"#{0}\").text(\"{1}\");";
        }


        private string GetText(string value)
        {
            foreach (var item in this.Items)
            {
                if (item.Value == value)
                {
                    return item.Text;
                }
            }

            return string.Empty;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string text = GetText(this.Value);

            writer.AddAttribute("id", GetClientID());

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
            

            writer.RenderBeginTag(HtmlTextWriterTag.Label);

            writer.WriteEncodedText(text);

            writer.RenderEndTag();
            

        }

        string m_Value;

        public override string Value
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
                        MiniScript.Add(this.SetValueScript, this.GetClientID(), "");
                    }
                    else
                    {
                        string text = GetText(value);

                        string txt = text.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n").Replace("\t", @"\t");

                        MiniScript.Add(this.SetValueScript, this.GetClientID(), txt);
                    }
                }
            }
        }

    }
}
