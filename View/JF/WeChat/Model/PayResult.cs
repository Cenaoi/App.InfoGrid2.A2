using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace App.InfoGrid2.JF.WeChat.Model
{
    /// <summary>
    /// 企业账号付款给微信用户返回的结果对象
    /// </summary>
    [Serializable]
    public class PayResult
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
        /// 商户公众号ID
        /// </summary>
        [XmlElement]
        public string mch_appid { get; set; }


        /// <summary>
        /// 商户号
        /// </summary>
        [XmlElement]
        public string mchid { get; set; }

        /// <summary>
        /// 错误编码说明
        /// </summary>
        [XmlElement]
        public string err_code_des { get; set; }


        /// <summary>
        /// 随机字符串
        /// </summary>
        [XmlElement]
        public string nonce_str { get; set; }


        /// <summary>
        /// 商户订单号
        /// </summary>
        [XmlElement]
        public string partner_trade_no { get; set; }


        /// <summary>
        /// 设备信息
        /// </summary>
        [XmlElement]
        public string device_info { get; set; }

        /// <summary>
        /// 结果编码
        /// </summary>
        [XmlElement]
        public string result_code { get; set; }


        /// <summary>
        /// 微信订单号
        /// </summary>
        [XmlElement]
        public string payment_no { get; set; }

        /// <summary>
        /// 假的微信支付成功时间 是字符串 psyment_time_date 才是真正的时间
        /// </summary>
        [XmlElement]
        public string payment_time
        {
            get
            {
                return payment_time_date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {

                if (string.IsNullOrWhiteSpace(value))
                {
                    payment_time_date = new DateTime(2000, 1, 1);
                }
                else
                {
                    payment_time_date = DateTime.Parse(value);
                }
            }

        }

        /// <summary>
        /// 真的微信支付成功时间 
        /// </summary>
        public DateTime payment_time_date { get; set; }


        /// <summary>
        /// 错误编码
        /// </summary>
        [XmlElement]
        public string err_code { get; set; }
    }
}