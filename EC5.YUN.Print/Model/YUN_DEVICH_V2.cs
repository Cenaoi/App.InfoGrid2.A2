using ProtoBuf;
using System;

namespace EC5.YUN.Print.Model
{
    /// <summary>
    /// 序列化专用 设备列表
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class YUN_DEVICH_V2
    {


        /// <summary>
        /// 设备IP
        /// </summary>
        [ProtoMember(1)]
        public String DEV_IP { get; set; }


        /// <summary>
        /// 设备 MAC 地址
        /// </summary>
        [ProtoMember(2)]
        public String DEV_MAC { get; set; }


        /// <summary>
        /// 通道 SN 码
        /// </summary>
        [ProtoMember(3)]
        public String CH_SN { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        [ProtoMember(4)]
        public String DEV_NAME { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [ProtoMember(5)]
        public String REMARK { get; set; }

        /// <summary>
        /// 设备状态 2--在线 0--断线
        /// </summary>
        [ProtoMember(6)]
        public Int32 DEV_SID { get; set; }


        /// <summary>
        /// 缺纸  0--有纸  4--没纸
        /// </summary>
        [ProtoMember(7)]
        public Int32 PAGER_OUT { get; set; }

        /// <summary>
        /// 设备尺寸
        /// </summary>
        [ProtoMember(8)]
        public Int32 DEV_SIZE { get; set; }


        /// <summary>
        /// 设备类型
        /// </summary>
        [ProtoMember(9)]
        public String DEV_TYPE { get; set; }

        /// <summary>
        /// 打印内容
        /// </summary>
        [ProtoMember(10)]
        public String PRINT_CONTENT { get; set; }

        /// <summary>
        /// 二维码路径
        /// </summary>
        [ProtoMember(12)]
        public byte[] IMG_BYTE { get; set; }

    }
}
