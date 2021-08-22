using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.IG2.Core
{
    /// <summary>
    /// IG2 系统设计
    /// </summary>
    public class IG2Param
    {
        public static class PageMode
        {
            public const string MInJs2 = "MInJs2";
        }

        /// <summary>
        /// 系统角色
        /// </summary>
        public static class Role
        {
            /// <summary>
            /// 设计师角色
            /// </summary>
            public const string BUILDER = "BUILDER";

            /// <summary>
            /// 系统管理员角色
            /// </summary>
            public const string ADMIN = "ADMIN";

            /// <summary>
            /// /员工角色
            /// </summary>
            public const string EMPLOYEE = "EMPLOYEE";

            /// <summary>
            /// 其它人员
            /// </summary>
            public const string OTHER = "OTHER";

        }

    }
}
