using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.WScript
{
    partial class VBA
    {
        /// <summary>
        /// 对参数值求反。当要确保一个值不等于某一特定值时，可以使用 NOT 函数。 
        /// </summary>
        /// <param name="value">为一个可以计算出 TRUE 或 FALSE 的逻辑值或逻辑表达式。</param>
        /// <returns></returns>
        public static dynamic NOT(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.GetType() == typeof(bool))
            {
                return !(bool)value;
            }

            throw new Exception("未实现此函数");
        }

        public static bool AND(params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                return false;
            }



            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];

                if (value.GetType() == typeof(bool))
                {
                    bool v = (bool)value;

                    if (!v)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool OR(params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];

                if (value.GetType() == typeof(bool))
                {
                    bool v = (bool)value;

                    if (v)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static dynamic IF(bool logical_test, object value_if_true, object value_if_false)
        {
            return logical_test ? value_if_true : value_if_false;
        }

        public static object IFERROR()
        {

            throw new Exception("未实现此函数");
        }
    }
}
