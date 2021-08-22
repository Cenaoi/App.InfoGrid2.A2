using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using EasyClick.Web.Mini;

namespace EC5.SystemBoard.Web
{

    [Obsolete]
    public static class JsUtility
    {
        static string m_GroupName = "JsUtility";

        public static void Alert(string messgae)
        {
            string js = string.Format("alert(\"{0}\");", JsonUtility.ToJson(messgae));

            MiniScript.AddForGroup(m_GroupName, js);
        }

        public static void Alert(string messageFormat, params string[] args)
        {
            for (int i = 0; i < args.Length; i++)
			{
			    args[i] = JsonUtility.ToJson(args[i]);
			}

            string msg = string.Format(messageFormat, args);

            MiniScript.AddForGroup(m_GroupName, msg);
        }

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

            MiniScript.AddForGroup(m_GroupName, "window.location.href='{0}';", url);

        }

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

            MiniScript.AddForGroup(m_GroupName, "window.location.reload();");

        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResponseEnd()
        {
            string[] js = MiniScript.GetForGroup(m_GroupName);

            HttpContext context = HttpContext.Current;

            HttpResponse response = context.Response;

            response.Clear();

            foreach (string item in js)
            {
                response.Write(item);

                if (!item.EndsWith(";"))
                {
                    response.Write(";");
                }
            }

            context.Items["ResponseJS"] = true;
            //response.End();
        }
    }
}
