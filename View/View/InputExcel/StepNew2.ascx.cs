using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class StepNew2 : WidgetControl, IView
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
        /// 从选择窗口拿到表
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
                log.Error("选择表出错了！",ex);
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

            string url = "/App/InfoGrid2/View/InputExcel/FileUpload.aspx?id="+id;

            EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowInputFile('{0}')",url);


        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void Refresh() 
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(id);

            if(rule == null)
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


            if(string.IsNullOrEmpty(tbxTableName.Value))
            {
                MessageBox.Alert("请选择表名！");
                return;
            }

            if(string.IsNullOrEmpty(this.tbxExcelName.Value))
            {
                MessageBox.Alert("请上传Excel文件");
                return;
            }


            int id = WebUtil.QueryInt("id");

            MiniPager.Redirect("StepNew3.aspx?id=" + id);


        }




    }
}