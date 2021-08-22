
using EasyClick.Web.Mini2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 主界面的 Tab 标签
    /// </summary>
    public class EcView
    {
        /// <summary>
        /// 显示 EcView 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public static void Show(string url,string title )
        {
            string js = $"Mini2.EcView.show(\"{url}\",\"{JsonUtil.ToJson(title)}\");";

            ScriptManager.Eval(js);
        }


        /// <summary>
        /// 显示 EcView 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        public static void Show(string url, string title, string iconClass)
        {
            string js = $"Mini2.EcView.show(\"{url}\",\"{JsonUtil.ToJson(title)}\", \"{iconClass}\");";

            ScriptManager.Eval(js);
        }

    }
}
