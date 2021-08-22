using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 延迟显示
    /// </summary>
    public interface IDelayRender
    {
        /// <summary>
        /// 是否延迟显示
        /// </summary>
        bool IsDelayRender { get; set; }
    }
}
