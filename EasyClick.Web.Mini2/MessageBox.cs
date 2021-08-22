using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 消息对话框
    /// </summary>
    [Description("消息对话框")]
    public class MessageBox
    {

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg">消息内容</param>
        public static void Alert( string msg)
        {
            ScriptManager.Eval("Mini2.Msg.alert(\"提示\", \"{0}\");", JsonUtil.ToJson(msg) );
        }


        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息内容</param>
        public static void Alert(string title, string msg)
        {
            ScriptManager.Eval("Mini2.Msg.alert(\"{0}\", \"{1}\");", JsonUtil.ToJson(title), JsonUtil.ToJson(msg));
        }



        /// <summary>
        /// 输入
        /// </summary>
        /// <param name="msg"></param>
        public static void Prompt(string msg)
        {
            ScriptManager.Eval("Mini2.Msg.prompt(\"输入\", \"{0}\");", JsonUtil.ToJson(msg));
        }

        /// <summary>
        /// 输入框
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="callback">回调函数</param>
        public static void Prompt(string msg, Delegate callback)
        {
            string code = GetDelegateCode(callback);

            ScriptManager.Eval("Mini2.Msg.prompt(\"{0}\", \"{1}\", {2});", "输入", JsonUtil.ToJson(msg), code);
        }

        /// <summary>
        /// 输入
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg"></param>
        /// <param name="callback">回调函数</param>
        public static void Prompt(string title, string msg, Delegate callback)
        {
            string code = GetDelegateCode(callback);

            ScriptManager.Eval("Mini2.Msg.prompt(\"{0}\", \"{1}\", {2});", title, JsonUtil.ToJson(msg), code);
        }




        /// <summary>
        /// 询问
        /// </summary>
        /// <param name="msg"></param>
        public static void Confirm(string msg)
        {
            ScriptManager.Eval("Mini2.Msg.confirm(\"询问\", \"{0}\");", JsonUtil.ToJson(msg));
        }

        /// <summary>
        /// 询问
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="callback">回调函数</param>
        public static void Confirm(string msg , Delegate callback)
        {
            string code = GetConfirmDelegateCode(callback);

            ScriptManager.Eval("Mini2.Msg.confirm(\"询问\", \"{0}\", {1});", JsonUtil.ToJson(msg), code);
        }


        private static string GetDelegateCode(Delegate delegateFun)
        {
            if (delegateFun == null) throw new ArgumentNullException("okDelegate", "回调函数不能为空.");

            StringBuilder code = new StringBuilder();

            string method = delegateFun.Method.Name;

            code.AppendLine("function(e){");

            //code.AppendLine("    alert( Mini2.Json.toJson(e) ); ");

            // code.AppendLine("    if( e.result != 'ok') return false;

            code.AppendLine("    var value = this.ownerWindow.getValue();");

            code.AppendLine("    widget1.submit('form', {");
            code.AppendLine("        RMode:'callback', ");
            code.AppendLine($"        action: '{method}', ");
            code.AppendLine($"        actionPs: value ");

            code.AppendLine("    });");

            code.AppendLine("}");

            return code.ToString();
        }

        private static string GetConfirmDelegateCode(Delegate delegateFun)
        {
            if (delegateFun == null) throw new ArgumentNullException("okDelegate", "回调函数不能为空.");

            StringBuilder code = new StringBuilder();

            string method = delegateFun.Method.Name;

            code.AppendLine("function(e){");
            
            code.AppendLine("    widget1.submit('form', {");
            code.AppendLine("        RMode:'callback', ");
            code.AppendLine($"        action: '{method}', ");
            code.AppendLine($"        actionPs: 'ok' ");

            code.AppendLine("    });");

            code.AppendLine("}");

            return code.ToString();
        }

    }


}
