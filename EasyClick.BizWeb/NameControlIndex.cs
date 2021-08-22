using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.BizWeb
{
    /// <summary>
    /// 按控件名
    /// </summary>
    public class NameControlIndex : SortedDictionary<string, Type>
    {

        public void Add(Type t)
        {
            base.Add(t.Name, t);
        }



    }
}
