using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.IO;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    public enum ButtonFieldTypes
    {
        Button,
        Link,
        Image
    }

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ButtonField : BoundField
    {
        string m_CommandName;
        string m_CommandParam;

        string m_Text = "按钮";

        /// <summary>
        /// 取消验证
        /// </summary>
        bool m_CansesValidation = false;

        ButtonFieldTypes m_ButtonType = ButtonFieldTypes.Button;
                
        string m_OnClientClick;

        string m_ImageUrl;






        [DefaultValue("")]
        public string ImageUrl
        {
            get { return m_ImageUrl; }
            set { m_ImageUrl = value; }
        }

        int m_ImageWidth = 0;
        int m_ImageHeight = 0;

        [DefaultValue(0)]
        public int ImageWidth
        {
            get { return m_ImageWidth; }
            set { m_ImageWidth = value; }
        }

        [DefaultValue(0)]
        public int ImageHeight
        {
            get { return m_ImageHeight; }
            set { m_ImageHeight = value; }
        }

        [DefaultValue("")]
        public string OnClientClick
        {
            get { return m_OnClientClick; }
            set { m_OnClientClick = value; }
        }

        [DefaultValue("")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        [DefaultValue(ButtonFieldTypes.Button)]
        public ButtonFieldTypes ButtonType
        {
            get { return m_ButtonType; }
            set { m_ButtonType = value; }
        }

        [DefaultValue("")]
        public string CommandName
        {
            get { return m_CommandName; }
            set { m_CommandName = value; }
        }

        [DefaultValue("")]
        public string CommandParam
        {
            get { return m_CommandParam; }
            set { m_CommandParam = value; }
        }

        [DefaultValue(false)]
        public bool CansesValidation
        {
            get { return m_CansesValidation; }
            set { m_CansesValidation = value; }
        }


        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            TextWriter tw = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(tw);

            if (this.Width > 0)
            {
                writer.AddAttribute("width", this.Width + "px");
            }

            if (!this.Visible)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }


            if (cellType == MiniDataControlCellType.Header)
            {
                this.CreateHeaderHtmlTemplate(writer, cellType, MiniDataControlRowState.Normal);
                //writer.AddAttribute("nowrap", "nowrap");
                //writer.RenderBeginTag(HtmlTextWriterTag.Th);
                //writer.Write(this.HeaderText);
                //writer.RenderEndTag();
            }
            else if (cellType == MiniDataControlCellType.DataCell)
            {
                if (this.ItemAlign != CellAlign.Left)
                {
                    writer.AddAttribute("align", this.ItemAlign.ToString().ToLower());
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if (m_ButtonType == ButtonFieldTypes.Button)
                {
                    writer.AddAttribute("type", "button");

                    writer.AddAttribute("command", m_CommandName);
                    writer.AddAttribute("commandParam", m_CommandParam);

                    writer.AddAttribute("value", this.Text);

                    if (string.IsNullOrEmpty(m_OnClientClick))
                    {
                        writer.AddAttribute("onclick", this.Owner.Parent.ClientID + ".submit(this);return false;");
                    }
                    else
                    {
                        writer.AddAttribute("onclick", this.m_OnClientClick);
                    }

                    if (!string.IsNullOrEmpty(this.Tooltip))
                    {
                        writer.AddAttribute("title", this.Tooltip);
                    }

                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                }
                else if (m_ButtonType == ButtonFieldTypes.Link)
                {
                    writer.AddAttribute("href", "#");

                    writer.AddAttribute("command", m_CommandName);
                    writer.AddAttribute("commandParam", m_CommandParam);

                    writer.AddAttribute("nowrap", "nowrap");
                    
                    if (string.IsNullOrEmpty(m_OnClientClick))
                    {
                        writer.AddAttribute("onclick", this.Owner.Parent.ClientID + ".submit(this);return false;");
                    }
                    else
                    {
                        writer.AddAttribute("onclick", this.m_OnClientClick);
                    }

                    if (!string.IsNullOrEmpty(this.Tooltip))
                    {
                        writer.AddAttribute("title", this.Tooltip);
                    }

                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write( this.Text);
                    writer.RenderEndTag();
                }
                else if (m_ButtonType == ButtonFieldTypes.Image)
                {
                    writer.AddAttribute("href", "#");

                    writer.AddAttribute("command", m_CommandName);
                    writer.AddAttribute("commandParam", m_CommandParam);


                    if (string.IsNullOrEmpty(m_OnClientClick))
                    {
                        writer.AddAttribute("onclick", this.Owner.Parent.ClientID + ".submit(this);return false;");
                    }
                    else
                    {
                        writer.AddAttribute("onclick", this.m_OnClientClick);
                    }

                    if (!string.IsNullOrEmpty(this.Tooltip))
                    {
                        writer.AddAttribute("title", this.Tooltip);
                    }

                    writer.AddAttribute("alt", m_Text);

                    writer.RenderBeginTag(HtmlTextWriterTag.A);

                    writer.AddAttribute("border", "0");
                    writer.AddAttribute("src", m_ImageUrl);


                    if (m_ImageWidth != 0) { writer.AddAttribute("width", m_ImageWidth.ToString() ); }
                    if (m_ImageHeight != 0) { writer.AddAttribute("height", m_ImageHeight.ToString() ); }

                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();

                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }

            string txt = tw.ToString();

            writer.Dispose();
            tw.Dispose();

            return txt;

        }
    }
}
