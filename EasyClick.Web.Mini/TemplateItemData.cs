using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 模板条目的数据
    /// </summary>
    public class TemplateItemData:SortedList<string,object>
    {
        TemplateItemState m_State = TemplateItemState.None;

        /// <summary>
        /// 条目状态
        /// </summary>
        [DefaultValue( TemplateItemState.None)]
        public TemplateItemState State
        {
            get { return m_State; }
            set { m_State = value; }
        }


    }


}
