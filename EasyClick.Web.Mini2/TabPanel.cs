using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Web.UI;
using System.Security.Permissions;
using System.IO;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// Tab 版面
    /// </summary>
    [Description("Tab 版面")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    public class TabPanel:Component, IPanel, EasyClick.Web.Mini.IMiniControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// (构造函数)Tab版面构造函数
        /// </summary>
        public TabPanel()
        {

        }

        /// <summary>
        /// Tab 页集合
        /// </summary>
        TabCollection m_Items = new TabCollection();

        int m_Width = 0;

        int m_Height = 0;

        bool m_Plain = false;

        /// <summary>
        /// 隐藏 Panel 的按钮
        /// </summary>
        bool m_ButtonVisible = true;


        #region Padding

        int m_Padding = 0;

        int m_PaddingLeft = 0;
        int m_PaddingRight = 0;
        int m_PaddingTop = 0;
        int m_PaddingBottom = 0;

        /// <summary>
        /// 间距.子控件样式 padding
        /// </summary>
        [Description("间距.子控件样式 padding")]
        [DefaultValue(0)]
        [Category("Padding")]
        public int Padding
        {
            get { return m_Padding; }
            set { m_Padding = value; }
        }

        /// <summary>
        /// 间距.子控件样式 padding-left
        /// </summary>
        [Description("间距.子控件样式 padding-left")]
        [DefaultValue(0)]
        [Category("Padding")]
        public int PaddingLeft
        {
            get { return m_PaddingLeft; }
            set { m_PaddingLeft = value; }
        }


        /// <summary>
        /// 间距.子控件样式 padding-right
        /// </summary>
        [Description("间距.子控件样式 padding-right")]
        [DefaultValue(0)]
        [Category("Padding")]
        public int PaddingRight
        {
            get { return m_PaddingRight; }
            set { m_PaddingRight = value; }
        }

        /// <summary>
        /// 间距.子控件样式 padding-top
        /// </summary>
        [Description("间距.子控件样式 padding-top")]
        [DefaultValue(0)]
        [Category("Padding")]
        public int PaddingTop
        {
            get { return m_PaddingTop; }
            set { m_PaddingTop = value; }
        }

        /// <summary>
        /// 间距.子控件样式 padding-bottom
        /// </summary>
        [Description("间距.子控件样式 padding-bottom")]
        [DefaultValue(0)]
        [Category("Padding")]
        public int PaddingBottom
        {
            get { return m_PaddingBottom; }
            set { m_PaddingBottom = value; }
        }

        #endregion

        /// <summary>
        /// 显示状态: normal-普通,min-最小化, max-最大化
        /// </summary>
        public WindowState State { get; set; } = WindowState.Normal;

        /// <summary>
        /// 版面所属区域
        /// </summary>
        RegionType m_Region = RegionType.North;

        /// <summary>
        /// UI 配合样式使用
        /// </summary>
        [Description("UI 配合样式使用")]
        [DefaultValue("default")]
        public string UI { get; set; } = "default";

        /// <summary>
        /// 隐藏 Panel 的按钮
        /// </summary>
        [DefaultValue(true)]
        public bool ButtonVisible
        {
            get { return m_ButtonVisible; }
            set { m_ButtonVisible = value; }
        }

        /// <summary>
        /// 版面所属区域,配合 Viewport 控件使用。
        /// </summary>
        [DefaultValue(RegionType.North)]
        [Description("版面所属区域,配合 Viewport 控件使用。")]
        public RegionType Region
        {
            get { return m_Region; }
            set { m_Region = value; }
        }

        /// <summary>
        /// 标签加上背景
        /// </summary>
        [DefaultValue(false)]
        [Description("标签加上背景")]
        public bool Plain
        {
            get { return m_Plain; }
            set { m_Plain = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [DefaultValue(0)]
        [Description("宽度")]
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }


        /// <summary>
        /// 高度
        /// </summary>
        [DefaultValue(0)]
        [Description("高度")]
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// Tab 标签起始位置
        /// </summary>
        [DefaultValue(0)]
        [Description("Tab 标签起始位置")]
        public int TabLeft { get; set; } = 0;


        /// <summary>
        /// Tabs 页集合
        /// </summary>
        [Description("Tabs 页集合")]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public TabCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new TabCollection();
                }
                return m_Items;
            }
        }
        


        public override bool HasControls()
        {
            return (m_Items != null && m_Items.Count > 0);
        }

        protected void FullScript_Padding(StringBuilder sb)
        {
            if (m_Padding == 0 &&
                m_PaddingBottom == 0 &&
                m_PaddingTop == 0 &&
                m_PaddingLeft == 0 &&
                m_PaddingRight == 0)
            {
                return;
            }

            sb.AppendLine("    padding: {");
            {
                JsParam(sb, "  left", this.PaddingLeft > 0 ? this.PaddingLeft : this.Padding);
                JsParam(sb, "  top", this.PaddingTop > 0 ? this.PaddingTop : this.Padding);
                JsParam(sb, "  right", this.PaddingRight > 0 ? this.PaddingRight : this.Padding);
                JsParam(sb, "  bottom", this.PaddingBottom > 0 ? this.PaddingBottom : this.Padding);

                sb.AppendLine("      _:0");
                sb.AppendLine("    },");
            }
        }


        private void FullScript(StringBuilder sb)
        {

            sb.AppendLine("  var tabPanel = Mini2.create('Mini2.ui.tab.Panel', {");

            JsParam(sb, "id", this.ID);
            JsParam(sb, "applyTo", "#" + this.ClientID);

            JsParam(sb, "ui", this.UI, "default");

            JsParam(sb, "plain", this.Plain,false);

            JsParam(sb, "width", this.Width);
            JsParam(sb, "height", this.Height);



            JsParam(sb, "dock", this.Dock, TextTransform.Lower);
            JsParam(sb, "region", this.Region, TextTransform.Lower);

            JsParam(sb, "buttonVisible", this.ButtonVisible, true);

            JsParam(sb, "state", this.State, WindowState.Normal);
            JsParam(sb, "tabLeft", this.TabLeft, 0);

            //sb.AppendLine("    isTabPanel:true");
            FullScript_Padding(sb);

            sb.Append("    items:[");

            for (int i = 0; i < this.Items.Count; i++)
            {
                Tab tab = this.Items[i];

                if (i > 0) { sb.Append(","); }

                sb.AppendLine("{");

                sb.Append("      ");
                sb.AppendFormat("text:'{0}', ", tab.Text);
                sb.AppendFormat("id:'{0}', ", tab.ID);
                sb.AppendFormat("scroll:'{0}', ", tab.Scroll.ToString().ToLower());

                if (tab.Closable)
                {
                    sb.Append("closable: true, ");
                }

                if (tab.IsDelayRender)
                {
                    sb.Append("isDelayRender:true, ");
                }

                sb.AppendFormat("contentEl:'#{0}'", tab.ClientID); 

                sb.Append("\n    }");
            }

            sb.AppendLine("]");

            sb.AppendLine("  });");

            sb.AppendLine("  tabPanel.render();");

            sb.AppendFormat("  window.{0} = tabPanel;\n", this.ClientID);



            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = tabPanel;\n", this.ID);


        }

        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            base.CreateChildControls();

            foreach (Tab tab in this.Items)
            {
                if(tab == null)
                {
                    log.Error("Tab 异常");
                    continue;
                }

                this.Controls.Add(tab);
            }

        }



        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            EnsureChildControls();

            ScriptManager script = ScriptManager.GetManager(this.Page);



            if (script != null)
            {

                StringBuilder jsSb = new StringBuilder();


                BeginReady(jsSb);

                FullScript(jsSb);

                EndReady(jsSb);

                script.AddScript(jsSb.ToString());
            }
            else
            {

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);

                //RenderClientScript(htmlWriter);

                writer.Write(sw.ToString());

                sw.Dispose();
            }



            writer.WriteLine("  <div id='{0}' style=''>", this.ClientID);

            foreach (Tab tab in this.Items)
            {
                writer.WriteLine("    <div id='{0}'>", tab.ClientID);

                //tab.RenderControl(writer);

                if (tab.IFrame)
                {
                    writer.WriteLine($"<iframe src='{tab.Url}' dock='full' style='border:none;' ></iframe>");
                }
                else
                {
                    foreach (Control item in tab.Controls)
                    {
                        item.RenderControl(writer);
                    }
                }

                writer.WriteLine();
                writer.WriteLine("    </div>");
            }

            writer.WriteLine("  </div>");

        }



        public override Control FindControl(string id)
        {
            if (m_Items == null || m_Items.Count == 0)
            {
                return null;
            }


            Control con = null;

            
            foreach (var item in m_Items)
            {
                con = item.FindControl(id);

                if (con != null)
                {
                    break;
                }
            }

            return con;
        }


        public void LoadPostData()
        {

        }
    }
}
