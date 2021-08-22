using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;

namespace EasyClick.BizWeb2
{

    /// <summary>
    /// 配合 Store 数据仓库使用.
    /// </summary>
    public class TSqlWhereParam:EasyClick.Web.Mini2.Param
    {
        public TSqlWhereParam()
        {
            this.ParamMode = Web.Mini2.StoreParamMode.TSql;
        }

        public TSqlWhereParam(string tSqlWhere)
        {
            this.ParamMode = Web.Mini2.StoreParamMode.TSql;

            m_Where = tSqlWhere;
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
            string whereTxt = m_Where.Replace("{$P.LOGIN_NAME}", BizServer.LoginID)
                .Replace("{$P.LOGIN_NAME}",BizServer.LoginName)
                .Replace("{$P.USER_IDENTITY}", BizServer.UserIdentity.ToString())
                .Replace("{$P.HOST_IP}", BizServer.HostIP);
            
            return whereTxt;
        }
    }
}
