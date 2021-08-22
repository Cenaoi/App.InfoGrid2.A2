using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;

namespace EC5.DbCascade
{

    /// <summary>
    /// 操作步骤集合
    /// </summary>
    public class BizDbStepCollection : List<BizDbStep>,IDisposable
    {
        BizDbStep m_Owner;

        public BizDbStepCollection(BizDbStep owner)
        {
            m_Owner = owner;
        }

        public BizDbStep Owner
        {
            get { return m_Owner; }
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        public BizDbStep Add(BizDbStepType stepType)
        {
            BizDbStep item = new BizDbStep(stepType);
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType">步骤类型</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BizDbStep Add(BizDbStepType stepType, LModel model)
        {
            BizDbStep item = new BizDbStep(stepType, model); 
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public BizDbStep Add(BizDbStepType stepType, IList<LModel> models)
        {
            BizDbStep item = new BizDbStep(stepType, models);
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }


        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        public BizDbStep Add(string stepType)
        {
            BizDbStep item = new BizDbStep(stepType);
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BizDbStep Add(string stepType, LModel model)
        {
            BizDbStep item = new BizDbStep(stepType, model);
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="stepType"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public BizDbStep Add(string stepType, IList<LModel> models)
        {
            BizDbStep item = new BizDbStep(stepType, models);
            item.LogEnabled = m_Owner.LogEnabled;
            item.Depth = m_Owner.Depth + 1;

            base.Add(item);

            return item;
        }

        public void Dispose()
        {
            m_Owner = null;

            GC.SuppressFinalize(this);
        }
    }
}
