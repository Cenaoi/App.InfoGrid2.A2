using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini;
using EC5.Utility;
using System.IO;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.PrintTemplate
{
    public partial class TemplateUpload : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 模板ID
        /// </summary>
        public int m_id;


        protected void Page_Load(object sender, EventArgs e)
        {
            FileUpload1.Uploader += FileUpload1_Uploader;

            m_id = WebUtil.QueryInt("id");

            if(m_id == 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }
        }

        private void FileUpload1_Uploader(object sender, EventArgs e)
        {

            string fileName = FileUpload1.FileName;


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


            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_PRINT_TEMPLATE bpt = null;

            try
            {

                bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(m_id);

                if (bpt == null)
                {
                    MessageBox.ShowTips("取模板数据时出错了！");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("拿不到当前新建的模板数据", ex);
            }




            ///文件名格式--测试页面_136_19_UT_090_UT_091.xls
            fileName = string.Format(
                    "{0}_{1}_{2}_{3}_{4}.xls",
                    bpt.PAGE_TEXT,
                    bpt.PAGE_ID,
                    m_id,
                    bpt.MAIN_TABLE_NAME,
                    bpt.SUB_TABLE_NAME
                    );



            //存放的物理位置
            string path = Server.MapPath("/PrintTemplate");

            DirectoryInfo info = new DirectoryInfo(path);

            if (!info.Exists)
            {
                info.Create();
            }

            try
            {
                this.FileUpload1.SaveAs(path + "\\" + fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("保存文件失败！", ex);
            }


            try
            {

                bpt.TEMPLATE_URL = "/PrintTemplate/" + fileName;

                decipher.UpdateModelProps(bpt, "TEMPLATE_URL");

            }
            catch (Exception ex)
            {
                log.Error("上传失败！", ex);
                MiniHelper.Alert("上传失败！");
                return;
            }



            //InputData(path + "\\" + fileName);

            MiniHelper.Alert("上传完成！");

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',id:'" + m_id + "'});");

        }


    }
}