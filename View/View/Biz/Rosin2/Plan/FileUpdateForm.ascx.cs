using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace App.InfoGrid2.View.Biz.Rosin2.Plan
{
    public partial class FileUpdateForm : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fieldUpdate1.Uploader += FieldUpdate1_Uploader;
        }

        private void FieldUpdate1_Uploader(object sender, EventArgs e)
        {
            //拿到文件名
            string fileName = this.fieldUpdate1.FileName;

            //判断是否为空
            if (StringUtil.IsBlank(fileName))
            {
                MessageBox.Alert("请选择要上传的文件");
                return;
            }

            string fileEx = Path.GetExtension(fileName);

            if (!StringUtil.IsBlank(this.fieldUpdate1.Filter))
            {
                string[] filters = StringUtil.Split(this.fieldUpdate1.Filter, "|");

                if(!ArrayUtil.Exist(filters, fileEx))
                {
                    MessageBox.Alert("上传格式错误, 只能上传此类型 : " + this.fieldUpdate1.Filter);
                    return;
                }
            }


            WebFileInfo wFile = new WebFileInfo("/_Temporary/excel_temp_update", FileUtil.NewFielname(fileEx));

            this.fieldUpdate1.SaveAs(wFile.PhysicalPath);

            this.fieldUpdate1.Value = wFile.RelativePath;

            H_FILE_PATH.Value = wFile.RelativePath;



            Toast.Show("上传成功.");

            this.fileName1.Value = fieldUpdate1.FileName;

            //string json = "ownerWindow.close({result:\"ok\", path : \"" + wFile.RelativePath + "\"});";

            //ScriptManager.Eval(json);
        }


        public void GoOkClick()
        {
            string json = "ownerWindow.close({result:\"ok\", path : \"" + H_FILE_PATH.Value + "\"});";

            ScriptManager.Eval(json);
        }

    }
}