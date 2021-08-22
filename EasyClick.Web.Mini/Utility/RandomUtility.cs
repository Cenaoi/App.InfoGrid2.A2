using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini.Utility
{
    /// <summary>
    /// 随机数
    /// </summary>
    static class RandomUtility
    {
        static Random m_Random;

        static RandomUtility()
        {
            long tick = DateTime.Now.Ticks;

            m_Random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        public static int Next(int minValue, int maxValue)
        {
            return m_Random.Next(minValue, maxValue);
        }

        public static int Next(int maxValue)
        {
            return m_Random.Next(maxValue);
        }
    }
}
