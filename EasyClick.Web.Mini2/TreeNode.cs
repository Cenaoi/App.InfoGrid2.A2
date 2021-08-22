using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
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

        TreePanel m_Owner;

        string m_OwnerNodeID;

        string m_ParetnId;
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
        /// 附加数据
        /// </summary>
        string m_Tag; 

        internal bool m_ChildLoaded = false;

        internal string[] m_Children;

        public string[] Children
        {
            get { return m_Children; }
        }

        /// <summary>
        /// 加载子节点
        /// </summary>
        public bool ChildLoaded
        {
            get { return m_ChildLoaded; }
            set
            {
                m_ChildLoaded = value;

                if (m_Owner != null)
                {
                    EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.setNodeLoaded('{1}',{2});",
                        m_Owner.ClientID,
                        m_Owner.ID + "_NODE_" + this.Value,
                        value.ToString().ToLower());
                }

            }
        }



        /// <summary>
        /// 父主键值
        /// </summary>
        [DefaultValue("")]
        public string ParentId
        {
            get { return m_ParetnId; }
            set { m_ParetnId = value; }
        }


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
                    m_CheckState = CheckState.Checked;
                }
                else
                {
                    m_CheckState = CheckState.Unchecked;
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


        public TreeNode(TreePanel owner)
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
        /// 附加数据
        /// </summary>
        public string Tag
        {
            get { return m_Tag; }
            set
            {
                m_Tag = value;

                if (m_Owner != null)
                {
                    EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.setTag('{1}','{2}');",
                        m_Owner.ClientID,
                        m_Owner.ID + "_NODE_" + this.Value,
                        EC5.Utility.JsonUtil.ToJson(value, EC5.Utility.JsonQuotationMark.SingleQuotes));
                }

            }
        }

        /// <summary>
        /// 删除子节点集合
        /// </summary>
        public void RemoveChilds()
        {
            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.removeChilds('{1}');",
                m_Owner.ClientID,
                m_Owner.ID + "_NODE_" + this.Value);
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
        public TreePanel Owner
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
            //m_Owner.FocusText = m_Text;
            //m_Owner.FocusValue = m_Value;
            //m_Owner.FoucsDataPath = m_DataPath;
            //m_Owner.FoucsNodeID = m_Owner.ID + "_" + m_Identity;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Render(System.Web.UI.HtmlTextWriter writer)
        {


        }

        /// <summary>
        /// 输出 json 数据
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);
            st.RetractBengin("{");

            st.WriteParam("value", this.Value);
            st.WriteParam("text", this.Text);
            st.WriteParam("par_id", this.ParentId);
            st.WriteParam("type", this.NodeType, "default");
            st.WriteParam("child_loaded", m_ChildLoaded);
            st.WriteParam("expand", this.IsExpanded);
            st.WriteParam("checked", this.Checked);
            st.WriteParam("icon", this.IconPath);
            st.WriteParam("tag", this.Tag);

            st.RetractEnd("}");
            
            return st.ToString();
        }

        private void FuillScript_TreeNode(StringBuilder sb, TreeNode parent)
        {
            if (!parent.HasChildNode())
            {
                return;
            }

            sb.Append(",").AppendLine("children':[");

            string nodeJson;
            int i = 0;

            foreach (TreeNode node in parent.ChildNodes)
            {
                if (i++ > 0) { sb.AppendLine(","); }
                nodeJson = node.ToJson();
                sb.Append(nodeJson);
            }

            sb.Append("]");
        }
    }
}
