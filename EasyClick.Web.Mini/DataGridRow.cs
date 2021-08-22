using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini
{
    public class DataGridRow
    {
        DataGrid m_Owner;

        int m_RowGuid;

        public DataGrid Owner
        {
            get { return m_Owner; }
        }

        public DataGridRow(DataGrid owner,int rowGuid)
        {
            m_Owner = owner;
            m_RowGuid = rowGuid;
        }

        /// <summary>
        /// 获取单元格 CELL 值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetValue<T>( string id) where T : struct
        {
            string key = string.Format("{0}_{1}${2}", m_Owner.ClientID, m_RowGuid, id);

            HttpContext content = HttpContext.Current;

            string value = content.Request.Form[key];

            if (value == null)
            {
                return default(T);
            }


            T valueT = (T)Convert.ChangeType(value, typeof(T));

            return valueT;
        }

        public int GetInt(string id)
        {
            return GetValue<int>(id);
        }

        public Int64 GetInt64(string id)
        {
            return GetValue<Int64>(id);
        }

        public decimal GetDecimal(string id)
        {
            return GetValue<decimal>(id);
        }

        public float GetFloat(string id)
        {
            return GetValue<float>(id);
        }

        public string GetString(string id)
        {
            string key = string.Format("{0}_{1}${2}", m_Owner.ClientID, m_RowGuid, id);

            HttpContext content = HttpContext.Current;

            string value = content.Request.Form[key];

            return value;
        }

        public double GetDouble(string id)
        {
            return GetValue<double>(id);
        }


    }
}
