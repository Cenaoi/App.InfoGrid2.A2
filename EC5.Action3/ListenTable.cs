using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{


    /// <summary>
    /// 监听表
    /// </summary>
    public class ListenTable : ListenBase
    {

        public ListenTable()
        {

        }

        public ListenTable(string code) : base(code)
        {
            this.Table = code;
        }

        public ListenTable(string code, ListenMethod method):base(code)
        {
            this.Table = code;
            this.Method = method;
        }

      

        /// <summary>
        /// 操作函数
        /// </summary>
        public ListenMethod Method { get; set; }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string Table { get; set; }

        public string TableText { get; set; }


        /// <summary>
        /// （未编译）监听条件的过滤 C# 脚本
        /// </summary>
        public ScriptBase CondScript { get; set; }

        /// <summary>
        /// 监听条件过滤 ( 通过 CodeScript 编译后产生, XML 或 Json 产生的对象)
        /// </summary>
        public FilterItem CondFilter { get; set;  }


        /// <summary>
        /// （未编译）值检测变化的脚本
        /// </summary>
        public ScriptBase VChangeScript { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }



    }
}
