using System;
namespace EasyClick.Web.Mini
{
    public interface IMiniJsonFactory
    {
        string GetItemJson(object item);
        string GetItemJson(object item, string[] fields);

        string GetItemJson(object item,DataFormatCollection dataFormats, string[] fields);
    }


}
