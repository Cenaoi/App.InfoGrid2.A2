using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.JsonSQL
{
    public struct Limit
    {
        //起始行
        public int Start { get; set; }

        /// <summary>
        /// 页的数量
        /// </summary>
        public int Count { get; set; }

        public Limit(int count)
        {
            this.Start = 0;
            this.Count = count;
        }

        public Limit(int start,int count)
        {
            this.Start = start;
            this.Count = count;
        }


        public bool IsEmpty()
        {
            return (this.Start == 0 && this.Count == 0);
        }
    }
}
