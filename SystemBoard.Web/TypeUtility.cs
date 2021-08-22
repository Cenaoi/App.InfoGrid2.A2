using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class TypeUtility
    {
        /// <summary>
        /// 判断 A 是否继承 B 对象
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsInherit(Type a, Type b)
        {
            bool isInherit = false;

            Type baseT = a.BaseType;

            for (int i = 0; i < 99; i++)
			{
                if (baseT == null)
                {
                    break;
                }

			    if(b == baseT)
                {
                    isInherit = true;
                    break;
                }

                baseT = baseT.BaseType;
			}

            return isInherit;
        }

        ///// <summary>
        ///// 判断 A 是否继承 B 对象
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="b"></param>
        ///// <returns></returns>
        //public static bool IsInheritInterface(Type a, string name)
        //{
        //    Type b = a.GetInterface(name);


            
        //    bool isInherit = false;

        //    Type baseT = a.BaseType;

        //    for (int i = 0; i < 99; i++)
        //    {
        //        if (baseT == null)
        //        {
        //            break;
        //        }

        //        if (b == baseT)
        //        {
        //            isInherit = true;
        //            break;
        //        }

        //        baseT = baseT.BaseType;
        //    }

        //    return isInherit;
        //}
        
    }


}
