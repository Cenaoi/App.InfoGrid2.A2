using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.Action3.Xml
{
    /// <summary>
    /// 路由
    /// </summary>
    public class XRoute
    {
        /// <summary>
        /// 路由
        /// </summary>
        public XRoute()
        {

        }

        /// <summary>
        /// 路由构造函数
        /// </summary>
        /// <param name="name">路由名称</param>
        /// <param name="linkTo">连接到</param>
        public XRoute(string name, string linkTo)
        {
            this.Name = name;
            this.LinkTo = linkTo;
        }


        /// <summary>
        /// 路由名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }


        /// <summary>
        /// 连接到
        /// </summary>
        [XmlAttribute("link-to")]
        public string LinkTo { get; set; }

    }
}
