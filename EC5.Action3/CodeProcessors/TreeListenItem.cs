using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 监听项目
    /// </summary>
    public class TreeListenItem
    {

        /// <summary>
        /// 触发源数据
        /// </summary>
        public object SourceData { get; set; }


        /// <summary>
        /// 监听
        /// </summary>
        public ListenTable Listen { get; set; }

        /// <summary>
        /// 监听条件成立（默认成立true）
        /// </summary>
        [DefaultValue(true)]
        public bool IsConditionsHold { get; set; } = true;

        
    }
}
