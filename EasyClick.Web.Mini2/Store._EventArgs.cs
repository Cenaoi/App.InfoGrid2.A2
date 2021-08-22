using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 实体列表的事件参数
    /// </summary>
    public class ObjectListEventArgs : EventArgs
    {
        IList m_ObjectList;

        string[] m_BlemishFieldAll;


        /// <summary>
        /// 实体列表的事件参数
        /// </summary>
        /// <param name="objectList">记录集合</param>
        public ObjectListEventArgs(IList objectList)
        {
            m_ObjectList = objectList;
        }

        /// <summary>
        /// 实体列表的事件参数
        /// </summary>
        /// <param name="objectList">记录集合</param>
        /// <param name="blemishFieldAll"></param>
        public ObjectListEventArgs(IList objectList, string[] blemishFieldAll)
        {
            m_ObjectList = objectList;
            m_BlemishFieldAll = blemishFieldAll;
        }

        ObjectEventItemCollection m_Items;

        public ObjectEventItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ObjectEventItemCollection();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList ObjectList
        {
            get { return m_ObjectList; }
        }

        /// <summary>
        /// 全部包含弄脏的字段
        /// </summary>
        public string[] BlemishFieldAll
        {
            get { return m_BlemishFieldAll; }
        }
    }


    /// <summary>
    /// 实体列表的事件参数
    /// </summary>
    public class ObjectListCancelEventArgs : CancelEventArgs
    {
        IList m_ObjectList;

        /// <summary>
        /// 实体列表的事件参数
        /// </summary>
        /// <param name="objectList">记录集合</param>
        public ObjectListCancelEventArgs(IList objectList)
        {
            m_ObjectList = objectList;
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList ObjectList
        {
            get { return m_ObjectList; }
        }


        ObjectEventItemCollection m_Items;

        public ObjectEventItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ObjectEventItemCollection();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }
    }


    /// <summary>
    /// 事件的项目集合
    /// </summary>
    public class ObjectEventItemCollection : SortedDictionary<string, object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (base.TryGetValue(key, out object value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                base[key] = value;
            }
        }
    }


    /// <summary>
    /// 实体的事件参数
    /// </summary>
    public class ObjectEventArgs : EventArgs
    {
        object m_Object;

        DataRecord m_SrcRecord;

        bool m_DeleteRecycle = false;

        Exception m_Exception;


        ObjectEventItemCollection m_Items;




        /// <summary>
        /// 属性指示该异常是否已在事件处理程序中得到处理。
        /// </summary>
        bool m_ExceptionHandled = false;

        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="obj">记录对象</param>
        public ObjectEventArgs(object obj)
        {
            m_Object = obj;
        }


        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="exp">异常信息</param>
        /// <param name="obj">记录对象</param>
        public ObjectEventArgs(Exception exp, object obj)
        {
            m_Exception = exp;
            m_Object = obj;
        }

        /// <summary>
        /// 项目集合
        /// </summary>
        public ObjectEventItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ObjectEventItemCollection();
                }

                return m_Items;
            }
            set { m_Items = value; }
        }


        /// <summary>
        /// 属性指示该异常是否已在事件处理程序中得到处理。
        /// </summary>
        public bool ExceptionHandled
        {
            get { return m_ExceptionHandled; }
            set { m_ExceptionHandled = value; }
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception
        {
            get { return m_Exception; }
            set { m_Exception = value; }
        }

        /// <summary>
        /// 记录对象
        /// </summary>
        public object Object
        {
            get { return m_Object; }
        }


        /// <summary>
        /// 原数据库对象
        /// </summary>
        public DataRecord SrcRecord
        {
            get { return m_SrcRecord; }
            set { m_SrcRecord = value; }
        }

        /// <summary>
        /// 删除回收模式
        /// </summary>
        public bool DeleteRecycle
        {
            get { return m_DeleteRecycle; }
            set { m_DeleteRecycle = value; }
        }
    }


    /// <summary>
    /// 实体的事件参数
    /// </summary>
    public class ObjectCancelEventArgs : CancelEventArgs
    {
        object m_Object;

        /// <summary>
        /// 原数据对象
        /// </summary>
        DataRecord m_SrcRecord;

        /// <summary>
        /// 删除回收模式
        /// </summary>
        bool m_DeleteRecycle = false;


        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="obj">记录对象</param>
        public ObjectCancelEventArgs(object obj)
        {
            m_Object = obj;
        }


        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="obj">记录对象</param>
        public ObjectCancelEventArgs(object obj, bool deleteRecycle)
        {
            m_Object = obj;
        }

        ObjectEventItemCollection m_Items;

        public ObjectEventItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ObjectEventItemCollection();
                }
                return m_Items;
            }
            set { m_Items = value; }
        }


        /// <summary>
        /// 删除回收模式
        /// </summary>
        public bool DeleteRecycle
        {
            get { return m_DeleteRecycle; }
        }

        /// <summary>
        /// 记录对象
        /// </summary>
        public object Object
        {
            get { return m_Object; }
        }

        /// <summary>
        /// 原数据库对象
        /// </summary>
        public DataRecord SrcRecord
        {
            get { return m_SrcRecord; }
            set { m_SrcRecord = value; }
        }
    }
}
