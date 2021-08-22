using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 可编辑的数值文本框
    /// </summary>
    [Description("数值文本编辑框")]
    public class EditorNumberCell:EditorTextCell
    {
        /// <summary>
        /// 
        /// </summary>
        public EditorNumberCell()
        {
            this.ItemAlign = CellAlign.Right;
            this.Width = 60;
            this.ImeMode = EditorImeMode.Disabled;
        }

        #region 集成后，重写的属性

        /// <summary>
        /// 输入法模式
        /// </summary>
        [DefaultValue(EditorImeMode.Disabled)]
        public override EditorImeMode ImeMode
        {
            get { return base.ImeMode; }
            set { base.ImeMode = value; }
        }
        
        /// <summary>
        /// 宽度
        /// </summary>
        [DefaultValue(60)]
        public override int Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        [DefaultValue(CellAlign.Right)]
        public override CellAlign ItemAlign
        {
            get { return base.ItemAlign; }
            set { base.ItemAlign = value; }
        }

        #endregion
    }
}
