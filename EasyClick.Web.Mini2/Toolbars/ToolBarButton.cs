using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 工具栏-按钮
    /// </summary>
    [Description("工具栏-按钮")]
    public class ToolBarButton : ToolBarItem
    {
        string m_Text;

        string m_OnClick;

        string m_Icon;

        string m_IconClass;

        string m_Command;
        string m_Params;

        string m_Tooltip;

        bool m_Valid = false;

        /// <summary>
        /// 点击按钮前触发的询问
        /// </summary>
        string m_BeforeAskText;



        /// <summary>
        /// 点击前触发的事件
        /// </summary>
        string m_OnBeforeClick;


        string m_Href;

        string m_HrefTarget = "_blank";

        /// <summary>
        /// (构造函数)工具栏-按钮
        /// </summary>
        public ToolBarButton()
        {
        }

        /// <summary>
        /// (构造函数)工具栏-按钮
        /// </summary>
        /// </summary>
        /// <param name="text">按钮文本</param>
        public ToolBarButton(string text)
        {
            m_Text = text;
        }

        /// <summary>
        /// (构造函数)工具栏-按钮
        /// </summary>
        /// <param name="text">按钮文本</param>
        /// <param name="onClick">点击事件</param>
        public ToolBarButton(string text, string onClick)
        {
            m_Text = text;
            m_OnClick = onClick;
        }


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
        /// 是否需要验证
        /// </summary>
        [DefaultValue(false)]
        [Description("是否需要验证")]
        public bool Valid
        {
            get { return m_Valid; }
            set { m_Valid = value; }
        }


        /// <summary>
        /// 服务器命令名称
        /// </summary>
        [Description("服务器命令名称")]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        /// <summary>
        /// 服务器命令的参数
        /// </summary>
        [Description("服务器命令的参数")]
        public string Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

        /// <summary>
        /// 按钮名称
        /// </summary>
        [Description("按钮名称")]
        public string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;

                if (!string.IsNullOrEmpty(ID))
                {
                    MiniHelper.EvalFormat("if(window.{0}){{window.{0}.setText('{1}');}}", this.ID, value);
                }
            }
        }


        /// <summary>
        /// 点击前触发的询问信息
        /// </summary>
        [DefaultValue("")]
        [Description("点击前触发的询问信息")]
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
        /// 用户点击的 js 方法名称. 
        /// 例: OnClick="alert('你好')"
        /// </summary>
        [Description("用户点击的 js 方法名称.\n例: OnClick=\"alert('你好')\" ")]
        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        /// <summary>
        /// 图标路径
        /// </summary>
        [Description("图标路径")]
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        /// <summary>
        /// 图标样式
        /// </summary>
        [Description("图标样式")]
        public string IconClass
        {
            get { return m_IconClass; }
            set { m_IconClass = value; }
        }

        protected internal override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.Write("<button style='margin-right:4px;padding:6px;'>" + this.Text + "</button>");

            }
        }

        public string Format_OnClick(string code)
        {
            string clickCode;

            if (code.StartsWith("ser:"))
            {
                code = code.Substring(4);

                int n = code.IndexOf(".");

                int ghL = code.IndexOf("(", n + 1);

                string conName = code.Substring(0, n);
                string methodName = code.Substring(n+1, ghL - n-1);

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

        public override string GetConfigJS()
        {
            StringBuilder sb = new StringBuilder();

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");

            st.WriteParam("id", this.ID);

            st.WriteParam("icon", this.Icon);
            st.WriteParam("iconCls", this.IconClass);

            st.WriteParam("text", this.Text);
            st.WriteParam("code", this.Code);
            st.WriteParam("dock", this.Align, TextTransform.Lower);
            st.WriteParam("userCls", this.Class);
            st.WriteParam("href", this.Href);
            st.WriteParam("hrefTarget", this.HrefTarget,"_blank");
            st.WriteParam("visible", this.Visible, true);
            st.WriteParam("tooltip", this.Tooltip);
            st.WriteParam("beforeAskText", this.BeforeAskText);
            st.WriteParam("command", this.Command);

            st.WriteParam("secFunCode", this.SecFunCode);   //权限编码

            if (!string.IsNullOrEmpty(this.Params))
            {
                if (StringUtil.StartsWith(this.Params, "{") && StringUtil.EndsWith(this.Params, "}"))
                {
                    st.WriteFunction("commandParams", this.Params);
                }
                else
                {
                    st.WriteParam("commandParams", this.Params);
                }
            }

            if (!string.IsNullOrEmpty(this.OnClick))
            {
                string formatOnClick = Format_OnClick(this.OnClick);

                if (!formatOnClick.EndsWith(";"))
                {
                    formatOnClick += ";";
                }
                st.WriteFunction("click",$"function(){{ {formatOnClick} }}");
            }

            if (!StringUtil.IsBlank(this.m_OnBeforeClick))
            {
                st.WriteFunction("beforeClick", $"function(){{ return {m_OnBeforeClick}; }}");
            }

            st.RetractEnd("}");


            return sb.ToString();
        }
    }


}
