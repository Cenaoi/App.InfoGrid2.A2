using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 指定标识符以指示对话框的返回值。
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// 从对话框返回了 Nothing。这表明有模式对话框继续运行。
        /// </summary>
        None,
        /// <summary>
        /// 对话框的返回值是 OK（通常从标签为“确定”的按钮发送）。
        /// </summary>
        OK,
        /// <summary>
        /// 对话框的返回值是 Cancel（通常从标签为“取消”的按钮发送）。
        /// </summary>
        Cancel,
        /// <summary>
        /// 对话框的返回值是 Abort（通常从标签为“中止”的按钮发送）。
        /// </summary>
        Abort,
        /// <summary>
        /// 对话框的返回值是 Retry（通常从标签为“重试”的按钮发送）。
        /// </summary>
        Retry,
        /// <summary>
        /// 对话框的返回值是 Ignore（通常从标签为“忽略”的按钮发送）。
        /// </summary>
        Ignore,
        /// <summary>
        /// 对话框的返回值是 Yes（通常从标签为“是”的按钮发送）。
        /// </summary>
        Yes,
        /// <summary>
        /// 对话框的返回值是 No（通常从标签为“否”的按钮发送）。
        /// </summary>
        No
    }

    /// <summary>
    /// 视图窗体
    /// </summary>
    public class EcView
    {
        /// <summary>
        /// 关闭窗体
        /// </summary>
        public static void close()
        {
            
            MiniHelper.Eval("EcView.close();");
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="args">参数.例 {a1:'1',a2:'2'}</param>
        public static void close(string args)
        {
            MiniHelper.Eval("EcView.close(" + args + ");");
        }

        

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="args">参数对象</param>
        public static void close(object args)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("EcView.close(");

            sb.Append(");");
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="url">窗体url地址</param>
        /// <param name="title">窗口标题</param>
        /// <param name="width">窗体宽度</param>
        /// <param name="height">窗体高度</param>
        public static void show(string url, string title, int width, int height)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("EcView.show(\"{0}\",\"{1}\",{2},{3});", url, title, width, height);

            MiniHelper.Eval(sb.ToString());
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="url">窗体url地址</param>
        /// <param name="title">窗口标题</param>
        public static void show(string url, string title)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("EcView.show(\"{0}\",\"{1}\")", url, title);




            sb.Append(";");

            MiniHelper.Eval(sb.ToString());
        }

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        /// <param name="url">窗体url地址</param>
        /// <param name="title">窗口标题</param>
        /// <param name="width">窗体宽度</param>
        /// <param name="height">窗体高度</param>
        public static void showDialog(string url, string title, int width, int height)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("EcView.showDialog(\"{0}\",\"{1}\", {2}, {3})", url, title,width,height);

            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (context.Items.Contains("EcView_ClosedFn"))
            {
                string fun = (string)context.Items["EcView_ClosedFn"];
                sb.Append(".closed(function(sender,e){ ");

                sb.Append(fun);

                //sb.Append(";} catch(ecViewE){ alert(ecViewE.Message;) }");
                sb.Append(" })");

                context.Items.Remove("EcView_ClosedFn");
            }

            sb.Append(";");
            MiniHelper.Eval(sb.ToString());
        }

        /// <summary>
        /// 设置关闭窗体触发的事件名称 fun(sender,e)
        /// </summary>
        /// <param name="fun">参数例子: fun(sender,e)</param>
        public static void SetClosedClientScript(string fun)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            context.Items["EcView_ClosedFn"] = fun;
        }

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        /// <param name="url">窗体url地址</param>
        /// <param name="title">窗口标题</param>
        public static void showDialog(string url, string title)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("EcView.showDialog(\"{0}\",\"{1}\")", url, title);

            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (context.Items.Contains("EcView_ClosedFn"))
            {
                string fun = (string)context.Items["EcView_ClosedFn"];
                sb.Append(".closed(function(sender,e){ ");
                
                sb.Append(fun);

                //sb.Append(";} catch(ecViewE){ alert(ecViewE.Message;) }");
                sb.Append(" })");

                context.Items.Remove("EcView_ClosedFn");
            }

            sb.Append(";");
            MiniHelper.Eval(sb.ToString());
        }
    }
}
