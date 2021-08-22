using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 普通文本项目
    /// </summary>
    public class DynamicItem 
    {
        string m_Value;
        string m_Text;

        string m_Tooltip;

        internal DynamicField m_Owner;

        public string Tooltip
        {
            get { return m_Tooltip; }
            set { m_Tooltip = value; }
        }
      

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }


        public virtual void RenderLogin(StringBuilder sb)
        {
            sb.AppendFormat("$T.{0} == \"{1}\"", m_Owner.DataField, m_Value);
        }

        public virtual void Render(StringBuilder sb)
        {
            sb.Append(m_Text);
        }


        public virtual void RenderLogin(HtmlTextWriter writer)
        {
            writer.Write("$T.{0} == \"{1}\"", m_Owner.DataField, m_Value);
        }

        public virtual void Render(HtmlTextWriter writer)
        {
            writer.Write(m_Text);
        }
    }

    /// <summary>
    /// 图片项目
    /// </summary>
    public class ImageDItem : DynamicItem
    {
        string m_Src;

        int m_Width = 0;
        int m_Height = 0;

        public string Src
        {
            get { return m_Src; }
            set { m_Src = value; }
        }

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public override void Render(StringBuilder sb)
        {

            sb.AppendFormat("<img src=\"{0}\" ", m_Src);

            if (m_Width > 0)
            {
                sb.AppendFormat("width=\"{0}px\" ", m_Width);
            }

            if (m_Height > 0)
            {
                sb.AppendFormat("height=\"{0}px\" ", m_Height);
            }

            sb.Append("/>");
        }
    }

    /// <summary>
    /// 图片按钮项目
    /// </summary>
    public class ImageButtonDItem : DynamicItem
    {
        string m_Src;
        int m_Width = 0;
        int m_Height = 0;

        string m_OnClick;

        public string Src
        {
            get { return m_Src; }
            set { m_Src = value; }
        }

        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }



        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }



        public override void Render(StringBuilder sb)
        {

            sb.AppendFormat("<a href=\"{0}\" ", "#");

            if (!string.IsNullOrEmpty(m_OnClick))
            {
                sb.AppendFormat("onclick=\"{0}\" ", m_OnClick);
            }

            sb.Append(">");
            {

                sb.AppendFormat("<img border=\"0\" src=\"{0}\" ", m_Src);

                if (m_Width > 0)
                {
                    sb.AppendFormat("width=\"{0}px\" ", m_Width);
                }

                if (m_Height > 0)
                {
                    sb.AppendFormat("height=\"{0}px\" ", m_Height);
                }

                sb.Append("/>");
            }
            sb.Append("</a>");
        }


    }

    /// <summary>
    /// 超链接
    /// </summary>
    public class HyperLinkDItem : DynamicItem
    {
        string m_Url;

        string m_Target;

        string m_OnClick;


        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        public string Target
        {
            get { return m_Url; }
            set { m_Url = value; }
        }


        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }


        public override void Render(HtmlTextWriter writer)
        {

            writer.Write("<a href=\"{0}\" ", m_Url);

            if (!string.IsNullOrEmpty(m_Target))
            {
                writer.Write("target=\"{0}\" ", m_Target);
            }

            writer.Write(">");
            writer.Write(this.Text);
            writer.Write("</a>");
        }
    }

    /// <summary>
    /// 按钮项目
    /// </summary>
    public class ButtonDItem : DynamicItem
    {
        string m_Src;

        int m_Width = 0;
        int m_Height = 0;

        string m_OnClick;

        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }


        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }



        public override void Render(StringBuilder sb)
        {

            sb.AppendFormat("<button type=\"button\" onclick=\"{0}\" ", "#");
            if (m_Width > 0)
            {
                sb.AppendFormat("width=\"{0}px\" ", m_Width);
            }

            if (m_Height > 0)
            {
                sb.AppendFormat("height=\"{0}px\" ", m_Height);
            }

            sb.Append(">");

            sb.Append(this.Text);

            sb.Append("</button>");
        }


    }

    public class TemplateDItem : DynamicItem
    {
        string m_Content;

        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        public override void Render(StringBuilder sb)
        {
            sb.Append(m_Content);
        }
    }
}
