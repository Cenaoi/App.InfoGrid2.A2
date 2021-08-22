using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class AttrUtility
    {

        public static T GetAttr<T>(PropertyInfo owner) where T : Attribute
        {
            object[] objs = owner.GetCustomAttributes(typeof(T), true);

            if (objs == null || objs.Length == 0)
            {
                return default(T);
            }

            T attrT = objs[0] as T;

            return attrT;
        }

        public static T GetAttr<T>(Type owner) where T : Attribute
        {
            object[] objs = owner.GetCustomAttributes(typeof(T), true);

            if (objs == null || objs.Length == 0)
            {
                return default(T);
            }

            T attrT = objs[0] as T;

            return attrT;
        }

        public static T GetAttr<T>(object owner) where T : Attribute
        {
            object[] objs = owner.GetType().GetCustomAttributes(typeof(T), true);

            if (objs == null || objs.Length == 0)
            {
                return default(T);
            }

            T attrT = objs[0] as T;

            return attrT;
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static object GetDefaultValue(PropertyInfo owner)
        {
            DefaultValueAttribute attr = GetAttr<DefaultValueAttribute>(owner);

            if (attr == null) return null;

            return attr.Value;
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="emptyValue">找不到才用的默认值</param>
        /// <returns></returns>
        public static object GetDefaultValue(PropertyInfo owner,object emptyValue)
        {
            DefaultValueAttribute attr = GetAttr<DefaultValueAttribute>(owner);

            if (attr == null) return emptyValue;

            return attr.Value;
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static string GetDescription(PropertyInfo owner)
        {
            DescriptionAttribute attr = GetAttr<DescriptionAttribute>(owner);

            if (attr == null) return string.Empty;

            return attr.Description;
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static string GetDescription(Type owner)
        {
            DescriptionAttribute attr = GetAttr<DescriptionAttribute>(owner);

            if (attr == null) return string.Empty;

            return attr.Description;
        }

        /// <summary>
        /// 指定一个属性 (Property) 或事件是否应显示在“属性”窗口中。
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool GetBrowsable(PropertyInfo owner)
        {
            BrowsableAttribute attr = GetAttr<BrowsableAttribute>(owner);

            if (attr == null) return true;

            return attr.Browsable;
        }



    }
}
