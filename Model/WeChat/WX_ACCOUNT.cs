using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.WeChat
{
    /// <summary>
    /// 微信账号
    /// </summary>
    [DBTable("WX_ACCOUNT")]
    [Description("微信账号")]
    [Serializable]
    public class WX_ACCOUNT : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 WX_ACCOUNT_ID { get; set; }


        /// <summary>
        /// 自定义主键
        /// </summary>
        [DBField]
        [DisplayName("自定义主键")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_W_CODE { get; set; }


        /// <summary>
        /// 上级推荐人自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("上级推荐人自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARENT_W_CODE_1 { get; set; }


        /// <summary>
        /// 微信昵称
        /// </summary>
        [DBField]
        [DisplayName("微信昵称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_NICKNAME { get; set; }


        /// <summary>
        /// 微信唯一标示码
        /// </summary>
        [DBField]
        [DisplayName("微信唯一标示码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String W_OPENID { get; set; }


        /// <summary>
        /// 微信头像地址
        /// </summary>
        [DBField]
        [DisplayName("微信头像地址")]
        [LMValidate(maxLen = 200)]
        [UIField(visible = true)]
        public String HEAD_IMG_URL { get; set; }


        /// <summary>
        /// 微信里面填写的地址
        /// </summary>
        [DBField]
        [DisplayName("微信里面填写的地址")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String W_ADDRESS { get; set; }

        /// <summary>
        /// 是否激活（默认激活）
        /// </summary>
        [DBField]
        [DisplayName("是否激活（默认激活）")]
        [UIField(visible = true)]
        public Boolean IS_ENABLE { get; set; }


        /// <summary>
        /// 性别（男或女）
        /// </summary>
        [DBField]
        [DisplayName("性别（男或女）")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String SEX { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String REMARKS { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

        /// <summary>
        ///  地理位置纬度
        /// </summary>
        [DBField]
        [DisplayName(" 地理位置纬度")]
        [UIField(visible = true)]
        public Decimal W_LATITUDE { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        [DBField]
        [DisplayName("地理位置经度")]
        [UIField(visible = true)]
        public Decimal W_LONGITUDE { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        [DBField]
        [DisplayName("地理位置精度")]
        [UIField(visible = true)]
        public Decimal W_PRECISION { get; set; }

        /// <summary>
        /// 领取红包总数
        /// </summary>
        [DBField]
        [DisplayName("领取红包总数")]
        [UIField(visible = true)]
        public Int32 COL_1 { get; set; }

        /// <summary>
        /// 红包收入
        /// </summary>
        [DBField]
        [DisplayName("红包收入")]
        [UIField(visible = true)]
        public Decimal COL_2 { get; set; }

        /// <summary>
        /// 推荐的用户数
        /// </summary>
        [DBField]
        [DisplayName("推荐的用户数")]
        [UIField(visible = true)]
        public Int32 COL_3 { get; set; }

        /// <summary>
        /// 推荐用户收入
        /// </summary>
        [DBField]
        [DisplayName("推荐用户收入")]
        [UIField(visible = true)]
        public Decimal COL_4 { get; set; }

        /// <summary>
        /// 推荐的商家数
        /// </summary>
        [DBField]
        [DisplayName("推荐的商家数")]
        [UIField(visible = true)]
        public Int32 COL_5 { get; set; }

        /// <summary>
        /// 推荐商家的收入
        /// </summary>
        [DBField]
        [DisplayName("推荐商家的收入")]
        [UIField(visible = true)]
        public Decimal COL_6 { get; set; }

        /// <summary>
        /// 关注的公众号数
        /// </summary>
        [DBField]
        [DisplayName("关注的公众号数")]
        [UIField(visible = true)]
        public Int32 COL_7 { get; set; }

        /// <summary>
        /// 关注公众号的收入
        /// </summary>
        [DBField]
        [DisplayName("关注公众号的收入")]
        [UIField(visible = true)]
        public Decimal COL_8 { get; set; }

        /// <summary>
        /// 浏览的消息数
        /// </summary>
        [DBField]
        [DisplayName("浏览的消息数")]
        [UIField(visible = true)]
        public Int32 COL_9 { get; set; }

        /// <summary>
        /// 浏览消息的收入
        /// </summary>
        [DBField]
        [DisplayName("浏览消息的收入")]
        [UIField(visible = true)]
        public Decimal COL_10 { get; set; }

        /// <summary>
        /// 到店签到数量
        /// </summary>
        [DBField]
        [DisplayName("到店签到数量")]
        [UIField(visible = true)]
        public Int32 COL_11 { get; set; }

        /// <summary>
        /// 到店签到的收入
        /// </summary>
        [DBField]
        [DisplayName("到店签到的收入")]
        [UIField(visible = true)]
        public Decimal COL_12 { get; set; }

        /// <summary>
        /// 累计总收入
        /// </summary>
        [DBField]
        [DisplayName("累计总收入")]
        [UIField(visible = true)]
        public Decimal COL_13 { get; set; }

        /// <summary>
        /// 累计总提现
        /// </summary>
        [DBField]
        [DisplayName("累计总提现")]
        [UIField(visible = true)]
        public Decimal COL_14 { get; set; }

        /// <summary>
        /// 已申请提现金额
        /// </summary>
        [DBField]
        [DisplayName("已申请提现金额")]
        [UIField(visible = true)]
        public Decimal COL_15 { get; set; }

        /// <summary>
        /// 未提现金额
        /// </summary>
        [DBField]
        [DisplayName("未提现金额")]
        [UIField(visible = true)]
        public Decimal COL_16 { get; set; }


        /// <summary>
        /// 专属二维码地址
        /// </summary>
        [DBField]
        [DisplayName("专属二维码地址")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String COL_17 { get; set; }

        /// <summary>
        /// 专属二维码过期时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("专属二维码过期时间")]
        [UIField(visible = true)]
        public DateTime COL_18 { get; set; }

        /// <summary>
        /// 是否是商家
        /// </summary>
        [DBField]
        [DisplayName("是否是商家")]
        [UIField(visible = true)]
        public Boolean COL_19 { get; set; }


        /// <summary>
        /// 商家电话
        /// </summary>
        [DBField]
        [DisplayName("商家电话")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COL_20 { get; set; }

        /// <summary>
        /// 商家账户余额
        /// </summary>
        [DBField]
        [DisplayName("商家账户余额")]
        [UIField(visible = true)]
        public Decimal COL_21 { get; set; }

        /// <summary>
        /// 商家的系统余额
        /// </summary>
        [DBField]
        [DisplayName("商家的系统余额")]
        [UIField(visible = true)]
        public Decimal COL_22 { get; set; }


        /// <summary>
        /// 商家姓名
        /// </summary>
        [DBField]
        [DisplayName("商家姓名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String COL_23 { get; set; }

        /// <summary>
        /// 商家地理位置的经度值
        /// </summary>
        [DBField]
        [DisplayName("商家地理位置的经度值")]
        [UIField(visible = true)]
        public Decimal COL_24 { get; set; }

        /// <summary>
        /// 商家地理位置纬度
        /// </summary>
        [DBField]
        [DisplayName("商家地理位置纬度")]
        [UIField(visible = true)]
        public Decimal COL_25 { get; set; }

        /// <summary>
        /// 商家地理位置精度
        /// </summary>
        [DBField]
        [DisplayName("商家地理位置精度")]
        [UIField(visible = true)]
        public Decimal COL_26 { get; set; }



        /// <summary>
        /// 商家真实地址
        /// </summary>
        [DBField]
        [DisplayName("商家真实地址")]
        [UIField(visible = true)]
        public string COL_27 { get; set; }


        /// <summary>
        /// 是否是永久二维码
        /// </summary>
        [DBField]
        [DisplayName("是否是永久二维码")]
        [UIField(visible = true)]
        public Boolean IS_FOREVER_QRCODE { get; set; }


        /// <summary>
        /// 登录账号
        /// </summary>
        [DBField]
        [DisplayName("登录账号")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LOGIN_NAME { get; set; }


        /// <summary>
        /// 登录密码
        /// </summary>
        [DBField]
        [DisplayName("登录密码")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String LOGIN_PASS { get; set; }


    }


}
