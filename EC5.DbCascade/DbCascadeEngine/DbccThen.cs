using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{

    /// <summary>
    /// 条件集合
    /// </summary>
    public class DbccThenCollection:List<DbccThen>
    {

    }


    /// <summary>
    /// 条件
    /// </summary>
    public class DbccThen
    {
        public string A_TypeID { get; set; }

        public string A_FieldText { get; set; }

        public string A_Field { get; set; }

        public HWQ.Entity.Filter.Logic A_Login { get; set; }

        /// <summary>
        /// 函数值，普通值
        /// </summary>
        public string A_Value { get; set; }

        /// <summary>
        /// 统计函数
        /// </summary>
        public string A_TotalFun { get; set; }

        /// <summary>
        /// 是否停止
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string ResultMessage { get; set; }

        

    }
}
