using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 子表参数
    /// </summary>
   public class SubTableParam
    {


        public SubTableParam() { }

        /// <summary>
        /// 有参构造参数
        /// </summary>
        /// <param name="firstRowIndex">开始行索引</param>
        /// <param name="lastRowIndex">结束行索引</param>
        /// <param name="isTotal">是否合计 默认不合计</param>
        public SubTableParam(int firstRowIndex,int lastRowIndex,bool isTotal = false)
        {


            FirstRowIndex = firstRowIndex;
            LastRowIndex = lastRowIndex;
            IsTotal = isTotal;

        }


        /// <summary>
        /// 子表开始行索引
        /// </summary>
        public int FirstRowIndex { get; set; }


        /// <summary>
        /// 子表结束行索引
        /// </summary>
        public int LastRowIndex { get; set; }


        /// <summary>
        /// 是否合计
        /// </summary>
        public bool IsTotal { get; set; }



    }
}
