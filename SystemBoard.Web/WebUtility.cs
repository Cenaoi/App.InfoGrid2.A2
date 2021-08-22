using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Net;
using System.Web;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// Request 辅助功能
    /// </summary>
    [Obsolete]
    public static class WebUtility
    {

        public static T Query<T>(string key, T defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string v = request.QueryString[key];

            if(v == null)
            {
                return defaultValue;
            }

            T newValue = default(T);

            try
            {
                newValue = (T)Convert.ChangeType(v, typeof(T));
            }
            catch
            {
                return defaultValue;
            }

            return newValue;
        }

        public static T Query<T>(string key)
        {
            return Query<T>(key, default(T));
        }

        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int QueryInt(string key, int defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            int v = 0;

            if (!int.TryParse(request.QueryString[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int QueryInt(string key)
        {
            return QueryInt(key, 0);
        }

        public static long QueryLong(string key, long defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            long v = 0;

            if (!long.TryParse(request.QueryString[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static long QueryLong(string key)
        {
            return QueryLong(key, 0);
        }


        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool QueryBool(string key, bool defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            bool v = false;

            if (!bool.TryParse(request.QueryString[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool QueryBool(string key)
        {
            return QueryBool(key, false);
        }

        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Query(string key, string defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;
            
            if (request.QueryString[key] == null)
            {
                return defaultValue;
            }

            return request.QueryString[key];
        }

        public static string Query(string key)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string v = request.QueryString[key];

            if (v == null)
            {
                return string.Empty;
            }

            return v;
        }

        /// <summary>
        /// 获取窗体变量集合。增强 Request.Form
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int FormInt(string key, int defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            int v = 0;

            if (!int.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static DateTime FormDateTime(string key)
        {
            return FormDateTime(key, DateTime.MinValue);
        }

        public static DateTime FormDateTime(string key, DateTime defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            DateTime v = DateTime.MinValue;

            if (!DateTime.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static string Form(string key)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string value = request.Form[key];

            return value;
        }

        public static string Form(string key, string defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string value = request.Form[key];

            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(defaultValue))
            {
                return defaultValue;
            }

            return value;
        }

        public static bool FormBool(string key,bool defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            bool v = false;

            if (!bool.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static bool FormBool(string key)
        {
            return FormBool(key, false);
        }

        /// <summary>
        /// 获取窗体变量集合。增强 Request.Form
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int FormInt(string key)
        {
            return FormInt(key, 0);
        }

        /// <summary>
        ///  获取窗体变量集合。增强 Request.Form
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int[] FormIntList(string key)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;

            string[] txtList = request.Form.GetValues(key);

            if (txtList == null)
            {
                return new int[0];
            }

            List<int> ids = new List<int>(txtList.Length);

            for (int i = 0; i < txtList.Length; i++)
            {
                int n = 0;

                if (int.TryParse(txtList[i], out n))
                {
                    ids.Add(n);
                }
            }

            return ids.ToArray();
        }

        

        public static string[] FormStrList(string key)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;

            string[] txtList = request.Form.GetValues(key);

            return txtList;
        }

        public static Int64[] FormInt64List(string key)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;

            string[] txtList = request.Form.GetValues(key);

            if (txtList == null)
            {
                return new Int64[0];
            }

            List<Int64> ids = new List<Int64>(txtList.Length);

            for (int i = 0; i < txtList.Length; i++)
            {
                int n = 0;

                if (int.TryParse(txtList[i], out n))
                {
                    ids.Add(n);
                }
            }

            return ids.ToArray();
        }


    }

}
