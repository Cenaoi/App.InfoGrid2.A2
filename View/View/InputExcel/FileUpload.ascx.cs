using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility.Web;
using EC5.Utility;
using EasyClick.Web.Mini;
using System.IO;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.InputExcel
{
    public partial class FileUpload : System.Web.UI.UserControl, EC5.SystemBoard.Interfaces.IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int m_id;

        protected void Page_Load(object sender, EventArgs e)
        {

            m_id = WebUtil.QueryInt("id");


        }



        /// <summary>
        /// 上传完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void suTest_Uploader(object sender, EventArgs e)
        {

            //拿到文件名
            string fileName = this.suTest.FileName;

            //判断是否为空
            if (StringUtil.IsBlank(fileName))
            {
                MiniHelper.Alert("请选择要上传的文件");
                return;
            }


            FileInfo fileInfo = new FileInfo(fileName);

            //看文件是否是zip格式的
            if (fileInfo.Extension != ".xls")
            {
                MiniHelper.Alert("只能上传xls文件");
                return;
            }

            WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));

            wFile.CreateDir();


            this.suTest.SaveAs(wFile.PhysicalPath);

            try
            {

                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_IMPORT_RULE rule = decipher.SelectModelByPk<IG2_IMPORT_RULE>(m_id);

                rule.SRC_FILE = wFile.RelativePath;

                decipher.UpdateModelProps(rule, "SRC_FILE");


                MiniHelper.Alert("上传完成！");

                EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok'});");

            }
            catch (Exception ex)
            {
                log.Error("上传失败！", ex);
                MiniHelper.Alert("上传完成！");
            }



        }



        
    }
}