using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyClick.Web.Mini;
using EC5.SystemBoard;
using EC5.SystemBoard.EcReflection;
using EC5.Utility.Web;
using EC5.SystemBoard.Web;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;

namespace EasyClick.BizWeb.UI
{
    [ParseChildren(true, "ExTemplate")]
    public class Widget : UserControl, IAttributeAccessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region 字段

        //WidgetVisibleMode m_EcVisibleMode = WidgetVisibleMode.Common;

        Exception m_Exception = null;

        UserControl m_Con;

        /// <summary>
        /// 地址字符串
        /// </summary>
        string m_UriString;

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        bool m_EcDesignMode = false;

        bool m_Ajax = false;

        /// <summary>
        /// 容器ID,给 AJAX 使用的
        /// </summary>
        string m_PanelID;

        /// <summary>
        /// 网页上的请求信息
        /// </summary>
        string m_Query;

        string m_Params;

        /// <summary>
        /// 延迟加载
        /// </summary>
        bool m_EcDelay = false;

        /// <summary>
        /// 返回的数据格式 [html|script]
        /// </summary>
        string m_EcReturnFormat = "html";

        /// <summary>
        /// 注意：不要放入默认值
        /// </summary>
        string m_DisplayLanguage;

        #endregion

        #region 属性

        //[DefaultValue(WidgetVisibleMode.Common)]
        //public WidgetVisibleMode EcVisibleMode
        //{
        //    get { return m_EcVisibleMode; }
        //    set { m_EcVisibleMode = value; }
        //}

        /// <summary>
        /// 延迟加载，默认 true
        /// </summary>
        [Description("延迟加载，默认 false")]
        [DefaultValue(false)]
        public bool EcDelay
        {
            get { return m_EcDelay; }
            set { m_EcDelay = value; }
        }


        /// <summary>
        /// 返回的数据类型,[html|script]
        /// </summary>
        [DefaultValue("html")]
        [Description("返回的数据类型, [html|script]")]
        public string EcReturnFormat
        {
            get { return m_EcReturnFormat; }
            set { m_EcReturnFormat = value; }
        }

        /// <summary>
        /// 容器ID,给 AJAX 使用的
        /// </summary>
        [DefaultValue("")]
        [Description("容器ID,给 AJAX 使用的")]
        public string PanelID
        {
            get { return m_PanelID; }
            set { m_PanelID = value; }
        }

        /// <summary>
        /// 启动 Ajax  ,默认 false 
        /// </summary>
        [DefaultValue(false)]
        [Description("启动 Ajax  ,默认 false")]
        public bool Ajax
        {
            get { return m_Ajax; }
            set { m_Ajax = value; }
        }

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        [DefaultValue(false)]
        [Description("获取一个值，用以指示当前是否处于设计模式。")]
        public bool EcDesignMode
        {
            get { return m_EcDesignMode; }
            set { m_EcDesignMode = value; }
        }


        /// <summary>
        /// 参数
        /// </summary>
        [DefaultValue("")]
        [Description("参数。")]
        public string Params
        {
            get { return m_Params; }
            set { m_Params = value; }
        }


        /// <summary>
        /// 语言. CH/EN/....
        /// </summary>
        [DefaultValue("")]
        [Description("语言. [CH | EN]")]
        public string DisplayLanguage
        {
            get { return m_DisplayLanguage; }
            set { m_DisplayLanguage = value; }
        }


        /// <summary>
        /// 组件路径
        /// </summary>
        [Description("组件路径")]
        [DefaultValue("")]
        public string UriString
        {
            get { return m_UriString; }
            set { m_UriString = value; }
        }

        #endregion

        bool m_CreateParentBox = true;

        /// <summary>
        /// 创建父容器
        /// </summary>
        [DefaultValue(true)]
        public bool CreateParentBox
        {
            get { return m_CreateParentBox; }
            set { m_CreateParentBox = value; }
        }

        bool m_IsPostBack = false;

        public new bool IsPostBack
        {
            get
            {
                return m_IsPostBack;                                                 
            }
        }

        protected new internal char ClientIDSeparator
        {
            get { return base.ClientIDSeparator; }
        }

        /// <summary>
        /// 扩展属性的模板
        /// </summary>
        string m_ExTemplate;

