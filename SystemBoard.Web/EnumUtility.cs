using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    [Obsolete]
    public static class EnumUtility
    {

        public static EnumT Parse<EnumT>(string value, bool ignoreCase)
        {
            EnumT obj = (EnumT)Enum.Parse(typeof(Enum), value,ignoreCase);

            return obj;
        }

        public static EnumT Parse<EnumT>(string value)
        {
            EnumT obj = (EnumT)Enum.Parse(typeof(Enum), value);

            return obj;
        }
    }
}
