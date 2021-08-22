using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 标签控件
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class TabControl:Control
    {
        HiddenField m_SelectedIndexHid;


        TabHeadCollection m_Heads = new TabHeadCollection();

        TabContentCollection m_Contents;

        string m_OnSelect;

        /// <summary>
        /// 
        /// </summary>
        [Description("tab 改变以后,触发的事件. 客户端 JS 的方法名称")]
        public string OnSelect
        {
            get { return m_OnSelect; }
            set { m_OnSelect = value; }
        }

        public TabControl()
        {
            m_SelectedIndexHid = new HiddenField();
            
            CreateChildControls();

        }

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;


        [DefaultValue(ClientIDMode.AutoID)]
        public new ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }


        /// <summary>
        /// 标签头集合
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("标签头集合")]
        public TabHeadCollection Heads
        {
            get
            {
                if (m_Heads == null)
                {
                    m_Heads = new TabHeadCollection();
                }
                return m_Heads;
            }
        }

        /// <summary>
        /// 标签内容集合
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("标签内容集合")]
        public TabContentCollection Contents
        {
            get
            {
                if (m_Contents == null)
                {
                    m_Contents = new TabContentCollection();
                }
                return m_Contents;
            }
        }

        public int SelectedIndex
        {
            get
            {
                int n = -1;

                if (int.TryParse(m_SelectedIndexHid.Value, out n))
                {
                    return n;
                }

                return -1;
            }
            set
            {
                m_SelectedIndexHid.Value = value.ToString();

                MiniScript ms = MiniScriptManager.ClientScript;

                if (!ms.ReadOnly)
                {
                    //ms.Write("");
                }
            }
        }


                
        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            m_SelectedIndexHid.ID = "SelectedIndex";
            this.Controls.Add(m_SelectedIndexHid);

        }

        private void CreateTabContent()
        {
            if (m_Contents == null)
            {
                return;
            }

            foreach (TabContent item in m_Contents)
            {
                this.Controls.Add(item);
            }
        }



        protected override void Render(HtmlTextWriter writer)
        {

            writer.WriteLine("<div id=\"{0}\" class=\"tabs\" >", GetClientID());
            {
                writer.WriteLine("  <ul>");

                foreach (TabHead tab in m_Heads)
                {
                    Render_TabHeads(writer, tab);
                }

                writer.WriteLine("  </ul>");


                if (m_Contents != null && m_Contents.Count > 0)
                {
                    foreach (TabContent item in m_Contents)
                    {
                        item.RenderControl(writer);
                    }
                }
                else
                {
                    writer.Write("<div id='{0}$empty' style='margin:0; padding:0; border:0; display:none;'>",GetClientID());
                    writer.Write("</div>");
                }

            }
            writer.WriteLine("</div>");

            m_SelectedIndexHid.RenderControl(writer);
            

            Render_Js(writer);
        }

        private void Render_Js(HtmlTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("<script type=\"text/javascript\">");


            writer.WriteLine("$(document).ready(function(){");
            {
                writer.WriteLine("    $(\"#{0}\").tabs({{", GetClientID());
                writer.WriteLine("        select: function (event, ui) {");
                writer.WriteLine("            $(\"#{0}\").val(ui.index);", m_SelectedIndexHid.GetClientID());

                if (!string.IsNullOrEmpty(m_OnSelect))
                {
                    writer.WriteLine(m_OnSelect);
                }

                writer.WriteLine("        }");
                writer.WriteLine("    }).removeClass('ui-widget-content').css('padding', '0px').children('.ui-tabs-panel');");
            }

            writer.WriteLine("});");


            writer.WriteLine("</script>");
        }


        private void Render_TabHeads(HtmlTextWriter writer,TabHead tab)
        {
            writer.Write("    <li ");

            if (tab.Width > 0)
            {
                writer.Write("style=\"width:{0}px;\" ",tab.Width);
            }

            writer.Write(">");

            if (!string.IsNullOrEmpty(tab.Text))
            {
                string targetPanel = tab.TargetPanel;

                if (string.IsNullOrEmpty(targetPanel))
                {
                    targetPanel = string.Format("#{0}$empty", GetClientID());
                }

                writer.Write("<a href=\"{0}\" >{1}</a>", targetPanel, tab.Text);
            }


            writer.WriteLine("</li>");
        }

        
    }

    /// <summary>
    /// 内容集合
    /// </summary>
    public class TabContentCollection : List<TabContent>
    {

    }


    /// <summary>
    /// 标签内容
    /// </summary>
    public class TabContent : Panel
    {

    }

    /// <summary>
    /// 标签头集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TabHeadCollection : List<TabHead>
    {
    }

    /// <summary>
    /// 标签头
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TabHead
    {
        string m_TargetPanel = "";

        string m_Text;

        int m_Width = 0;

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 标题内容
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        public string TargetPanel
        {
            get { return m_TargetPanel; }
            set { m_TargetPanel = value; }
        }


    }
}
