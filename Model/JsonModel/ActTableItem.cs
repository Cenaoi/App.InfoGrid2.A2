using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Model.JsonModel
{
    public class ActTableItem
    {
        public string type_id { get; set; }

        public int view_id { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string table_name { get; set; }

        /// <summary>
        /// 所属的表id
        /// </summary>
        public string owner_table_id { get; set; }
    }
}
