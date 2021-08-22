using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.BizCoder
{

    /// <summary>
    /// 业务编码定义
    /// </summary>
    public class BizCodeDefine
    {
        public BizCodeDefine()
        {
            this.CodeMode = BizCodeMode.Auto;
            this.NumAdd = 1;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 编码名称
        /// </summary>
        public string TCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 起始值
        /// </summary>
        public int NumStart { get; set; }

        /// <summary>
        /// 终止值
        /// </summary>
        public int NumEnd { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public int NumCur { get; set; }

        /// <summary>
        /// 递增值
        /// </summary>
        public int NumAdd { get; set; }

        /// <summary>
        /// 编码前缀
        /// </summary>
        public string CodePrefix { get; set; }

        /// <summary>
        /// 编码后缀
        /// </summary>
        public string CodeSuffix { get; set; }

        /// <summary>
        /// 格式化内容
        /// </summary>
        public string TFormat { get; set; }

        /// <summary>
        /// 编码模式
        /// </summary>
        public BizCodeMode CodeMode { get; set; }



    }
}
