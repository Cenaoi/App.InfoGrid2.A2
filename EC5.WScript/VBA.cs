using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.WScript
{
    public partial class VBA
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static long ABS(params object[] values)
        {
            return 0;
        }



        /// <summary>
        /// 获取数组的第一个对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic FIRST(object value)
        {
            object item = null;

            if (value is Array)
            {
                Array items = (Array)value;

                if (items.Length > 0)
                {
                    item = items.GetValue(0);
                }
            }
            else
            {
                item = value;
            }

            return item;
        }

        /// <summary>
        /// 获取最后一条记录
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic LAST(object value)
        {
            object item = null;

            if (value is Array)
            {
                Array items = (Array)value;

                if (items.Length > 0)
                {
                    item = items.GetValue(items.Length - 1);
                }
            }
            else
            {
                item = value;
            }

            return item;
        }
    }
}
