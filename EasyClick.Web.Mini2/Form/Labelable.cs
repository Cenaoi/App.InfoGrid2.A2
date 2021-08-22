using System;
using System.Collections.Generic;
using System.Text;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// Form 元素标签
    /// </summary>
    public class Labelable
    {
        string m_FieldLabel;

        TextAlign m_LabelAlign = TextAlign.Left;

        int m_LabelWidth = 100;

        int m_LabelPad = 5;

        string m_LabelSeparator = ":";

        bool m_HideLabel = false;

        /// <summary>
        /// 标签
        /// </summary>
        public string FieldLabel
        {
            get { return m_FieldLabel; }
            set { m_FieldLabel = value; }
        }

        /// <summary>
        /// 标签位置
        /// </summary>
        public TextAlign LabelAlign
        {
            get { return m_LabelAlign; }
            set { m_LabelAlign = value; }
        }

        /// <summary>
        /// 标签宽度
        /// </summary>
        public int LabelWidth
        {
            get { return m_LabelWidth; }
            set { m_LabelWidth = value; }
        }

        public int LabelPad
        {
            get { return m_LabelPad; }
            set { m_LabelPad = value; }
        }


        public string LabelSeparator
        {
            get { return m_LabelSeparator; }
            set { m_LabelSeparator = value; }
        }

        /// <summary>
        /// 隐藏标签
        /// </summary>
        public bool HideLabel
        {
            get { return m_HideLabel; }
            set { m_HideLabel = value; }
        
        }

    }
}
