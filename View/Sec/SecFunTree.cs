using System;
using System.Collections.Generic;
using System.Web;

namespace App.Sec
{
    /// <summary>
    /// 权限树
    /// </summary>
    public class SecFunTree
    {
        public string SecType { get; set; }


        public int RowID { get; set; }

        List<SecModule> m_Modules = new List<SecModule>();

        public List<SecModule> Modules
        {
            get
            {
                if (m_Modules == null)
                {
                    m_Modules = new List<SecModule>();
                }

                return m_Modules;
            }
        }

    }

    /// <summary>
    /// 权限模块
    /// </summary>
    public class SecModule
    {
        public bool Checked { get; set; }

        List<SecFun> m_Funs = new List<SecFun>();

        public List<SecFun> Funs
        {
            get
            {
                if (m_Funs == null)
                {
                    m_Funs = new List<SecFun>();
                }
                return m_Funs;
            }
        }
    }

    /// <summary>
    /// 权限方法
    /// </summary>
    public class SecFun
    {
        public int Pk { get; set; }

        public bool Checked { get; set; }
    }
}