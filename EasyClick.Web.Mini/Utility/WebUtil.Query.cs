using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini.Utility
{
    static partial class WebUtil
    {

        public static T Query<T>(string key, T defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string v = request.QueryString[key];

            if (v == null)
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


        /// <summary>
        /// 获取 HTTP 查询字符串变量集合。增强 Request.QueryString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
        public static DateTime QueryDateTime(string key, DateTime defaultValue)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;

            DateTime v;

            if (!DateTime.TryParse(request.QueryString[key], out v))
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
        public static DateTime QueryDateTime(string key)
        {
            return QueryDateTime(key, DateTime.MinValue);
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


            if (string.IsNullOrEmpty( request.QueryString[key]))
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
    }
}
