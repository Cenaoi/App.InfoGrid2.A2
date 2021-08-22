using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{

    /// <summary>
    /// 赋值
    /// </summary>
    public class DbccItem
    {
        /// <summary>
        /// 左边字段.变量
        /// </summary>
        public string L_Field { get; set; }

        ///// <summary>
        ///// 右边字段.值
        ///// </summary>
        //public string R_Field { get; set; }

        /// <summary>
        /// 值模式. 值类型,值模式.
        /// </summary>
        public DbccValueModes R_ValueMode { get; set; }

        /// <summary>
        /// 固定值
        /// </summary>
        public string R_ValueFixed { get; set; }

        public string R_ValueFun { get; set; }

        /// <summary>
        /// 值的字段 
        /// </summary>
        public string R_ValueTable { get; set; }

        /// <summary>
        /// 值的字段
        /// </summary>
        public string R_ValueCol { get; set; }

        /// <summary>
        /// 值统计函数
        /// </summary>
        public string R_ValueTotalFun { get; set; }

        /// <summary>
        /// 更新与新建，分别赋值。。默认是 E-更新。
        /// A-新建，E-更新， A-E - 如果不存在就新建
        /// </summary>
        public string L_ActCode { get; set; }


        /// <summary>
        /// 自定义公式
        /// </summary>
        public string R_ValueUserFunc { get; set; }
    }

}
