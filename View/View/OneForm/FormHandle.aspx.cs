using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using App.InfoGrid2.Model;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;

namespace App.InfoGrid2.View.OneForm
{
    
    /// <summary>
    /// 
    /// </summary>
    public partial class FormHandle : System.Web.UI.Page, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            string method = WebUtil.QueryTrimUpper("method");    //执行的函数名称
            
            SModel result = null;

            switch (method)
            {
                case "IMAGE_UPLOAD":
                    result = ImageUpload();
                    break;
                default:
                    result = new SModel();
                    result["success"] = false;

                    break;
            }


            Response.Clear();

            string json = result.ToJson();

            Response.Write(json);

            Response.End();

        }


        private SModel ImageUpload()
        {
            SModel result = null;

            if (this.Request.Files.Count == 0)
            {

                return result;
            }

            string table = WebUtil.Query("table");    //CMS 模块

            string cata_code = WebUtil.Query("cata_code");

            int item_id = WebUtil.QueryInt("item_id");

            string item_code = WebUtil.Query("item_code");

            string tag_code = WebUtil.Query("TAG_CODE");

            string field = WebUtil.Query("field");

            try
            {
                HttpPostedFile file = this.Request.Files[0];
                result = ImageUpload(table, field, item_id, item_code,  tag_code, file);
            }
            catch (Exception ex)
            {
                log.Error("上传文件错误. url=" + this.Request.RawUrl, ex);
            }

            return result;

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="table"></param>
        /// <param name="item_id"></param>
        /// <param name="item_code"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private SModel ImageUpload(string table, string field, int item_id, string item_code, string tag_code, HttpPostedFile file)
        {
            int span = 500;
            int m0 = (item_id - item_id % span)/ span;
            int m1 = m0 + 1;

            int span0 = m0 * span;
            int span1 = m1 * span;

            string revDir = $"/UserFile/form/{table}/{span0:0000}-{span1:0000}/{item_id:0000}";

            string dir = MapPath(revDir);

            //string fullname = dir + file.FileName;

            #region  上传文件要把文件名的一些关键字给换成空格  2017-09-11  小渔夫

            char[] c = new char[] {'#','&',' ','!','@','+','='};
            var new_fname = file.FileName;

            foreach (var item in c)
            {
                new_fname = new_fname.Replace(item, ' ');
            }
            string newFilename = FileUtil.GetUniqueName(dir, new_fname);

            #endregion


            WebFileInfo fi = new WebFileInfo(revDir, newFilename);

            FileUtil.AutoCreateDir(fi.PhysicalDir);
            file.SaveAs(fi.PhysicalPath);


            #region 产生缩略图

            string thumbPath = fi.RelativePath;

            if (ImageUtil.IsImage(file.FileName))
            {
                string thumbRevDir = revDir + "/_thumb";

                string thumbDir = MapPath(thumbRevDir);

                //string thumbFullname = thumbDir + newFilename;

                string thumbFullname = FileUtil.GetUniqueName(thumbDir, newFilename);

                WebFileInfo thumbFI = new WebFileInfo(thumbRevDir, newFilename);

                Bitmap bmp = ImageUtil.CreateThumb(file.InputStream, 128, 128);

                FileUtil.AutoCreateDir(thumbFI.PhysicalDir);

                bmp.Save(thumbFI.PhysicalPath, ImageFormat.Jpeg);

                thumbPath = thumbFI.RelativePath;
            }

            #endregion



            EcUserState userState = EcContext.Current.User;

            BIZ_FILE bFile = new BIZ_FILE();
            bFile.TAG_CODE = tag_code;

            bFile.FILE_NAME = new_fname;
            bFile.TABLE_NAME = table;
            bFile.FIELD_NAME = field;

            bFile.ROW_ID = item_id;
            bFile.ROW_CODE = item_code;
            bFile.FILE_PATH = fi.RelativePath;
            bFile.FILE_SIZE = file.ContentLength;
            bFile.FILE_EX = fi.Extension;
            bFile.TABLE_TYPE = "table";
            bFile.ROW_AUTHOR_USER_CODE = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.InsertModel(bFile);



            SModel msg = new SModel();
            msg["state"] = "SUCCESS";
            msg["type"] = fi.Extension;
            msg["size"] = file.ContentLength;
            msg["url"] = fi.RelativePath;
            msg["name"] = newFilename;
            msg["thumb_url"] = thumbPath;
            msg["original"] = newFilename;
            msg["code"] = bFile.BIZ_FILE_ID;

            return msg;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}