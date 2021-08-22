using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{

    public class MiniJsonFactory : EasyClick.Web.Mini.IMiniJsonFactory
    {
        public virtual string GetItemJson(object item)
        {
            return GetItemJson(item, null);
        }


        public virtual string GetItemJson(object item, string[] fields)
        {
            return MiniHelper.GetItemJson(item,null, fields);
        }

        public virtual string GetItemJson(object item,DataFormatCollection dataFormats, string[] fields)
        {
            return MiniHelper.GetItemJson(item,dataFormats, fields);
        }

    }
}
