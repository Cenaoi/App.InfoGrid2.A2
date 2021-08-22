using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 拼接的 SQL 查询语句。这样切分，是为了照顾多数据分页的设计。
    /// </summary>
    public class StoreTSqlQuerty
    {
        bool m_Enabled = false;

        string m_Select;

        string m_Form;

        string m_Where;

        string m_OrderBy;

        /// <summary>
        /// 关联的主键字段
        /// </summary>
        string m_IdField;




        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabeld
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public string IdField
        {
            get { return m_IdField; }
            set { m_IdField = value; }
        }

        /// <summary>
        /// T-SQL 语句的 Select 子语句
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string Select
        {
            get { return m_Select; }
            set { m_Select = value; }
        }

        /// <summary>
        /// T-SQL 语句的 Form 子语句
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string Form
        {
            get { return m_Form; }
            set { m_Form = value; }
        }

        /// <summary>
        /// T-SQL 语句的 Where 子语句
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string Where
        {
            get { return m_Where; }
            set { m_Where = value; }
        }

        /// <summary>
        /// T-SQL 语句的 Order By 子语句
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string OrderBy
        {
            get { return m_OrderBy; }
            set { m_OrderBy = value; }
        }
    }
}
