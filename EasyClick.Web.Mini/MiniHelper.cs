using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using EasyClick.Web.Mini.Utility;
using System.IO;
using System.Collections;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// HTML Mini 助手类
    /// </summary>
    public static class MiniHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        public static MiniWindow Parent
        {
            get
            {
                MiniWindow mw = new MiniWindow();
                mw.m_LevelIndex = 1;

                return mw;
            }
        }

        /// <summary>
        /// 组件的脚本
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static string WidgetScript(Control con)
        {
            return WidgetScript(con, MiniConfiguration.ActionPath);
        }

        /// <summary>
        /// 组件的脚本
        /// </summary>
        /// <returns></returns>
        public static string WidgetScript(Control con,string actionPath)
        {
            string cID = con.ClientID;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("<!--");


            sb.AppendLine("$(document).ready(function(){");

            sb.AppendFormat("    var {0} = new Mini.ui.Widget({{", cID).AppendLine();
            sb.AppendFormat("        __Path: '{0}',", con.TemplateControl.AppRelativeVirtualPath).AppendLine();
            sb.AppendFormat("        __CID: '{0}',", cID).AppendLine();
            sb.AppendFormat("        __PID: '{0}__Box',", cID).AppendLine();
            sb.AppendFormat("        __IsUser: 'True',").AppendLine();
            sb.AppendFormat("        __Lang: '{0}',", "CH").AppendLine();
            sb.AppendFormat("        __WidgetPs: '',").AppendLine();
            sb.AppendFormat("        __Query: '{0}',", con.Page.Request.QueryString.ToString()).AppendLine();
            sb.AppendFormat("        __ReturnFormat: '{0}',", "script").AppendLine();
            sb.AppendLine("        __EcDelay: 'False'").AppendLine();
            sb.AppendLine("    });");

            sb.AppendFormat("    {0}.ActionPath = '{1}';", cID, actionPath).AppendLine();

            if (con.Parent == null)
            {
                sb.AppendFormat("    window.{0} = {0};",cID).AppendLine();
            }
            else
            {
                string pID = con.Parent.ID;

                if (string.IsNullOrEmpty(pID))
                {
                    pID = cID.Substring(0, cID.Length - 3);
                }

                sb.AppendFormat("    window.{0} = window.{1} = {1};", pID, cID).AppendLine();
            }

            sb.AppendLine("});");

            sb.AppendLine("-->");
            sb.AppendLine("</script>");

            return sb.ToString();
        }


        public static string Submit(string form, string owner)
        {
            return string.Format("javascript:{0}.Submit({1})", form, owner);
        }

        public static string ValidateCode(string form)
        {
            return string.Format("if ( $('#{0}').valid() == false) {{ return false; }}", form);
        }

        /// <summary>
        /// 控件的默认属性索引
        /// </summary>
        static SortedDictionary<string, PropertyInfo> m_BufferControlDefaultPropertys = new SortedDictionary<string, PropertyInfo>();


        /// <summary>
        /// 取出控件的值 (默认属性的值)
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static object GetValue(Control con)
        {
            Type conT = con.GetType();
            
            PropertyInfo pi;

            if (m_BufferControlDefaultPropertys.ContainsKey(conT.FullName))
            {
                pi = m_BufferControlDefaultPropertys[conT.FullName];
            }
            else
            {
                //下面这行代码消耗 CPU （ conT.GetCustomAttributes(...) ) 
                object[] attrs = conT.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);

                if (attrs.Length == 0)
                {
                    return null;
                }

                DefaultPropertyAttribute dp = (DefaultPropertyAttribute)attrs[0];

                pi = conT.GetProperty(dp.Name);

                m_BufferControlDefaultPropertys.Add(conT.FullName, pi);
            }

            return pi.GetValue(con, null);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SSID()
        {
            string url = HttpContext.Current.Session.SessionID + "   " + DateTime.Now.ToString(); ;

            return url;
        }

        /// <summary>
        /// (JScript) 弹出 Html 的 Message 框
        /// </summary>
        /// <param name="message"></param>
        public static void Alert(string message)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            message = JsonUtility.ToJson(message);

            MiniScript.Add("alert(\"{0}\");",message);

        }

        /// <summary>
        /// (JScript) 弹出 Html 的 Message 框
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public static void Alert(string messageFormat, params object[] args)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            //messageFormat = JsonUtility.ToJson(messageFormat);// messageFormat.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");

            string mgr = string.Format(messageFormat, args);

            MiniScript.Add( "alert(\"{0}\");", JsonUtility.ToJson( mgr));
        }

        #region Redirect 跳转

        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="url"></param>
        public static void Redirect(string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            //MiniScript.Add("window.location.href='{0}';",url);

            MiniScript.Add("if($.browser.msie){{ window.navigate('{0}'); }}else{{ window.location.href='{0}'; }}",url);

        }

        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="frameName"></param>
        /// <param name="url"></param>
        public static void Redirect(string frameName, string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            //MiniScript.Add("window.frames['{0}'].location.href='{1}';",frameName,url);

            MiniScript.Add("$('#{0}').attr('src','{1}');", frameName, url);
        }


        /// <summary>
        /// (JScript) 跳转
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="url"></param>
        public static void Redirect(int frameIndex, string url)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            MiniScript.Add("window.frames[{0}].location.href='{1}';", frameIndex, url);
        }

        #endregion

        /// <summary>
        /// (JScript) 刷新页面
        /// </summary>
        /// <param name="url"></param>
        public static void Reload()
        {
            HttpResponse response = HttpContext.Current.Response;

            if (response == null)
            {
                return;
            }

            MiniScript.Add("window.location.reload();");

        }

        /// <summary>
        /// (JScript) 关闭窗体 
        /// </summary>
        public static void CloseForm()
        {
            MiniScript.Add("window.external.CloseForm();");
        }

        /// <summary>
        /// (JScript) 设置窗体标题
        /// </summary>
        /// <param name="title"></param>
        public static void SetTitle(string title)
        {
            MiniScript.Add("window.external.Text = \"{0}\";",JsonUtility.ToJson(title) );
        }

        /// <summary>
        /// 把 Request 的值赋予控件
        /// </summary>
        /// <param name="owner">this</param>
        public static void ValueFormRequest(Control owner)
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                return;
            }

            string wId = owner.ClientID;

            ValueFormRequest(owner, wId);
        }

        /// <summary>
        /// 把 Request 的值赋予控件
        /// </summary>
        /// <param name="owner">this</param>
        /// <param name="ownerPath">组件前缀</param>
        public static void ValueFormRequest(Control owner,string ownerPath)
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                return;
            }

            string wId = ownerPath;

            int wLen = ownerPath.Length;


            foreach (Control con in owner.Controls)
            {


                if (con.HasControls())
                {
                    ValueFormRequest(con,ownerPath);
                }

                if (con.ClientID.Length <= wLen + 1)
                {
                    continue;
                }

                string ps = con.ClientID.Substring(wLen + 1);
                string v =  context.Request.QueryString[ps];

                SetConValue(con, v);
            }
        }


        public static void SetValue(Control con, string value)
        {
            SetConValue(con, value);
        }

        static void SetConValue(Control con, string value)
        {
            if (value == null)
            {
                return;
            }

            value = System.Web.HttpUtility.UrlDecode(value);


            Type conT = con.GetType();
            PropertyInfo pi;

            lock (m_BufferControlDefaultPropertys)
            {
                if (!m_BufferControlDefaultPropertys.TryGetValue(conT.FullName, out pi))
                {
                    //这行 "GetCustomAttributes" 代码消耗 CPU
                    object[] attrs = conT.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);

                    if (attrs.Length == 0)
                    {
                        return;
                    }

                    DefaultPropertyAttribute dp = (DefaultPropertyAttribute)attrs[0];

                    pi = conT.GetProperty(dp.Name);

                    m_BufferControlDefaultPropertys.Add(conT.FullName, pi);
                }
            }


            try
            {
                if (pi.PropertyType == typeof(bool))
                {
                    bool objV = false;

                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Trim().ToUpper();

                        if(value == "FALSE" || value == "0")
                        {
                            
                        }
                        else
                        {
                            objV = true;
                        }
                    }

                    pi.SetValue(con, objV, null);

                }
                else
                {
                    object objV = Convert.ChangeType(value, pi.PropertyType);
                    pi.SetValue(con, objV, null);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("转换字符串失败,控件={0}, Value={1}, PropertyType={2}",con.ID, value,pi.PropertyType.FullName), ex);
            }
        }



        public static string GetItemJson(object item)
        {
            return GetItemJson(item,null, null);
        }

        /// <summary>
        /// 格式化对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="prop"></param>
        /// <param name="dataField"></param>
        /// <param name="dataFormats"></param>
        /// <returns></returns>
        static string DataFormatForObject(object item, PropertyDescriptor prop, string dataField, DataFormatCollection dataFormats)
        {

            string dataForamt = string.Empty;
            string v;

            if (dataFormats != null)
            {
                dataForamt = dataFormats.GetFormatString(dataField);

                if (!string.IsNullOrEmpty(dataForamt))
                {
                    v = string.Format(dataForamt, prop.GetValue(item));
                }
                else
                {
                    object srcValue = prop.GetValue(item);

                    if (srcValue == null)
                    {
                        v = string.Empty;
                    }
                    else
                    {
                        v = srcValue.ToString();
                    }
                }
            }
            else
            {
                object srcValue = prop.GetValue(item);

                if (srcValue == null)
                {
                    v = string.Empty;
                }
                else
                {
                    v = srcValue.ToString();
                }
            }

            return v;
        }

        public static string GetItemJson(object item,DataFormatCollection dataFormats,string[] fields)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (item is string )
            {
                string v = JsonUtility.ToJson(item);

                return v;
            }
            else if (item is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)item;
                PropertyDescriptorCollection propList = customTs.GetProperties();
                PropertyDescriptor prop = null;

                StringBuilder sb = new StringBuilder("{");

                if (fields != null && fields.Length > 0)
                {
                    List<string> srcFs = new List<string>(fields);
                    List<string> tmpFields = new List<string>();

                    for (int i = 0; i < propList.Count; i++)
                    {
                        string name = propList[i].Name;

                        if (srcFs.Contains(name))
                        {
                            tmpFields.Add(name);
                        }
                    }

                    int fieldCount = tmpFields.Count;

                    for (int i = 0; i < fieldCount; i++)
                    {
                        string tmpField = tmpFields[i];
                        prop = propList[tmpField];

                        string v = DataFormatForObject(item, prop, tmpField, dataFormats);
                        v = JsonUtility.ToJson(v);

                        if(prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
                        {
                            sb.AppendFormat("\"{0}\":{1}", prop.Name, v);
                        }
                        else
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\"", prop.Name, v);
                        }

                        if (i < fieldCount - 1) { sb.Append(","); } 
                    }

                }
                else if (propList.Count > 0)
                {
                    int fieldCount = propList.Count;

                    for (int i = 0; i < fieldCount; i++)
                    {
                        prop = propList[i];

                        string v = DataFormatForObject(item, prop, prop.Name, dataFormats);
                        v = JsonUtility.ToJson(v);

                        if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
                        {
                            sb.AppendFormat("\"{0}\":{1}", prop.Name, v);
                        }
                        else
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\"", prop.Name, v);
                        }

                        if (i < fieldCount - 1) { sb.Append(","); } 
                    }
                }

                sb.Append("}");

                return sb.ToString();
            }
            else
            {
                Type itemT = item.GetType();
                PropertyInfo[] propList = itemT.GetProperties();

                PropertyInfo prop;

                int fieldCount = propList.Length;

                StringBuilder sb = new StringBuilder("{");

                for (int i = 0; i < fieldCount; i++)
                {
                    prop = propList[i];

                    Type propT = prop.PropertyType;

                    //string v = DataFormatForObject(item, prop, prop.Name, dataFormats);

                    object tmpValue = prop.GetValue(item, null);

                    string v;

                    if (tmpValue != null)
                    {
                        v = tmpValue.ToString();
                    }
                    else
                    {
                        v = string.Empty;
                    }

                    v = JsonUtility.ToJson(v);

                    if (propT == typeof(Int16) ||
                        propT == typeof(Int32) ||
                        propT == typeof(Int64) ||
                        propT == typeof(double) ||
                        propT == typeof(float) ||
                        propT == typeof(UInt16) ||
                        propT == typeof(UInt32) ||
                        propT == typeof(UInt64) ||
                        propT == typeof(decimal))
                    {
                        if (v == null || v.Length == 0)
                        {
                            v = "0";
                        }

                        sb.AppendFormat("\"{0}\":{1}", prop.Name, v);
                    }
                    else
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\"", prop.Name, v);
                    }

                    if (i < fieldCount - 1) { sb.Append(","); }
                }


                sb.Append("}");

                return sb.ToString();
            }


        }


        public static void Eval(string script)
        {
            MiniScript.Add(script);
        }

        public static void EvalFormat(string scriptFormat, object arg0)
        {
            MiniScript.Add(scriptFormat, arg0);
        }

        public static void EvalFormat(string scriptFormat, object arg0, object arg1)
        {
            MiniScript.Add(scriptFormat, arg0,arg1);
        }

        public static void EvalFormat(string scriptFormat, params object[] args)
        {
            MiniScript.Add(scriptFormat, args);
        }

        public static void Tooltip(string message)
        {
            //MiniScript.Add("$.sticky(\"{0}\");", JsonUtility.ToJson(message));

            MiniScript.Add("Mini_Tooltop.show(\"{0}\");", JsonUtility.ToJson(message));
        }

        public static void Tooltip(string messageFormat,params object[] args)
        {
            string message = string.Format(messageFormat, args);

            //MiniScript.Add("$.sticky(\"{0}\");", JsonUtility.ToJson(message));

            MiniScript.Add("Mini_Tooltop.show(\"{0}\");", JsonUtility.ToJson(message));
        }

        /// <summary>
        /// 合并 Mini.js
        /// </summary>
        /// <param name="targetPath"></param>
        public static void UniteMiniSystemJS(string targetPath)
        {
            log.InfoFormat("{0}() Begin: 合并 Mini.js", System.Reflection.MethodBase.GetCurrentMethod().Name);

            HttpContext con = HttpContext.Current;

            if (con == null || con.Server == null) { return; }

            string uniteConfigFile = con.Server.MapPath("~/Core/Scripts/Mini/unite.txt");
            string targetFile = con.Server.MapPath(targetPath);

            StringBuilder fs = new StringBuilder();

            fs.AppendFormat("/// MiniHtml.js 创建日期:{0}", DateTime.Now).AppendLine();


            string[] jsFiles = File.ReadAllLines(uniteConfigFile);


            foreach (string jsFile in jsFiles)
            {
                if (jsFile.Trim().Length == 0) { continue; }

                string file = con.Server.MapPath("~/Core/Scripts/Mini/" + jsFile);

                if (!File.Exists(file))
                {
                    log.ErrorFormat("文件不存在:\"{0}\"", jsFile);
                    continue;
                }

                string lines = File.ReadAllText(file);

                fs.Append(lines);
            }

            File.WriteAllText(targetFile, fs.ToString());

            log.InfoFormat("{0}() End", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// 合并 Mini.js
        /// </summary>
        public static void UniteMiniSystemJS()
        {
            UniteMiniSystemJS("~/Core/Scripts/MiniHtml.js");
        }

    }
}
