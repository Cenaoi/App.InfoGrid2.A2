using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using EC5.Utility;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 版面接口
    /// </summary>
    public interface IPanel
    {
        string ClientID { get; }
    }

    /// <summary>
    /// 版面
    /// </summary>
    [ParseChildren(false)]
    [PersistChildren(true)]
    [Description("版面")]
    public class Panel : WebControl,IPanel,IDelayRender
    {
        /// <summary>
        /// (构造函数)版面
        /// </summary>
        public Panel()
        {
            this.InReady = "Mini2.ui.panel.Panel";

            this.JsNamespace = "Mini2.ui.panel.Panel";

            this.ItemClass = "mi-box-item";
        }

        /// <summary>
        /// (构造函数)版面
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Panel(Unit width, Unit height)
        {
            this.InReady = "Mini2.ui.panel.Panel";

            this.JsNamespace = "Mini2.ui.panel.Panel";

            this.ItemClass = "mi-box-item";

            this.Width = width;
            this.Height = height;
        }

        string m_InReady;
        string m_JsNamespace;

        bool m_Visible = true;

        LayoutStyle m_Layout = LayoutStyle.Auto;
        UiStyle m_UiStyle = UiStyle.Default;

        /// <summary>
        /// 文本
        /// </summary>
        string m_Text;

        /// <summary>
        /// 容器内容水平对齐
        /// </summary>
        HorizontalAlign m_Align = HorizontalAlign.Left;

        /// <summary>
        /// 固定位置布局
        /// </summary>
        bool m_FixedLayout = false;

        /// <summary>
        /// 控件布局方向
        /// </summary>
        FlowDirection m_FlowDirection = FlowDirection.LeftToRight;
        /// <summary>
        /// 自动换行
        /// </summary>
        bool m_WrapContents = true;


        #region ItemMargin

        string m_ItemMargin ;
        string m_ItemMarginLeft ;
        string m_ItemMarginRight ;
        string m_ItemMarginTop ;
        string m_ItemMarginBottom;

        /// <summary>
        /// 子控件样式 margin
        /// </summary>
        [DefaultValue(0)]
        [Category("ItemMargin")]
        [Description("子控件样式 margin")]
        public string ItemMargin
        {
            get { return m_ItemMargin; }
            set { m_ItemMargin = value; }
        }



        /// <summary>
        /// 子控件样式 margin-left
        /// </summary>
        [DefaultValue(0)]
        [Category("ItemMargin")]
        [Description("子控件样式 margin-left")]
        public string ItemMarginLeft
        {
            get { return m_ItemMarginLeft; }
            set { m_ItemMarginLeft = value; }
        }

        /// <summary>
        /// 子控件样式 margin-right
        /// </summary>
        [DefaultValue(0)]
        [Category("ItemMargin")]
        [Description("子控件样式 margin-right")]
        public string ItemMarginRight
        {
            get { return m_ItemMarginRight; }
            set { m_ItemMarginRight = value; }
        }

        /// <summary>
        /// 子控件样式 margin-top
        /// </summary>
        [Description("子控件样式 margin-top")]
        [DefaultValue(0)]
        [Category("ItemMargin")]
        public string ItemMarginTop
        {
            get { return m_ItemMarginTop; }
            set { m_ItemMarginTop = value; }
        }

        /// <summary>
        /// 子控件样式 margin-bottom
        /// </summary>
        [Description("子控件样式 margin-bottom")]
        [DefaultValue(0)]
        [Category("ItemMargin")]
        public string ItemMarginBottom
        {
            get { return m_ItemMarginBottom; }
            set { m_ItemMarginBottom = value; }
        }

        #endregion


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


        #region margin

        string m_Margin;
        string m_MarginTop;
        string m_MarginBottom;
        string m_MarginLeft;
        string m_MarginRight;

        /// <summary>
        /// 空白间距
        /// </summary>
        [DefaultValue("")]
        public string Margin
        {
            get { return m_Margin; }
            set { m_Margin = value; }
        }

        [DefaultValue("")]
        public string MarginLeft
        {
            get { return m_MarginLeft; }
            set { m_MarginLeft = value; }
        }

        [DefaultValue("")]
        public string MarginRight
        {
            get { return m_MarginRight; }
            set { m_MarginRight = value; }
        }

        [DefaultValue("")]
        public string MarginTop
        {
            get { return m_MarginTop; }
            set { m_MarginTop = value; }
        }

        [DefaultValue("")]
        public string MarginBottom
        {
            get { return m_MarginBottom; }
            set { m_MarginBottom = value; }
        }
        


        #endregion


        /// <summary>
        /// 自动调节大小
        /// </summary>
        bool m_AutoSize = false;

        string m_MinWidth ;
        string m_MinHeight;

        string m_MaxWidth;

        string m_MaxHeight;


        /// <summary>
        /// 控件布局方向
        /// </summary>
        [Description("控件布局方向")]
        [DefaultValue(FlowDirection.LeftToRight)]
        public virtual FlowDirection FlowDirection
        {
            get { return m_FlowDirection; }
            set { m_FlowDirection = value; }
        }

        /// <summary>
        /// 自动换行
        /// </summary>
        [Description("自动换行")]
        [DefaultValue(true)]
        public bool WrapContents
        {
            get { return m_WrapContents; }
            set { m_WrapContents = value; }
        }

        /// <summary>
        /// 可关闭的 Panel
        /// </summary>
        [Description("可关闭的 Panel")]
        [DefaultValue(false)]
        public bool Closable { get; set; } = false;

        /// <summary>
        /// 最小宽度
        /// </summary>
        [Description("最小宽度")]
        [DefaultValue(null)]
        public string MinWidth
        {
            get { return m_MinWidth; }
            set { m_MinWidth = value; }
        }

        /// <summary>
        /// 最小高度
        /// </summary>
        [Description("最小高度")]
        [DefaultValue(null)]
        public string MinHeight
        {
            get { return m_MinHeight; }
            set { m_MinHeight = value; }
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [Description("最大宽度")]
        [DefaultValue(null)]
        public string MaxWidth
        {
            get { return m_MaxWidth; }
            set { m_MaxWidth = value; }
        }

        /// <summary>
        /// 最大高度
        /// </summary>
        [Description("最大高度")]
        [DefaultValue(null)]
        public string MaxHeight
        {
            get { return m_MaxHeight;  }
            set { m_MaxHeight = value; }
        }




        /// <summary>
        /// 位置状态
        /// </summary>
        [DefaultValue(StylePosition.Static)]
        public StylePosition Position { get; set; } = StylePosition.Static;


        #region  坐标

        /// <summary>
        /// 左边
        /// </summary>
        [DefaultValue("")]
        public string Left { get; set; }


        /// <summary>
        /// 顶部
        /// </summary>
        [DefaultValue("")]
        public string Top { get; set; }

        /// <summary>
        /// 右边
        /// </summary>
        public string Right { get; set; }

        /// <summary>
        /// 底部
        /// </summary>
        public string Bottom { get; set; }

        #endregion


        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        [DefaultValue("")]
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }


        /// <summary>
        /// 自动改变尺寸
        /// </summary>
        [Description("自动改变尺寸")]
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return m_AutoSize; }
            set { m_AutoSize = value; }
        }

        /// <summary>
        /// 固定位置布局.
        /// </summary>
        [DefaultValue(false)]
        [Description("固定位置布局")]
        public bool FixedLayout
        {
            get { return m_FixedLayout; }
            set { m_FixedLayout = value; }
        }

        /// <summary>
        /// 可视
        /// </summary>
        [DefaultValue(true)]
        [Description("可视")]
        public new bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = value;

                if (!this.DesignMode)
                {
                    EasyClick.Web.Mini.MiniHelper.EvalFormat("if({0} && {0}.setVisible){{ {0}.setVisible({1}); }};", this.ClientID, value.ToString().ToLower());
                }
            }
        }

        /// <summary>
        /// 水平对齐
        /// </summary>
        [DefaultValue(HorizontalAlign.Left)]
        [Description("水平对齐")]
        public HorizontalAlign Align
        {
            get { return m_Align; }
            set { m_Align = value; }
        }

        string m_OnResize;

        /// <summary>
        /// 触发事件
        /// </summary>
        [Description("触发事件")]
        [DefaultValue("")]
        public string OnResize
        {
            get { return m_OnResize; }
            set { m_OnResize = value; }
        }

        /// <summary>
        /// 对应的 js class
        /// </summary>
        [Browsable(false)]
        protected string InReady
        {
            get { return m_InReady; }
            set { m_InReady = value; }
        }

        /// <summary>
        /// 对应的 js class
        /// </summary>
        [Browsable(false)]
        protected string JsNamespace
        {
            get { return m_JsNamespace; }
            set { m_JsNamespace = value; }
        }


        /// <summary>
        /// UI 样式
        /// </summary>
        [DefaultValue(UiStyle.Default)]
        [Description("UI 样式")]
        public UiStyle Ui
        {
            get { return m_UiStyle; }
            set { m_UiStyle = value; }
        }

        /// <summary>
        /// 布局样式
        /// </summary>
        [DefaultValue(LayoutStyle.Auto)]
        [Description("布局样式")]
        public LayoutStyle Layout
        {
            get { return m_Layout; }
            set { m_Layout = value; }
        }

        /// <summary>
        /// 客户端脚本模式
        /// </summary>
        [Browsable(false)]
        [DefaultValue(JavascriptMode.Auto)]
        [Description("客户端脚本模式")]
        public JavascriptMode JavascriptMode
        {
            get
            {
                JavascriptMode jsMode = JavascriptMode.Auto;

                HttpContext context = HttpContext.Current;

                if (context != null && context.Items.Contains("JS_MODE"))
                {
                    string jsModeStr = (string)context.Items["JS_MODE"];

                    jsMode = EnumUtil.Parse<JavascriptMode>(jsModeStr, true);
                }

                return jsMode;
            }
        }

        /// <summary>
        /// 开始输出 <script type=\"text/javascript\">
        /// </summary>
        /// <param name="sb"></param>
        protected void BeginScript(StringBuilder sb)
        {
            sb.AppendLine("<script type=\"text/javascript\">");
        }

        /// <summary>
        /// 开始输出 $(document).ready(function(){
        /// </summary>
        /// <param name="sb"></param>
        protected void BeginReady(StringBuilder sb)
        {

            JavascriptMode jsMode = this.JavascriptMode;

            //if (jsMode == JavascriptMode.MInJs2)
            //{
            //    sb.Append("In.ready('" + this.m_InReady + "', function () {").AppendLine();
            //}
            //else
            //{
                sb.AppendLine("$(document).ready(function(){");
            //}
        }

        /// <summary>
        /// 开始输出
        /// </summary>
        /// <param name="sb"></param>
        protected void EndReady(StringBuilder sb)
        {
            sb.AppendLine("});");
        }

        /// <summary>
        /// 开始输出  </script>
        /// </summary>
        /// <param name="sb"></param>
        protected void EndScript(StringBuilder sb)
        {
            sb.AppendLine("</script>");
        }


        #region Render_JsParam

        protected void JsParam(StringBuilder sb, string mame, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string mame, int value)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string mame, decimal value, decimal defaultValue)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string mame, decimal value)
        {
            if (value == 0)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", mame, value).AppendLine();
        }


        protected void JsParam(StringBuilder sb, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            sb.AppendFormat("    {0}: '{1}',", name, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, string value, string defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: '{1}',", name, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, bool value, bool defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            sb.AppendFormat("    {0}: {1},", name, value.ToString().ToLower()).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, Enum value, Enum defaultValue)
        {
            JsParam(sb, name, value, defaultValue, TextTransform.None);
        }

        protected void JsParam(StringBuilder sb, string name, Enum value,  TextTransform textTransform)
        {

            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            sb.AppendFormat("    {0}: '{1}',", name, valueStr).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, Enum value, Enum defaultValue, TextTransform textTransform)
        {
            if (value == defaultValue)
            {
                return;
            }

            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            sb.AppendFormat("    {0}: '{1}',", name, valueStr).AppendLine();
        }

        #endregion

        string m_Width = "100%";

        string m_Height;

        DockStyle m_Dock = DockStyle.Top;
        RegionType m_Region = RegionType.North;
        ScrollBars m_ScrollMode = ScrollBars.Auto;

        /// <summary>
        /// 停靠方位
        /// </summary>
        [Description("停靠方位")]
        [DefaultValue(DockStyle.Top)]
        public DockStyle Dock
        {
            get { return m_Dock; }
            set { m_Dock = value; }
        }

        /// <summary>
        /// 版面所属区域,配合 Viewport 控件使用。
        /// </summary>
        [DefaultValue(RegionType.North)]
        [Description("版面所属区域,配合 Viewport 控件使用。停靠方位")]
        public RegionType Region
        {
            get { return m_Region; }
            set { m_Region = value; }
        }



        /// <summary>
        /// 滚动条模式
        /// </summary>
        [DefaultValue(ScrollBars.Auto)]
        [Description("滚动条模式")]
        public virtual ScrollBars Scroll
        {
            get { return m_ScrollMode; }
            set { m_ScrollMode = value; }
        }

        

        string m_ItemClass;

        /// <summary>
        /// 子项附加样式名
        /// </summary>
        [Description("子项附加样式名")]
        public string ItemClass
        {
            get { return m_ItemClass; }
            set { m_ItemClass = value; }
        }



        /// <summary>
        /// 查找 ControT 控件
        /// </summary>
        /// <typeparam name="ControlT"></typeparam>
        /// <returns></returns>
        public List<ControlT> FindBy<ControlT>()
        {
            List<ControlT> cons = new List<ControlT>();

            foreach (var item in this.Controls)
            {
                if (item is ControlT )
                {
                    cons.Add((ControlT)item);
                }
            }

            return cons;
        }


        /// <summary>
        /// 根据字段名获取
        /// </summary>
        /// <param name="dataField">字段名</param>
        /// <returns></returns>
        public Control FindByDbField(string dataField)
        {
            Control con = null;

            foreach (var item in this.Controls)
            {
                if (item is FieldBase)
                {
                    FieldBase fb = (FieldBase)item;

                    if(dataField == fb.DataField)
                    {
                        con = fb;
                        break;
                    }
                }
            }

            return con;
        }

        /// <summary>
        /// 根据类型查找控件
        /// </summary>
        /// <param name="controlType"></param>
        /// <returns></returns>
        public List<Control> FindBy(Type controlType)
        {
            if (controlType == null)
            {
                throw new ArgumentNullException("controlType");
            }

            List<Control> cons = new List<Control>();

            foreach (Control item in this.Controls)
            {
                bool isIMyInterface = controlType.IsAssignableFrom(item.GetType());

                if (isIMyInterface)
                {
                    cons.Add(item);
                }
            }

            return cons;
        }


        /// <summary>
        /// 填充脚本
        /// </summary>
        /// <param name="sb"></param>
        protected void FullScript_Layout(ScriptTextWriter st)
        {

            st.RenderBengin("layout");
            {
                st.WriteParam("itemCls", m_ItemClass);
                st.WriteParam("align", this.Align, HorizontalAlign.Left, TextTransform.Lower);
                st.WriteParam("type", this.Layout, TextTransform.Lower);
            }
            st.RenderEnd();
        }

        /// <summary>
        /// 填充脚本
        /// </summary>
        /// <param name="st"></param>
        protected void FullScript_ItemMargin(ScriptTextWriter st)
        {
            if (StringUtil.IsBlank(m_ItemMarginLeft, m_ItemMarginRight, m_ItemMarginTop, m_ItemMarginBottom))
            {
                return;
            }

            st.RenderBengin("itemMargin");
            {
                st.WriteParam("left", this.ItemMarginLeft);
                st.WriteParam("top", this.ItemMarginTop);
                st.WriteParam("right", this.ItemMarginRight);
                st.WriteParam("bottom", this.ItemMarginBottom);
            }
            st.RenderEnd();
        }



        protected void FullScript_Padding(ScriptTextWriter st)
        {
            if (StructUtil.IsZero(m_Padding, m_PaddingBottom, m_PaddingLeft, m_PaddingRight, m_PaddingTop))
            {
                return;
            }

            st.RenderBengin("padding");
            {
                st.WriteParam("left", this.PaddingLeft > 0 ? this.PaddingLeft : this.Padding);
                st.WriteParam("top", this.PaddingTop > 0 ? this.PaddingTop : this.Padding);
                st.WriteParam("right", this.PaddingRight > 0 ? this.PaddingRight : this.Padding);
                st.WriteParam("bottom", this.PaddingBottom > 0 ? this.PaddingBottom : this.Padding);

            }
            st.RenderEnd();
        }


        /// <summary>
        /// 填充脚本
        /// </summary>
        /// <param name="sb"></param>
        protected virtual void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);
            st.RetractBengin();

            st.RetractBengin("var panel = Mini2.create('" + this.JsNamespace + "', {");
            {
                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", clientId);

                FullScript_Layout(st);
                FullScript_ItemMargin(st);
                FullScript_Padding(st);

                st.WriteParam("margin", this.Margin);
                st.WriteParam("marginLeft", this.MarginLeft);
                st.WriteParam("marginRight", this.MarginRight);
                st.WriteParam("marginTop", this.MarginTop);
                st.WriteParam("marginBottom", this.MarginBottom);

                st.WriteParam("userCls", this.CssClass);
                st.WriteParam("autoSize", this.AutoSize, false);

                st.WriteParam("width", this.Width.ToString());
                st.WriteParam("height", this.Height.ToString());

                st.WriteParam("minWidth", m_MinWidth);
                st.WriteParam("minHeight", m_MinHeight);

                st.WriteParam("maxWidth", m_MaxWidth);
                st.WriteParam("maxHeight", m_MaxHeight);

                st.WriteParam("position", this.Position, StylePosition.Static, TextTransform.Lower);

                st.WriteParam("left", this.Left);
                st.WriteParam("top", this.Top);
                st.WriteParam("right", this.Right);
                st.WriteParam("bottom", this.Bottom);

                st.WriteParam("flowDirection", this.FlowDirection, FlowDirection.LeftToRight, TextTransform.Lower);
                st.WriteParam("wrapContents", this.WrapContents, true);

                st.WriteParam("scroll", this.Scroll, ScrollBars.Auto, TextTransform.Lower);
                st.WriteParam("ui", this.Ui, UiStyle.Default, TextTransform.Lower);
                st.WriteParam("dock", this.Dock, TextTransform.Lower);
                st.WriteParam("region", this.Region, TextTransform.Lower);

                //固定位置项目
                st.WriteParam("fixed", this.FixedLayout, false);

                st.WriteParam("closable", this.Closable, false);

                //if (string.IsNullOrEmpty(m_OnResize))
                //{
                //    sb.AppendFormat("resizeFun:{0},\n", m_OnResize);
                //}
                st.WriteParam("visible", this.Visible, true);

                st.WriteParam("contentEl", "#" + clientId);
                
            }
            st.RetractEnd("});");

            if (m_IsDelayRender)
            {
                st.WriteCodeLine("panel.delayRender();");
            }
            else
            {
                st.WriteCodeLine("panel.render();");
            }
            
            st.RetractBengin("panel.resize(function(){");
            st.WriteCodeLine(m_OnResize);
            st.RetractEnd("});");

            st.WriteCodeLine($"window.{clientId} = panel;");

            st.WriteCodeLine($"Mini2.onwerPage.controls['{this.ID}'] = panel;");
            st.RetractEnd();
        }


        protected override void Render(HtmlTextWriter writer)
        {


            if (this.DesignMode)
            {
                foreach (Control item in this.Controls)
                {
                    item.RenderControl(writer);
                }

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

                script.AddScript(sbJs.ToString());

                writer.Write(sb.ToString());

                writer.Write("<div id=\"{0}\" ", this.ClientID);

                foreach (string attrName in this.Attributes.Keys)
                {
                    writer.Write("{0}=\"{1}\" ", attrName, this.Attributes[attrName]);
                }

                writer.Write(">");

                foreach (Control con in this.Controls)
                {
                    con.RenderControl(writer);
                }
                
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


        /// <summary>
        /// 延迟显示
        /// </summary>
        bool m_IsDelayRender = false;

        /// <summary>
        /// 延迟显示
        /// </summary>
        [Description("延迟显示")]
        [DefaultValue(false)]
        public bool IsDelayRender
        {
            get { return m_IsDelayRender; }
            set { m_IsDelayRender = value; }
        }
        
    }
}
