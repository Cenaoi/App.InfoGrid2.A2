using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;
using EasyClick.Web.Mini;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// 工具栏元素（抽象类）
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerDisplay("SecFunCode={SecFunCode}, SecFunID={SecFunID}, Visible={Visible}")]
    public abstract class ToolBarItem:EasyClick.Web.Mini.ISecControl
    {
        bool m_Visible = true;
        bool m_Enabled = true;
        ToolBarItemAlign m_Align = ToolBarItemAlign.Left;

        string m_ID;

        bool m_DesignMode = false;

        /// <summary>
        /// 设计模式
        /// </summary>
        protected internal bool DesignMode
        {
            get { return m_DesignMode; }
            set { m_DesignMode = value; }
        }



        /// <summary>
        /// 控件ID
        /// </summary>
        [Description("控件ID")]
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
        [Description("获取或设置显示")]
        public bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = value;

                if (this.DesignMode)
                {
                    return;
                }

                MiniHelper.EvalFormat("{0}.setVisible({1});", this.ID, value.ToString().ToLower());
            }
        }

        /// <summary>
        /// 获取或设置激活状态
        /// </summary>
        [DefaultValue(true)]
        [Description("获取或设置激活状态")]
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }


        /// <summary>
        /// 对齐方式
        /// </summary>
        [DefaultValue(ToolBarItemAlign.Left)]
        [Description("对齐方式")]
        public ToolBarItemAlign Align
        {
            get { return m_Align; }
            set { m_Align = value; }
        }

        protected internal virtual void Render(HtmlTextWriter writer)
        {

        }

        /// <summary>
        /// 获取配置的 js 代码
        /// </summary>
        /// <returns></returns>
        public virtual string GetConfigJS()
        {
            return string.Empty;
        }

        #region 安全权限

        string m_SecFunCode;
        string m_SecFunID;


        /// <summary>
        /// 权限代码
        /// </summary>
        [Category("Security")]
        [Description("权限模块代码")]
        public virtual string SecFunCode
        {
            get { return m_SecFunCode; }
            set { m_SecFunCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Security")]
        [Description("操作函数ID")]
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
        [Description("权限组件集合")]
        public virtual SecControlCollection SecControls
        {
            get { return null; }
        }

        #endregion
    }
}
