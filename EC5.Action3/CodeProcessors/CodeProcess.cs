using HWQ.Entity;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace EC5.Action3.CodeProcessors
{


    /// <summary>
    /// 代码编译器
    /// </summary>
    public class CodeProcess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CodeContext m_Context;

        /// <summary>
        /// 编码的上下文
        /// </summary>
        public CodeContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        /// <summary>
        /// (构造函数)代码编译器
        /// </summary>
        public CodeProcess()
        {

        }

        /// <summary>
        /// (构造函数)代码编译器
        /// </summary>
        /// <param name="context">编码的上下文</param>
        public CodeProcess(CodeContext context)
        {
            m_Context = context;
        }



        public void Exec(List<LightModel> models, OperateMethod method)
        {
            if (models == null) throw new ArgumentNullException("models");

            if (models.Count == 0) { throw new ArgumentNullException("model"); }



        }

        /// <summary>
        /// 创建一个操作的触发源
        /// </summary>
        /// <param name="opMethod"></param>
        /// <param name="table"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private TreeOperateGroup CreateOperateTable(OperateMethod opMethod, string table, LightModel model)
        {
            OperateTable opRoot = new OperateTable("_ROOT", opMethod);
            opRoot.Table = table;


            TreeOperateGroup opGroup = new TreeOperateGroup();
            opGroup.Operates.Add(new TreeOperateItem(opRoot, model));
            opGroup.Status = StepStatus.End;

            return opGroup;
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        private CodeContext GetContext()
        {
            CodeContext context = m_Context;

            if (context == null)
            {
                context = new CodeContext();
                m_Context = context;
            }

            return context;
        }



        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="method"></param>
        public void Exec(LightModel model, OperateMethod method)
        {
            Console.WriteLine();

            Stopwatch sw = new Stopwatch();


            if (model == null) { throw new ArgumentNullException("model"); }


            LModelElement modelElem = ModelElemHelper.GetElem(model);
            string table = modelElem.DBTableName;

            CodeContext context = GetContext();


            DbDecipher decipher = context.Decipher;

            sw.Restart();

            switch (method)
            {
                case OperateMethod.Insert:
                    decipher.InsertModel(model);
                    break;
                case OperateMethod.Update:
                    decipher.UpdateModel(model,true);
                    break;
                case OperateMethod.Delete:
                    decipher.DeleteModel(model);
                    break;
            }

            sw.Stop();
            log.Debug($"操数据库时间: {sw.Elapsed.TotalMilliseconds:#,##0.000} 毫秒");

            TreeNode opGroup = CreateOperateTable(method, table, model);


            //创建一个操作的触发源
            context.RootNode = opGroup;

            ConcurrentStack<TreeNode> path = context.Path;

            //开始执行, 并计算路径
            path.Push(opGroup);

            context.CurNode = opGroup;


            MoveDirection moveDir;

            TreeNode cur;
            


            while (true)
            {
                cur = context.CurNode;

                if (cur == null)
                {
                    break;
                }


                if (!cur.IsPass)
                {
                    sw.Restart();

                    cur.Read(context);

                    sw.Stop();

                    cur.ReadTime = sw.Elapsed;
                }


                if (cur.Status == StepStatus.End)
                {
                    moveDir = MoveNode(context);
                }


                if (path.Count == 0)
                {
                    break;
                }

            }
        }

        /// <summary>
        /// 向前读取节点
        /// </summary>
        /// <param name="context">上下文</param>
        public void Read(CodeContext context)
        {
            TreeNode cur = context.CurNode;


            cur.Context = context;

            if (cur.IsPass)
            {
                return;
            }

            cur.Read();
        }

        /// <summary>
        /// 移动到下一个节点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cur"></param>
        /// <returns></returns>
        private MoveDirection MoveNode(CodeContext context)
        {
            MoveDirection moveDrt = MoveDirection.None;

            TreeNode cur = context.CurNode;

            if (cur == null || cur.Status != StepStatus.End)
            {
                return moveDrt;
            }

            TreeNode first = cur.FirstChild;
            TreeNode next = cur.Next;

            //获取下一个节点
            if (first != null && first.Status == StepStatus.None)
            {
                cur.IsPass = true;

                cur = first;

                context.CurNode = cur;
                context.CurSubItem = null;

                context.Path.Push(cur);

                moveDrt = MoveDirection.Child;
            }
            else if (next != null && next.Status == StepStatus.None)
            {
                cur.IsPass = true;

                cur = next;

                context.CurNode = cur;
                context.CurSubItem = null;

                context.Path.Push(cur);

                moveDrt = MoveDirection.Next;
            }
            else
            {
                TreeNode tmpCur = null;
                context.Path.TryPop(out tmpCur);

                context.CurNode = tmpCur.Parent;
                context.CurSubItem = null;

                moveDrt = MoveDirection.Back;
            }

            return moveDrt;
        }
    }
}
