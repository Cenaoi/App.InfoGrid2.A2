using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Model.SecModels
{


    /// <summary>
    /// 用户方法字典档
    /// </summary>
    public class SecUserFunDict : Dictionary<int, SEC_USER_FUN>
    {

    }

    /// <summary>
    /// 角色方法字典档
    /// </summary>
    public class SecRoleFunDict : Dictionary<int, SEC_ROLE_FUN>
    {

    }


    /// <summary>
    /// 用户权限集
    /// </summary>
    public class UserSecritySet
    {
        #region 菜单权限控制

        /// <summary>
        /// 用户界面函数
        /// </summary>
        SecUserFunDict m_UserFuns;

        /// <summary>
        /// 角色界面函数
        /// </summary>
        SecRoleFunDict m_RoleFuns;

        /// <summary>
        /// 权限模式.
        /// 0-没有权限，1-根据角色权限，2-根据用户权限，3-角色和用户
        /// </summary>
        int m_ModeId = 0;

        /// <summary>
        /// 权限模式.
        /// 0-没有权限，1-根据角色权限，2-根据用户权限，3-角色和用户
        /// </summary>
        public int ModeId
        {
            get { return m_ModeId; }
            set { m_ModeId = value; }
        }

        /// <summary>
        /// 用户界面函数
        /// </summary>
        public SecUserFunDict UserFuns
        {
            get { return m_UserFuns; }
            set { m_UserFuns = value; }
        }

        /// <summary>
        /// 角色界面函数
        /// </summary>
        public SecRoleFunDict RoleFuns
        {
            get { return m_RoleFuns; }
            set { m_RoleFuns = value; }
        }

        #endregion


        #region 数据访问权限，和界面权限角色

        /// <summary>
        /// ARR 权限控制模式.0-没有，1-角色，2-用户
        /// </summary>
        int m_ArrModeId = 0;

        /// <summary>
        /// 可看的用户集
        /// </summary>
        string[] m_ArrUserCode;

        /// <summary>
        /// 角色集
        /// </summary>
        string[] m_ArrRoleCode;
        
        /// <summary>
        /// 结构代码集
        /// </summary>
        string[] m_ArrStructCode;

        /// <summary>
        /// 目录代码集
        /// </summary>
        string[] m_ArrCatalogCode;

        string m_ArrCatalogCodeString = null;


        /// <summary>
        /// ARR 权限控制模式.0-没有，1-角色，2-用户
        /// </summary>
        public int ArrModeId
        {
            get { return m_ArrModeId; }
            set { m_ArrModeId = value; }
        }

        /// <summary>
        /// 角色的权限代码集
        /// </summary>
        public string[] ArrRoleCode
        {
            get { return m_ArrRoleCode; }
            set { m_ArrRoleCode = value; }
        }

        /// <summary>
        /// 权限结构集
        /// </summary>
        public string[] ArrStructCode
        {
            get { return m_ArrStructCode; }
            set { m_ArrStructCode = value; }
        }

        /// <summary>
        /// 用户的可访问的权限代码集
        /// </summary>
        public string[] ArrUserCode
        {
            get { return m_ArrUserCode; }
            set { m_ArrUserCode = value; }
        }

        /// <summary>
        /// 用户可访问的类型代码
        /// </summary>
        public string[] ArrCatalogCode
        {
            get { return m_ArrCatalogCode; }
            set { m_ArrCatalogCode = value; }
        }


        /// <summary>
        /// 用户可访问的类型代码
        /// </summary>
        /// <returns></returns>
        public string ArrCatalogCodeString()
        {
            if(m_ArrCatalogCode == null || m_ArrCatalogCode.Length == 0)
            {
                return "NULL";
            }

            if (m_ArrCatalogCodeString == null)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < m_ArrCatalogCode.Length; i++)
                {
                    if (i > 0) { sb.Append(","); }

                    sb.Append("'").Append(m_ArrCatalogCode[i]).Append("'");
                }

                m_ArrCatalogCodeString = sb.ToString();
            }

            return m_ArrCatalogCodeString;
        }

        #endregion


    }

}
