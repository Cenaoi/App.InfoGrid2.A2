using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 工具栏-按钮
    /// </summary>
    public class ToolBarButton : ToolBarItem
    {
        string m_Text;

        string m_OnClick;

        string m_Icon;

        string m_Command;
        string m_Params;

        string m_Tooltip;

        bool m_Valid = false;

        /// <summary>
        /// 验证
        /// </summary>
        [DefaultValue(false)]
        public bool Valid
        {
            get { return m_Valid; }
            set { m_Valid = value; }
        }

        /// <summary>
        /// 按钮提示信息
        /// </summary>
        [DefaultValue("")]
        public string Tooltip
        {
            get { return m_Tooltip; }
            set { m_Tooltip = value; }
        }


        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        public string Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 用户点击的 js 方法名称. 
        /// 例: OnClick="alert('你好')"
        /// </summary>
        public string OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        /// <summary>
        /// 图标路径
        /// </summary>
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected internal override void Render(HtmlTextWriter writer)
        {

            int len = 0;

            if (!string.IsNullOrEmpty(m_Text))
            {
                len = UnicodeEncoding.Default.GetByteCount(m_Text);
            }

            if (!string.IsNullOrEmpty(m_Icon))
            {
                len += 2;
            }

            writer.Write("<a href='#' class='Len{0}' ", len);

            if (!string.IsNullOrEmpty(this.ID))
            {
                writer.Write("id=\"{0}\" ", this.ID);
            }

            if (this.Align == ToolBarItemAlign.Right)
            {
                writer.Write("style=\"float:{0};\" ", this.Align.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(m_Tooltip))
            {
                writer.Write("title=\"{0}\" ", m_Tooltip);
            }

            if (!string.IsNullOrEmpty(m_Command))
            {
                writer.Write("command='{0}' ", m_Command);
            }

            if (!m_Valid)
            {
                writer.Write("valid='false' ");
            }

            if (string.IsNullOrEmpty(m_OnClick))
            {
                writer.Write("onclick=\"{0}\" >", MiniConfiguration.ToolBarBtnConfig.OnClickDefaultValue);
            }
            else
            {
                writer.Write("onclick=\"{0}\" >", m_OnClick);
            }

            if (!string.IsNullOrEmpty(m_Icon))
            {
                if (m_Icon[0] == '$')
                {
                    
                    writer.Write("<img src=\"{0}\" border='0' class='ico' />", MiniConfiguration.IconResConfig.GetUrl(m_Icon));
                }
                else
                {
                    writer.Write("<img src=\"{0}\" border='0' class='ico' />", m_Icon);
                }
            }

            if (!string.IsNullOrEmpty(m_Text))
            {
                if (string.IsNullOrEmpty(m_Icon))
                {
                    writer.Write(m_Text);
                }
                else
                {
                    writer.Write("<span>{0}</span>", m_Text);
                }
            }

            writer.Write("</a>");

        }

    }


}
