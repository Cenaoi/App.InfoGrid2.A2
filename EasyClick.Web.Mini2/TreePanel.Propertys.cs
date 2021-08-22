﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini2
{
    partial class TreePanel
    {

        #region 动态加载节点

        /// <summary>
        /// 动态加载节点
        /// </summary>
        bool m_LoadDynamic = false;

        /// <summary>
        /// 动态加载。默认值:false
        /// </summary>
        [DefaultValue(false)]
        public bool LoadDynamic
        {
            get { return m_LoadDynamic; }
            set { m_LoadDynamic = value; }
        }

        #endregion

        string m_StoreID;

        #region 数据绑定属性

        string m_ParentField ;   //父字段名称
        string m_TextField  ;
        string m_ValueField ;
        string m_DataPathField;


        /// <summary>
        /// 默认根ID
        /// </summary>
        string m_RootID ;

        /// <summary>
        /// 节点输出
        /// </summary>
        string m_TextFormatString;  
        string m_ValueFormatString;
        string m_DataPathFormatString;

        /// <summary>
        /// 节点类型字段
        /// </summary>
        string m_TypeField;

        /// <summary>
        /// 图标路径字段
        /// </summary>
        string m_IconPathField;

        /// <summary>
        /// 显示节点的复选框
        /// </summary>
        bool m_ShowCheckBox = false;

        /// <summary>
        /// 复选框值的字段名
        /// </summary>
        string m_CheckValueField;


        int m_Width;

        int m_Height;


        [DefaultValue(0)]
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        [DefaultValue(0)]
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        /// <summary>
        /// 数据仓库ID
        /// </summary>
        [Description("数据仓库ID")]
        [DefaultValue("")]
        public string StoreID
        {
            get { return m_StoreID; }
            set { m_StoreID = value; }
        }

        /// <summary>
        /// 复选框值的字段名
        /// </summary>
        [DefaultValue("")]
        [Description("复选框值的字段名")]
        public string CheckValueField
        {
            get { return m_CheckValueField; }
            set { m_CheckValueField = value; }
        }

        /// <summary>
        /// 显示节点的复选框
        /// </summary>
        [DefaultValue(false)]
        [Description("显示节点的复选框")]
        public bool ShowCheckBox
        {
            get { return m_ShowCheckBox; }
            set { m_ShowCheckBox = value; }
        }

        /// <summary>
        /// 节点的类型字段
        /// </summary>
        [DefaultValue("")]
        [Description("节点的类型字段")]
        public string TypeField
        {
            get { return m_TypeField; }
            set { m_TypeField = value; }
        }

        /// <summary>
        /// 图标路径字段
        /// </summary>
        [DefaultValue("")]
        [Description("图标路径字段")]
        public string IconPathField
        {
            get { return m_IconPathField; }
            set { m_IconPathField = value; }
        }


        [Description("")]
        [DefaultValue("")]
        [Category("DataBase")]
        public string DataPathFormatString
        {
            get { return m_DataPathFormatString; }
            set { m_DataPathFormatString = value; }
        }

        /// <summary>
        /// 标签格式化显示
        /// </summary>
        [Description("标签格式化显示")]
        [DefaultValue("")]
        [Category("DataBase")]
        public string TextFormatString
        {
            get { return m_TextFormatString; }
            set { m_TextFormatString = value; }
        }


        /// <summary>
        /// 值格式化显示
        /// </summary>
        [Description("值格式化显示")]
        [DefaultValue("")]
        [Category("DataBase")]
        public string ValueFormatString
        {
            get { return m_ValueFormatString; }
            set { m_ValueFormatString = value; }
        }

        /// <summary>
        /// 默认根节点ID
        /// </summary>
        [Category("DataBase")]
        public string RootID
        {
            get { return m_RootID; }
            set { m_RootID = value; }
        }

        /// <summary>
        /// 父级字段名称
        /// </summary>
        [Category("DataBase")]
        public string ParentField
        {
            get { return m_ParentField; }
            set { m_ParentField = value; }
        }

        /// <summary>
        /// 显示字段名称
        /// </summary>
        [Category("DataBase")]
        public string TextField
        {
            get { return m_TextField; }
            set { m_TextField = value; }
        }

        /// <summary>
        /// 值字段名称
        /// </summary>
        [Category("DataBase")]
        public string ValueField
        {
            get { return m_ValueField; }
            set { m_ValueField = value; }
        }

        /// <summary>
        /// 数据路径的字段名称
        /// </summary>
        [Category("DataBase")]
        public string DataPathField
        {
            get { return m_DataPathField; }
            set { m_DataPathField = value; }
        }


        #endregion
    }
}
