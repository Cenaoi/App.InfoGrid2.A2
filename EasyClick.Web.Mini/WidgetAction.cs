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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 微型组件事务
    /// </summary>
    public partial class WidgetAction : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region 字段

        protected internal Control m_UserControl;

        protected internal string m_Action;
        protected internal string m_ActionPs;

        /// <summary>
        /// 组件路径
        /// </summary>
        protected internal string m_WidgetPath;

        /// <summary>
        /// 执行模式 , 组件名称,组件函数,组件事件
        /// </summary>
        protected internal string m_RMode = string.Empty;

        /// <summary>
        /// 组件名称
        /// </summary>
        protected internal string m_SubWidgetName;

        /// <summary>
        /// 组件函数
        /// </summary>
        protected internal string m_SubWidgetMethod;

        /// <summary>
        /// 组件事件名称
        /// </summary>
        protected internal string m_SubWidgetEvent;

        /// <summary>
        /// 客户端ID = ClientID
        /// </summary>
        protected internal string m_CID = string.Empty;

        /// <summary>
        /// 时候为用户自定义
        /// </summary>
        protected internal bool m_IsUser = false;

        /// <summary>
        /// 延迟加载
        /// </summary>
        protected internal bool m_EcDelay = false;

        /// <summary>
        /// 语言或目录的名称 “CH"、EN
        /// </summary>
        protected internal string m_Lang;

        /// <summary>
        /// 组件参数
        /// </summary>
        protected internal string m_WidgetPs;

        /// <summary>
        /// 超链接的查询语句
        /// </summary>
        protected internal string m_Query;

        /// <summary>
        /// post 提交标记，
        /// </summary>
        protected internal bool m_IsPost = false;

        /// <summary>
        /// 返回客户端的格式
        /// </summary>
        protected internal string m_ReturnFormat = "html";



        internal SortedList<string, string> m_Attrs = new SortedList<string, string>();

        protected internal Exception m_Exception = null;

        protected internal string m_PID;

        #endregion

        readonly string[] SPLIT_TTT = new string[]{ "$$$"};

        private void ProWidgets()
        {
            if (string.IsNullOrEmpty(m_WidgetPs))
            {
                return;
            }

            string[] ps = m_WidgetPs.Split(SPLIT_TTT, StringSplitOptions.RemoveEmptyEntries);

            string key, value;
            int n ;

            for (int i = 0; i < ps.Length; i++)
            {
                string item = ps[i];

                n = item.IndexOf("=");

                if (n > 0)
                {
                    key = item.Substring(0, n);
                    value = item.Substring(n + 1);

                    m_Attrs.Add(key, value);
                }
                else
                {
                    m_Attrs.Add(item, string.Empty);
                }
                    
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsPostBack
        {
            get
            {
                return m_IsPost;
            }
        }

        /// <summary>
        /// 安全允许访问
        /// </summary>
        /// <returns></returns>
        protected virtual bool SecurityAllow()
        {
            return true;
        }


        private bool GetIsPost(string isPostStr)
        {
            bool isPost = false;

            //判断有没有这个标记，作为提交标记
            if (!string.IsNullOrEmpty(isPostStr) && isPostStr == "0")
            {
                isPost = false;
            }
            else if (!string.IsNullOrEmpty(isPostStr) && isPostStr == "1")
            {
                isPost = true;
            }
            else if (this.Request.Form.Count > 0)
            {
                isPost = true;
            }

            return isPost;
        }

        private bool GetIsUser(string langInt)
        {
            bool isUser = false;

            if (langInt != null && (langInt == "1" || "true".Equals(langInt.ToLower(), StringComparison.Ordinal)))
            {
                isUser = true;
            }

            return isUser;
        }

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);


            m_Action = Request.QueryString["__Action"];
            m_ActionPs = Request.QueryString["__ActionPs"];

            m_WidgetPath = Request.QueryString["__Path"];

            m_RMode = Request.QueryString["__RMode"];
            m_SubWidgetEvent = Request.QueryString["__SubEvent"];
            m_SubWidgetName = Request.QueryString["__SubName"];
            m_SubWidgetMethod = Request.QueryString["__SubMethod"];



            m_CID = Request.QueryString["__CID"];
            m_PID = Request.QueryString["__PID"];

            m_Lang = Request.QueryString["__Lang"];
            m_WidgetPs = Request.QueryString["__WidgetPs"];


            //if (!string.IsNullOrEmpty(m_WidgetPath) && !m_WidgetPath.EndsWith(".aspx"))
            //{
            //    m_WidgetPath += ".aspx";
            //}


            if ("undefined" == m_Action ) m_Action = string.Empty;
            if ("undefined" == m_ActionPs) m_ActionPs = string.Empty;
            if ("undefined" == m_SubWidgetName) m_SubWidgetName = string.Empty;
            if ("undefined" == m_SubWidgetEvent) m_SubWidgetEvent = string.Empty;
            if ("undefined" == m_SubWidgetMethod) m_SubWidgetMethod = string.Empty;


            string langInt = Request.QueryString["__IsUser"];
            string isPost = Request.QueryString["__IsPost"];
            string ecDelay = Request.QueryString["__EcDelay"];

            m_Query = Request.QueryString["__Query"];

            m_ReturnFormat = WebUtil.Query("__ReturnFormat","script");

        


            //判断有没有这个标记，作为提交标记
            m_IsPost = GetIsPost(isPost);

            bool.TryParse(ecDelay,out  m_EcDelay);

            m_IsUser = GetIsUser(langInt);


            bool secAllow = SecurityAllow();

            
            if (!secAllow)
            {
                this.Response.Clear();

                this.Response.Write("alert('没有操作权限.')");

                this.Response.End();

                return;
            }

            ProWidgets();
                
            //widgetPath = "~/Controls/Widgets/Tests/LoginBox.ascx";

            if (this.m_WidgetPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    StringWriter sw = new StringWriter();

                    MiniScript miScript = MiniScriptManager.ClientScript;
                    miScript.ReadOnly = true;
                    miScript.Items.Clear();

                    ITemplate tmp = Page.LoadTemplate(this.m_WidgetPath);

                    string script = miScript.ToString();

                    this.Response.Clear();
                    this.Response.Write(script);
                    this.Response.End();
                }
                catch (Exception ex)
                {
                    log.Error(ex.InnerException);
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.MinValue;
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

        }


        /// <summary>
        /// 获取控件路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetControlPath()
        {
            return m_WidgetPath;

        }

        /// <summary>
        /// 清理所有 CheckBox 的默认选项
        /// </summary>
        protected void CheckBoxClearAll()
        {
            if (!this.IsPostBack)
            {
                return;
            }

            if (m_UserControl == null)
            {
                log.Debug("con = null");
                return;
            }

            CheckBoxChildenClearALL(m_UserControl);

            //foreach (Control item in m_UserControl.Controls)
            //{
            //    if (item is CheckBox)
            //    {
            //        ((CheckBox)item).Checked = false;
            //    }
            //}
            
        }

        private void CheckBoxChildenClearALL(Control parent)
        {
            if (!parent.HasControls())
            {
                return;
            }

            foreach (Control item in parent.Controls)
            {
                if (item is CheckBox)
                {
                    ((CheckBox)item).Checked = false;
                }
                else
                {
                    CheckBoxChildenClearALL(item);
                }
            }
        }

        protected void SetMiniControlValue(string cid)
        {

            foreach (string item in Request.Form.Keys)
            {
                if (item != null && item.Length < cid.Length && !item.StartsWith(cid))
                {
                    continue;
                }

                string keyName = item.Substring(cid.Length);

                Control child = m_UserControl.FindControl(keyName);

                if (child == null)
                {
                    continue;
                }

                string value = Request.Form[item];

                if (child is CheckBox)
                {
                    ((CheckBox)child).Checked = true;
                }
                else
                {
                    MiniHelper.SetValue(child, value);
                }
            }

            LoadPostData(m_UserControl);


            MiniScriptManager.ClientScript.ReadOnly = false;
            MiniScriptManager.ClientScript.Items.Clear();
        }

        protected void LoadPostData(Control parent)
        {
            foreach (Control conSub in parent.Controls)
            {
                if (conSub is IMiniControl)
                {
                    ((IMiniControl)conSub).LoadPostData();
                }

                if (conSub.HasControls())
                {
                    LoadPostData(conSub);
                }
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            MiniScriptManager.ClientScript.ReadOnly = true;

  
            string path = GetControlPath();

            HttpContext context = HttpContext.Current;
            string newQuery = string.Empty;

            if (!string.IsNullOrEmpty(m_Query))
            {
                newQuery = m_Query.Replace("$$$", "&");
            }

            if (!string.IsNullOrEmpty(newQuery) && newQuery.Length == 0)
            {
                newQuery += string.Format("__CID={0}&__IsPost=true", m_CID);
            }
            else
            {
                newQuery += string.Format("&__CID={0}&__IsPost=true", m_CID);
            }

            context.RewritePath(path, "", newQuery,false);

            try
            {
                m_UserControl = Page.LoadControl(path);
            }
            catch (Exception ex)
            {
                log.Debug("con = null, Path=" + path);
                log.Error(ex);

                return;
            }
            
            

            //对控件进行初始值设置
            SetChildAttributes(m_UserControl);

            CheckBoxClearAll();

            m_UserControl.ID = m_CID;

            string cid = m_CID + "_";

            SetMiniControlValue(cid);

            this.Controls.Add(m_UserControl);



        }

        /// <summary>
        /// 对控件进行初始值设置
        /// </summary>
        /// <param name="con"></param>
        protected void SetChildAttributes(Control con)
        {
            if (m_Attrs == null || m_Attrs.Count == 0)
            {
                return;
            }

            Type conT = con.GetType();

            foreach (string key in m_Attrs.Keys)
            {
                PropertyInfo prop = conT.GetProperty(key);

                if (prop == null)
                {
                    continue;
                }

                string txt = this.m_Attrs[key];

                object v = txt;

                if (v != null && v.GetType() != prop.PropertyType)
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

        public string CID
        {
            get
            {
                NameValueCollection ps = Request.QueryString;

                if (ps["__CID"] != null && !string.IsNullOrEmpty(ps["__CID"]))
                {
                    return ps["__CID"];
                }

                return this.ClientID;
            }
        }

        /// <summary>
        /// 获取参数的数量
        /// </summary>
        /// <returns></returns>
        protected int GetInvokeParamCount()
        {
            int paramCount = 0;

            if (m_ActionPs == null)
            {
                return paramCount;
            }


            if (m_ActionPs.StartsWith("{") && m_ActionPs.EndsWith("}"))
            {
                JObject obj = (JObject)JsonConvert.DeserializeObject(m_ActionPs);

                foreach (JProperty prop in obj.Properties())
                {
                    paramCount++;
                }
            }
            else if (!string.IsNullOrEmpty(m_ActionPs))
            {
                paramCount = 1;
            }
            else
            {
                paramCount = 0;
            }

            return paramCount;
        }

        protected object[] GetInvokeParams(ParameterInfo[] ps)
        {
            object[] objs = new object[ps.Length];

            if (string.IsNullOrEmpty(m_ActionPs))
            {
                return new object[0];
            }

            if (m_ActionPs.StartsWith("{") && m_ActionPs.EndsWith("}"))
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

                    if(token == null)
                    {
                        throw new Exception(string.Format("解析提交的参数 actionPs 错误. i={0}, pi.Name={1}", i,pi.Name));
                    }

                    if (token.Type == JTokenType.String && t == typeof(string))
                    {
                        objs[i] = token.Value<string>();    // Convert.ChangeType(token, typeof(string));
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

        /// <summary>
        /// 执行返回的值
        /// </summary>
        protected object m_InvokeReturnValue = null;

        /// <summary>
        /// 获取函数名称
        /// </summary>
        /// <param name="contT"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        protected virtual MethodInfo GetWidgetMethod(Type contT, string methodName,int paramCount)
        {
            MethodInfo[] methods = contT.GetMethods();

            MethodInfo curMi = null;

            //int paramCount = (args == null ?0: args.Length);

            foreach (MethodInfo mi in methods)
            {
                if (!methodName.Equals(mi.Name, StringComparison.Ordinal))
                {
                    continue;
                }

                if (mi.GetParameters().Length == paramCount)
                {
                    curMi = mi;
                    break;
                }
            }

            return curMi;
        }


        /// <summary>
        /// 执行控件函数
        /// </summary>
        protected virtual void Shell_By_WidgetMethod()
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

                HttpContext.Current.Items["EC_Command"] = m_SubWidgetMethod;

                Type subConT = subCon.GetType();
                string methodName = m_SubWidgetMethod;


                int paramCount = GetInvokeParamCount();

                MethodInfo mi = GetWidgetMethod(subConT, methodName,paramCount);

                object[] ps = GetInvokeParams(mi.GetParameters());
                
                //, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance);

                if (mi == null)
                {
                    log.ErrorFormat("没有找到相关的函数名称 \"{0}\"", m_SubWidgetMethod);

                    string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("没有找到相关的函数名称 \"{0}.{1}\"",m_SubWidgetName,  m_SubWidgetMethod));

                    MiniScript.Add("alert(\"{0}\");", txt);

                    return;
                }

                mi.Invoke(subCon, ps);
            }
            catch (Exception ex)
            {
                string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("执行函数错误,函数名称 \"{0}.{1}()\".", m_SubWidgetName, m_SubWidgetMethod));

                log.Error(txt,ex);

                MiniScript.Add("alert(\"{0}\");", txt);
            }

        }


        /// <summary>
        /// 执行控件事件
        /// </summary>
        protected virtual void Shell_By_WidgetEvent()
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

                MethodInfo ei = subConT.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance);

                if (ei == null)
                {
                    log.ErrorFormat("没有找到相关的方法名称 \"{0}\"", "On" + m_SubWidgetEvent);

                    string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("没有找到相关的方法名称 \"{0}\"", "On" + m_SubWidgetEvent));

                    MiniScript.Add("alert(\"{0}\");", txt);

                    return;
                }

                ei.Invoke(subCon, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("事件执行错误,事件名称 \"{0}\".", "On" + m_SubWidgetEvent));

                MiniScript.Add("alert(\"{0}\");", txt);
            }

        }

        /// <summary>
        /// 执行普通函数.
        /// </summary>
        protected virtual void Shell_By_Command()
        {
            Type uType = m_UserControl.GetType();

            MethodInfo mi = uType.GetMethod(m_Action);

            HttpContext.Current.Items["EC_Command"] = m_Action;

            if (mi != null)
            {
                try
                {
                    object[] ps = GetInvokeParams(mi.GetParameters());

                    m_InvokeReturnValue = mi.Invoke(m_UserControl, ps);

                }
                catch (Exception ex)
                {
                    m_Exception = ex.InnerException;

                    log.Error(ex.InnerException);

                    string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("执行错误 \"{0}\".", m_Action));
                    MiniScript.Add("alert(\"{0}\");", txt);
                }
            }
            else
            {
                string txt = EasyClick.Web.Mini.Utility.JsonUtility.ToJson(string.Format("没有找到相关的方法名称 \"{0}\".", m_Action));

                MiniScript.Add("alert(\"{0}\");", txt);
            }
        }




        protected override void OnSaveStateComplete(EventArgs e)
        {
            if (m_UserControl == null)
            {
                log.Error("con is not null ");
                return;
            }

            if (!string.IsNullOrEmpty(m_Action))
            {
                Shell_By_Command();
            }
            else if (!string.IsNullOrEmpty(m_SubWidgetName))
            {
                if (!string.IsNullOrEmpty(m_SubWidgetMethod))
                {
                    Shell_By_WidgetMethod();
                }
                else if (!string.IsNullOrEmpty(m_SubWidgetEvent))
                {
                    Shell_By_WidgetEvent();
                }
            }


            base.OnSaveStateComplete(e);

        }

        protected Control FindControlByChilds(Control parent, string id)
        {
            if (parent == null || !parent.HasControls())
            {
                return null;
            }

            Control tCon = null;

            foreach (Control con in parent.Controls)
            {
                if (id.StartsWith(con.ClientID))
                {
                    tCon = con;
                    break;
                }

                if (id == con.ID)
                {
                    tCon = con;
                    break;
                }

                Control tmpCon = FindControlByChilds(con, id);

                if (tmpCon != null)
                {
                    tCon = tmpCon;
                    break;
                }
            }

            return tCon;       
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            }

            if (m_Exception != null)
            {

                StringBuilder exSb = new StringBuilder(m_Exception.Message);
                exSb = exSb.Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\"");


                if (Request.QueryString["__ReturnFormat"] == "script")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"alert('错误!\r\n\r\n");
                    sb.Append(exSb.ToString());
                    sb.Append("!');");

                    Response.Clear();
                    Response.Write(sb.ToString());
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    //sb.AppendLine("<script>");
                    sb.Append(@"alert('错误!\r\n\r\n").Append(exSb.ToString()).Append("!');");
                    //sb.AppendLine("</script>");

                    Response.Clear();
                    Response.Write(sb.ToString());
                }

                return;
            }



            if (this.m_ReturnFormat == "script" && this.IsPostBack)
            {
                Response.Clear();

                MiniScriptManager.ClientScript.Write(writer);
            }
            else if (this.m_ReturnFormat == "text")
            {
                Response.Clear();

                if (m_InvokeReturnValue != null)
                {
                    Response.Write(m_InvokeReturnValue.ToString());
                }

            }
            else
            {
                NameValueCollection qs = Request.QueryString;


                m_UserControl.RenderControl(writer);


                MiniWidget mw = new MiniWidget();
                mw.ClientID = m_CID;

                mw.EcDelay = m_EcDelay;
                mw.EcDesignMode = false;
                mw.EcReturnFormat = "html";

                mw.Language = "";// WebUtility.Query("__Lang", this.DisplayLanguage);
                mw.PanelID = m_PID;
                mw.Params = m_WidgetPs;
                mw.Path = m_WidgetPath;
                mw.Query = m_Query;
                mw.Con = (UserControl)m_UserControl;

                mw.IsUser = true;

                mw.RenderAJAX(writer);

                mw.Dispose();
            }
        }



    }
}
