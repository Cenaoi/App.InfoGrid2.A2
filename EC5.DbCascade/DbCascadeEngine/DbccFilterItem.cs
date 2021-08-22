using HWQ.Entity.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{
    /// <summary>
    /// 过滤条件
    /// </summary>
    public class DbccFilterItem
    {

        public string A_Table { get; set; }

        public string A_Col { get; set; }

        public Logic A_Logic { get; set; }

        public DbccValueModes B_ValueMode { get; set; }

        public string B_ValueTable { get; set; }

        public string B_ValueCol { get; set; }

        public string B_ValueFixed { get; set; }

        public string B_ValueFun { get; set; }


        /// <summary>
        /// 自定义公式
        /// </summary>
        public string B_ValueUserFunc { get; set; }
    }
}
