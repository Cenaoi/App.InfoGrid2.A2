using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 样式
    /// </summary>
    public enum StylePosition
    {
        Static ,
        Relative ,
        Absolute ,
        Fixed ,
        Center ,
        Page ,
        Sticky
    }

    /// <summary>
    /// 帮助提示信息的位置
    /// </summary>
    public enum HelperLayouts
    {
        /// <summary>
        /// 顶部
        /// </summary>
        Top,
        /// <summary>
        /// 右边
        /// </summary>
        Rigth,
        /// <summary>
        /// 底部
        /// </summary>
        Bottom
    }

    /// <summary>
    /// 帮助样式
    /// </summary>
    public enum HelperStyles
    {
        /// <summary>
        /// 全部显示
        /// </summary>
        IconText,
        /// <summary>
        /// 文本内容
        /// </summary>
        Text,
        /// <summary>
        /// 图标,例如一个问号的图标
        /// </summary>
        Icon
    }

    /// <summary>
    /// 表单控件的基础类
    /// </summary>
    [DefaultProperty("Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public abstract class FieldBase :Control, IAttributeAccessor, IDelayRender
    {
        string m_Name;

        string m_InReady = "Mini2.ui.form.field.Text";

        string m_JsNamaspace = "Mini2.ui.form.field.Text";

        string m_Width = "100%";

        string m_MinWidth;

        string m_MaxWidth;

        /// <summary>
        /// 自动切掉左右空格
        /// </summary>
        bool m_AutoTrim = true;

        string m_Tag;

        /// <summary>
        /// 只读
        /// </summary>
        bool m_ReadOnly = false;

        TextAlign m_BodyAlign = TextAlign.Left;


        DockStyle m_Dock = DockStyle.None;

        /// <summary>
        /// 设备类型
        /// </summary>
        DeviceTypes m_DeviceType = DeviceTypes.Auto;


        /// <summary>
        /// 输入提示
        /// </summary>
        Typeahead m_Typeahead;

        #region 帮助信息

        /// <summary>
        /// 隐藏帮助
        /// </summary>
        bool m_HideHelper = false;

        /// <summary>
        /// 帮助信息
        /// </summary>
        string m_HelperText;

        /// <summary>
        /// 帮助的版面位置
        /// </summary>
        HelperLayouts m_HelperLayout = HelperLayouts.Rigth;

        /// <summary>
        /// 帮助样式
        /// </summary>
        HelperStyles m_HelperStyle = HelperStyles.IconText;

        #endregion

        /// <summary>
        /// 子项模式
        /// </summary>
        [DefaultValue(false)]
        public bool SubItemMode { get;  set; } = false;

        /// <summary>
        /// 脚本
        /// </summary>
        internal List<string> SubScript { get; set; }
        


        string m_Style;


        bool m_Visible = true;

        /// <summary>
        /// 弄脏
        /// </summary>
        bool m_Dirty = false;

        bool m_TabStop = true;

        /// <summary>
        /// 输入提示的参数
        /// </summary>
        ParamCollection m_TypeaheadParams;

        /// <summary>
        /// 权限函数编码
        /// </summary>
        public string SecFunCode { get; set; }

        /// <summary>
        /// 只读权限设置
        /// </summary>
        public string SecReadonly { get; set; }


        /// <summary>
        /// 设备类型
        /// </summary>
        [DefaultValue(DeviceTypes.Auto)]
        public DeviceTypes DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }

        /// <summary>
        /// 输入提示
        /// </summary>
        [Description("输入提示")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Typeahead Typeahead
        {
            get
            {
                if (m_Typeahead == null)
                {
                    m_Typeahead = new Typeahead();
                }
                return m_Typeahead;
            }
        }



        /// <summary>
        /// 输入提示的参数
        /// </summary>
        [Description("输入提示的参数")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParamCollection TypeaheadParams
        {
            get
            {
                if(m_TypeaheadParams == null)
                {
                    m_TypeaheadParams = new ParamCollection();
                }
                return m_TypeaheadParams;
            }
        }


        MapItemCollection m_TypeaheadMapItems;

        /// <summary>
        /// 映射集合
        /// </summary>
        [Description("快速录入的映射映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public virtual MapItemCollection TypeaheadMapItems
        {
            get
            {
                if (m_TypeaheadMapItems == null)
                {
                    m_TypeaheadMapItems = new MapItemCollection();
                }
                return m_TypeaheadMapItems;
            }
        }


        /// <summary>
        /// 存在快速录入的映射
        /// </summary>
        /// <returns></returns>
        public bool HasTypeaheadMapItems()
        {
            return m_TypeaheadMapItems != null && m_TypeaheadMapItems.Count > 0;
        }


        MapItemCollection m_MapItems;

        /// <summary>
        /// 映射集合
        /// </summary>
        [Description("映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public virtual MapItemCollection MapItems
        {
            get
            {
                if (m_MapItems == null)
                {
                    m_MapItems = new MapItemCollection();
                }
                return m_MapItems;
            }
        }


        /// <summary>
        /// 存在映射对象
        /// </summary>
        /// <returns></returns>
        public bool HasMapItems()
        {
            return m_MapItems != null && m_MapItems.Count > 0;
        }

        /// <summary>
        /// 存在输入提示
        /// </summary>
        /// <returns></returns>
        public bool HasTypeahead()
        {
            return m_Typeahead != null && m_Typeahead.Enabled;
        }

        /// <summary>
        /// 用户自定义样式
        /// </summary>
        public string Style
        {
            get { return m_Style; }
            set { m_Style = value; }
        }

        /// <summary>
        /// 值发生变化的回调函数
        /// </summary>
        public event CallbackEventHandler ChangedCallback;


        /// <summary>
        /// 触发回调函数
        /// </summary>
        protected void OnChangedCallback(string data)
        {
            ChangedCallback?.Invoke(this, data);
        }


        /// <summary>
        /// 值发生变化的回调函数 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldControl"></param>
        protected void ChangedCallbackScript(StringBuilder code, string fieldControl)
        {

            if (this.ChangedCallback != null)
            {
                string method = this.ChangedCallback.Method.Name;

                code.AppendLine($"{fieldControl}.bind('changed_callback',function(e){{");

                code.AppendLine("    widget1.submit('form', {");
                code.AppendLine("        RMode:'callback', ");
                code.AppendLine($"        action: '{method}', ");
                code.AppendLine($"        actionPs: e ");

                code.AppendLine("    });");

                code.AppendLine("});");
            }
        }

        /// <summary>
        /// 自动去掉左右空格
        /// </summary>
        [Description("自动去掉左右空格")]
        [DefaultValue(true)]
        public bool AutoTrim
        {
            get { return m_AutoTrim; }
            set { m_AutoTrim = value; }
        }

        #region 帮助信息
        
        /// <summary>
        /// 隐藏帮助信息
        /// </summary>
        [Description("隐藏帮助")]
        [DefaultValue(false)]
        public bool HideHelper
        {
            get { return m_HideHelper; }
            set { m_HideHelper = value; }
        }

        /// <summary>
        /// 帮助信息
        /// </summary>
        [Description("帮助信息")]
        public string HelperText {
            get { return m_HelperText; }
            set { m_HelperText = value; }
        }

        /// <summary>
        /// 帮助的版面位置
        /// </summary>
        [Description("帮助的版面位置")]
        [DefaultValue(HelperLayouts.Rigth)]
        public HelperLayouts HelperLayout
        {
            get { return m_HelperLayout;}
            set { m_HelperLayout = value; }
        }


        /// <summary>
        /// 帮助样式
        /// </summary>
        [Description("帮助样式")]
        [DefaultValue(HelperStyles.Text)]
        public HelperStyles HelperStyle
        {
            get { return m_HelperStyle; }
            set { m_HelperStyle = value; }
        }

        #endregion


        /// <summary>
        /// 扩展 Body 里面的字段
        /// </summary>
        [Description("扩展 Body 里面的字段")]
        public string ExtraFieldBodyCls { get; set; }


        /// <summary>
        /// 验证规则
        /// </summary>
        public string Validate { get; set; }

        /// <summary>
        /// 位置状态
        /// </summary>
        [DefaultValue(StylePosition.Static)]
        public StylePosition Position { get; set; } = StylePosition.Static;


        /// <summary>
        /// 数据源，一般填写 Store ID
        /// </summary>
        [Description("数据源，一般填写 Store ID")]
        [DefaultValue("")]
        public string DataSource { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool TabStop
        {
            get { return m_TabStop; }
            set { m_TabStop = value; }
        }

        /// <summary>
        /// 必填
        /// </summary>
        [DefaultValue("必填")]
        public bool Required { get; set; } = false;

        /// <summary>
        /// 弄脏
        /// </summary>
        [DefaultValue(false)]
        public bool Dirty
        {
            get { return m_Dirty; }
            set { m_Dirty = value; }
        }

        /// <summary>
        /// 主单元格内容对齐方式
        /// </summary>
        [DefaultValue(TextAlign.Left)]
        public TextAlign BodyAlign
        {
            get { return m_BodyAlign; }
            set { m_BodyAlign = value; }
        }

        protected string JsNamespace
        {
            get { return m_JsNamaspace; }
            set { m_JsNamaspace = value; }
        }       
        
        protected string InReady
        {
            get { return m_InReady; }
            set { m_InReady = value; }
        }

        /// <summary>
        /// 获取或设置是否可视
        /// </summary>
        [DefaultValue(true)]
        public new bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue( DockStyle.None)]
        public DockStyle Dock
        {
            get { return m_Dock; }
            set { m_Dock = value; }
        }


        /// <summary>
        /// 标签
        /// </summary>
        [DefaultValue("")]
        [Description("标签")]
        public string Tag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }


        Labelable m_Labelable = new Labelable();

        /// <summary>
        /// 获取或设置文本框只读
        /// </summary>
        [DefaultValue(false)]
        [Description("获取或设置文本框只读")]
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;

                if (this.DesignMode)
                {
                    return;
                }

                MiniHelper.EvalFormat("{0}.setReadOnly({1});", this.ClientID, value.ToString().ToLower());
            }
        }


        /// <summary>
        /// 配合 Form 表单提交的名称
        /// </summary>
        [DefaultValue("")]
        [Description("配合 Form 表单提交的名称")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 标签属性
        /// </summary>
        [Browsable(false)]
        [Description("标签")]
        public Labelable Labelable
        {
            get { return m_Labelable; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [Description("宽度")]
        [DefaultValue("100%")]
        public string Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 右边位置
        /// </summary>
        [Description("右边位置")]
        public string Top { get; set; }

        /// <summary>
        /// 左位置
        /// </summary>
        [Description("左位置")]
        public string Left { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        [Description("高度")]
        public string Height { get; set; }
        

        /// <summary>
        /// 最小高度
        /// </summary>
        [Description("最小高度")]
        public string MinHeight { get; set; }

        /// <summary>
        /// 最大高度
        /// </summary>
        [Description("最大高度")]
        public string MaxHeight { get; set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        [Description("最小宽度")]
        public string MinWidth
        {
            get { return m_MinWidth; }
            set { m_MinWidth = value; }
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [Description("最大宽度")]
        public string MaxWidth
        {
            get { return m_MaxWidth; }
            set { m_MaxWidth = value; }
        }


        #region Labelabel

        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签")]
        [Category("Labelabel")]
        public string FieldLabel
        {
            get { return m_Labelable.FieldLabel; }
            set { m_Labelable.FieldLabel = value; }
        }

        /// <summary>
        /// 标签位置
        /// </summary>
        [Description("标签位置")]
        [DefaultValue(TextAlign.Left)]
        [Category("Labelabel")]
        public TextAlign LabelAlign
        {
            get { return m_Labelable.LabelAlign; }
            set { m_Labelable.LabelAlign = value; }
        }

        /// <summary>
        /// 标签宽度
        /// </summary>
        [Description("标签宽度")]
        [DefaultValue(100)]
        [Category("Labelabel")]
        public int LabelWidth
        {
            get { return m_Labelable.LabelWidth; }
            set { m_Labelable.LabelWidth = value; }
        }

        /// <summary>
        /// 标签填补空白
        /// </summary>
        [Description("标签填补空白")]
        [DefaultValue(5)]
        [Category("Labelabel")]
        public int LabelPad
        {
            get { return m_Labelable.LabelPad; }
            set { m_Labelable.LabelPad = value; }
        }

        /// <summary>
        /// 标签分隔符
        /// </summary>
        [Description("标签分隔符")]
        [DefaultValue(":")]
        [Category("Labelabel")]
        public string LabelSeparator
        {
            get { return m_Labelable.LabelSeparator; }
            set { m_Labelable.LabelSeparator = value; }
        }

        /// <summary>
        /// 标签隐藏
        /// </summary>
        [Description("标签隐藏")]
        [DefaultValue(false)]
        [Category("Labelabel")]
        public bool HideLabel
        {
            get { return m_Labelable.HideLabel; }
            set { m_Labelable.HideLabel = value; }

        }

        #endregion


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


        protected void BeginScript(StringBuilder sb)
        {
            sb.AppendLine("<script type=\"text/javascript\">");
        }

        protected void BeginReady(StringBuilder sb)
        {

            JavascriptMode jsMode = this.JavascriptMode;

            //if (jsMode == JavascriptMode.MInJs2)
            //{
            //    sb.Append("In.ready('" + m_InReady + "', function () {").AppendLine();
            //}
            //else
            //{
                sb.AppendLine("$(document).ready(function(){");
            //}
        }

        protected void EndReady(StringBuilder sb)
        {
            sb.AppendLine("});");
        }

        protected void EndScript(StringBuilder sb)
        {
            sb.AppendLine("</script>");
        }

        string m_Value;

        /// <summary>
        /// 默认值
        /// </summary>
        string m_DefaultValue;

        /// <summary>
        /// 获取或设置文本框值
        /// </summary>
        [Description("获取或设置文本框值")]
        [DefaultValue("")]
        public virtual string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;

                if (this.DesignMode)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    MiniHelper.EvalFormat("{0}.setValue(\"\");", this.ClientID);
                }
                else
                {
                    MiniHelper.EvalFormat("{0}.setValue(\"{1}\");", this.ClientID, JsonUtil.ToJson(value));
                }
            }
        }


        Store m_DataSource;


        /// <summary>
        /// 获取当前控件中的 StoreID 对应的唯一ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        protected string GetDataSourceIdByPage(string storeId)
        {
            if (m_DataSource != null)
            {
                return m_DataSource.ClientID;
            }

            if (string.IsNullOrEmpty(storeId))
            {
                return string.Empty;
            }

            Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            if (con == null)
            {
                return string.Empty;
            }

            return con.ClientID;
        }


        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        [DefaultValue("")]
        public virtual string DefaultValue
        {
            get { return m_DefaultValue; }
            set { m_DefaultValue = value; }
        }


        /// <summary>
        /// 创建异常提示信息
        /// </summary>
        public void MarkInvalid(string error)
        {
            string valueStr = MiniHelper.GetItemJson(error);
            MiniHelper.EvalFormat("{0}.markInvalid(\"{1}\");", this.ClientID, valueStr);
        }

        /// <summary>
        /// 清理异常信息
        /// </summary>
        public void ClearInvalid()
        {
            MiniHelper.EvalFormat("{0}.clearInvalid();", this.ClientID);
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

        protected void JsParam(StringBuilder sb, string mame, decimal value,decimal defaultValue)
        {
            if (value == defaultValue)
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
            if (string.IsNullOrEmpty( value))
            {
                return;
            }

            sb.AppendFormat("    {0}: '{1}',", name, value).AppendLine();
        }

        protected void JsParam(StringBuilder sb, string name, string value,string defaultValue)
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



        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion


        string m_Groups;

        /// <summary>
        /// 分组名称
        /// </summary>
        [Description("分组名称")]
        public string Groups
        {
            get { return m_Groups; }
            set { m_Groups = value; }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {

            this.Value = this.DefaultValue;

        }


        #region bind data

        /// <summary>
        /// 数据类型
        /// </summary>
        [DefaultValue(DataType.Auto)]
        public DataType DataType { get; set; } = DataType.Auto;


        /// <summary>
        /// 绑定的数据表名称
        /// </summary>
        string m_DataTable;

        /// <summary>
        /// 绑定的数据字段名称
        /// </summary>
        string m_DataField;
        
        /// <summary>
        /// 逻辑运算符
        /// </summary>
        string m_DataLogic;

        /// <summary>
        /// 数据模糊查询的格式.默认是: %{0}%
        /// </summary>
        string m_DataLikeFormat;

        /// <summary>
        /// 数据模糊查询的格式.默认是: %{0}%
        /// </summary>
        [Description("数据模糊查询的格式.默认是: %{0}%")]
        public string DataLikeFormat
        {
            get { return m_DataLikeFormat; }
            set { m_DataLikeFormat = value; }
        }

        /// <summary>
        /// 绑定的数据表名称
        /// </summary>
        public string DataTable
        {
            get { return m_DataTable; }
            set { m_DataTable = value; }
        }

        /// <summary>
        /// 绑定的数据字段名称
        /// </summary>
        public string DataField
        {
            get { return m_DataField; }
            set { m_DataField = value; }
        }

        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public string DataLogic
        {
            get { return m_DataLogic; }
            set { m_DataLogic = value; }
        }



#endregion


        /// <summary>
        /// 延迟加载
        /// </summary>
        bool m_IsDelayRender = false;

        /// <summary>
        /// 延迟加载
        /// </summary>
        [Description("延迟加载")]
        [DefaultValue(false)]
        public bool IsDelayRender
        {
            get { return m_IsDelayRender; }
            set { m_IsDelayRender = value; }
        }
    }
}