        /// <summary>
        /// 扩展属性的模板
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public string ExTemplate
        {
            get { return m_ExTemplate; }
            set { m_ExTemplate = value; }
        }



        protected override void OnInit(EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            m_IsPostBack = WebUtil.Query<bool>("__IsPost",false);

            if (!m_IsPostBack)
            {
                NameValueCollection nv = HttpUtility.ParseQueryString(context.Request.Url.Query);

                nv.Remove("ViewUri");

                if (nv.Count > 0)
                {
                    m_Query = WebUtil.ToQueryString(nv);
                }
                else
                {
                    m_Query = string.Empty;
                }

            }

            base.OnInit(e);
        }


        /// <summary>
        /// 给设置子组件的属性
        /// </summary>
        /// <param name="con"></param>
        private void SetChildAttributes(UserControl con)
        {
            Type conT = con.GetType();



            foreach (string key in m_HtmlAttrs.Keys)
            {
                PropertyInfo prop = conT.GetProperty(key);

                if (prop == null)
                {
                    continue;
                }

                string txt = m_HtmlAttrs[key];

                object v = txt;

                if (v.GetType() != prop.PropertyType)
                {
                    if (prop.PropertyType == typeof(Unit))
                    {
                        v = Unit.Parse(txt);
                    }
                    else
                    {
                        v = Convert.ChangeType(v, prop.PropertyType);
                    }
                }

                prop.SetValue(con, v, null);
            }
        }



        private static string RenewViewUri(string uriStr)
        {
            if (!uriStr.StartsWith("view://", StringComparison.OrdinalIgnoreCase))
            {
                string code = SysBoardManager.DefaultAppCode;

                if (uriStr.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    uriStr = string.Concat("view://", code, uriStr);
                }
                else
                {
                    uriStr = string.Concat("view://", code, "/", uriStr);
                }
            }

            return uriStr;
        }

        string m_appCode;
        string m_moduleCode;

        string m_viewPath;

        private string GetAscxPath(string ascxPath)
        {
            if (m_UriString.StartsWith("~", StringComparison.Ordinal))
            {
                return ascxPath;
            }

            string newAscxPath = ascxPath ;

            EcAppInfo app = SysBoardManager.CurrentApp;

            string uriStr = RenewViewUri(ascxPath);

            Uri uri = new Uri(uriStr);


            string[] paths = StringUtil.Split(uri.LocalPath, "/");

            if (paths.Length >= 2)
            {
                m_appCode = uri.DnsSafeHost;
                m_moduleCode = paths[0].ToLower();

                m_viewPath = StringUtil.Join("/",paths,1).ToLower();


                if (app.IsOneMode && m_moduleCode.Equals(app.DebugModuleCode, StringComparison.OrdinalIgnoreCase))
                {
                    newAscxPath = string.Concat( "~/" , m_viewPath ,".ascx");
                }
                else
                {
                    newAscxPath = string.Concat("~/", app.Code, "/modules/", m_moduleCode, "/", m_viewPath, ".ascx");
                }


            }

            return newAscxPath;
        }



