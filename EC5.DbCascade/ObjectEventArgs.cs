using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade
{

    /// <summary>
    /// 实体的事件参数
    /// </summary>
    public class ObjectEventArgs : EventArgs
    {
        object m_Object;

        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="data">记录对象</param>
        public ObjectEventArgs(object obj)
        {
            m_Object = obj;
        }




        /// <summary>
        /// 记录对象
        /// </summary>
        public object Object
        {
            get { return m_Object; }
        }

    }
}
