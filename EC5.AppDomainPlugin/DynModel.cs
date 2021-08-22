using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EC5.AppDomainPlugin
{
    /// <summary>
    /// 动态实体.
    /// </summary>
    public class DynModel: DynamicObject
    {
        object m_SrcModel;

        bool m_IsSModel = false;
        bool m_IsLightModel = false;

        /// <summary>
        /// (构造函数) 动态实体
        /// </summary>
        /// <param name="srcModel"></param>
        public DynModel(object srcModel)
        {
            m_SrcModel = srcModel;

            if (srcModel is LightModel)
            {
                m_IsLightModel = true;
            }
            else if (srcModel is SModel)
            {
                m_IsSModel = true;
            }
        }

        /// <summary>
        /// (构造函数) 动态实体
        /// </summary>
        protected DynModel()
        {
        }

        /// <summary>
        /// 原实体对象
        /// </summary>
        public object SrcModel
        {
            get { return m_SrcModel; }
            internal set { m_SrcModel = value; }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                object result = null;

                TryGet(name, out result);

                return result;
            }
            set
            {
                TrySet(name, value);
            }
        }


        private bool TrySet(string name, object value)
        {
            bool tryResult = true;

            if (m_IsSModel)
            {
                SModel sm = (SModel)m_SrcModel;
                sm[name] = value;

            }
            else if (m_IsLightModel)
            {
                LightModel model = (LightModel)m_SrcModel;
                model[name] = value;

            }
            else
            {
                tryResult = false;
            }

            return tryResult;
        }

        private bool TryGet(string name, out object result)
        {
            bool tryResult = true;

            if (m_IsSModel)
            {
                SModel sm = (SModel)m_SrcModel;
                result = sm[name];
            }
            else if (m_IsLightModel)
            {
                LightModel model = (LightModel)m_SrcModel;
                result = model[name];
            }
            else
            {
                result = null;
                tryResult = false;
            }

            return tryResult;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return TrySet(binder.Name, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return TryGet(binder.Name, out result);
        }


    }
}
