using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Diagnostics;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 节点类型的集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TreeNodeTypeCollection : List<TreeNodeType>
    {

    }

    /// <summary>
    /// 节点类型
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("Name={Name},Icon={Icon}")]
    public class TreeNodeType
    {
        string m_Name;
        string m_Icon;

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 图标路径
        /// </summary>
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }
    }
}
