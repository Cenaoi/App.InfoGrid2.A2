using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 消息样式
    /// </summary>
    public enum ToastStyle
    {
        Base,
        /// <summary>
        /// 成功完成的消息
        /// </summary>
        Success ,

        /// <summary>
        /// 警告信息
        /// </summary>
        Warning
    }

    /// <summary>
    /// 消息
    /// </summary>
    public class Toast
    {
        /// <summary>
        /// 完成信息
        /// </summary>
        public const string STYLE_SUCCESS = "success";

        /// <summary>
        /// 警告信息
        /// </summary>
        public const string STYLE_WARNING = "warning";

        /// <summary>
        /// 基础信息
        /// </summary>
        public const string STYLE_BASE = "base";


        /// <summary>
        /// 上级容器
        /// </summary>
        public string Parent { get; set; } = "left-notify";

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 显示时间
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string style { get; set; }


        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="text"></param>
        public static void Show(string text)
        {
            ScriptManager.Eval("Mini2.toast(\"{0}\");", JsonUtil.ToJson(text));
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public static void Show(string text, string style)
        {
            ScriptManager.Eval("Mini2.toast(\"{0}\",null,\"{1}\");", JsonUtil.ToJson(text), style);
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        public static void Show(string text, ToastStyle style)
        {
            ScriptManager.Eval("Mini2.toast(\"{0}\",null,\"{1}\");", JsonUtil.ToJson(text), style.ToString().ToLower());
        }
    }
}
