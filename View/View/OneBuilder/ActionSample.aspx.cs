using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.Utility.Web;
using App.BizCommon;

namespace App.InfoGrid2.View.OneBuilder
{
    /// <summary>
    /// 输出设计模式的控件
    /// </summary>
    public partial class ActionSample : System.Web.UI.Page,IView
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.ToolBar1.SetDesignMode(true);
            this.DataGrid1.SetDesignMode(true);

            
            
            int pageViewId = WebUtil.QueryInt("PageViewId");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE m = decipher.SelectModelByPk<IG2_TABLE>(pageViewId);

            List<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0}", pageViewId);

            Define_DataGrid(cols, "ROW_IDENTITY_ID");

            base.OnPreInit(e);


        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region 定义 DataGrid 控件

        /// <summary>
        /// 定义 DataGrid 控件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pkField">主键字段</param>
        private void Define_DataGrid(List<IG2_TABLE_COL> list, string pkField)
        {
            DataGridView dataGrid = this.DataGrid1;
            DataGridColumnCollection cols = dataGrid.Columns;

            dataGrid.DataKeys = pkField;

            CheckBoxField cbf = new CheckBoxField();
            cbf.DataField = pkField;    // GetTableNamePk();
            cbf.Name = pkField; // GetTableNamePk();

            cols.Add(cbf);

            foreach (IG2_TABLE_COL item in list)
            {
                if (!item.IS_VISIBLE || !item.IS_LIST_VISIBLE)
                {
                    continue;
                }

                EditorTextCell col = CreateGridCol(item);

                cols.Add(col);
            }

            cols.Add(new EmptyField());
        }


        /// <summary>
        /// 创建表格列
        /// </summary>
        /// <param name="list"></param>
        private EditorTextCell CreateGridCol(IG2_TABLE_COL item)
        {
            EditorTextCell baseCol = null;


            switch (item.DB_TYPE)
            {
                case "datetime":
                    {
                        EditorDateCell col = new EditorDateCell();
                        baseCol = col;
                    }
                    break;
                case "boolean":
                    {
                        EditorSelectCell col = new EditorSelectCell();
                        col.Items.Add("True", "True");
                        col.Items.Add("False", "False");

                        baseCol = col;
                    }
                    break;
                case "string":
                default:
                    {
                        EditorTextCell col = new EditorTextCell();
                        baseCol = col;
                    }
                    break;
            }

            baseCol.DataField = item.DB_FIELD;
            baseCol.HeaderText = GetHeaderText(item.DISPLAY, item.F_NAME);
            baseCol.ReadOnly = item.IS_READONLY;
            baseCol.DataFormatString = item.FORMAT;

            baseCol.ItemAlign = EnumUtil.Parse<CellAlign>(item.ANGLE, CellAlign.Left);

            if (item.DISPLAY_LEN > 0)
            {
                baseCol.Width = item.DISPLAY_LEN;
            }

            return baseCol;
        }

        #endregion


        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="display"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        public string GetHeaderText(string display, string fName)
        {
            if (!StringUtil.IsBlank(display))
            {
                return display;
            }

            return fName;
        }
    }
}