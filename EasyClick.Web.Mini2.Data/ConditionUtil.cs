using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 条件如果
    /// </summary>
    public static class IfUtil
    {
        /// <summary>
        /// 如果第一个参数不为空，就返回第一个参数，否则返回第二个参数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="falseText"></param>
        /// <returns></returns>
        public static string NotBlank(string field, string falseText)
        {
            if (!string.IsNullOrEmpty(field))
            {
                return field;
            }

            return falseText;
        }

        /// <summary>
        /// 如果第一个参数不为空，就返回第一个参数，否则返回第二个参数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="falseText"></param>
        /// <returns></returns>
        public static string NotBlank(object field, string falseText)
        {
            if (field == null)
            {
                return falseText;
            }

            if (!string.IsNullOrEmpty(field.ToString()))
            {
                return ParseNum(field).ToString();
            }

            return falseText;
        }

        /// <summary>
        /// 去除 Decimal 对象后面的0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ParseNum(object value)
        {
            string v = value.ToString();
            int n = v.LastIndexOf('.');

            //如果没有小数，就不处理
            if (n == -1) return value;

            v = v.TrimEnd('0');
            n = v.Length - 1;

            if (v[n] == '.') v = v.Remove(n);

            return v;
        }
        


        /// <summary>
        /// 如果第3个参数 Value 不等于空，就格式化合并
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="foramt"></param>
        /// <param name="value"></param>
        public static bool NotBlank_AppendFormat(StringBuilder sb, string foramt,string value)
        {
            if (sb == null) { throw new ArgumentNullException("sb"); }

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            sb.AppendFormat(foramt, value);

            return true;
        }

    }
}
