using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    public class CodeIndexItem
    {
        /// <summary>
        /// 监听对象的编码
        /// </summary>
        public string Code { get; set; }


        public CodeIndexItem()
        {


        }

        public CodeIndexItem(string code)
        {
            this.Code = code;
        }

    }




}
