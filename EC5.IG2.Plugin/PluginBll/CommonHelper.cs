using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EC5.IG2.Plugin.PluginBll
{
    public class CommonHelper
    {
        public static string CuttingSz(string str)
        {
            //取出字符串中所有的数字
            string res = Regex.Replace(str, "[a-z]", "", RegexOptions.IgnoreCase);

            return res;
        }


        public static string CuttingZm(string str)
        {
            //取出字符串中所有的英文字母      
            string res = Regex.Replace(str, "[0-9]", "", RegexOptions.IgnoreCase);

            return res;
        }


        /// <summary>
        /// 解析字符串里面{字段名}
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回字段名集合</returns>
        public static List<string> GetFieldNameByString(string str)
        {
            List<string> list = new List<string>();

            if (string.IsNullOrWhiteSpace(str))
            {
                return list;
            }

            while (true)
            {
                int sindex = str.IndexOf("{");

                if (sindex == -1)
                {
                    break;
                }

                int eindex = str.IndexOf("}");

                if (eindex == -1)
                {
                    break;
                }

                sindex++;

                string fitem = str.Substring(sindex, eindex - sindex);

                list.Add(fitem);

                eindex++;

                str = str.Substring(eindex);
            }

            return list;
        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static List<string> SplitString(string str, string splitStr)
        {
            List<string> res = new List<string>();

            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(splitStr))
            {
                return res;
            }

            char[] charArr = splitStr.ToArray();

            string[] arr = str.Split(charArr);

            foreach (var item in arr)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                res.Add(item);
            }

            return res;
        }


        /// <summary>
        /// 尝试获取整型类型的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TryGetInt(string str)
        {
            int v = 0;

            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            if (!int.TryParse(str, out v))
            {
                return 0;
            }

            return v;
        }


        /// <summary>
        /// 尝试获取浮点型类型的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float TryGetFloat(string str)
        {
            float v = (float)0;

            if (string.IsNullOrWhiteSpace(str))
            {
                return (float)0;
            }

            if (!float.TryParse(str, out v))
            {
                return (float)0;
            }

            return v;
        }


        /// <summary>
        /// 尝试获取日期时间类型的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime TryGetDateTime(string str)
        {
            DateTime v = DateTime.Now;

            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.Now;
            }

            if (!DateTime.TryParse(str, out v))
            {
                return DateTime.Now;
            }

            return v;
        }


        /// <summary>
        /// 尝试获取布尔类型的值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TryGetBool(string str)
        {
            bool v = false;

            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            str = str.ToLower();

            if (str == "true" || str == "1")
            {
                return true;
            }

            return v;
        }


    }
}
