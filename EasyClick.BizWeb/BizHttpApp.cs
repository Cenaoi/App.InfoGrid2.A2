using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using App.Register;
using EasyClick.BizWeb;
using EasyClick.BizWeb.Affair;
using EasyClick.BizWeb.MiniExpand;
using EasyClick.Web.Mini;
using EC5.SystemBoard.Configuration;
using EC5.SystemBoard.EcReflection;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class BizHttpApp
    {
        static string m_AppCode = "app";
        static string m_AppPath = "~/App";

        /// <summary>
        /// 安全对象模块
        /// </summary>
        static List<ISecurity> m_SecModuleList = new List<ISecurity>();


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 激活安全模块
        /// </summary>
        static bool m_SecPageEnabled = true;

        /// <summary>
        /// 激活安全模块
        /// </summary>
        internal static bool SecPageEnabled
        {
            get { return m_SecPageEnabled; }
            set { m_SecPageEnabled = value; }
        }

        //public static void AddSecModule(ISecurity secModule)
        //{
        //    m_SecModuleList.Add(secModule);
        //}

        /// <summary>
        /// 安全对象模块
        /// </summary>
        public static List<ISecurity> SecModuleList
        {
            get { return m_SecModuleList; }
        }

        /// <summary>
        /// App 代码。例如：app
        /// </summary>
        public static string AppCode
        {
            get { return m_AppCode; }
            set { m_AppCode = value; }
        }

        /// <summary>
        /// App 代码路径。例如：~/App
        /// </summary>
        public static string AppPath
        {
            get { return m_AppPath; }
            set { m_AppPath = value; }
        }

        static string CrateViewUri(string[] items)
        {
            int lastIndex = items.Length - 1;

            StringBuilder sb = new StringBuilder();

            sb.Append(items[2]);

            for (int i = 3; i < lastIndex; i++)
            {
                sb.Append("/").Append( items[i]);
            }

            int pageLen = items[lastIndex].Length;

            sb.Append("/").Append(items[lastIndex].Substring(0, pageLen - 5));

            return sb.ToString();
        }



        /// <summary>
        /// 初始化安全模块
        /// </summary>
        private static void InitSecurity()
        {

            HttpContext context = HttpContext.Current;
            

            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(m_AppPath + "/Modules/SecModules"));

            if (!di.Exists) return;

            DirectoryInfo[] dirs = di.GetDirectories();

            m_SecModuleList.Clear();

            foreach (DirectoryInfo d in dirs)
            {
                string moduleFile = d.FullName + "\\module.config";

                if (!File.Exists(moduleFile))
                {
                    continue;
                }


                string binDir = d.FullName + "\\bin";

                LoadSecurity(binDir);
            }


            foreach (ISecurity item in m_SecModuleList)
            {
                try
                {
                    item.Init();
                }
                catch (Exception ex)
                {
                    log.Error("初始化安全模块失败.Init()模块名称：" + item.GetType().FullName , ex);
                }
            }

            

        }

        /// <summary>
        /// 加载安全模块
        /// </summary>
        /// <param name="binDir"></param>
        static void LoadSecurity(string binDir)
        {
            if (!Directory.Exists(binDir)) { return; }

            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册"); 



            string[] dllFiles = Directory.GetFiles(binDir, "*.dll");

            EcAppInfo appInfo = SysBoardManager.CurrentApp;

            if (appInfo.ProbingModuleDir)
            {

                foreach (string dllFile in dllFiles)
                {
                    Assembly ass = Assembly.LoadFrom(dllFile);

                    Type[] typeList = GetTypeForAssembly(ass, typeof(ISecurity));

                    if (typeList == null || typeList.Length == 0)
                    {
                        continue;
                    }

                    foreach (Type t in typeList)
                    {
                        try
                        {
                            ISecurity secObj = (ISecurity)Activator.CreateInstance(t);

                            m_SecModuleList.Add(secObj);
                        }
                        catch (Exception ex)
                        {
                            log.Error("加载安全模块失败,模块名称:\"" + t.FullName + "\"", ex);
                        }
                    }
                }
            }
            else
            {
                HttpContext context = HttpContext.Current;

                foreach (string dllFile in dllFiles)
                {
                    string binPath = context.Server.MapPath("~/bin/" + Path.GetFileName(dllFile));

                    Assembly ass = Assembly.LoadFrom(binPath);

                    Type[] typeList = GetTypeForAssembly(ass, typeof(ISecurity));

                    if (typeList == null || typeList.Length == 0)
                    {
                        continue;
                    }

                    foreach (Type t in typeList)
                    {
                        try
                        {
                            ISecurity secObj = (ISecurity)Activator.CreateInstance(t);

                            m_SecModuleList.Add(secObj);
                        }
                        catch (Exception ex)
                        {
                            log.Error("加载安全模块失败,模块名称:\"" + t.FullName + "\"", ex);
                        }
                    }
                }
            }


        }


        static Type[] GetTypeForAssembly(Assembly assembly, Type IFace)
        {
            List<Type> typeList = new List<Type>();

            Type[] ts = assembly.GetExportedTypes();

            string ifaceName = IFace.Name;

            foreach (Type t in ts)
            {
                bool equalsBaseT = TypeUtil.IsInheritInterface(t, ifaceName);

                if (equalsBaseT)
                {
                    typeList.Add(t);
                }
            }

            return typeList.ToArray();
        }


        /// <summary>
        /// 初始化 Web.Mini 
        /// </summary>
        private static void InitMini()
        {
            log.Info("InitMini() Begin：初始化 Web.Mini ");

            MiniConfiguration.JsonFactory = new ModelJsonFactory();
            MiniConfiguration.ActionPath = "/Core/Mini/EcWidgetAction.aspx";
            MiniConfiguration.ServerAttrTags = new string[] { "DBText", "DBField", "DBLogic" };

            log.Info("InitMini() End");
        }

        /// <summary>
        /// 初始化地板
        /// </summary>
        private static void InitSysBoard()
        {
            log.Info("{0}() Begin：初始化地板 ");

            HttpContext context = HttpContext.Current;

            string appDir = context.Server.MapPath(m_AppPath);
            SysBoardManager.DefaultAppCode = m_AppCode;

            if (!Directory.Exists(appDir))
            {
                throw new Exception(string.Format("没有找到相关的目录 {0}.", appDir));
            }

            DirectoryInfo bizDir = new DirectoryInfo(appDir);
            

            AppConfig appCfg = ModuleConfigurationManager.OpenAppConfig(bizDir.FullName);
            appCfg.RelativeVirtualPath = m_AppPath;
            appCfg.Code = m_AppCode;

            DbDecipherManager.DataDirectory = context.Server.MapPath(appCfg.RelativeVirtualPath + "/App_Data");

            if (appCfg.DbConfig != null && !string.IsNullOrEmpty(appCfg.DbConfig.Src))
            {
                string dbFile = bizDir.FullName + "\\" + appCfg.DbConfig.Src.Replace("/", "\\");

                log.DebugFormat("加载数据库配置文件：{0}", dbFile);
               
                DbDecipherManager.Configure(dbFile);
            }


            log.DebugFormat("加载 App 配置文件");
            SysBoardManager.OpenAppConfig(appCfg);


            if (appCfg.AffairConfig != null && !string.IsNullOrEmpty(appCfg.AffairConfig.Src))
            {
                log.Debug("加载 '事务' 配置文件");
                string affairPath = bizDir.FullName + "\\" + appCfg.AffairConfig.Src.Replace("/", "\\");
                AffairManager.Configer(affairPath);
            }


            log.InfoFormat("{0}() End", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        public static void End(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_End");
        }

        static void InitStart()
        {
            //log.InfoFormat("{0}() Begin: 合并 Mini.js", System.Reflection.MethodBase.GetCurrentMethod().Name);
            //HttpApplication httpApp = (HttpApplication)sender;

            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender2, ResolveEventArgs args)
            {

                Assembly ass = null;

                try
                {
                    ass = System.Reflection.Assembly.LoadFile(args.Name);
                }
                catch (Exception ex)
                {
                    log.Error("AppDomain.CurrentDomain.AssemblyResolve 加载 " + args.Name + "失败.", ex);
                }

                return ass;

            };


            AppDomain app = AppDomain.CurrentDomain;

            app.ReflectionOnlyAssemblyResolve += delegate(object sender2, ResolveEventArgs args)
            {
                Assembly ass = null;

                try
                {
                    ass = System.Reflection.Assembly.ReflectionOnlyLoadFrom(args.Name);
                }
                catch (Exception ex)
                {
                    log.Error("AssemblyResolve 加载 " + args.Name + "失败.", ex);
                }
                return ass;
            };

            app.UnhandledException += delegate(object sender2, UnhandledExceptionEventArgs e2)
            {
                log.Error("未知错误!");
                log.Error(e2.ExceptionObject);
            };

            app.AssemblyResolve += delegate(object sender2, ResolveEventArgs args)
            {
                Assembly ass = null;

                try
                {
                    HttpContext context = HttpContext.Current;

                    string[] ps = args.Name.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
                    string path =  context.Server.MapPath("/Bin/" + ps[0] + ".dll");

                    ass = System.Reflection.Assembly.LoadFile(path);
                }
                catch (Exception ex)
                {
                    log.Error("AssemblyResolve 加载 " + args.Name + "失败.", ex);
                }

                //Assembly ass = AppDomain.CurrentDomain.Load(args.Name);

                return ass;
            };

            SysBoardManager.AssemblyLoader += delegate(object sender2, EcAssemblyEventArgs ee)
            {
                try
                {
                    HWQ.Entity.LightModels.LightModel.LoadModelDNA(ee.Assembly);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat(string.Format("加载 {0} 实体失败!", ee.Assembly.FullName), ex);
                }
            };
        }


        public static void StartBiz(object sender, EventArgs e)
        {



            try
            {
                InitStart();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }


            InitMini();     //初始化轻量级组件

            InitSysBoard(); //初始化底板

            InitSecurity(); //初始化安全模块

            log.InfoFormat("{0}() End", System.Reflection.MethodBase.GetCurrentMethod().Name);

            License();

        }

        public static void License()
        {

            try
            {
                log.Debug("准备读取授权信息.");
                ProResultData();
                log.Debug("授权成功");
            }
            catch
            {
                log.Error("授权失败，请重新提交授权.");

                string licensePath = "/App_Biz/UserLicense/License.config";
                HttpContext context = HttpContext.Current;
                File.Delete(context.Server.MapPath(licensePath)); //注册失败后,删除注册文件
            }
        }


        /// <summary>
        /// 处理数据
        /// </summary>
        static void ProResultData()
        {
            string licensePath = "/App_Biz/UserLicense/License.config";

            if (!WebFile.Exists(licensePath))
            {
                return;
            }

            log.Debug("处理授权信息");

            RegFactory rf = new RegFactory();

            byte[] resultData = WebFile.ReadAllBytes(licensePath);

            string xml = rf.AESDecrypt(resultData, "EC5_License");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlElement root = doc.DocumentElement;


            XmlNode REG_INFO = root["REG_INFO"];
            XmlNode FILES = root["Files"];

            string BUILDER_ID = REG_INFO["BUILDER_ID"].InnerText;
            string USER_ID = REG_INFO["USER_ID"].InnerText;
            string TO_TIME = REG_INFO["TO_TIME"].InnerText;
            string MODE = REG_INFO["MODE"].InnerText;
            string S_CODE = REG_INFO["S_CODE"].InnerText;



            HttpContext context = HttpContext.Current;

            BindingFlags bf = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            Assembly ass;
            Type objT;


            bool isRegisterNow = false;

            string file = context.Server.MapPath("~/App_Biz/Config/Main.WebSite.config");

            if (File.Exists(file))
            {
                MainWebSiteConfig ws = XmlUtil.OpenXmlFile<MainWebSiteConfig>(file);

                isRegisterNow = ws.IsRegisterNow;
            }


            AppDomain curApp = AppDomain.CurrentDomain;

            Assembly[] assList = curApp.GetAssemblies();

            foreach (XmlNode xFile in FILES.ChildNodes)
            {
                string data = xFile.FirstChild.InnerText;

                byte[] bs = Convert.FromBase64String(data);

                string xOrgFilename = xFile.Attributes["NAME"].Value;
                string path = context.Server.MapPath("/bin/" + xOrgFilename);

                object[] mPs = new object[] { bs,S_CODE, USER_ID, TO_TIME, MODE, isRegisterNow };

                log.DebugFormat("注册文件:{0}", xOrgFilename);

                try
                {
                    ass = GetAssembly(assList, xOrgFilename);

                    objT = ass.GetType("App.Register.RegHelp");
                    objT.InvokeMember("SetData", bf, null, null, mPs);

                    //延迟注册
                    bool isValidRegister = (bool)objT.InvokeMember("IsValidRegister", bf, null, null, null);
                    //已经注册
                    bool isRegister = (bool)objT.InvokeMember("IsRegister", bf, null, null, null);

                    if (isRegisterNow && !isRegister)
                    {
                        log.Error("注册失败:" + xOrgFilename);

                        //File.Delete(context.Server.MapPath( licensePath)); //注册失败后,删除注册文件
                    }
                }
                catch (Exception ex)
                {
                    log.Error("注册失败:\"" + xOrgFilename + "\"", ex);

                    //File.Delete(context.Server.MapPath(licensePath)); //注册失败后,删除注册文件
                }
            }



        }


        static Assembly GetAssembly(Assembly[] assList, string dllName)
        {
            Assembly ass = null;

            string fileName;
            dllName = dllName.ToLower();

            foreach (Assembly item in assList)
            {
                fileName = Path.GetFileName(item.CodeBase).ToLower();

                if (fileName == dllName)
                {
                    ass = item;
                    break;
                }
            }

            return ass;
        }


        public static void Application_Start(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_Start");
        }


        static void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);

        }

        private static void FlashSWFile()
        {
            HttpContext context = HttpContext.Current;

            if (context == null) return;

            HttpRequest request = context.Request;

            if ("POST".Equals(request.HttpMethod, StringComparison.CurrentCultureIgnoreCase) ) return;


            #region 采用 Flash 上传机制的组件
            //log.Info("Application_BeginRequest");

            /* Fix for the Flash Player Cookie bug in Non-IE browsers.
		     * Since Flash Player always sends the IE cookies even in FireFox
		     * we have to bypass the cookies by sending the values as part of the POST or GET
		     * and overwrite the cookies with the passed in values.
		     * 
		     * The theory is that at this point (BeginRequest) the cookies have not been read by
		     * the Session and Authentication logic and if we update the cookies here we'll get our
		     * Session and Authentication restored correctly
		     */

            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SESSIONID";

                if (request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, request.Form[session_param_name]);
                }
                else if (request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, request.QueryString[session_param_name]);
                }
            }
            catch (Exception)
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write("Error Initializing Session");
            }

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string authObj = HttpContext.Current.Request.Form[auth_param_name];

                    if (authObj != null)
                    {
                        UpdateCookie(auth_cookie_name, authObj);
                    }
                    else if (request.QueryString[auth_param_name] != null)
                    {
                        UpdateCookie(auth_cookie_name, request.QueryString[auth_param_name]);
                    }
                }

            }
            catch (Exception)
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write("Error Initializing Forms Authentication");
            }

            #endregion
        }


        public static void BeginRequest(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_BeginRequest");

            BizHttpApp.ViewUriMap(sender, e);

            FlashSWFile();
        }

        public static void EndRequest(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_EndRequest");


            HttpApplication httpApp = (HttpApplication)sender;

            if (httpApp.Context != null && httpApp.Context.Items.Contains(WebDecipherManage.Name))
            {
                HttpContext content = HttpContext.Current;

                //清除连接
                WebDecipherManage wd = content.Items[WebDecipherManage.Name] as WebDecipherManage;

                if (wd != null)
                {
                    wd.DisposeAll();
                }
            }

        }

        static int AddRedirectNum(HttpContext httpContext)
        {

            int redirectNum = 0;

            if (!httpContext.Items.Contains("EC_REDIRECT_NUM"))
            {
                httpContext.Items.Add("EC_REDIRECT_NUM", 0);
            }

            redirectNum = (int)httpContext.Items["EC_REDIRECT_NUM"]; //重定向数量
            if (redirectNum > 2)
            {
                return redirectNum;
            }

            httpContext.Items["EC_REDIRECT_NUM"] = (redirectNum + 1);

            return redirectNum;
        }


        private static bool ViewUriMap_S4(HttpContext httpContext,string[] items)
        {
            int itemLen = items.Length;

            if (itemLen == 3 && SysBoardManager.DefaultAppCode.Equals(items[1], StringComparison.OrdinalIgnoreCase))
            {
                string defaultUriStr = SysBoardManager.CurrentApp.AppSettings["DefaultUri"];

                StringBuilder sb = new StringBuilder("/");
                sb.Append(m_AppCode).Append("/");

                
                if (!string.IsNullOrEmpty(defaultUriStr) && defaultUriStr.EndsWith(".aspx", StringComparison.Ordinal))
                {
                    sb.Append(defaultUriStr);
                }
                else
                {
                    sb.Append(defaultUriStr).Append(".aspx");
                }

                httpContext.Response.Redirect(sb.ToString(), true);

                return true;
            }

            return false;
        }


        /// <summary>
        /// 视图地址映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static bool ViewUriMap(object sender, EventArgs e)
        {
            HttpApplication httpApp = (HttpApplication)sender;

            HttpContext httpContext = httpApp.Context;


            Uri uri = httpContext.Request.Url;
            
            string[] items = uri.AbsolutePath.Split('/');
            int itemLen = items.Length;

            if (itemLen <= 4 )
            {
                ViewUriMap_S4(httpContext,items);
                return false;
            }

            string appCode = items[1];
            string modelCode = items[2];
            string pageName = items[itemLen - 1];

            if (!SysBoardManager.DefaultAppCode.Equals(appCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }



            if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) &&
                !pageName.EndsWith(".ashx", StringComparison.OrdinalIgnoreCase))
            {
                //ResUriMap(sender, e);//资源文件访问跳转
                return false;
            }

            EcAppInfo appInfo = SysBoardManager.CurrentApp;

            string miniPage = "/Core/Mini/MiniPage.aspx";

            #region 跳转次数

            //AddRedirectNum(httpContext);

            #endregion


            if (appInfo.IsOneMode && appInfo.DebugModuleCode.Equals(modelCode, StringComparison.OrdinalIgnoreCase))
            {
                //处理 Debug 模式
                return ViewUriMap_OneMode(httpContext, items, miniPage, uri);
            }
            else
            {
                return ViewUriMap_Globel(httpContext, items, miniPage, uri);
            }


        }


        private static bool ViewUriMap_OneMode(HttpContext httpContext, string[] items, string miniPage, Uri uri)
        {
            string viewUri = CrateViewUri(items);

            string[] strList = viewUri.Split('/');

            string strAPath = "~";

            for (int i = 1; i < strList.Length; i++)
            {
                strAPath += "/" + strList[i];
            }

            string mapPath = httpContext.Server.MapPath(strAPath);
            

            if (File.Exists(mapPath + ".aspx"))
            {
                string newUrl = strAPath + ".aspx";

                string newQuery = uri.Query;

                if (!string.IsNullOrEmpty(newQuery) && newQuery.StartsWith("?", StringComparison.Ordinal))
                {
                    if (newQuery.Length > 1)
                    {
                        newQuery = newQuery.Substring(1);
                    }
                    else
                    {
                        newQuery = string.Empty;
                    }
                }

                EcContext.Current.Items["EC_ViewUri"] = uri.ToString();// viewUri;

                httpContext.RewritePath(newUrl, string.Empty, newQuery, false);
            }
            else if(File.Exists(mapPath + ".ashx"))
            {
                string newUrl = strAPath + ".ashx";

                string newQuery = uri.Query;

                if (!string.IsNullOrEmpty(newQuery) && newQuery.StartsWith("?", StringComparison.Ordinal))
                {
                    if (newQuery.Length > 1)
                    {
                        newQuery = newQuery.Substring(1);
                    }
                    else
                    {
                        newQuery = string.Empty;
                    }
                }

                EcContext.Current.Items["EC_ViewUri"] = uri.ToString();// viewUri;

                httpContext.RewritePath(newUrl, string.Empty, newQuery, false);
            }
            else 
            {
                string newUrl = miniPage;// "/Core/Mini/MiniPage.aspx";

                string query = uri.Query;
                string newQuery = ("ViewUri=" + viewUri);

                EcContext.Current.Items["EC_ViewUri"] = uri.ToString();// viewUri;

                if (!string.IsNullOrEmpty(query) && query.StartsWith("?", StringComparison.Ordinal))
                {
                    newQuery += "&" + query.TrimStart('?');
                }

                httpContext.RewritePath(newUrl, string.Empty, newQuery, false);
            }

            return true;
        }

        /// <summary>
        /// 视图地址映射 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="items"></param>
        /// <param name="miniPage"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static bool ViewUriMap_Globel(HttpContext httpContext, string[] items, string miniPage,Uri uri)
        {
            string viewUri = CrateViewUri(items);

            EcTypeInfo ecType = SysBoardManager.GetViewType(viewUri);

            if (ecType == null)
            {
                httpContext.Response.Clear();
                httpContext.Response.Write("访问的路径不存在." + uri);
                httpContext.Response.End();
                
                return false;
            }

            if ("ascx".Equals(ecType.Extension, StringComparison.Ordinal)) //中转 ASCX 控件
            {
                string newUrl = miniPage;// "/Core/Mini/MiniPage.aspx";

                string query = uri.Query;
                string newQuery = ("ViewUri=" + viewUri);

                EcContext.Current.Items["EC_ViewUri"] = uri.ToString(); // ecType.Module.Code + "/" + ecType.FullPath;

                if (!string.IsNullOrEmpty(query) && query.StartsWith("?", StringComparison.Ordinal))
                {
                    newQuery += "&" + query.TrimStart('?');
                }

                httpContext.RewritePath(newUrl, string.Empty, newQuery, false);
            }
            else if ("aspx".Equals(ecType.Extension, StringComparison.Ordinal))
            {
                EcContext.Current.Items["EC_ViewUri"] = uri.ToString();// ecType.Module.Code + "/" + ecType.FullPath + ".aspx";

                httpContext.Items["EcPageInfo"] = ecType;

                //临时代码，欺骗 ASP.net 存在这个路径
                httpContext.RewritePath("~" + miniPage, false);

            }

            return true;
        }


        /// <summary>
        /// 重定向资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ResUriMap(object sender, EventArgs e)
        {
            HttpApplication httpApp = (HttpApplication)sender;

            HttpContext httpContext = httpApp.Context;

            Uri uri = httpContext.Request.Url;
            

            string[] items = uri.AbsolutePath.Split('/');
            int itemLen = items.Length;

            if (itemLen <= 4
                || !SysBoardManager.DefaultAppCode.Equals(items[1], StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string moduleCode = items[2];

            EcAppInfo app = SysBoardManager.CurrentApp;

            if (!app.ModuleInfos.ContainsCode(moduleCode.ToLower()))
            {
                if (app.IsOneMode && app.DebugModuleCode.Equals(moduleCode, StringComparison.OrdinalIgnoreCase))
                {
                    string ppat2 = GetObjFullname(items, 3);

                    string newUrl2 = string.Format("/{0}", ppat2);

                    httpContext.RewritePath(newUrl2, true);
                }
                
                return;
            }

            string ppat = GetObjFullname(items,3);

            string newUrl = string.Format("/{0}/Modules/{1}/{2}", SysBoardManager.DefaultAppCode, moduleCode, ppat);

            httpContext.RewritePath(newUrl, true);
            
        }


        private static string GetObjFullname(string[] strList, int n1)
        {
            int n2 = strList.Length - 1;

            return GetObjFullname(strList, n1, n2);
        }

        private static string GetObjFullname(string[] strList, int n1, int n2)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(strList[n1]);

            for (int i = n1 + 1; i <= n2; i++)
            {
                sb.Append("/");
                sb.Append(strList[i]);
            }

            return sb.ToString();
        }

        public static void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_PreRequestHandlerExecute");
            
            //提供设置 Session 
            SecAllow();

        }

        /// <summary>
        /// 安全通行验证
        /// </summary>
        static void SecAllow()
        {
            if (!m_SecPageEnabled)
            {
                return;
            }
           
            HttpContext context = HttpContext.Current;

            string viewUri = context.Request.QueryString["ViewUri"];

            if (string.IsNullOrEmpty(viewUri))
            {
                if (context.Items["EcPageInfo"] != null)
                {
                    EcTypeInfo ecTI = (EcTypeInfo)context.Items["EcPageInfo"];
                    viewUri = ecTI.Module.Code + "/" + ecTI.FullPath;
                }
                else
                {
                    return;
                }
            }



            bool isAllow = true;
            ISecurity sec;

            for (int i = 0; i < m_SecModuleList.Count; i++)
            {
                sec = m_SecModuleList[i];

                try
                {
                    isAllow = sec.Allow();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    isAllow = false;
                }

                if (!isAllow)
                {
                    break;
                }

            }

            if (!isAllow)
            {
                if (context.Items.Count > 0)
                {
                    List<string> keys = new List<string>(context.Items.Count);

                    foreach (string item in context.Items.Keys)
                    {
                        if (item == "AspSession" || 
                            item == "__EcContext" ||
                            item == "AspSessionIDManagerInitializeRequestCalled")
                        {

                        }
                        else
                        {
                            keys.Add(item);
                        }
                    }

                    foreach (string key in keys)
                    {
                        context.Items.Remove(key);
                    }
                }


                string newUrl = "/Core/Mini/MiniPage.aspx?ViewUri=" + "BizExplorer/View/ErrorRedirect";

                EcContext.Current.Items["EC_ViewUri"] = "BizExplorer/View/ErrorRedirect";

                context.RewritePath(newUrl, true);
            }

        }


        public static void PostRequestHandlerExecute(object sender, EventArgs e)
        {

            HttpContext context = HttpContext.Current;

            if (context.Items.Contains("EcPageInfo"))
            {
                EcTypeInfo ecType = context.Items["EcPageInfo"] as EcTypeInfo;

                if (ecType != null)
                {
                    try
                    {
                        context.Response.Clear();

                        System.Web.UI.Page pageObj = (System.Web.UI.Page)Activator.CreateInstance(ecType.Src);

                        ((IHttpHandler)pageObj).ProcessRequest(context);


                        pageObj.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }

            }

            AffairManager.Exe("Application_PostRequestHandlerExecute");

            BindingFlags bf = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            Type objT = typeof(BizHttpApp).Assembly.GetType("App.Register.RegHelp");

            //已经注册
            bool isRegister = (bool)objT.InvokeMember("IsRegister", bf, null, null, null);

            if (!isRegister)
            {
                CreateNotValidCode();
            }
            else
            {
                //延迟注册
                bool isValidRegister = (bool)objT.InvokeMember("IsValidRegister", bf, null, null, null);
                
                if (!isValidRegister)
                {
                    CreateLastRegister();
                }
            }


        }

        private static void CreateLastRegister()
        {
            HttpContext context = HttpContext.Current;

            if ("GET".Equals(context.Request.RequestType, StringComparison.Ordinal) &&
                "text/html".Equals(context.Response.ContentType, StringComparison.Ordinal))
            {

                string isPost = context.Request.QueryString["__IsPost"];

                object rJs = context.Items["ResponseJS"];

                if ("true".Equals(isPost, StringComparison.Ordinal) ||
                    "1".Equals(isPost, StringComparison.Ordinal) ||
                    true.Equals(rJs))
                {

                }
                else
                {
                    HttpResponse response = context.Response;

                    response.Write("<div style=\"");
                    response.Write("border-width: 1px; border-color: #FFFFFF;  background-color: #FFCC00; position: fixed; top: 0px; left: 0px;width:3px;height:3px; border-right-style: solid; border-bottom-style: solid;");
                    response.Write("\">");
                    response.Write("</div>");
                }

            }
        }

        private static void CreateNotValidCode()
        {
            HttpContext context = HttpContext.Current;

            if ("GET".Equals(context.Request.RequestType, StringComparison.Ordinal) &&
                "text/html".Equals(context.Response.ContentType, StringComparison.Ordinal))
            {

                string isPost = context.Request.QueryString["__IsPost"];

                object rJs = context.Items["ResponseJS"];

                if ("true".Equals(isPost, StringComparison.Ordinal) ||
                    "1".Equals(isPost, StringComparison.Ordinal) ||
                    true.Equals(rJs))
                {

                }
                else
                {
                    HttpResponse response = context.Response;

                    response.Write("<div style=\"");
                    response.Write("border-width: 1px; border-color: #FFFFFF; padding: 0px 2px 0px 2px; background-color: #FFCC00; position: fixed; top: 0px; left: 0px; border-right-style: solid; border-bottom-style: solid;");
                    response.Write("\">");
                    response.Write("<a href=\"/app/DLLRegister/View/Setup1.aspx\" target=\"_blank\">调试版</a>");
                    response.Write("</div>");
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SessionEnd(object sender, EventArgs e)
        {
            AffairManager.Exe("Application_SessionEnd");
        }
    }
}
