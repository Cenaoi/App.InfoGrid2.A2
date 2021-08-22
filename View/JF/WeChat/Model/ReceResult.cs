using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace App.InfoGrid2.JF.WeChat.Model
{
    /// <summary>
    /// 微信用户付款给企业账号调用统一下单接口返回的对象
    /// </summary>
    [Serializable]
    public class ReceResult
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        [XmlElement]
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [XmlElement]
        public string return_msg { get; set; }
        /// <summary>
        /// 公众号id
        /// </summary>
        [XmlElement]
        public string appid { get; set; }
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        [XmlElement]
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        [XmlElement]
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement]
        public string sign { get; set; }
        /// <summary>
        /// 微信统一下单的订单号
        /// </summary>
        [XmlElement]
        public string prepay_id { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        [XmlElement]
        public string trade_type { get; set; }

        /// <summary>
        /// 按二维码下单生成的数据，要拿来生成二维码的
        /// </summary>
        [XmlElement]
        public string code_url { get; set; }



        /// <summary>
        /// 结果编码
        /// </summary>
        [XmlElement]
        public string result_code { get; set; }





    }
}