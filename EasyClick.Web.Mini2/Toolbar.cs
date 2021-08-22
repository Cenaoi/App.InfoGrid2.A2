using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 工具栏
    /// </summary>
    [Description("工具栏")]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class Toolbar : Panel, ISecControl
    {

        ToolBarItemCollection m_Items;


        /// <summary>
        /// (构造函数)工具栏
        /// </summary>
        public Toolbar()
        {
            this.InReady = "Mini2.ui.toolbar.Toolbar";
            this.JsNamespace = "Mini2.ui.toolbar.Toolbar";
        }

        ///// <summary>
        ///// 显示的区域
        ///// </summary>
        //[DefaultValue(RegionType.North)]
        //public RegionType Region
        //{
        //    get { return m_Region; }
        //    set { m_Region = value; }
        //}



        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public ToolBarItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ToolBarItemCollection();
                }
                return m_Items;
            }
        }


        private void FullScript(StringBuilder sb)
        {
            sb.AppendLine("    var toolbar = Mini2.create('Mini2.ui.toolbar.Toolbar', {");
            JsParam(sb, "clientId", this.ClientID);
            JsParam(sb, "id", this.ID);

            JsParam(sb, "region", this.Region, TextTransform.Lower);


            //固定位置项目
            JsParam(sb, "fixed", this.FixedLayout, false);

            JsParam(sb, "visible", this.Visible, true);

            sb.AppendLine("items:[");

            if (this.Items.Count > 0)
            {
                ToolBarItem tb ;
                string jsItem ;

                for (int i = 0; i < m_Items.Count; i++)
                {
                    tb = m_Items[i];
                    jsItem = tb.GetConfigJS();

                    if (i > 0) { sb.Append(","); }

                    sb.AppendLine(jsItem);
                }
            }

            sb.AppendLine("],");

            sb.AppendFormat("applyTo:'#{0}'", this.ClientID);

            sb.AppendLine("});");

            if (this.IsDelayRender)
            {
                sb.AppendLine("toolbar.delayRender();");
            }
            else
            {
                sb.AppendLine("toolbar.render();");
            }

            sb.AppendFormat("  window.{0} = toolbar;\n", this.ClientID);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = toolbar;\n", this.ID);

        }

        private void Render_DesignMode(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("padding","6px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            foreach (ToolBarItem item in this.Items)
            {
                item.DesignMode = true;
                item.Render(writer);
            }

            writer.RenderEndTag();
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode)
            {
                Render_DesignMode(writer);
                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;


            sb.AppendFormat("<div id=\"{0}\"></div>", this.ClientID);

            ScriptManager script = ScriptManager.GetManager(this.Page);

            if (script != null)
            {
                StringBuilder sbJs = new StringBuilder();

                BeginReady(sbJs);
                FullScript(sbJs);
                EndReady(sbJs);

                script.AddScript(sbJs.ToString());

                writer.Write(sb.ToString());
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


        #region 权限设置


        string m_SecFunCode;
        string m_SecFunID;

        SecControlCollection m_SecControls;

        /// <summary>
        /// 权限模块代码
        /// </summary>
        [Category("Security")]
        [Description("权限模块代码")]
        public string SecFunCode
        {
            get { return m_SecFunCode; }
            set { m_SecFunCode = value; }
        }


        /// <summary>
        /// 权限函数ID 
        /// </summary>
        [Category("Security")]
        [Description("权限函数ID ")]
        public string SecFunID
        {
            get { return m_SecFunID; }
            set { m_SecFunID = value; }
        }

        /// <summary>
        /// 权限组件集合
        /// </summary>
        [Category("Security")]
        [Browsable(false)]
        [Description("权限组件集合")]
        public SecControlCollection SecControls
        {
            get
            {
                if (m_SecControls == null)
                {
                    m_SecControls = new SecControlCollection();

                    foreach (ISecControl item in m_Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        m_SecControls.Add(item);
                    }
                }


                return m_SecControls;
            }
        }

        #endregion
    }
}
