using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 按钮比例
    /// </summary>
    public enum ButtonScale
    {
        /// <summary>
        /// 小号按钮
        /// </summary>
        Small,
        /// <summary>
        /// 中等按钮
        /// </summary>
        Medium,
        /// <summary>
        /// 大按钮
        /// </summary>
        Large
    }

    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum ButtonTypes
    {
        /// <summary>
        /// 普通按钮
        /// </summary>
        Button,
        /// <summary>
        /// 复选框按钮
        /// </summary>
        Check,
        /// <summary>
        /// 单选框按钮
        /// </summary>
        Radio
    }

    /// <summary>
    /// 按钮
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class Button : Component
    {
        /// <summary>
        /// 按钮
        /// </summary>
        public Button()
        {
            this.InReady = "Mini2.ui.button.Button";
            this.JsNamespace = "Mini2.ui.button.Button";
        }

        string m_Command;

        string m_CommandParams;

        string m_OnClick;
        string m_Text;

        int m_Width = 0;
        int m_Height = 0;

        string m_Class;

        /// <summary>
        /// 'small', 'medium', 'large'
        /// </summary>
        ButtonScale m_Scale = ButtonScale.Small;// "small";


        /// <summary>
        /// 点击前触发的事件
        /// </summary>
        string m_OnBeforeClick;

        /// <summary>
        /// 点击按钮前触发的询问
        /// </summary>
        string m_BeforeAskText;

        string m_Href;

        string m_HrefTarget = "_blank";

        /// <summary>
        /// 给复选框使用
        /// </summary>
        string m_Value;

        /// <summary>
        /// 按钮尺寸比例
        /// </summary>
        [DefaultValue(ButtonScale.Small)]
        public ButtonScale Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        
        public string MarginAll { get; set; }
        public string MarginRight { get; set; }
        public string MarginLeft { get; set; }
        public string MarginTop { get; set; }
        public string MarginButtom { get; set; }




        /// <summary>
        /// 复选框模式下的按钮样式
        /// </summary>
        [Description("复选框模式下的按钮样式")]
        [DefaultValue("")]
        public string CheckdClass { get; set; }

        /// <summary>
        /// 单选框模式下的按钮样式
        /// </summary>
        [Description("单选框模式下的按钮样式")]
        [DefaultValue("")]
        public string RadioClass { get; set; }


        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 按钮类型
        /// </summary>
        [Description("按钮类型")]
        [DefaultValue(ButtonTypes.Button)]
        public ButtonTypes ButtonType { get; set; } = ButtonTypes.Button;

        /// <summary>
        /// 复选框状态
        /// </summary>
        [Description("复选框状态")]
        [DefaultValue(false)]
        public bool Checked { get; set; }


        /// <summary>
        /// 超链接
        /// </summary>
        [Description("超链接")]
        [DefaultValue("")]
        public string Href
        {
            get { return m_Href; }
            set { m_Href = value; }
        }


        /// <summary>
        /// 超链接
        /// </summary>
        [Description("超链接")]
        [DefaultValue("")]
        public string HrefTarget 
        {
            get
            {
                return m_HrefTarget;
            }
            set
            {
                m_HrefTarget = value;
            }
        }



        /// <summary>
        /// 服务器命令
        /// </summary>
        [Description("服务器命令")]
        [DefaultValue("")]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        /// <summary>
        /// 服务器命令参数
        /// </summary>
        [Description("服务器命令参数")]
        [DefaultValue("")]
        public string CommandParams
        {
            get { return m_CommandParams; }
            set { m_CommandParams = value; }
        }

        /// <summary>
        /// 自定义样式
        /// </summary>
        public string Class
        {
            get { return m_Class; }
            set { m_Class = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 高度
        /// </summary>
        [Description("高度")]
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        public new string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 点击按钮前触发的询问
        /// </summary>
        [DefaultValue("")]
        [Description("点击按钮前触发的询问")]
        public string BeforeAskText
        {
            get { return m_BeforeAskText; }
            set { m_BeforeAskText = value; }
        }

        /// <summary>
        /// 点击前触发的事件
        /// </summary>
        [DefaultValue("")]
        [Description("点击前触发的事件")]
        public string OnBeforeClick
        {
            get { return m_OnBeforeClick; }
            set { m_OnBeforeClick = value; }
        }

        /// <summary>
        /// 客户端点击事件
        /// </summary>
        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private void FullScript_Margin(ScriptTextWriter st)
        {
            if (!StringUtil.IsBlank(this.MarginButtom, this.MarginLeft, this.MarginRight, this.MarginTop))
            {
                st.RenderBengin("margin");

                st.WriteParam("left", this.MarginLeft);
                st.WriteParam("top", this.MarginTop);
                st.WriteParam("right", this.MarginRight);
                st.WriteParam("buttom", this.MarginButtom);

                st.RenderEnd();

            }
            else if (!StringUtil.IsBlank(this.MarginAll))
            {
                st.WriteParam("margin", this.MarginAll);
            }

        }

        private void FullScript(ScriptTextWriter st)
        {
            st.RetractBengin("var component = Mini2.create('Mini2.ui.button.Button', {");
            {
                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", this.ClientID);

                st.WriteParam("buttonType", this.ButtonType, ButtonTypes.Button, TextTransform.Lower);
                st.WriteParam("chekced", this.Checked, false);
                st.WriteParam("group", this.Group);

                st.WriteParam("checkCls", this.CheckdClass);
                st.WriteParam("radioCls", this.RadioClass);

                st.WriteParam("value", this.Value);


                st.WriteParam("text", this.Text);
                st.WriteParam("width", this.Width);
                st.WriteParam("height", this.Height);

                st.WriteParam("minWidth", this.MinWidth);
                st.WriteParam("maxWidth", this.MaxWidth);

                st.WriteParam("minHeight", this.MinHeight);
                st.WriteParam("maxHeight", this.MaxHeight);

                st.WriteParam("scale", this.Scale, ButtonScale.Small, TextTransform.Lower);

                st.WriteParam("href", this.Href);
                st.WriteParam("hrefTarget", this.HrefTarget, "_blank");

                st.WriteParam("secFunCode", this.SecFunCode);   //权限编码

                st.WriteParam("command", this.Command);

                st.WriteParam("secFunCode", this.SecFunCode);

                if (!string.IsNullOrEmpty(this.CommandParams))
                {
                    if (StringUtil.StartsWith(CommandParams, "{") && StringUtil.EndsWith(CommandParams, "}"))
                    {
                        st.WriteFunction("commandParams", this.CommandParams);
                    }
                    else
                    {
                        st.WriteParam("commandParams", this.CommandParams);
                    }
                }

                st.WriteParam("userCls", this.Class);

                FullScript_Margin(st);


                st.WriteParam("dock", this.Dock, TextTransform.Lower);

                st.WriteParam("beforeAskText", this.m_BeforeAskText);

                if (!StringUtil.IsBlank(m_OnClick))
                {
                    string formatOnClick = Format_OnClick(this.OnClick);

                    if (!StringUtil.EndsWith(formatOnClick, ";"))
                    {
                        formatOnClick += ";";
                    }


                    st.WriteFunction("click", $"function(){{ {formatOnClick} }}");

                }

                //点击前触发的事件
                if (!StringUtil.IsBlank(m_OnBeforeClick))
                {
                    st.WriteFunction("beforeClick", $"function(){{ return {m_OnBeforeClick} ;}},");
                }


                st.WriteParam("applyTo", "#" + this.ClientID);
                
            }
            st.RetractEnd("});");
            
            st.WriteCodeLine("component.render();");

            
            st.WriteCodeLine($"window.{this.ClientID} = component;");
            
            st.WriteCodeLine($"Mini2.onwerPage.controls['{this.ID}'] = component;");

        }

        public string Format_OnClick(string code)
        {
            string clickCode;

            if (StringUtil.StartsWith( code,"ser:"))
            {
                code = code.Substring(4);

                int n = code.IndexOf(".");

                int ghL = code.IndexOf("(", n + 1);

                string conName = code.Substring(0, n);
                string methodName = code.Substring(n + 1, ghL - n - 1);

                StringBuilder sb = new StringBuilder();
                sb.Append("widget1.subMethod($('form:first'), {");
                sb.AppendFormat("subName: '{0}', subMethod: '{1}'", conName, methodName);
                sb.Append("});");

                clickCode = sb.ToString();
            }
            else
            {
                clickCode = code;
            }

            return clickCode;
        }

        private void Render_DesignMode(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute( HtmlTextWriterStyle.Width, this.Width + "px");
            writer.AddStyleAttribute( HtmlTextWriterStyle.Height, this.Height + "px");

            writer.RenderBeginTag( HtmlTextWriterTag.Button);

            

            writer.Write(this.Text);

            writer.RenderEndTag();

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                Render_DesignMode(writer);
                return;
            }


            ScriptManager script = ScriptManager.GetManager(this.Page);


            StringBuilder sb = new StringBuilder();

            if (script != null)
            {
                StringBuilder jsSb = new StringBuilder();
                ScriptTextWriter st = new ScriptTextWriter(jsSb, QuotationMarkConvertor.SingleQuotes);

                BeginReady(jsSb);

                st.RetractBengin();
                FullScript(st);
                st.RetractEnd();

                EndReady(jsSb);

                script.AddScript(jsSb.ToString());
            }
            else
            {
                ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

                BeginScript(sb);
                BeginReady(sb);

                FullScript(st);

                EndReady(sb);
                EndScript(sb);

            }

            sb.AppendFormat("<a id=\"{0}\" style=\"width: 100%;display:none; \"></a>", this.ClientID);

            writer.Write(sb.ToString());

        }

    }
}
