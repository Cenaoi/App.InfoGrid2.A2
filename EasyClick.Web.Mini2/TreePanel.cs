using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EC5.Utility;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 命令事件
    /// </summary>
    public class TreeCommandEventArgs :EventArgs
    {

        /// <summary>
        /// 命令事件
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandParam"></param>
        /// <param name="node">树节点</param>
        /// <param name="record"></param>
        public TreeCommandEventArgs(string commandName, string commandParam, TreeNode node, DataRecord record)
        {
            this.CommandName = commandName;
            this.CommandParam = commandParam;
            this.Record = record;

            this.Node = node;
        }

        /// <summary>
        /// 树节点
        /// </summary>
        public TreeNode Node { get; internal set; }

        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; internal set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string CommandParam { get; internal set; }


        /// <summary>
        /// 当前记录数据
        /// </summary>
        public DataRecord Record { get; internal set; }

    }



    /// <summary>
    /// 可取消的事件源
    /// </summary>
    public class TreeNodeCancelEventArgs : CancelEventArgs
    {
        TreeNode m_Node;

        public TreeNodeCancelEventArgs()
        {
        }

        public TreeNodeCancelEventArgs(TreeNode node)
        {
            m_Node = node;
        }

        public TreeNode Node
        {
            get { return m_Node; }
            set { m_Node = value; }
        }
    }

    /// <summary>
    /// 节点重命名的事件源
    /// </summary>
    public class TreeNodeRenameEventArgs : CancelEventArgs
    {
        TreeNode m_Node;

        string m_Text;

        string m_Old;

        public TreeNode Node
        {
            get { return m_Node; }
            set { m_Node = value; }
        }

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        public string Old
        {
            get { return m_Old; }
            set { m_Old = value; }
        }

    }

    /// <summary>
    /// 节点移动的事件源
    /// </summary>
    public class TreeNodeMoveEventArgs : CancelEventArgs
    {
        public TreeNodeMoveEventArgs()
        {
        }

        public TreeNodeMoveEventArgs(string nodeId, string parentId, int position)
        {
            m_NodeId = nodeId;
            m_ParentId = parentId;
            m_Position = position;
        }

        string m_NodeId;
        string m_ParentId;
        int m_Position;


        /// <summary>
        /// 当前移动的节点
        /// </summary>
        public string NodeId
        {
            get { return m_NodeId; }
        }

        /// <summary>
        /// 转移到某父节点
        /// </summary>
        public string ParentId
        {
            get { return m_ParentId; }
        }

        public int Position
        {
            get { return m_Position; }
        }
    }

    /// <summary>
    /// 移动节点触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TreeNodeMoveEventHander(object sender,TreeNodeMoveEventArgs e);

    /// <summary>
    /// 重命名的事件名
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TreeNodeRenameEventHander(object sender ,TreeNodeRenameEventArgs e);

    /// <summary>
    /// 允许节点取消的事件名称
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TreeNodeCancelEventHander(object sender,TreeNodeCancelEventArgs e);


    /// <summary>
    /// 树型面板
    /// </summary>
    [Description("树型面板")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class TreePanel : Component, IPanel, EasyClick.Web.Mini.IMiniControl
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 树型面板构造函数
        /// </summary>
        public TreePanel()
        {
            m_StatusHid = new Mini.HiddenField();
            m_StatusHid.ID = "Status";
        }

        #region 事件

        /// <summary>
        /// 节点移动
        /// </summary>
        public event TreeNodeMoveEventHander NodeMoved;

        /// <summary>
        /// 触发节点移动事件
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <param name="dropParentId">移动的上级ID</param>
        /// <param name="pos"></param>
        public void OnNodeMoved()
        {
            //string nodeId, string parentId, int position
            

            if (NodeMoved == null)
            {
                return;
            }


            string json = GetStatusJson();

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            JToken jtMove = o["move"];

            if (jtMove == null)
            {
                return;
            }

            string nodeId = jtMove.Value<string>("node_id");
            string parentId = jtMove.Value<string>("parent_id");
            int pos = jtMove.Value<int>("pos");


            TreeNodeMoveEventArgs ea = new TreeNodeMoveEventArgs(nodeId, parentId, pos);

            try
            {
                NodeMoved(this, ea);

                if (ea.Cancel)
                {

                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行 {0}.NodeMoved 事件错误.", this.ID),ex.InnerException);
            }
        }

        /// <summary>
        /// 选择节点的事件
        /// </summary>
        public event EventHandler Selected;

        /// <summary>
        /// 触发选择节点
        /// </summary>
        public void OnSelected()
        {
            if (Selected != null) { Selected(this, EventArgs.Empty); }
        }


        public event EventHandler Creating;

        public void OnCreating()
        {
            if (Creating != null) { Creating(this, EventArgs.Empty); }
        }

        /// <summary>
        /// 加载子节点
        /// </summary>
        public event EventHandler Loading;

        public void OnLoading()
        {
            if (Loading != null) { Loading(this, EventArgs.Empty); }
        }

       



        //public event EventHandler Created;

        //public void OnCreated()
        //{
        //    if (Created != null) { Created(this, EventArgs.Empty); }
        //}



        //public event EventHandler Renamed;

        //public void OnRenamed()
        //{
        //    if (Renamed != null) { Renamed(this, EventArgs.Empty); }
        //}


        //public event EventHandler Removed;

        //public void OnRemoved()
        //{
        //    if (Removed != null) { Removed(this, EventArgs.Empty); }
        //}

        /// <summary>
        /// 删除
        /// </summary>
        public event TreeNodeCancelEventHander Removeing;

        /// <summary>
        /// 触发删除
        /// </summary>
        public void OnRemoveing()
        {

            if (Removeing == null)
            {
                return;
            }

            TreeNodeCancelEventArgs ea = new TreeNodeCancelEventArgs();

            string json = GetStatusJson();

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            JToken jtSelect = o["select"];

            TreeNode node = new TreeNode();

            node.Text = jtSelect.Value<string>("text");
            node.Value = jtSelect.Value<string>("value");
            node.ParentId = jtSelect.Value<string>("par_id");
            node.ChildLoaded = jtSelect.Value<bool>("child_loaded");


            ea.Node = node;

            Removeing(this, ea);

            if (!ea.Cancel)
            {
                this.Delete(node.Value);
            }

        }

        /// <summary>
        /// 重命名
        /// </summary>
        public event TreeNodeRenameEventHander Renaming;

        /// <summary>
        /// 触发重命名事件
        /// </summary>
        public void OnRenaming()
        {
            if (Renaming == null)
            {
                return;
            }

            TreeNodeRenameEventArgs ea = new TreeNodeRenameEventArgs();

            string json = GetStatusJson();

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            JObject o = (JObject)JsonConvert.DeserializeObject(json);

            JToken jtSelect = o["rename"];

            TreeNode node = new TreeNode(this);

            node.Text = jtSelect.Value<string>("text");
            node.Value = jtSelect.Value<string>("value");
            node.ParentId = jtSelect.Value<string>("par_id");
            node.ChildLoaded = jtSelect.Value<bool>("child_loaded");
            node.Tag = jtSelect.Value<string>("tag");

            ea.Node = node;

            ea.Old = jtSelect.Value<string>("old");
            ea.Text = jtSelect.Value<string>("text");


            string oldText = ea.Old;

            Renaming(this, ea);

            if (ea.Cancel)
            {
                SetNodeText(node.Value, oldText);
            }

            
        }

        /// <summary>
        /// 设置节点标签
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="text"></param>
        public void SetNodeText(string nodeId, string text)
        {
            string clientNodeId = this.ID + "_NODE_" + nodeId;

            ScriptManager.Eval("{0}.setNodeText('{1}','{2}',true);", this.ClientID, clientNodeId, text);

        }




        /// <summary>
        /// 命令事件
        /// </summary>
        public event EventHandler<TreeCommandEventArgs> Command;

        /// <summary>
        /// 触发命令事件
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="cmdParam"></param>
        /// <param name="record"></param>
        protected void OnCommand(string cmdName, string cmdParam, TreeNode node, DataRecord record)
        {
            if (Command != null)
            {
                Command(this, new TreeCommandEventArgs(cmdName, cmdParam, node, record));
            }
        }

        /// <summary>
        /// 给与内部调用
        /// </summary>
        public void PreCommand(string cmdName, string cmdParam, string node, string record)
        {
            Newtonsoft.Json.Linq.JToken jt;
            DataRecord dr = null;

            TreeNode nodeTN = null;

            if (!StringUtil.IsBlank(node))
            {
                JObject o = (JObject)JsonConvert.DeserializeObject(node);
                
                try
                {
                    nodeTN = new TreeNode(this);

                    nodeTN.Text = o.Value<string>("text");
                    nodeTN.Value = o.Value<string>("value");
                    nodeTN.ParentId = o.Value<string>("par_id");
                    nodeTN.ChildLoaded = o.Value<bool>("child_loaded");
                    nodeTN.Tag = o.Value<string>("tag");

                }
                catch (Exception ex)
                {
                    log.Debug("解析 node 参数错误: node=" + node);
                    return;
                }
            }

            if (!StringUtil.IsBlank(record))
            {
                try
                {
                    jt = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(record);

                    dr = DataRecord.Parse(jt);
                }
                catch (Exception ex)
                {
                    log.Debug("解析 record 参数错误: record=" + record);
                    return;
                }
            }


            OnCommand(cmdName, cmdParam, nodeTN, dr);
        }

        #endregion


        /// <summary>
        /// 节点渲染脚本
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string NodeRenderer
        {
            get;
            set;
        }


        TreeNode m_RootNode;

        EasyClick.Web.Mini.HiddenField m_StatusHid;


        RegionType m_Region = RegionType.North;
        ScrollBars m_ScrollMode = ScrollBars.Auto;


        /// <summary>
        /// 节点类型的集合,一般用于图标
        /// </summary>
        TreeNodeTypeCollection m_Types;

        /// <summary>
        /// 允许拖放
        /// </summary>
        bool m_AllowDragDrop = false;

        /// <summary>
        /// 显示菜单
        /// </summary>
        bool m_ShowMenu = true;

        bool m_ShowMenuDelete = true;
        bool m_ShowMenuRename = true;
        bool m_ShowMenuNew = true;

        /// <summary>
        /// 显示菜单
        /// </summary>
        [DefaultValue(true)]
        [Description("显示菜单")]
        public bool ShowMenu
        {
            get { return m_ShowMenu; }
            set { m_ShowMenu = value; }
        }


        /// <summary>
        /// 显示删除菜单
        /// </summary>
        [DefaultValue(true)]
        public bool ShowMenuDelete
        {
            get { return m_ShowMenuDelete; }
            set { m_ShowMenuDelete = value; }
        }

        /// <summary>
        /// 显示重命名菜单
        /// </summary>
        [DefaultValue(true)]
        public bool ShowMenuRename
        {
            get { return m_ShowMenuRename; }
            set { m_ShowMenuRename = value; }
        }

        /// <summary>
        /// 显示新建菜单
        /// </summary>
        [DefaultValue(true)]
        public bool ShowMenuNew
        {
            get { return m_ShowMenuNew; }
            set { m_ShowMenuNew = value; }
        }


        /// <summary>
        /// 上下文菜单
        /// </summary>
        ContextMenu m_ContextMenu;

        /// <summary>
        /// 上下文菜单
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("上下文菜单")]
        public ContextMenu ContextMenu
        {
            get
            {
                if(m_ContextMenu == null)
                {
                    m_ContextMenu = new ContextMenu();
                }
                return m_ContextMenu;
            }
            set { m_ContextMenu = value; }
        }

        /// <summary>
        /// 节点类型的集合,一般用于图标
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("节点类型的集合,一般用于图标")]
        public TreeNodeTypeCollection Types
        {
            get
            {
                if (m_Types == null)
                {
                    m_Types = new TreeNodeTypeCollection();
                }

                return m_Types;
            }
        }

        /// <summary>
        /// 允许拖放
        /// </summary>
        [DefaultValue(false)]
        [Description("允许拖放")]
        public bool AllowDragDrop
        {
            get { return m_AllowDragDrop; }
            set { m_AllowDragDrop = value; }
        }


        /// <summary>
        /// 版面区域.配合 Viewport 控件使用
        /// </summary>
        [DefaultValue(RegionType.North)]
        [Description("版面区域,配合 Viewport 控件使用")]
        public RegionType Region
        {
            get { return m_Region; }
            set { m_Region = value; }
        }

        /// <summary>
        /// 滚动条模式
        /// </summary>
        [DefaultValue(ScrollBars.Auto)]
        [Description("滚动条")]
        public ScrollBars Scroll
        {
            get { return m_ScrollMode; }
            set { m_ScrollMode = value; }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        internal TreeNode RootNode
        {
            get
            {
                if (this.m_RootNode == null)
                {
                    this.m_RootNode = new TreeNode(this);
                }
                return this.m_RootNode;
            }
        }

        List<string> m_CheckNodeIds;


        /// <summary>
        /// （支持JS）对节点进行设置复选
        /// </summary>
        /// <param name="nodeId"></param>
        public void CheckNode(string nodeId)
        {
            if (StringUtil.IsBlank(nodeId))
            {
                return;
            }

            if (m_CheckNodeIds == null)
            {
                m_CheckNodeIds = new List<string>();
            }

            m_CheckNodeIds.Add(nodeId);

            string nodeClientId = string.Concat(this.ID, "_NODE_", nodeId);

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.checkNode('{1}');",this.ClientID,nodeClientId);
        }

        /// <summary>
        /// （支持JS）对节点进行设置复选
        /// </summary>
        /// <param name="nodeIds"></param>
        public void CheckNodes(string[] nodeIds)
        {
            if (nodeIds == null || nodeIds.Length == 0)
            {
                return;
            }

            if (m_CheckNodeIds == null)
            {
                m_CheckNodeIds = new List<string>();
            }

            m_CheckNodeIds.AddRange(nodeIds);

            
            string values = GetJsonNodeIDs(nodeIds);

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.checkNode({1});", this.ClientID, values);
        }

        private string GetJsonNodeIDs(string[] nodeIds)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;

            sb.Append("[");

            foreach (string nodeId in nodeIds)
            {
                string nodeClientId = string.Concat(this.ID, "_NODE_", nodeId);

                if (i++ > 0) { sb.Append(", "); }

                sb.Append("'");
                sb.Append(nodeClientId);
                sb.Append("'");
            }

            sb.Append("]");

            return sb.ToString();
        }


        /// <summary>
        /// 获取提交状态的 json 数据
        /// </summary>
        /// <returns></returns>
        public string GetStatusJson()
        {
            return m_StatusHid.Value;
        }


        private void Render_Types(StringBuilder sb)
        {

            if (m_Types == null || m_Types.Count == 0)
            {
                return;
            }

            sb.AppendLine("    types: {");
            {

                TreeNodeType nType;

                for (int i = 0; i < m_Types.Count; i++)
                {
                    nType = m_Types[i];

                    if (i > 0) { sb.AppendLine(","); }

                    sb.Append("      ");
                    sb.Append(nType.Name).Append(":");
                    sb.Append("{ icon: ").AppendFormat("'{0}'", nType.Icon).Append("}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("    },");
            
        }


        private void Render_ContextMenu(StringBuilder sb)
        {
            if(m_ContextMenu == null || m_ContextMenu.Count == 0)
            {
                return;
            }


            sb.AppendLine("    contextMenu: [");
            {

                MenuItem item;

                for (int i = 0; i < m_ContextMenu.Count; i++)
                {
                    item = m_ContextMenu[i];

                    if (i > 0) { sb.AppendLine(","); }

                    sb.Append("      {");

                    sb.Append("text: '" +  JsonUtil.ToJson( item.Text, JsonQuotationMark.SingleQuotes) + "',");

                    sb.Append("command:'" + item.Command + "'");

                    sb.Append("}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("],");
        }



        /// <summary>
        /// 被选中的节点
        /// </summary>
        TreeNode m_NodeSelected = null; 


        /// <summary>
        /// 选中的节点
        /// </summary>
        [Browsable(false)]
        public TreeNode NodeSelected
        {
            get
            {
                if (m_NodeSelected != null)
                {
                    return m_NodeSelected;
                }
                
                string json = GetStatusJson();

                if (string.IsNullOrEmpty(json) || json == "{}")
                {
                    return null;
                }

                try
                {

                    JObject o = (JObject)JsonConvert.DeserializeObject(json);

                    JToken jtSelect = o["select"];

                    if (jtSelect == null) { return null; }

                    TreeNode node = new TreeNode();

                    node.Text = jtSelect.Value<string>("text");
                    node.Value = jtSelect.Value<string>("value");
                    node.ParentId = jtSelect.Value<string>("par_id");

                    node.ChildLoaded = jtSelect.Value<bool>("child_loaded");

                    node.Tag = jtSelect.Value<string>("tag");

                    JArray jp = (JArray)jtSelect["children"];
                    List<string> childs = new List<string>();

                    foreach (JValue item in jp)
                    {
                        childs.Add(item.Value.ToString());
                    }

                    node.m_Children = childs.ToArray();

                    if (node.m_Children.Length > 0)
                    {
                        node.m_ChildLoaded = true;
                    }

                    node.Owner = this;

                    m_NodeSelected = node;
                }
                catch (Exception ex)
                {
                    log.Error("TreePanel.NodeSelected 解析 json 节点错误." + json,ex); 

                    m_NodeSelected = null;
                }

                return m_NodeSelected;
            }
        }


        /// <summary>
        /// 节点集合
        /// </summary>
        [Browsable(false)]
        [MergableProperty(false), DefaultValue((string)null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public TreeNodeCollection Nodes
        {
            get
            {
                return this.RootNode.ChildNodes;
            }
        }


        private TreeStore GetStoreByPage(string storeId)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return null;
            }

            Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            return con as TreeStore;
        }

        /// <summary>
        /// 获取当前控件中的 StoreID 对应的唯一ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private string GetStoreIdByPage(string storeId)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return string.Empty;
            }

            Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            if (con == null)
            {
                return string.Empty;
            }

            return con.ClientID;
        }

        /// <summary>
        /// 获取复选框选中的 ID 号
        /// </summary>
        /// <returns></returns>
        public string[] GetCheckeds()
        {
            string json = GetStatusJson();

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            try
            {
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(json);


                JToken checked_ids = jsonObj["checked_ids"];

                if (checked_ids == null) { throw new Exception("缺乏 checked_ids 节点."); }

                string value = checked_ids.Value<string>();

                string[] ids = EC5.Utility.StringUtil.Split(value);


                return ids;
            
            }
            catch (Exception ex)
            {
                throw new Exception("解析 json 错误.原文:\r\n" + json,ex);
            }
        }

        
        /// <summary>
        /// 获取复选框选中半选中状态的id
        /// </summary>
        /// <returns></returns>
        public string[] GetUndetermineds()
        {
            string json = GetStatusJson();

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(json);


            JToken checked_ids = jsonObj["unde_ids"];

            string value = checked_ids.Value<string>(); 

            string[] ids = EC5.Utility.StringUtil.Split(value);


            return ids;
        }


        private void FullScript(StringBuilder sb)
        {
            TreeStore store = GetStoreByPage(m_StoreID);

            string globelStoreId = GetStoreIdByPage(m_StoreID);

            string rootId = StringUtil.NoBlank(this.RootID, store?.RootValue);
            string parField = StringUtil.NoBlank(this.ParentField, store?.ParentField);
            string idField = StringUtil.NoBlank(this.ValueField, store?.IdField);

            sb.AppendLine("  var tree = Mini2.create('Mini2.ui.tree.Panel', {");

            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", this.ClientID);
            JsParam(sb, "applyTo", "#" + this.ClientID);
            JsParam(sb, "checkbox", this.ShowCheckBox, false);


            JsParam(sb, "store", globelStoreId);

            JsParam(sb, "width", this.Width);
            JsParam(sb, "height", this.Height);

            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "minHeight", this.MinHeight);


            JsParam(sb, "showMenu",this.ShowMenu,true);
            JsParam(sb, "showMenuDelete", this.ShowMenuDelete, true);
            JsParam(sb, "showMenuNew", this.ShowMenuNew, true);
            JsParam(sb, "showMenuRename", this.ShowMenuRename, true);


            JsParam(sb, "allowDragDrop", this.AllowDragDrop, false);


            JsParam(sb, "scroll", this.Scroll, ScrollBars.Auto, TextTransform.Lower);
            JsParam(sb, "dock", this.Dock, TextTransform.Lower);
            JsParam(sb, "region", this.Region, TextTransform.Lower);

            JsParam(sb, "rootId", rootId);
            JsParam(sb, "parField", parField);
            JsParam(sb, "valueField", idField);
            JsParam(sb, "textField", this.TextField);
            JsParam(sb, "checkValueField", this.CheckValueField);

            if (this.Selected != null)
            {
                JsParam(sb, "triggerEvent_selected", true,false);
            }

            if(this.Creating != null)
            {
                JsParam(sb, "triggerEvent_create", true, false);
            }

            if(this.Removeing != null)
            {
                JsParam(sb, "triggerEvent_remove", true, false);
            }

            if(this.Renaming != null)
            {
                JsParam(sb, "triggerEvent_rename", true, false);
            }

            if (!StringUtil.IsBlank(this.NodeRenderer))
            {
                sb.AppendFormat("nodeRenderer: '{0}', ", JsonUtil.ToJson( this.NodeRenderer, JsonQuotationMark.SingleQuotes) );
            }

            Render_Types(sb);

            Render_ContextMenu(sb);


            if (m_AsyncNodeList != null && m_AsyncNodeList.Count > 0)
            {
                sb.Append("  data:[");

                string nodeJson ;
                int i = 0;

                foreach(TreeNode node in m_AsyncNodeList)
                {
                    if (i++ > 0) { sb.Append(","); }
                    
                    nodeJson = node.ToJson();

                    sb.Append(nodeJson);
                }
                
                sb.AppendLine("],");

            }

            if (m_CheckNodeIds != null && m_CheckNodeIds.Count > 0)
            {
                //string checkIds = GetJsonNodeIDs(m_CheckNodeIds.ToArray());

                StringBuilder cSb = new StringBuilder();
                int i = 0;

                cSb.Append("[");

                foreach (string nodeId in m_CheckNodeIds)
                {
                    //string nodeClientId = string.Concat(this.ID, "_NODE_", nodeId);

                    if (i++ > 0) { cSb.Append(", "); }

                    cSb.Append("'");
                    cSb.Append(nodeId);
                    cSb.Append("'");
                }

                cSb.Append("]");

                sb.AppendLine("    checkNodes : " + cSb.ToString() + ",");
            }


            //如果有事件，就给客户端
            if (this.Selected != null)
            {
                sb.AppendLine("event_selected:true,");
            }

            if (this.NodeMoved != null)
            {
                sb.AppendLine("event_move:true,");
            }

            sb.AppendLine("isTree: true");

            sb.AppendLine("  });");

            sb.AppendLine("  tree.render();");

            sb.AppendFormat("  window.{0} = tree;\n", this.ClientID);

            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = tree;\n", this.ID);

        }



        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            base.CreateChildControls();


            m_StatusHid.ID = this.ID + "_Status";

            this.Controls.Add(m_StatusHid);
        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            EnsureChildControls();

            ScriptManager script = ScriptManager.GetManager(this.Page);


            writer.WriteLine("    <div id='{0}' style=''></div>", this.ClientID);

            string jsCode = string.Format("$('#{0}').val({1}.getJson())", m_StatusHid.ClientID, this.ClientID);
            
            m_StatusHid.SetAttribute("SubmitBufore", jsCode);

            m_StatusHid.RenderControl(writer);

            if (script != null)
            {

                StringBuilder jsSb = new StringBuilder();


                BeginReady(jsSb);

                FullScript(jsSb);

                EndReady(jsSb);

                script.AddScript(jsSb.ToString());
            }
            else
            {

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);

                //RenderClientScript(htmlWriter);

                writer.Write(sw.ToString());

                sw.Dispose();
            }

        }

        /// <summary>
        /// 展开节点
        /// </summary>
        /// <param name="id"></param>
        public void OpenNode(string id)
        {
            if(string.IsNullOrEmpty(id)){throw new ArgumentNullException("id","参数不能为空");}

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.open_node('{1}');", this.ClientID, this.ID + "_NODE_" + id);
        }

        /// <summary>
        /// (支持 JS) 重命名
        /// </summary>
        /// <param name="id"></param>
        public void Edit(string id)
        {
            if(string.IsNullOrEmpty(id)){throw new ArgumentNullException("id","参数不能为空");}

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.edit('{1}');", this.ClientID, this.ID + "_NODE_" + id);
        }

        /// <summary>
        /// (支持 JS) 删除节点
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            if(string.IsNullOrEmpty(id)){throw new ArgumentNullException("id","参数不能为空");}

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.delete('{1}');", this.ClientID, this.ID + "_NODE_" + id);
        }

        /// <summary>
        /// (支持 JS) 删除所有节点
        /// </summary>
        public void Clear()
        {
            if (m_AsyncNodeList != null)
            {
                m_AsyncNodeList.Clear();
            }

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.clear();", this.ClientID);
        }

        /// <summary>
        /// 同步树节点
        /// </summary>
        List<TreeNode> m_AsyncNodeList;

        /// <summary>
        /// (支持 JS)添加节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(TreeNode node)
        {
            if (node == null) { throw new ArgumentNullException("node"); }

            if (m_AsyncNodeList == null)
            {
                m_AsyncNodeList = new List<TreeNode>();
            }

            m_AsyncNodeList.Add(node);
        }


        /// <summary>
        /// (支持 JS)刷新树节点.
        /// </summary>
        public void Refresh()
        {
            if (m_AsyncNodeList == null || m_AsyncNodeList.Count == 0)
            {
                return;
            }


            int i = 0;
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (TreeNode  node in m_AsyncNodeList)
            {
                string nodeJson = node.ToJson();

                if (i++ > 0) { sb.AppendLine(","); }

                sb.AppendLine(nodeJson);
            }

            sb.Append("]");

            EasyClick.Web.Mini.MiniHelper.EvalFormat("{0}.add_ForRecord({1});", this.ClientID, sb.ToString());
        }

        

        /// <summary>
        /// (支持 JS)添加节点
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TreeNode Add(string text, string value)
        {


            if (m_AsyncNodeList == null)
            {
                m_AsyncNodeList = new List<TreeNode>();
            }

            TreeNode node = new TreeNode(text, value);

            m_AsyncNodeList.Add(node);

            return node;
        }



        public void LoadPostData()
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request= context.Request;


            this.m_StatusHid.Value = request.Form[this.ClientID + "_Status"];

        }
    }
}
