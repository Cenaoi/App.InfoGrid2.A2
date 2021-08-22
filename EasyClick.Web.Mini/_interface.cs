using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 安全组件
    /// </summary>
    public interface ISecControl
    {
        /// <summary>
        /// 代码 集合
        /// </summary>
        string SecFunCode { get; set; }

        /// <summary>
        /// ID 集合
        /// </summary>
        string SecFunID { get; set; }

        /// <summary>
        /// 安全组件的集合
        /// </summary>
        SecControlCollection SecControls {get;}

        /// <summary>
        /// 是否可以显示的状态
        /// </summary>
        bool Visible { get; set; }

    }

    /// <summary>
    /// 安全指令的接口
    /// </summary>
    public interface ISecCommand
    {
        /// <summary>
        /// 安全指令
        /// </summary>
        /// <param name="funCode"></param>
        void SecCommand(string funCode);
    }


    /// <summary>
    /// 安全组件的集合
    /// </summary>
    public class SecControlCollection : List<ISecControl>
    {
        
    }
}
