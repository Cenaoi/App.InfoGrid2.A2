using System;
using System.Collections.Generic;
using System.Web;
using NPOI.SS.UserModel;
using System.Diagnostics;

namespace App.InfoGrid2.Excel_Template
{
    /// <summary>
    /// 单元格的属性
    /// </summary>
    [DebuggerDisplay("X={X},Y={Y},Width={Width},Height={Height}")]
    public class CellPro
    {

        public CellPro()
        {
            this.Display = true;
        }

        /// <summary>
        /// 行的位置
        /// </summary>
        public int RowIndex { get; set; }
        /// <summary>
        /// 列的位置
        /// </summary>
        public int ColsIndex { get; set; }
        /// <summary>
        /// 字体颜色
        /// </summary>
        public short FontColor { get; set; }
        /// <summary>
        /// 字体名字
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public short FontSize { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 下边框类型
        /// </summary>
        public BorderStyle BorderBottomName { get; set; }
        /// <summary>
        /// 下边框颜色
        /// </summary>
        public short BorderBottomColor { get; set; }
        /// <summary>
        /// 上边框类型
        /// </summary>
        public BorderStyle BorderTopName { get; set; }
        /// <summary>
        /// 上边框颜色
        /// </summary>
        public short BorderTopColor { get; set; }
        /// <summary>
        /// 左边框类型
        /// </summary>
        public BorderStyle BorderLeftName { get; set; }
        /// <summary>
        /// 左边框颜色
        /// </summary>
        public short BorderLeftColor { get; set; }
        /// <summary>
        /// 右边框类型
        /// </summary>
        public BorderStyle BorderRightName { get; set; }
        /// <summary>
        /// 右边框颜色
        /// </summary>
        public short BorderRightColor { get; set; }
        /// <summary>
        /// 左右对齐方式
        /// </summary>
        public HorizontalAlignment Alignment { get; set; }
        /// <summary>
        /// 垂直对齐方式
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// 跨行开始位置
        /// </summary>
        public int SpanFirstRow { get; set; }

        /// <summary>
        /// 跨行结束位置
        /// </summary>
        public int SpanLastRow { get; set; }


        /// <summary>
        /// 跨列开始位置
        /// </summary>
        public int SpanFirstCell { get; set; }

        /// <summary>
        /// 跨列结束位置
        /// </summary>
        public int SpanLastCell { get; set; }

        /// <summary>
        /// 字体粗细
        /// </summary>
        public short FontBlod { get; set; }

        /// <summary>
        /// 数据格式的名称
        /// </summary>
        public string FormatName { get; set; }
        /// <summary>
        /// 数据格式的数字
        /// </summary>
        public short DataFormat { get; set; }

        /// <summary>
        /// 单元格类型
        /// </summary>
        public CellTypeName CellType { get; set; }

        /// <summary>
        /// 填充背景色
        /// </summary>
        public short FillBackgroundColor { get; set; }

        /// <summary>
        /// 填充前景色
        /// </summary>
        public short FillForegroundColor { get; set; }
        /// <summary>
        /// 填充模式
        /// </summary>
        public FillPattern FillPattern { get; set; }

        /// <summary>
        /// X轴坐标
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y轴坐标
        /// </summary>
        public float Y { get; set; }


        /// <summary>
        /// 计算后的宽度
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public float Height { get; set; }


        /// <summary>
        /// 动态高度  像素单位
        /// </summary>
        public float DynHeight { get; set; }

        


        /// <summary>
        /// 跨列后宽度  单位磅
        /// </summary>
        public float SpanWidth { get; set; }


        /// <summary>
        ///跨行后高度
        /// </summary>
        public float SpanHeight { get; set; }


        /// <summary>
        /// 是否显示，true -- 显示 ,false -- 不显示
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 是否缩放
        /// </summary>
        public bool ShrinkToFit { get; set; }

        /// <summary>
        /// 是否是斜体， true -- 是
        /// </summary>
        public bool IsItalic { get; set; }

        /// <summary>
        /// 是否换行
        /// </summary>
        public bool WrapText { get; set; }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public CellPro Clone()
        {
            CellPro cp1 = new CellPro();

            cp1.Alignment = this.Alignment;
            cp1.BorderBottomColor = this.BorderBottomColor;
            cp1.BorderBottomName = this.BorderBottomName;
            cp1.BorderLeftColor = this.BorderLeftColor;
            cp1.BorderLeftName = this.BorderLeftName;
            cp1.BorderRightColor = this.BorderRightColor;
            cp1.BorderRightName = this.BorderRightName;
            cp1.BorderTopColor = this.BorderTopColor;
            cp1.BorderTopName = this.BorderTopName;
            cp1.ColsIndex = this.ColsIndex;
            cp1.FontBlod = this.FontBlod;
            cp1.FontColor = this.FontColor;
            cp1.FontName = this.FontName;
            cp1.FontSize = this.FontSize;
            cp1.RowIndex = this.RowIndex;
            cp1.SpanFirstCell = this.SpanFirstCell;
            cp1.SpanFirstRow = this.SpanFirstRow;
            cp1.SpanLastCell = this.SpanLastCell;
            cp1.SpanLastRow = this.SpanLastRow;
            cp1.Value = this.Value;
            cp1.VerticalAlignment = this.VerticalAlignment;
            cp1.ShrinkToFit = this.ShrinkToFit;
            cp1.IsItalic = this.IsItalic;
            cp1.WrapText = this.WrapText;
            cp1.Height = this.Height;
            cp1.Width = this.Width;
            cp1.DynHeight = this.DynHeight;
            cp1.SpanWidth = this.SpanWidth;

            return cp1;
        }
    }
    /// <summary>
    /// 单元格类型的枚举
    /// </summary>
    public enum CellTypeName 
    {
        /// <summary>
        /// 这是字符串类型
        /// </summary>
        String,
        /// <summary>
        /// 这是数字类型
        /// </summary>
        Double,
        /// <summary>
        /// 这是时间类型
        /// </summary>
        Date,
        /// <summary>
        /// 这是空类型
        /// </summary>
        Blank
    }

   
}