using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// Excel中间数据区类型
    /// </summary>
    public enum DataAreaType
    {
        /// <summary>
        /// 没有类型
        /// </summary>
        NONE,
        /// <summary>
        /// 列重复类型
        /// </summary>
        COLUMN_REPEAT,
        /// <summary>
        /// 多子表类型
        /// </summary>
        MORE_SUB_TABLE,
        /// <summary>
        ///  这是行合并有图片的，就是某一列的值相同行，都要合并在一起
        /// </summary>
        ROW_MERGE_IMG,

    }
}
