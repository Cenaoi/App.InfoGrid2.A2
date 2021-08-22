using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace App.InfoGrid2.View.CMS
{
    public partial class CmsUploadHandler : System.Web.UI.Page,IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            string module = WebUtil.Query("module");    //CMS 模块

            string cata_code = WebUtil.Query("cata_code");

            int item_id = WebUtil.QueryInt("item_id");
            string item_code = WebUtil.Query("item_code");  



            string method = WebUtil.Query("method");    //执行的函数名称

            method = method.ToUpper();

            SModel result = null;

            if (this.Request.Files.Count == 0)
            {
                return;
            }                

            try
            {
                HttpPostedFile file = this.Request.Files[0];
                result = ImageUpload(module, cata_code,item_id, item_code, file);
            }
            catch(Exception ex)
            {
                log.Error("上传文件错误. url=" + this.Request.RawUrl, ex);
            }


            Response.Clear();

            string json = result.ToJson();

            Response.Write(json);

            Response.End();

        }


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cata_code"></param>
        /// <returns></returns>
        private SModel ImageUpload(string module, string cata_code, int item_id,string item_code, HttpPostedFile file)
        {
            string revDir = $"/UserFile/CMS/{module}/{item_id:0000}";

            string dir = MapPath(revDir);

            string fullname = dir + file.FileName;

            string newFilename = FileUtil.GetUniqueName(dir, file.FileName);



            WebFileInfo fi = new WebFileInfo(revDir, newFilename) ;

            FileUtil.AutoCreateDir(fi.PhysicalDir);
            file.SaveAs(fi.PhysicalPath);
            

            SModel result = new SModel();
            result["state"] = "SUCCESS";
            result["type"] = fi.Extension;
            result["size"] = file.ContentLength;
            result["url"] = fi.RelativePath;
            result["name"] = fi.Filename;
            result["original"] = file.FileName;


            return result;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}