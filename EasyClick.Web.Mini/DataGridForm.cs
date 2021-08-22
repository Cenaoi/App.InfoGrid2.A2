using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// Form 表格
    /// </summary>
    [DisplayName("Form 表格")]
    public class DataGridForm : DataGrid
    {
        /// <summary>
        /// 数据主键
        /// </summary>
        string m_DataKeys;


        /// <summary>
        /// 锁行标记字段名
        /// </summary>
        string m_LockedKey = "_MODEL_LOCKED";

        bool m_ReadOnly = false;


        /// <summary>
        /// 锁行标记字段名
        /// </summary>
        [DefaultValue("_MODEL_LOCKED")]
        [Description("锁行标记字段名")]
        public string LockedKey
        {
            get { return m_LockedKey; }
            set { m_LockedKey = value; }
        }

        /// <summary>
        /// 数据主键。字段1,字段2,...
        /// </summary>
        [Description("数据主键。字段1,字段2,...")]
        public string DataKeys
        {
            get { return m_DataKeys; }
            set { m_DataKeys = value; }
        }



    }
}
