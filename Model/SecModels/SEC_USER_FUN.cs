
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{

    /// <summary>
    /// 用户的权限设定
    /// </summary>
    [DBTable("SEC_USER_FUN")]
    [Description("用户的权限设定")]
    [Serializable]
    public class SEC_USER_FUN : LightModel
    {
        #region Fun 代码索引

        SortedList<string, bool> m_ArrChecekedCode;

        /// <summary>
        /// 被选中的代码集合
        /// </summary>
        public SortedList<string, bool> ArrCheckedCode
        {
            get { return m_ArrChecekedCode; }
            set { m_ArrChecekedCode = value; }
        }

        #endregion



        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey,DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 SEC_USER_FUN_ID { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [DBField]
        [DisplayName("权限ID")]
        [UIField(visible = true)]
        public Int32 SEC_FUN_DEF_ID { get; set; }

        /// <summary>
        /// 方法类型.0-目录,2-模块,4-方法
        /// </summary>
        [DBField]
        [DisplayName("方法类型.0-目录,2-模块,4-方法")]
        [UIField(visible = true)]
        public Int32 FUN_TYPE_ID { get; set; }

        /// <summary>
        /// 登录的用户编码
        /// </summary>
        [DBField]
        public string LOGIN_CODE { get; set; }

        /// <summary>
        /// 用户登陆的ID
        /// </summary>
        [DBField]
        [DisplayName("用户登陆的ID")]
        [UIField(visible = true)]
        public Int32 LOGIN_ID { get; set; }

        /// <summary>
        /// 是否可视
        /// </summary>
        [DBField]
        [DisplayName("是否可视")]
        [UIField(visible = true)]
        public Boolean VISIBLE { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [DBField]
        [DisplayName("是否激活")]
        [UIField(visible = true)]
        public Boolean ENABLE { get; set; }


        /// <summary>
        /// 子方法集合
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        [DisplayName("子方法集合")]
        [LMValidate(maxLen = 65535)]
        [UIField(visible = true)]
        public String FUN_ARR_CHILD_ID { get; set; }


        /// <summary>
        /// 针对复选框进行增加的
        /// </summary>
        [DBField]
        public int CHECK_STATE_ID { get; set; }

    }



}