using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 脚本管理
    /// </summary>
    public class ScriptManager:Component
    {
        
        /// <summary>
        /// 只读
        /// </summary>
        bool m_ReadOnly = false;

        List<string> m_Items = new List<string>();

        List<string> m_LastItems = new List<string>();

        /// <summary>
        /// 输出执行脚本
        /// </summary>
        /// <param name="js"></param>
        public static void Eval(string js)
        {
            EasyClick.Web.Mini.MiniScript.Add(js);
        }

        /// <summary>
        /// 输出执行脚本
        /// </summary>
        /// <param name="jsFormat"></param>
        /// <param name="arg0"></param>
        public static void Eval(string jsFormat, object arg0)
        {
            string code = string.Format(jsFormat, arg0);

            EasyClick.Web.Mini.MiniScript.Add(code);
        }

        /// <summary>
        /// 输出执行脚本
        /// </summary>
        /// <param name="jsFormat"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Eval(string jsFormat, object arg0,object arg1)
        {
            string code = string.Format(jsFormat, arg0,arg1);

            EasyClick.Web.Mini.MiniScript.Add(code);
        }

        /// <summary>
        /// 输出执行脚本
        /// </summary>
        /// <param name="jsFormat"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Eval(string jsFormat, object arg0,object arg1,object arg2)
        {
            string code = string.Format(jsFormat, arg0,arg1,arg2);

            EasyClick.Web.Mini.MiniScript.Add(code);
        }

        /// <summary>
        /// 输出执行脚本
        /// </summary>
        /// <param name="jsFormat"></param>
        /// <param name="args"></param>
        public static void Eval(string jsFormat,params object[] args)
        {
            string code = string.Format(jsFormat, args);

            EasyClick.Web.Mini.MiniScript.Add(code);
        }



        /// <summary>
        /// 
        /// </summary>
        public ScriptManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOnly"></param>
        public ScriptManager(bool readOnly)
        {
            m_ReadOnly = readOnly;
        }

        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
        }

        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <param name="js"></param>
        public void AddScript(string js)
        {
            if (m_ReadOnly) { return; }

            m_Items.Add(js);
        }

        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <param name="js"></param>
        public void AddScript(StringBuilder js)
        {
            if (m_ReadOnly) { return; }

            m_Items.Add(js.ToString());
        }

        /// <summary>
        /// 添加脚本到最后面
        /// </summary>
        /// <param name="js"></param>
        public void AddScriptForLast(string js)
        {
            if (m_ReadOnly) { return; }

            m_LastItems.Add(js);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string jsMode = null;

            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs" || jsMode == "MInJs" || jsMode == "MInJs2")
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<script type=\"text/javascript\">");

                //sb.AppendLine("if(window.In){");

                foreach (string item in m_Items)
                {
                    sb.AppendLine(item);
                }

                foreach (string item in m_LastItems)
                {
                    sb.AppendLine(item);
                }

                //sb.AppendLine("window.In.ready('Mini2.LoaderManager','Mini2.ui.panel.Panel','Mini2.ui.container.Viewport',function(){");
                //sb.AppendLine("  setTimeout(function(){");
                //sb.AppendLine("    Mini2.LoaderManager.preRenderComplete();");
                //sb.AppendLine("  },500);");
                //sb.AppendLine("});");

                //sb.AppendLine("}");

                sb.AppendLine("$(document).ready(function(){");
                sb.AppendLine("    var m2 = window.Mini2;");
                sb.AppendLine("    if(m2 && m2.LoaderManager){");
                sb.AppendLine("        m2.LoaderManager.preRenderComplete();");
                sb.AppendLine("    }");
                sb.AppendLine("});");

                sb.AppendLine("</script>");

                writer.Write(sb.ToString());
            }


        }



        /// <summary>
        /// 获取管理管理脚本的容器
        /// </summary>
        /// <param name="parent">父级容器</param>
        /// <returns></returns>
        public static ScriptManager GetManager(Control parent)
        {
            if (parent == null)
            {
                return null;
            }

            System.Web.HttpContext content = HttpContext.Current;

            if (content != null && content.Items.Contains("EC5_ScriptManager"))
            {
                //这个代码需要优化，有可能在跳转页面过程中产生错误。
                ScriptManager sm = content.Items["EC5_ScriptManager"] as ScriptManager;

                return sm;
            }


            if (parent == null || !parent.HasControls())
            {
                return null;
            }

            Control tCon = null;

            foreach (Control con in parent.Controls)
            {
                if (con is ScriptManager)
                {
                    tCon = con;
                    break;
                }

                Control tmpCon = GetManager(con);

                if (tmpCon != null)
                {
                    tCon = tmpCon;
                    break;
                }
            }

            if (tCon != null)
            {
                content.Items["EC5_ScriptManager"] = tCon;
            }

            return tCon as ScriptManager;
        }
    }
}
