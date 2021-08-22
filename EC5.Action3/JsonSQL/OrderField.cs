using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.JsonSQL
{

    public struct OrderField
    {
        public string Field { get; set; }

        public OrderType Type { get; set; }

        public OrderField(string field)
        {
            this.Field = field;
            this.Type = OrderType.ASC;
        }


        public OrderField(string field, OrderType type)
        {
            this.Field = field;
            this.Type = type;
        }
    }
}
