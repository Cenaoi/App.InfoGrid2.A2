using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web;
using EasyClick.Web.Mini;
using EC5.SystemBoard;
using EC5.SystemBoard.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.Utility;
using HWQ.Entity.Filter;

namespace EasyClick.BizWeb.UI
{
    /// <summary>
    /// 树节点
    /// </summary>
    /// <typeparam name="ModelT"></typeparam>
    /// <typeparam name="PkValueT"></typeparam>
    public class TreeViewAction<ModelT,PkValueT> where ModelT : class
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        LModelList<ModelT> m_Models;

        TreeView m_Tree;


        DbDecipher m_Decipher = null;

        /// <summary>
        /// 查询过滤条件
        /// </summary>
        List<ConditionElement> m_FilterSearch = new List<ConditionElement>();

        /// <summary>
        /// 查询过滤
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">过滤值</param>
        /// <param name="logic">逻辑</param>
        public void FilterSearch(string fieldName, object value, Logic logic)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, logic));
        }

        /// <summary>
        /// 查询过滤
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">过滤值</param>
        public void FilterSearch(string fieldName, object value)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, Logic.Equality));
        }

        private DbDecipher OpenDecipher()
        {
            if (m_Decipher != null && m_Decipher.State == ConnectionState.Open)
            {
                return m_Decipher;
            }

            string commonDecipher = "APP.COMMON_DECIPHER";

            HttpContext context = HttpContext.Current;

            DbDecipher decipher = null;

            if (context.Items[commonDecipher] == null)
            {

                EcUserState userState = EcContext.Current.User;

                if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                {
                    userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                }

                if (string.IsNullOrEmpty(userState.DbDecipherName))
                {
                    decipher = DbDecipherManager.GetDecipher();
                }
                else
                {
                    decipher = DbDecipherManager.GetDecipher(userState.DbDecipherName);
                }

                decipher.Open();

                context.Items[commonDecipher] = decipher;
            }
            else
            {
                decipher = (DbDecipher)context.Items[commonDecipher];

                if (decipher.State == ConnectionState.Closed)
                {
                    EcUserState userState = EcContext.Current.User;

                    if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                    {
                        userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                    }

                    if (string.IsNullOrEmpty(userState.DbDecipherName))
                    {
                        decipher = DbDecipherManager.GetDecipher();
                    }
                    else
                    {
                        decipher = DbDecipherManager.GetDecipher(userState.DbDecipherName);
                    }

                    decipher.Open();

                    context.Items[commonDecipher] = decipher;
                }
            }

            m_Decipher = decipher;

            return decipher;
        }



        public TreeViewAction(TreeView tree)
        {
            m_Tree = tree;
        }

        public TreeViewAction(TreeView tree, DbDecipher decipher)
        {
            m_Tree = tree;
            m_Decipher = decipher;
        }


        public LModelList<ModelT> Models
        {
            get { return m_Models; }
        }

        /// <summary>
        /// 初始化节点
        /// </summary>
        public void LoadNodes()
        {
            DbDecipher decipher = this.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));

            foreach (var item in m_FilterSearch)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            LModelList<ModelT> models = decipher.SelectModels<ModelT>(filter);

            //展开节点
            ExplandNodes( models);
        }


        /// <summary>
        /// 展开节点
        /// </summary>
        public void ExplandNodes(LModelList<ModelT> models)
        {
            m_Models = models;

            LModelGroup<ModelT,PkValueT> groupList = models.ToGroup<PkValueT>(m_Tree.ParentField);

            //InitTree(m_Tree, groupList, rootId);

            if (!StringUtil.IsNullOrWhiteSpace(m_Tree.DefaultRootID))
            {
                PkValueT rootId = (PkValueT)Convert.ChangeType(m_Tree.DefaultRootID, typeof(PkValueT));
                
                ModelT m = models.FindByPk(rootId, true);

                if (m == null)
                {
                    throw new Exception(string.Format("属性 DefaultRootID = {0}，对应的实体为空。没有找到根节点。",rootId)); 
                }

                TreeNode rootNode = null;
                PkValueT pkValue = default(PkValueT);


                rootNode = CoventNode(m, out pkValue);

                if (rootNode != null)
                {
                    rootNode.Expand();

                    m_Tree.Nodes.Add(rootNode);

                    InitTreeNodes(rootNode, groupList, pkValue);
                }

            }

            if (m_Tree.Nodes.Count > 0)
            {
                TreeNode node = m_Tree.Nodes[0];
                node.Focus(); 
            }
        }

        /// <summary>
        /// 实体转换为 TreeNode 
        /// </summary>
        /// <param name="m">实体</param>
        /// <param name="pkValue">主键值</param>
        /// <returns>TreeNode</returns>
        private TreeNode CoventNode(object m, out PkValueT pkValue)
        {
            if (m == null)
            {
                throw new Exception("无法转换为 TreeNode ,实体对象不能为空.");
            }

            TreeNode node = null;

            string valueField = m_Tree.ValueField;

            if (m is LightModel)
            {
                LightModel model = m as LightModel;
                node = GetNodeForData(model);
                pkValue = (PkValueT)model[valueField];
            }
            else if (m is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor customTD = m as ICustomTypeDescriptor;
                node = GetNodeForData(customTD);

                PropertyDescriptorCollection propList = customTD.GetProperties();
                PropertyDescriptor prop = null;
                prop = propList[valueField];

                pkValue = (PkValueT)prop.GetValue(customTD);

            }
            else
            {
                pkValue = (PkValueT)ObjectUtil.GetPropertyValue(m, valueField); 
            }

            if (m_Tree.LoadDynamic)
            {
                DbDecipher decipher = OpenDecipher();

                LightModelFilter filter = new LightModelFilter(typeof(ModelT));
                filter.And(m_Tree.ParentField, pkValue);

                foreach (var item in m_FilterSearch)
                {
                    filter.And(item.FieldName, item.FieldValue, item.Logic);
                }

                bool exist = decipher.ExistsModels(filter);

                node.StatusID = (exist ? 1 : 0);
            }

            return node;
        }

        /// <summary>
        /// 初始化树节点
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="groupList"></param>
        /// <param name="rootId"></param>
        private void InitTreeNode(TreeView tree, LModelGroup<ModelT, PkValueT> groupList, PkValueT rootId)
        {
            if (!groupList.ContainsKey(rootId))
            {
                return;
            }

            IList<ModelT> models = groupList[rootId];

            TreeNode node = null;

            PkValueT pkValue = default(PkValueT);

            foreach (ModelT m in models)
            {
                node = CoventNode(m, out pkValue);

                if (node == null) { continue; }
                
                tree.Nodes.Add(node);

                InitTreeNodes(node, groupList, pkValue);
            }

        }

        /// <summary>
        /// 初始化树节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="groupList"></param>
        /// <param name="parentId"></param>
        private void InitTreeNodes(TreeNode treeNode, LModelGroup<ModelT, PkValueT> groupList, PkValueT parentId)
        {
            if (!groupList.ContainsKey(parentId))
            {
                return;
            }


            LModelList<ModelT> models = groupList[parentId];

            if (!StringUtil.IsBlank( m_Tree.DefaultSortField))
            {
                models.Sort(m_Tree.DefaultSortField);
            }

            TreeNode node = null;

            PkValueT pkValue = default(PkValueT);

            foreach (ModelT m in models)
            {
                node = CoventNode(m, out pkValue);

                if (node == null) { continue; }

                treeNode.ChildNodes.Add(node);

                InitTreeNodes(node, groupList, pkValue);
            }

        }


        /// <summary>
        /// 获取节点 Text
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetNodeText(LightModel item)
        {
            string text = string.Empty;

            if (!string.IsNullOrEmpty(m_Tree.TextFormatString))
            {
                text = item.ToString(m_Tree.TextFormatString);
            }
            else if (!string.IsNullOrEmpty(m_Tree.TextField))
            {
                text = StringUtil.ToString(item[m_Tree.TextField]);
            }

            return text;
        }

        /// <summary>
        /// 获取节点 Value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetNodeValue(LightModel item)
        {
            string value = string.Empty;

            if (!string.IsNullOrEmpty(m_Tree.ValueFormatString))
            {
                value = item.ToString(m_Tree.ValueFormatString);
            }
            else if (!string.IsNullOrEmpty(m_Tree.ValueField))
            {
                value = StringUtil.ToString(item[m_Tree.ValueField]);
            }

            return value;
        }

        /// <summary>
        /// 获取节点 DataPath
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetNodeDataPath(LightModel item)
        {
            string dataPath = string.Empty;

            if (!string.IsNullOrEmpty(m_Tree.DataPathFormatString))
            {
                dataPath = item.ToString(m_Tree.DataPathFormatString);
            }
            else if (!string.IsNullOrEmpty(m_Tree.DataPathField))
            {
                dataPath = StringUtil.ToString(item[m_Tree.DataPathField]);
            }

            return dataPath;
        }

        /// <summary>
        /// 获取节点的 Type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetNodeType(LightModel item)
        {
            string nodeType = string.Empty;

            if (!string.IsNullOrEmpty(m_Tree.TypeField))
            {
                nodeType = StringUtil.ToString(item[m_Tree.TypeField]);

                if (m_NodeTypeChangeList.ContainsKey(nodeType))
                {
                    nodeType = m_NodeTypeChangeList[nodeType];
                }
            }

            return nodeType;
        }

        private TreeNode GetNodeForData(LightModel item)
        {

            string text = string.Empty;
            string value = string.Empty;
            //string parentId = string.Empty;
            string dataPath = string.Empty;

            string nodeType = string.Empty;
            string iconPath = string.Empty;

            bool isCheck = false;

            text = GetNodeText(item);

            value = GetNodeValue(item);

            dataPath = GetNodeDataPath(item);

            nodeType = GetNodeType(item);

            if (!string.IsNullOrEmpty(m_Tree.IconPathField))
            {
                iconPath = StringUtil.ToString(item[m_Tree.IconPathField]);
            }

            if (!string.IsNullOrEmpty(m_Tree.CheckValueField))
            {
                isCheck = Convert.ToBoolean(item[m_Tree.CheckValueField]);
            }

            TreeNode node = new TreeNode(text, value);
            node.DataPath = dataPath;
            node.NodeType = nodeType;
            node.IconPath = iconPath;
            node.Checked = isCheck;

            return node;
        }

        private TreeNode GetNodeForData(ICustomTypeDescriptor item)
        {
            ICustomTypeDescriptor customTs = item;
            PropertyDescriptorCollection propList = customTs.GetProperties();
            PropertyDescriptor prop = null;

            string text = string.Empty;
            string value = string.Empty;
            //string parentId = string.Empty;
            string dataPath = string.Empty;

            if (!string.IsNullOrEmpty(m_Tree.TextField))
            {
                prop = propList[m_Tree.TextField];
                text = StringUtil.ToString(prop.GetValue(item));
            }

            if (!string.IsNullOrEmpty(m_Tree.ValueField))
            {
                prop = propList[m_Tree.ValueField];
                value = StringUtil.ToString(prop.GetValue(item));
            }

            if (!string.IsNullOrEmpty(m_Tree.DataPathField))
            {
                prop = propList[m_Tree.DataPathField];
                dataPath = StringUtil.ToString(prop.GetValue(item));
            }

            TreeNode node = new TreeNode(text, value);
            node.DataPath = dataPath;

            return node;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        SortedDictionary<string, string> m_NodeTypeChangeList = new SortedDictionary<string, string>(); 

        /// <summary>
        /// 节点类型的转换
        /// </summary>
        /// <param name="nodeTypeA">节点类型 A</param>
        /// <param name="nodeTypeB">节点类型 B</param>
        public void NodeTypeChange(string nodeTypeA, string nodeTypeB)
        {
            if (m_NodeTypeChangeList.ContainsKey(nodeTypeA))
            {
                m_NodeTypeChangeList[nodeTypeA] = nodeTypeB;
            }
            else
            {
                m_NodeTypeChangeList.Add(nodeTypeA, nodeTypeB);
            }
        }
    }
}
