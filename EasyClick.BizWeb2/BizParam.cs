using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using EC5.SystemBoard;
using System.Web.UI;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 服务器业务参数
    /// </summary>
    [Description("服务器业务参数")]
    public class BizParam:EasyClick.Web.Mini2.Param
    {
        string m_BizField;

        /// <summary>
        /// 参数名称
        /// </summary>
        [Description("服务器参数名")]
        public string BizField
        {
            get { return m_BizField; }
            set { m_BizField = value; }
        }

        public override object Evaluate(HttpContext context, Control control)
        {
            object value = null;

            string bizField = m_BizField.ToUpper();

            switch (bizField)
            {
                case "USER_IDENTTIY": value = BizServer.UserIdentity; break;    //用户数据库 id
                case "LOGIN_ID": value = BizServer.LoginID; break;              //登陆名
                case "LOGIN_NAME": value = BizServer.LoginName; break;          //真实姓名
                case "FIRST_ROLE_NAME": value = BizServer.FirstRoleName; break; //第一个角色名称
                case "ROLE_ALL": value = BizServer.RoleAll; break; //全部角色名称
                case "HOST_IP": value = BizServer.HostIP; break;                //登陆 IP
                case "USER_CODE": value = BizServer.UserCode; break;            //用户编码。作为系统业务编码
                case "ORG_CODE": value = BizServer.OrgCode; break;              //组织结构编码
                case "OP_COMP_CODE": value = BizServer.OpCompCode; break;
                default:
                    throw new Exception("参数名 \"" + m_BizField + "\" 不存在.");

            }

            return value;

        }

    }
}
