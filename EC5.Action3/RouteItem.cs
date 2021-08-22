using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 路由项目
    /// </summary>
    public class RouteItem
    {
        /// <summary>
        /// （构造函数）路由项目
        /// </summary>
        public RouteItem()
        {

        }

        /// <summary>
        /// 激活
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 起源
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 目标节点
        /// </summary>
        public ActionItemBase TargetNode { get; set; }
    }

}
