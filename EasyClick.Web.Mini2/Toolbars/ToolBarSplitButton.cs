using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;


namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 工具栏-分隔符
    /// </summary>
    [Description("工具栏-分隔符")]
    public class ToolBarSplitButton : ToolBarButton
    {
        /// <summary>
        /// (构造函数)工具栏-分隔符
        /// </summary>
        public ToolBarSplitButton()
            : base()
        {
            this.Icon = "/res/icon/DownBtn.png";
        }

        string m_Target;
        string m_EventType = "hover";

        DockStyle m_Dock = DockStyle.Left;

        /// <summary>
        /// 版块位置
        /// </summary>
        [DefaultValue(DockStyle.Left)]
        public DockStyle Dock
        {
            get { return m_Dock; }
            set { m_Dock = value; }
        }

        [DefaultValue("")]
        public string EventType
        {
            get { return m_EventType; }
            set { m_EventType = value; }
        }
        
        public string Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }


        protected internal override void Render(System.Web.UI.HtmlTextWriter writer)
        {

            string tmpClassName;

            if (string.IsNullOrEmpty(this.ID))
            {
                int classID = RandomUtil.Next(int.MaxValue);
                tmpClassName = "SplitBtn" + classID;
            }
            else
            {
                tmpClassName = this.ID;
            }


            writer.Write("<a href='#' class='{0}' ", (m_Dock == DockStyle.Left) ? "DcokRight" : "DcokLeft");

            //if (!string.IsNullOrEmpty(this.ID))
            //{
            writer.Write("id=\"{0}\" ", tmpClassName);
            //}

            if (this.Align == ToolBarItemAlign.Right)
            {
                writer.Write("style=\"float:{0};\" ", this.Align.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.Tooltip))
            {
                writer.Write("title=\"{0}\" ", this.Tooltip);
            }

            if (!string.IsNullOrEmpty(this.Command))
            {
                writer.Write("command='{0}' ", this.Command);
            }

            if (!this.Valid)
            {
                writer.Write("valid='false' ");
            }


            writer.Write("onclick=\"{0}\" >", this.OnClick);
            
            writer.Write("<img src=\"{0}\" border='0' class='ico' />", this.Icon);

            writer.Write("</a>");


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script type='text/javascript'>");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                sb.AppendLine("In.ready('jq.powerFloat',function () {");
            }
            else
            {
                sb.AppendLine("$(document).ready(function () {");
            }

            sb.AppendFormat("$('#{0}').powerFloat({{", tmpClassName);
            sb.AppendFormat("  position: '{0}',", (m_Dock == DockStyle.Left?"3-2":"4-1"));
            sb.AppendFormat("  eventType: '{0}',",m_EventType);
            sb.AppendFormat("  target: '{0}'",m_Target);
            sb.Append("});");

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            writer.Write(sb.ToString());
        }
    }
}
