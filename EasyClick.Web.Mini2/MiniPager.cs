using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 页面助手
    /// </summary>
    public static class MiniPager
    {
        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="url"></param>
        public static void Redirect(string url)
        {
            string js = $"window.location.href = \"{url}\";";

            ScriptManager.Eval(js);
        }


        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="url"></param>
        /// <param name="iframeId">IFrame ID</param>
        public static void Redirect(string iframeId, string url)
        {
            string js = $"$(\"#{iframeId}\").attr(\"src\", \"{url}\");";

            ScriptManager.Eval(js);
        }
    }
}
