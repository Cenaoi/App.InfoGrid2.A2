using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 日期帮助类
    /// </summary>
    [Obsolete]
    public static class DateUtility
    {

        public static DateTime StartDay(string text)
        {
            DateTime now = DateTime.Now;

            if (DateTime.TryParse(text, out now))
            {
                return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            }

            return now;
        }

        public static DateTime EndDay(string text)
        {
            DateTime now = DateTime.Now;

            if (DateTime.TryParse(text, out now))
            {
                return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            }

            return now;
        }

        /// <summary>
        /// 开始时间（当天）
        /// </summary>
        /// <returns></returns>
        public static DateTime StartByToday()
        {
            DateTime now = DateTime.Now;

            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }

        /// <summary>
        /// 开始时间（当天）
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime StartByToday(int day)
        {
            DateTime now = DateTime.Now.AddDays(day);

            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }

        /// <summary>
        /// 结束时间（当天）
        /// </summary>
        /// <returns></returns>
        public static DateTime EndByTodaty()
        {
            DateTime now = DateTime.Now;

            return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        /// <summary>
        /// 当前月份 1日 0：0 
        /// </summary>
        /// <returns></returns>
        public static DateTime StartByMonth()
        {
            DateTime now = DateTime.Now;

            return new DateTime(now.Year, now.Month, 1, 0, 0, 0);
        }

        /// <summary>
        /// 当前月份 
        /// </summary>
        /// <param name="momths"></param>
        /// <returns></returns>
        public static DateTime StartByMonth(int momths)
        {
            DateTime now = DateTime.Now;

            //if (momths != 0)
            //{
            //    now = now.AddMonths(momths);
            //}

            return new DateTime(now.Year, momths, 1, 0, 0, 0);
        }

        public static DateTime StartByMonth(int year, int momths)
        {
            return new DateTime(year, momths, 1, 0, 0, 0);
        }

        /// <summary>
        /// 当月结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime EndByMonth()
        {
            DateTime now = DateTime.Now;

            int dayNum = DateTime.DaysInMonth(now.Year, now.Month);

            DateTime d = new DateTime(now.Year, now.Month, dayNum, 23, 59, 59);

            return d;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="momths"></param>
        /// <returns></returns>
        public static DateTime EndByMonth(int year, int momths)
        {
            DateTime now = DateTime.Now;

            //now = now.AddMonths(momths);

            int dayNum = DateTime.DaysInMonth(year, momths);

            DateTime d = new DateTime(year, momths, dayNum, 23, 59, 59);

            return d;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="momths"></param>
        /// <returns></returns>
        public static DateTime EndByMonth(int momths)
        {
            DateTime now = DateTime.Now;

            now = now.AddMonths(momths);

            int dayNum = DateTime.DaysInMonth(now.Year, now.Month);

            DateTime d = new DateTime(now.Year, now.Month, dayNum, 23, 59, 59);

            return d;
        }

        /// <summary>
        /// 指定当年的 1月1日 0:0:0
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public static DateTime StartByYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0);
        }

        /// <summary>
        /// 指定当年的结束时间 1月1日 0:0:0
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public static DateTime EndByYear(int year)
        {
            DateTime now = DateTime.Now;

            int dayNum = DateTime.DaysInMonth(year, 12);

            return new DateTime(year, 12, dayNum, 23, 59, 59);
        }

        public static DateTime StartByYear()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, 1, 1, 0, 0, 0);
        }

        public static DateTime EndByYear()
        {
            DateTime now = DateTime.Now;
            int dayNum = DateTime.DaysInMonth(now.Year, 12);

            return new DateTime(now.Year, 12, dayNum, 23, 59, 59);
        }

        /// <summary>
        /// 星期几
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static string DayOfWeek(DayOfWeek week, string[] language)
        {
            int n = (int)week;

            return language[n];
        }

        public static string DayOfWeek(DayOfWeek week)
        {

            return DayOfWeek(week, new string[] { "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" });
        }

        public static string DayOfWeek()
        {
            return DayOfWeek(DateTime.Today.DayOfWeek);
        }

    }
}
