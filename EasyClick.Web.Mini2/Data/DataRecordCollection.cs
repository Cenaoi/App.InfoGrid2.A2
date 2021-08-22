using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// 记录集合
    /// </summary>
    public class DataRecordCollection:List<DataRecord>
    {
        SortedDictionary<string, DataRecord> m_Indexs;


        /// <summary>
        /// 输出 Json 格式
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            ToJson(sb);

            return sb.ToString();
        }

        /// <summary>
        /// 输出 Json 格式
        /// </summary>
        /// <param name="sb"></param>
        public void ToJson(StringBuilder sb)
        {
            int n = 0;

            sb.Append("[");
            
            foreach (DataRecord dr in this)
            {
                if(n ++ > 0) { sb.Append(", "); }

                dr.Fields.ToJson(sb);
            }
            
            sb.Append("]");
        }



        /// <summary>
        /// 获取全部 ID 集合.默认类型都是字符串
        /// </summary>
        /// <returns></returns>
        public string[] GetIds()
        {
            int i = 0;

            string[] ids = new string[this.Count];

            foreach (DataRecord dr in this)
            {
                ids[i++] = dr.Id;
            }

            return ids;
        }


        /// <summary>
        /// 获取全部 ID 集合
        /// </summary>
        /// <typeparam name="ValueT"></typeparam>
        /// <returns></returns>
        public ValueT[] GetIds<ValueT>()
        {
            int i = 0;

            Type valueT = typeof(ValueT);
            ValueT[] ids = new ValueT[this.Count];

            foreach (DataRecord dr in this)
            {
                ids[i++] = (ValueT)Convert.ChangeType(dr.Id, valueT);
            }

            return ids;
        }



        /// <summary>
        /// 按 id 查找记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRecord FindById(string id)
        {
            DataRecord rec = null;

            if (m_Indexs == null)
            {
                m_Indexs = new SortedDictionary<string, DataRecord>();
            }

            if (m_Indexs.Count != base.Count)
            {
                m_Indexs.Clear();

                foreach (DataRecord item in this)
                {
                    if (!m_Indexs.ContainsKey(item.Id))
                    {
                        m_Indexs.Add(item.Id, item);
                    }
                }
            }

            if (m_Indexs.TryGetValue(id, out rec))
            {
                return rec;
            }

            return null;
        }


    }
}
