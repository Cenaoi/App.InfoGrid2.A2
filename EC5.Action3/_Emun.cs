using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{


    /// <summary>
    /// 监听表方法
    /// </summary>
    public enum ListenMethod
    {
        /// <summary>
        /// 监听全部
        /// </summary>
        All,
        /// <summary>
        /// 监听查询(备用)
        /// </summary>
        Select,
        /// <summary>
        /// 监听插入
        /// </summary>
        Insert,
        /// <summary>
        /// 监听更新
        /// </summary>
        Update,
        /// <summary>
        /// 监听删除
        /// </summary>
        Delete
    }

    /// <summary>
    /// 操作方法
    /// </summary>
    public enum OperateMethod
    {
        /// <summary>
        /// 查询操作
        /// </summary>
        Select,
        /// <summary>
        /// 插入操作
        /// </summary>
        Insert,
        /// <summary>
        /// 更新操作
        /// </summary>
        Update,
        /// <summary>
        /// 删除操作
        /// </summary>
        Delete
    }

    /// <summary>
    /// 脚本类型
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// C# 脚本
        /// </summary>
        CSharp,

        Xml,

        Json
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateActionType
    {
        /// <summary>
        /// 更新字段
        /// </summary>
        UpdateField,

        /// <summary>
        /// 执行 C# 脚本
        /// </summary>
        CSharp,

        /// <summary>
        /// 执行 XML
        /// </summary>
        Xml
    }

    /// <summary>
    /// 操作的混合模式
    /// </summary>
    public enum OperateMixingMode
    {
        /// <summary>
        /// 没有混合
        /// </summary>
        None,

        /// <summary>
        /// 先更新字段 -> 执行 C# 脚本
        /// </summary>
        UpdateField_2_Script,

        /// <summary>
        /// 先执行脚本 -> 更新字段
        /// </summary>
        Script_2_UpdateField
    }

    public enum ActionModeType
    {
        /// <summary>
        /// 没有任何操作
        /// </summary>
        None,

        /// <summary>
        /// 函数值
        /// </summary>
        Fun,        

        /// <summary>
        /// 固定值
        /// </summary>
        Fixed
    }


}