        protected override void CreateChildControls()
        {
            if (string.IsNullOrEmpty(m_UriString))
            {
                return;
            }

            if (m_EcDelay)
            {
                return;
            }

            string ascxPath = GetAscxPath(m_UriString);

            try
            {
                string mapAPath = MapPath(ascxPath);

                if (File.Exists(mapAPath))
                {
                    m_Con = this.LoadControl(ascxPath) as UserControl;
                }
                else if ((StringUtil.StartsWith( m_UriString,"/") || StringUtil.StartsWith(m_UriString,"~")) 
                    && File.Exists(MapPath(m_UriString)))
                {
                    m_Con = this.LoadControl(m_UriString) as UserControl;
                }
                else
                {
                    EcTypeInfo viewT = SysBoardManager.GetViewType(m_UriString);

                    if (viewT == null)
                    {
                        return;
                    }

                    m_Con = Activator.CreateInstance(viewT.Src) as UserControl;

                    string uriStr = RenewViewUri(m_UriString);
                    m_Con.AppRelativeVirtualPath = GetVirtualPath(uriStr);
                }

            }
            catch (Exception ex)
            {
                m_Exception = new Exception(string.Format("加载 ascx 文件失败: \"{0}\"!", ascxPath), ex);

                log.Error(m_Exception);
                return;
            }


            try
            {
                if (m_Con is WidgetControl)
                {
                    if (!string.IsNullOrEmpty(m_moduleCode))
                    {
                        EcModuleInfo moduleInfo = SysBoardManager.GetModuleInfo(m_moduleCode);

                        ((WidgetControl)m_Con).SetModuleInfo(moduleInfo);
                    }
                    else if (SysBoardManager.CurrentApp.IsOneMode)
                    {
                        string modeuleCode = SysBoardManager.CurrentApp.DebugModuleCode;
                        EcModuleInfo moduleInfo = SysBoardManager.CurrentApp.ModuleInfos[modeuleCode];

                        ((WidgetControl)m_Con).SetModuleInfo(moduleInfo);
                    }
                }

                SetChildAttributes(m_Con);

                this.EnableViewState = false;

                m_Con.ID = "I";
                this.Controls.Add(m_Con);
            }
            catch (Exception ex)
            {
                string errCode = (string)this.Context.Items["ERR_CODE"];

                if (errCode != "200")
                {
                    m_Exception = new Exception(string.Format("加载 ascx 文件失败: \"{0}\"!", ascxPath), ex);

                    log.Error(m_Exception);
                }
                return;
            }

        }


        /// <summary>
        /// 获取虚拟路径
        /// </summary>
        /// <param name="viewUri"></param>
        /// <returns></returns>
        private string GetVirtualPath(string viewUri)
        {
            Uri uri = new Uri(viewUri);

            string[] paths = StringUtil.Split( uri.LocalPath,"/");

            string appCode = uri.DnsSafeHost;
            string moduleCode = paths[0];

            string typeFullname = StringUtil.Join("/", paths, 1);

            string virtualPath = string.Concat("~/", appCode, "/Modules/", moduleCode, "/", typeFullname);

            return virtualPath;
        }


        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode)
            {
                writer.Write("组件:" + m_UriString);
                return;
            }

            if (!App.Register.RegHelp.IsRegister()) { writer.Write("调用限制：未注册"); return; }; 

            base.Render(writer);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (!App.Register.RegHelp.IsRegister()) { writer.Write("调用限制：未注册"); return; }; 

            if (string.IsNullOrEmpty(m_UriString))
            {
                return;
            }

            if (m_Con == null && m_Exception == null)
            {
                CreateChildControls();
            }

            if (m_Exception == null && m_Ajax )
            {
                MiniWidget mw = new MiniWidget();
                mw.ClientID = WebUtil.Query("__CID", this.ClientID);
                mw.EcDelay = m_EcDelay;
                mw.EcDesignMode = this.EcDesignMode;
                mw.EcReturnFormat = this.EcReturnFormat;
                mw.Language = WebUtil.Query("__Lang", this.DisplayLanguage);
                mw.PanelID = WebUtil.Query("__PID", this.PanelID);
                mw.Params = m_Params;
                mw.Path = m_UriString;
                mw.Query = m_Query;
                mw.Con = m_Con;
                mw.IsUser = false;
                mw.CreateParentBox = m_CreateParentBox;

                mw.RenderAJAX(writer);

                mw.Dispose();

                return;
            }
            else if (m_Con != null)
            {
                base.RenderControl(writer);
            }


            if (m_Exception != null)
            {
                log.Error(m_Exception);

                writer.Write(m_Exception.Message);
                writer.Write("<br />");

                if (m_Exception.InnerException != null && m_Exception.InnerException.StackTrace != null)
                {
                    writer.Write(m_Exception.InnerException.StackTrace.Replace("\n", "<br />"));
                }
            }
        }

        
        internal System.Collections.Generic.SortedList<string, string> m_HtmlAttrs = new SortedList<string, string>();

        public string GetAttribute(string key)
        {
            if (m_HtmlAttrs.ContainsKey(key))
            {
                return m_HtmlAttrs[key];
            }

            return string.Empty;
        }

        public void SetAttribute(string key, string value)
        {
            if (m_HtmlAttrs.ContainsKey(key))
            {
                m_HtmlAttrs[key] = value;
            }
            else
            {
                m_HtmlAttrs.Add(key, value);
            }
        }
    }
}
