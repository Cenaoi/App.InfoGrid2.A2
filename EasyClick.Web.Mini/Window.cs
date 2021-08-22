using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// Html 窗体
    /// </summary>
    public class Window
    {
        Unit m_Width = Unit.Empty;
        Unit m_Height = Unit.Empty;

        bool m_Modal = false;

        bool m_Resizable = true;

        bool m_Stack = true;

        string m_Title = "Window";

        Uri m_ContentUri;

        string m_ContentPath;

        string m_ContentHtml;

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
            m_Width = new Unit(width, UnitType.Pixel);
            m_Height = new Unit(height, UnitType.Pixel);
        }

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        /// <param name="title">标题</param>
        public Window(string title)
        {
            m_Title = title;
        }

        /// <summary>
        /// Html 窗体构造方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="width">窗体宽度</param>
        /// <param name="height">窗体高度</param>
        public Window(string title, int width, int height)
        {
            m_Title = title;

            m_Width = new Unit(width, UnitType.Pixel);
            m_Height = new Unit(height, UnitType.Pixel);
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public Unit Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// 模式窗体
        /// </summary>
        public bool Modal
        {
            get { return m_Modal; }
            set { m_Modal = value; }
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
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public bool Stack
        {
            get { return m_Stack; }
            set { m_Stack = value; }
        }

        public Uri ContentUri
        {
            get { return m_ContentUri; }
            set { m_ContentUri = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentHtml
        {
            get { return m_ContentHtml; }
            set { m_ContentHtml = value; }
        }

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
            m_Modal = true;

            Show();
        }

        /// <summary>
        /// 显示普通窗体
        /// </summary>
        public virtual void Show()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(m_ContentPath))
            {
                sb.Append("<iframe frameborder='0'  ");
                if (!string.IsNullOrEmpty(m_ContentPath))
                {
                    sb.AppendFormat("src='{0}' ", m_ContentPath);
                }
                else
                {
                    sb.AppendFormat("src='{0}' ", m_ContentUri.ToString());
                }
                sb.Append("></iframe>");
            }
            else
            {
                sb.Append(m_ContentHtml);
            }

            StringBuilder code = new StringBuilder();

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                code.Append("In('mi.Window');");
            }

            code.AppendFormat("curDialog = new Mini.ui.Window({{contentHtml:\"{0}\", ", sb.ToString());

            code.AppendFormat("title:'{0}', ", m_Title) ;
            code.AppendFormat("resizable:{0}, ", m_Resizable.ToString().ToLower());
            code.AppendFormat("stack:{0}, ", m_Stack.ToString().ToLower());
            code.AppendFormat("width: {0}, ", (int)(m_Width.Value + 30));
            code.AppendFormat("height:{0}, ", (int)(m_Height.Value + 50));
            code.AppendFormat("modal:{0}" , m_Modal.ToString().ToLower());
            code.Append("});");
            code.Append("curDialog.show();");

            MiniScript.Add(code.ToString());

        }

    }
}
