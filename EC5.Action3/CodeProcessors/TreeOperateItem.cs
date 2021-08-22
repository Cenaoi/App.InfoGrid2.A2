using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.CodeProcessors
{
    /// <summary>
    /// 操作对象
    /// </summary>
    public class TreeOperateItem
    {
        public TreeOperateItem()
        {

        }

        public TreeOperateItem(OperateTable operate, object opData)
        {
            this.Operate = operate;
            this.OperateData = opData;
        }


        /// <summary>
        /// 原对象(操作前的对象)
        /// </summary>
        public object SourceData { get; set; }

        /// <summary>
        /// 操作后的数据对象
        /// </summary>
        public object OperateData { get; set; }


        public OperateTable Operate { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public TimeSpan ExecTime { get; set; }

    }
}
