using EC5.DbCascade.DbCascadeEngine;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade
{
    /// <summary>
    /// 步骤路径
    /// </summary>
    public class BizDbStepPath
    {
        /// <summary>
        /// 根步骤
        /// </summary>
        BizDbStep m_Root = new BizDbStep();

        BizDbStep m_Cur = null;

        /// <summary>
        /// 进寨
        /// </summary>
        Stack<BizDbStep> m_Items = new Stack<BizDbStep>();


        /// <summary>
        /// 错误消息
        /// </summary>
        List<string> m_Errors = new List<string>();

        public BizDbStep Root
        {
            get { return m_Root; }
        }



        public BizDbStep Cur
        {
            get
            {
                if (m_Cur == null)
                {
                    m_Cur = m_Root;
                }

                return m_Cur;
            }
            set { m_Cur = value; }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public List<string> Errors
        {
            get { return m_Errors; }
        }


        /// <summary>
        /// 创建被阻止的节点
        /// </summary>
        /// <param name="resultMsg">返回的消息</param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        public void Create(string resultMsg, DbccModel dbccModel, LModel srcModel)
        {
            Create(resultMsg, dbccModel, srcModel, this.Cur);
        }

        /// <summary>
        /// 创建被阻止的节点
        /// </summary>
        /// <param name="resultMsg">返回的消息</param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        public void Error(string resultMsg, DbccModel dbccModel, LModel srcModel)
        {
            Create(resultMsg, dbccModel, srcModel, this.Cur);

            m_Errors.Add(resultMsg);
        }

        /// <summary>
        /// 创建被阻止的节点
        /// </summary>
        /// <param name="resultMsg">返回的消息</param>
        /// <param name="dbccModel"></param>
        /// <param name="srcModel"></param>
        /// <param name="parStep">上级步骤</param>
        public void Create(string resultMsg, DbccModel dbccModel, LModel srcModel, BizDbStep parStep)
        {
            BizDbStep step = parStep.Childs.Add(BizDbStepType.NONE);
            step.ActionId = dbccModel.ID;

            if (srcModel != null)
            {
                LModelElement modelElem = srcModel.GetModelElement();
                step.Table = modelElem.DBTableName;

                step.Models.Add(srcModel);
            }

            step.CreateLog();

            step.ResultMessage = resultMsg;// "过滤阻止.";
        }

        public BizDbStep Create(BizDbStep step)
        {
            this.Cur.Childs.Add(step);

            step.CreateLog();

            return step;
        }

    }
}
