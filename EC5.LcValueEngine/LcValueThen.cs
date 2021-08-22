using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LcValueEngine
{
    /// <summary>
    /// Then 
    /// </summary>
    public class LcValueThen
    {

        public const string IF_NULL = "IF_NULL";

        public const string EVERY = "EVERY";

        public int ID { get; set; }


        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string ValueTo { get; set; }

        /// <summary>
        /// 逻辑。默认 = 
        /// </summary>
        public LcValueThenLogic Logic { get; set; }

    }
}
