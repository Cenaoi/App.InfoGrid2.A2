using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;
using EC5.SystemBoard;
using EC5.SystemBoard.Configuration;
using EC5.SystemBoard.EcReflection;
using EC5.SystemBoard.Web;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.WebExpand;
using System.Diagnostics;

namespace EasyClick.BizWeb
{
    /// <summary>
    /// 改写了 System.Web.HttpApplication
    /// </summary>
    public class EC5HttpApplication: System.Web.HttpApplication
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 默认模块名称
        /// </summary>
        static string m_DefaultModuleName ;

        /// <summary>
        /// 初始化成功
        /// </summary>
        static bool m_InitSuccess = false;

        /// <summary>
        /// 网站配置信息
        /// </summary>
        MainWebSiteConfig m_MainWebSite = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public EC5HttpApplication()
        {

        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="defaultModuleName">默认模块名称</param>
        public EC5HttpApplication(string defaultModuleName)
        {
            m_DefaultModuleName = defaultModuleName;

            Type t = typeof(EasyClick.Web.Validator.ValidatorError);

        }

        /// <summary>
        /// 默认模块名称
        /// </summary>
        public string DefaultModuleName
        {
            get { return m_DefaultModuleName; }
            set { m_DefaultModuleName = value; }
        }


        /// <summary>
        /// 初始化 Log4Net
        /// </summary>
        private static void InitLog()
        {
            HttpContext context = HttpContext.Current;
            FileInfo fi = new FileInfo(context.Server.MapPath("~/app_biz/Config/log4net.Config"));
            if (!fi.Exists) { return; }
            log4net.Config.XmlConfigurator.Configure( fi);
        }

        /// <summary>
        /// 加载 Bin 目录下面的实体
        /// </summary>
        private void LoadOneselfEntity()
        {
            Stopwatch st = new Stopwatch();//实例化类

            st.Start();//开始计时

            log.Debug("加载实体...");

            string[] dlls;

            if (m_MainWebSite != null)
            {
                List<string> tmpDlls = new List<string>();

                foreach (var item in m_MainWebSite.Models)
                {
                    if (StringUtil.IsBlank(item)) { continue; }

                    tmpDlls.Add(Server.MapPath("~/bin/" + item));
                }

                dlls = tmpDlls.ToArray();
            }
            else
            {
                dlls = WebDirectory.GetFiles("~/bin", "*.dll");
            }

            st.Stop();

            log.Debug($"加载时间: {st.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            

            HWQ.Entity.LightModels.LModelDna.BeginEdit();

            foreach (string dllPath in dlls)
            {

                st.Restart();

                Assembly ass = Assembly.LoadFrom(dllPath);

                string dllName = Path.GetFileName(dllPath);

                st.Stop();

                log.Debug($"加载实体文件: {dllName}  {st.Elapsed.TotalMilliseconds:0.0000}(毫秒)");


                st.Restart();

                try
                {
                    HWQ.Entity.LightModels.LightModel.LoadModelDNA(ass);

                    st.Stop();
                    log.Debug($"寻找实体时间: {st.Elapsed.TotalMilliseconds:0.0000}(毫秒)");

                }
                catch (Exception ex)
                {
                    throw new Exception("加载实体 DNA 失败:" + dllName, ex);
                }
            }

            HWQ.Entity.LightModels.LModelDna.EndEdit();
        }

        /// <summary>
        /// 读取 module.config 文件
        /// </summary>
        /// <param name="modulePPath"></param>
        /// <param name="module"></param>
        private void ReadModuleConfig(string modulePPath, EcModuleInfo module)
        {

            if (!File.Exists(modulePPath))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(modulePPath);

            ModuleConfig mc = ModuleConfigurationManager.OpenModuleConfig(doc);


            foreach (var item in mc.AppSettings)
            {
                module.AppSettings.Add(item.Key, item.Value);
            }

            module.Text = mc.Text;

        }

        /// <summary>
        /// 配置自己网站模式
        /// </summary>
        /// <param name="modelName"></param>
        private void SetupOneselfMode(string modelName)
        {
            if (string.IsNullOrEmpty(modelName)) { return; }

            log.InfoFormat("{0}() Begin：配置自己网站模式", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Stopwatch st = new Stopwatch();//实例化类

            st.Start();//开始计时

            //string modelName = "Sample1";
            string modelPath = "App." + modelName;
            string modelRVPath = "App/" + modelName;


            EcAppInfo app = SysBoardManager.CurrentApp;
            app.SetOneMode(true, modelName);


            EcModuleInfo module = new EcModuleInfo();
            module.Code = modelName;
            module.RootNamespace = modelPath;
            module.RelativeVirtualPath = modelRVPath;
            module.PhysicalPath = HttpContext.Current.Server.MapPath("~/");

            app.ModuleInfos.Add(module);


            #region 读取 module.config 文件

            string modulePPath = module.PhysicalPath + "module.config";

            ReadModuleConfig(modulePPath, module);

            #endregion


            if (m_MainWebSite != null)
            {
                foreach (string item in m_MainWebSite.Blls)
                {
                    if (item.Contains("PublicKeyToken="))
                    {
                        try
                        {
                            log.Debug(item);

                            //App.EShop.Bll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

                            Assembly ass = Assembly.Load(item);

                            SysBoardManager.LoadAssembly(app, module, ass);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    else
                    {
                        string dllPath = Server.MapPath("~/bin/" + item);

                        if (!File.Exists(dllPath)) { continue; }

                        try
                        {
                            log.Debug(dllPath);

                            //App.EShop.Bll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null


                            Assembly ass = Assembly.LoadFile(dllPath);

                            SysBoardManager.LoadAssembly(app, module, ass);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }

            //EcTypeInfo typeInfo = new EcTypeInfo(typeof(App.Wharf.Bll.ShipMgr)) { FullPath = "Bll/ShipMgr" };
            //module.Blls.Add(typeInfo);

            //加载 bin 目录特定
            //SysBoardManager.LoadAssembly(app, module, typeof(App.EShop.Bll.EShopMgr).Assembly);

            st.Stop();

            log.Info($"End {MethodBase.GetCurrentMethod().Name} , {st.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void InitMainWebSite()
        {
            if (m_MainWebSite != null)
            {
                return;
            }

            string file = Server.MapPath("~/App_Biz/Config/Main.WebSite.config");

            if (File.Exists(file))
            {
                try
                {
                    m_MainWebSite = XmlUtil.OpenXmlFile<MainWebSiteConfig>(file);

                    Application["Main.WebSite"] = m_MainWebSite;

                    log.Info("加载 Main.WebSite.config 文件成功.");
                }
                catch (Exception ex)
                {
                    log.Error("加载 Main.WebSite.config 文件失败.",ex);
                }

                return;
            }


            m_MainWebSite = new MainWebSiteConfig();

            Application["Main.WebSite"] = m_MainWebSite;

            string path = Path.GetDirectoryName(file);

            try
            {
                Directory.CreateDirectory(path);

                XmlUtil.SaveXmlFile(file, m_MainWebSite);

                log.Info("创建 Main.WebSite.config 文件成功.");
            }
            catch (Exception ex)
            {
                log.Error("创建 Main.WebSite.config 文件失败。", ex);
            }
        }


        public virtual void StartWebSite(string moduleName, object sender, EventArgs e)
        {
            InitMainWebSite();

            log.Info("Begin: 清空实体");
            HWQ.Entity.LightModels.LModelDna.BeginEdit();
            HWQ.Entity.LightModels.LModelDna.Clear();
            HWQ.Entity.LightModels.LModelDna.EndEdit();
            log.Info("End:清空实体完成");


            Stopwatch sw = new Stopwatch();

            log.Info("Begin：初始化 BizApp");

            sw.Start();
            
            try
            {
                BizHttpApp.AppCode = "app";
                BizHttpApp.AppPath = "~/App_Biz";
                BizHttpApp.SecPageEnabled = m_MainWebSite.Sec_Page_Enabled;

                BizHttpApp.StartBiz(sender, e);

                sw.Stop();

                log.Info($"End: 初始化 BizApp 完成.  {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)") ;
            }
            catch (Exception ex)
            {
                log.Error("初始化 BizApp 错误.", ex);
                return;
            }
            

            try
            {
                sw.Restart();

                log.Debug("加载 bin 目录下的实体文件.");
                
                LoadOneselfEntity();    //加载实体

                log.Debug($"加载 bin 完成.  {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
            catch (Exception ex)
            {
                throw new Exception("加载 bin 目录的实体文件错误.", ex);
            }

            
            SetupOneselfMode(moduleName); //配置自己网站模式
            
            
            //验证数据表
            if (m_MainWebSite.AutoCreateTable)
            {
                log.Info("开始 AutoCreateTable 自动创建表");

                sw.Restart();

                LDatabaseHelper.AutoCreateTables();

                sw.Stop();

                log.Info($"结束 AutoCreateTable 自动创建表   , {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
            else
            {
                log.Info("AutoCreateTable 自动创建表: 关闭");
            }

            //创建数据表主键
            if (m_MainWebSite.AutoCreatePrimaryKey)
            {
                log.Info("开始 AutoCreatePrimaryKey 自动创建主键");

                sw.Restart();

                LDatabaseHelper.AutoCreatePrimaryKey();

                sw.Stop();

                log.Info($"结束 AutoCreatePrimaryKey 自动创建主键   {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
            else
            {
                log.Info("AutoCreatePrimaryKey 自动创建主键: 关闭");
            }

            //验证字段的差异
            if (m_MainWebSite.ValidDifferenceTableALL)
            {
                log.Info("开始 ValidDifferenceTableALL 验证字段的差异");

                sw.Restart();

                LDatabaseHelper.ValidDifferenceTableALL();

                sw.Stop();

                log.Info($"结束 ValidDifferenceTableALL 验证字段的差异   {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
            else
            {
                log.Info("ValidDifferenceTableALL 验证字段的差异: 关闭");
            }

            //自动创建数据字段描述
            if (m_MainWebSite.AutoCreateDesc)
            {
                log.Info("开始 AutoCreateDesc 自动创建数据字段描述");


                sw.Start();

                LDatabaseHelper.AutoCreateDescription();
                sw.Stop();

                log.Info($"结束 AutoCreateDesc 自动创建数据字段描述   {sw.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
            else
            {
                log.Info("AutoCreateDesc 自动创建数据字段描述: 关闭");
            }

            m_InitSuccess = true;
        }

        object m_LockTag = new object();

        public virtual void Application_Start(object sender, EventArgs e)
        {
            lock (m_LockTag)
            {
                InitLog();

                Stopwatch st = new Stopwatch();//实例化类
                st.Start();//开始计时

                log.Info("首次加载 StartWebSite(sender, e);");

                StartWebSite(m_DefaultModuleName, sender, e);

                BizHttpApp.Application_Start(sender, e);

                EC5_Init();

                st.Stop();
                log.Info($"首次加载结束 , {st.Elapsed.TotalMilliseconds:0.0000}(毫秒)");
            }
        }


        public virtual void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void EC5_Init()
        {

        }

        /// <summary>
        /// 步骤1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_BeginRequest(object sender, EventArgs e)
        {

            HttpContext context = HttpContext.Current;

            if (context.Request != null && context.Request.RawUrl == "/App/Reset.aspx")
            {
                m_InitSuccess = false;
            }


            if (!m_InitSuccess)
            {
                lock (m_LockTag)
                {
                    if (!m_InitSuccess)
                    {

                        InitLog();

                        log.Info("二次加载 StartWebSite(sender, e);");

                        StartWebSite(m_DefaultModuleName, sender, e);

                        BizHttpApp.Application_Start(sender, e);

                        EC5_Init();
                    }
                }
            }



            if (!m_InitSuccess)
            {
                log.Warn("初始化失败 .");

                Response.Clear();

                Response.Write("（" + DateTime.Now.ToString() + "）网站正在更新，请稍后再访问....！");

                Response.End();

                return;
            }


            BizHttpApp.BeginRequest(sender, e);
        }

        public virtual void Application_MapRequestHandler(object sender, EventArgs e)
        {
            HttpApplication App = (HttpApplication)sender;
        }

        /// <summary>
        /// 步骤2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //在执行验证前发生，这是创建验证逻辑的起点     

            HttpApplication App = (HttpApplication)sender;

        }


        /// <summary>
        /// 步骤3；当安全模块已经验证了当前用户的授权时执行 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_AuthorizeRequest(object sender, EventArgs e)
        {

            HttpApplication App = (HttpApplication)sender;


        }

        /// <summary>
        /// 步骤4:当ASP.NET完成授权事件以使缓存模块从缓存中为请求提供服务时发生，从而跳过处理程序（页面或者是WebService）的执行。
        /// 这样做可以改善网站的性能，这个事件还可以用来判断正文是不是从Cache中得到的。  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_ResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication App = (HttpApplication)sender;

        }

        /// <summary>
        /// 步骤5；读取了Session所需的特定信息并且在把这些信息填充到Session之前执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication App = (HttpApplication)sender;

        }

        /// <summary>
        /// 步骤6；在合适的处理程序执行请求前调用，
        ///        这个时候，Session就可以用了 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            BizHttpApp.PreRequestHandlerExecute(sender, e);
        }

        /// <summary>
        /// 步骤7.当处理程序完成对请求的处理后被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            BizHttpApp.PostRequestHandlerExecute(sender, e);
        }



        public virtual void Application_EndRequest(object sender, EventArgs e)
        {
            BizHttpApp.EndRequest(sender, e);

        }



        public virtual void Application_Error(object sender, EventArgs e)
        {

        }

        public virtual void Session_End(object sender, EventArgs e)
        {
            BizHttpApp.SessionEnd(sender,e);

        }

        public virtual void Application_End(object sender, EventArgs e)
        {
            BizHttpApp.End(sender, e);
        }
    }
}