using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数据仓库引擎管理
    /// </summary>
    public static class StoreEngineManager
    {
        /// <summary>
        /// 默认数据仓库类型
        /// </summary>
        static string m_DefaultStoreName;
        /// <summary>
        /// 默认数据仓库类型
        /// </summary>
        static Type m_DefaultStoreT;

        /// <summary>
        /// 数据仓库字典
        /// </summary>
        static SortedDictionary<string, Type> m_StoreList = new SortedDictionary<string, Type>();

        /// <summary>
        /// 默认数据仓库类型
        /// </summary>
        public static string DefaultStoreName
        {
            get { return m_DefaultStoreName; }
            set
            {
                m_DefaultStoreName = value;

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

        /// <summary>
        /// 添加数据仓库类型
        /// </summary>
        /// <param name="name">仓库名称</param>
        /// <param name="storeT">仓库类型</param>
        public static void AddStoreT(string name, Type storeT)
        {
            name = name.ToLower();

            m_StoreList.Add(name, storeT);

            if (!string.IsNullOrEmpty(m_DefaultStoreName) &&
                m_DefaultStoreName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                m_DefaultStoreT = storeT;
            }
        }

        /// <summary>
        /// 获取数据仓库类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetStoreT(string name)
        {
            name = name.ToLower();
            Type storeT;

            if (!m_StoreList.TryGetValue(name, out storeT))
            {
                throw new Exception(string.Format("实体引擎 “{0}”不存在。", name));
            }

            return storeT;
        }

        /// <summary>
        /// 清理数据仓库集合
        /// </summary>
        public static void Clear()
        {
            m_StoreList.Clear();
        }

        /// <summary>
        /// 获取数据仓库并实例化
        /// </summary>
        /// <param name="name">数据引擎名称</param>
        /// <returns></returns>
        public static IStoreEngine GetStoreEngine(string name)
        {
            name = name.ToLower();
            Type storeT;

            if (!m_StoreList.TryGetValue(name, out storeT))
            {
                throw new Exception(string.Format("实体引擎 “{0}”不存在。",name));
            }

            IStoreEngine store = (IStoreEngine)Activator.CreateInstance(storeT);

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
        public static IStoreEngine GetDefaultStoreEngine()
        {
            if (m_DefaultStoreT == null)
            {
                throw new Exception("默认是数据仓库类型不存在。错误原因可能是采用 Net 4.0 版本，改回 2.0 就正常。");
            }

            IStoreEngine store = (IStoreEngine)Activator.CreateInstance(m_DefaultStoreT);

            return store;
        }
    }
}
