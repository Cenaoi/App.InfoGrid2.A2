using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.ComponentModel;

namespace App.BizCommon.Models
{
    /// <summary>
    /// 业务单号
    /// </summary>
    [DBTable("C_BILL_IDENTITY")]
    public class C_BILL_IDENTITY:LightModel
    {
        #region 字段

        /// <summary>
        /// 
        /// </summary>
        int m_C_BILL_IDENTITY_ID = 0;
        /// <summary>
        /// 
        /// </summary>
        string m_BILL_TYPE_ID = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        int m_BILL_YEAR = 0;
        /// <summary>
        /// 
        /// </summary>
        int m_BILL_MONTH = 0;
        /// <summary>
        /// 
        /// </summary>
        int m_BILL_DAY = 0;
        /// <summary>
        /// 
        /// </summary>
        int m_BILL_IDENTITY = 0;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [Description("")]
        public int C_BILL_IDENTITY_ID
        {
            get { return m_C_BILL_IDENTITY_ID; }
            set { m_C_BILL_IDENTITY_ID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        public string BILL_TYPE_ID
        {
            get { return m_BILL_TYPE_ID; }
            set { m_BILL_TYPE_ID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        public int BILL_YEAR
        {
            get { return m_BILL_YEAR; }
            set { m_BILL_YEAR = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        public int BILL_MONTH
        {
            get { return m_BILL_MONTH; }
            set { m_BILL_MONTH = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        public int BILL_DAY
        {
            get { return m_BILL_DAY; }
            set { m_BILL_DAY = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [Description("")]
        public int BILL_IDENTITY
        {
            get { return m_BILL_IDENTITY; }
            set { m_BILL_IDENTITY = value; }
        }

    }
}
