using System;
using System.Collections.Generic;
using System.Text;
using EC5;
using System.ComponentModel;

namespace EasyClick.BizWeb
{
    [Serializable]
    public class MainWebSiteConfig
    {
        
        
        StringList m_Views;
        StringList m_Blls;
        StringList m_Models;

        /// <summary>
        /// 激活安全模块
        /// </summary>
        [DefaultValue(true)]
        public bool Sec_Page_Enabled { get; set; } = true;

        [DefaultValue(true)]
        public bool Sec_Fun_Enabled { get; set; } = true;



        /// <summary>
        /// 立刻验证注册
        /// </summary>
        [DefaultValue(false)]
        public bool IsRegisterNow { get; set; } = false;

        /// <summary>
        /// 自动创建数据表
        /// </summary>
        [DefaultValue(true)]
        public bool AutoCreateTable { get; set; } = true;

        /// <summary>
        /// 验证差异
        /// </summary>
        [DefaultValue(true)]
        public bool ValidDifferenceTableALL { get; set; } = true;


        /// <summary>
        /// 自动创建描述
        /// </summary>
        [DefaultValue(false)]
        public bool AutoCreateDesc { get; set; } = false;


        /// <summary>
        /// 自动创建主键
        /// </summary>
        [DefaultValue(true)]
        public bool AutoCreatePrimaryKey { get; set; } = true;

        public StringList Views
        {
            get
            {
                if (m_Views == null)
                {
                    m_Views = new StringList();
                }
                return m_Views;
            }
            set { m_Views = value; }
        }

        public StringList Blls
        {
            get
            {
                if (m_Blls == null)
                {
                    m_Blls = new StringList();
                }
                return m_Blls;
            }
            set { m_Blls = value; }
        }

        public StringList Models
        {
            get
            {
                if (m_Models == null)
                {
                    m_Models = new StringList();
                }

                return m_Models;
            }
            set { m_Models = value; }
        }


    }
}
