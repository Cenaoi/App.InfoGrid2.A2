using System;
using System.Collections.Generic;
using System.Web;

namespace EC5.IG2.Plugin
{
    /// <summary>
    /// 插件管理
    /// </summary>
    public static class PluginManager
    {
        static SortedList<string, Type> m_Items = new SortedList<string, Type>();


        public static void Clear()
        {
            m_Items.Clear();
        }

        public static void Add(string plugName, Type plugType)
        {
            m_Items[plugName] = plugType;
        }

        public static Type Get(string plugName)
        {
            Type type = null;

            if (m_Items.TryGetValue(plugName, out type))
            {

            }

            return type;
        }

    }
}