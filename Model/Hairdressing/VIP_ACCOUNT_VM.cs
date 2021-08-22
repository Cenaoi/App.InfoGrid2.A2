using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Hairdressing
{
    /// <summary>
    /// 会员账号
    /// </summary>
    [DBTable("EC_WX_ACCOUNT")]
    [Description("会员账号")]
    [Serializable]
    public class VIP_ACCOUNT_VM : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 ROW_IDENTITY_ID { get; set; }




        /// <summary>
        /// 昵称
        /// </summary>
        [DBField]
        [DisplayName("昵称")]
        [UIField(visible = true)]
        public string WX_NICKNAME { get; set; }





        /// <summary>
        /// 邮箱
        /// </summary>
        [DBField]
        [DisplayName("邮箱")]
        [UIField(visible = true)]
        public string EMAIL { get; set; }



        /// <summary>
        /// 手机
        /// </summary>
        [DBField]
        [DisplayName("手机")]
        [UIField(visible = true)]
        public string PHONE { get; set; }



        /// <summary>
        /// 性别
        /// </summary>
        [DBField]
        [DisplayName("性别")]
        [UIField(visible = true)]
        public string SEX { get; set; }


        /// <summary>
        /// 生日
        /// </summary>
        [DBField]
        [DisplayName("生日")]
        [UIField(visible = true)]
        public string BIRTHDAY { get; set; }




    }
}
