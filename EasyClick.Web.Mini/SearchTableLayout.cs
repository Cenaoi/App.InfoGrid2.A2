using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 查询表格版面
    /// </summary>
    [ToolboxData("<{0}:SearchTableLayout runat=\"server\" ><Fields></Fields></{0}:SearchTableLayout>")]
    public class SearchTableLayout:TableLayoutPanel
    {
        public SearchTableLayout()
            : base()
        {            
            this.SetAttribute("border","0");
            this.SetAttribute("cellpadding","0");
            this.SetAttribute("cellspacing", "0");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);



        }

        bool m_Expand = true;
        string m_ToggleFnName = "searchToggle";

        string m_StoreId;

        /// <summary>
        /// 数据仓库ID
        /// </summary>
        [Description("数据仓库ID")]
        public string StoreId
        {
            get { return m_StoreId; }
            set { m_StoreId = value; }
        }

        /// <summary>
        /// 显示隐藏方法名称
        /// </summary>
        [DefaultValue("")]
        public string ToggleFnName
        {
            get { return m_ToggleFnName; }
            set { m_ToggleFnName = value; }
        }

        /// <summary>
        /// 展开
        /// </summary>
        [DefaultValue(true)]
        public bool Expand
        {
            get { return m_Expand; }
            set { m_Expand = value; }
        }

        string m_Groups = "Search";

        /// <summary>
        /// 分组
        /// </summary>
        [DefaultValue("Search")]
        public string Groups
        {
            get { return m_Groups; }
            set { m_Groups = value; }
        }

        protected void SetChildGroups()
        {
            IAttributeAccessor attrs = null;

            if (string.IsNullOrEmpty(m_Groups))
            {
                return;
            }

            foreach (Control item in this.Fields)
            {
                attrs = item as IAttributeAccessor;

                if (attrs == null)
                {
                    continue;
                }

                string groups = attrs.GetAttribute("Groups");

                if(string.IsNullOrEmpty(groups))
                {
                    attrs.SetAttribute("Groups", m_Groups);
                }
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            SetChildGroups();


        }




        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            }

            this.SetAttribute("width", "");

            writer.AddAttribute("class", "search");

            writer.RenderBeginTag(HtmlTextWriterTag.Fieldset);
            {
                //writer.RenderBeginTag(HtmlTextWriterTag.Legend);
                //{
                //    writer.AddAttribute("type", "button");
                //    writer.AddAttribute("onclick", "$('#" + this.GetClientID() + "').toggle(200)");

                //    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                //    {
                //        writer.Write("<img src='/res/icon/zoom2.png' border='0' />查询");
                //    }
                //    writer.RenderEndTag();
                //}
                //writer.RenderEndTag();

                base.Render(writer);

            }
            writer.RenderEndTag();


            StringBuilder sb = new StringBuilder();

            sb.Append("<script type=\"text/javascript\">");

            if (!m_Expand)
            {
                sb.AppendLine("$(document).ready(function(){ $('fieldset.search:first').hide(); });");
            }

            if (!string.IsNullOrEmpty(m_ToggleFnName))
            {
                sb.Append("function " + m_ToggleFnName + "(){");
                sb.Append("  $('fieldset.search:first').slideToggle('normal');");
                sb.AppendLine("}");
            }

            sb.Append("</script>");

            writer.Write(sb.ToString());
        }
    }
}
