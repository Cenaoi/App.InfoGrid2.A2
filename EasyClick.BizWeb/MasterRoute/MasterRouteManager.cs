using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.BizWeb.MasterRoute
{
    /// <summary>
    /// 母版配置
    /// </summary>
    public static class MasterRouteManager
    {
        static System.Collections.Concurrent.BlockingCollection<MasterRoute> m_Ttems = new BlockingCollection<MasterRoute>(); 
                   
        /// <summary>
        /// 获取映射路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetMap(string url)
        {
            foreach (var item in m_Ttems)
            {
                if (url.StartsWith(item.Url, StringComparison.OrdinalIgnoreCase))
                {
                    return item.MapTo;
                }
            }

            return null;
        }

        /// <summary>
        /// 添加映射
        /// </summary>
        /// <param name="route"></param>
        public static void AddMap(MasterRoute route)
        {
            m_Ttems.Add(route);
        }

        /// <summary>
        /// 添加映射
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mapTo"></param>
        public static void AddMap(string url, string mapTo)
        {
            m_Ttems.Add(new MasterRoute()
            {
                Url = url,
                MapTo = mapTo
            });
        }


    }

    public class MasterRoute
    {
        /// <summary>
        /// 访问的地址前缀; 例如 : /App/Abc/
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 映射母版到
        /// </summary>
        public string MapTo { get; set; }
    }
    
}
