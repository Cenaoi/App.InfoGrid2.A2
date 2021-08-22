using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 节点项目
    /// </summary>
    public class ActionItemBase : CodeIndexItem
    {
        public ActionItemBase()
        {

        }

        public ActionItemBase(string code) : base(code)
        {

        }

        /// <summary>
        /// 路由集合
        /// </summary>
        RouteList m_Routes = new RouteList();

        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabled { get; set; } = true;


        /// <summary>
        /// 是否有路由
        /// </summary>
        /// <returns></returns>
        public bool HasRoute()
        {
            return (m_Routes != null && m_Routes.Count > 0);
        }


        /// <summary>
        /// 链接到
        /// </summary>
        /// <param name="routeName">路由名</param>
        /// <param name="target">目标对象</param>
        public void LinkTo(string routeName, string target)
        {
            m_Routes.Add(routeName, target);
        }

        /// <summary>
        /// 路由集合
        /// </summary>
        public RouteList Routes
        {
            get { return m_Routes; }
        }

    }


}
