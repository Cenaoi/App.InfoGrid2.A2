using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using log4net;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 轻量级组件
    /// </summary>
    public class MiniWidget:IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region  字段

        UserControl m_Con;

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        bool m_EcDesignMode = false;

        bool m_Ajax = false;

        /// <summary>
        /// 容器ID,给 AJAX 使用的
        /// </summary>
        string m_PanelID;

        /// <summary>
        /// 网页上的请求信息
        /// </summary>
        string m_Query;

        /// <summary>
        /// 延迟加载
        /// </summary>
        bool m_EcDelay = true;

        string m_EcReturnFormat = "html";

        string m_Params;


        string m_Path;

        /// <summary>
        /// 注意：不要放入默认值
        /// </summary>
        string m_Language;

        bool m_IsUser = false;

        Exception m_Exception = null;

        string m_ClinetID;

        System.Web.UI.AttributeCollection m_Attributes;

        bool m_CreateParentBox = true;

        #endregion

        #region 属性

        /// <summary>
        /// 创建父容器
        /// </summary>
        [DefaultValue(true)]
        public bool CreateParentBox
        {
            get { return m_CreateParentBox; }
            set { m_CreateParentBox = value; }
        }

        public UserControl Con
        {
            get { return m_Con; }
            set { m_Con = value; }
        }

        //
        // 摘要:
        //     获取在 .aspx 文件中的用户控件标记中声明的所有属性名和值对的集合。
        //
        // 返回结果:
        //     包含在用户控件标记中声明的所有名称和值对的 System.Web.UI.AttributeCollection 对象。
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                if (m_Attributes == null)
                {
                    m_Attributes = new System.Web.UI.AttributeCollection(new StateBag());
                }

                return m_Attributes;
            }
            set
            {
                m_Attributes = value;
            }
        }


        public string Query
        {
            get { return m_Query; }
            set { m_Query = value; }
        }

        public string ClientID
        {
            get { return m_ClinetID; }
            set { m_ClinetID = value; }
        }

        /// <summary>
        /// 延迟加载，默认 true
        /// </summary>
        [Description("延迟加载，默认 true")]
        [DefaultValue(true)]
        public bool EcDelay
        {
            get { return m_EcDelay; }
            set { m_EcDelay = value; }
        }


        /// <summary>
        /// 返回的数据类型
        /// </summary>
        [DefaultValue("html")]
        public string EcReturnFormat
        {
            get { return m_EcReturnFormat; }
            set { m_EcReturnFormat = value; }
        }

        /// <summary>
        /// 容器ID,给 AJAX 使用的
        /// </summary>
        [DefaultValue("")]
        public string PanelID
        {
            get { return m_PanelID; }
            set { m_PanelID = value; }
        }

        /// <summary>
        /// 启动 Ajax  ,默认 false 
        /// </summary>
        [DefaultValue(false)]
        [Description("启动 Ajax  ,默认 false")]
        public bool Ajax
        {
            get { return m_Ajax; }
            set { m_Ajax = value; }
        }

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        [DefaultValue(false)]
        [Description("获取一个值，用以指示当前是否处于设计模式。")]
        public bool EcDesignMode
        {
            get { return m_EcDesignMode; }
            set { m_EcDesignMode = value; }
        }


        /// <summary>
        /// 参数
        /// </summary>
        [DefaultValue("")]
        [Description("参数。")]
        public string Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }


        /// <summary>
        /// 语言
        /// </summary>
        [DefaultValue("")]
        [Description("语言. CH / EN")]
        public string Language
        {
            get { return m_Language; }
            set { m_Language = value; }
        }


        /// <summary>
        /// 组件路径
        /// </summary>
        [Description("组件路径")]
        [DefaultValue("")]
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }


        /// <summary>
        /// 用户自定义组件，默认 false
        /// </summary>
        [DefaultValue(false)]
        [Description("用户自定义组件，默认 false")]
        public bool IsUser
        {
            get { return m_IsUser; }
            set { m_IsUser = value; }
        }

        #endregion

        public void RenderAJAX(HtmlTextWriter writer)
        {

            if (m_Exception != null)
            {
                if (m_Exception.StackTrace != null)
                {
                    writer.Write(m_Exception.StackTrace.Replace("\n", "<br />"));
                }
                else
                {
                    writer.Write(m_Exception.Message);
                }

                return;
            }


            StringBuilder sb = new StringBuilder();

            if (this.EcDelay)
            {

                //如果没有容器，就自己创建一个容器出来
                if (string.IsNullOrEmpty(m_PanelID))
                {
                    m_PanelID = this.ClientID + "__Box";

                    writer.Write("<table style=\"width:100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td id=\"" + m_PanelID + "\">");
                    
                    writer.Write("</td></tr></table>");

                    CreateAjaxScript(writer, sb, m_PanelID);
                }
                else
                {
                    CreateAjaxScript(writer, sb, this.m_PanelID);
                }

                writer.Write(sb.ToString());
            }
            else
            {            
                
                if (m_Con == null)
                {
                    writer.Write("组件不存在：" + this.Path);
                    return;
                }

                if (string.IsNullOrEmpty(m_PanelID))
                {
                    m_PanelID = this.ClientID + "__Box";

                    CreateAjaxHtml(writer, sb, m_PanelID);

                    if (m_CreateParentBox)
                    {
                        writer.Write("<table style='width:100%' cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td id='" + m_PanelID + "'>");

                        m_Con.RenderControl(writer);

                        writer.Write("</td></tr></table>");
                    }
                    else
                    {
                        m_Con.RenderControl(writer);
                    }


                }
                else
                {
                    CreateAjaxHtml(writer, sb, this.ClientID);
                }
            }

            //writer.Write(sb.ToString());
        }



        /// <summary>
        /// 获取 AJAX 控件的参数
        /// </summary>
        /// <returns></returns>
        private string GetAjaxWidgetPs()
        {

            if (Attributes.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            int n = 0;

            foreach (string key in Attributes.Keys)
            {
                string txt = Attributes[key];

                if (n > 0)
                {
                    sb.Append("$$$");
                }

                sb.AppendFormat("{0}={1}", key, txt);
                n++;
            }


            return sb.ToString();
        }


        /// <summary>
        /// 非延迟加载的html 数据
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sb"></param>
        /// <param name="panelId"></param>
        private void CreateAjaxHtml(HtmlTextWriter writer, StringBuilder sb, string panelId)
        {
            //m_Query = Request.Url.Query;

            if (!string.IsNullOrEmpty(m_Query) && m_Query.StartsWith("?"))
            {
                m_Query = m_Query.Substring(1);
            }

            if (!string.IsNullOrEmpty(m_Query))
            {
                m_Query = m_Query.Replace("&", "$$$");
            }



            string widgetPs = GetAjaxWidgetPs();

            string cId = m_Con.ClientID;

            writer.WriteLine();
            writer.WriteLine("<script type='text/javascript'>");
            //writer.WriteLine("<!--");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.Widget',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.Write("    var miWidget = new Mini.ui.Widget(", cId);
            writer.WriteLine("{");
            writer.WriteLine("        __Path: '{0}',", m_Path);
            writer.WriteLine("        __CID: '{0}',", cId);
            writer.WriteLine("        __PID: '#{0}',", panelId);
            writer.WriteLine("        __IsUser: '{0}', ", this.IsUser);
            writer.WriteLine("        __Lang: '{0}', ", this.Language);
            writer.WriteLine("        __WidgetPs: '{0}',", widgetPs);
            writer.WriteLine("        __Query:'{0}',", m_Query);
            writer.WriteLine("        __ReturnFormat: '{0}',", m_EcReturnFormat);
            writer.WriteLine("        __EcDelay: '{0}'", m_EcDelay);
            writer.WriteLine("    });");

            writer.WriteLine("    {0}.ActionPath = '{1}';", "miWidget", MiniConfiguration.ActionPath);
            writer.WriteLine("    window.{0} = {1};", this.ClientID , "miWidget");
            writer.WriteLine("    window.{0} = {1};", cId, "miWidget");

            writer.WriteLine("});");

            //writer.WriteLine("-->");
            writer.WriteLine("</script>");
        }



        private void CreateAjaxScript(HtmlTextWriter writer, StringBuilder sb, string panelId)
        {
            //m_Query = Request.Url.Query;

            if (!string.IsNullOrEmpty(m_Query) && m_Query.StartsWith("?"))
            {
                m_Query = m_Query.Substring(1);
            }

            if (!string.IsNullOrEmpty(m_Query))
            {
                m_Query = m_Query.Replace("&", "$$$");
            }

            if (panelId[0] == '#')
            {
                panelId = panelId.Substring(1);
            }

            string widgetPs = GetAjaxWidgetPs();

            sb.AppendLine("<script type=\"text/javascript\">");
            //sb.AppendLine("<!--");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.Widget',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            sb.AppendLine("        var exPath = {");
            sb.AppendLine("            __Path: '" + m_Path + "',");    //加上 __ ,避免跟正常的组件属性冲突
            sb.AppendLine("            __CID: '" + this.ClientID + "',");
            sb.AppendLine("            __PID: '#" + panelId + "',");
            sb.AppendLine("            __IsPost: '0',");
            sb.AppendLine("            __Query:'" + m_Query + "',");
            sb.AppendLine("            __WidgetPs:'" + widgetPs + "',");
            sb.AppendLine("            __EcDelay: false,");
            sb.AppendLine("            __EcReturnFormat: '" + this.EcReturnFormat + "',");
            if (this.IsUser)
            {
                sb.AppendLine("            __IsUser: '1',");
            }

            sb.AppendLine("            __Lang: '" + this.Language + "',");
            sb.AppendLine("            __Rum__: Math.random()");
            sb.AppendLine("        };");

            sb.AppendLine("        var uu = $.param(exPath);");


            sb.AppendLine("        if(exPath.__Query ){");
            sb.AppendLine("            uu += '&' + exPath.__Query.replace('$$$','&')");
            sb.AppendLine("        }");

            sb.AppendLine("        $('#" + panelId + "').load('" + MiniConfiguration.ActionPath + "?' + uu);");

            sb.AppendLine("    });");
            //sb.AppendLine("-->");
            sb.Append("</script>");
        }

        public void Dispose()
        {
            m_Attributes = null;
            m_Con = null;
            m_Exception = null;
            

        }
    }
}
