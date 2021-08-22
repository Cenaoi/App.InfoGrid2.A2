
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 临时表
    /// </summary>
    [DBTable("IG2_TMP_TABLE")]
    [Description("临时表")]
    [Serializable]
    public class IG2_TMP_TABLE : LightModel
    {

        /// <summary>
        /// 
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("")]
        [UIField(visible = true)]
        public Int32 IG2_TMP_TABLE_ID { get; set; }

        /// <summary>
        /// 所属目录ID
        /// </summary>
        [DBField]
        public int IG2_CATALOG_ID { get; set; }


        /// <summary>
        /// 表的 GUID,留着以后用
        /// </summary>
        [DBField]
        public Guid TABLE_UID { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [DBField]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_TABLE_ID { get; set; }


        /// <summary>
        /// 表类型.TABLE-数据表;VIEW-视图表;AREA-页区域表
        /// </summary>
        [DBField]
        [DisplayName("表类型.TABLE-数据表;VIEW-视图表")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TABLE_TYPE_ID { get; set; }


        /// <summary>
        /// 表的子类型.
        /// PAGE_AREA = 表区域的子类型
        /// SELECT = 备选的区域界面.例:弹出下拉框,弹出模式窗体选择....
        /// </summary>
        [DBField]
        public string TABLE_SUB_TYPE_ID { get; set; }

        /// <summary>
        /// 页面模板 IG2_PAGE 主键ID
        /// </summary>
        [DBField]
        [DisplayName("页面模板 IG2_PAGE 主键ID")]
        [UIField(visible = true)]
        public Int32 PAGE_ID { get; set; }


        /// <summary>
        /// 页面模板的区域类型。SEARCH-查询区,A-区域,B-区域....名称任意定
        /// </summary>
        [DBField]
        [DisplayName("页面模板的区域类型。SEARCH-查询区,A-区域,B-区域....名称任意定")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String PAGE_AREA_ID { get; set; }


        /// <summary>
        /// 展示模式。SEARCH-查询;FORM-表单;GRID-表格;TREE-树模式
        /// </summary>
        [DBField]
        [DisplayName("展示模式。SEARCH-查询;FORM-表单;GRID-表格;TREE-树模式")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String DISPLAY_MODE { get; set; }

        /// <summary>
        /// 界面显示模式
        /// </summary>
        [DBField]
        public string UI_TYPE_ID { get; set; }


        /// <summary>
        /// 父表ID
        /// </summary>
        [DBField]
        [DisplayName("父表ID")]
        [UIField(visible = true)]
        public Int32 PARENT_ID { get; set; }


        /// <summary>
        /// 视图才使用的属性。所属的表ID
        /// </summary>
        [DBField]
        public int VIEW_OWNER_TABLE_ID { get; set; }

        /// <summary>
        /// 视图才使用的属性。字段名
        /// </summary>
        [DBField]
        public string VIEW_OWNER_COL_ID { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 显示名
        /// </summary>
        [DBField]
        [DisplayName("显示名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY { get; set; }


        /// <summary>
        /// 显示大标题
        /// </summary>
        [DBField]
        public bool IS_BIG_TITLE_VISIBLE { get; set; }

        /// <summary>
        /// 主键字段名
        /// </summary>
        [DBField]
        [DisplayName("主键字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ID_FIELD { get; set; }


        /// <summary>
        /// 自增长字段名
        /// </summary>
        [DBField]
        [DisplayName("自增长字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String IDENTITY_FIELD { get; set; }


        /// <summary>
        /// 用户排序字段名
        /// </summary>
        [DBField]
        [DisplayName("用户排序字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String USER_SEQ_FIELD { get; set; }

        /// <summary>
        /// 控制字段id
        /// </summary>
        [DBField]
        [DisplayName("控制字段id")]
        [UIField(visible = true)]
        public Int32 COL_IDENTITY { get; set; }

        /// <summary>
        /// 记录单选. true=单选,false=多选
        /// </summary>
        [DBField]
        [DisplayName("记录单选. true=单选,false=多选")]
        [UIField(visible = true)]
        public Boolean SINGLE_SELECTION { get; set; }


        /// <summary>
        /// 工具栏按钮集合
        /// </summary>
        [DBField]
        [DisplayName("工具栏按钮集合")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public String TOOLBAR_ITEMS { get; set; }


        /// <summary>
        /// 标签名称
        /// </summary>
        [DBField]
        [DisplayName("标签名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TAB_TEXT { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [DBField]
        [DisplayName("备注")]
        [LMValidate(maxLen = 250)]
        [UIField(visible = true)]
        public string REMARK { get; set; }


        /// <summary>
        /// 页面模板。。配合 TABLE_TYPE_ID = "PAGE" 才有效
        /// </summary>
        [DBField]
        public string PAGE_TEMPLATE { get; set; }


        /// <summary>
        /// 激活关联
        /// </summary>
        [DBField]
        public bool JOIN_ENABLED { get; set; }

        /// <summary>
        /// 关联-自己字段名
        /// </summary>
        [DBField]
        [DisplayName("关联-自己字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ME_COL_NAME { get; set; }


        /// <summary>
        /// 关联-目标表名
        /// </summary>
        [DBField]
        [DisplayName("关联-目标表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_TAB_NAME { get; set; }


        /// <summary>
        /// 关联-目标字段名
        /// </summary>
        [DBField]
        [DisplayName("关联-目标字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String JOIN_COL_NAME { get; set; }



        /// <summary>
        /// join 版本号
        /// </summary>
        [DBField]
        public int JOIN_VERSION { get; set; } = 1;

        /// <summary>
        /// join 配置文件
        /// </summary>
        [DBField(StringEncoding.UTF8, true)]
        public string JOIN_V2_CONFIG { get; set; }




        /// <summary>
        /// 权限等级。0-用户自定义；2-普通；4-主键、锁行；6-系统字段；8-配合 UI，单元格的提示信息
        /// </summary>
        [DBField]
        [DisplayName("表ID")]
        [UIField(visible = true)]
        public Int32 SEC_LEVEL { get; set; }


        /// <summary>
        /// 访问权限.ALL-全局,ORG-部门,ROLE-角色,USER-用户
        /// </summary>
        [DBField]
        [DisplayName("访问权限.ALL-全局,ORG-部门,ROLE-角色,USER-用户")]
        [LMValidate(maxLen = 10)]
        [UIField(visible = true)]
        public String SEC_MODE_ID { get; set; }

        /// <summary>
        /// 权限-部门ID
        /// </summary>
        [DBField]
        [DisplayName("权限-部门ID")]
        [UIField(visible = true)]
        public Int32 SEC_ORG_ID { get; set; }


        /// <summary>
        /// 权限-部门名称
        /// </summary>
        [DBField]
        [DisplayName("权限-部门名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_ORG_TEXT { get; set; }

        /// <summary>
        /// 权限-角色ID
        /// </summary>
        [DBField]
        [DisplayName("权限-角色ID")]
        [UIField(visible = true)]
        public Int32 SEC_ROLE_ID { get; set; }


        /// <summary>
        /// 权限-角色名称
        /// </summary>
        [DBField]
        [DisplayName("权限-角色名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_ROLE_TEXT { get; set; }

        /// <summary>
        /// 权限-用户ID
        /// </summary>
        [DBField]
        [DisplayName("权限-用户ID")]
        [UIField(visible = true)]
        public Int32 SEC_USER_ID { get; set; }


        /// <summary>
        /// 权限-用户名称
        /// </summary>
        [DBField]
        [DisplayName("权限-用户名称")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String SEC_USER_TEXT { get; set; }

        /// <summary>
        /// 记录-状态ID
        /// </summary>
        [DBField]
        [DisplayName("记录-状态ID")]
        [UIField(visible = true)]
        public Int32 ROW_SID { get; set; }

        /// <summary>
        /// 记录-创建时间
        /// </summary>
        [DBField, DefaultValue("(GETDATE())")]
        [DisplayName("记录-创建时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_CREATE { get; set; }

        /// <summary>
        /// 记录-更新时间
        /// </summary>
        [DBField,DefaultValue("(GETDATE())")]
        [DisplayName("记录-更新时间")]
        [UIField(visible = true)]
        public DateTime ROW_DATE_UPDATE { get; set; }

        /// <summary>
        /// 记录-删除时间
        /// </summary>
        [DBField]
        [DisplayName("记录-删除时间")]
        [UIField(visible = true)]
        public DateTime? ROW_DATE_DELETE { get; set; }


        /// <summary>
        /// 记录-作者ID
        /// </summary>
        [DBField]
        [DisplayName("记录-作者ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_ID { get; set; }


        /// <summary>
        /// 记录-作者名称
        /// </summary>
        [DBField]
        [DisplayName("记录-作者名称")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ROW_AUTHOR_TEXT { get; set; }


        /// <summary>
        /// 记录-临时Session
        /// </summary>
        [DBField]
        [DisplayName("记录-临时Session")]
        [LMValidate(maxLen = 32)]
        [UIField(visible = true)]
        public String TMP_SESSION_ID { get; set; }



        /// <summary>
        /// 枚举值.值的字段名
        /// </summary>
        [DBField]
        public string ENUM_VALUE_FIELD { get; set; }

        /// <summary>
        /// 枚举值.显示的字段名
        /// </summary>
        [DBField]
        public string ENUM_TEXT_FIELD { get; set; }

        /// <summary>
        /// 记录-临时操作。A-新建，D-删除，E-修改
        /// </summary>
        [DBField]
        [DisplayName("记录-临时操作。A-新建，D-删除，E-修改")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String TMP_OP_ID { get; set; }


        [DBField]
        public Guid TMP_GUID { get; set; }


        [DBField,DefaultValue("(GETDATE())")]
        public DateTime TMP_OP_TIME { get; set; }


        /// <summary>
        /// 汇总显示
        /// </summary>
        [Description("汇总显示")]
        [DBField]
        public bool SUMMARY_VISIBLE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        public string STYLE_JSON_FIELD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DBField]
        public string STYLE_TEXT_FIELD { get; set; }


        /// <summary>
        /// 排序文本。。
        /// T-SQL 子句 Order By
        /// </summary>
        [DBField]
        [Description("排序文本。T-SQL 子句 Order By")]
        public string SORT_TEXT { get; set; }

        /// <summary>
        /// 插入记录的位置
        /// </summary>
        [DBField]
        [Description("插入记录的位置")]
        public string INSERT_POS { get; set; }


        /// <summary>
        /// 显示验证消息
        /// </summary>
        [DBField]
        [Description("显示验证消息")]
        public bool VALID_MSG_ENABLED { get; set; }


        /// <summary>
        /// 删除逻辑，如果业务对象
        /// </summary>
        [DBField]
        public int DELETE_BY_BIZ_SID { get; set; }

        /// <summary>
        /// 服务器扩展类
        /// </summary>
        [DBField]
        public string SERVER_CLASS { get; set; }


        /// <summary>
        /// 根目录代码
        /// </summary>
        [DBField]
        public string BIZ_CATALOG_IDENTITY { get; set; }

        /// <summary>
        /// 激活根目录。默认关闭
        /// </summary>
        [DBField]
        public bool BIZ_CATALOG_ENABLED { get; set; }

        /// <summary>
        /// 激活结构权限
        /// </summary>
        [DBField]
        public bool SEC_STRUCT_ENABLED { get; set; }

        /// <summary>
        /// 拷贝至那个表
        /// </summary>
        [DBField]
        public int COPY_FROM_TABLE_ID { get; set; }


        #region 表单属性

        /// <summary>
        /// 显示按钮编辑按钮
        /// </summary>
        [DBField]
        public bool VISIBLE_BTN_EDIT { get; set; }

        [DBField]
        public string FORM_NEW_TYPE { get; set; }

        /// <summary>
        /// (表单属性)
        /// </summary>
        [DBField]
        public string FORM_EDIT_TYPE { get; set; }

        [DBField]
        public int FORM_NEW_PAGEID { get; set; }

        /// <summary>
        /// (表单属性)修改的页ID
        /// </summary>
        [DBField]
        public int FORM_EDIT_PAGEID { get; set; }

        /// <summary>
        /// (表单属性)新建表单的别名
        /// </summary>
        [DBField]
        public string FORM_NEW_ALIAS_TITLE { get; set; }

        /// <summary>
        /// (表单属性)修改表单的别名
        /// </summary>
        [DBField]
        public string FORM_EDIT_ALIAS_TITLE { get; set; }


        /// <summary>
        /// 表单项目的标签宽度
        /// </summary>
        [DBField]
        [DisplayName("表单项目的标签宽度")]
        [UIField(visible = true)]
        public Int32 FORM_ITEM_LABLE_WIDTH { get; set; }

        /// <summary>
        /// 表单左上角印章显示
        /// </summary>
        [DBField]
        [DisplayName("表单左上角印章显示")]
        [UIField(visible = true)]
        public Boolean FORM_BIG_BIZSID_VISIBLE { get; set; }

        /// <summary>
        /// 表单右上角状态区域
        /// </summary>
        [DBField]
        [DisplayName("表单右上角状态区域")]
        [UIField(visible = true)]
        public Boolean FORM_RT_VISIBLE { get; set; }

        /// <summary>
        /// 表单宽度
        /// </summary>
        [DBField]
        [DisplayName("表单宽度")]
        [UIField(visible = true)]
        public Int32 FORM_WIDTH { get; set; }


        /// <summary>
        /// 表单位置
        /// </summary>
        [DBField]
        [DisplayName("表单位置")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FORM_ALIGN { get; set; }

        #endregion

        /// <summary>
        /// 针对 [复杂表] 特殊定制的属性， 同一个界面， 共享一个表的数据。。默认是 true=共享。
        /// 后来, 解码复杂了, 需要每个 tab 独立过滤...
        /// </summary>
        [DBField]
        public bool IS_SHARE_DATA { get; set; }


        /// <summary>
        /// 删除到回收站
        /// </summary>
        [DBField]
        [DefaultValue(true)]
        public bool DELETE_RECYCLE { get; set; } = true;

        /// <summary>
        /// 是否显示附件
        /// </summary>
        [DBField]
        public bool ATTACH_FILE_VISIBLE { get; set; }


        /// <summary>
        /// 查询框, 展开
        /// </summary>
        [DBField]
        public bool SEARCH_PANEL_EXPAND { get; set; }

        #region 锁行

        /// <summary>
        /// 锁行模式.AUTO-自动, FIELD-字段, RULE-规则
        /// </summary>
        [DBField]
        public string LOCKED_MODE_ID { get; set; }

        /// <summary>
        /// 锁行字段
        /// </summary>
        [DBField]
        public string LOCKED_FIELD { get; set; }

        /// <summary>
        /// 锁行规则
        /// </summary>
        [DBField]
        public string LOCKED_RULE { get; set; }

        #endregion


        #region 展示规则

        /// <summary>
        /// 展示规则编码
        /// </summary>
        [DBField]
        public string DISPLAY_RULE_CODE { get; set; }

        /// <summary>
        /// 展示的 CSS 代码
        /// </summary>
        [DBField]
        public string DISPLAY_EX_CSS { get; set; }

        /// <summary>
        /// 展示的 JS 代码
        /// </summary>
        [DBField]
        public string DISPLAY_EX_JS { get; set; }


        #endregion
    }

}