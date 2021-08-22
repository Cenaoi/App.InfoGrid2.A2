using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class StepManyNew2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }




        /// <summary>
        /// 从选择窗口拿到主表信息
        /// </summary>
        /// <param name="id"></param>
        public void SetMainTableName(string id) 
        {
            try
            {
                int reulID = WebUtil.QueryInt("id");


                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(reulID);

                IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

                if (it == null || rule == null)
                {
                    MessageBox.Alert("没有选择表");
                    return;
                }

                this.tbxMainName.Value = it.TABLE_NAME;
                this.tbxMainDsiplay.Value = it.DISPLAY;

                rule.MAIN_TABLE = it.TABLE_NAME;
                rule.MAIN_TABLE_TEXT = it.DISPLAY;

                decipher.UpdateModelProps(rule, "MAIN_TABLE", "MAIN_TABLE_TEXT");



            }
            catch (Exception ex)
            {
                log.Error("选择表出错了！", ex);
                MessageBox.Alert("选择表出错了！");
                return;
            }
        }


        /// <summary>
        /// 从选择窗口拿到子表信息
        /// </summary>
        public void SetTableName(string id)
        {
            try
            {
                int reulID = WebUtil.QueryInt("id");


                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(reulID);

                IG2_TABLE it = decipher.SelectModelByPk<IG2_TABLE>(id);

                if (it == null || rule == null)
                {
                    MessageBox.Alert("没有选择表");
                    return;
                }

                this.tbxTableName.Value = it.TABLE_NAME;
                this.tbxDisplay.Value = it.DISPLAY;

                rule.TARGET_TABLE = it.TABLE_NAME;
                rule.TARGET_TABLE_TEXT = it.DISPLAY;

                decipher.UpdateModelProps(rule, "TARGET_TABLE", "TARGET_TABLE_TEXT");



            }
            catch (Exception ex)
            {
                log.Error("选择表出错了！", ex);
                MessageBox.Alert("选择表出错了！");
                return;
            }


        }

        /// <summary>
        /// 显示上传界面
        /// </summary>
        public void ShowInputFile()
        {
            int id = WebUtil.QueryInt("id");

            string url = "/App/InfoGrid2/View/InputExcel/FileUpload.aspx?id=" + id;

            EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowInputFile('{0}')", url);


        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void Refresh()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(id);

            if (rule == null)
            {
                return;
            }

            this.tbxExcelName.Value = rule.SRC_FILE;


        }


        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {


            if (string.IsNullOrEmpty(tbxTableName.Value))
            {
                MessageBox.Alert("请选择子表！");
                return;
            }

            if(string.IsNullOrEmpty(tbxMainName.Value))
            {
                MessageBox.Alert("请选择主表！");
                return;
            }


            if (string.IsNullOrEmpty(this.tbxExcelName.Value))
            {
                MessageBox.Alert("请上传Excel文件");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            int id = WebUtil.QueryInt("id");


            try
            {


                IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(id);

                if (rule == null)
                {
                    Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
                }


                IG2_TABLE it = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' and ROW_SID >=0 and TABLE_NAME='{0}' ", rule.MAIN_TABLE);


                if (it == null)
                {
                    Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
                }

                List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and SEC_LEVEL <= 6 ", it.IG2_TABLE_ID);

                foreach (IG2_TABLE_COL item in colList)
                {
                    IG2_IMPORT_RULE_MAP map = new IG2_IMPORT_RULE_MAP();
                    map.TARGET_TABLE = it.TABLE_NAME;
                    map.TARGET_FIELD = item.DB_FIELD;
                    map.TARGET_FIELD_TEXT = item.DISPLAY;
                    map.ROW_DATE_CREATE = DateTime.Now;
                    map.ROW_DATE_UPDATE = DateTime.Now;
                    map.IG2_IMPORT_RULE_ID = id;
                    map.EQUAL = "=";
                    map.SRC_FIELD_INDEX = -1;

                    decipher.InsertModel(map);

                }

                it = decipher.SelectToOneModel<IG2_TABLE>("TABLE_TYPE_ID='TABLE' and ROW_SID >=0 and TABLE_NAME='{0}' ", rule.TARGET_TABLE);


                if (it == null)
                {
                    Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
                }

                colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and SEC_LEVEL <= 6 ", it.IG2_TABLE_ID);



                foreach (IG2_TABLE_COL item in colList)
                {
                    IG2_IMPORT_RULE_MAP map = new IG2_IMPORT_RULE_MAP();
                    map.TARGET_TABLE = it.TABLE_NAME;
                    map.TARGET_FIELD = item.DB_FIELD;
                    map.TARGET_FIELD_TEXT = item.DISPLAY;
                    map.ROW_DATE_CREATE = DateTime.Now;
                    map.ROW_DATE_UPDATE = DateTime.Now;
                    map.IG2_IMPORT_RULE_ID = id;
                    map.EQUAL = "=";
                    map.SRC_FIELD_INDEX = -1;

                    decipher.InsertModel(map);

                }

            }
            catch (Exception ex) 
            {
                log.Error("插入数据出错了！",ex);
                MessageBox.Alert("插入数据出错了！");
                return;
            }

            MiniPager.Redirect("StepManyNew3.aspx?id=" + id);


        }

    }
}