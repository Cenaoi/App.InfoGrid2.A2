using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini.Utility;
using System.IO;

namespace EasyClick.Web.Mini
{
    
    /// <summary>
    /// 树节点视图
    /// </summary>
    [Description("树节点视图")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class TreeView : Control, IAttributeAccessor, IMiniControl
    {

        /// <summary>
        /// 动态加载节点
        /// </summary>
        bool m_LoadDynamic = false;

        /// <summary>
        /// 动态加载。默认值:false
        /// </summary>
        [DefaultValue(false)]
        public bool LoadDynamic
        {
            get { return m_LoadDynamic; }
            set { m_LoadDynamic = value; }
        }


        #region 事件


        #endregion

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        bool m_EcDesignMode = false;

        /// <summary>
        /// 过滤字段
        /// </summary>
        bool m_FilterField = true;

        /// <summary>
        /// 根节点
        /// </summary>
        TreeNode m_RootNode;

        /// <summary>
        /// 选择节点触发的客户端 js
        /// </summary>
        string m_OnSelectNodeClientScript;

        /// <summary>
        /// 打开节点触发的客户端 js
        /// </summary>
        string m_OnOpenNodeClientScript;

        string m_OnRenameNodeClientScript;

        /// <summary>
        /// 焦点参数
        /// </summary>
        HiddenField m_FocusValue;
        HiddenField m_FocusText;
        HiddenField m_FocusDataPath;
        HiddenField m_FocusNodeID;


        bool m_InitCreateChild = false;

        /// <summary>
        /// 默认排序字段
        /// </summary>
        string m_DefaultSortField;

        /// <summary>
        /// 节点类型的集合,一般用于图标
        /// </summary>
        TreeNodeTypeCollection m_Types;



        /// <summary>
        /// 节点类型的集合,一般用于图标
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
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
        /// 默认排序字段
        /// </summary>
        [DefaultValue("")]
        public string DefaultSortField
        {
            get { return m_DefaultSortField; }
            set { m_DefaultSortField = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateChildControls();
        }

        public TreeView()
        {
            //m_NodeIdentity = new HiddenField();
            //m_NodeIdentity.Value = "0";

            m_FocusNodeID = new HiddenField();
            m_FocusValue = new HiddenField();
            m_FocusDataPath = new HiddenField();
            m_FocusText = new HiddenField();
        }

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        public bool EcDesignMode
        {
            get
            {

                return m_EcDesignMode;
            }
            set { m_EcDesignMode = value; }
        }


        /// <summary>
        /// 过滤字段,默认 true
        /// </summary>
        [Description("过滤字段,默认 true")]
        [DefaultValue(true)]
        public bool FilterField
        {
            get { return m_FilterField; }
            set { m_FilterField = value; }
        }




        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
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
 
        /// <summary>
        /// 节点集合
        /// </summary>
        [Browsable(false)]
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TreeNodeCollection Nodes
        {
            get
            {
                return this.RootNode.ChildNodes;
            }
        }


        /// <summary>
        /// 输出节点的 HTML 格式
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parnetNode"></param>
        /// <param name="dept"></param>
        private void RenderNode(HtmlTextWriter writer,TreeNode parnetNode,int dept)
        {

            TreeNode pNode = parnetNode;

            string dataPath;

            if (pNode.Value != null)
            {
                dataPath = pNode.Value.Replace('\\', '_');
            }

            string treeStatus = string.Empty;

            if (m_LoadDynamic)
            {
                treeStatus = (pNode.StatusID > 0 ? "jstree-closed " : "jstree-leaf ");
            }

            string isChecked = string.Empty ;   //= (pNode.CheckState == CheckState. ? "jstree-checked " : string.Empty);

            if (m_ShowCheckBox)
            {
                switch (pNode.CheckState)
                {
                    case CheckState.Checked:
                        isChecked = "jstree-checked ";
                        break;
                    case CheckState.Indeterminate:
                        isChecked = "jstree-undetermined ";
                        break;
                    case CheckState.Unchecked:
                        break;
                }
            }

            string className = treeStatus + isChecked;

            writer.Write("<li id='{0}_{1}' dataPath='{2}' value='{3}' class='{4}' rel='{5}'>",
                this.ClientID, pNode.Value, pNode.DataPath, pNode.Value,
                className, pNode.NodeType);

            writer.Write("<a href='#'>{0}</a>", pNode.Text);

            if (pNode.HasChildNode())
            {
                writer.Write("<ul>");

                foreach (TreeNode node in pNode.ChildNodes)
                {
                    RenderNode(writer, node, dept + 1);
                }

                writer.Write( "</ul>");
            }

            writer.Write( "</li>");
        }


        /// <summary>
        /// 选择节点产生的客户端事件
        /// </summary>
        [Description("选择节点产生的客户端事件")]
        public string OnSelectNodeClientScript
        {
            get { return m_OnSelectNodeClientScript; }
            set { m_OnSelectNodeClientScript = value; }
        }

        /// <summary>
        /// 展开节点事件
        /// </summary>
        [Description("展开节点事件")]
        public string OnOpenNodeClientScript
        {
            get { return m_OnOpenNodeClientScript; }
            set { m_OnOpenNodeClientScript = value; }
        }

        /// <summary>
        /// 节点重命名的事件
        /// </summary>
        [Description("节点重命名的事件")]
        public string OnRenameNodeClientScript
        {
            get { return m_OnRenameNodeClientScript; }
            set { m_OnRenameNodeClientScript = value; }
        }


        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;
            this.Controls.Clear();

            //m_NodeIdentity.ID = this.ID + "_NodeIdentity";
            //this.Controls.Add(m_NodeIdentity);

            m_FocusNodeID.ID = this.ID + "_FocusNodeID";
            this.Controls.Add(m_FocusNodeID);

            m_FocusValue.ID = this.ID + "_FocusValue";
            this.Controls.Add(m_FocusValue);

            m_FocusText.ID = this.ID + "_FocusText";
            this.Controls.Add(m_FocusText);

            m_FocusDataPath.ID = this.ID + "_FocusDataPath";
            this.Controls.Add(m_FocusDataPath);

        }

        /// <summary>
        /// 获取焦点节点的值
        /// </summary>
        [DefaultValue(""),Description("获取焦点节点的值")]
        public string FocusValue
        {
            get { return m_FocusValue.Value; }
            set
            {
                m_FocusValue.Value = value;
            }
        }

        /// <summary>
        /// 获取焦点节点的 Text
        /// </summary>
        [Description("获取焦点节点的 Text")]
        public string FocusText
        {
            get { return m_FocusText.Value; }
            set { m_FocusText.Value = value; }
        }

        /// <summary>
        /// 获取焦点节点的 ID
        /// </summary>
        [Description("获取焦点节点的 ID")]
        public string FoucsNodeID
        {
            get { return m_FocusNodeID.Value; }
            set { m_FocusNodeID.Value = value; }
        }

        /// <summary>
        /// 获取焦点节点的 DataPath
        /// </summary>
        [Description("获取焦点节点的 DataPath")]
        public string FoucsDataPath
        {
            get { return m_FocusDataPath.Value; }
            set { m_FocusDataPath.Value = value; }
        }


        /// <summary>
        /// 获取展开的节点
        /// </summary>
        /// <param name="parnetNode">父节点</param>
        /// <param name="expandNodes">展开的节点集合</param>
        /// <returns></returns>
        private void GetExpandNodes(TreeNode parnetNode, List<TreeNode> expandNodes)
        {
            if (!parnetNode.HasChildNode())
            {
                return;
            }

            if (parnetNode.IsExpanded)
            {
                expandNodes.Add(parnetNode);
            }

            foreach (TreeNode node in parnetNode.ChildNodes)
            {
                GetExpandNodes(node, expandNodes);
            }
        }

        List<TreeNode> m_AsyncNodeList ;

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="parentNodeId">父节点 ID</param>
        /// <param name="node">节点</param>
        public void AddNode(string parentNodeId, TreeNode node )
        {
            if (m_AsyncNodeList == null)
            {
                m_AsyncNodeList = new List<TreeNode>();
            }

            node.OwnerNodeID = parentNodeId;
            m_AsyncNodeList.Add(node);

        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="node"></param>
        public void AddNode(int parentNodeId, TreeNode node)
        {
            string strPNodeID = this.ClientID + "_" + parentNodeId;

            AddNode(strPNodeID, node);
        }


        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="nodeId"></param>
        public void RemoveNode(string nodeId)
        {
            MiniHelper.EvalFormat("$('#{0}').remove();", nodeId);
        }

        /// <summary>
        /// 改变节点类型
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <param name="nodeType">节点类型</param>
        public void ChangeNodeType(string nodeId, string nodeType)
        {
            MiniHelper.EvalFormat("$('#{0}').attr('rel','{1}');", nodeId, nodeType);
        }


        /// <summary>
        /// 异步添加显示
        /// </summary>
        /// <param name="parentNodeId"></param>
        /// <param name="sb"></param>
        private void AsyncRender(string parentNodeId,StringBuilder sb)
        {
            if (m_AsyncNodeList == null )
            {
                return;
            }

            //StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            HtmlTextWriter writer = new HtmlTextWriter(sw);


            writer.Write("<ul>");

            foreach (TreeNode node in m_AsyncNodeList)
            {
                //writer.Write("<li id='{0}_{1}' dataPath='{2}' value='{3}' class='{4}' rel='{5}'>",
                //    this.ClientID, node.Value, node.DataPath, node.Value,
                //    (node.StatusID > 0 ? "jstree-closed" : "jstree-leaf"), node.NodeType);

                //writer.Write("<a href='#'>{0}</a>", node.Text);


                string treeStatus = string.Empty;

                if (m_LoadDynamic)
                {
                    treeStatus = (node.StatusID > 0 ? "jstree-closed " : "jstree-leaf ");
                }

                string isChecked = string.Empty;   //= (pNode.CheckState == CheckState. ? "jstree-checked " : string.Empty);

                if (m_ShowCheckBox)
                {
                    switch (node.CheckState)
                    {
                        case CheckState.Checked:
                            isChecked = "jstree-checked ";
                            break;
                        case CheckState.Indeterminate:
                            isChecked = "jstree-undetermined ";
                            break;
                        case CheckState.Unchecked:
                            break;
                    }
                }

                string className = treeStatus + isChecked;

                writer.Write("<li id='{0}_{1}' dataPath='{2}' value='{3}' class='{4}' rel='{5}'>",
                    this.ClientID, node.Value, node.DataPath, node.Value,
                    className, node.NodeType);

                writer.Write("<a href='#'>{0}</a>", node.Text);




                int dept = 0;

                if (node.HasChildNode())
                {
                    writer.Write("<ul>");

                    foreach (TreeNode subNode in node.ChildNodes)
                    {
                        RenderNode(writer, node, dept + 1);
                    }

                    writer.Write("</ul>");
                }

                writer.Write("</li>");

            }

            writer.Write("</ul>");
        }

        
        /// <summary>
        /// (JScript)刷新
        /// </summary>
        /// <param name="parentNodeId"></param>
        public void Refresh(int parentNodeId)
        {
            string strPNodeID = this.ClientID + "_" + parentNodeId;

            Refresh(strPNodeID);
        }

        /// <summary>
        /// (JScript)刷新
        /// </summary>
        /// <param name="parentNodeId">父节点 ID</param>
        public void Refresh(string parentNodeId)
        {
            if (m_AsyncNodeList != null && m_AsyncNodeList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                AsyncRender(parentNodeId, sb);

                MiniHelper.EvalFormat("$(\"#{0}\").append(\"{1}\");", parentNodeId, sb.ToString().Replace("\"", "\\\""));
            }

            MiniHelper.EvalFormat("jQuery.jstree._reference('#{0}').refresh();",parentNodeId);
        }

        /// <summary>
        /// 刷新节点的 Text 属性
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="text"></param>
        public void ResetNodeText(string nodeId, string text)
        {
            MiniHelper.EvalFormat("$(\"#{0} a:first\").html(\"{1}\");",nodeId, JsonUtility.ToJson(text));
            //MiniHelper.EvalFormat("jQuery.jstree._reference('#{0}').refresh();", nodeId);
        }


        private void Render_Types(HtmlTextWriter writer)
        {

            if (m_Types == null || m_Types.Count == 0)
            {
                return;
            }

            writer.WriteLine("types: { ");

            //writer.Write("'max_depth' : -2,");
            //writer.Write("'max_children' : -2,");
            {
                writer.WriteLine("types: {");
                {

                    TreeNodeType nType = m_Types[0];

                    writer.Write("{0} : ", nType.Name);

                    writer.Write("{ icon: {image: ");

                    writer.Write("'{0}'", nType.Icon);

                    writer.Write("}}");

                    for (int i = 1; i < m_Types.Count; i++)
                    {
                        nType = m_Types[i];

                        writer.Write(",\n{0} : ", nType.Name);

                        writer.Write("{ icon: {image: ");

                        writer.Write("'{0}'", nType.Icon);

                        writer.Write("}}");
                    }
                }
                writer.WriteLine("}");

            }
            writer.WriteLine("},");
        }

        /// <summary>
        /// 查找所有叶
        /// </summary>
        /// <param name="leafList"></param>
        private void FindLeafList(List<TreeNode> leafList,TreeNode parentNode)
        {
            if (parentNode == null)
            {
                return;
            }

            if (!parentNode.HasChildNode())
            {
                leafList.Add(parentNode);
                return;
            }

            foreach (var node in parentNode.ChildNodes)
            {
                FindLeafList(leafList, node);
            }
        }

        private void GetNodeALL(List<TreeNode> list, TreeNode parentNode)
        {
            if (parentNode == null)
            {
                return;
            }

            if (parentNode.HasChildNode())
            {
                foreach (var node in parentNode.ChildNodes)
                {
                    GetNodeALL(list, node);
                }
            }

            list.Add(parentNode);
        }

        protected List<TreeNode> GetNodeALL()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            GetNodeALL(nodes,this.RootNode);

            return nodes;
        }

        /// <summary>
        /// 特殊处理复选框
        /// </summary>
        private void ProCheckNode()
        {
            if (!m_ShowCheckBox)
            {
                return;
            }

            List<TreeNode> nodeList = GetNodeALL();

            foreach (var node in nodeList)
            {
                if (node.CheckState == CheckState.Unchecked)
                {
                    continue;
                }

                if (node.Parent == null)
                {
                    continue;
                }

                TreeNode pNode = node.Parent;

                CheckState parentCS = CheckState.Unchecked;

                int checkedCount = 0;
                int indetCount = 0;
                int uncheckedCount = 0;

                //查找同级的
                foreach (var tongNode in pNode.ChildNodes)
                {
                    switch (tongNode.CheckState)
                    {
                        case CheckState.Checked: checkedCount++;
                            break;
                        case CheckState.Indeterminate: indetCount++;
                            break;
                        case CheckState.Unchecked: uncheckedCount++;
                            break;
                    }
                }

                if (checkedCount == pNode.ChildNodes.Count)
                {
                    parentCS = CheckState.Checked;
                }
                else if (uncheckedCount == pNode.ChildNodes.Count)
                {
                    parentCS = CheckState.Unchecked;
                }
                else
                {
                    parentCS = CheckState.Indeterminate;
                }


                //查找所有上级,是否为复选
                for (int i = 0; i < 20; i++)
                {
                    if (pNode.CheckState != CheckState.Indeterminate)
                    {
                        pNode.CheckState = parentCS;
                    }


                    pNode = pNode.Parent;

                    if (pNode == null)
                    {
                        break;
                    }
                }


            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            EnsureChildControls();


            m_FocusValue.RenderControl(writer);
            m_FocusText.RenderControl(writer);
            m_FocusDataPath.RenderControl(writer);

            writer.WriteLine("<div id='{0}' style='height:400px; display:none;'>", this.ClientID);
            writer.WriteLine("  <ul>");

            //RenderNode(writer, this.RootNode, 0);

            //对拥有复选框的树节点,进行特殊处理
            ProCheckNode();

            foreach (TreeNode item in this.RootNode.ChildNodes)
            {
                RenderNode(writer, item, 0);
            }


            writer.WriteLine("  </ul>");
            writer.WriteLine("</div>");

            
            List<TreeNode> expandNodes = new List<TreeNode>();
            foreach (TreeNode node in this.Nodes)
            {
                GetExpandNodes(node, expandNodes);
            }

            writer.WriteLine("<script type='text/javascript' >");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs" || jsMode == "MInJs")
            {
                writer.WriteLine("In.ready('jq.jstree',function() {");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            string plgCheckbox = (m_ShowCheckBox ? ",'checkbox'" : string.Empty);
            string plgTypes = (m_Types != null && m_Types.Count > 0) ? ", 'types'" : string.Empty;

            writer.WriteLine("        $('#" + this.ClientID + "').jstree({");
            writer.WriteLine("            plugins: ['themes', 'html_data', 'ui', 'crrm' " + plgCheckbox + plgTypes + " ],");


            Render_Types(writer);



            writer.Write("            core: {");

            Render_initiallyOpen(writer, expandNodes);  //默认展开的节点

            writer.WriteLine("animation:0}");
            writer.WriteLine("        })");


            Render_openNode(writer);//打开节点触发的事件

            Render_selectNode(writer); //选中节点触发的事件

            Render_renameNode(writer);  //重命名触发的事件

            writer.WriteLine(";");

            writer.WriteLine("$('#" + this.ClientID + "').show();");

            writer.WriteLine("    });");
            writer.WriteLine("</script>");

        }

        /// <summary>
        /// 默认展开的节点
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="expandNodes"></param>
        private void Render_initiallyOpen(HtmlTextWriter writer, List<TreeNode> expandNodes)
        {
            if (expandNodes.Count == 0)
            {
                return;
            }

            writer.Write(" initially_open : [");

            writer.Write("'{0}_{1}'", this.ClientID, expandNodes[0].Value.Replace('\\', '_'));

            for (int i = 1; i < expandNodes.Count; i++)
            {
                writer.Write(",'{0}_{1}'", this.ClientID, expandNodes[i].Value.Replace('\\', '_'));
            }

            writer.WriteLine("],");

        }


        /// <summary>
        /// 打开节点触发的事件
        /// </summary>
        /// <param name="writer"></param>
        private void Render_openNode(HtmlTextWriter writer)
        {
            writer.WriteLine("        .bind('open_node.jstree', function(event,data){");

            writer.WriteLine("            var id = data.rslt.obj.attr('id');");
            writer.WriteLine("            if ($('#' + id + ' li').length > 0) { return; }");

            if (!string.IsNullOrEmpty(m_OnOpenNodeClientScript))
            {
                writer.WriteLine("         " + m_OnOpenNodeClientScript);
            }

            writer.WriteLine("        })");
        }

        /// <summary>
        /// 选中节点触发的事件
        /// </summary>
        /// <param name="writer"></param>
        private void Render_selectNode(HtmlTextWriter writer)
        {
            writer.WriteLine("        .bind('select_node.jstree', function (event, data) {");

            writer.WriteLine("            var obj = data.rslt.obj;");

            writer.WriteLine("            $('#{0}').val( $(obj).attr('id') );", m_FocusNodeID.ClientID);

            writer.WriteLine("            $('#{0}').val( obj.attr('value') );", m_FocusValue.ClientID);
            writer.WriteLine("            $('#{0}').val( obj.attr('text') );", m_FocusText.ClientID);
            writer.WriteLine("            $('#{0}').val( obj.attr('dataPath') );", m_FocusDataPath.ClientID);

            if (!string.IsNullOrEmpty(m_OnSelectNodeClientScript))
            {
                writer.WriteLine("         " + m_OnSelectNodeClientScript);
            }

            writer.WriteLine("        })");


        }


        /// <summary>
        /// 选中节点触发的事件
        /// </summary>
        /// <param name="writer"></param>
        private void Render_renameNode(HtmlTextWriter writer)
        {
            writer.WriteLine("        .bind('rename_node.jstree', function (event, data) {");

            writer.WriteLine("            var obj = data.rslt.obj;");

            writer.WriteLine("            $('#{0}').val( $(obj).attr('id') );", m_FocusNodeID.ClientID);

            writer.WriteLine("            $('#{0}').val( obj.attr('value') );", m_FocusValue.ClientID);
            writer.WriteLine("            $('#{0}').val( obj.attr('text') );", m_FocusText.ClientID);
            writer.WriteLine("            $('#{0}').val( obj.attr('dataPath') );", m_FocusDataPath.ClientID);

            if (!string.IsNullOrEmpty(m_OnRenameNodeClientScript))
            {
                writer.WriteLine("         " + m_OnRenameNodeClientScript);
            }

            writer.WriteLine("        })");

        }

        public void LoadPostData()
        {
            //m_NodeIDIdentity = StringUtility.ToInt(m_NodeIdentity.Value);


        }


    }




}
