using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data">客户端回调的数据</param>
    public delegate void CallbackEventHandler(object sender, string data);

    /// <summary>
    /// 回调的数据类型
    /// </summary>
    public enum CallbackDataType
    {
        /// <summary>
        /// 全部数据提交的模式
        /// </summary>
        Full,
        /// <summary>
        /// 简单模式
        /// </summary>
        Sample
    }

}
