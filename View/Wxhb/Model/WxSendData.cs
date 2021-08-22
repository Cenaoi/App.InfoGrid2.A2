using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace App.InfoGrid2.Wxhb.Model
{
    /// <summary>
    /// 微信推送过来的数据对象
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class WxSendData
    {

        /// <summary>
        /// 开发者微信号
        /// </summary>
        [XmlElement]
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        [XmlElement]
        public string FromUserName { get; set; }


        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        [XmlElement]
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [XmlElement]
        public string MsgType { get; set; }


        /// <summary>
        /// 文本消息内容
        /// </summary>
        [XmlElement]
        public string Content { get; set; }


        /// <summary>
        /// 事件类型
        /// </summary>
        [XmlElement]
        public string Event { get; set; }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        [XmlElement]
        public decimal Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        [XmlElement]
        public decimal Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        [XmlElement]
        public decimal Precision { get; set; }


        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        [XmlElement]
        public Int64 MsgId { get; set; }


        /// <summary>
        /// 加密的字符串
        /// </summary>
        [XmlElement]
        public string Encrypt { get; set; }

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }


        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }






    }
}