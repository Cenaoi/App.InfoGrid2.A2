using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Excel_Template
{
    public class PrintPro
    {
        /// <summary>
        /// 等于true是横向，false是纵向
        /// </summary>
        public bool Landscape { get; set; }


        /// <summary>
        /// 这是打印缩放比例的，1--100
        /// </summary>
        public short Scale { get; set; }


        /// <summary>
        /// 页宽
        /// </summary>
        public short FitWidth { get; set; }


        /// <summary>
        /// 页高
        /// </summary>
        public short FitHeight { get; set; }

        /// <summary>
        /// 纸张设置，9--正常A4纸大小 http://www.cnblogs.com/wolfplan/archive/2013/01/13/2858991.html
        /// </summary>
        public short PaperSize { get; set; }


        /// <summary>
        /// 起始页码，true自动并PageStart值不起作用
        /// </summary>
        public bool UsePage { get; set; }


        /// <summary>
        /// 起始页码
        /// </summary>
        public short PageStart { get; set; }


        /// <summary>
        /// 单色打印,true为单色打印
        /// </summary>
        public bool NoColors { get; set; }

        /// <summary>
        /// 草稿打印，true为草稿打印
        /// </summary>
        public bool IsDraft { get; set; }


        /// <summary>
        /// 打印顺序,当为true时，则表示“先行后列”；如果是false，则表示“先列后行”。
        /// </summary>
        public bool LeftToRight { get; set; }
        


    }
}
