using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;
using System.Web;
using System.IO;
using EC5.SystemBoard.IO;
using EC5.SystemBoard.EcReflection;
using EC5.SystemBoard;
using System.Web.UI;
using EC5.SystemBoard.Interfaces;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HWQ.Entity.LightModels;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class EcWidgetAction:WidgetAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string GetObjFullname(string[] strList, int n1)
        {
            int n2 = strList.Length - 1;

            return GetObjFullname(strList, n1, n2);
        }

        private string ToJson(object v)
        {
            if (v == null)
            {
                return string.Empty;
            }

            string vStr = v.ToString();

            if (vStr.Length == 0)
            {
                return string.Empty;
            }

            return ToJson(vStr);
        }


        /// <summary>
        /// 字符串转换为 json 数据;
        /// 默认左右两边是用双引号括起来的
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string ToJson(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '\"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '/': sb.Append("\\/"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default: sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        private string GetObjFullname(string[] strList, int n1, int n2)
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

        protected override bool SecurityAllow()
        {
            string path = m_WidgetPath;

            HttpContext context = HttpContext.Current;

            //string viewUri = context.Request.QueryString["ViewUri"];

            if (string.IsNullOrEmpty(path))
            {
                return true;
            }

            if (path.StartsWith("~/APP/") || path.StartsWith("/APP"))
            {

            }
            else
            {
                return true;
            }

            bool isAllow = true;
            ISecurity sec;

            List<ISecurity> secList = EC5.SystemBoard.Web.BizHttpApp.SecModuleList;

            for (int i = 0; i < secList.Count; i++)
            {
                sec = secList[i];

                try
                {
                    isAllow = sec.Allow(path);
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

            return isAllow;
        }

        private string RenewViewUri(string uriStr)
        {
            if (uriStr.StartsWith("~/app/Modules/", StringComparison.OrdinalIgnoreCase))
            {
                return "view://App/" + uriStr.Substring(14) ;
            }

            if (!uriStr.StartsWith("view://", StringComparison.Ordinal))
            {
                if (uriStr.StartsWith("/"))
                {
                    uriStr = string.Format("view://{0}{1}", EC5.SystemBoard.SysBoardManager.DefaultAppCode, uriStr);
                }
                else
                {
                    uriStr = string.Format("view://{0}/{1}", EC5.SystemBoard.SysBoardManager.DefaultAppCode, uriStr);
                }
            }

            return uriStr;
        }

        protected override void CreateChildControls()
        {
            //base.CreateChildControls();

            MiniScriptManager.ClientScript.ReadOnly = true;


            string path = GetControlPath();

            HttpContext context = HttpContext.Current;
            string newQuery = m_Query.Replace("$$$", "&");

            if (newQuery.Length > 0 && !newQuery.EndsWith("&", StringComparison.Ordinal))
            {
                newQuery += "&";
            }

            newQuery += string.Format("__CID={0}&__IsPost=true", m_CID);

            context.RewritePath(path, "", newQuery, false);

            try
            {
                if ( File.Exists(MapPath(path)))
                {
                    m_UserControl = Page.LoadControl(path);

                    if (m_UserControl is EC5.SystemBoard.Web.UI.WidgetControl)
                    {
                        //string moduleName = GetModuleName();

                        EcAppInfo app = SysBoardManager.CurrentApp;
                        string moduleCode = app.DebugModuleCode;

                        EcModuleInfo moduleInfo = SysBoardManager.GetModuleInfo(moduleCode);

                        ((EC5.SystemBoard.Web.UI.WidgetControl)m_UserControl).SetModuleInfo(moduleInfo);
                    }
                }
                else
                {
                    string uriStr = RenewViewUri(m_WidgetPath);

                    EcTypeInfo typeT = SysBoardManager.GetViewType(uriStr);

                     // typeT.Module.Code + "/" + typeT.FullPath;

                    m_UserControl = Activator.CreateInstance(typeT.Src) as UserControl;


                    if (m_UserControl is EC5.SystemBoard.Web.UI.WidgetControl)
                    {
                        string moduleName = GetModuleName();

                        EcModuleInfo moduleInfo = SysBoardManager.GetModuleInfo(moduleName);

                        ((EC5.SystemBoard.Web.UI.WidgetControl)m_UserControl).SetModuleInfo(moduleInfo);
                    }
                }

                EcContext.Current.Items["EC_ViewUri"] = context.Request.Url.ToString();
            }
            catch (Exception ex)
            {
                log.Debug("con = null, Path=" + path);
                log.Error(ex);

                throw ex;
            }


            m_UserControl.ID = m_CID;

            //对控件进行初始值设置
            SetChildAttributes(m_UserControl);

            CheckBoxClearAll();

            if (!(m_UserControl is EC5.SystemBoard.Web.UI.WidgetControl))
            {
                string cid = m_CID + "_";
                SetMiniControlValue(cid);

                this.Controls.Add(m_UserControl);   //警告：这一行代码放这里或最后,出现初始化错误 FrameworkInitialize

                CheckBoxClearAll();
                SetMiniControlValue(cid);
            }
            else
            {
                this.Controls.Add(m_UserControl);   //警告：这一行代码放这里或最后,出现初始化错误 FrameworkInitialize
            }


        }


        protected string GetModuleName()
        {
            if (m_WidgetPath.StartsWith("~", StringComparison.Ordinal))
            {
                if (m_WidgetPath.StartsWith("~/app/Modules/", StringComparison.Ordinal))
                {

                }
                else
                {
                    return m_WidgetPath;
                }
            }


            EC5.SystemBoard.EcReflection.EcAppInfo app = EC5.SystemBoard.SysBoardManager.CurrentApp;

            string uriStr = RenewViewUri(m_WidgetPath);

            Uri uri = new Uri(uriStr);

            string[] paths = uri.LocalPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string appCode = uri.DnsSafeHost;
            string moduleCode = paths[0];

            return moduleCode;
        }

        protected override string GetControlPath()
        {
            if (m_IsUser && (m_WidgetPath.StartsWith("~", StringComparison.Ordinal) || m_WidgetPath.StartsWith("/", StringComparison.Ordinal) == false))
            {
                return m_WidgetPath;
            }

            if (m_WidgetPath.StartsWith("~", StringComparison.Ordinal))
            {
                if (m_WidgetPath.StartsWith("~/app/Modules/", StringComparison.OrdinalIgnoreCase))
                {

                }
                else
                {
                    return m_WidgetPath;
                }
            }

            #region 构造 控件路径

            EC5.SystemBoard.EcReflection.EcAppInfo app = EC5.SystemBoard.SysBoardManager.CurrentApp;

            string uriStr = RenewViewUri(m_WidgetPath);
            
            Uri uri = new Uri(uriStr);

            string[] paths = uri.LocalPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string appCode = uri.DnsSafeHost;
            string moduleCode = paths[0];

            string viewPath = GetObjFullname(paths, 1);

            string ascxPath;

            if (app.IsOneMode && moduleCode.Equals(app.DebugModuleCode, StringComparison.OrdinalIgnoreCase))
            {
                ascxPath = string.Concat( "~/" , viewPath , ".ascx");
            }
            else
            {
                ascxPath = string.Concat( "~/" , app.Code , "/" , moduleCode , "/" , viewPath , ".ascx");
            }

            #endregion

            return ascxPath;


        }



        /// <summary>
        /// 执行控件函数
        /// </summary>
        protected override void Shell_By_WidgetMethod()
        {
            string strParams = m_WidgetPs;

            try
            {
                Control subCon = null;

                if (m_UserControl.ClientID == m_SubWidgetName)
                {
                    subCon = m_UserControl;
                }
                else
                {
                    subCon = FindControlByChilds(m_UserControl, m_SubWidgetName);
                }

                if (subCon == null)
                {
                    throw new Exception(string.Format("事件或函数源“{0}”对象不存在", m_SubWidgetName));
                }

                string methodName = m_SubWidgetMethod;
                HttpContext.Current.Items["EC_Command"] = m_SubWidgetMethod;
                Type subConT = subCon.GetType();

                if (subCon is EC5.SystemBoard.Web.UI.IExMethodCollection)
                {
                    EC5.SystemBoard.Web.UI.IExMethodCollection exMethods = (EC5.SystemBoard.Web.UI.IExMethodCollection)subCon;

                    EC5.SystemBoard.Web.UI.ExMethodInfo exMethod = exMethods.GetExMethod(m_SubWidgetMethod);

                    if (exMethod != null)
                    {
                        MethodInfo srcMI = exMethod.SrcMethod;
                        object[] srcPs = GetInvokeParams(srcMI.GetParameters());

                        exMethod.Invoke(srcPs);

                        return;
                    }
                }


                int paramCount = GetInvokeParamCount();

                MethodInfo mi = GetWidgetMethod(subConT, methodName, paramCount);

                object[] ps = GetInvokeParams_v2(mi.GetParameters());

                //, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance);

                if (mi == null)
                {
                    log.ErrorFormat("没有找到相关的函数名称 \"{0}\"", m_SubWidgetMethod);

                    string txt = ToJson(string.Format("没有找到相关的函数名称 \"{0}.{1}\"", m_SubWidgetName, m_SubWidgetMethod));

                    MiniScript.Add("alert(\"{0}\");", txt);

                    return;
                }

                mi.Invoke(subCon, ps);
            }
            catch (Exception ex)
            {
                string txt = ToJson(string.Format("执行函数错误,函数名称 \"{0}.{1}()\".", m_SubWidgetName, m_SubWidgetMethod));

                log.Error(txt, ex);

                MiniScript.Add("alert(\"{0}\");", txt);
            }

        }


        /// <summary>
        /// 执行控件事件
        /// </summary>
        protected override void Shell_By_WidgetEvent()
        {

            try
            {
                Control subCon = null;

                if (m_UserControl.ClientID == m_SubWidgetName)
                {
                    subCon = m_UserControl;
                }
                else
                {
                    subCon = FindControlByChilds(m_UserControl, m_SubWidgetName);
                }

                HttpContext.Current.Items["EC_Command"] = "On" + m_SubWidgetEvent;

                Type subConT = subCon.GetType();
                string methodName = "On" + m_SubWidgetEvent;


                if (subCon is EC5.SystemBoard.Web.UI.IExMethodCollection)
                {
                    EC5.SystemBoard.Web.UI.IExMethodCollection exMethods = (EC5.SystemBoard.Web.UI.IExMethodCollection)subCon;

                    EC5.SystemBoard.Web.UI.ExMethodInfo exMethod = exMethods.GetExMethod(m_SubWidgetMethod);

                    if (exMethod != null)
                    {
                        MethodInfo srcMI = exMethod.SrcMethod;
                        object[] srcPs = GetInvokeParams(srcMI.GetParameters());

                        exMethod.Invoke(srcPs);

                        return;
                    }
                }


                // BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly

                MethodInfo ei = subConT.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance );

                if (ei == null)
                {
                    log.ErrorFormat("没有找到相关的方法名称 \"{0}\"", "On" + m_SubWidgetEvent);

                    string txt = ToJson(string.Format("没有找到相关的方法名称 \"{0}\"", "On" + m_SubWidgetEvent));

                    MiniScript.Add("alert(\"{0}\");", txt);

                    return;
                }

                ei.Invoke(subCon, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                string txt = ToJson(string.Format("事件执行错误,事件名称 \"{0}\".", "On" + m_SubWidgetEvent));

                MiniScript.Add("alert(\"{0}\");", txt);
            }

        }

        /// <summary>
        /// 执行普通函数.
        /// </summary>
        protected override void Shell_By_Command()
        {
            Type uType = m_UserControl.GetType();


            HttpContext.Current.Items["EC_Command"] = m_Action;

            if (m_UserControl is EC5.SystemBoard.Web.UI.IExMethodCollection)
            {
                EC5.SystemBoard.Web.UI.IExMethodCollection exMethods = (EC5.SystemBoard.Web.UI.IExMethodCollection)m_UserControl;

                EC5.SystemBoard.Web.UI.ExMethodInfo exMethod = exMethods.GetExMethod(m_SubWidgetMethod);

                if (exMethod != null)
                {
                    MethodInfo srcMI = exMethod.SrcMethod;

                    try
                    {
                        object[] srcPs = GetInvokeParams(srcMI.GetParameters());

                        exMethod.Invoke(srcPs);
                    }
                    catch (Exception ex)
                    {
                        m_Exception = ex.InnerException;

                        log.Error(ex.InnerException);

                        string txt = ToJson(string.Format("执行错误 \"{0}\".", m_Action));
                        MiniScript.Add("alert(\"{0}\");", txt);
                    }

                    return;
                }
            }



            MethodInfo mi = uType.GetMethod(m_Action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public );

            if (mi != null)
            {
                try
                {
                    ParameterInfo[] piList = mi.GetParameters();

                    object[] ps = GetInvokeParams_v2(piList);

                    m_InvokeReturnValue = mi.Invoke(m_UserControl, ps);

                }
                catch (Exception ex)
                {
                    m_Exception = ex.InnerException;

                    log.Error(ex.InnerException);

                    string txt = ToJson(string.Format("执行错误 \"{0}\".", m_Action));
                    MiniScript.Add("alert(\"{0}\");", txt);
                }
            }
            else
            {
                string txt = ToJson(string.Format("没有找到相关的方法名称 \"{0}\".", m_Action));

                MiniScript.Add("alert(\"{0}\");", txt);
            }
        }



        protected object[] GetInvokeParams_v2(ParameterInfo[] ps)
        {
            object[] objs = new object[ps.Length];


            if (m_RMode == "callback" && ps.Length > 0)
            {
                string callback_owner = Request.QueryString["__callback_owner"];

                string rdata = Request.QueryString["__rdata"];

                Type ownerT = null;
                object owner;

                if (callback_owner == "Window")
                {

                }

                if (ownerT != null)
                {
                    owner = Activator.CreateInstance(ownerT);
                }

                int lastIndex = ps.Length - 1;

                ParameterInfo lastPI = ps[lastIndex];

                if (lastPI.ParameterType == typeof(string))
                {
                    objs[lastIndex] = m_ActionPs;

                    return objs;
                }
            }


            if (m_ActionPs != null && m_ActionPs.StartsWith("{") && m_ActionPs.EndsWith("}"))
            {
                //多参数


                JObject obj = (JObject)JsonConvert.DeserializeObject(m_ActionPs);

                //int n = 0;

                //object value = null;

                for (int i = 0; i < ps.Length; i++)
                {
                    ParameterInfo pi = ps[i];
                    Type t = pi.ParameterType;

                    JToken token = obj[pi.Name];

                    if (token == null)
                    {
                        throw new Exception(string.Format("解析提交的参数 actionPs 错误. i={0}, pi.Name={1}", i, pi.Name));
                    }

                    if (token.Type == JTokenType.String && t == typeof(string))
                    {
                        //objs[i] = token.Value<string>();    // Convert.ChangeType(token, typeof(string));

                        var str = token.Value<string>();    // Convert.ChangeType(token, typeof(string));

                        if (str.StartsWith("ref "))
                        {
                            string refName = str.Substring(4).Trim();// EC5.Utility.Web.WebUtil.Form(pi.Name);

                            str = EC5.Utility.Web.WebUtil.Form(refName);
                        }

                        objs[i] = str;

                    }
                    else
                    {
                        objs[i] = Convert.ChangeType(token.ToString(), t);
                    }

                }

            }
            else
            {
                if (ps.Length == 1)
                {
                    Type pType = ps[0].ParameterType;

                    if (pType == typeof(string))
                    {
                        objs = new object[] { m_ActionPs };
                    }
                    else if (pType == typeof(int))
                    {
                        int tmpV = 0;

                        if (int.TryParse(m_ActionPs, out tmpV))
                        {
                            objs = new object[] { tmpV };
                        }
                    }
                    else
                    {
                        objs = new object[] { Convert.ChangeType(m_ActionPs, pType) };
                    }
                }
            }


            return objs;
        }


    }
}
