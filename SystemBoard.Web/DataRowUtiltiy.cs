using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace EC5.SystemBoard.Web
{
    class DataRowUtiltiy
    {
    }

    [Obsolete]
    public class DataRowExpand
    {
        DataRow m_Row;

        public DataRowExpand()
        {
        }

        public DataRowExpand(DataRow row)
        {
            m_Row = row;
        }

        public DataRow SourceRow
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public ValueT To<ValueT>(string colName)
        {
            return m_Row.IsNull(colName) ? default(ValueT) : (ValueT)Convert.ChangeType(m_Row[colName], typeof(ValueT));
        }

        public int ToInt(string colName)
        {
            return m_Row.IsNull(colName) ? 0 : Convert.ToInt32(m_Row[colName]);
        }

        public decimal ToDecimal(string colName)
        {
            return m_Row.IsNull(colName) ? 0 : Convert.ToDecimal(m_Row[colName]);
        }

        public string ToStr(string colName)
        {
            return m_Row.IsNull(colName) ? string.Empty : Convert.ToString(m_Row[colName]);
        }

        public DateTime ToDateTime(string colName)
        {
            return m_Row.IsNull(colName) ? DateTime.Now : Convert.ToDateTime(m_Row[colName]);
        }

        public bool ToBool(string colName)
        {
            return m_Row.IsNull(colName) ? false : Convert.ToBoolean(m_Row[colName]);
        }


    }
}
