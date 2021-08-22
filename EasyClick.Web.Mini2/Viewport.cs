using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 视窗配置(区域内可配置上下左右)
    /// </summary>
    [Description("视窗配置（区域内可配置上下左右）")]
    [ParseChildren(false)]
    [PersistChildren(true)]
    public class Viewport : Panel
    {
        public Viewport()
        {
            this.InReady = "Mini2.ui.container.Viewport";

            this.JsNamespace = "Mini2.ui.container.Viewport";

            this.Scroll = ScrollBars.None;
        }

        /// <summary>
        /// 是否为主窗体
        /// </summary>
        bool m_Main = true;

        int m_MarginTop = 0;

        /// <summary>
        /// 容器模式
        /// </summary>
        [Description("容器模式")]
        [DefaultValue(false)]
        public bool IsContainer { get; set; } = false;

        /// <summary>
        /// 各个方位之间的间距
        /// </summary>
        [Description("各个方位之间的间距")]
        [DefaultValue(0)]
        public int Space { get; set; }

        /// <summary>
        /// 滚动条
        /// </summary>
        [Description("滚动条")]
        [DefaultValue(ScrollBars.None)]
        public override ScrollBars Scroll
        {
            get
            {
                return base.Scroll;
            }
            set
            {
                base.Scroll = value;
            }
        }

        /// <summary>
        /// Margin top 样式间距
        /// </summary>
        [DefaultValue(0)]
        [Description("Margin top 样式间距")]
        public int MarginTop
        {
            get { return m_MarginTop;}
            set { m_MarginTop = value;}
        }

        /// <summary>
        /// 是否为主窗体
        /// </summary>
        [Description("是否为主窗体")]
        [DefaultValue(true)]
        public bool Main
        {
            get { return m_Main; }
            set { m_Main = value; }
        }

        private List<IPanel> GetPanels()
        {
            List<IPanel> items = new List<IPanel>();

            foreach (Control item in this.Controls)
            {
                if (item is IPanel)
                {
                    items.Add((IPanel)item);
                }
            }

            return items;
        }

        private void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            sb.AppendLine("  var viewport = Mini2.create('" + this.JsNamespace + "', {");

            JsParam(sb, "clientId", clientId);
            JsParam(sb, "space", this.Space);

            JsParam(sb, "isContainer", this.IsContainer,false);


            sb.AppendFormat("    scroll: '{0}', \n", this.Scroll.ToString().ToLower());

            if (m_MarginTop > 0)
            {
                sb.AppendLine("    margin:{");

                sb.AppendFormat("      top:{0}", this.m_MarginTop);

                sb.AppendLine("    },");
            }

            sb.AppendFormat("    contentEl: '#{0}',\n", clientId);

            sb.Append("    items: [");

            List<IPanel> panels = GetPanels();

            if (panels.Count > 0)
            {

                IPanel con = panels[0];

                sb.Append("{\n");
                sb.AppendLine("        isAfterRender: true,");
                sb.AppendLine("        id: '" + con.ClientID + "'");
                sb.Append("    }");

                for (int i = 1; i < panels.Count; i++)
                {
                    sb.Append(",");

                    con = panels[i] ;

                    sb.Append("{\n");
                    sb.AppendLine("        isAfterRender: true,");
                    sb.AppendLine("        id: '" + con.ClientID + "'");
                    sb.Append("    }");
                }

            }

            sb.AppendLine("]");
            
            sb.AppendLine("  });");

            sb.AppendLine("  viewport.render();");

            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = viewport;\n", this.ID);
            sb.AppendFormat("  window.{0} = viewport;\n", clientId);

            if (m_Main)
            {
                sb.AppendLine("        $(document.body).css('overflow', 'hidden').attr('scroll', 'no');");
            }

        }

        
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.Write("<div style='width:100%'>");

                foreach (Control con in this.Controls)
                {
                    con.RenderControl(writer);
                }

                writer.Write("</div>");

                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;


            ScriptManager script = ScriptManager.GetManager(this.Page);

            if (script != null)
            {

                StringBuilder sbJs = new StringBuilder();

                BeginReady(sbJs);
                FullScript(sbJs);
                EndReady(sbJs);

                script.AddScript(sbJs);
                

                writer.Write(sb.ToString());

                writer.Write("<div id=\"{0}\">", this.ClientID);
                //writer.Write("  <div class=\"mi-viewport-inner\" >");

                foreach (Control con in this.Controls)
                {
                    con.RenderControl(writer);
                }

                //writer.Write("   </div>");
                writer.Write("</div>");
            }
            else
            {

                BeginScript(sb);
                BeginReady(sb);

                FullScript(sb);

                EndReady(sb);
                EndScript(sb);

                writer.Write(sb.ToString());
            }


        }
    }
}
