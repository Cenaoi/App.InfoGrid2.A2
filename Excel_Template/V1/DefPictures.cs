using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    ///  从数据库里面拿出来的图片对象 ， 不是模板里固定的那种
    /// </summary>
    public class DefPictures
    {


        /// <summary>
        /// 图片字节集合
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// 起始单元格的x偏移量，如例子中的255表示直线起始位置距A1单元格左侧的距离；
        /// </summary>
        public int Dx1 { get; set; }

        /// <summary>
        /// 起始单元格的y偏移量，如例子中的125表示直线起始位置距A1单元格上侧的距离；
        /// </summary>
        public int Dy1 { get; set; }

        /// <summary>
        /// 终止单元格的x偏移量，如例子中的1023表示直线起始位置距C3单元格左侧的距离；
        /// </summary>
        public int Dx2 { get; set; }

        /// <summary>
        /// 终止单元格的y偏移量，如例子中的150表示直线起始位置距C3单元格上侧的距离；
        /// </summary>
        public int Dy2 { get; set; }


        /// <summary>
        /// 左上角列的索引
        /// </summary>
        public int Col1 { get; set; }


        /// <summary>
        /// 左上角列的索引
        /// </summary>
        public int Row1 { get; set; }


        /// <summary>
        /// 右下角列的索引
        /// </summary>
        public int Col2 { get; set; }

        /// <summary>
        /// 右下角的行索引
        /// </summary>
        public int Row2 { get; set; }


        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int  Height{ get; set; }


    }
}
