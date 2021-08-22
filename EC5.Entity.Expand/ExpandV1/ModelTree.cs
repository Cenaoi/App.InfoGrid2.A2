using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Entity.Expanding.ExpandV1
{
    /// <summary>
    /// 实体树
    /// </summary>
    public class ModelTree:ModelTreeNode
    {

    }


    public class ModelTreeNode
    {
        /// <summary>
        /// 节点名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附加实体数据
        /// </summary>
        public object Data { get; set; }

        ModelTreeNodeCollection m_Childs;

        /// <summary>
        /// 下级数节点元素
        /// </summary>
        public ModelTreeNodeCollection Childs
        {
            get
            {

                if (m_Childs == null)
                {
                    m_Childs = new ModelTreeNodeCollection(this);
                }

                return m_Childs;
            }
        }
    }

    public class ModelTreeNodeCollection :List<ModelTreeNode>
    {
        ModelTreeNode m_Owner;
        
        public ModelTreeNodeCollection(ModelTreeNode owner)
        {
            m_Owner = owner;
        }  

        public ModelTreeNode Owner
        {
            get { return m_Owner; }
            internal set { m_Owner = value; }
        }


    }
}
