using System;
using System.Collections.Generic;
using System.Text;
using EC5.SystemBoard;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 业务服务器参数或操作
    /// </summary>
    public static class BizServer
    {
        /// <summary>
        /// 用户IP
        /// </summary>
        public static string HostIP
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;


                if (user != null)
                {
                    return user.HostIP;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 是否为平板模式
        /// </summary>
        /// <returns></returns>
        public static bool IsTouch()
        {
            EcContext context = EcContext.Current;
            EcUserState user = context.User;

            string screen_type = EC5.SystemBoard.SysBoardManager.CurrentApp.AppSettings["screen_type"];

            if (screen_type == "touch")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 首个角色名称
        /// </summary>
        public static string FirstRoleName
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;


                if (user != null)
                {
                    return user.FirstRoleName;
                }

                return string.Empty;
            }
        }


        /// <summary>
        /// 所有角色的集合
        /// </summary>
        public static string RoleAll
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;


                if (user != null)
                {
                    return null;
                }

                return user.Roles.ToString();
            }
        }


        /// <summary>
        /// 组织机构编码
        /// </summary>
        public static string OrgCode
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;

                if (user == null)
                {
                    return null;
                }

                return user.ExpandPropertys["ORG_CODE"];
            }
        }

        /// <summary>
        /// 当前操作的公司代码...
        /// 作为同个账号,可以切换操作不同公司机构
        /// </summary>
        public static string OpCompCode
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;

                if (user == null)
                {
                    return null;
                }

                return user.ExpandPropertys["OP_COMP_CODE"];
            }
        }
            



        /// <summary>
        /// 用户编码。
        /// 作为系统用的
        /// </summary>
        public static string UserCode
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;

                if (user == null)
                {
                    return null;
                }
                    
                string userCode = user.ExpandPropertys["USER_CODE"];


                return userCode;
            }
        }




        /// <summary>
        /// 登陆用户的记录主键pk值
        /// </summary>
        /// <returns></returns>
        public static int UserIdentity
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;


                if (user != null)
                {
                    return user.Identity;
                }

                return -1;
            }
        }



        /// <summary>
        /// 登陆的账号
        /// </summary>
        /// <returns></returns>
        public static string LoginID
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;

                if (user != null)
                {
                    return user.LoginID;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 用户名称,一般存放真实姓名之类的
        /// </summary>
        /// <returns></returns>
        public static string LoginName
        {
            get
            {
                EcContext context = EcContext.Current;
                EcUserState user = context.User;

                if (user != null)
                {
                    return user.LoginName;
                }

                return string.Empty;
            }
        }
    }
}
