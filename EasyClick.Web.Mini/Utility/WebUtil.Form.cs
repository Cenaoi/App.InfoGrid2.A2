using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini.Utility
{
	static partial class WebUtil
	{

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

        public static Int64 FormInt64(string key, Int64 defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            Int64 v = 0;

            if (!Int64.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static Int16 FormInt16(string key, Int16 defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            Int16 v = 0;

            if (!Int16.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static double FormDouble(string key, double defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            double v = 0;

            if (!double.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static double FormDouble(string key)
        {
            return FormDouble(key, 0);
        }


        public static decimal FormDecimal(string key, decimal defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            decimal v = 0;

            if (!decimal.TryParse(request.Form[key], out v))
            {
                v = defaultValue;
            }

            return v;
        }

        public static decimal FormDecimal(string key)
        {
            return FormDecimal(key, 0);
        }

        public static T Form<T>(string key)
        {
            return Form<T>(key, default(T));
        }

        public static T Form<T>(string key, T defaultValue)
        {
            HttpContext context = HttpContext.Current;

            HttpRequest request = context.Request;

            string valueStr = request.Form[key];

            if (valueStr == null) { return defaultValue; }

            T value;

            try
            {
                value = (T)Convert.ChangeType(valueStr, typeof(T));
            }
            catch
            {
                value = defaultValue;
            }

            return value;
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

        public static bool FormBool(string key, bool defaultValue)
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
