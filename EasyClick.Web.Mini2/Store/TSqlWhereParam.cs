using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// T-SQL 的 Where 子语句
    /// </summary>
    public class TSqlWhereParam : Param
    {
        public TSqlWhereParam()
        {
            this.ParamMode = Web.Mini2.StoreParamMode.TSql;
        }

        public TSqlWhereParam(string tWhere)
        {
            this.ParamMode = StoreParamMode.TSql;
            m_Where = tWhere;
        }

        string m_Where;

        /// <summary>
        /// T-SQL Where 预计
        /// </summary>
        [DefaultValue("")]
        public string Where
        {
            get { return m_Where; }
            set { m_Where = value; }
        }


        public override object Evaluate(HttpContext context, System.Web.UI.Control control)
        {
            return m_Where;
        }
    }
}
