using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{



    /// <summary>
    /// 角色的权限设定
    /// </summary>
    [DBTable("SEC_ROLE_FUN")]
    [Description("角色的权限设定")]
    [Serializable]
    public class SEC_ROLE_FUN : LightModel
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
        public Int32 SEC_ROLE_FUN_ID { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [DBField]
        [DisplayName("权限ID")]
        [UIField(visible = true)]
        public Int32 SEC_FUN_DEF_ID { get; set; }


        /// <summary>
        /// 权限类型.0-目录;2-模块,4-方法
        /// </summary>
        [DBField]
        [DisplayName("权限类型.0-目录;2-模块,4-方法")]
        public int FUN_TYPE_ID { get; set; }


        /// <summary>
        /// 角色ID
        /// </summary>
        [DBField]
        [DisplayName("角色ID")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public int  SEC_ROLE_ID { get; set; }

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
        /// 方法的 ID 集合
        /// </summary>
        [DBField]
        public string FUN_ARR_CHILD_ID { get; set; }


        /// <summary>
        /// 复选框的状态.0-未选中,1-选中,2-不明确
        /// </summary>
        [DBField]
        [DisplayName("复选框的状态..0-未选中,1-选中,2-不明确")]
        public int CHECK_STATE_ID { get; set; }

    }
}
