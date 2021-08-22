using System.Collections.Generic;
using EasyClick.Web.Mini;
using HWQ.Entity.LightModels;

namespace EasyClick.BizWeb.UI
{
    public static class BizDataHelper
    {
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        public static void Fill(MiniHtmlListBase listControl, IList<LModel> data, string textField)
        {
            Fill(listControl, data, textField, textField);
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        public static void Fill(MiniHtmlListBase listControl, IList<LModel> data, string valueField, string textField)
        {
            listControl.Items.Clear();

            foreach (LModel item in data)
            {
                string value = item.Field<string>(valueField);
                string text = item.Field<string>(textField);

                listControl.Items.Add(value, text);
            }
        }


    }
}
