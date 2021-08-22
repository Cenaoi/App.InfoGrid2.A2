using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyClick.Web.Mini.Utility
{
    static class StringUtility
    {


        public static int[] ToIntList(string strValues)
        {
            if (strValues == null) { return new int[0]; }

            string[] sp = strValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int[] ids = new int[sp.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = int.Parse(sp[i]);
            }

            return ids;
        }

        public static Int64[] ToInt64List(string strValues)
        {
            if (strValues == null) { return new Int64[0]; }

            string[] sp = strValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Int64[] ids = new Int64[sp.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = int.Parse(sp[i]);
            }

            return ids;
        }



        public static string ToString(int[] arrayInt)
        {
            StringBuilder sb = new StringBuilder();

            if (arrayInt.Length > 0)
            {
                sb.Append(arrayInt[0]);

                for (int i = 1; i < arrayInt.Length; i++)
                {
                    sb.AppendFormat(",{0}", arrayInt[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 最长字数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chatCount"></param>
        /// <returns></returns>
        public static string Sub(string text, int chatCount)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (chatCount <= 0)
            {
                return text;
            }

            if (text.Length > chatCount)
            {
                return text.Substring(0, chatCount) + "…";
            }

            return text;
        }

        public static DateTime ToDateTime(string vStr, DateTime defaultValue)
        {
            DateTime v = DateTime.MinValue;

            if (DateTime.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static DateTime ToDateTime(string vStr)
        {
            return ToDateTime(vStr, DateTime.Now);
        }

        public static bool ToBool(string vStr, bool defaultValue)
        {
            bool v;

            if (bool.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static bool ToBool(string key)
        {
            return ToBool(key, false);
        }



        public static byte ToByte(string vStr, byte defaultValue)
        {
            byte v;

            if (byte.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static decimal ToByte(string key)
        {
            return ToByte(key, 0);
        }


        public static decimal ToDecimal(string vStr, decimal defaultValue)
        {
            decimal v;

            if (decimal.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static decimal ToDecimal(string key)
        {
            return ToDecimal(key, 0);
        }

        public static int ToInt16(string vStr, int defaultValue)
        {
            Int16 v;

            if (Int16.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static int ToInt16(string key)
        {
            return ToInt16(key, 0);
        }

        public static int ToInt(string vStr, int defaultValue)
        {
            int v;

            if (int.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static int ToInt(string key)
        {
            return ToInt(key, 0);
        }

        public static int ToInt32(string vStr, int defaultValue)
        {
            int v;

            if (int.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static int ToInt32(string key)
        {
            return ToInt(key, 0);
        }

        public static Int64 ToInt64(string vStr, Int64 defaultValue)
        {
            int v;

            if (int.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static Int64 ToInt64(string key)
        {
            return ToInt64(key, 0);
        }

        public static UInt32 ToUInt32(string vStr, UInt32 defaultValue)
        {
            UInt32 v;

            if (UInt32.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static UInt32 ToUInt32(string key)
        {
            return ToUInt32(key, 0);
        }

        public static UInt64 ToUInt64(string vStr, UInt64 defaultValue)
        {
            UInt64 v;

            if (UInt64.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static UInt64 ToUInt64(string key)
        {
            return ToUInt64(key, 0);
        }

        public static UInt16 ToUInt16(string vStr, UInt16 defaultValue)
        {
            UInt16 v;

            if (UInt16.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static UInt16 ToUInt16(string key)
        {
            return ToUInt16(key, 0);
        }


        public static bool IsDecimal(string judgeStr)
        {
            return !string.IsNullOrEmpty(judgeStr) && Regex.Match(judgeStr, "^(-){0,1}\\d+(.\\d+){0,1}$").Success;
        }

        public static float ToFloat(string vStr, float defaultValue)
        {
            float v;

            if (float.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static float ToFloat(string vStr)
        {
            return ToFloat(vStr, 0);
        }

        public static double ToDouble(string vStr, double defaultValue)
        {
            double v;

            if (double.TryParse(vStr, out v)) return v;

            return defaultValue;
        }

        public static double ToDouble(string vStr)
        {
            return ToDouble(vStr, 0);
        }

        /// <summary>
        /// 判断是否可以转换数据类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="validateType"></param>
        /// <returns></returns>
        public static bool TryChangeType(string strValue, Type validateType)
        {
            return TryChangeType(strValue, validateType, true);
        }

        /// <summary>
        /// 判断是否可以转换数据类型
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="validateType"></param>
        /// <returns></returns>
        public static bool TryChangeType(string strValue, Type validateType, bool mandatory)
        {
            if (validateType == typeof(string))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(strValue) && strValue.Length > 0)
            {

                if (validateType == typeof(Int16)) return TryInt16(strValue);
                if (validateType == typeof(Int32)) return TryInt32(strValue);
                if (validateType == typeof(Int64)) return TryInt64(strValue);
                if (validateType == typeof(decimal)) return TryDecimal(strValue);
                if (validateType == typeof(bool)) return TryBool(strValue);

                if (validateType == typeof(DateTime)) return TryDateTime(strValue);
                if (validateType == typeof(byte)) return TryByte(strValue);

                if (validateType == typeof(float)) return TryFloat(strValue);
                if (validateType == typeof(double)) return TryDouble(strValue);

                if (validateType == typeof(UInt16)) return TryUInt16(strValue);
                if (validateType == typeof(UInt32)) return TryUInt32(strValue);
                if (validateType == typeof(UInt64)) return TryUInt64(strValue);

                return true;
            }
            else if (!mandatory && string.IsNullOrEmpty(strValue))
            {
                return true;
            }

            return false;
        }


        static bool TryInt(string strValue)
        {
            int v;
            return int.TryParse(strValue, out v);
        }


        static bool TryInt16(string strValue)
        {
            Int16 v;
            return Int16.TryParse(strValue, out v);
        }


        static bool TryInt32(string strValue)
        {
            Int32 v;
            return Int32.TryParse(strValue, out v);
        }

        static bool TryInt64(string strValue)
        {
            Int64 v;
            return Int64.TryParse(strValue, out v);
        }


        static bool TryUInt16(string strValue)
        {
            UInt16 v;
            return UInt16.TryParse(strValue, out v);
        }


        static bool TryUInt32(string strValue)
        {
            UInt32 v;
            return UInt32.TryParse(strValue, out v);
        }

        static bool TryUInt64(string strValue)
        {
            UInt64 v;
            return UInt64.TryParse(strValue, out v);
        }


        static bool TryDecimal(string strValue)
        {
            decimal v;
            return decimal.TryParse(strValue, out v);
        }

        static bool TryBool(string strValue)
        {
            bool v;
            return bool.TryParse(strValue, out v);
        }

        static bool TryDateTime(string strValue)
        {
            DateTime v;
            return DateTime.TryParse(strValue, out v);
        }

        static bool TryByte(string strValue)
        {
            byte v;
            return byte.TryParse(strValue, out v);
        }

        static bool TryFloat(string strValue)
        {
            float v;
            return float.TryParse(strValue, out v);
        }

        static bool TryDouble(string strValue)
        {
            double v;

            return double.TryParse(strValue, out v);
        }

        public static object ChangeType(string strValue, Type conversionType)
        {
            if (conversionType == typeof(int)) return ToInt(strValue);
            if (conversionType == typeof(Int64)) return ToInt64(strValue);
            if (conversionType == typeof(bool)) return ToBool(strValue);
            if (conversionType == typeof(DateTime)) return ToDateTime(strValue);
            if (conversionType == typeof(decimal)) return ToDecimal(strValue);
            if (conversionType == typeof(byte)) return ToByte(strValue);
            if (conversionType == typeof(float)) return ToFloat(strValue);
            if (conversionType == typeof(double)) return ToDouble(strValue);

            return strValue;
        }

        public static object ChangeType(string strValue, Type conversionType, object defaultValue)
        {
            if (conversionType == typeof(int)) return ToInt(strValue, Convert.ToInt32(defaultValue));
            if (conversionType == typeof(Int64)) return ToInt64(strValue, Convert.ToInt64(defaultValue));
            if (conversionType == typeof(bool)) return ToBool(strValue, Convert.ToBoolean(defaultValue));
            if (conversionType == typeof(DateTime)) return ToDateTime(strValue, Convert.ToDateTime(defaultValue));
            if (conversionType == typeof(decimal)) return ToDecimal(strValue, Convert.ToDecimal(defaultValue));
            if (conversionType == typeof(byte)) return ToByte(strValue, Convert.ToByte(defaultValue));
            if (conversionType == typeof(float)) return ToFloat(strValue, Convert.ToSingle(defaultValue));
            if (conversionType == typeof(double)) return ToDouble(strValue, Convert.ToDouble(defaultValue));

            return strValue;

        }

        /// <summary>
        /// 切割字符串.默认为逗号分隔符
        /// </summary>
        /// <param name="str">准备切割的字符串</param>
        /// <param name="separator">切割符号</param>
        /// <returns>数组</returns>
        public static string[] Split(string str, params string[] separator)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new string[0];
            }

            if (separator == null)
            {
                separator = new string[] { "," };
            }

            string[] sp = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return sp;
        }

        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return obj.ToString();
        }


    }
}
