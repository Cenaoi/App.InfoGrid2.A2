using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    public class DataStoreCellCollection : List<DataStoreCell>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            bool exist = false;

            foreach (DataStoreCell item in this)
            {
                if (name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }

        public DataStoreCell this[string name]
        {
            get
            {
                foreach (DataStoreCell item in this)
                {
                    if (name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

    }

}
