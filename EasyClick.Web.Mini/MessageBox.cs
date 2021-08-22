using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 指定若干常数，用以定义 System.Windows.Forms.MessageBox 上将显示哪些按钮
    /// </summary>
    public enum MessageBoxButtons
    {
        /// <summary>
        /// 消息框包含“确定”按钮。
        /// </summary>
        OK = 0,
        /// <summary>
        /// 消息框包含“确定”和“取消”按钮。
        /// </summary>
        OKCancel = 1,
        /// <summary>
        /// 消息框包含“中止”、“重试”和“忽略”按钮。
        /// </summary>
        AbortRetryIgnore = 2,
        /// <summary>
        /// 消息框包含“是”、“否”和“取消”按钮。
        /// </summary>
        YesNoCancel = 3,
        /// <summary>
        /// 消息框包含“是”和“否”按钮。
        /// </summary>
        YesNo = 4,
        /// <summary>
        /// 消息框包含“重试”和“取消”按钮。
        /// </summary>
        RetryCancel = 5,
    }

    /// <summary>
    /// 指定定义哪些信息要显示的常数。
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// 消息框未包含符号。
        /// </summary>
        None = 0,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        /// </summary>
        Error = 16,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        /// </summary>
        Hand = 16,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个红色背景的圆圈及其中的白色 X 组成的。
        /// </summary>
        Stop = 16,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个圆圈和其中的一个问号组成的。
        /// 不再建议使用问号消息图标，原因是该图标无法清楚地表示特定类型的消息，并且问号形式的消息表述可应用于任何消息类型。
        /// 此外，用户还可能将问号消息符号与帮助信息混淆。
        /// 因此，请不要在消息框中使用此问号消息符号。系统继续支持此符号只是为了向后兼容。
        /// </summary>
        Question = 32,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个黄色背景的三角形及其中的一个感叹号组成的。
        /// </summary>
        Exclamation = 48,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个黄色背景的三角形及其中的一个感叹号组成的。
        /// </summary>
        Warning = 48,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的。
        /// </summary>
        Information = 64,
        /// <summary>
        /// 该消息框包含一个符号，该符号是由一个圆圈及其中的小写字母 i 组成的。
        /// </summary>
        Asterisk = 64,
    }


    /// <summary>
    /// 指定若干常数，用以定义 System.Windows.Forms.MessageBox 上的默认按钮。
    /// </summary>
    public enum MessageBoxDefaultButton
    {
        /// <summary>
        /// 消息框上的第一个按钮是默认按钮。
        /// </summary>
        Button1 = 0,
        /// <summary>
        /// 消息框上的第二个按钮是默认按钮。
        /// </summary>
        Button2 = 256,
        /// <summary>
        /// 消息框上的第三个按钮是默认按钮。
        /// </summary>
        Button3 = 512,
    }


    /// <summary>
    /// 未完成；消息框
    /// </summary>
    public class MessageBox
    {
        string m_Text;
        string m_Caption;
        MessageBoxButtons m_Buttons = MessageBoxButtons.OK;
        MessageBoxIcon m_Icon = MessageBoxIcon.Information;
        MessageBoxDefaultButton m_DefaultButton = MessageBoxDefaultButton.Button1;

        public static void Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            MessageBox msg = new MessageBox();
            msg.m_Text = text;
            msg.m_Caption = caption;
            msg.m_Buttons = buttons;
            msg.m_Icon = icon;
            msg.m_DefaultButton = defaultButton;

            MiniHelper.EvalFormat("Mini2.Msg.alert(\"{0}\",\"{1}\");", caption, text);
        }

        public static void Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Show(text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }


        public static void ShowTips(string text)
        {
            Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string text)
        {
            Show(text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string text)
        {
            Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



    }
}
