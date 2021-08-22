using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.WScript
{
    partial class VBA
    {

        public static decimal ABS(decimal number)
        {
            return Math.Abs(number);
        }

        public static int INT(decimal number)
        {
            return Convert.ToInt32(number);
        }

        public static double PI()
        {
            return Math.PI;
        }


    }
}
