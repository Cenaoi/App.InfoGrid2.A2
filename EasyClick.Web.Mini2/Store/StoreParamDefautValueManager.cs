using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    public static class StoreParamValueManager
    {
        static StoreParamValueCollection m_Items = new StoreParamValueCollection();

        public static void Clear()
        {
            m_Items.Clear();
        }

        public static void Add(string name, StoreParamExtend exDelegate)
        {
            m_Items.Add(name,exDelegate);
        }

        public static object Exec(string name,object[] paramList)
        {
            StoreParamValue pv = m_Items.GetItem(name);

            object result = null;

            try
            {
                result = pv.ExDelegate(paramList);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取“{0}”参数失败。", name), ex);
            }
            

            return result;
        }
    }

    class StoreParamValueCollection
    {
        SortedList<string, StoreParamValue> m_Items = new SortedList<string, StoreParamValue>();

        public void Clear()
        {
            m_Items.Clear();
        }

        public void Add(string name, StoreParamExtend exDelegate)
        {
            StoreParamValue pv = new StoreParamValue();
            pv.FunName = name;
            pv.ExDelegate = exDelegate;

            m_Items.Add(name.ToUpper(), pv);
        }

        public StoreParamValue GetItem(string name)
        {
            StoreParamValue pv = null;

            m_Items.TryGetValue(name.ToUpper(),out pv);

            return pv;
        }

    }

    /// <summary>
    /// 数据仓库默认值处理
    /// </summary>
    class StoreParamValue
    {
        public string FunName { get; set; }

        public StoreParamExtend ExDelegate { get; set; }


    }

    /// <summary>
    /// 数据仓库的实体函数扩展
    /// </summary>
    /// <returns></returns>
    public delegate object StoreParamExtend(params object[] paramList);

}
