using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 复选框
    /// </summary>
    [Description("复选框")]
    [DefaultProperty("Checked")]
    [ToolboxData("<{0}:CheckBox  runat=\"server\" Value=\"\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class CheckBox:MiniHtmlBase
    {
        /// <summary>
        /// 复选框的构造方法
        /// </summary>
        public CheckBox()
        {
            this.Type = "checkbox";
        }

        bool m_Checked = false;

        string m_Text = string.Empty;

        string m_TrueValue;

        string m_FalseValue;

        string m_Value;

        [DefaultValue("")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        [DefaultValue("")]
        public string TrueValue
        {
            get { return m_TrueValue; }
            set { m_TrueValue = value; }
        }

        [DefaultValue("")]
        public string FalseValue
        {
            get { return m_FalseValue; }
            set { m_FalseValue = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        [DefaultValue("")]
        public override string Value
        {
            get
            {
                if (m_Checked)
                {
                    return m_TrueValue;
                }
                else
                {
                    return m_FalseValue;
                }

            }
            set
            {
                if (value == m_TrueValue)
                {
                    this.Checked = true;
                }
                else if(value == m_FalseValue)
                {
                    this.Checked = false;
                }


            }
        }

        /// <summary>
        /// (JScript) 被选中
        /// </summary>
        [DefaultValue(false)]
        public bool Checked
        {
            get { return m_Checked; }
            set 
            {
                if (!this.DesignMode && m_Checked != value && !MiniScriptManager.ClientScript.ReadOnly)
                {
                    MiniScript.Add("$('#{0}').attr('checked', {1})", GetClientID(), value.ToString().ToLower());
                }

                m_Checked = value; 
            }
        }

        

        //protected override void LoadPostData()
        //{
        //    string value = this.Page.Request.Form[this.ID + "_Checked"];

        //    bool.TryParse(value, out m_Checked);
        //}

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Checked)
            {
                writer.AddAttribute("checked", "checked");
            }

            base.Render(writer);

            if (!string.IsNullOrEmpty(m_Text))
            {
                writer.AddAttribute("for", GetClientID());
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.WriteEncodedText(this.Text);
                writer.RenderEndTag();
            }
        }

        
    }
}
