using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 验证
    /// </summary>
    [Obsolete]
    public static class ValidateUtility
    {

        static string[] m_SqlStrs;

        static ValidateUtility()
        {
            //取得webconfig中过滤字符串  
            string sqlStr = "=|declare|exec|varchar|cursor|begin|open|drop|creat|select|truncate";
            m_SqlStrs = sqlStr.Split('|');
        }

        /// <summary>
        /// 验证是否包含 SQL 注入攻击
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SqlInput(string value)
        {
            bool returnValue = true;

            try
            {
                if (value.Trim().Length >= 1)
                {
                    for (int i = 0; i < m_SqlStrs.Length; i++)
                    {
                        string ss = m_SqlStrs[i];
                        if (value.ToLower().IndexOf(ss) >= 0)
                        {
                            returnValue = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }


        public static bool Email(string value)
        {
            string reg = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            bool valid = System.Text.RegularExpressions.Regex.IsMatch(value, reg);

            //if (!valid)
            //{
            //    return new ValidatorError("请输入正确格式的电子邮件");
            //}

            return valid;
        }

        public static bool Url(string value)
        {
            string reg = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";

            bool valid = System.Text.RegularExpressions.Regex.IsMatch(value, reg);

            //if (!valid)
            //{
            //    return new ValidatorError("请输入合法的网址");
            //}

            return valid;
        }

        public static bool Date(string value)
        {
            DateTime target;

            bool valid = DateTime.TryParse(value, out target);

            //if (!valid)
            //{
            //    return new ValidatorError("请输入合法的日期");
            //}

            return valid;
        }

        public static bool DateISO(string value)
        {

            string reg = @"^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$";

            bool valid = System.Text.RegularExpressions.Regex.IsMatch(value, reg);

            //if (!valid)
            //{
            //    return new ValidatorError("请输入合法的日期 (ISO)");
            //}

            return valid;
        }

        public static bool Number(string value)
        {
            decimal target;

            bool valid = decimal.TryParse(value, out target);

            //if (!valid)
            //{
            //    return new ValidatorError("请输入合法的数字");
            //}

            return valid;
        }

        public static bool Digits(string value)
        {
            string reg = @"^[0-9]*$";

            bool valid = System.Text.RegularExpressions.Regex.IsMatch(value, reg);

            //if (!valid)
            //{
            //    return new ValidatorError("只能输入整数");
            //}

            return valid;
        }

        public static bool MaxLength(string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return false;// new ValidatorError("请输入一个长度最多是 {0} 的字符串", maxLength);
            }

            return true;
        }

        public static bool MinLength(string value, int minlength)
        {
            if (value.Length < minlength)
            {
                return false;// new ValidatorError("请输入一个长度最少是 {0} 的字符", minlength);
            }

            return true;
        }

        public static bool Max(string value, decimal max)
        {
            decimal target;

            bool valid = decimal.TryParse(value, out target);

            if (!valid || target < max)
            {
                return false;// new ValidatorError("请输入一个最大为 {0} 的值", max);
            }

            return true;
        }

        public static bool Min(string value, decimal min)
        {
            decimal target;

            bool valid = decimal.TryParse(value, out target);

            if (!valid || target < min)
            {
                return false;// new ValidatorError("请输入一个最小为 {0} 的值", min);
            }

            return true;
        }

        /// <summary>
        /// 请输入一个长度介于 {0} 和 {1} 之间的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="elem"></param>
        /// <param name="rangeLength"></param>
        /// <returns></returns>
        public static bool RangeLength(string value, decimal[] rangeLength)
        {
            if (rangeLength == null || rangeLength.Length != 2)
            {
                return false;
            }

            if (value.Length < rangeLength[0] || value.Length > rangeLength[1])
            {
                return false;// new ValidatorError(string.Format("请输入一个长度介于 {0} 和 {1} 之间的字符串", rangeLength[0], rangeLength[1]));
            }


            return true;
        }

        /// <summary>
        /// 请输入一个介于 {0} 和 {1} 之间的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="elem"></param>
        /// <param name="rangeLength"></param>
        /// <returns></returns>
        public static bool Range(string value, decimal[] range)
        {
            if (range == null || range.Length != 2)
            {
                return false;
            }

            decimal target;

            bool valid = decimal.TryParse(value, out target);

            if (target < range[0] || target > range[1])
            {
                return false;// new ValidatorError(string.Format("请输入一个介于 {0} 和 {1} 之间的值", range[0], range[1]));
            }


            return true;
        }


        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="elem"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool Regex(string value,string regex)
        {
            bool valid = System.Text.RegularExpressions.Regex.IsMatch(value, regex);

            //if (!valid)
            //{
            //    return new ValidatorError("验证失败");
            //}

            return valid;
        }

    }
}
