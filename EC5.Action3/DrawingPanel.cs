using EC5.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 绘制面板
    /// </summary>
    public class DrawingPanel:CodeIndexItem
    {


        public DrawingPanel()
        {

        }

        public DrawingPanel(string code) : base(code)
        {

        }

        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 所有节点
        /// </summary>
        NodeCollection m_Nodes = new NodeCollection();

        /// <summary>
        /// 监听集合
        /// </summary>
        ListenCollection m_Listens = new ListenCollection();


        /// <summary>
        /// 操作集合
        /// </summary>
        OperateCollection m_Operates = new OperateCollection();


        _AC3MethodTableIDX m_MethodTableListIndex = new _AC3MethodTableIDX();

        

        public List<ListenTable> GetListenItems(string tabelName, ListenMethod method)
        {
            return m_MethodTableListIndex.TryGet(tabelName, method);
        }


        /// <summary>
        /// 构造连接线
        /// </summary>
        private void BuilderLine()
        {
            foreach (ActionItemBase node in m_Nodes)
            {
                BuilderRoute(node);
            }

            foreach (ListenTable listen in m_Listens)
            {
                m_MethodTableListIndex.TryAdd(listen);
            }
        }

        /// <summary>
        /// 构造连接线的路由
        /// </summary>
        /// <param name="node"></param>
        private void BuilderRoute(ActionItemBase node)
        {
            foreach (RouteItem route in node.Routes)
            {
                ActionItemBase target;

                if (!m_Nodes.TryGet(route.Target, out target))
                {
                    throw new Exception($"没有这个节点. 【{route.Target}】");
                }

                route.TargetNode = target;
            }
        }

        /// <summary>
        /// 构造监听过滤器
        /// </summary>
        private void BuilderFilter_listen()
        {
            ScriptBase listenScript;

            foreach (ListenTable listen in m_Listens)
            {
                listenScript = listen.CondScript;

                if (listenScript == null)
                {
                    continue;
                }

                string code = listenScript.Code;

                if (listenScript is ScriptJson)
                {
                    try
                    {
                        listen.CondFilter = JsonSQL.ScriptJQL.ParseJson(code); 
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("转换 json 编码失败, code=\n" + code, ex);
                    }
                }
                else if (listenScript is ScriptCSharp)
                {
                    try
                    {
                        listen.CondFilter = CSharpFilter.ParseCSarp(code);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("转换 C# 编码失败, code=\n" + code, ex);
                    }
                }
                else
                {
                    throw new Exception($"未知脚本类型: {listenScript.GetType().FullName}");
                }


            }
        }

        /// <summary>
        /// 构造操作的
        /// </summary>
        private void BuilderFilter_operate()
        {
            string code;

            ScriptBase script = null;


            foreach (OperateTable op in m_Operates)
            {
                if (op.FilterScript == null)
                {
                    continue;
                }
                
                script = op.FilterScript;
                code = script.Code;

                if (StringUtil.IsBlank(code))
                {
                    continue;
                }

                if (script is ScriptJson)
                {
                    
                    try
                    {
                        op.Filter = JsonSQL.ScriptJQL.ParseJson(code);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("转换 json 编码失败, code=\n" + code, ex);
                    }
                }
                else if (script is ScriptCSharp)
                {
                    try
                    {
                        op.Filter = CSharpFilter.ParseCSarp(code);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("转换 C# 编码失败, code=\n" + code, ex);
                    }
                }
                else
                {
                    throw new Exception($"未知脚本类型: {script.GetType().FullName}");
                }
            }

            


            foreach (OperateTable op in m_Operates)
            {
                BuilderOpFields(op.NewFields);
                BuilderOpFields(op.UpdateFields);
            }
        }

        private void BuilderOpFields(OperateFieldCollection fields)
        {
            foreach (var f in fields)
            {
                if (f.ValueMode == ActionModeType.Fun)
                {
                    if (!string.IsNullOrWhiteSpace(f.Value))
                    {
                        if (!f.Value.Contains("return "))
                        {
                            f.Value = "return " + f.Value + ";";
                        }
                    }
                    else
                    {
                        f.ValueMode = ActionModeType.Fun;
                    }
                }
            }
        }


        /// <summary>
        /// 构造过滤器
        /// </summary>
        private void BuilderFilter()
        {
            BuilderFilter_listen();

            BuilderFilter_operate();
        }


        
        /// <summary>
        /// 构造
        /// </summary>
        public void Builder()
        {            
            BuilderLine();      //构造连接线            
            BuilderFilter();    //构造过滤器
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(ActionItemBase item)
        {
            if (item == null) throw new ArgumentNullException("item 不能为空.");

            m_Nodes.Add(item);


            if (item is ListenBase)
            {
                this.m_Listens.Add((ListenBase)item);
            }
            else if(item is OperateBase)
            {
                this.m_Operates.Add((OperateBase)item);
            }
            else
            {
                throw new Exception("未知类型");
            }
            
                        

        }

        public void AddRange(ActionItemBase item0, ActionItemBase item1, ActionItemBase item2, params ActionItemBase[] items)
        {
            Add(item0);
            Add(item1);
            Add(item2);
            AddRange(items);
        }

        public void AddRange(ActionItemBase item0, ActionItemBase item1, ActionItemBase item2)
        {
            Add(item0);
            Add(item1);
            Add(item2);
        }

        public void AddRange(ActionItemBase item0, ActionItemBase item1)
        {
            Add(item0);
            Add(item1);
        }


        public void AddRange(IEnumerable<ActionItemBase> items)
        {
            foreach (var item in items)
            {
                
                if (item == null) throw new ArgumentNullException("item 不能为空.");

                if(m_Nodes.ContainsCode(item.Code))
                {
                    throw new Exception($"code={item.Code} 重复.");
                }
                
            }

            foreach (var item in items)
            {
                if (item == null) throw new ArgumentNullException("item 不能为空.");

                this.Add(item);
            }

        }
        

    }



}
