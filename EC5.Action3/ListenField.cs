using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{


    /// <summary>
    /// 监听字段(有可能作废, 或改为作为索引使用)
    /// </summary>
    public class ListenField:CodeIndexItem
    {
        /// <summary>
        /// 对应的表名
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 对应的表名称
        /// </summary>
        public string TableText { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        public string Name
        {
            get { return this.Code; }
            set { this.Code = value; }
        }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
