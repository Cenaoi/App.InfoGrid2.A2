using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    public enum TextAlign
    {
        Left = 1,
        Center,
        Right
    }

    
    /// <summary>
    /// 指定哪些滚动条在控件上可见。
    /// </summary>
    public enum ScrollBars
    {
        /// <summary>
        /// 不显示任何滚动条。
        /// </summary>
        None = 0,
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto = 1,
        /// <summary>
        /// 只显示水平滚动条。
        /// </summary>
        Horizontal = 2,
        /// <summary>
        /// 只显示垂直滚动条。
        /// </summary>
        Vertical = 3,
        /// <summary>
        /// 同时显示水平滚动条和垂直滚动条。
        /// </summary>
        Both = 4,
    }

    /// <summary>
    /// 滚动条设置
    /// </summary>
    public enum ScrollMode
    {
        /// <summary>
        /// 不显示
        /// </summary>
        None,
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto,
        /// <summary>
        /// 水平滚动条
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直滚动条
        /// </summary>
        Vertical
    }

    /// <summary>
    /// 控件布局方向
    /// </summary>
    public enum FlowDirection
    {
        /// <summary>
        /// 上到下
        /// </summary>
        TopDown,
        /// <summary>
        /// 左到右
        /// </summary>
        LeftToRight
    }

    public enum UiStyle
    {
        Default,
        Head,
        Body, 
        Footer
    }

    /// <summary>
    /// 版面区域
    /// </summary>
    public enum RegionType
    {
        /// <summary>
        /// 北
        /// </summary>
        North,
        /// <summary>
        /// 南
        /// </summary>
        South,
        /// <summary>
        /// 西
        /// </summary>
        West,
        /// <summary>
        /// 东
        /// </summary>
        East,
        /// <summary>
        /// 中间
        /// </summary>
        Center,
    }

    /// <summary>
    /// dock 
    /// </summary>
    public enum DockStyle
    {
        None,

        Left,
        Right,
        Top,
        Bottom,
        Center,
        Full
    }

    /// <summary>
    /// 布局样式
    /// </summary>
    public enum LayoutStyle
    {
        /// <summary>
        /// 自动模式
        /// </summary>
        Auto,

        /// <summary>
        /// 绝对位置
        /// </summary>
        Absolute,
        /// <summary>
        /// 手风琴效果
        /// </summary>
        Accordion ,
        /// <summary>
        /// 锚定
        /// </summary>
        Anchor,

        /// <summary>
        /// 边界式: 把容器分成东南西北中五个区域
        /// 分别由east，south, west，north, cente来表示
        /// </summary>
        Border,
        
        /// <summary>
        /// 卡牌模式
        /// </summary>
        Card,
        /// <summary>
        /// 列式
        /// </summary>
        Column,
        /// <summary>
        /// 填充整个布局，只显示一个
        /// </summary>
        Fit,
        /// <summary>
        /// 表单布局
        /// </summary>
        /// <remarks>
        /// 专门用于管理表单中输入字段的布局，这种布局主要用于在程序中创建表单字段或表单元素等使用。
        /// hideLabels：tru表示隐藏标签，默认为false。
        /// labelAlign：标签队齐方式left、right、center,默认为left。
        /// </remarks>
        Form,
        /// <summary>
        /// 表格布局
        /// </summary>
        Table,
        /// <summary>
        /// 水平对齐  Horizontal Box，
        /// </summary>
        HBox,
        /// <summary>
        /// 垂直对齐 vertical Box
        /// </summary>
        VBox,
        /// <summary>
        /// 工具条式
        /// </summary>
        Toolbar
    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum Directions
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal,

        /// <summary>
        /// 垂直
        /// </summary>
        Vertical
    }

    /// <summary>
    /// 复选框属性接口
    /// </summary>
    public interface IChecked
    {
        bool Checked { get; set; }

        string TrueValue { get; set; }

        string FalseValue { get; set; }
    }
}
