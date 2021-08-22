using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;
using System.Reflection;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 窗体显示状态
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// 默认
        /// </summary>
        Normal,
        /// <summary>
        /// 最大化
        /// </summary>
        Max,
        /// <summary>
        /// 最小化
        /// </summary>
        Min
    }

    /// <summary>
    /// 窗体开始显示的位置
    /// </summary>
    public enum WindowStartPosition
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 屏幕中心
        /// </summary>
        CenterScreen,
        /// <summary>
        /// 相对于父窗体位置
        /// </summary>
        CenterParent
    }




    /// <summary>
    /// 窗体对象
    /// </summary>
    public class Window
    {
        int m_Width = 800;
        int m_Height = 600;

        bool m_Mode = false;

        bool m_Resizable = false;

        /// <summary>
        /// 命令控件
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string JsNamespace { get; set; } = "Mini2.ui.Window";

        WindowState m_State = WindowState.Normal;

        WindowStartPosition m_StartPosition = WindowStartPosition.Default;

        /// <summary>
        /// 窗体关闭的事件
        /// </summary>
        public event CallbackEventHandler WindowClosed;

        /// <summary>
        /// 触发窗体关闭的事件
        /// </summary>
        /// <param name="data">回调的数据</param>
        protected void OnWindowClosed(string data)
        {
            WindowClosed?.Invoke(this, data);
        }


        /// <summary>
        /// 注册新的属性
        /// </summary>
        Dictionary<string,string> m_RegionPropertys = null;

        /// <summary>
        /// 注册新属性
        /// </summary>
        /// <param name="propName"></param>
        public void RegionProperty(string propName,string jsPropName)
        {
            if (m_RegionPropertys == null)
            {
                m_RegionPropertys = new Dictionary<string, string>();
            }

            if (!m_RegionPropertys.ContainsKey(propName))
            {
                m_RegionPropertys.Add(propName,jsPropName);
            }
        }

        string m_Text = "Window";



        string m_ContentPath;

        string m_ContentHtml;

        /// <summary>
        /// 阴影
        /// </summary>
        bool m_ShadowVisible = true;

        /// <summary>
        /// 标题显示
        /// </summary>
        bool m_HeaderVisible = true;

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        public Window()
        {
        }

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        /// <param name="width">窗体宽度</param>
        /// <param name="height">窗体高度</param>
        public Window(int width, int height)
        {
            m_Width = width;
            m_Height = height;
        }

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        /// <param name="text">标题</param>
        public Window(string text)
        {
            m_Text = text;
        }

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        /// <param name="text">标题</param>
        /// <param name="width">窗体宽度</param>
        /// <param name="height">窗体高度</param>
        public Window(string text, int width, int height)
        {
            m_Text = text;

            m_Width = width;
            m_Height = height;
        }

        /// <summary>
        /// 支持回调的标记数据
        /// </summary>
        [Description("支持回调的标记数据")]
        [DefaultValue("")]
        public string Tag { get; set; }

        /// <summary>
        /// 窗体状态
        /// </summary>
        [Description("窗体状态")]
        [DefaultValue(WindowState.Normal)]
        public WindowState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        /// <summary>
        /// 窗体启动的显示模式
        /// </summary>
        [Description("窗体启动的显示模式")]
        [DefaultValue(WindowStartPosition.Default)]
        public WindowStartPosition StartPosition
        {
            get { return m_StartPosition; }
            set { m_StartPosition = value; }
        }


        /// <summary>
        /// 显示阴影
        /// </summary>
        [DefaultValue(true)]
        public bool ShadowVisible
        {
            get { return m_ShadowVisible; }
            set { m_ShadowVisible = value; }
        }

        /// <summary>
        /// 标题显示
        /// </summary>
        [DefaultValue(true)]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set { m_HeaderVisible = value; }
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
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// 模式窗体
        /// </summary>
        public bool Mode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        /// <summary>
        /// 允许缩放窗体
        /// </summary>
        public bool Resizable
        {
            get { return m_Resizable; }
            set { m_Resizable = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        public string ContentHtml
        {
            get { return m_ContentHtml; }
            set { m_ContentHtml = value; }
        }

        /// <summary>
        /// 内容的 url
        /// </summary>
        public string ContentPath
        {
            get { return m_ContentPath; }
            set { m_ContentPath = value; }
        }

        bool m_IsParentShow = false;

        public bool IsParentShow
        {
            get
            {
                return m_IsParentShow;
            }
            set { m_IsParentShow = value; }
        }

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        public virtual void ShowDialog()
        {
            m_Mode = true;

            Show();
        }

        private void ProExPropertys(StringBuilder code)
        {
            if (m_RegionPropertys != null && m_RegionPropertys.Count > 0)
            {
                Type meType = this.GetType();

                foreach (var regProp in m_RegionPropertys)
                {
                    PropertyInfo pi = meType.GetProperty(regProp.Key);

                    if (pi == null)
                    {
                        throw new Exception($"注册的新属性 \"{regProp.Key}\"不存在");
                    }

                    object piValue = pi.GetValue(this, null);

                    if (piValue == null)
                    {
                        code.AppendFormat("{0}: null,", regProp.Value);
                        continue;
                    }

                    Type valueType = piValue.GetType();

                    if (valueType == typeof(string))
                    {
                        code.AppendFormat("{0}: '{1}',", regProp.Value, piValue);
                    }
                    else if (valueType == typeof(DateTime))
                    {
                        code.AppendFormat("{0}: '{1}',", regProp.Value, piValue);
                    }
                    else if (valueType == typeof(bool))
                    {
                        code.AppendFormat("{0}: {1},", regProp.Key, piValue.ToString().ToLower());
                    }
                    else if (valueType == typeof(int) || valueType == typeof(long) || valueType == typeof(decimal))
                    {
                        code.AppendFormat("{0}: {1},", regProp.Value, piValue);
                    }
                    else
                    {
                        code.AppendFormat("{0}: '{1}',", regProp.Value, piValue);
                    }
                }
            }

        }

        /// <summary>
        /// 显示普通窗体
        /// </summary>
        public virtual void Show()
        {
            StringBuilder code = new StringBuilder();


            code.AppendFormat("var curDialog = Mini2.createTop('{0}', {{", this.JsNamespace);

            code.AppendFormat("text:'{0}', ", m_Text);
            code.AppendFormat("width: {0}, ", m_Width);
            code.AppendFormat("height:{0}, ", m_Height);

            if (!m_HeaderVisible)
            {
                code.AppendFormat("headerVisible:{0}, ", m_HeaderVisible.ToString().ToLower());
            }

            if (!m_ShadowVisible)
            {
                code.AppendFormat("shadowVisible:{0}, ", m_ShadowVisible.ToString().ToLower());
            }

            if (m_State != WindowState.Normal)
            {
                code.AppendFormat("state:'{0}', ", m_State.ToString().ToLower());
            }

            if (m_StartPosition != WindowStartPosition.Default)
            {
                switch (m_StartPosition)
                {
                    case WindowStartPosition.CenterScreen:
                        code.Append("startPosition: 'center_screen', ");
                        break;
                    case WindowStartPosition.CenterParent:
                        code.Append("startPosition: 'center_parent', ");
                        break;
                }
            }


            ProExPropertys(code);



            if (!StringUtil.IsBlank(this.ContentPath))
            {
                code.AppendFormat("iframe:{0}, ", "true");
                code.AppendFormat("url: '{0}', ", m_ContentPath);
            }

            code.AppendFormat("mode:{0}", m_Mode.ToString().ToLower());
            code.Append("});");
            code.AppendLine("curDialog.show();");

            if(this.WindowClosed != null)
            {
                string method = this.WindowClosed.Method.Name;

                code.AppendLine("curDialog.formClosed(function(e){");

                // code.AppendLine("    alert( Mini2.Json.toJson(e) ); ");
                // code.AppendLine("    if( e.result != 'ok') return false;

                code.AppendLine("    widget1.submit('form', {");
                code.AppendLine("        RMode:'callback', ");
                code.AppendLine("        callback_owner:'Window', ");
                code.AppendLine("        rdata:{");

                if (!string.IsNullOrEmpty( this.Tag))
                {
                    string tag = JsonUtil.ToJson(this.Tag);

                    code.AppendLine($"         tag : '{tag}' ");
                }

                code.AppendLine("       },");

                code.AppendLine($"        action: '{method}', ");
                code.AppendLine($"        actionPs: e ");

                code.AppendLine("    });");

                code.AppendLine("});");
            }

            if (!StringUtil.IsBlank(m_FormClosedForJS))
            {
                code.AppendLine("curDialog.formClosed(" + m_FormClosedForJS + ");");
            }


            ScriptManager.Eval(code.ToString());
        }


        /// <summary>
        /// 窗体关闭后,触发的脚本
        /// </summary>
        string m_FormClosedForJS;

        /// <summary>
        /// 窗体关闭后,触发的脚本
        /// </summary>
        public string FormClosedForJS
        {
            get { return m_FormClosedForJS; }
            set { m_FormClosedForJS = value; }
        }



        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName">函数名称</param>
        /// <returns></returns>
        public string GetMethodJs(string methodName)
        {
            return GetMethodJs(methodName, null);
        }

        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodParams">函数参数</param>
        /// <returns></returns>
        public string GetMethodJs(string methodName, string methodParams)
        {
            string js;

            if (string.IsNullOrEmpty(methodParams))
            {
                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}'}});", "", methodName);
            }
            else
            {
                if (StringUtil.StartsWith(methodParams, "{") && StringUtil.EndsWith(methodParams, "}"))
                {

                }
                else
                {
                    methodParams = string.Concat("'", methodParams, "'");
                }

                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}', commandParam:{2} }});", "", methodName, methodParams);
            }

            return js;
        }

    }
}
