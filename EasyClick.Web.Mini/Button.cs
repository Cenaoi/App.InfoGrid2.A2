using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 按钮
    /// </summary>
    [Description("按钮")]
    [DefaultProperty("Value")]
    [ToolboxData("<{0}:Button runat=\"server\" >按钮</{0}:Button>")]
    [ParseChildren(true, "InnerHtml"), PersistChildren(false)]
    public class Button:MiniHtmlBase
    {
        /// <summary>
        /// 按钮
        /// </summary>
        public Button()
        {
            HtmlTag = HtmlTextWriterTag.Button;
            this.Type = "button";
        }

        string m_ReturnFormat = "script";
        string m_Command = "Submit";
        string m_CommandParam;

        bool m_Valid = true;

        ButtonTypes m_ButtonType = ButtonTypes.Button;

        string m_InnerHtml;

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Browsable(false)]
        public string InnerHtml
        {
            get { return m_InnerHtml; }
            set { m_InnerHtml = value; }
        }

        /// <summary>
        /// 获取或设置提交时候，是否需要验证,默认 true
        /// </summary>
        [DefaultValue(true)]
        [Description("获取或设置提交时候，是否需要验证,默认 true")]
        public bool Valid
        {
            get { return m_Valid; }
            set { m_Valid = value; }
        }

        [DefaultValue("script")]
        public string ReturnFormat
        {
            get { return m_ReturnFormat; }
            set { m_ReturnFormat = value; }
        }

        /// <summary>
        /// 按钮类型
        /// </summary>
        [Description("按钮类型")]
        [DefaultValue(ButtonTypes.Button)]
        public ButtonTypes ButtonType
        {
            get { return m_ButtonType; }
            set
            {
                m_ButtonType = value;
            }
        }

        /// <summary>
        /// 配合 Command 属性，服务器端事件参数
        /// </summary>
        [Description("配合 Command 属性，服务器端事件参数")]
        [DefaultValue("")]
        public string CommandParam
        {
            get { return m_CommandParam; }
            set { m_CommandParam = value; }
        }

        /// <summary>
        /// 服务器端事件名称
        /// </summary>
        [Description("服务器端事件名称")]
        [DefaultValue("")]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        protected override void RenderMiniControl(HtmlTextWriter writer)
        {
            EnsureChildControls();

            writer.AddAttribute("id", GetClientID());

            if (!string.IsNullOrEmpty(this.Type))
            {
                writer.AddAttribute("type", this.Type);
            }

            writer.AddAttribute("name", this.ClientID);

            //if (!string.IsNullOrEmpty(this.Value))
            //{
            //    writer.AddAttribute("value", this.Value);
            //}

            
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

            if (!string.IsNullOrEmpty(this.Value))
            {
                writer.Write(this.Value);
            }
            else
            {
                writer.Write(m_InnerHtml);
            }

            writer.RenderEndTag();
        }


        protected override void CreateChildControls()
        {
            this.Type = m_ButtonType.ToString().ToLower();

            string onclick = GetAttribute("onclick");

            if (m_ButtonType != ButtonTypes.Reset && string.IsNullOrEmpty(onclick))
            {
                Control widget = this.FindWidget();

                if (widget != null)
                {
                    this.SetAttribute("onclick", string.Format("javascript:{0}.submit(this);return false;", widget.ClientID));
                }
            }

            if (!string.IsNullOrEmpty(m_Command))
            {
                SetAttribute("command", m_Command);
            }

            if (!string.IsNullOrEmpty(m_CommandParam))
            {
                SetAttribute("commandParam", m_CommandParam);
            }

            if (!"script".Equals(m_ReturnFormat))
            {
                SetAttribute("returnFormat", m_ReturnFormat);
            }

            if (!m_Valid)
            {
                SetAttribute("valid", "false");
            }

            base.CreateChildControls();
        }



        private UserControl FindWidget()
        {
            Control con = this.Parent;

            for (int i = 0; i < 9; i++)
            {
                if (con is UserControl)
                {
                    break;
                }

                if (con.Parent == null)
                {
                    break;
                }

                con = con.Parent;
            }

            return con as UserControl;
        }
    }
}
