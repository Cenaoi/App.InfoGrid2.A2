using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Model.WeChat
{
    /// <summary>
    /// 支付成功回调传过来的参数对象
    /// </summary>
    [Serializable]
    public class CallbackResult
    {
        /// <summary>
        /// 公众号id
        /// </summary>
        [XmlElement]
        public string appid { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        [XmlElement]
        public string bank_type { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        [XmlElement]
        public int cash_fee { get; set; }


        /// <summary>
        /// 货币种类
        /// </summary>
        [XmlElement]
        public string fee_type { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        [XmlElement]
        public string is_subscribe { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        [XmlElement]
        public string mch_id { get; set; }


        /// <summary>
        /// 随机字符串
        /// </summary>
        [XmlElement]
        public string nonce_str { get; set; }


        /// <summary>
        /// 用户标识
        /// </summary>
        [XmlElement]
        public string openid { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        [XmlElement]
        public string out_trade_no { get; set; }

        /// <summary>
        /// 结果编码
        /// </summary>
        [XmlElement]
        public string result_code { get; set; }

        /// <summary>
        /// 返回编码
        /// </summary>
        [XmlElement]
        public string return_code { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement]
        public string sign { get; set; }


        /// <summary>
        /// 支付完成时间
        /// </summary>
        [XmlElement]
        public string time_end { get; set; }

        /// <summary>
        /// 订单金额  分 单位
        /// </summary>
        [XmlElement]
        public int total_fee { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        [XmlElement]
        public string trade_type { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        [XmlElement]
        public string transaction_id { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [XmlElement]
        public string return_msg { get; set; }


        /// <summary>
        /// 错误代码
        /// </summary>
        [XmlElement]
        public string err_code { get; set; }


        /// <summary>
        /// 错误代码描述
        /// </summary>
        [XmlElement]
        public string err_code_des { get; set; }





    }
}
