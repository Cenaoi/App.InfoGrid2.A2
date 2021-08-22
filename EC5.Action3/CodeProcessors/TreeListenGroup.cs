using EC5.Action3.SEngine;
using HWQ.Entity;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 监听组节点, 大部分只有一个监听, 有些是需要大于一个的监听组合的.
    /// </summary>
    public class TreeListenGroup:TreeNode
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TreeListenGroup()
        {

        }

        public TreeListenGroup(TreeListenItem item)
        {
            this.Listens.Add(item);
        }
       


        int m_ListenIndex = 0;

        List<TreeListenItem> m_Listens;

        /// <summary>
        /// 条件成立的项目
        /// </summary>
        List<TreeListenItem> m_CondHoldItems;

        public List<TreeListenItem> Listens
        {
            get
            {
                if(m_Listens == null)
                {
                    m_Listens = new List<TreeListenItem>();
                }

                return m_Listens;
            }
        }
        

        /// <summary>
        /// 运行代码块
        /// </summary>
        /// <param name="i"></param>
        /// <param name="item"></param>
        private void RunCodeBlock(int i, TreeListenItem item)
        {
            ListenTable li = item.Listen;

            CodeContext context = this.Context;

            if(context.CurParams == null)
            {
                context.CurParams = new SModel();
            }

            string code = item.Listen.Code;



            SModel itemParam = new SModel();
            itemParam["role"] = "listen";   //监听角色
            itemParam["code"] = code;
            itemParam["data"] = item.SourceData;

            //设置监听的数据
            context.CurParams[code] = itemParam ;


                
            if (li.CondFilter != null)
            {
                FilterItem filter = li.CondFilter;

                if (item.SourceData is LightModel)
                {
                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();



                    bool valid = filter.Valid(this.Context, (LightModel)item.SourceData);

                    //sw.Stop();

                    //log.Debug($"执行代码块，耗时：{sw.Elapsed.TotalMilliseconds:#,##0.000} 毫秒");

                    item.IsConditionsHold = valid;

                }
                else
                {
                    object sd = item.SourceData;

                    if (sd != null)
                    {
                        throw new Exception($"未知数据源类型。Type={sd.GetType().FullName}");
                    }
                }

            }

            

        }

        /// <summary>
        /// 添加条件成立的项目
        /// </summary>
        /// <param name="item">监听项目</param>
        private void AddHoldItem(TreeListenItem item)
        {
            if (m_CondHoldItems == null)
            {
                m_CondHoldItems = new List<TreeListenItem>();
            }

            m_CondHoldItems.Add(item);
        }

        public override bool Read(CodeContext context)
        {
            this.Context = context;

            if (this.Status == StepStatus.None)
            {                
                this.Status = StepStatus.Running;
            }

            if (this.Status == StepStatus.Running)
            {
                if (m_ListenIndex < m_Listens.Count)
                {
                    TreeListenItem item = m_Listens[m_ListenIndex];

                    context.CurSubItem = item.Listen;
                    

                    RunCodeBlock(m_ListenIndex, item);  //运行代码块

                    m_ListenIndex++;

                    if (item.IsConditionsHold)
                    {
                        AddHoldItem(item);
                    }
                }
                
                if(m_ListenIndex >= m_Listens.Count)
                {
                    this.Status = StepStatus.End;
                }

            }            

            if(this.Status == StepStatus.End)
            {
                ProRead_End();
            }
             

            return (this.Status == StepStatus.End);
        }

        private void ProRead_End()
        {
            if (m_CondHoldItems != null && m_CondHoldItems.Count > 0)
            {

                foreach (var item in m_CondHoldItems)
                {
                    TreeOperateGroup opGroup = CreateOpGroup(item);

                    this.Childs.Add(opGroup);
                }
            }
        }

        /// <summary>
        /// 创建操作组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TreeOperateGroup CreateOpGroup(TreeListenItem item)
        {
            ListenTable listen = item.Listen;

            RouteItem route = listen.Routes[0];

            OperateTable opTab = route.TargetNode as OperateTable;

            TreeOperateItem opItem = new TreeOperateItem
            {
                Operate = opTab,
                SourceData = item.SourceData
            };

            TreeOperateGroup opGroup = new TreeOperateGroup();
            opGroup.Operates.Add(opItem);

            return opGroup;

        }

    }
}
