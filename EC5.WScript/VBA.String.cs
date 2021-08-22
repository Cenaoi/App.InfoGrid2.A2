using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.WScript
{
    partial class VBA
    {

		public static dynamic LEN(object text)
        {
			if(text == null) { return 0; }

            string sText = Convert.ToString(text);

            return sText.Length;
        }

		public static dynamic VALUE(object text)
        {
			if(text == null) { return 0; }

            string sText = Convert.ToString(text);

            decimal dec;

			if(decimal.TryParse(sText,out dec))
            {
                return dec;
            }

            return 0;
        }

		/// <summary>
        /// 格式化
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format_text"></param>
        /// <returns></returns>
		public static dynamic TEXT(object text, string format_text)
        {
            return string.Format(format_text, text);
        }
    }
}
