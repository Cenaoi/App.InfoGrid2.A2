
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;
using System.Diagnostics;

namespace App.InfoGrid2.Model
{
    /// <summary>
    /// 字段定义
    /// </summary>
    [DBTable("IG2_TABLE_COL")]
    [Description("字段定义")]
    [Serializable]
    [DebuggerDisplay("DB_FIELD={DB_FIELD}, F_NAME={F_NAME}")]
    public class IG2_TABLE_COL : LightModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [DBField, DBKey, DBIdentity]
        [DisplayName("ID")]
        [UIField(visible = true)]
        public Int32 IG2_TABLE_COL_ID { get; set; }

        /// <summary>
        /// 表的 GUID,留着以后用
        /// </summary>
        [DBField]
        public Guid TABLE_UID { get; set; }

        /// <summary>
        /// 表名。
        /// （以后整个框架体系成熟后，这个字段删除）
        /// </summary>
        [DBField]
        [DisplayName("表名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TABLE_NAME { get; set; }


        /// <summary>
        /// 表ID
        /// </summary>
        [DBField]
        [DisplayName("表ID")]
        [UIField(visible = true)]
        public Int32 IG2_TABLE_ID { get; set; }


        /// <summary>
        /// 权限等级。0-用户自定义；2-普通；4-主键、锁行；6-系统字段；8-配合 UI，单元格的提示信息
        /// </summary>
        [DBField]
        [DisplayName("表ID")]
        [UIField(visible = true)]
        public Int32 SEC_LEVEL { get; set; }


        /// <summary>
        /// 视图的原字段名。
        /// 格式： 表名.字段名
        /// </summary>
        [DBField]
        public string VIEW_FIELD_SRC { get; set; }

        /// <summary>
        /// 内部列名
        /// </summary>
        [DBField]
        [DisplayName("内部列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_FIELD { get; set; }


        /// <summary>
        /// 备注字段，ntext 类型的字段
        /// </summary>
        [DBField]
        public bool IS_REMARK { get; set; }


        /// <summary>
        /// 列名
        /// </summary>
        [DBField]
        [DisplayName("列名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String F_NAME { get; set; }


        /// <summary>
        /// 显示名
        /// </summary>
        [DBField]
        [DisplayName("显示名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY { get; set; }


        /// <summary>
        /// 显示类型.例如控件
        /// </summary>
        [DBField]
        [DisplayName("显示类型.例如控件")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DISPLAY_TYPE { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        [DBField]
        [DisplayName("列宽度")]
        [UIField(visible = true)]
        public Int32 DISPLAY_LEN { get; set; }

        /// <summary>
        /// 列高度
        /// </summary>
        [DBField]
        [DisplayName("列宽度")]
        [UIField(visible = true)]
        public int DISPLAY_HEIGHT { get; set; }


        /// <summary>
        /// 显示格式
        /// </summary>
        [DBField]
        [DisplayName("显示格式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FORMAT { get; set; }


        /// <summary>
        /// 数据类型.sring,int...
        /// </summary>
        [DBField]
        [DisplayName("数据类型.sring,int...")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DB_TYPE { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [DBField]
        [DisplayName("显示顺序")]
        [UIField(visible = true)]
        public Decimal FIELD_SEQ { get; set; }


        /// <summary>
        /// 默认值
        /// </summary>
        [DBField]
        [DisplayName("默认值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String DEFAULT_VALUE { get; set; }


        /// <summary>
        /// 分组ID
        /// </summary>
        [DBField]
        [DisplayName("分组ID")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String GROUP_ID { get; set; }


        /// <summary>
        /// 查询框的子元素的显示模式
        /// </summary>
        [DBField]
        public string V_SEARCH_MODE_ID { get; set; }

        /// <summary>
        /// 表单的显示模式
        /// 例如:下拉框,弹出框
        /// </summary>
        [DBField]
        public string V_EDIT_MODE_ID { get; set; }

        /// <summary>
        /// 表格列的显示模式
        /// 例如:下拉框,弹出框
        /// </summary>
        [DBField]
        public string V_LIST_MODE_ID { get; set; }


        /// <summary>
        /// 控件的触发模式.不可填写,可填写.两种
        /// </summary>
        [DBField]
        public string V_TRIGGER_MODE { get; set; }


        /// <summary>
        /// 帮助提示
        /// </summary>
        [DBField]
        [DisplayName("帮助提示")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String TOOLTIP { get; set; }


        /// <summary>
        /// 对齐方式
        /// </summary>
        [DBField]
        [DisplayName("对齐方式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ANGLE { get; set; }

        /// <summary>
        /// 小数点
        /// </summary>
        [DBField]
        [DisplayName("小数点")]
        [UIField(visible = true)]
        public Int32 DB_DOT { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        [DBField]
        [DisplayName("字段长度")]
        [UIField(visible = true)]
        public Int32 DB_LEN { get; set; }

        /// <summary>
        /// 必填
        /// </summary>
        [DBField]
        public bool IS_MANDATORY { get; set; }


        /// <summary>
        /// 业务必填
        /// </summary>
        [DBField]
        public bool IS_BIZ_MANDATORY { get; set; }


        /// <summary>
        /// 只读
        /// </summary>
        [DBField]
        [DisplayName("只读")]
        [UIField(visible = true)]
        public Boolean IS_READONLY { get; set; }

        /// <summary>
        /// 可视
        /// </summary>
        [DBField]
        [DisplayName("可视")]
        [UIField(visible = true)]
        public Boolean IS_VISIBLE { get; set; }

        /// <summary>
        /// 查询可视
        /// </summary>
        [DBField]
        [DisplayName("查询可视")]
        [UIField(visible = true)]
        public Boolean IS_SEARCH_VISIBLE { get; set; }

        /// <summary>
        /// 列可视
        /// </summary>
        [DBField]
        [DisplayName("列可视")]
        [UIField(visible = true)]
        public Boolean IS_LIST_VISIBLE { get; set; }


        /// <summary>
        /// 选择模式.FIXED-固定模式, TABLE-表模式
        /// </summary>
        [DBField]
        [DisplayName("选择模式.FIXED-固定模式, TABLE-表模式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ACT_MODE { get; set; }


        /// <summary>
        /// 固定模式值
        /// </summary>
        [DBField]
        [DisplayName("固定模式值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ACT_FIXED_ITEMS { get; set; }


        /// <summary>
        /// 工作表模式
        /// </summary>
        [DBField]
        [DisplayName("工作表模式")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String ACT_TABLE_ITEMS { get; set; }

        /// <summary>
        /// 关联的视图ID, 作用是为了删除无效的视图表,配合 ACT_TABLE_ITEMS 字段联动使用
        /// </summary>
        [DBField]
        public int ACT_LINK_VIEW_ID { get; set; }


        /// <summary>
        /// 过滤模式.THIS-单值模式,TSQL 自定义SQL
        /// </summary>
        [DBField]
        [DisplayName("过滤模式.THIS-单值模式,TSQL 自定义SQL")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FILTER_MODE { get; set; }


        /// <summary>
        /// 逻辑
        /// </summary>
        [DBField]
        [DisplayName("逻辑")]
        [LMValidate(maxLen = 20)]
        [UIField(visible = true)]
        public String FILTER_LOGIC { get; set; }


        /// <summary>
        /// 值
        /// </summary>
        [DBField]
        [DisplayName("值")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_VALUE { get; set; }


        /// <summary>
        /// 自定义SQL 的 Where 语句
        /// </summary>
        [DBField]
        [DisplayName("自定义SQL 的 Where 语句")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String FILTER_TSQL_WHERE { get; set; }


        /// <summary>
        /// (用于弹出选择框)-执行事件后,目标字段名
        /// </summary>
        [DBField]
        [DisplayName("(用于弹出选择框)-执行事件后,目标字段名")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EVENT_AFTER_FIELD_ID { get; set; }


        /// <summary>
        /// (用于弹出选择框)-执行字段后,目标字段描述
        /// </summary>
        [DBField]
        [DisplayName("(用于弹出选择框)-执行字段后,目标字段描述")]
        [LMValidate(maxLen = 50)]
        [UIField(visible = true)]
        public String EVENT_AFTER_FIELD_DESC { get; set; }

        #region 数据记录常规属性

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
        [DBField, DefaultValue("(GETDATE())")]
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

        #endregion

        /// <summary>
        /// 简单表达式.
        /// ={$T.COL_1:单价} * {$T.COL_2:数量}
        /// </summary>
        [DBField]
        public string L_CODE { get; set; }


        /// <summary>
        /// 是否为视图字段
        /// </summary>
        [DBField]
        public bool IS_VIEW_FIELD { get; set; }

        /// <summary>
        /// 视图字段参数
        /// </summary>
        [DBField]
        public string VIEW_FIELD_CONFIG { get; set; }

        #region 表单属性

        /// <summary>
        /// (表单属性) 是否换新行
        /// </summary>
        [DBField]
        public bool FORM_IS_NEWLINE { get; set; }

        /// <summary>
        /// (表单属性) 水印
        /// </summary>
        [DBField]
        public string FORM_PLACEHOLER { get; set; }

        #endregion

        /// <summary>
        /// 配合视图字段使用
        /// </summary>
        [VField]
        public string VIEW_TAG
        {
            get;
            set;
        }

        /// <summary>
        /// 视图字段的 SQL 语句
        /// </summary>
        [DBField]
        public string VIEW_SQL { get; set; }

        /// <summary>
        /// 按钮 URL 格式化地址,配合表单按钮使用
        /// </summary>
        [DBField]
        public string BTN_URL_FORAMT { get; set; }

        /// <summary>
        /// 表单字段的宽度
        /// </summary>
        [DBField]
        public int FORM_FIELD_WIDTH { get; set; }

        /// <summary>
        /// 表单字段的高度
        /// </summary>
        [DBField]
        public int FORM_FIELD_HEIGHT { get; set; }

        /// <summary>
        /// 表单字段的顺序
        /// </summary>
        [DBField]
        public decimal FORM_FIELD_SEQ { get; set; }

        /// <summary>
        /// 映射到其它视图表
        /// </summary>
        [DBField]
        [Description("映射到其它视图表")]
        public bool CAN_APPLY_VIEW { get; set; }

        #region 权限类别

        /// <summary>
        /// 权限类别-表名
        /// </summary>
        [DBField]
        public string FILTER_CATA_TABLE { get; set; }

        /// <summary>
        /// 权限类别-字段名
        /// </summary>
        [DBField]
        public string FILTER_CATA_FIELD { get; set; }



        #endregion


        #region  汇总

        /// <summary>
        /// 汇总类型
        /// </summary>
        [DBField]
        public string SUMMARY_TYPE { get; set; }

        /// <summary>
        /// 汇总格式化
        /// </summary>
        [DBField]
        public string SUMMARY_FORAMT { get; set; }

        /// <summary>
        /// 其它汇总定义
        /// </summary>
        [DBField]
        public string SUMMARY_OTHER { get; set; }

        /// <summary>
        /// 汇总过滤
        /// </summary>
        [DBField]
        public string SUMMARY_FILTER { get; set; }

        #endregion


        #region 验证


        /// <summary>
        /// 验证条件；
        /// 
        /// </summary>
        [DBField]
        public string VALID_CRITERIA { get; set; }

        /// <summary>
        /// 验证类型。 AUTO 自动 | META_DATA 元数据
        /// </summary>
        [DBField]
        public string VALID_TYPE_ID { get; set; }


        /// <summary>
        /// 必填
        /// </summary>
        [DBField]
        public bool VALID_REQUIRED { get; set; }

        /// <summary>
        /// 唯一性
        /// </summary>
        [DBField]
        public bool VALID_UNIQUE { get; set; }

        /// <summary>
        /// 验证元数据
        /// </summary>
        [DBField]
        public string VALID_METADATA { get; set; }

        /// <summary>
        /// 验证插件
        /// </summary>
        [DBField]
        public string VALID_PLUG { get; set; }

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

        /// <summary>
        /// 列或控件的显示函数
        /// </summary>
        [DBField]
        public string DISPLAY_EX_RETURN_JS_FUN { get; set; }

        /// <summary>
        /// 表格列, td 单元格增加样式名
        /// </summary>
        [DBField]
        public string GRID_COL_TD_CLS { get; set; }

        /// <summary>
        /// 表格列, 内部 Inner 增加样式名
        /// </summary>
        [DBField]
        public string GRID_COL_INNER_CLS { get; set; }


        #endregion

    }

}