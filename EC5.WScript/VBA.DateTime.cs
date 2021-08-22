using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EC5.WScript
{
    partial class VBA
    {
        public static dynamic DATE(object year,object month, object day)
        {
            int iYear = Convert.ToInt32(year);
            int iMonth = Convert.ToInt32(month);
            int iDay = Convert.ToInt32(day);

            return new DateTime(iYear,iMonth,iDay);
        }

        /// <summary>
        /// 主要用于计算两个日期之间的天数、月数或年数。
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="unit">
        /// 1	"Y"：计算两个日期间隔的年数	计算出生日期为1975-1-30人的年龄	=DATEDIF("1975-1-30",TODAY(),"Y")	33
        /// 2	"M"：计算两个日期间隔的月份数 计算日期为1975-1-30与当前日期的间隔月份数	=DATEDIF("1975-1-30", TODAY(),"M")	398
        /// 3	"D"：计算两个日期间隔的天数 计算日期为1975-1-30和当前日期的间隔天数	=DATEDIF("1975-1-30", TODAY(),"D")	12122
        /// 4	"YD"：忽略年数差，计算两个日期间隔的天数 计算日期为1975-1-30和当前日期的不计年数的间隔天数	=DATEDIF("1975-1-30", TODAY(),"YD")	68
        /// 5	"MD"：忽略年数差和月份差，计算两个日期间隔的天数 计算日期为1975-1-30和当前日期的不计月份和年份的间隔天数	=DATEDIF("1975-1-30", TODAY(),"MD")	9
        /// 6	"YM"：忽略相差年数，计算两个日期间隔的月份数 计算日期为1975-1-30和当前日期的不计年份的间隔月份数	=DATEDIF("1975-1-30", TODAY(),"YM")	2
        /// </param>
        /// <returns>其返回的值是两个日期之间的年\月\日间隔数。 </returns>
        public static dynamic DATEDIF(object start_date, object end_date, object unit)
        {
            DateTime dStart = Convert.ToDateTime(start_date);
            DateTime dEnd = Convert.ToDateTime(end_date);

            string sUnit = Convert.ToString(unit);
            
            
            if(sUnit == "Y")
            {
                int start_year = dStart.Year;
                int end_year = dEnd.Year;

                return end_year - start_year;
            }
            else if(sUnit == "M")
            {
                return DateUtil.TotalMonths(dStart, dEnd);
            }
            else if(sUnit =="D" )
            {
                return DateUtil.TotalDays(dStart, dEnd);
            }
            else if(sUnit == "YD")
            {
                int year = DateTime.Today.Year;

                DateTime newDStart = new DateTime(year, dStart.Month, dStart.Day);
                DateTime newDEnd = new DateTime(year, dEnd.Month, dEnd.Day);

                TimeSpan span = newDEnd - newDStart;

                return Math.Abs(span.TotalDays);
            }
            else if(sUnit == "MD")
            {
                return Math.Abs(dEnd.Month - dStart.Month);
            }
            else if(sUnit == "YM")
            {
                return Math.Abs(dEnd.Year - dStart.Year);
            }
            else
            {
                //参数错误
                return null;
            }
            
        }

        /// <summary>
        /// 今天日期
        /// </summary>
        /// <returns></returns>
        public static dynamic TODAY()
        {
            return DateTime.Today;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial_number">Serial_number 表示一个顺序的序列号，代表要查找的那一天的日期。
        /// 应使用 DATE 函数输入日期，或者将函数作为其他公式或函数的结果输入。
        /// 例如，使用 DATE(2008,5,23) 输入 2008 年 5 月 23 日。
        /// 如果日期以文本的形式输入，则会出现问题。 </param>
        /// <param name="return_type">Return_type 为确定返回值类型的数字。 
        /// Return_type 返回的数字
        /// 1或省略 数字1（星期日）到数字7（星期六）。
        /// 2	数字1（星期一）到数字7（星期日）。
        /// 3	数字0（星期一）到数字6（星期日）。</param>
        /// <returns></returns>
        public static dynamic WEEKDAY(object serial_number,object return_type )
        {
            DateTime sDate = Convert.ToDateTime(serial_number);
            int reType = Convert.ToInt32(return_type ?? 1);

            if(reType == 1)
            {
                return (int)sDate.DayOfWeek + 1;
            }
            else if(reType == 2)
            {
                if(sDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return 7;
                }
                else
                {
                    return (int)sDate.DayOfWeek;
                }
            }
            else if(reType == 3)
            {
                if (sDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return 6;
                }
                else
                {
                    return (int)sDate.DayOfWeek - 1;
                }
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial_number">Serial_number 表示一个顺序的序列号，代表要查找的那一天的日期。
        /// 应使用 DATE 函数输入日期，或者将函数作为其他公式或函数的结果输入。
        /// 例如，使用 DATE(2008,5,23) 输入 2008 年 5 月 23 日。
        /// 如果日期以文本的形式输入，则会出现问题。 </param>
        /// <returns></returns>
        public static dynamic WEEKDAY(object serial_number)
        {
            return WEEKDAY(serial_number, 1);
        }

        /// <summary>
        /// 返回一个数字，该数字代表一年中的第几周。
        /// </summary>
        /// <param name="serial_num">
        /// Serial_num 代表一周中的日期。 
        /// <c>
        /// 应使用 DATE 函数来输入日期，或者将日期作为其他公式或函数的结果输入。
        /// 例如，使用 DATE(2008,5,23) 输入 2008 年 5 月 23 日。
        /// 如果日期以文本的形式输入，则会出现问题。
        /// </c>
        /// </param>
        /// <param name="return_type"></param>
        /// <returns></returns>
        public static dynamic WEEKNUM(object serial_num, object return_type)
        {
            DateTime sDate = Convert.ToDateTime(serial_num);
            int reType = Convert.ToInt32(return_type ?? 1);

            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(sDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return weekOfYear;
        }
    }
}
