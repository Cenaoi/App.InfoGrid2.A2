using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 脚本管理器
    /// </summary>
    public static class MiniScriptManager
    {
        /// <summary>
        /// 客户端脚本
        /// </summary>
        public static MiniScript ClientScript
        {
            get
            {

                MiniScript script = HttpContext.Current.Items["MiniClientScriptManager"] as MiniScript;

                if (script == null)
                {
                    script = new MiniScript();

                    HttpContext.Current.Items["MiniClientScriptManager"] = script;
                }

                return script;
            }
        }
    }

}
