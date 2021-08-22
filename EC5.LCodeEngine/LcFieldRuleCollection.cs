using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LCodeEngine
{

    /// <summary>
    /// 代码规则集合
    /// </summary>
    public class LcFieldRuleCollection : List<LcFieldRule>
    {

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (var item in this)
            {
                if (i++ > 0) { sb.Append(",").AppendLine(); };

                sb.AppendFormat("【{0}={1}】", item.Field, item.Code);
            }

            return sb.ToString();
        }
    }
}
