using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 复选框状态
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// 选中状态
        /// </summary>
        Checked,
        /// <summary>
        /// 不明确
        /// </summary>
        Indeterminate,
        /// <summary>
        /// 未选中
        /// </summary>
        Unchecked
    }

    /// <summary>
    /// 树节点
    /// </summary>
    public  class TreeNode : ICloneable
    {
        TreeNodeCollection m_ChildNodes;

        TreeView m_Owner;

        string m_OwnerNodeID;

        TreeNode m_Parent;

        /// <summary>
        /// 获取一个值，用以指示树节点是否处于可展开状态。
        /// </summary>
        bool m_IsExpanded = false;

        int m_Depth = 0;

        string m_DataPath;

        string m_Text;
        string m_Value;

        int m_Identity = 0;

        int m_StatusID = 0;

        bool m_Checked = false;

        CheckState m_CheckState = CheckState.Unchecked;


        string m_NodeType;

        string m_IconPath;

        /// <summary>
        /// 图标路径
        /// </summary>
        [DefaultValue("")]
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        [DefaultValue("")]
        public string NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }


        /// <summary>
        /// 复选框被选中
        /// </summary>
        [Description("复选框被选中")]
        [DefaultValue(false)]
        public bool Checked
        {
            get { return m_Checked; }
            set
            {
                m_Checked = value;

                if (value)
                {
                    m_CheckState = Mini.CheckState.Checked;
                }
                else
                {
                    m_CheckState = Mini.CheckState.Unchecked;
                }
            }
        }

        public CheckState CheckState
        {
            get { return m_CheckState; }
            set
            {
                m_CheckState = value;

                switch (value)
                {
                    case CheckState.Checked:
                        m_Checked = true;
                        break;
                    case CheckState.Unchecked:
                        m_Checked = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 节点所属的树视图 ID
        /// </summary>
        internal string OwnerNodeID
        {
            get { return m_OwnerNodeID; }
            set { m_OwnerNodeID = value; }
        }

        /// <summary>
        /// 节点状态：0=叶，1=根部
        /// </summary>
        public int StatusID
        {
            get { return m_StatusID; }
            set { m_StatusID = value; }
        }

        #region 构造方法

        public TreeNode()
        {
        }

        public TreeNode(string text)
        {
            m_Text = text;
            m_Value = text;
        }

        public TreeNode(string text, string value)
        {
            m_Text = text;
            m_Value = value;
        }

        public TreeNode(string text, int value)
        {
            m_Text = text;
            m_Value = value.ToString();
        }

        public TreeNode(string text, object value)
        {
            m_Text = text;
            m_Value = value.ToString();
        }


        public TreeNode(TreeView owner)
        {
            m_Owner = owner;
        }

        #endregion

        public int Identity
        {
            get { return m_Identity; }
            set { m_Identity = value; }
        }

        /// <summary>
        /// 节点的路径
        /// </summary>
        public string DataPath
        {
            get { return m_DataPath; }
            set { m_DataPath = value; }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }

        /// <summary>
        /// 获取一个值，用以指示树节点是否处于可展开状态。
        /// </summary>
        public bool IsExpanded
        {
            get { return m_IsExpanded; }
        }

        /// <summary>
        /// 展开树节点。
        /// </summary>
        public void Expand()
        {
            m_IsExpanded = true;
        }

        /// <summary>
        /// 折叠树节点。
        /// </summary>
        public void Collapse()
        {
            m_IsExpanded = false;
        }

        /// <summary>
        /// 父节点对象
        /// </summary>
        public TreeNode Parent
        {
            get { return m_Parent; }
            internal set { m_Parent = value; }
        }

        /// <summary>
        /// 节点所属的树视图
        /// </summary>
        public TreeView Owner
        {
            get { return m_Owner; }
            internal set { m_Owner = value; }
        }

        /// <summary>
        /// 判断是否存在子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChildNode()
        {
            return (m_ChildNodes != null && m_ChildNodes.Count > 0);
        }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public TreeNodeCollection ChildNodes
        {
            get
            {
                if (m_ChildNodes == null)
                {
                    m_ChildNodes = new TreeNodeCollection(this);
                }

                return m_ChildNodes;
            }
        }

        /// <summary>
        /// 节点文本
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// 节点值
        /// </summary>
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        public void Focus()
        {
            m_Owner.FocusText = m_Text;
            m_Owner.FocusValue = m_Value;
            m_Owner.FoucsDataPath = m_DataPath;
            m_Owner.FoucsNodeID = m_Owner.ID + "_" + m_Identity;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Render(System.Web.UI.HtmlTextWriter writer)
        {


        }
    }
}
