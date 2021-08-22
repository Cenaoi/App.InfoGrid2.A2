using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 实体列表的事件参数
    /// </summary>
    public class DataListEventArgs : EventArgs
    {
        IList m_DataList;

        /// <summary>
        /// 实体列表的事件参数
        /// </summary>
        /// <param name="dataList"></param>
        public DataListEventArgs(IList dataList)
        {
            m_DataList = dataList;
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList DataList
        {
            get { return m_DataList; }
        }
    }


    /// <summary>
    /// 实体列表的事件参数
    /// </summary>
    public class DataListCancelEventArgs : CancelEventArgs
    {
        IList m_DataList;

        /// <summary>
        /// 实体列表的事件参数
        /// </summary>
        /// <param name="dataList"></param>
        public DataListCancelEventArgs(IList dataList)
        {
            m_DataList = dataList;
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IList DataList
        {
            get { return m_DataList; }
        }
    }

    /// <summary>
    /// 实体的事件参数
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        object m_Data;

        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="data"></param>
        public DataEventArgs(object data)
        {
            m_Data = data;
        }

        public object Data
        {
            get { return m_Data; }
        }
    }


    /// <summary>
    /// 实体的事件参数
    /// </summary>
    public class DataCancelEventArgs : CancelEventArgs
    {
        object m_Data;

        /// <summary>
        /// 实体的事件参数
        /// </summary>
        /// <param name="data"></param>
        public DataCancelEventArgs(object data)
        {
            m_Data = data;
        }

        public object Data
        {
            get { return m_Data; }
        }
    }

    public delegate void DataListEventHandler(object sender, DataListEventArgs e);
    public delegate void DataListCancelEventHandler(object sender, DataListCancelEventArgs e);

    public delegate void DataEventHandler(object sender, DataEventArgs e);
    public delegate void DataCancelEventHandler(object sender, DataCancelEventArgs e);


    public static class DataStoreManager
    {
        static string m_DefaultStoreType;
        static Type m_DefaultStoreT;

        static SortedDictionary<string, Type> m_StoreList = new SortedDictionary<string, Type>();

        public static string DefaultStoreType
        {
            get { return m_DefaultStoreType; }
            set
            {
                m_DefaultStoreType = value;

                string name = value.ToLower();

                if (m_StoreList.ContainsKey(name))
                {
                    m_DefaultStoreT = m_StoreList[name];
                }
                else
                {
                    m_DefaultStoreT = null;
                }
            }
        }

        public static void AddStoreT(string name,Type storeT)
        {
            name = name.ToLower();

            m_StoreList.Add(name, storeT);

            if(!string.IsNullOrEmpty(m_DefaultStoreType) && 
                m_DefaultStoreType.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                m_DefaultStoreT = storeT;
            }
        }

        public static Type GetStoreT(string name)
        {

            name = name.ToLower();

            return m_StoreList[name];
        }

        public static void Clear()
        {
            m_StoreList.Clear();
        }

        public static DataStoreControl GetStore(string name)
        {
            name = name.ToLower();
            Type storeT = m_StoreList[name];

            DataStoreControl store = (DataStoreControl)Activator.CreateInstance(storeT);

            return store;
        }

        /// <summary>
        /// 获取默认数据仓库的类型
        /// </summary>
        /// <returns></returns>
        public static Type GetDefaultStoreT()
        {
            return m_DefaultStoreT;
        }

        /// <summary>
        /// 获取默认仓库的实例化对象
        /// </summary>
        /// <returns></returns>
        public static DataStoreControl GetDefaultStore()
        {
            DataStoreControl store = (DataStoreControl)Activator.CreateInstance(m_DefaultStoreT);

            return store;
        }
        
    }

    /// <summary>
    /// 数据仓库控件
    /// </summary>
    public abstract class DataStoreControl :Control, IListSource
    {

        #region 事件定义

        public event DataListCancelEventHandler PageChanging;
        public event DataListEventHandler PageChanged;

        public event DataListCancelEventHandler Selecting;
        public event DataListEventHandler Selected;


        public event DataCancelEventHandler Updating;
        public event DataEventHandler Updated;

        public event DataCancelEventHandler Deleting;
        public event DataEventHandler Deleted;

        public event DataCancelEventHandler Inserting;
        public event DataEventHandler Inserted;


        public event DataCancelEventHandler Saving;
        public event DataListEventHandler Saved;


        #endregion


        protected void OnSelected(IList dataList)
        {
            if (Selected != null) { Selected(this, new DataListEventArgs(dataList)); }
        }

        protected bool OnSelecting(IList dataList)
        {
            if (Selecting == null) { return false; }

            DataListCancelEventArgs e = new DataListCancelEventArgs(dataList);

            Selecting(this, e);

            return e.Cancel;

        }


        int m_CurPage;

        public int CurPage
        {
            get { return m_CurPage; }
            set { m_CurPage = value; }
        }

        public virtual IList LoadPage(int page)
        {
            throw new Exception("未实现");
        }

        public virtual IList Select()
        {
            throw new Exception("未实现");
        }

        public virtual int Update()
        {
            throw new Exception("未实现");
        }

        public virtual int Insert()
        {
            throw new Exception("未实现");
        }

        public virtual int Delete()
        {

            throw new Exception("未实现");
        }

        public bool ContainsListCollection
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IList GetList()
        {

            throw new Exception("未实现");
        }

    }
}
