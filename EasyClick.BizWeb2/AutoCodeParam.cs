using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using App.BizCommon;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 自动编码参数
    /// </summary>
    [Description("服务器业务参数")]
    public class AutoCodeParam:EasyClick.Web.Mini2.Param
    {

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BillType { get; set; } = "COMMON";

        /// <summary>
        /// 编码前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 分隔符
        /// </summary>
        public string SeparatorString { get; set; }

        /// <summary>
        /// 数字长度
        /// </summary>
        public int SeqLen { get; set; } = 4;

        public BillCodeType BillCodeType { get; set; } = BillCodeType.Day;

        public override object Evaluate(HttpContext context, Control control)
        {
            return BillIdentityMgr.NewCode(this.BillCodeType, this.BillType, this.Prefix, this.SeparatorString, this.SeqLen);
        }

    }
}
