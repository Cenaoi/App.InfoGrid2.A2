using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    /// <summary>
    /// 路由集合
    /// </summary>
    public class RouteList : IEnumerable<RouteItem>
    {
        /// <summary>
        /// (构造函数)路由集合
        /// </summary>
        public RouteList()
        {

        }

        /// <summary>
        /// (构造函数)路由集合
        /// </summary>
        /// <param name="origin"></param>
        public RouteList(string origin)
        {
            this.Origin = origin;
        }

        public RouteItem this[int index]
        {
            get { return m_Items[index]; }
        }


        List<RouteItem> m_Items = new List<RouteItem>();

        /// <summary>
        /// 路由名称
        /// </summary>
        ConcurrentDictionary<string, RouteItem> m_NamesIndex = new ConcurrentDictionary<string, RouteItem>();

        /// <summary>
        /// 跳转的目标节点
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// 添加路由项目
        /// </summary>
        /// <param name="routeName">路由名称</param>
        /// <param name="target">跳转的目标节点</param>
        public void Add(string routeName, string target)
        {
            if (m_NamesIndex.ContainsKey(routeName))
            {
                throw new Exception($"路由名重复 \"{routeName}\".");
            }


            RouteItem item = new RouteItem();
            item.Name = routeName;
            item.Target = target;

            item.Origin = this.Origin;

            m_Items.Add(item);

            m_NamesIndex.TryAdd(routeName, item);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// 删除路由项目
        /// </summary>
        /// <param name="routeName">路由的项目名称</param>
        public void Remove(string routeName)
        {
            RouteItem item = null;

            if (m_NamesIndex.TryGetValue(routeName, out item))
            {
                m_Items.Remove(item);
            }
        }

        /// <summary>
        /// 尝试获取路由
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public RouteItem Get(string routeName)
        {
            RouteItem item = null;

            if(m_NamesIndex.TryGetValue(routeName,out item))
            {

            }

            return item;
        }

        public IEnumerator<RouteItem> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>
        /// 路由数量
        /// </summary>
        public int Count
        {
            get { return m_Items.Count; }
        }



    }

}
