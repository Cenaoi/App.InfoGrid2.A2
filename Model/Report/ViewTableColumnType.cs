using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Model.Report
{
    /// <summary>
    /// 报表界面表格中列的自定义类
    /// </summary>
    [Serializable]
    public class ViewTableColumnType
    {


        public ViewTableColumnType()
        {

        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="text">字段说明</param>
        /// <param name="dbType">显示类型</param>
        public ViewTableColumnType(string field, string text, LMFieldDBTypes dbType)
        {
            this.Field = field;
            this.Text = text;
            this.DbType = dbType;
        }


        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 是否要合计
        /// </summary>
        public bool Is_Total { get; set; }

        /// <summary> 
        /// 字段说明
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 是否是大字段
        /// </summary>
        public bool Is_Remarks { get; set; }



        /// <summary>
        /// 是否是分组列
        /// </summary>
        public bool Is_Group { get; set; }


        /// <summary>
        /// 列宽度
        /// </summary>
        public int Col_Width { get; set; }



        /// <summary>
        /// 要显示的类型
        /// </summary>
        public LMFieldDBTypes DbType { get; set; }

    }
}
