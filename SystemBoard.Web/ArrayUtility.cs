using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 数组帮助类
    /// </summary>
    [Obsolete]
    public static class ArrayUtility
    {

        #region ToString(object[] values)

       

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(byte[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(Int16[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(Int32[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }           

            return sb.ToString();
        }
        
        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(Int64[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(decimal[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(double[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append(values[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例："一","二","三",……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(string[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");
                sb.Append("\"");
                sb.Append(values[i]);
                sb.Append("\"");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,"一","二",3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <param name="emptyArrayValue">数组为空，返回的默认值</param>
        /// <returns></returns>
        public static string ToString(object[] values, string emptyArrayValue)
        {
            if (values == null || values.Length == 0)
            {
                return emptyArrayValue;
            }

            StringBuilder sb = new StringBuilder();

            if (values[0] is string)
            {
                sb.Append("\"");
                sb.Append(values[0]);
                sb.Append("\"");
            }
            else
            {
                sb.Append(values[0]);
            }

            for (int i = 1; i < values.Length; i++)
            {
                sb.Append(",");

                if (values[i] is string)
                {
                    sb.Append("\"");
                    sb.Append(values[i]);
                    sb.Append("\"");
                }
                else
                {
                    sb.Append(values[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(byte[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(Int16[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(Int32[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(Int64[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(decimal[] values)
        {
            return ToString(values, string.Empty);
        }

        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(double[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(object[] values)
        {
            return ToString(values, string.Empty);
        }


        /// <summary>
        /// 输出字符串格式。例：1,2,3,4,……
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToString(string[] values)
        {
            return ToString(values, string.Empty);
        }

        #endregion
    }
}
