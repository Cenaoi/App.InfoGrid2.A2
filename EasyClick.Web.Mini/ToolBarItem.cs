using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 工具栏元素
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("SecFunCode={SecFunCode}, SecFunID={SecFunID}, Visible={Visible}")]
    public abstract class ToolBarItem:ISecControl
    {
        bool m_Visible = true;
        bool m_Enabled = true;
        ToolBarItemAlign m_Align = ToolBarItemAlign.Left;

        string m_ID;


        [DefaultValue("")]
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 获取或设置显示
        /// </summary>
        [DefaultValue(true)]
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// 获取或设置激活状态
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }


        /// <summary>
        /// 对齐方式
        /// </summary>
        [DefaultValue(ToolBarItemAlign.Left)]
        public ToolBarItemAlign Align
        {
            get { return m_Align; }
            set { m_Align = value; }
        }

        protected internal virtual void Render(HtmlTextWriter writer)
        {

        }

        #region 安全权限

        string m_SecFunCode;
        string m_SecFunID;


        /// <summary>
        /// 
        /// </summary>
        [Category("Security")]
        public virtual string SecFunCode
        {
            get { return m_SecFunCode; }
            set { m_SecFunCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Security")]
        public virtual string SecFunID
        {
            get { return m_SecFunID; }
            set { m_SecFunID = value; }
        }

        /// <summary>
        /// 权限组件集合
        /// </summary>
        [Category("Security")]
        [Browsable(false)]
        public virtual SecControlCollection SecControls
        {
            get { return null; }
        }

        #endregion
    }
}
