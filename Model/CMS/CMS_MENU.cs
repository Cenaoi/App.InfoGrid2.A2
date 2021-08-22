using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.CMS
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [DBTable("CMS_MENU")]
    [Description("菜单表")]
    [Serializable]
    public class CMS_MENU : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 CMS_MENU_ID { get; set; }


        /// <summary>
        /// 自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PK_MENU_CODE { get; set; }


        /// <summary>
        /// 数据库表名
        /// </summary>
        [DBField]
        [DisplayName("数据库表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }



        /// <summary>
        /// 明细数据表ID
        /// </summary>
        [DBField]
        [DisplayName("明细数据表ID")]
        [UIField(visible = true)]
        public Int32 ITEM_TABLE_ID { get; set; }


        /// <summary>
        /// 目录数据表ID
        /// </summary>
        [DBField]
        [DisplayName("目录数据表ID")]
        [UIField(visible = true)]
        public Int32 CATA_TABLE_ID { get; set; }



        /// <summary>
        /// 菜单类型
        /// </summary>
        [DBField]
        [DisplayName("菜单类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_TYPE { get; set; }


        /// <summary>
        /// 上级菜单自定义主键编码
        /// </summary>
        [DBField]
        [DisplayName("上级菜单自定义主键编码")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PARENT_MENU_CODE { get; set; }


        /// <summary>
        /// 菜单名称
        /// </summary>
        [DBField]
        [DisplayName("菜单名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String MENU_TEXT { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        [DBField]
        [DisplayName("激活")]
        [UIField(visible = true)]
        public Boolean ENABLED { get; set; } = true;


        /// <summary>
        /// 标识符
        /// </summary>
        [DBField]
        [DisplayName("标识符")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String M_IDENTIFIER { get; set; }


        /// <summary>
        /// 界面类型
        /// </summary>
        [DBField]
        [DisplayName("界面类型")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAGE_TYPE { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        [DBField]
        [DisplayName("菜单排序")]
        [UIField(visible = true)]
        public Decimal MENU_SEQ { get; set; }


        /// <summary>
        /// 界面模板
        /// </summary>
        [DBField]
        [DisplayName("界面模板")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAGE_TEMPLATE { get; set; }


        /// <summary>
        /// 界面目录模板
        /// </summary>
        [DBField]
        [DisplayName("界面目录模板")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAGE_CATA_TEMPLATE { get; set; }


        /// <summary>
        /// 页面分类模板
        /// </summary>
        [DBField]
        [DisplayName("页面分类模板")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String PAGE_CATE_TEMPLATE { get; set; }


        /// <summary>
        /// 图标路径
        /// </summary>
        [DBField]
        [DisplayName("图标路径")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ICON_URL { get; set; }

        /// <summary>
        /// 目录激活
        /// </summary>
        [DBField]
        [DisplayName("目录激活")]
        [UIField(visible = true)]
        public Boolean CATALOG_ENABLED { get; set; }

        /// <summary>
        /// 分类激活
        /// </summary>
        [DBField]
        [DisplayName("分类激活")]
        [UIField(visible = true)]
        public Boolean CATEGORY_ENABLED { get; set; }


        /// <summary>
        /// 业务状态
        /// </summary>
        [DBField]
        [DisplayName("业务状态")]
        [UIField(visible = true)]
        public Int32 BIZ_SID { get; set; }

        /// <summary>
        /// 记录状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        [DBField]
        [DefaultValue("(GETDATE())")]
        [DisplayName("记录更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }

    }
}
